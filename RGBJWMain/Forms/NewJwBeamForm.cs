using JwShapeCommon;
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
            
            //this.Name = this._jwbeam.BeamCode;
            InitializeComponent();
            this.Text = this._jwbeam.BeamCode;
        }

        private void NewJwBeamForm_Shown(object sender, EventArgs e)
        {
            if (this._jwbeam != null)
            {
                this.newSingleBeamControl1.ShowBeam = this._jwbeam;
            }

        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (_jwbeam != null)
            {
                var _jwDrawShape = new NewJwBeamJwDraw(_jwbeam);
                //JwBeamJwDraw jwDraw = new JwBeamJwDraw(_beam);
                _jwDrawShape.CreateBeam();
                if (_jwDrawShape.Sens.Count > 0)
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.Description = "";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string foldPath = dialog.SelectedPath;
                        if (!Directory.Exists(foldPath))
                        {
                            Directory.CreateDirectory(foldPath);
                        }
                        using var a = new JwwHelper.JwwWriter();
                        string wjm = string.Format("{0}.jww", _jwbeam.BeamCode);
                        //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                        //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                        //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                        a.InitHeader("template.jww");
                        foreach (var s in _jwDrawShape.Datas)
                        {
                            a.AddData(s);
                        }
                        //foreach(var b in jwDraw.Biaozhu)
                        //{
                        //    a.AddData(b);
                        //}
                        a.Write(foldPath + "\\" + wjm);
                        var msgshow = string.Format("{0}は正常に保存されました", _jwbeam.BeamCode);
                        UIMessageBox.ShowSuccess(msgshow);
                    }
                }
            }
        }
    }
}
