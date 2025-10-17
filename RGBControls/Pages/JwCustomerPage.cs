using JwCore;
using JwShapeCommon;
using Microsoft.EntityFrameworkCore;
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
    public partial class JwCustomerPage : BasePage
    {
        public JwCustomerPage()
        {
            InitializeComponent();
            base.InitData();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.dbContext.Database.EnsureCreated();

            this.dbContext.JwCustomerDatas.Load();

            this.jwCustomerDataBindingSource.DataSource = dbContext.JwCustomerDatas.Local.ToBindingList();
        }

        private void uiDataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (!e.Row.IsNewRow)
                e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }

        private void uiDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (!uiDataGridView1.Rows[e.RowIndex].IsNewRow)
                {
                    var z = uiDataGridView1.Rows[e.RowIndex].DataBoundItem as JwCustomerData;

                    MessageBox.Show(z.CompanyName);
                }
            }


        }

        private void uiDataGridView1_SelectIndexChange(object sender, int index)
        {
            if (!uiDataGridView1.Rows[index].IsNewRow && uiDataGridView1.Rows[index].Selected)
            {
                HasSelectedRow = true;
                SelectedRow = uiDataGridView1.Rows[index];
            }
            else
            {
                HasSelectedRow = false;
                SelectedRow = null;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (HasSelectedRow)
            {
                var dobj = SelectedRow!.DataBoundItem as JwCustomerData;
                if (dobj != null)
                {
                    CustomerDesignSetting setting = new CustomerDesignSetting(dobj);
                    setting.ShowDialog();
                    //MessageBox.Show(dobj.Telephone);
                }
            }
        }

        private void uiDataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //if(HasSelectedRow)
                //{
                //    if(SelectedRow.Index!=
                //    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                //}

            }
        }

        private void uiDataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (HasSelectedRow)
                {
                    if (SelectedRow!.Index != e.RowIndex && e.RowIndex >= 0)
                    {
                        uiDataGridView1.ClearSelection();
                        SelectedRow = uiDataGridView1.Rows[e.RowIndex];
                        uiDataGridView1.Rows[e.RowIndex].Selected = true;
                    }
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
                else
                {
                    if (e.RowIndex >= 0)
                    {
                        uiDataGridView1.ClearSelection();
                        HasSelectedRow = true;
                        uiDataGridView1.Rows[e.RowIndex].Selected = true;
                        SelectedRow = uiDataGridView1.Rows[e.RowIndex];
                        contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }

        private void uiDataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            uiDataGridView1.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private void uiDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (String.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                string fmsg = string.Format("{0} 列目空ではない", e.ColumnIndex + 1);
                uiDataGridView1.Rows[e.RowIndex].ErrorText = string.Format("{0} 列目空ではない", e.ColumnIndex + 1);
                UIMessageBox.ShowError(fmsg);
                e.Cancel = true;
            }
        }

        private void uiDataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void uiDataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!e.Cancel)
            {
                if (dbContext != null)
                {
                    dbContext.SaveChanges();
                }
                //dbContext.ch
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            UIEditOption option = new UIEditOption();
            option.AutoLabelWidth = true;
            option.Text = "追加";
            option.AddText("CompanyName", "会社名", null, true);
            option.AddText("CompanyAddress", "会社住所", null, false);
            option.AddText("Contact", "会社の連絡先", null, false);
            option.AddText("Telephone", "電話", null, false);
            
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
                JwCustomerData customerdata = new JwCustomerData();
                customerdata.CompanyName = frm["CompanyName"].ToString();
                customerdata.CompanyAddress = frm["CompanyAddress"].ToString();
                customerdata.Contact = frm["Contact"].ToString();
                customerdata.Telephone = frm["Telephone"].ToString();
                this.dbContext.JwCustomerDatas.Add(customerdata);
                this.dbContext.SaveChanges();
            }
        }

        private bool Frm_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Form["CompanyName"].ToString()))
            {
                e.Form.SetEditorFocus("CompanyName");
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
    }
}
