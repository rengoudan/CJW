using JwShapeCommon;
using JwShapeCommon.JwService;
using RGB.Jw.JW.Dtos;
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
    public partial class ProjectMainPage : UIPage
    {
        public ProjectMainPage()
        {
            InitializeComponent();
            //uiDataGridView1.AddColumn("projectname", "ProjectName", 100/*占比*/).SetFixedMode(200/*固定宽度*/);
            //uiDataGridView1.AddColumn("CompanyName", "CustomerName");
            //uiDataGridView1.AddColumn("CreateDesigner", "CreateDesigner");
            //uiDataGridView1.AddColumn("ProjectCost", "ProjectCost");
        }

        private async void ProjectMainPage_Load(object sender, EventArgs e)
        {

        }

        private void uiDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var z = this.ParentForm as JwMainForm;

            if (e.RowIndex == -1)
            {
                return;
            }
            JwParseSub.GetJwparsesub().Settingobj = new SettingObject();
            var q = uiDataGridView1.Rows[e.RowIndex].DataBoundItem as JwProjectClientDto;
            if (q != null)
            {
                if (!string.IsNullOrEmpty(z.Optype))
                {
                    if (z.Optype == "1")
                    {
                        string titlemsg = string.Format("将JW选择的内容作为{0}的一部分处理", q.ProjectName);
                        if (ShowAskDialog(titlemsg))
                        {
                            string titlemsg2 = "if change parse setting";
                            if (ShowAskDialog(titlemsg2))
                            {
                                SettingForm frm = new SettingForm(q);
                                frm.Render();
                                frm.ShowDialog();
                                if (frm.IsOK)
                                {
                                    JwParseSub.GetJwparsesub().Settingobj = frm.SettingObject;
                                }
                                frm.Dispose();
                                OpenJwParsePages(q);
                            }
                            else
                            {
                                OpenJwParsePages(q);
                            }

                        }
                        else
                        {
                            ShowErrorTip("cancel");
                        }
                    }
                    else
                    {
                        ShowWarningNotifier("注釈の説明を読み込もうとしています");
                    }
                }

            }
        }

        private void OpenJwParsePages(JwProjectClientDto dto)
        {
            ChangePageArgs args = new ChangePageArgs
            {
                PageId = 100101,
                JwProject = dto
            };
            if (GlobalEvent.GetGlobalEvent().ChangeJwPage != null)
            {
                GlobalEvent.GetGlobalEvent().ChangeJwPage(this, args);
            }
            ShowSuccessTip("sure");
        }

        private async void ProjectMainPage_Shown(object sender, EventArgs e)
        {
            try
            {
                //string api = "/api/services/app/JwProjects/GetAllList";
                //List<JwProjectClientDto> lst = await JwApiClient.GetClient().GetAsync<List<JwProjectClientDto>>(api, null);
                //uiDataGridView1.DataSource = lst;
            }
            catch (Exception ex)
            {

            }

        }

        private async void ProjectMainPage_Initialize(object sender, EventArgs e)
        {
            string api = "/api/services/app/JwProjects/GetAllList";
            List<JwProjectClientDto> lst = await JwApiClient.GetClient().GetAsync<List<JwProjectClientDto>>(api, null);
            uiDataGridView1.DataSource = lst;
        }
    }
}
