using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月22日 画面フォントの統一（表示のみ。解析・出力には一切関係しない）。
    /// 设计器里各窗体写死的是「宋体」（中文字体，汉字按中文字形显示），首页用的是「微软雅黑」。
    /// 运行时把这些字体换成日文界面字体 Meiryo UI：只换字体名，字号、粗细、单位都不变。
    /// 本机没有安装 Meiryo UI 时（例如中文开发机）什么都不做，保持原样。
    /// 生效时机：窗体激活之前（Load 之后、第一次绘制之前），因此不会先显示宋体再跳变；之后追加的控件也会处理。
    /// 多行文本框（日志、CSV 预览）换成等宽的 MS Gothic，保持字符对齐。
    /// </summary>
    public static class UiFont
    {
        public const string DefaultTarget = "Meiryo UI";
        public const string DefaultMonospace = "MS Gothic";

        /// <summary>置き換え対象のフォント名（要求名・実際名のどちらかが一致すれば対象）。</summary>
        private static readonly HashSet<string> ReplaceNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "宋体", "SimSun", "新宋体", "NSimSun",
            "微软雅黑", "Microsoft YaHei", "Microsoft YaHei UI",
            "Microsoft Sans Serif", "MS UI Gothic",
        };

        private static string? _target;
        private static string? _mono;
        private static readonly Dictionary<(string, float, FontStyle, GraphicsUnit), Font> Cache = new();
        private static readonly ConditionalWeakTable<Control, object> Hooked = new();

        private delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
        private static HookProc? _proc; // GC されないよう保持
        private static IntPtr _hook;

        public static bool Enabled => _target != null;

        /// <summary>起動時に一度だけ呼ぶ。対象フォントが無い環境では何もしない。</summary>
        public static void Install(string target = DefaultTarget, string monospace = DefaultMonospace)
        {
            if (_hook != IntPtr.Zero) return;
            if (!IsInstalled(target)) return;
            _target = target;
            _mono = IsInstalled(monospace) ? monospace : null;
            try
            {
                _proc = CbtProc;
                _hook = SetWindowsHookEx(WH_CBT, _proc, IntPtr.Zero, GetCurrentThreadId());
            }
            catch (Exception ex)
            {
                JwLogging.AppLog.Warn(ex, "UiFont: hook install failed");
            }
        }

        /// <summary>指定したコントロール以下に適用する（以後追加される子コントロールにも適用）。</summary>
        public static void Apply(Control root)
        {
            if (_target == null) return;
            try { ApplyTree(root); }
            catch (Exception ex) { JwLogging.AppLog.Warn(ex, "UiFont: apply failed on {Control}", root.GetType().Name); }
        }

        private static bool IsInstalled(string name)
        {
            try
            {
                using var f = new FontFamily(name);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private static void ApplyTree(Control c)
        {
            FixControl(c);
            foreach (Control child in c.Controls) ApplyTree(child);
            if (c.ContextMenuStrip != null) FixToolStrip(c.ContextMenuStrip);
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

        private static bool IsTarget(Font? f) =>
            f != null && (ReplaceNames.Contains(f.Name) || (f.OriginalFontName != null && ReplaceNames.Contains(f.OriginalFontName)));

        private static bool IsMultilineText(Control c) =>
            (c is TextBoxBase tb && tb.Multiline) || (c is Sunny.UI.UITextBox ut && ut.Multiline);

        private static Font Get(Font f, string name)
        {
            var key = (name, f.Size, f.Style, f.Unit);
            if (!Cache.TryGetValue(key, out var nf))
            {
                nf = new Font(name, f.Size, f.Style, f.Unit);
                Cache[key] = nf;
            }
            return nf;
        }

        private static void FixControl(Control c)
        {
            // 親から継承しているフォントは親を直した時点で新しいフォントになっているので、ここでは一致しない
            if (IsTarget(c.Font))
            {
                if (IsMultilineText(c))
                {
                    if (_mono != null) c.Font = Get(c.Font, _mono);
                }
                else
                {
                    c.Font = Get(c.Font, _target!);
                }
            }
            if (c is DataGridView g) FixGrid(g);
            else if (c is ToolStrip ts) FixToolStrip(ts);
        }

        private static void FixStyle(DataGridViewCellStyle? s)
        {
            if (s?.Font != null && IsTarget(s.Font)) s.Font = Get(s.Font, _target!);
        }

        private static void FixGrid(DataGridView g)
        {
            FixStyle(g.DefaultCellStyle);
            FixStyle(g.ColumnHeadersDefaultCellStyle);
            FixStyle(g.RowHeadersDefaultCellStyle);
            FixStyle(g.RowsDefaultCellStyle);
            FixStyle(g.AlternatingRowsDefaultCellStyle);
            foreach (DataGridViewColumn col in g.Columns)
                if (col.HasDefaultCellStyle) FixStyle(col.DefaultCellStyle);
        }

        private static void FixToolStrip(ToolStrip ts)
        {
            if (IsTarget(ts.Font)) ts.Font = Get(ts.Font, _target!);
            foreach (ToolStripItem item in ts.Items) FixItem(item);
        }

        private static void FixItem(ToolStripItem item)
        {
            if (IsTarget(item.Font)) item.Font = Get(item.Font, _target!);
            if (item is ToolStripDropDownItem dd && dd.HasDropDownItems)
                foreach (ToolStripItem sub in dd.DropDownItems) FixItem(sub);
        }

        // ── 窓のアクティブ化直前（表示前）に適用するためのスレッド内 CBT フック ──
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
