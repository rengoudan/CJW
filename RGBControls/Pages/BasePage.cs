using JwData;
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
    public partial class BasePage : UIPage
    {
        public JwDataContext? dbContext;
        public BasePage()
        {
            InitializeComponent();
        }

        public virtual void InitData()
        {
            //dbContext = ContextFactory.GetContext();
            //BindingList = dbContext.Set<TModel>().Local.ToBindingList();
        }

        public bool HasSelectedRow;

        public DataGridViewRow? SelectedRow;

        public bool IsClose = false;

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            IsClose = true;
            //ContextFactory.DisposeContext();
            //this.dbContext?.Dispose();
            //this.dbContext = null;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            this.dbContext?.SaveChanges();
        }

        public void WarningMsg(string msg)
        {
            AntdUI.Message.warn(this, msg, Font);
        }

    }
}
