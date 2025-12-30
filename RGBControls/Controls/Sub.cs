using JwCore;
using JwServices;
using JwShapeCommon;
using RGBControls.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBControls.Controls
{
    public partial class Sub : UserControl
    {

        private JwProjectSubData _subdata;

        public JwProjectSubData SubData
        {
            get { return _subdata; }
            set
            {
                _subdata = value;
                if (_subdata != null)
                {
                    this.pageHeader1.Text = _subdata.FloorName;
                    this.pageHeader1.Description = "解析されたデータからさまざまな処理図または関連する CSV ファイルをエクスポートします。";
                    //this.pageHeader1.Refresh();
                }
            }
        }

        public Sub()
        {
            InitializeComponent();
        }

        public Sub(JwProjectSubData subdata)
        {
            InitializeComponent();
            SubData = subdata;
            //this.Resize += Sub_Resize;

        }



        private void Sub_Resize(object? sender, EventArgs e)
        {
            this.Invalidate();
            this.jwCanvasControl1.Invalidate();
        }

        private void Sub_Load(object sender, EventArgs e)
        {
            try
            {
                if (_subdata != null)
                {
                    JwCanvas jwc = _subdata.DataToCanvas();
                    JwCanvasDraw canvasDraw = new JwCanvasDraw(jwc);
                    this.jwCanvasControl1.CanvasDraw = canvasDraw;
                    //this.jwShowBeams1.ShowBeams = true;
                    //this.jwShowBeams1.ShowDownB = true;
                    //this.jwShowBeams1.ShowFuzhu = true;
                    //this.jwShowBeams1.ShowGoujian = true;
                    //this.jwShowBeams1.ShowGoujiantext = false;
                    //this.jwShowBeams1.ShowPillar = true;

                    this.liangtable.DataSource = _subdata.JwBeamDatas;

                    this.zhutable.DataSource = _subdata.JwPillarDatas;

                    this.lianjietable.DataSource = _subdata.JwLianjieDatas;

                    this.bbgtable.DataSource = _subdata.JwLinkPartDatas;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sub控件加载失败: " + ex.Message);
            }

        }

        /// <summary>
        /// 梁双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void liangtable_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var beam = liangtable[e.RowIndex - 1].record as JwBeamData;
                if (beam != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = beam.Id,
                            DrawShapeType = JwCore.DrawShapeType.Beam
                        });
                    }
                }
            }
        }

        private void zhutable_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var pillar = zhutable[e.RowIndex - 1].record as JwPillarData;
                if (pillar != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = pillar.Id,
                            DrawShapeType = JwCore.DrawShapeType.Pillar
                        });
                    }
                }
            }
        }

        private void lianjietable_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var lj = lianjietable[e.RowIndex - 1].record as JwLianjieData;
                if (lj != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = lj.Id,
                            IsLianjie = true
                        });
                    }
                }

            }
        }

        private void bbgtable_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var beam = bbgtable[e.RowIndex - 1].record as JwLinkPartData;
                if (beam != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = beam.Id,
                            DrawShapeType = JwCore.DrawShapeType.LinkPart
                        });
                    }
                }

            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (AntdUI.Modal.open(this.ParentForm, "ヒント", "プロジェクトデータをすべてエクスポートするかどうか") == DialogResult.OK)
            {
                await Progress(async () => { await SaveSubBeams(_subdata); });
                if (!string.IsNullOrEmpty(_nowsavefold))
                {
                    this.ParentForm.SuccessModal("エクスポートされたビーム-->" + _nowsavefold);
                    //UIMessageBox.ShowSuccess("エクスポートされたビーム-->" + _nowsavefold);
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if(_subdata != null)
            {
                await Progress(async () => { await SaveSubTopCanvas(_subdata); });
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (_subdata != null)
            {
                await Progress(async () => { await SaveSubBottomCanvas(_subdata); });
                //SaveSubBottomCanvas(selectedsubData);
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (_subdata != null)
            {
                await Progress(async () => { await SaveSubCanvasLines(_subdata); });
                //SaveSubCanvasLines(selectedsubData);
            }
        }

        private async Task Progress(Action action)
        {
            await AntdUI.Spin.open(this.ParentForm, AntdUI.Localization.Get("Loading2", "読み込み中..."), async config =>
            {
                Thread.Sleep(100);
                this.Invoke(() =>
                {
                    action();
                });
                for (int i = 0; i < 101; i++)
                {
                    config.Value = i / 100F;
                    config.Text = AntdUI.Localization.Get("Processing", "処理") + " " + i + "%";
                    Thread.Sleep(20);
                }
                Thread.Sleep(1000);
                config.Value = null;
                config.Text = AntdUI.Localization.Get("PleaseWait", "お待ちください。...");
                Thread.Sleep(2000);
            }, () =>
            {
                System.Diagnostics.Debug.WriteLine("仕上げる");
            });
        }

        #region 各类jww及施工图导出

        private string _nowsavefold = "";
        private async Task SaveSubBeams(JwProjectSubData data)
        {

            var maindata = _subdata.JwProjectMainData;

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //this.ShowProcessForm(200);
                string foldPath = dialog.SelectedPath + "\\" + maindata.ProjectName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }
                else
                {
                    Directory.Delete(foldPath, true);
                    Directory.CreateDirectory(foldPath);
                }
                _nowsavefold = foldPath;

                
                string subpath = foldPath + "\\" + data.FloorName;
                if (!Directory.Exists(subpath))
                {
                    Directory.CreateDirectory(subpath);
                }
                //this.dbContext.Entry(sub).Collection(e => e.jwho).Load();
                JwCanvas jwCanvas = data.DataToCanvas();
                if (jwCanvas.Beams.Count > 0)
                {
                    var gbeams = jwCanvas.Beams.GroupBy(t => t.BeamCode).ToList();
                    foreach (var b in gbeams)
                    {
                        var bm = b.First();
                        string sl = "";
                        if (b.ToList().Count > 1)
                        {
                            sl = "(" + b.ToList().Count.ToString() + ")";
                        }
                        string wjm = string.Format("{0}{1}.jww", b.Key, sl);
                        NewJwBeamJwDraw jwDraw = new NewJwBeamJwDraw(bm);
                        jwDraw.CreateBeam();
                        if (jwDraw.Sens.Count > 0)
                        {
                            using var a = new JwwHelper.JwwWriter();

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
                            a.Write(subpath + "\\" + wjm);
                        }
                    }
                }
            }
        }

        private async Task SaveSubTopCanvas(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath + "\\" + data.FloorName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }

                //await JwProjectMainService.LoadSubCollectionAsync(data);
                //if (data.JwBeamDatas.Count > 0)
                //{
                //    foreach (var bd in data.JwBeamDatas)
                //    {
                //        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                //    }
                //}
                JwCanvas jwCanvas = data.DataToCanvas();
                jwCanvas.BindDrawOtherEvent();
                var lst = jwCanvas.DrawShigong(true);
                using var a = new JwwHelper.JwwWriter();
                //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                a.InitHeader("template.jww");
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu+
                //)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + data.FloorName + ".jww");
            }
        }

        private async Task SaveSubBottomCanvas(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath + "\\" + data.FloorName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }

                //await JwProjectMainService.LoadSubCollectionAsync(data);
                //if (data.JwBeamDatas.Count > 0)
                //{
                //    foreach (var bd in data.JwBeamDatas)
                //    {
                //        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                //    }
                //}
                JwCanvas jwCanvas = data.DataToCanvas();
                jwCanvas.BindDrawOtherEvent();
                var lst = jwCanvas.DrawShigong(false);
                using var a = new JwwHelper.JwwWriter();
                //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                a.InitHeader("template.jww");
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu+
                //)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + data.FloorName + ".jww");
            }
        }
        private async Task SaveSubCanvasLines(JwProjectSubData data)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "";
            string flname = string.Format("{0}_ブレース施工図.jww", data.FloorName);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath + "\\" + data.FloorName;
                if (!Directory.Exists(foldPath))
                {
                    Directory.CreateDirectory(foldPath);
                }
                //await JwProjectMainService.LoadSubCollectionAsync(data);
                //if (data.JwBeamDatas.Count > 0)
                //{
                //    foreach (var bd in data.JwBeamDatas)
                //    {
                //        await JwProjectMainService.LoadBeamCollectionAsync(bd);
                //    }
                //}
                JwCanvas jwCanvas = data.DataToCanvas();
                jwCanvas.BindDrawOtherEvent();
                var lst = jwCanvas.DrawLinesToJww();
                using var a = new JwwHelper.JwwWriter();
                //JwwHelper.dllと同じフォルダに"template.jww"が必要です。
                //"template.jww"は適当なjwwファイルでそのファイルからjwwファイルのヘッダーをコピーします。
                //Headerをプログラムから設定してもいいのですが、項目が多いので大変です。
                a.InitHeader("template.jww");
                foreach (var s in lst)
                {
                    a.AddData(s);
                }
                //foreach(var b in jwDraw.Biaozhu)
                //{
                //    a.AddData(b);
                //}
                a.Write(foldPath + "\\" + flname);
            }
        }



        #endregion
    }
}
