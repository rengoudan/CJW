using JwCore;
using JwShapeCommon;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Pages
{
    public partial class JwBaseDataPage : BasePage
    {
        public JwBaseDataPage()
        {
            InitializeComponent();
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            UIEditOption option = new UIEditOption();
            option.AutoLabelWidth = true;
            option.Text = "追加";
            option.AddText("MaterialTypeName", "タイプ名稱", null, true);
            //option.AddText("GeneralTitle", "通用标识", null, true);
            //option.AddDouble("UnitPrice", "単価", 0);
            var matype = JwExtend.CreateBindList<MaterialType>();

            option.AddCombobox("MaterialType", "カテゴリー", matype, "Name", "Value", 0);
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
                JwMaterialTypeData materialData = new JwMaterialTypeData();
                materialData.MaterialTypeName = frm["MaterialTypeName"].ToString();

                //materialData.UnitPrice = Convert.ToDecimal(frm["UnitPrice"].ToString());
                materialData.MaterialType = (MaterialType)(frm["MaterialType"]);
                //materialData.Remark = "";
                this.dbContext.JwMaterialTypeDatas.Add(materialData);
                this.dbContext.SaveChanges();
            }
        }

        private bool Frm_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Form["MaterialTypeName"].ToString()))
            {
                e.Form.SetEditorFocus("MaterialTypeName");
                ShowWarningTip("項目名を空にすることはできません");
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

        private void JwBaseDataPage_Load(object sender, EventArgs e)
        {
            this.InitData();

            this.dbContext.Database.EnsureCreated();

            this.dbContext.JwMaterialTypeDatas.Load();

            this.jwMaterialTypeDataBindingSource.DataSource = dbContext.JwMaterialTypeDatas.Local.ToBindingList();
            uiSymbolButton2.Enabled = false;
        }

        private JwMaterialTypeData selectedmain;
        bool hasselected = false;   

        private void uiDataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex >= 0)
                {
                    uiDataGridView1.SelectedIndex = e.RowIndex;
                    selectedmain = uiDataGridView1.Rows[e.RowIndex].DataBoundItem as JwMaterialTypeData;
                    if (selectedmain != null)
                    {
                        this.dbContext.Entry(selectedmain).Collection(e => e.JwMaterialDatas).Load();
                        uiSymbolButton2.Enabled = true;
                        hasselected = true;
                    }
                }
            }
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            if(hasselected)
            {
                UIEditOption option = new UIEditOption();
                option.AutoLabelWidth = true;
                option.Text = "追加";
                option.AddText("MaterialName", "材料名称", null, true);
                option.AddText("MaterialParameter", "仕     様", null, true);
                
                option.AddText("GeneralTitle", "通用标识", null, true);
                option.AddText("UnitName", "単位", null, true);
                
                option.AddDouble("UnitPrice", "単価", 0);
                option.AddText("Remark", "摘  要", null, true);
                //var matype = JwExtend.CreateBindList<MaterialType>();

                //option.AddCombobox("MaterialType", "カテゴリー", matype, "Name", "Value", 0);
                //option.AddCombobox("Info", "关联", infoList, "Name", "Id", "2");
                //option.AddSwitch("Switch", "选择", false, "打开", "关闭");
                //option.AddComboTreeView("ComboTree", "选择", nodes, nodes[1].Nodes[1]);
                //option.AddComboCheckedListBox("checkedList", "选择", checkedItems, "CCC");
                UIEditForm frm = new UIEditForm(option);
                frm.Render();
                frm.CheckedData += Frm_subCheckedData;//校验
                frm.ShowDialog();
                if (frm.IsOK)
                {
                    JwMaterialData materialData = new JwMaterialData();
                    materialData.MaterialName = frm["MaterialName"].ToString();
                    materialData.MaterialParameter= frm["MaterialParameter"].ToString();
                    materialData.GeneralTitle = frm["GeneralTitle"].ToString();
                    materialData.UnitName = frm["UnitName"].ToString();
                    materialData.UnitPrice = Convert.ToDecimal(frm["UnitPrice"].ToString());
                    materialData.MaterialType = selectedmain.MaterialType;
                    materialData.JwMaterialTypeData = selectedmain;
                    materialData.JwMaterialTypeDataId = selectedmain.Id;
                    materialData.Remark = "";
                    this.dbContext.JwMaterialDatas.Add(materialData);
                    this.dbContext.SaveChanges();
                }
            }
        }

        private bool Frm_subCheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Form["MaterialName"].ToString()))
            {
                e.Form.SetEditorFocus("MaterialTypeName");
                ShowWarningTip("項目名を空にすることはできません");
                return false;
            }
            if (Convert.ToDouble(e.Form["UnitPrice"]) == 0)
            {
                e.Form.SetEditorFocus("単価");
                ShowWarningTip("単価をゼロにすることはできません");
                return false;
            }
            var mn = e.Form["MaterialName"].ToString();
            var mp = e.Form["MaterialParameter"].ToString();
            var fmnp=dbContext.JwMaterialDatas.Where(t => t.MaterialName == mn && t.MaterialParameter == mp);
            if (fmnp.Count() > 0)
            {
                UIMessageBox.ShowError("同じ材料名称仕様 がすでに存在します");
                return false;
            }

            return true;
        }
    }
}
