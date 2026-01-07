 using JwShapeCommon;
using MathNet.Numerics;
using RGBControls.Forms;
using RGBControls.Pages;
using RGBJWMain.Pages;
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

namespace RGBControls
{
    public partial class MainForm : AntdUI.Window, IFrame
    {
        public MainForm()
        {
            InitializeComponent();
            int pageIndex = 1000;

            //uiTabControl1 = value;
            //uiTabControl1.Frame = this;
            //uiTabControl1.PageAdded += DealPageAdded;
            //uiTabControl1.PageRemoved += DealPageRemoved;
            uiTabControl1.Selected += MainTabControl_Selected;
            //uiTabControl1.Deselected += MainTabControl_Deselected;
            //uiTabControl1.TabPageAndUIPageChanged += MainTabControl_TabPageAndUIPageChanged;


            //uiNavBar1.
            uiNavBar1.CreateNode(AddPage(new EmptyPage(), ++pageIndex));
            //uiNavBar1.CreateNode(AddPage(new TestsPage(), ++pageIndex));
            //uiNavBar1.CreateNode(AddPage(new JwProjectMainPage(), ++pageIndex));
            uiNavBar1.CreateNode(AddPage(new NewProjectMainPage(this), ++pageIndex));
            uiNavBar1.CreateNode(AddPage(new JwBudgePage(), ++pageIndex));
            uiNavBar1.CreateNode(AddPage(new JwCustomerPage(), ++pageIndex));
            uiNavBar1.CreateNode(AddPage(new NewBaseDataPage(), ++pageIndex));
            
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            var settingsForm = new CsvSettings(this);
            if (AntdUI.Modal.open(this, AntdUI.Localization.Get("Setting", "設定"), settingsForm) == DialogResult.OK)
            {
                JwFileConsts.CsvHxJianju = settingsForm.Hxjianju;
                JwFileConsts.CsvHxNum = settingsForm.Hxnum; 
                JwFileConsts.CsvZxJianju = settingsForm.Zxjianju;
                JwFileConsts.CsvZxNum = settingsForm.Zxnum;
                Refresh();
            }
        }

        public UIPage AddPage(UIPage page, int pageIndex)
        {
            page.PageIndex = pageIndex;
            return AddPage(page);
        }

        public UIPage AddPage(UIPage page, Guid pageGuid)
        {
            page.PageGuid = pageGuid;
            return AddPage(page);
        }

        public UIPage AddPage(UIPage page)
        {
            

            //page.Frame = this;
            //page.OnFrameDealPageParams += Page_OnFrameDealPageParams;
            uiTabControl1?.AddPage(page);
            return page;
        }

        private void MainTabControl_Selected(object sender, TabControlEventArgs e)
        {
            List<UIPage> controls = e.TabPage.GetControls<UIPage>();
            SelectedPage = ((controls.Count == 1) ? controls[0] : null);
        }

        public bool SelectPage(int pageIndex)
        {
            return uiTabControl1.SelectPage(pageIndex);
        }

        public bool SelectPage(Guid pageGuid)
        {
            return uiTabControl1.SelectPage(pageGuid);
        }

        public UIPage GetPage(int pageIndex)
        {
            return uiTabControl1.GetPage(pageIndex);
        }

        public UIPage GetPage(Guid pageGuid)
        {
            return uiTabControl1.GetPage(pageGuid);
            
        }

        public bool RemovePage(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public bool RemovePage(Guid pageGuid)
        {
            throw new NotImplementedException();
        }

        public bool ExistPage(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public bool ExistPage(Guid pageGuid)
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void Final()
        {
            throw new NotImplementedException();
        }

        public T GetPage<T>() where T : UIPage
        {
            throw new NotImplementedException();
        }

        public List<T> GetPages<T>() where T : UIPage
        {
            throw new NotImplementedException();
        }

        private UIPage selectedPage;
        public UIPage SelectedPage
        {
            get
            {
                return selectedPage;
            }
            private set
            {
                if (selectedPage != value)
                {
                    selectedPage = value;
                    //this.PageSelected?.Invoke(this, new UIPageEventArgs(SelectedPage));
                }
            }
        }

        public UITabControl MainTabControl => throw new NotImplementedException();

        public event OnUIPageChanged PageSelected;

    }
}
