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

namespace RGBJWMain.FormSet
{
    public partial class SystemSetting : UIEditForm
    {
        public SystemSetting()
        {
            InitializeComponent();
        }

        private void SystemSetting_Load(object sender, EventArgs e)
        {
            uiTextBox1.Text = Properties.Settings.Default.ServerUrl; ;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["ServerUrl"] = uiTextBox1.Text;
            Properties.Settings.Default.Save();
        }
    }
}
