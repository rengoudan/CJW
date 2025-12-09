using JwCore;
using RGBJWMain.Pages;
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

namespace RGBControls.Pages
{
    public partial class NewProjectMainPage : BasePage
    {
        public NewProjectMainPage()
        {
            InitializeComponent();
            Initform();
        }

        private void Initform()
        {
            projectmaintable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("ProjectName", "工事名"),
                                new AntdUI.Column("Biaochi", "縮尺"),
                                new AntdUI.Column("BeamsNumber", "梁数"),
                                new AntdUI.Column("KPillarCount", "K 柱 トータル"),
                                new AntdUI.Column("SinglePillarCount", "単柱"),
                                new AntdUI.Column("CustomerName", "顧客名"),
                                new AntdUI.Column("BCount", "B"),
                                new AntdUI.Column("BGCount", "BG"),
                                new AntdUI.Column("FloorQuantity", "階数"),
                                new AntdUI.Column("ParsedQuantity", "解析数")

            };
            this.table1.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("SubProjectName", "サブ工事名"),
                                new AntdUI.Column("BeamsNumber", "梁数"),
                                new AntdUI.Column("KPillarCount", "K 柱 トータル"),
                                new AntdUI.Column("SinglePillarCount", "単柱"),
                                new AntdUI.Column("BCount", "B"),
                                new AntdUI.Column("BGCount", "BG"),
                                new AntdUI.Column("FloorQuantity", "階数"),
                                new AntdUI.Column("ParsedQuantity", "解析数")
            };
            base.InitData();
        }

        private void NewProjectMainPage_Load(object sender, EventArgs e)
        {
            this.dbContext?.Database.EnsureCreated();

            this.dbContext?.JwProjectMainDatas.Load();

            this.dbContext?.JwCustomerDatas.Load();
            //var z = this.uiDataGridView1.Columns["jwCustomerDataIdDataGridViewTextBoxColumn"] as DataGridViewComboBoxColumn;
            //z.DataSource = dbContext?.JwCustomerDatas.ToList();
            this.jwProjectMainDataBindingSource.DataSource = dbContext?.JwProjectMainDatas.Local.ToBindingList();
            this.projectmaintable.DataSource = this.jwProjectMainDataBindingSource;
        }

        private void projectmaintable_SelectIndexChanged(object sender, EventArgs e)
        {
            if (projectmaintable.SelectedIndex > 0)
            {
                var selecteddata = this.projectmaintable[projectmaintable.SelectedIndex-1].record as JwProjectMainData;
                if (selecteddata != null)
                {
                    if (selecteddata.JwProjectSubDatas != null)
                    {
                        this.dbContext.Entry(selecteddata).Collection(e => e.JwProjectSubDatas).Load();
                        //this.table1.Refresh();
                        //try
                        //{
                            
                        //    //
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                        this.table1.DataSource= selecteddata.JwProjectSubDatas.ToList();
                    }
                }
                HasSelectedRow = true;
/*                SelectedRow = projectmaintable.DataSource[projectmaintable.SelectedIndex]*/;
            }
            else
            {
                HasSelectedRow = false;
                SelectedRow = null;
            }
        }
    }
}
