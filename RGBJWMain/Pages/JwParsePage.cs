using JwShapeCommon;
using JwShapeCommon.JwService;
using JwShapeCommon.JwService.Models;
using RGB.Jw.JW.Dtos;
using Sunny.UI;
using Sunny.UI.Win32;
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
    public partial class JwParsePage : UIPage
    {
        private JwParseSub parseSub;

        public JwProjectClientDto projectDto { get; set; }

        public JwParsePage()
        {
            InitializeComponent();
            parseSub = JwParseSub.GetJwparsesub();
            GlobalEvent.GetGlobalEvent().ShowParseLogEvent += ShowParseLog;
            GlobalEvent.GetGlobalEvent().ApiErrorEvent += ApiErrorEvent;


        }

        private void ApiErrorEvent(object sender, ApiErrorArgs e)
        {
            if (e.Error != null)
            {
                ShowErrorNotifier(e.Error.Message);
                Frame.SelectPage(1001);
            }
        }

        private void parse()
        {
            ParseFrist();
            invokealllogmsg("begin parse beam!");
            parseSub.CreateBeam();
            invokealllogmsg("begin parse pillers!");
            parseSub.CreateBlocks();
            invokealllogmsg("begin parse tagg!");
            parseSub.CreateTagg();
            parseSub.parsePillarBeam();
            invokealllogmsg("parse complete!!");
            this.BeginInvoke(new Action(() =>
            {
                uiProgressIndicator1.Stop();
                uiProgressIndicator1.Visible = false;
                uiTitlePanel1.Visible = true;
                putresult();
            }));
        }

        private void putresult()
        {
            uiTextBox1.Text = parseSub.SubName;
            uiTextBox2.Text = parseSub.Biaochi;
            uiTextBox3.Text = parseSub.BeamsCount.ToString();
            uiTextBox4.Text = parseSub.HorizontalBeamsCount.ToString();
            uiTextBox5.Text = parseSub.VerticalBeamsCount.ToString();
            uiTextBox6.Text = parseSub.KPillarCount.ToString();
            uiTextBox7.Text = parseSub.SinglePillarCount.ToString();
            uiTextBox8.Text = parseSub.BBCount.ToString();
            uiTextBox9.Text = parseSub.BGCount.ToString();
        }

        private void ParseFrist()
        {
            string fileLocation = string.Format(@"{0}\JWC_TEMP.TXT", Application.StartupPath);
            string[] neirong = File.ReadAllLines(fileLocation, System.Text.Encoding.GetEncoding("Shift-JIS"));
            //
            parseSub.init(neirong);
            //uiTextBox1.Text = parseSub.SubName;
            //uiTextBox2.Text = parseSub.Biaochi;

        }

        private void invokealllogmsg(string msg)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() =>
                {
                    logmsg(msg);
                }));
            }
            else
            {
                logmsg(msg);
            }

        }

        private void logmsg(string msg)
        {
            parselogtxt.AppendText(msg);
            parselogtxt.AppendText("\r\n");
        }

        private void ShowParseLog(object sender, ShowParseLogArgs e)
        {
            //if(this.InvokeRequired)
            //{
            //    Invoke(new Action(() =>
            //    {

            //    }));
            //}

            //this.BeginInvoke(new Action(() =>
            //{
            //    logmsg(e.Msg);
            //}));
            invokealllogmsg(e.Msg);
        }

        public UIProgressIndicator ProgressIndicator = new UIProgressIndicator();

        private async void JwParsePage_Initialize(object sender, EventArgs e)
        {
            JwMainForm j = this.ParentForm as JwMainForm;
            if (j != null)
            {
                projectDto = j.JwProject;
            }
            string api = "/api/services/app/JwCustomerDesignTags/GetClientLst";
            parseSub.CustomerDesignTagClientDtos = await JwApiClient.GetClient().GetAsync<List<JwCustomerDesignTagClientDto>>(api, new { customerid = projectDto.JwCustomerId.Value });

            parseSub.ProjectId = projectDto.Id;
            this.uiTitlePanel1.Visible = false;
            uiProgressIndicator1.Dock = DockStyle.Fill;
            uiProgressIndicator1.Start();
            logmsg("begin parse!");
            Thread t = new Thread(() =>
            {
                //ParseFrist();
                parse();
                this.BeginInvoke(new Action(() =>
                {

                }));
            });
            t.Start();
        }

        private void JwParsePage_Load(object sender, EventArgs e)
        {

        }

        private void JwParsePage_Shown(object sender, EventArgs e)
        {

        }

        private async void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            //Task.Run(new Action(async ()=>{
            //    await JwApiClient.GetClient().PostAsync("api/services/app/JwProjectSubs/CreateProjectsub", parseSub);
            //}));
            await JwApiClient.GetClient().PostAsync("api/services/app/JwProjectSubs/CreateProjectsub", parseSub);
            if (GlobalEvent.GetGlobalEvent().ChangeJwPage!=null)
            {
                GlobalEvent.GetGlobalEvent().ChangeJwPage(this, new ChangePageArgs
                {
                    ChangeHead = true,
                    PageId = 1002
                });
            }
            //Frame.SelectPage(1002);
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            //page
            Frame.SelectPage(1001);
        }
    }
}
