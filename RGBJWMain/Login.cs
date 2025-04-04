using JwShapeCommon;
using JwShapeCommon.JwService;
using JwShapeCommon.JwService.Models;
using Newtonsoft.Json;
using RGBJWMain.FormSet;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain
{
    public partial class Login : UILoginForm
    {

        public bool isloginclose = false;
        public Login()
        {
            InitializeComponent();
        }

        private async void Login_ButtonLoginClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Password))
                {
                    UIMessageTip.ShowOk("account or password can not be empty");
                    return;
                }
                AbpAuthenticateModel lm = new AbpAuthenticateModel();
                lm.UserNameOrEmailAddress = this.UserName;
                lm.Password = this.Password;
                await JwApiClient.GetClient().LoginAsync(lm);
                if (JwApiClient.GetClient().AuthenticateResult != null)
                {
                    JwApiClient.GetClient().IsLogin = true;
                    isloginclose = true;
                    string token = JwApiClient.GetClient().AuthenticateResult.AccessToken;
                    this.IsLogin = true;
                    DialogResult = DialogResult.OK;
                    Properties.Settings.Default["LastToken"] = JwApiClient.GetClient().AuthenticateResult.AccessToken;
                    Properties.Settings.Default.Save();
                    var uz = await JwApiClient.GetClient().LoginWithLastToken();
                    JwConsts.CurrentUser = uz.Result.User;
                    if (GlobalEvent.GetGlobalEvent().LoginUserLoadEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().LoginUserLoadEvent(this, new EventArgs());
                    }
                }

            }
            catch (Exception ex)
            {
                JwApiClient.GetClient().IsLogin = false;
                this.IsLogin = false;
                //DialogResult = DialogResult.No;
                UIMessageTip.ShowOk("account or password is wrong");
            }

        }

        private void Login_ButtonCancelClick(object sender, EventArgs e)
        {
            this.IsLogin = false;
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isloginclose)
            {
                Application.Exit();
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            SystemSetting systemform=new SystemSetting();
            systemform.ShowDialog();
        }
    }
}
