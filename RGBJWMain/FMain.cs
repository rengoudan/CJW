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
using static System.Windows.Forms.AxHost;

namespace RGBJWMain
{
    public partial class FMain : UIForm2
    {
        public FMain()
        {
            InitializeComponent();
            int pageIndex = 1000;
            
            //uiNavBar1.
            uiNavBar1.CreateNode(AddPage(new EmptyPage(), ++pageIndex));
            uiNavBar1.CreateNode(AddPage(new JwProjectMainPage(), ++pageIndex));
            uiNavBar1.CreateNode(AddPage(new JwBudgePage(),++pageIndex));
            uiNavBar1.CreateNode(AddPage(new JwCustomerPage(), ++pageIndex));
            uiNavBar1.CreateNode(AddPage(new JwBaseDataPage(), ++pageIndex));
            //uiNavBar1.Nodes.Add("项目");
            //uiNavBar1.Nodes.Add("客户");
            //uiNavBar1.Nodes.Add("基础");
            //uiNavBar1.CreateChildNode(uiNavBar1.Nodes[0],);
            AddPage(new FileParsePage(), ++pageIndex);
            //uiNavBar1.CreateChildNode(uiNavBar1.Nodes[1], AddPage(new JwProjectMainPage(), ++pageIndex));
            //uiNavBar1.CreateChildNode(uiNavBar1.Nodes[2], AddPage(new JwCustomerPage(), ++pageIndex));
            
            // CustomerManagementPage TreeNode parent = uiNavMenu1.CreateNode("绘制", 61451, 24, pageIndex);
            //uiNavMenu1.CreateChildNode(parent, AddPage(new FileParsePage(), ++pageIndex));
        }
    }
}

