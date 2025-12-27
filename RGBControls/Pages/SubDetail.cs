using JwCore;
using JwData;
using JwServices;
using JwShapeCommon;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Pages
{
    public partial class SubDetail : UIPage
    {
        public JwDataContext? dbContext;
        public SubDetail()
        {
            InitializeComponent();
        }

        private JwProjectSubData _subData;
        public JwqitaService jwqitaService => ServiceFactory.GetInstance().CreateJwqitaService();

        public JwProjectMainService jwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();

        public SubDetail(JwProjectSubData subData)
        {
            InitializeComponent();
            _subData = subData;
            uiLine1.Text = _subData.FloorName;

            dbContext = ContextFactory.GetContext();
        }
        JwCanvas canvas;
        private void SubDetail_Load(object sender, EventArgs e)
        {
            if (_subData != null)
            {
                this.Name = _subData.FloorName;
                this.Text = _subData.FloorName;
                //jwCanvasControl1. = _subData;

                canvas = _subData.DataToCanvas();
                ObservableCollectionListSource<JwProjectSubData> subDatas = new ObservableCollectionListSource<JwProjectSubData> { _subData };

                JwCanvasDraw canvasDraw = new JwCanvasDraw(canvas);
                jwCanvasControl1.CanvasDraw = canvasDraw;

                //this.dbContext?.Database.EnsureCreated();

                //this.dbContext?.JwProjectMainDatas.Load();

                //this.jwLianjieDatasBindingSource.DataSource = dbContext?.JwProjectSubDatas.Local.ToBindingList(); ;
                this.bindingSource1.DataSource = subDatas;

            }
        }

        /// <summary>
        /// 手动增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            UIEditOption option = new UIEditOption();
            option.AutoLabelWidth = true;
            option.Text = "認識されません。手動で追加してください";
            option.AddDouble("Length", "長さ", 0, true);

            UIEditForm frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += Frm_CheckedData;//校验
            frm.ShowDialog();
            if (frm.IsOK)
            {
                JwLianjieData lianjieData = new JwLianjieData(true);
                lianjieData.Length = Convert.ToDouble(frm["Length"].ToString());
                lianjieData.JwProjectSubDataId = _subData.Id;
                lianjieData.ProjectSubName = _subData.FloorName;
                lianjieData.CreateFrom = CreateFromType.ManuallyAdd;
                //lianjieData.Id = Guid.NewGuid().ToString();

                await jwProjectMainService.AddLianjie(lianjieData);
            }
        }

        private bool Frm_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            var dl = Convert.ToDouble(e.Form["Length"]);
            if (dl <= 0)
            {
                e.Form.SetEditorFocus("Length");
                ShowWarningTip("長さはゼロ以下にすることはできません");
                return false;
            }
            //if (string.IsNullOrEmpty(e.Form["Length"].ToString()))
            //{
            //    e.Form.SetEditorFocus("ProjectName");
            //    ShowWarningTip("プロジェクトを空にすることはできません");
            //    return false;
            //}
            //if (Convert.ToDouble(e.Form["UnitPrice"]) == 0)
            //{
            //    e.Form.SetEditorFocus("単価");
            //    ShowWarningTip("単価をゼロにすることはできません");
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            await ExcelExporter();
        }


        private async Task ExcelExporter()
        {
            
            var materdatas = await jwqitaService.GetMaterialDataAsync(); 
            FileStream file = new FileStream(@"lianjietemplate.xlsx", FileMode.Open, FileAccess.Read);
            XSSFWorkbook hssfworkbook = new XSSFWorkbook(file);
            ISheet XSSFSheet = hssfworkbook.GetSheetAt(0);
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "RGB COMPANY";
            //hssfworkbook.se.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Quotation";
            string filename = string.Format("{0}.xlsx", _subData.FloorName);
            //FileDto files = new FileDto(filename, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //var workbook = new XSSFWorkbook();
            int i = 1;
            //var subss = mainData.JwBudgetSubDatas.OrderBy(t => t.MaterialType).ToList();
            if (_subData != null)
            {
                if (_subData.JwLianjieDatas.Count > 0)
                {
                    var ljs = _subData.JwLianjieDatas;

                    var ljgs = ljs.GroupBy(t => t.Length).ToList();
                    double alllength = 0;
                    int allnum = 0;
                    foreach (var lj in ljgs)
                    {
                        IRow c = XSSFSheet.CopyRow(8, 8 + i);

                        c.GetCell(3).SetCellValue(lj.Key.ToString());
                        var sl = lj.Count();
                        c.GetCell(5).SetCellValue(sl.ToString());
                        c.GetCell(6).SetCellValue(sl.ToString());
                        double alslc = lj.Key * sl;
                        c.GetCell(9).SetCellValue(alslc.ToString());
                        alllength = alllength + alslc;
                        allnum += sl;
                        i++;
                    }

                    IRow onerow = XSSFSheet.GetRow(8);
                    onerow.GetCell(11).SetCellValue(allnum);
                    onerow.GetCell(12).SetCellValue(alllength.ToString());
                    XSSFSheet.GetRow(9).GetCell(0).SetCellValue(_subData.FloorName);
                    var zl = Math.Round(alllength / 1000 * 1.15, 0);
                    IRow c1 = XSSFSheet.GetRow(8 + i + 1);
                    string zls = string.Format("{0}KG", zl);
                    c1.GetCell(12).SetCellValue(zls);
                    IRow c2 = XSSFSheet.GetRow(8 + i + 1);
                    c2.GetCell(5).SetCellValue(allnum);
                    c2.GetCell(6).SetCellValue(allnum);
                    c2.GetCell(12).SetCellValue(zls);
                    IRow c3 = XSSFSheet.GetRow(8 + i + 2);
                    c3.GetCell(4).SetCellValue(alllength);

                }

                string compname = _subData.JwProjectMainData.JwCustomerData?.CompanyName;
                IRow c0 = XSSFSheet.GetRow(0);
                c0.GetCell(0).SetCellValue(compname);
                c0.GetCell(5).SetCellValue(DateTime.Now.ToShortDateString());
                string siteadr = _subData.JwProjectMainData.SiteAddress;
                if (!string.IsNullOrEmpty(siteadr))
                {
                    XSSFSheet.GetRow(4).GetCell(0).SetCellValue(siteadr);
                }

                XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9, 8 + i - 1, 0, 0));


                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel ファイル(*.xls)|*.xls|Excel ファイル(*.xlsx)|*.xlsx";
                saveFileDialog.FileName = string.Format("ブレース寸法{0}-{1}.xlsx", _subData.JwProjectMainData.ProjectName, _subData.FloorName);
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        hssfworkbook.Write(stream);
                        //_tempFileCacheManager.SetFile(files.FileToken, stream.ToArray());
                    }
                }
                UIMessageBox.ShowSuccess("ブレース寸法正常にエクスポートされました");
            }




            //foreach (var z in subss)
            //{
            //    IRow c = XSSFSheet.CopyRow(10, 10 + i);
            //    if (z.MaterialType == MaterialType.柱)
            //    {
            //        var tdata = materdatas.Find(t => t.Id == z.JwMaterialDataId);
            //        if (tdata != null)
            //        {
            //            c.GetCell(0).SetCellValue(tdata.JwMaterialTypeData.MaterialTypeName);
            //        }

            //        c.GetCell(1).SetCellValue(z.BudgetItemName);
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrEmpty(z.BudgetItemName))
            //        {
            //            c.GetCell(0).SetCellValue(z.BudgetItemName);
            //            XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10 + i, 10 + i, 0, 3));
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(z.ModelParm))
            //    {
            //        c.GetCell(4).SetCellValue(z.ModelParm);
            //    }
            //    if (!string.IsNullOrEmpty(z.Number.ToString()))
            //    {
            //        c.GetCell(6).SetCellValue(z.Number.ToString());
            //    }
            //    if (!string.IsNullOrEmpty(z.UnitName))
            //    {
            //        c.GetCell(8).SetCellValue(z.UnitName);
            //    }
            //    if (!string.IsNullOrEmpty(z.UnitPrice.ToString()))
            //    {
            //        c.GetCell(9).SetCellValue(z.UnitPrice.ToString());
            //    }
            //    c.GetCell(10).SetCellValue(z.Amount.ToString());
            //    i++;
            //}
            //ICell cc = XSSFSheet.GetRow(0).GetCell(0);
            //string yu = cc.StringCellValue;
            //cc.SetCellValue(yu + mainData.ProjectName);
            ////15
            //ICell cdate = XSSFSheet.GetRow(1).GetCell(14);
            //cdate.SetCellType(CellType.String);
            //cdate.SetCellValue(mainData.CreationTime.ToShortDateString());

            //IRow xiaoji = XSSFSheet.CopyRow(10, 10 + i);

            //xiaoji.GetCell(0).SetCellValue("小  計");
            //XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10 + i, 10 + i, 0, 3));
            //double hj = Convert.ToDouble(mainData.JwBudgetSubDatas.Sum(t => t.Amount));
            //xiaoji.GetCell(10).SetCellValue(hj);
            //IRow heji = XSSFSheet.CopyRow(10, 11 + i);
            //heji.GetCell(0).SetCellValue("合  計");
            //XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(11 + i, 11 + i, 0, 3));

            //heji.GetCell(10).SetCellValue(hj);

            //IRow xiaofeishui = XSSFSheet.CopyRow(10, 12 + i);
            //xiaofeishui.GetCell(0).SetCellValue("消費税");
            //XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + i, 12 + i, 0, 3));
            //xiaofeishui.GetCell(6).SetCellValue("10");
            //xiaofeishui.GetCell(8).SetCellValue("%");

            //double zhj = hj * 0.1;
            //xiaofeishui.GetCell(10).SetCellValue(zhj);

            //IRow zheji = XSSFSheet.CopyRow(10, 13 + i);
            //zheji.GetCell(0).SetCellValue("総  合  計");
            //XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(13 + i, 13 + i, 0, 3));
            //zheji.GetCell(10).SetCellValue(hj + zhj);
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Excel ファイル(*.xls)|*.xls|Excel ファイル(*.xlsx)|*.xlsx";
            //saveFileDialog.FileName = string.Format("{0}見積書.xlsx", mainData.ProjectName);
            //if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
            //    {
            //        hssfworkbook.Write(stream);
            //        //_tempFileCacheManager.SetFile(files.FileToken, stream.ToArray());
            //    }
            //}
            //UIMessageBox.ShowSuccess("見積が正常にエクスポートされました");

            //return _jwProjectsExcelExporter.ExportToFile(jwProjectListDtos);

        }

        /// <summary>
        /// 双击链接cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var ljl = uiDataGridView1.Rows[e.RowIndex].DataBoundItem as JwLianjieData;
                if (ljl != null)
                {
                    if (ljl.CreateFrom == CreateFromType.ManuallyAdd)
                    {
                        ShowErrorNotifier("手作業で作成したものは設計図に記入できない");
                    }
                    else
                    {
                        if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                        {
                            GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                            {
                                Id = ljl.Id,
                                IsLianjie = true
                            });
                        }
                    }

                }
                //if (beam != null)
                //{
                //    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                //    {
                //        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                //        {
                //            Id = beam.Id,
                //            DrawShapeType = JwCore.DrawShapeType.Beam
                //        });
                //    }
                //}
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 输出加工csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (canvas != null && canvas.Beams.Count != 0)
            {
                var csvstr = canvas.ToProcessCsv();
                if (!string.IsNullOrEmpty(csvstr))
                {
                    SaveFileDialog saveDataSend = new SaveFileDialog();
                    // Environment.SpecialFolder.MyDocuments 表示在我的文档中
                    saveDataSend.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);   // 获取文件路径
                    saveDataSend.Filter = "*.csv|csv file";   // 设置文件类型为文本文件
                    saveDataSend.DefaultExt = ".csv";   // 默认文件的拓展名
                    saveDataSend.FileName = string.Format("{0}-3015-2.csv", _subData.FloorName);   // 文件默认名
                    if (saveDataSend.ShowDialog() == DialogResult.OK)   // 显示文件框，并且选择文件
                    {
                        string fName = saveDataSend.FileName;   // 获取文件名
                                                                // 参数1：写入文件的文件名；参数2：写入文件的内容
                        byte[] bs = Encoding.GetEncoding("UTF-8").GetBytes(csvstr);
                        bs = Encoding.Convert(Encoding.GetEncoding("UTF-8"), Encoding.Default, bs);
                        string q = Encoding.Default.GetString(bs);
                        System.IO.File.WriteAllText(fName, q, Encoding.GetEncoding("Shift-JIS"));   // 向文件中写入内容
                        AntdUI.Modal.open(new AntdUI.Modal.Config(this, "完了プロンプト", "Some contents...Some contents...Some contents...Some contents...Some contents...Some contents...Some contents...", AntdUI.TType.Success)
                        {
                            OnButtonStyle = (id, btn) =>
                            {
                                btn.BackExtend = "135, #6253E1, #04BEFE";
                            },
                            CancelText = null,
                            OkText = "YES"
                        });
                    }
                }
            }

        }

        /// <summary>
        /// 输出jw格式 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (UIMessageBox.ShowAsk("プロジェクトデータをすべてエクスポートするかどうか"))
            {

                SaveSubBeams(_subData);
                Thread.Sleep(2000);
                this.HideProcessForm();

                //if (!string.IsNullOrEmpty(_nowsavefold))
                //{
                //    UIMessageBox.ShowSuccess("エクスポートされたビーム-->" + _nowsavefold);
                //}
            }
        }

        private async void SaveSubBeams(JwProjectSubData data)
        {

            var maindata =await jwProjectMainService.GetByIdAsync(data.JwProjectMainDataId);

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.ShowProcessForm(200);
                string foldPath = dialog.SelectedPath + "\\" + maindata.ProjectName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }
                else
                {
                    Directory.Delete(foldPath, true);
                    Directory.CreateDirectory(foldPath);
                }
                var _nowsavefold = foldPath;

                await jwProjectMainService.LoadSubCollectionAsync(data);
                //this.dbContext.Entry(data).Collection(e => e.JwBeamDatas).Load();
                //this.dbContext.Entry(data).Collection(e => e.JwPillarDatas).Load();
                //this.dbContext.Entry(data).Collection(e => e.JwLinkPartDatas).Load();
                if (data.JwBeamDatas.Count > 0)
                {
                    foreach (var bd in data.JwBeamDatas)
                    {
                        await jwProjectMainService.LoadBeamCollectionAsync(bd);
                    }
                }
                string subpath = foldPath + "\\" + data.FloorName;
                if (!Directory.Exists(subpath))
                {
                    Directory.CreateDirectory(subpath);
                }
                //this.dbContext.Entry(sub).Collection(e => e.jwho).Load();
                JwCanvas jwCanvas = data.DataToCanvas();
                if (jwCanvas.Beams.Count > 0)
                {
                    var gbeams = jwCanvas.Beams.GroupBy(t => t.BeamCode).ToList();
                    foreach (var b in gbeams)
                    {
                        var bm = b.First();
                        string sl = "";
                        if (b.ToList().Count > 1)
                        {
                            sl = "(" + b.ToList().Count.ToString() + ")";
                        }
                        string wjm = string.Format("{0}{1}.jww", b.Key, sl);
                        NewJwBeamJwDraw jwDraw = new NewJwBeamJwDraw(bm);
                        jwDraw.CreateBeam();
                        if (jwDraw.Sens.Count > 0)
                        {
                            using var a = new JwwHelper.JwwWriter();

                            //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                            //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                            //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                            a.InitHeader("template.jww");
                            foreach (var s in jwDraw.Datas)
                            {
                                a.AddData(s);
                            }

                            //foreach(var b in jwDraw.Biaozhu)
                            //{
                            //    a.AddData(b);
                            //}
                            a.Write(subpath + "\\" + wjm);
                        }
                    }
                }
            }
        }

        private void uiDataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var ljl = uiDataGridView2.Rows[e.RowIndex].DataBoundItem as JwBeamData;
                if (ljl != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = ljl.Id,
                            DrawShapeType = DrawShapeType.Beam
                        });
                    }
                }
                //if (beam != null)
                //{
                //    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                //    {
                //        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                //        {
                //            Id = beam.Id,
                //            DrawShapeType = JwCore.DrawShapeType.Beam
                //        });
                //    }
                //}
            }
        }
    }
}

