using System.Runtime.ExceptionServices;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月21日 显示 AntdUI 加载动画，并真正等待异步任务完成。
    ///
    /// 原来的 Progress(Action) 把 async lambda 当作 Action（即 async void）调用：
    /// 任务只是被"触发"，加载动画是固定约 3.5 秒的假进度；任务耗时更长时动画先消失，
    /// 用户可继续操作而与仍在运行的任务并发访问共享 DbContext；任务中的异常也与调用方脱节。
    ///
    /// 这里在 Spin 的后台线程中：切回 UI 线程启动任务，然后阻塞等待其完成（UI 线程不被阻塞，
    /// 任务的 await 续体照常在 UI 线程执行）。任务中的异常在 RunAsync 返回时原样重新抛出。
    /// </summary>
    internal static class SpinRunner
    {
        public static async Task RunAsync(Control owner, Func<Task> work, string? text = null)
        {
            ExceptionDispatchInfo? error = null;
            await AntdUI.Spin.open(owner, text ?? AntdUI.Localization.Get("Processing", "処理中..."), config =>
            {
                try
                {
                    var task = (Task)owner.Invoke(work);   // 在 UI 线程启动
                    task.GetAwaiter().GetResult();          // 等待真正完成
                }
                catch (Exception ex)
                {
                    error = ExceptionDispatchInfo.Capture(ex);
                }
            }, null, null);
            error?.Throw();
        }

        public static Task RunAsync(Control owner, Action work, string? text = null)
            => RunAsync(owner, () => { work(); return Task.CompletedTask; }, text);
    }
}
