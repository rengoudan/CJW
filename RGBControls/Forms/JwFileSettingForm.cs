using JwShapeCommon;
using JwShapeCommon.Jwbase;
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

namespace RGBJWMain.Forms
{
    public partial class JwFileSettingForm : UIForm
    {
        public JwFileSettingForm()
        {
            InitializeComponent();
        }

        private void JwFileSettingForm_Load(object sender, EventArgs e)
        {
            List<JwColor> ls = JwFileConsts.GetJwColors();
            List<JwPenStyle> ps=JwFileConsts.GetJwPenStyles();

            uiCbBeamcolor.DataSource = ls;
            uiCbBeamcolor.DisplayMember = "JwColorName";
            uiCbBeamcolor.ValueMember = "ColorNumber";

            uiCbTextColor.DataSource = ls;
            uiCbTextColor.DisplayMember = "JwColorName";
            uiCbTextColor.ValueMember = "ColorNumber";

            uiCbpillarcolor.DataSource = ls;
            uiCbpillarcolor.DisplayMember = "JwColorName";
            uiCbpillarcolor.ValueMember = "ColorNumber";

            uiCbSplitcolor.DataSource = ls;
            uiCbSplitcolor.DisplayMember = "JwColorName";
            uiCbSplitcolor.ValueMember = "ColorNumber";

            uiComboBox2.DataSource = ps;
            uiComboBox2.DisplayMember = "JwPenStyleName";
            uiComboBox2.ValueMember = "StyleNumber";

            uiComboBox1.DataSource = ps;
            uiComboBox1.DisplayMember = "JwPenStyleName";
            uiComboBox1.ValueMember = "StyleNumber";
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {

            if ((int)uiCbBeamcolor.SelectedValue != -1)
            {
                JwFileConsts.BeamParseColor = uiCbBeamcolor.SelectedItem as JwColor;
            }
            if ((int)uiCbTextColor.SelectedValue != -1)
            {
                JwFileConsts.BeamSymbolTextColor = uiCbTextColor.SelectedItem as JwColor;
            }
            if ((int)uiCbSplitcolor.SelectedValue != -1)
            {
                JwFileConsts.BeamSplitParseColor = uiCbSplitcolor.SelectedItem as JwColor;
            }
            if ((int)uiCbpillarcolor.SelectedValue != -1)
            {
                JwFileConsts.BeamPillarParseColor = uiCbpillarcolor.SelectedItem as JwColor;
            }

            if ((int)uiComboBox2.SelectedValue != -1)
            {
                JwFileConsts.PillarPenStyle = uiComboBox2.SelectedItem as JwPenStyle;
            }

            if ((int)uiComboBox1.SelectedValue != -1)
            {
                JwFileConsts.SplitPenStyle = uiComboBox1.SelectedItem as JwPenStyle;
            }
            JwFileConsts.NearSpliteMax = uiNearSpliteMax.Value;
            DialogResult = DialogResult.OK;
        }
    }
}
