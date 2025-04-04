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

namespace RGBJWMain.Forms
{
    public partial class Base : UIForm
    {
        public JwDataContext? dbContext;
        public Base()
        {
            InitializeComponent();
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    //dbContext = ContextFactory.GetContext();
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //ContextFactory.DisposeContext();
            //this.dbContext?.Dispose();
            //this.dbContext = null;
        }

    }
}
