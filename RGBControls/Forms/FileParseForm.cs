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

        private async void FileParseForm_Load(object sender, EventArgs e)
        {
            if (jwProjectPathModel != null && !string.IsNullOrEmpty(jwProjectPathModel.Path))
            {
                _path = jwProjectPathModel.Path;
                await ParseAsync();
            }
        }
        JwFileHandle? _jwFileHandle = null!;

        /// <summary>解析中は閉じられない（中止機能は無いため、完了まで待つ）。</summary>
        private bool _parsing;

        /// <summary>
        /// 2026年11月22日 読み込み・解析をバックグラウンドで実行し、進捗カードに表示する。
        /// 原来在 UI 线程同步执行，解析期间画面完全卡住，日志也要等到结束才一次性显示。
        /// 失败时、没有有效数据时窗体保持打开，结果显示在进度卡片中。
        /// </summary>
        private async Task ParseAsync()
        {
            _parsing = true;
            uiSymbolButton1.Enabled = false;
            InitReparseButton();
            SetReparseEnabled(false);
            BeginProgress(Path.GetFileName(_path));

            var handle = new JwFileHandle(jwProjectPathModel)
            {
                // UI スレッドで生成した Progress<T> は通知を UI スレッドへ自動で戻す
                Progress = new Progress<ParseProgress>(OnParseProgress),
            };
            _jwFileHandle = handle;

            JwCanvas? jc = null;
            Exception? error = null;
            try
            {
                jc = await Task.Run(() =>
                {
                    handle.ReadJwFile();
                    handle.FollowTheStep();
                    return handle.BuildCanvas();
                });
                if (handle.HasCanvas)
                {
                    // 描画オブジェクトとコントロールへの設定は UI スレッドで行う
                    JwCanvasDraw canvasDraw = new JwCanvasDraw(jc);
                    jwCanvasControl1.IsNewCanvas = true;
                    jwCanvasControl1.CanvasDraw = canvasDraw;
                    jwCanvasControl1.SelectBeamEvent -= JwCanvas_Click; // 再解析で二重登録しない
                    jwCanvasControl1.SelectBeamEvent += JwCanvas_Click;
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                _parsing = false;
                if (!IsDisposed) SetReparseEnabled(true);
            }

            if (IsDisposed) return;
            if (error != null)
            {
                // ステップ失敗は Step()、読み込み失敗は ReadJwFile で既にログ済み。それ以外（描画準備など）はここで記録
                if (error is not ParseStepException)
                    JwLogging.AppLog.Error(error, "図面解析の後処理で失敗 file={File}", Path.GetFileName(_path));
                EndProgressFailed(error);
                return;
            }
            if (!handle.HasCanvas)
            {
                EndProgressNoData();
                return;
            }
            uiSymbolButton1.Enabled = true;
            EndProgressSucceeded(handle);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_parsing)
            {
                e.Cancel = true;
                MessageBox.Show(this, "図面を解析中です。完了するまでお待ちください。", "解析中",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            base.OnFormClosing(e); // Cancel を設定したうえで基底にも通知する（購読側から取消が見える）
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // 2026年11月22日 全局静态事件的订阅原来从不取消：每打开一次就多挂一个处理函数，
            // 已关闭的窗体也会继续收到其他解析的日志
            GlobalEvent.GetGlobalEvent().ShowParseLogEvent -= ShowParseLog;
            GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent -= DeleteSelectedSquare;
            GlobalEvent.GetGlobalEvent().UpdateNewGongQuEvent -= FileParseUpdateNewGongQuEvent;
            base.OnFormClosed(e);
        }

        private void ShowParseLog(object sender, ShowParseLogArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return; // 閉じた後に他の解析から届いた通知は無視
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
                    // 2026年11月22日 同じ階の梁を「前へ／次へ」で切り替えられるよう、図と梁の一覧も渡す
                    var canvas = jwCanvasControl1.CanvasDraw?.jwCanvas;
                    NewJwBeamForm j = canvas != null ? new NewJwBeamForm(z, canvas, canvas.Beams) : new NewJwBeamForm(z);
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
                catch (FloorLimitException ex)
                {
                    // 2026年11月22日 工事の階数を超える取り込み（別の窓で先に保存された場合など）。何も保存されていない
                    this.HideProcessForm();
                    UIMessageBox.ShowWarning(ex.Message);
                    return;
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
