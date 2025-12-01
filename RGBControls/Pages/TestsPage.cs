using JwData;
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
    public partial class TestsPage : UIPage
    {
        public TestsPage()
        {
            InitializeComponent();
            table1.Columns = new AntdUI.ColumnCollection
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
        }
        public JwDataContext? dbContext;
        private void TestsPage_Load(object sender, EventArgs e)
        {
            dbContext = ContextFactory.GetContext();
            this.dbContext?.Database.EnsureCreated();

            this.dbContext?.JwProjectMainDatas.Load();

            this.dbContext?.JwCustomerDatas.Load();
            this.jwProjectMainDataBindingSource.DataSource = dbContext?.JwProjectMainDatas.Local.ToBindingList();
            table1.DataSource = this.jwProjectMainDataBindingSource;
        }

        private void table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                //MessageBox.Show(e.RowIndex.ToString());
            }
        }

        private void table1_SelectIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(table1.SelectedIndex.ToString());
        }
    }
}
