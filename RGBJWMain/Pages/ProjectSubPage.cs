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
    public partial class ProjectSubPage : UIPage
    {
        public ProjectSubPage()
        {
            InitializeComponent();
        }

        private async void ProjectSubPage_Load(object sender, EventArgs e)
        {
            string api = "/api/services/app/JwProjectSubs/GetClientAll";
            List<JwProjectSubForClientDto> lst = await JwApiClient.GetClient().GetAsync<List<JwProjectSubForClientDto>>(api, null);
            uiDataGridView1.DataSource = lst;
        }
    }
}
