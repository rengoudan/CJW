using JwCore;
using JwData;
using JwShapeCommon;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Forms
{
    public partial class JwSingleBeamForm : UIForm
    {

        public JwSingleBeamForm()
        {
            InitializeComponent();
        }

        private JwBeam _beam;

        public JwSingleBeamForm(JwBeam beam)
        {
            _beam = beam;
            InitializeComponent();
        }

        private void JwSingleBeamForm_Shown(object sender, EventArgs e)
        {
            if (_beam != null)
            {
                beamSingleShow1.JwDrawShape = new JwSingleBeamDraw(_beam);
                ///写一个类  包含绘制方法 接受参数 PaintEventArgs 由控件调用该方法进行绘制
                //this.Text= _beam.BeamCode;
                this.Text = string.Format("{0} : 中心点間距離{1}; 梁長さ:{2}", _beam.BeamCode, _beam.XXLength, _beam.Length);
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (_beam != null)
            {
                JwBeamJwDraw jwDraw = new JwBeamJwDraw(_beam);
                jwDraw.Draw();
                if (jwDraw.Sens.Count > 0)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.Description = "";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string foldPath = dialog.SelectedPath ;
                        if (!Directory.Exists(foldPath))
                        {
                            Directory.CreateDirectory(foldPath);
                        }
                        using var a = new JwwHelper.JwwWriter();
                        string wjm = string.Format("{0}.jww", _beam.BeamCode);
                        //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                        //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                        //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                        a.InitHeader("template.jww");
                        foreach (var s in jwDraw.Datas)
                        {
                            a.AddData(s);
                        }
                        //foreach(var b in jwDraw.Biaozhu)
                        //{
                        //    a.AddData(b);
                        //}
                        a.Write(foldPath + "\\" + wjm);
                        var msgshow = string.Format("{0}は正常に保存されました", _beam.BeamCode);
                        UIMessageBox.ShowSuccess(msgshow);
                    }
                }
            }
        }

        private void JwSingleBeamForm_Load(object sender, EventArgs e)
        {
            var dbContext = ContextFactory.GetContext();
            var medlst = dbContext.JwMaterialDatas.Where(t => t.MaterialType == MaterialType.梁).ToList();
            if (medlst.Count > 0)
            {
                uiComboBox3.DataSource = medlst;
                uiComboBox3.DisplayMember = "MaterialName";
                uiComboBox3.ValueMember = "Id";
            }
            else
            {
                UIMessageBox.ShowError("基礎データがまだ整備されていない");
                this.Close();
            }
        }
    }
}
