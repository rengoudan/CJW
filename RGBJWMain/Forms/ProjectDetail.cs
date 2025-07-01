using JwCore;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RGBJWMain.Forms
{
    public partial class ProjectDetail : UIAsideHeaderMainFrame
    {
        public ProjectDetail()
        {
            InitializeComponent();
        }

        private JwProjectMainData _mainData;

        public ProjectDetail(JwProjectMainData mainData)
        {
            InitializeComponent();
            _mainData = mainData;
            Aside.TabControl = MainTabControl;
            load();
        }

        private void load()
        {
            if (_mainData != null)
            {
                this.Name= _mainData.ProjectName;
                this.Text= _mainData.ProjectName;
                if (_mainData.JwProjectSubDatas.Count>0)
                {
                    
                    AddPage(new ProjectOverview(_mainData), 1000);
                    Aside.CreateNode("プロジェクト概要", 1000);
                    int pageIndex = 1001;
                    foreach (var item in _mainData.JwProjectSubDatas)
                    {
                        AddPage(new SubDetail(item), pageIndex);
                        Aside.CreateNode(item.FloorName, pageIndex);
                        pageIndex++;
                    }
                    
                }
            }
        }


    }
}
