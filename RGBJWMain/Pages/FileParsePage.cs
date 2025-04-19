using JwShapeCommon;
using RGBJWMain.Controls;
using RGBJWMain.Forms;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RGBJWMain.Pages
{
    public partial class FileParsePage : UIPage
    {
        public FileParsePage()
        {
            InitializeComponent();
            GlobalEvent.GetGlobalEvent().ShowParseLogEvent += ShowParseLog;
            //uiTextBox1.AppendText(@"sdf"+System.Environment.NewLine);
            //uiTextBox1.AppendText("sdf");
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            var f = new OpenFileDialog();
            f.Filter = "Jww Files|*.jww|Jws Files|*.jws|All Files|*.*";
            if (f.ShowDialog() != DialogResult.OK) return;
            OpenFile(f.FileName);
            //ガベコレしないとunmanage側のString関係のオブジェクトでコンソールにメッセージが出るので
            //ここでガベコレしています。メッセージは出ていても問題はないそうですが、
            //デバッグ中のメモリリークの確認のためにここに入れました。無くても構いません。
            GC.Collect();
        }

        private string _path = "";

        void OpenFile(String path)
        {
            try
            {
                if (Path.GetExtension(path).ToLower() == ".jww")
                {

                    _path = path;

                    JwFileSettingForm jfsf = new JwFileSettingForm();
                    if (jfsf.ShowDialog() == DialogResult.OK)
                    {
                        ThreadStart start = new ThreadStart(readjww);
                        Thread thread = new Thread(start);
                        thread.Priority = ThreadPriority.Highest;
                        thread.IsBackground = true; //关闭窗体继续执行
                        thread.Start();
                    }
                    //JwwReaderが読み込み用のクラス。
                    //using var reader = new JwwHelper.JwwReader();
                    ////Completedは読み込み完了時に実行される関数。
                    //reader.Read(path, Completed);
                    //var a = reader.Header.m_jwwDataVersion;

                }
                else if (Path.GetExtension(path) == ".jws")
                {
                    ////jwsも読めますが、このプロジェクトでは確認用のコードがありません。
                    //using var a = new JwwHelper.JwsReader();
                    //a.Read(path, Completed2);
                }
            }
            catch (Exception exception)
            {
                //textBox1.Text = "";
                //MessageBox.Show(exception.Message, "Error");
            }
        }

        private void readjww()
        {
            JwFileHandle jwfh = new JwFileHandle(_path);
            jwfh.ReadJwFile();
            
            var bb = JwFileConsts.BeamParseColor;
            var bc = JwFileConsts.BeamSymbolTextColor;
            //jwfh.ParseBySetting();
            //jwfh.ChangeJwXianFromJwwSen();
            //var sl = jwfh.SenLst;
            //jwfh.ChangePillarFromJwwSolid();
            //jwfh.ParseSquareCreatePillar();
            //jwfh.ChangeJwwEnojiToText();
            //jwfh.ChangeQieGeSolis();
            jwfh.FollowTheStep();
            var jc = jwfh.CreateCanvas();
            JwCanvasDraw canvasDraw = new JwCanvasDraw(jc);
            if (jwCanvasControl1.InvokeRequired)
            {
                jwCanvasControl1.Invoke(() =>
                {
                    jwCanvasControl1.CanvasDraw = canvasDraw;
                    jwCanvasControl1.Click += JwCanvasControl1_Click;
                    jwCanvasControl1.SelectBeamEvent += JwCanvas_Click;
                });
            }
            else
            {
                jwCanvasControl1.CanvasDraw = canvasDraw;
                jwCanvasControl1.Click += JwCanvasControl1_Click;
                jwCanvasControl1.SelectBeamEvent += JwCanvas_Click;
            }
            
            //jwShowBeams1.Canvas = jc;
            //jwShowBeams1.CreateBeams();
            //jwShowBeams1.CanvasDraw = canvasDraw;
            //jwShowBeams1.Click += JwShowBeams1_Click;
            //jwfh.sss();
            //分割需要去除重复
            //var lst = jwfh.SolidLst.Select(t => t.m_nPenColor).ToList().Distinct().ToList();
            //var s = jwfh.RectangleBlocks;
        }

        private void JwCanvasControl1_Click(object? sender, EventArgs e)
        {

        }
        private void JwCanvas_Click(object? sender, EventArgs e)
        {
            if (jwCanvasControl1.BeamSelected)
            {
                var z = jwCanvasControl1.SelectedBeam;

                if (z != null)
                {
                    //JwSingleBeamForm jsForm = new JwSingleBeamForm(z);

                    ////jsForm.ShowBeam = js;
                    ////jsForm.sha
                    //jsForm.ShowDialog();

                    NewJwBeamForm j=new NewJwBeamForm(z);
                    j.ShowDialog();
                }
            }
        }

        private void ShowParseLog(object sender, ShowParseLogArgs e)
        {
            string mg = string.Format("{0}{1}", e.Msg, Environment.NewLine);
            if (uiTextBox1.InvokeRequired)
            {
                uiTextBox1.BeginInvoke(() =>
                {
                    uiTextBox1.AppendText(mg);
                });
            }
            else
            {

                if (e.ShowTime && e.UpdateTime.HasValue)
                {

                }
                uiTextBox1.AppendText(mg);
            }
        }

        private void uiSplitContainer1_RightToLeftChanged(object sender, EventArgs e)
        {

        }

        private void uiSplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            jwCanvasControl1.Refresh();
        }


        //private void JwShowBeams1_Click(object? sender, EventArgs e)
        //{

        //}
    }
}
