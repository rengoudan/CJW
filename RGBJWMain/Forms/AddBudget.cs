using JwCore;
using JwData;
using NPOI.HSSF.Record;
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

namespace RGBJWMain.Forms
{
    public partial class AddBudget : Base
    {
        public AddBudget()
        {
            InitializeComponent();
        }

        private JwBudgetMainData jwBudgetMainData;

        public AddBudget(JwBudgetMainData maindata)
        {
            InitializeComponent();
            jwBudgetMainData = maindata;
        }

        private void AddBudget_Load(object sender, EventArgs e)
        {
            dbContext = ContextFactory.GetContext();
            this.dbContext.Database.EnsureCreated();

            this.dbContext.JwMaterialDatas.Load();
            //this.bindingSource1.DataSource=this.dbContext.JwMaterialDatas.Local.ToBindingList();
            var lst = this.dbContext.JwMaterialDatas.ToList();
            lst.Add(new JwMaterialData { Id = "-1", MaterialName = "予算項目を選択", MaterialParameter = "" });
            this.uiComboBox1.DataSource = lst;
            this.uiComboBox1.DisplayMember = "MaterialDescription";
            this.uiComboBox1.ValueMember = "Id";
            this.uiComboBox1.SelectedValue = "-1";
            this.uiComboBox1.SelectedIndexChanged += uiComboBox1_SelectedIndexChanged;
            uiMarkLabel1.Visible = false;
            uiMarkLabel2.Visible = false;
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sdata = uiComboBox1.SelectedItem as JwMaterialData;
            if (sdata.Id != "-1")
            {
                //UIMessageBox.ShowSuccess(sdata.MaterialDescription);
                uiMarkLabel1.Visible = true;
                uiMarkLabel2.Visible = true;
                uiLabel1.Text = sdata.UnitName;
                uiLabel2.Text = sdata.UnitPrice.ToString();
                isselecteditem = true;
                jwMaterial = sdata;
            }
        }

        private bool isselecteditem = false;
        private JwMaterialData jwMaterial;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(uiTextBox1.Text))
            {
                string nr=uiTextBox1.Text;
                if (nr.IsNumber()&& isselecteditem)
                {
                    decimal sl=Convert.ToDecimal(nr);
                    saveNewBudget(sl);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    UIMessageBox.ShowError("正しい番号を入力してください");
                    uiTextBox1.Focus();
                }
            }
        }

        private void saveNewBudget(decimal num)
        {
            JwBudgetSubData subData = new JwBudgetSubData();
            subData.JwMaterialDataId = jwMaterial.Id;
            subData.JwBudgetMainDataId = jwBudgetMainData.Id;
            subData.BudgetItemName = jwMaterial.MaterialName;
            subData.MaterialType = jwMaterial.MaterialType;
            subData.ModelParm = jwMaterial.MaterialParameter;
            subData.UnitPrice=jwMaterial.UnitPrice;
            subData.UnitName = jwMaterial.UnitName;
            subData.Number = num;
            subData.Amount = num * subData.UnitPrice;
            subData.BudgetType = BudgetFrom.カスタム予算;

            dbContext.JwBudgetSubDatas.Add(subData);
            dbContext.SaveChanges();
        }
    }
}
