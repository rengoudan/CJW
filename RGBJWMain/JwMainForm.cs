using JwShapeCommon;
using JwShapeCommon.JwService.Dtos;
using JwShapeCommon.JwService.Models;
using JwShapeCommon.JwService;
using Microsoft.Extensions.Configuration;
using RGB.Jw.JW.Dtos;
using RGBJWMain.Pages;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain
{
    public partial class JwMainForm : UIHeaderMainFooterFrame
    {
        private readonly IConfiguration configuration;
        private string tqs;
        public JwMainForm(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            InitializeComponent();
            AddPage(new EmptyPage(), 1000);
            
            //设置Header节点索引
            //Header.CreateNode("Project", 61818, 24, 1001);

            //Header.CreateNode("ProjectSub", 61950, 24, 1002);
            //Header.CreateNode("Page3", 1003);
            //Header.SelectedIndex = 1000;
            Optype = "1";
            

            tqs = this.configuration["BaseUrl"];

            UILocalizeHelper.SetEN();
            //Configuration
            GlobalEvent.GetGlobalEvent().ChangeJwPage += ChangeJwPage;
            GlobalEvent.GetGlobalEvent().LoginUserLoadEvent += LoginUserLoad;
            GlobalEvent.GetGlobalEvent().SetNewPages += SetNewPage;
            timer1.Start();
        }

        public string Optype = "";

        public JwMainForm(string optype)
        {
            Optype = optype;
            InitializeComponent();
            AddPage(new EmptyPage(optype), 1000);
            AddPage(new ProjectMainPage(), 1001);
            AddPage(new JwParsePage(), 100101);
            AddPage(new ProjectSubPage(), 1002);
            //设置Header节点索引
            Header.CreateNode("プロジェクト", 61818, 24, 1001);
            Header.CreateNode("レコードの解析", 61950, 24, 1002);
            Header.CreateNode("Page3", 1003);
            if (!string.IsNullOrEmpty(optype))
            {
                SelectPage(1000);
                //if (optype == "1")
                //{

                //    //Header.SelectedIndex = 1000;
                //}
                //else
                //{
                //    AddPage(new EmptyPage(optype), 1000);
                //    //Header.SelectedIndex = 1000;

                //}

            }

            var bbb = tqs;

            UILocalizeHelper.SetEN();
            GlobalEvent.GetGlobalEvent().ChangeJwPage += ChangeJwPage;
        }

        public JwProjectClientDto JwProject { get; set; }

        private void SetNewPage(object sender,SetNewPageArgs e)
        {
            if(e.NewPage != null)
            {
                RemoveAllPages();
                AddPage(e.NewPage);
            }
        }
        private void ChangeJwPage(object sender, ChangePageArgs e)
        {

            if (e.JwProject != null)
            {
                JwProject = e.JwProject;
            }
            if (e.ChangeHead)
            {
                Header.SelectedIndex = e.PageId;
                SelectPage(e.PageId);
            }
            else
            {
                SelectPage(e.PageId);
            }
        }

        private void JwMainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }


        /// <summary>
        /// 窗体加载完后 处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JwMainForm_Shown(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastToken))
            {
                JwConsts.LastToken = Properties.Settings.Default.LastToken;
                //var z=JwApiClient.GetClient().LoginWithLastToken().
                Task<AjaxResponse<GetCurrentLoginInformationsOutput>> task = Task.Run(async () => { AjaxResponse<GetCurrentLoginInformationsOutput> t = await JwApiClient.GetClient().LoginWithLastToken(); return t; });
                var z = task.Result;
                if (z.Success)
                {
                    if (z.Result.User != null)
                    {
                        JwConsts.CurrentUser = z.Result.User;
                        if (GlobalEvent.GetGlobalEvent().LoginUserLoadEvent != null)
                        {
                            GlobalEvent.GetGlobalEvent().LoginUserLoadEvent(this, new EventArgs());
                        }
                        UIMessageTip.ShowOk("success");
                    }
                    else
                    {
                        Login frm = new Login();
                        frm.ShowDialog();
                        if (frm.IsLogin)
                        {
                            UIMessageTip.ShowOk("success");
                        }

                        frm.Dispose();
                    }
                }
                //var z = task.ConfigureAwait(false);
            }
        }

        private void LoginUserLoad(object sender, EventArgs e)
        {
            uiLabel1.Text = JwConsts.CurrentUser.UserName;
        }

        private void uiAvatar1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            uiPanel2.Text=DateTime.Now.DateTimeString();
        }
    }
}
