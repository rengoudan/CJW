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
    }
}
