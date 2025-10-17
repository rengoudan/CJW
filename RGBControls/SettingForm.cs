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

namespace RGBJWMain
{
    public partial class SettingForm : UIEditForm
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        JwProjectClientDto jwProjectClientDto;

        public SettingForm(JwProjectClientDto dto)
        {
            InitializeComponent();
            this.jwProjectClientDto = dto;
        }

        protected override bool CheckData()
        {
            return CheckEmpty(uiComboBox1, "please select parsecolor");
        }

        private SettingObject settingobject;

        private List<JwCustomerDesignTagClientDto> lst = new List<JwCustomerDesignTagClientDto>();

        private async void SettingForm_Load(object sender, EventArgs e)
        {
            if (jwProjectClientDto != null)
            {
                if (jwProjectClientDto.JwCustomerId.HasValue)
                {
                    uiLabel3.Visible= false;
                    string api = "/api/services/app/JwCustomerDesignTags/GetClientLst";
                    lst = await JwApiClient.GetClient().GetAsync<List<JwCustomerDesignTagClientDto>>(api, new { customerid = jwProjectClientDto.JwCustomerId.Value });                    uiDataGridView1.DataSource = lst;
                }
                else
                {
                    uiLabel3.Visible = true;
                    uiLabel3.Text = string.Format("{0}顧客情報なし", jwProjectClientDto.ProjectName);
                }
            }

        }

        public SettingObject SettingObject
        {
            get
            {
                if (settingobject == null)
                {
                    settingobject = new SettingObject();
                }
                settingobject.ParseColor = uiComboBox1.Text;
                settingobject.Overlengthjiagu = uiComboBox2.Text;
                return settingobject;
            }
            set
            {
                settingobject = value;
                uiComboBox1.SelectedIndex = uiComboBox1.Items.IndexOf(value.ParseColor);
                uiComboBox2.SelectedIndex = uiComboBox2.Items.IndexOf(value.Overlengthjiagu);
            }
        }
    }
}

