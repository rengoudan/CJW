using JwCore;
using JwServices;
using JwShapeCommon;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RGBControls.Classes;
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

namespace RGBControls.Controls
{
    public partial class Sub : UserControl
    {

        private JwProjectSubData _subdata;

        public JwProjectSubData SubData
        {
            get { return _subdata; }
            set
            {
                _subdata = value;
                if (_subdata != null)
                {
                    this.pageHeader1.Text = _subdata.FloorName;
                    this.pageHeader1.Description = "解析されたデータからさまざまな処理図または関連する CSV ファイルをエクスポートします。";
                    //this.pageHeader1.Refresh();
                }
            }
        }

        public Sub()
        {
            InitializeComponent();
        }

        public Sub(JwProjectSubData subdata)
        {
            InitializeComponent();
            SubData = subdata;
            inittable();
            //this.Resize += Sub_Resize;

        }

        private void inittable()
        {
            this.liangtable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("BeamCode", "梁番号"),
                                new AntdUI.Column("FloorName", "階"),
                                new AntdUI.Column("GongQu", "工区"),
                                new AntdUI.Column("Length", "長さ"),
                                new AntdUI.Column("XXLength", "コア長"),
                                new AntdUI.Column("StartTelosType", "終了タイプ"),
                                new AntdUI.Column("EndTelosType", "終了タイプ"),
                                new AntdUI.Column("IsQiegeBeam", "分割"),
                                new AntdUI.Column("BeamXHName", "梁のモデル"),
                                new AntdUI.Column("Id", "ID"),
                                //new AntdUI.Column("ParsedQuantity", "解析数")

            };

