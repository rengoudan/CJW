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

namespace RGBJWMain.Pages
{
    public partial class EmptyPage : UIPage
    {
        public EmptyPage()
        {
            InitializeComponent();
        }

        string optype = "";


        public EmptyPage(string optypes)
        {
            InitializeComponent();
            optype = optypes;
            if (!string.IsNullOrEmpty(optypes))
            {
                if (optype != "1")
                {
                    
                }
            }
        }

        private void uiLabel1_Click(object sender, EventArgs e)
        {
            if (GlobalEvent.GetGlobalEvent().SetNewPages != null)
            {
                TestsPage tp = new TestsPage();

                SetNewPageArgs ag = new SetNewPageArgs();
                ag.NewPage = tp;
                ag.PageId = 1;

                GlobalEvent.GetGlobalEvent().SetNewPages(this, ag);
            }
        }

        private void uibtnfileparse_Click(object sender, EventArgs e)
        {
            FileParsePage fileParsePage = new FileParsePage();
            SetNewPageArgs ag = new SetNewPageArgs();
            ag.NewPage = fileParsePage;
            ag.PageId = 1;

            GlobalEvent.GetGlobalEvent().SetNewPages(this, ag);
        }
    }
}
