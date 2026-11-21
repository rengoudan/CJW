using JwCore;
using JwData;
using JwServices;
using JwShapeCommon;
using RGBControls.Classes;
using RGBControls.Errors;
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
    public partial class FileParseForm : UIForm2
    {
        private JwProjectMainService JwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();
        public FileParseForm()
        {
            InitializeComponent();
            GlobalEvent.GetGlobalEvent().UpdateNewGongQuEvent += FileParseUpdateNewGongQuEvent;
        }

        private void FileParseUpdateNewGongQuEvent(object? sender, UpdateCodeArgs e)
        {
            if(_jwFileHandle.Beams.Count>0)
            {
                var bs = _jwFileHandle.Beams.Find(t => t.Id == e.Id);
                if (bs != null)
                {
                    bs.GongQu = e.NewCode;
                    //var msg = string.Format("梁番号:{0}、新しい工区コード:{1}", bs.BeamCode, e.NewCode);
                    var msg = $"梁番号:{bs.BeamCode}、新しい工区コード:{e.NewCode}";
                    this.SuccessModal(msg);
                }
            }
        }

        JwProjectPathModel? jwProjectPathModel = null!;
        string _path;
        public FileParseForm(JwProjectPathModel model)
        {
            InitializeComponent();
            jwProjectPathModel = model;
            GlobalEvent.GetGlobalEvent().ShowParseLogEvent += ShowParseLog;
            GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent += DeleteSelectedSquare;
            GlobalEvent.GetGlobalEvent().UpdateNewGongQuEvent += FileParseUpdateNewGongQuEvent;
        }

        private void DeleteSelectedSquare(object? sender, ControlSelectedSquareArgs e)
        {
            if(_jwFileHandle!=null)
            {
                _jwFileHandle.DeleteSquare(e);
            }
        }

        private void FileParseForm_Load(object sender, EventArgs e)
        {
            if (jwProjectPathModel != null)
            {
                if (!string.IsNullOrEmpty(jwProjectPathModel.Path))
                {
                    _path = jwProjectPathModel.Path;
                    this.ShowProcessForm(200);
                    try
                    {
                        readjww();
                    }
                    catch (Exception ex)
                    {
                        // 2026年11月21日 解析失败：先关进度窗，再提示失败的步骤，然后关闭本窗体
                        this.HideProcessForm();
                        var msg = (ex as ParseStepException)?.UserMessage ?? "図面解析中にエラーが発生しました。";
                        ErrorReporter.Report(ex, msg + "\n" + Path.GetFileName(_path));
                        this.Close();
                    }

                    //ThreadStart start = new ThreadStart(readjww);
                    //Thread thread = new Thread(start);
                    //thread.Priority = ThreadPriority.Highest;
                    //thread.IsBackground = true; //关闭窗体继续执行
                    //thread.Start();
                }
            }
        }
        JwFileHandle? _jwFileHandle = null!;

        private void readjww()
        {
            _jwFileHandle = new JwFileHandle(jwProjectPathModel);
            _jwFileHandle.ReadJwFile();

            var bb = JwFileConsts.BeamParseColor;
            var bc = JwFileConsts.BeamSymbolTextColor;
            //jwfh.ParseBySetting();
            //jwfh.ChangeJwXianFromJwwSen();
            //var sl = jwfh.SenLst;
            //jwfh.ChangePillarFromJwwSolid();
            //jwfh.ParseSquareCreatePillar();
            //jwfh.ChangeJwwEnojiToText();
            //jwfh.ChangeQieGeSolis();
            _jwFileHandle.FollowTheStep();
            var jc = _jwFileHandle.CreateCanvas();
            if (_jwFileHandle.HasCanvas) {
                JwCanvasDraw canvasDraw = new JwCanvasDraw(jc);
                if (jwCanvasControl1.InvokeRequired)
                {
                    this.HideProcessForm();
                    jwCanvasControl1.Invoke(() =>
                    {
                        jwCanvasControl1.IsNewCanvas = true;
                        jwCanvasControl1.CanvasDraw = canvasDraw;
                        //jwCanvasControl1.Click += JwCanvasControl1_Click;
                        jwCanvasControl1.SelectBeamEvent += JwCanvas_Click;
                    });
                }
                else
                {
                    this.HideProcessForm();
                    jwCanvasControl1.IsNewCanvas = true;
                    jwCanvasControl1.CanvasDraw = canvasDraw;
                    //jwCanvasControl1.Click += JwCanvasControl1_Click;
                    jwCanvasControl1.SelectBeamEvent += JwCanvas_Click;
                }
                if (uiSymbolButton1.InvokeRequired)
                {
                    uiSymbolButton1.Invoke(() => { uiSymbolButton1.Enabled = true; });
                }
                else
                {
                    uiSymbolButton1.Enabled = true;
                }
            }
            else
            {
                this.HideProcessForm();
                UIMessageBox.ShowError("現在の解析設定では有効なデータが検出されません!!");
                this.Close();
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
                //uiTextBox1.AppendText(mg);
                //uiTextBox1.BeginInvoke(() =>
                //{
                //    uiTextBox1.AppendText(mg);
                //});
            }
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

        private async void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (_jwFileHandle != null)
            {
                this.ShowProcessForm(200);
                try
                {
                    _jwFileHandle.CreateData();
                    if (_jwFileHandle._subData != null)
                    {
                        string fn = _jwFileHandle._subData.FloorName;
                        var sbd =await JwProjectMainService.FindSubData(t => t.FloorName == fn && t.JwProjectMainDataId == jwProjectPathModel.MainData.Id);
                        if (sbd == null)
                        {
                            await SaveDataAsync();
                        }
                        else
                        {
                            UIMessageBox.ShowError("このフロアは識別されアップロードされました");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 2026年11月21日 保存失败时不关闭窗体（不返回 OK），提示后允许用户重试或关闭
                    this.HideProcessForm();
                    ErrorReporter.Report(ex, "解析結果の保存に失敗しました。");
                    return;
                }
            }
            DialogResult = DialogResult.OK;
            this.HideProcessForm();
        }

        /// <summary>
        /// 2026年11月21日 原来用 Invoke(async lambda) 触发保存且不等待，窗体可能在保存完成前就关闭并刷新列表，
        /// 保存异常也无法被捕获。按钮事件本身就在 UI 线程，直接 await。
        /// </summary>
        private async Task SaveDataAsync()
        {
            if (_jwFileHandle?._subData != null)
            {
                await JwProjectMainService.SaveNewParseResult(_jwFileHandle);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //ContextFactory.DisposeContext();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