            this.zhutable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("PillarCode", "番号"),
                                new AntdUI.Column("BaseType", "タイプ"),
                                new AntdUI.Column("CenterLocation", "中心点"),
                                new AntdUI.Column("Length", "長さ"),
                                new AntdUI.Column("DirectionType", "方向"),
                                new AntdUI.Column("TaggTitle", "TaggTitle"),
                                //new AntdUI.Column("ParsedQuantity", "解析数")

            };
            this.lianjietable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("Start","開始座標点"),
                new AntdUI.Column("End","終了座標点"),
                new AntdUI.Column("Length","長さ(mm)"),
                new AntdUI.Column("CreateFrom","から"),
                new AntdUI.Column("Id","Id"),
            };
            this.bbgtable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("BujianName","金物"),
                new AntdUI.Column("GouJianType","タイプ"),
                new AntdUI.Column("Directed","方向"),
                new AntdUI.Column("BeamId","梁ID"),
                new AntdUI.Column("CreateFrom","起源"),
            };
        }


        private void Sub_Resize(object? sender, EventArgs e)
        {
            this.Invalidate();
            this.jwCanvasControl1.Invalidate();
        }

        private void Sub_Load(object sender, EventArgs e)
        {
            try
            {
                if (_subdata != null)
                {
                    JwCanvas jwc = _subdata.DataToCanvas();
                    JwCanvasDraw canvasDraw = new JwCanvasDraw(jwc);
                    this.jwCanvasControl1.CanvasDraw = canvasDraw;
                    //this.jwShowBeams1.ShowBeams = true;
                    //this.jwShowBeams1.ShowDownB = true;
                    //this.jwShowBeams1.ShowFuzhu = true;
                    //this.jwShowBeams1.ShowGoujian = true;
                    //this.jwShowBeams1.ShowGoujiantext = false;
                    //this.jwShowBeams1.ShowPillar = true;

                    this.liangtable.DataSource = _subdata.JwBeamDatas;

                    this.zhutable.DataSource = _subdata.JwPillarDatas;

                    this.lianjietable.DataSource = _subdata.JwLianjieDatas;

                    this.bbgtable.DataSource = _subdata.JwLinkPartDatas;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sub控件加载失败: " + ex.Message);
            }

        }

        /// <summary>
        /// 梁双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void liangtable_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var beam = liangtable[e.RowIndex - 1].record as JwBeamData;
                if (beam != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = beam.Id,
                            DrawShapeType = JwCore.DrawShapeType.Beam
                        });
                    }
                }
            }
        }

        private void zhutable_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var pillar = zhutable[e.RowIndex - 1].record as JwPillarData;
                if (pillar != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = pillar.Id,
                            DrawShapeType = JwCore.DrawShapeType.Pillar
                        });
                    }
                }
            }
        }

        private void lianjietable_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var lj = lianjietable[e.RowIndex - 1].record as JwLianjieData;
                if (lj != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = lj.Id,
                            IsLianjie = true
                        });
                    }
                }

            }
        }

        private void bbgtable_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var beam = bbgtable[e.RowIndex - 1].record as JwLinkPartData;
                if (beam != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = beam.Id,
                            DrawShapeType = JwCore.DrawShapeType.LinkPart
                        });
                    }
                }

            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (AntdUI.Modal.open(this.ParentForm, "ヒント", "プロジェクトデータをすべてエクスポートするかどうか") == DialogResult.OK)
            {
                await Progress(async () => { await SaveSubBeams(_subdata); });
                if (!string.IsNullOrEmpty(_nowsavefold))
                {
                    this.ParentForm.SuccessModal("エクスポートされたビーム-->" + _nowsavefold);
                    //UIMessageBox.ShowSuccess("エクスポートされたビーム-->" + _nowsavefold);
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (_subdata != null)
            {
                await Progress(async () => { await SaveSubTopCanvas(_subdata); });
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (_subdata != null)
            {
                await Progress(async () => { await SaveSubBottomCanvas(_subdata); });
                //SaveSubBottomCanvas(selectedsubData);
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (_subdata != null)
            {
                await Progress(async () => { await SaveSubCanvasLines(_subdata); });
                //SaveSubCanvasLines(selectedsubData);
            }
        }

        private async Task Progress(Action action)
        {
            await AntdUI.Spin.open(this.ParentForm, AntdUI.Localization.Get("Loading2", "読み込み中..."), async config =>
            {
                Thread.Sleep(100);
                this.Invoke(() =>
                {
                    action();
                });
                for (int i = 0; i < 101; i++)
                {
                    config.Value = i / 100F;
                    config.Text = AntdUI.Localization.Get("Processing", "処理") + " " + i + "%";
                    Thread.Sleep(20);
                }
                Thread.Sleep(1000);
                config.Value = null;
                config.Text = AntdUI.Localization.Get("PleaseWait", "お待ちください。...");
                Thread.Sleep(2000);
            }, () =>
            {
                System.Diagnostics.Debug.WriteLine("仕上げる");
            });
        }

        #region 各类jww及施工图导出

        private string _nowsavefold = "";
        private async Task SaveSubBeams(JwProjectSubData data)
        {

            var maindata = _subdata.JwProjectMainData;

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //this.ShowProcessForm(200);
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
                _nowsavefold = foldPath;


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

        private async Task SaveSubTopCanvas(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath + "\\" + data.FloorName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }

                //await JwProjectMainService.LoadSubCollectionAsync(data);
                //if (data.JwBeamDatas.Count > 0)
                //{
                //    foreach (var bd in data.JwBeamDatas)
                //    {
                //        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                //    }
                //}
                JwCanvas jwCanvas = data.DataToCanvas();
                jwCanvas.BindDrawOtherEvent();
                var lst = jwCanvas.DrawShigong(true);
                using var a = new JwwHelper.JwwWriter();
                //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                a.InitHeader("template.jww");
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu+
                //)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + data.FloorName + ".jww");
            }
        }

        private async Task SaveSubBottomCanvas(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath + "\\" + data.FloorName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }

                //await JwProjectMainService.LoadSubCollectionAsync(data);
                //if (data.JwBeamDatas.Count > 0)
                //{
                //    foreach (var bd in data.JwBeamDatas)
                //    {
                //        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                //    }
                //}
                JwCanvas jwCanvas = data.DataToCanvas();
                jwCanvas.BindDrawOtherEvent();
                var lst = jwCanvas.DrawShigong(false);
                using var a = new JwwHelper.JwwWriter();
                //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                a.InitHeader("template.jww");
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu+
                //)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + data.FloorName + ".jww");
            }
        }
        private async Task SaveSubCanvasLines(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            string flname = string.Format("{0}_ブレース施工図.jww", data.FloorName);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath + "\\" + data.FloorName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }
                //await JwProjectMainService.LoadSubCollectionAsync(data);
                //if (data.JwBeamDatas.Count > 0)
                //{
                //    foreach (var bd in data.JwBeamDatas)
                //    {
                //        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                //    }
                //}
                JwCanvas jwCanvas = data.DataToCanvas();
                jwCanvas.BindDrawOtherEvent();
                var lst = jwCanvas.DrawLinesToJww();
                using var a = new JwwHelper.JwwWriter();
                //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                a.InitHeader("template.jww");
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + flname);
            }
        }



        #endregion

        public JwqitaService jwqitaService => ServiceFactory.GetInstance().CreateJwqitaService();

        public JwProjectMainService jwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();

        private async void button5_Click(object sender, EventArgs e)
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
                lianjieData.JwProjectSubDataId = _subdata.Id;
                lianjieData.ProjectSubName = _subdata.FloorName;
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
                //ShowWarningTip("長さはゼロ以下にすることはできません");
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
            string filename = string.Format("{0}.xlsx", _subdata.FloorName);
            //FileDto files = new FileDto(filename, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //var workbook = new XSSFWorkbook();
            int i = 1;
            //var subss = mainData.JwBudgetSubDatas.OrderBy(t => t.MaterialType).ToList();
            if (_subdata != null)
            {
                if (_subdata.JwLianjieDatas.Count > 0)
                {
                    var ljs = _subdata.JwLianjieDatas;

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

                string compname = _subdata.JwProjectMainData.JwCustomerData?.CompanyName;
                IRow c0 = XSSFSheet.GetRow(0);
                c0.GetCell(0).SetCellValue(compname);
                c0.GetCell(5).SetCellValue(DateTime.Now.ToShortDateString());
                string siteadr = _subdata.JwProjectMainData.SiteAddress;
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

        }

        private async void button6_Click(object sender, EventArgs e)
        {
            await ExcelExporter();
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            await ExcelExporter();
        }
    }
}
