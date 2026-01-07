using JwCore;
using JwServices;
using JwShapeCommon;
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

namespace RGBControls.Pages
{
    public partial class NewBaseDataPage : UIPage
    {
        public NewBaseDataPage()
        {
            InitializeComponent();
            initform();
        }

        public JwqitaService jwqitaService => ServiceFactory.GetInstance().CreateJwqitaService();

        private async void button1_Click(object sender, EventArgs e)
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
                await jwqitaService.AddJwMaterialTypeDataAsync(materialData);
                await initdata();
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

        private async void button2_Click(object sender, EventArgs e)
        {
            if (_selected!=null)
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
                    materialData.MaterialParameter = frm["MaterialParameter"].ToString();
                    materialData.GeneralTitle = frm["GeneralTitle"].ToString();
                    materialData.UnitName = frm["UnitName"].ToString();
                    materialData.UnitPrice = Convert.ToDecimal(frm["UnitPrice"].ToString());
                    materialData.MaterialType = _selected.MaterialType;
                    //materialData.JwMaterialTypeData = _selected;
                    materialData.JwMaterialTypeDataId = _selected.Id;
                    materialData.Remark = "";
                    await jwqitaService.AddJwMaterialDataAsync(materialData);
                    await initdata();
                }
            }
        }

        private  bool Frm_subCheckedData(object sender, UIEditForm.EditFormEventArgs e)
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
            
            var fmnp = jwqitaService.GetMaterialData(t => t.MaterialName == mn && t.MaterialParameter == mp);
            if (fmnp.Count() > 0)
            {
                UIMessageBox.ShowError("同じ材料名称仕様 がすでに存在します");
                return false;
            }

            return true;
        }

        private async void NewBaseDataPage_Load(object sender, EventArgs e)
        {
           await initdata();
            button2.Enabled = false;
        }

        JwMaterialTypeData _selected;

        private void initform()
        {
            table1.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("MaterialTypeName", "データの種類"),
                                new AntdUI.Column("MaterialCount", "量"),
                                new AntdUI.Column("CreationTime", "CreationTime"),
                                new AntdUI.Column("DefaultDataId", "DefaultDataId"),

            };
            table2.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("MaterialName", "材料名称"),
                                new AntdUI.Column("MaterialParameter", "仕       様"),
                                new AntdUI.Column("GeneralTitle", "識別子"),
                                new AntdUI.Column("UnitPrice", "単価"),
                                new AntdUI.Column("UnitName", "単 位"),
                                new AntdUI.Column("Remark", "摘  要"),
                                new AntdUI.Column("CreationTime", "CreationTime"),

            };
        }

        private async Task initdata()
        {
            var lst = await jwqitaService.GetJwMaterialTypeDatasAsync();
            table1.DataSource = lst;
            if (_selected != null)
            {
                await jwqitaService.LoadSubDataAsync(_selected);
                table2.DataSource = _selected.JwMaterialDatas;
            }
        }

        private async void table1_SelectIndexChanged(object sender, EventArgs e)
        {
            if (table1.SelectedIndex > 0)
            {
                _selected = table1[table1.SelectedIndex - 1].record as JwMaterialTypeData;
                await jwqitaService.LoadSubDataAsync(_selected);
                table2.DataSource= _selected.JwMaterialDatas;
                button2.Enabled = true;
            }
        }
    }
}
