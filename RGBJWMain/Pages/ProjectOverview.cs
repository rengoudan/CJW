using JwCore;
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
    public partial class ProjectOverview : UIPage
    {
        public ProjectOverview()
        {
            InitializeComponent();
        }

        private JwProjectMainData _projectMainData; 

        public ProjectOverview(JwProjectMainData projectMainData)
        {
            InitializeComponent();
            _projectMainData = projectMainData;
            init();
        }

        private void init()
        {
            if (_projectMainData != null)
            {
                this.uiLine1.Text = _projectMainData.ProjectName;
                this.uiMarkLabel2.Text=_projectMainData.JwProjectSubDatas.Count.ToString();
            }
        }
    }
}
