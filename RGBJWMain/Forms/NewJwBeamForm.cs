using JwShapeCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Forms
{
    public partial class NewJwBeamForm : Form
    {
        private JwBeam _jwbeam;


        public NewJwBeamForm()
        {
            InitializeComponent();
        }

        public NewJwBeamForm(JwBeam jwbeam)
        {
            this._jwbeam = jwbeam;
            InitializeComponent();
        }

        private void NewJwBeamForm_Shown(object sender, EventArgs e)
        {
            if (this._jwbeam != null)
            {
                this.newSingleBeamControl1.ShowBeam = this._jwbeam;
            }
            
        }
    }
}
