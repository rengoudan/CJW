using JwCore;
using JwShapeCommon;
using RGBControls.Classes;
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
    public partial class NewJwBeamForm : AntdUI.Window
    {
        private JwBeam _jwbeam;

        public bool IsNewBeam = false;

        public NewJwBeamForm()
        {
            InitializeComponent();
        }

        public NewJwBeamForm(JwBeam jwbeam)
        {
            this._jwbeam = jwbeam;

            //this.Name = this._jwbeam.BeamCode;
            InitializeComponent();
            this.pageHeader1.Text = string.Format("梁预览-{0}", this._jwbeam.BeamCode);
        }

        private void NewJwBeamForm_Shown(object sender, EventArgs e)
        {
            if (this._jwbeam != null)
            {
                this.newSingleBeamControl1.ShowBeam = this._jwbeam;
                var csvshow = this._jwbeam.ToProcessCsv();
                this.uiTextBox1.Text = csvshow;
                if (!string.IsNullOrEmpty(this._jwbeam.GongQu))
                {
                    this.select7.SelectedValue = this._jwbeam.GongQu;
                }
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

        /// <summary>
        /// 确认工区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_Click(object sender, EventArgs e)
        {
            //var st = select7.SelectedValue;
            if (select7.SelectedValue != null)
            {
                if (IsNewBeam)
                {
                    if (GlobalEvent.GetGlobalEvent().UpdateNewGongQuEvent != null)
                    {
                        var args = new UpdateCodeArgs()
                        {
                            Id = this._jwbeam.Id,
                            NewCode = this.select7.SelectedValue.ToString()
                        };
                        GlobalEvent.GetGlobalEvent().UpdateNewGongQuEvent(this, args);
                    }
                }
                else
                {
                    if (GlobalEvent.GetGlobalEvent().UpdateCodeEvent != null)
                    {
                        var args = new UpdateCodeArgs()
                        {
                            Id = this._jwbeam.Id,
                            NewCode = this.select7.SelectedValue.ToString()
                        };
                        await GlobalEvent.GetGlobalEvent().UpdateCodeEvent.InvokeAsync(this, args);
                        //GlobalEvent.GetGlobalEvent().UpdateCodeEvent(this, args);
                    }
                }
            }
            else
            {
                this.SuccessModal("1つ選択してください");
            }
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            if (this._jwbeam!=null)
            {
                ExportCsv(this._jwbeam);
            }
        }

        private void ExportCsv(JwBeam data)
        {
            
                var csvstr = data.ToProcessCsv();
                if (!string.IsNullOrEmpty(csvstr))
                {
                    SaveFileDialog saveDataSend = new SaveFileDialog();
                    // Environment.SpecialFolder.MyDocuments 表示在我的文档中
                    saveDataSend.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);   // 获取文件路径
                    saveDataSend.Filter = "*.csv|csv file";   // 设置文件类型为文本文件
                    saveDataSend.DefaultExt = ".csv";   // 默认文件的拓展名
                    saveDataSend.FileName = string.Format("{0}-3015-2.csv", data.BeamCode);   // 文件默认名
                    if (saveDataSend.ShowDialog() == DialogResult.OK)   // 显示文件框，并且选择文件
                    {
                        string fName = saveDataSend.FileName;   // 获取文件名
                                                                // 参数1：写入文件的文件名；参数2：写入文件的内容
                        byte[] bs = Encoding.GetEncoding("UTF-8").GetBytes(csvstr);
                        bs = Encoding.Convert(Encoding.GetEncoding("UTF-8"), Encoding.Default, bs);
                        string q = Encoding.Default.GetString(bs);
                        System.IO.File.WriteAllText(fName, q, Encoding.GetEncoding("Shift-JIS"));   // 向文件中写入内容
                        AntdUI.Modal.open(new AntdUI.Modal.Config(this.ParentForm, "完了プロンプト", "CSVへのエクスポートが完了しました。", AntdUI.TType.Success)
                        {
                            OnButtonStyle = (id, btn) =>
                            {
                                btn.BackExtend = "135, #6253E1, #04BEFE";
                            },
                            CancelText = null,
                            OkText = "YES"
                        });
                    }
                }
            
        }

    }
}
