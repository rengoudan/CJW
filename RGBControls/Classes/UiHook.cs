using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月22日 画面の見た目をまとめて整えるための共通フック（表示のみ。解析・出力には関与しない）。
    /// 登録された処理（フォント統一・表の配色統一など）を、各窓がアクティブになる直前（Load 後・最初の描画前）に
    /// 窓内の全コントロールへ適用する。後から追加されたコントロールにも ControlAdded で適用する。
    /// </summary>
    internal static class UiHook
    {
        private static readonly List<Action<Control>> Fixers = new();
        private static readonly ConditionalWeakTable<Control, object> Hooked = new();

        private delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
        private static HookProc? _proc; // GC されないよう保持
        private static IntPtr _hook;

        /// <summary>処理を登録し、まだならスレッド内 CBT フックを入れる。</summary>
        public static void Register(Action<Control> fixer)
        {
            Fixers.Add(fixer);
            if (_hook != IntPtr.Zero) return;
            try
            {
                _proc = CbtProc;
                _hook = SetWindowsHookEx(WH_CBT, _proc, IntPtr.Zero, GetCurrentThreadId());
            }
            catch (Exception ex)
            {
                JwLogging.AppLog.Warn(ex, "UiHook: hook install failed");
            }
        }

        /// <summary>指定したコントロール以下に全処理を適用する（以後追加される子コントロールにも適用）。</summary>
        public static void Apply(Control root)
        {
            if (Fixers.Count == 0) return;
            try { ApplyTree(root); }
            catch (Exception ex) { JwLogging.AppLog.Warn(ex, "UiHook: apply failed on {Control}", root.GetType().Name); }
        }

        private static void ApplyTree(Control c)
        {
            foreach (var fix in Fixers)
            {
                try { fix(c); }
                catch (Exception ex) { JwLogging.AppLog.Debug("UiHook: fixer failed on {Control}: {Error}", c.GetType().Name, ex.Message); }
            }
            foreach (Control child in c.Controls) ApplyTree(child);
            if (!Hooked.TryGetValue(c, out _))
            {
                Hooked.Add(c, null!);
                c.ControlAdded += OnControlAdded;
            }
        }

        private static void OnControlAdded(object? sender, ControlEventArgs e)
        {
            if (e.Control != null) Apply(e.Control);
        }

        private const int WH_CBT = 5;
        private const int HCBT_ACTIVATE = 5;

        private static IntPtr CbtProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code == HCBT_ACTIVATE)
            {
                try
                {
                    if (Control.FromHandle(wParam) is Form f && !Hooked.TryGetValue(f, out _))
                        Apply(f);
                }
                catch
                {
                    // フックの中では例外を外へ出さない
                }
            }
            return CallNextHookEx(_hook, code, wParam, lParam);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();
    }
}
