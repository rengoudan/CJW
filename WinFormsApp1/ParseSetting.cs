using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class ParseSetting : Form
    {
        public ParseSetting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var q = this.Owner as Form1;
            q.settingS.ParseColor = comboBox1.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
        }
    }
}
