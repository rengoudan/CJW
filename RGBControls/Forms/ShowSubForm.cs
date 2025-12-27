using AntdUI;
using JwCore;
using JwShapeCommon;
using RGBJWMain.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBControls.Forms
{
    public partial class ShowSubForm : AntdUI.Window
    {
        private JwProjectSubData _subdata;
        public JwProjectSubData Subdata
        {
            get { return _subdata; }
            set
            {
                _subdata = value;
                this.pageHeader1.Text = _subdata.FloorName;
                //this.pageHeader1.Refresh();
            }
        }
        public ShowSubForm(JwProjectSubData subdata)
        {
            InitializeComponent();
            Subdata = subdata;
        }

        private void ShowSubForm_Load(object sender, EventArgs e)
        {
            if (_subdata != null)
            {
            
            }
            
        }
    }
}
