using JwCore;
using JwServices;
using JwShapeCommon;
using Microsoft.EntityFrameworkCore;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RGBJWMain.Forms;
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
    public partial class JwBudgePage : BasePage
    {
        public JwBudgePage()
        {
            InitializeComponent();
        }

        public JwqitaService jwqitaService => ServiceFactory.GetInstance().CreateJwqitaService();

        public JwProjectMainService jwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();

        private async void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            UIEditOption option = new UIEditOption();
            option.AutoLabelWidth = true;
            option.Text = "予算の作成";
            var cuslst =await jwProjectMainService.GetAllAsync();

            option.AddCombobox("ProjectMainId", "プロジェクト", cuslst, "ProjectName", "Id", 0);

            var matype = JwExtend.CreateBindList<MaterialType>();

            //option.AddCombobox("MaterialType", "種類", matype, "Name", "Value", 0);
            //option.AddCombobox("Info", "关联", infoList, "Name", "Id", "2");
            //option.AddSwitch("Switch", "选择", false, "打开", "关闭");
            //option.AddComboTreeView("ComboTree", "选择", nodes, nodes[1].Nodes[1]);
            //option.AddComboCheckedListBox("checkedList", "选择", checkedItems, "CCC");
            UIEditForm frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += Frm_CheckedData;//校验
            frm.ShowDialog();
            if (frm.IsOK)
            {
                //JwCustomerData customerdata = new JwCustomerData();
                var id = Convert.ToInt64(frm["ProjectMainId"]);
                var maindata =await jwProjectMainService.GetByIdAsync(id);
                var budgetlst =await jwqitaService.GetAllJwBudgetMainDataAsync(t => t.JwProjectMainDataId == id);
                if (maindata != null&&budgetlst.Count()==0)
                {
                    await jwProjectMainService.LoadSubDataAsync(maindata);
                    if (maindata.JwProjectSubDatas.Count > 0)
                    {
                        CreateBudge(maindata);
                    }
                    else
                    {
                        UIMessageBox.ShowError("プロジェクトの図面はまだ解決されていません!");
                    }
                }
                else
                {
                    if (budgetlst.Count() != 0)
                    {
                        if (UIMessageBox.ShowAsk("このプロジェクトの予算はすでに存在します。再生成しますか?"))
                        {
                            UIMessageBox.ShowError("この機能は開発中です");
                        }
                    }
                }
                //customerdata.CompanyAddress = frm["CompanyAddress"].ToString();
                //customerdata.Contact = frm["Contact"].ToString();
                //customerdata.Telephone = frm["Telephone"].ToString();
                //this.dbContext.JwCustomerDatas.Add(customerdata);
                //this.dbContext.SaveChanges();
            }
        }
        private bool Frm_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Form["ProjectMainId"].ToString()))
            {
                e.Form.SetEditorFocus("ProjectMainId");
                ShowWarningTip("会社名を空にすることはできません");
                return false;
            }
            //if (Convert.ToDouble(e.Form["UnitPrice"]) == 0)
            //{
            //    e.Form.SetEditorFocus("単価");
            //    ShowWarningTip("単価をゼロにすることはできません");
            //    return false;
            //}

            return true;
        }

        private async void JwBudgePage_Load(object sender, EventArgs e)
        {
            InitData();
            
            this.jwBudgetMainDataBindingSource.DataSource = await jwqitaService.GetAllJwBudgetMainDataAsync(null);
            this.uiDataGridView1.ClearSelection();
            this.uiDataGridView1.SelectedIndex = -1;
            this.panel3.Visible = false;
        }

        /// <summary>
        /// 根据识别 自动生成预算项
        /// </summary>
        /// <param name="data"></param>
        private async Task CreateBudge(JwProjectMainData data)
        {
            //var materdatas = dbContext.JwMaterialDatas.Include(t => t.JwMaterialTypeData).ToList();
            //this.dbContext.Entry(data).Collection(e => e.JwProjectSubDatas).Load();
            var wls = await jwqitaService.GetMaterialDataAsync();
            var subs = await jwProjectMainService.GetSubDatasAsync(t=>t.JwProjectMainDataId == data.Id);
            var subids = subs.Select(t => t.Id).ToList();
            var beams = await jwProjectMainService.GetBeamDatasAsync(t => subids.Contains(t.JwProjectSubDataId));
            var pillars = await jwProjectMainService.GetPillarDatasAsync(t => subids.Contains(t.JwProjectSubDataId));
            var links = await jwProjectMainService.GetLinkPartDatasAsync(t => subids.Contains(t.JwProjectSubDataId));
            JwBudgetMainData budget = new JwBudgetMainData();
            budget.JwProjectMainDataId = data.Id;
            budget.ProjectName = data.ProjectName;
            await jwqitaService.AddJwBudgetMainDataAsync(budget);

            var ltlst = links.GroupBy(t => t.GouJianType).ToList();

            foreach (var link in ltlst)
            {
                var z = link.Key.ToString();
                var pwl = wls.Where(t => t.GeneralTitle.Equals(link.Key.ToString())).FirstOrDefault();
                if (pwl == null)
                {
                    UIMessageBox.ShowError(link.Key.ToString() + "のマテリアル情報は存在しません");
                    return;
                }
                JwBudgetSubData plbudget = new JwBudgetSubData();
                plbudget.BudgetItemName = pwl.MaterialName;
                plbudget.Number = link.Count();
                plbudget.UnitPrice = pwl.UnitPrice;
                plbudget.UnitName = pwl.UnitName;
                plbudget.Amount = plbudget.Number * plbudget.UnitPrice;
                plbudget.JwBudgetMainDataId = budget.Id;
                plbudget.BudgetType = BudgetFrom.自動識別;
                plbudget.JwMaterialDataId = pwl.Id;
                plbudget.ModelParm = pwl.MaterialParameter;
                plbudget.MaterialType = pwl.MaterialType;
                budget.Amount += plbudget.Amount;
                await jwqitaService.AddJwBudgetSubDataAsync(plbudget);
            }

            var beamgpnames = beams.GroupBy(t => t.BeamXHId).ToList();
            foreach (var beam in beamgpnames)
            {
                var beamlength = beam.Sum(t => t.Length);
                var wl = wls.Where(t => t.Id.Equals(beam.Key)).First();
                if (wl == null)
                {
                    UIMessageBox.ShowError(beam.Key + "のマテリアル情報は存在しません");
                    return;
                }
                JwBudgetSubData beambudget = new JwBudgetSubData();
                beambudget.BudgetItemName = wl.MaterialName;
                beambudget.Number = Convert.ToDecimal(beamlength / 1000);
                //beambudget.UnitPrice
                beambudget.BudgetType = BudgetFrom.自動識別;
                beambudget.UnitPrice = wl.UnitPrice;
                beambudget.UnitName = wl.UnitName;
                beambudget.Amount = beambudget.Number * beambudget.UnitPrice;
                beambudget.JwBudgetMainDataId = budget.Id;
                beambudget.JwMaterialDataId = wl.Id;
                beambudget.ModelParm = wl.MaterialParameter;
                beambudget.MaterialType = wl.MaterialType;
                budget.Amount += beambudget.Amount;
                await jwqitaService.AddJwBudgetSubDataAsync(beambudget);
                //beambudget.UnitName=
            }

            var pillarnames = pillars.GroupBy(t => t.TaggTitle).ToList();
            foreach (var pillar in pillarnames)
            {
                if (string.IsNullOrEmpty(pillar.Key))
                {

                }
                else
                {
                    var pwl = wls.Where(t => t.GeneralTitle.Equals(pillar.Key)).First();
                    if (pwl == null)
                    {
                        UIMessageBox.ShowError(pillar.Key + "のマテリアル情報は存在しません");
                        return;
                    }


                    JwBudgetSubData plbudget = new JwBudgetSubData();
                    plbudget.BudgetItemName = pwl.MaterialName;
                    plbudget.Number = pillar.Count();
                    plbudget.BudgetType = BudgetFrom.自動識別;
                    plbudget.UnitPrice = pwl.UnitPrice;
                    plbudget.UnitName = pwl.UnitName;
                    plbudget.Amount = plbudget.Number * plbudget.UnitPrice;
                    plbudget.JwBudgetMainDataId = budget.Id;
                    plbudget.JwMaterialDataId = pwl.Id;
                    plbudget.ModelParm = pwl.MaterialParameter;
                    plbudget.MaterialType = pwl.MaterialType;
                    //plbudget
                    budget.Amount += plbudget.Amount;
                    await jwqitaService.AddJwBudgetSubDataAsync(plbudget);
                }

            }
            await jwqitaService.UpdateJwBudgetMainDataAsync(budget);

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (rightselectedindex != -1)
            {

            }
        }

        int rightselectedindex = -1;

        private void uiDataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    rightselectedindex = e.RowIndex;
                    var main = uiDataGridView1.Rows[e.RowIndex].DataBoundItem as JwBudgetMainData;
                    if (main != null)
                    {
                        contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                        isselected = true;
                        selectedmaiandata = main;
                    }

                }
            }
        }

        private JwBudgetMainData? selectedmaiandata;
        private bool isselected = false;

        private async void uiDataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex >= 0)
                {
                    var main = uiDataGridView1.Rows[e.RowIndex].DataBoundItem as JwBudgetMainData;
                    if (main != null)
                    {
                        isselected = true;
                        selectedmaiandata = main;
                        await jwqitaService.LoadBudgetSubCollectionAsync(main);
                        this.uilbprojectname.Text = main.ProjectName + "予算の詳細";
                        this.panel3.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// 导出预算excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            //ExcelExporter
            if (isselected)
            {
                ExcelExporter(selectedmaiandata);
            }
            else
            {
                UIMessageBox.ShowError("有効なデータが選択されていません");
            }
        }

        private async void ExcelExporter(JwBudgetMainData mainData)
        {
            var materdatas =await jwqitaService.GetMaterialDataAsync();
            FileStream file = new FileStream(@"template.xlsx", FileMode.Open, FileAccess.Read);
            XSSFWorkbook hssfworkbook = new XSSFWorkbook(file);
            ISheet XSSFSheet = hssfworkbook.GetSheetAt(0);
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "RGB COMPANY";
            //hssfworkbook.se.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Quotation";
            string filename = string.Format("{0}.xlsx", mainData.ProjectName);
            //FileDto files = new FileDto(filename, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //var workbook = new XSSFWorkbook();
            int i = 1;
            var subss = mainData.JwBudgetSubDatas.OrderBy(t => t.MaterialType).ToList();
            foreach (var z in subss)
            {
                IRow c = XSSFSheet.CopyRow(10, 10 + i);
                if (z.MaterialType == MaterialType.柱)
                {
                    var tdata = materdatas.Find(t => t.Id == z.JwMaterialDataId);
                    if (tdata != null)
                    {
                        c.GetCell(0).SetCellValue(tdata.JwMaterialTypeData.MaterialTypeName);
                    }

                    c.GetCell(1).SetCellValue(z.BudgetItemName);
                }
                else
                {
                    if (!string.IsNullOrEmpty(z.BudgetItemName))
                    {
                        c.GetCell(0).SetCellValue(z.BudgetItemName);
                        XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10 + i, 10 + i, 0, 3));
                    }
                }

                if (!string.IsNullOrEmpty(z.ModelParm))
                {
                    c.GetCell(4).SetCellValue(z.ModelParm);
                }
                if (!string.IsNullOrEmpty(z.Number.ToString()))
                {
                    c.GetCell(6).SetCellValue(z.Number.ToString());
                }
                if (!string.IsNullOrEmpty(z.UnitName))
                {
                    c.GetCell(8).SetCellValue(z.UnitName);
                }
                if (!string.IsNullOrEmpty(z.UnitPrice.ToString()))
                {
                    c.GetCell(9).SetCellValue(z.UnitPrice.ToString());
                }
                c.GetCell(10).SetCellValue(z.Amount.ToString());
                i++;
            }
            ICell cc = XSSFSheet.GetRow(0).GetCell(0);
            string yu = cc.StringCellValue;
            cc.SetCellValue(yu + mainData.ProjectName);
            //15
            ICell cdate = XSSFSheet.GetRow(1).GetCell(14);
            cdate.SetCellType(CellType.String);
            cdate.SetCellValue(mainData.CreationTime.ToShortDateString());

            IRow xiaoji = XSSFSheet.CopyRow(10, 10 + i);

            xiaoji.GetCell(0).SetCellValue("小  計");
            XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10 + i, 10 + i, 0, 3));
            double hj = Convert.ToDouble(mainData.JwBudgetSubDatas.Sum(t => t.Amount));
            xiaoji.GetCell(10).SetCellValue(hj);
            IRow heji = XSSFSheet.CopyRow(10, 11 + i);
            heji.GetCell(0).SetCellValue("合  計");
            XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(11 + i, 11 + i, 0, 3));

            heji.GetCell(10).SetCellValue(hj);

            IRow xiaofeishui = XSSFSheet.CopyRow(10, 12 + i);
            xiaofeishui.GetCell(0).SetCellValue("消費税");
            XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + i, 12 + i, 0, 3));
            xiaofeishui.GetCell(6).SetCellValue("10");
            xiaofeishui.GetCell(8).SetCellValue("%");

            double zhj = hj * 0.1;
            xiaofeishui.GetCell(10).SetCellValue(zhj);

            IRow zheji = XSSFSheet.CopyRow(10, 13 + i);
            zheji.GetCell(0).SetCellValue("総  合  計");
            XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(13 + i, 13 + i, 0, 3));
            zheji.GetCell(10).SetCellValue(hj + zhj);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel ファイル(*.xls)|*.xls|Excel ファイル(*.xlsx)|*.xlsx";
            saveFileDialog.FileName = string.Format("{0}見積書.xlsx", mainData.ProjectName);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    hssfworkbook.Write(stream);
                    //_tempFileCacheManager.SetFile(files.FileToken, stream.ToArray());
                }
            }
            UIMessageBox.ShowSuccess("見積が正常にエクスポートされました");

            //return _jwProjectsExcelExporter.ExportToFile(jwProjectListDtos);

        }

        /// <summary>
        /// 新增预算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            if (isselected)
            {
                AddBudget addBudget = new AddBudget(selectedmaiandata);
                if (addBudget.ShowDialog() == DialogResult.OK)
                {
                    UIMessageTip.ShowOk("予算が正常に追加されました");
                }
            }
            
        }
    }
}
