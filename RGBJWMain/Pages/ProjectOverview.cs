using JwCore;
using JwData;
using JwShapeCommon;
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
    public partial class ProjectOverview : UIPage
    {
        public JwDataContext? dbContext;
        public ProjectOverview()
        {
            InitializeComponent();
        }

        private JwProjectMainData _projectMainData;

        public ProjectOverview(JwProjectMainData projectMainData)
        {
            InitializeComponent();
            dbContext = ContextFactory.GetContext();
            _projectMainData = projectMainData;
            init();
        }

        private void init()
        {
            if (_projectMainData != null)
            {
                //this.uiLine1.Text = _projectMainData.ProjectName;
                this.Text = string.Format("{0} -プロジェクトの詳細", _projectMainData.ProjectName);
                this.uiMarkLabel2.Text = _projectMainData.JwProjectSubDatas.Count.ToString();
                this.uiMarkLabel3.Text = _projectMainData.BeamsNumber.ToString();
                this.uiMarkLabel4.Text = _projectMainData.PillarCount.ToString();
            }
        }

        /// <summary>
        /// 输出梁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (_projectMainData != null)
            {
                SaveBeams(_projectMainData);
            }
        }

        /// <summary>
        /// 导出连接线excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            if(_projectMainData!=null)
            {
                FileStream file = new FileStream(@"lianjietemplate.xlsx", FileMode.Open, FileAccess.Read);
                XSSFWorkbook hssfworkbook = new XSSFWorkbook(file);
                ISheet XSSFSheet = hssfworkbook.GetSheetAt(0);
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "RGB COMPANY";
                //hssfworkbook.se.DocumentSummaryInformation = dsi;

                //create a entry of SummaryInformation
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Subject = "Quotation";
                int i = 1;
                
                //FileDto files = new FileDto(filename, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
                //var workbook = new XSSFWorkbook();
                int mainallnum = 0;
                double mainalllength = 0;
                double mainallzl = 0;
                if (_projectMainData.JwProjectSubDatas.Count>0)
                {
                    foreach(var sub in _projectMainData.JwProjectSubDatas)
                    {
                        if (sub.JwLianjieDatas.Count > 0)
                        {
                            var ljs = sub.JwLianjieDatas;

                            var ljgs = ljs.GroupBy(t => t.Length).ToList();
                            double alllength = 0;
                            int allnum = 0;
                            int j = 0;//统计楼层内 连接线的数量
                            foreach (var lj in ljgs)
                            {
                                IRow c = XSSFSheet.CopyRow(8, 8+ i);
                                if(j== 0)
                                {
                                    c.GetCell(0).SetCellValue(sub.FloorName);
                                }
                                c.GetCell(3).SetCellValue(lj.Key.ToString());
                                var sl = lj.Count();
                                c.GetCell(5).SetCellValue(sl.ToString());
                                c.GetCell(6).SetCellValue(sl.ToString());
                                double alslc = lj.Key * sl;
                                c.GetCell(9).SetCellValue(alslc.ToString());
                                alllength = alllength + alslc;
                                allnum += sl;
                                i++;
                                j++;
                            }

                            IRow onerow = XSSFSheet.CopyRow(8,8+i);
                            onerow.GetCell(11).SetCellValue(allnum);
                            onerow.GetCell(12).SetCellValue(alllength.ToString());
                            i++;

                            var zl = Math.Round(alllength / 1000 * 1.15, 0);
                            IRow c1 = XSSFSheet.CopyRow(8, 8 + i);
                            string zls = string.Format("{0}KG", zl);
                            c1.GetCell(12).SetCellValue(zls);
                            i++;
                            
                            mainallnum+= allnum;
                            mainalllength += alllength;
                            mainallzl += zl;
                            XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(8 + i - 2 - j, 8 + i - 1, 0, 0));
                        }

                        


                    }
                    IRow c2 = XSSFSheet.GetRow(8 + i + 1);
                    c2.GetCell(5).SetCellValue(mainallnum);
                    c2.GetCell(6).SetCellValue(mainallnum);
                    c2.GetCell(12).SetCellValue(mainallzl);
                    IRow c3 = XSSFSheet.GetRow(8 + i + 2);
                    c3.GetCell(4).SetCellValue(mainalllength);

                    var _subData = _projectMainData.JwProjectSubDatas.FirstOrDefault();
                    if (_subData == null)
                    {
                        UIMessageBox.ShowWarning("プロジェクトにサブデータがありません。");
                        return;
                    }
                }
                else
                {
                    UIMessageBox.ShowWarning("プロジェクトにサブデータがありません。");
                    return;
                }
                string compname = _projectMainData.JwCustomerData?.CompanyName;
                IRow c0 = XSSFSheet.GetRow(0);
                c0.GetCell(0).SetCellValue(compname);
                c0.GetCell(5).SetCellValue(DateTime.Now.ToShortDateString());
                string siteadr = _projectMainData.SiteAddress;
                if (!string.IsNullOrEmpty(siteadr))
                {
                    XSSFSheet.GetRow(4).GetCell(0).SetCellValue(siteadr);
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel ファイル(*.xls)|*.xls|Excel ファイル(*.xlsx)|*.xlsx";
                saveFileDialog.FileName = string.Format("ブレース寸法-{0}-all.xlsx", _projectMainData.ProjectName);
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        hssfworkbook.Write(stream);
                        //_tempFileCacheManager.SetFile(files.FileToken, stream.ToArray());
                    }
                }
                UIMessageBox.ShowSuccess("ブレース寸法正常にエクスポートされました");





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
        }

        /// <summary>
        /// 增加新项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {

        }

        private string _nowsavefold = "";
        private void SaveBeams(JwProjectMainData data)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.ShowProcessForm(200);
                string foldPath = dialog.SelectedPath + "\\" + data.ProjectName;
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
                foreach (var sub in data.JwProjectSubDatas)
                {

                    //this.dbContext.Entry(sub).Collection(e => e.JwBeamDatas).Load();
                    //this.dbContext.Entry(sub).Collection(e => e.JwPillarDatas).Load();
                    //this.dbContext.Entry(sub).Collection(e => e.JwLinkPartDatas).Load();
                    if (sub.JwBeamDatas.Count > 0)
                    {
                        foreach (var bd in sub.JwBeamDatas)
                        {
                            this.dbContext.Entry(bd).Collection(e => e.JwHoles).Load();
                        }
                    }
                    string subpath = foldPath + "\\" + sub.FloorName;
                    if (!Directory.Exists(subpath))
                    {
                        Directory.CreateDirectory(subpath);
                    }
                    //this.dbContext.Entry(sub).Collection(e => e.jwho).Load();
                    JwCanvas jwCanvas = sub.DataToCanvas();
                    //foreach(var bj in jwCanvas.Beams)
                    //{

                    //}
                    if (jwCanvas.Beams.Count > 0)
                    {
                        var gbeams = jwCanvas.Beams.GroupBy(t => t.BeamCode).ToList();
                        foreach (var b in gbeams)
                        {
                            var bm = b.First();
                            string sl = "";
                            if (b.ToList().Count > 1)
                            {
                                sl = b.ToList().Count.ToString();
                            }
                            string wjm = string.Format("{0}{1}.jww", b.Key, sl);
                            //JwBeamJwDraw jwDraw = new JwBeamJwDraw(bm);
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
                        //foreach (var b in jwCanvas.Beams)
                        //{
                        //    JwBeamJwDraw jwDraw = new JwBeamJwDraw(b);
                        //    jwDraw.Draw();
                        //    if (jwDraw.Sens.Count > 0)
                        //    {
                        //        using var a = new JwwHelper.JwwWriter();

                        //        //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                        //        //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                        //        //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                        //        a.InitHeader("template.jww");
                        //        foreach (var s in jwDraw.Datas)
                        //        {
                        //            a.AddData(s);
                        //        }

                        //        //foreach(var b in jwDraw.Biaozhu)
                        //        //{
                        //        //    a.AddData(b);
                        //        //}
                        //        a.Write(subpath + "\\" + b.BeamCode + ".jww");
                        //    }
                        //}
                    }

                }
            }
        }

        private void ProjectOverview_Load(object sender, EventArgs e)
        {
            if (_projectMainData!=null)
            {
                this.jwProjectSubDataBindingSource.DataSource = _projectMainData.JwProjectSubDatas;
                //this.jwBeamDatasBindingSource.DataSource = _projectMainData.JwProjectSubDatas;
                ObservableCollectionListSource<JwBeamData> beamDatas = new ObservableCollectionListSource<JwBeamData>();
                ObservableCollectionListSource<JwPillarData> pillarDatas = new ObservableCollectionListSource<JwPillarData>();
                ObservableCollectionListSource<JwLianjieData> linkDatas = new ObservableCollectionListSource<JwLianjieData>();
                foreach (var sub in _projectMainData.JwProjectSubDatas)
                {
                    if (sub.JwBeamDatas != null && sub.JwBeamDatas.Count > 0)
                    {
                        foreach (var bd in sub.JwBeamDatas)
                        {
                            beamDatas.Add(bd);
                        }
                    }
                    if (sub.JwPillarDatas != null && sub.JwPillarDatas.Count > 0)
                    {
                        foreach (var pd in sub.JwPillarDatas)
                        {
                            pillarDatas.Add(pd);
                        }
                    }
                    if (sub.JwLianjieDatas != null && sub.JwLianjieDatas.Count > 0)
                    {
                        foreach (var ld in sub.JwLianjieDatas)
                        {
                            linkDatas.Add(ld);
                        }
                    }
                }
                this.jwBeamDatasBindingSource.DataSource= beamDatas;
                this.jwPillarDatasBindingSource.DataSource = pillarDatas;
                this.jwLianjieDatasBindingSource.DataSource = linkDatas;
                //this.uiDataGridView4.DataSource = beamDatas;
            }
        }


    }
}
