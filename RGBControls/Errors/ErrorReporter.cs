using JwLogging;

namespace RGBControls.Errors
{
    /// <summary>
    /// 全局异常处理与显示。程序启动时调用 Install()，各处 catch 中可调用 Report()。
    /// </summary>
    public static class ErrorReporter
    {
        private static bool _installed;
        private static bool _dialogShowing;
        private static string? _lastSignature;
        private static DateTime _lastClosedAt = DateTime.MinValue;

        /// <summary>
        /// 必须在创建任何窗体之前调用。
        /// </summary>
        public static void Install()
        {
            if (_installed) return;
            _installed = true;

            AppLog.Init(typeof(ErrorReporter).Assembly.GetName().Version?.ToString());

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => Report(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                AppLog.Error(e.Exception, "未監視のTask例外");
                e.SetObserved();
            };
            Application.ApplicationExit += (s, e) => AppLog.CloseAndFlush();
        }

        /// <summary>
        /// 记录异常并显示错误对话框（自动切换到 UI 线程）。
        /// </summary>
        /// <param name="userMessage">给用户看的说明，为空时使用通用文案。</param>
        public static void Report(Exception ex, string? userMessage = null)
        {
            AppLog.Error(ex, "例外: {Message}", userMessage ?? ex.Message);

            var owner = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.IsHandleCreated && !f.IsDisposed);
            if (owner != null && owner.InvokeRequired)
            {
                try { owner.BeginInvoke(() => ShowDialog(ex, userMessage, owner)); } catch { /* 窗体已关闭，仅记录 */ }
                return;
            }
            ShowDialog(ex, userMessage, owner);
        }

        private static void ShowDialog(Exception ex, string? userMessage, IWin32Window? owner)
        {
            // 防止对话框连环弹出：显示中不再弹；同一异常在关闭后 3 秒内再次发生只记录
            var signature = ex.GetType().FullName + "|" + ex.TargetSite + "|" + ex.Message;
            if (_dialogShowing) return;
            if (signature == _lastSignature && (DateTime.Now - _lastClosedAt).TotalSeconds < 3) return;

            _dialogShowing = true;
            try
            {
                using var dlg = new ErrorDialog(ex, userMessage);
                if (owner != null) dlg.ShowDialog(owner); else dlg.ShowDialog();
            }
            catch (Exception dialogEx)
            {
                AppLog.Error(dialogEx, "エラーダイアログの表示に失敗");
            }
            finally
            {
                _dialogShowing = false;
                _lastSignature = signature;
                _lastClosedAt = DateTime.Now;
            }
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception ?? new Exception(e.ExceptionObject?.ToString());
            AppLog.Fatal(ex, "致命的な例外 (IsTerminating={IsTerminating})", e.IsTerminating);

            if (e.IsTerminating)
            {
                AppLog.CloseAndFlush();
                // 可能处于非 UI 线程，只用 MessageBox
                MessageBox.Show(
                    "致命的なエラーが発生したため、アプリケーションを終了します。\n\n" +
                    $"{ex.GetType().Name}: {ex.Message}\n\n" +
                    $"ログの保存先:\n{AppLog.LogDir}",
                    "致命的なエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
