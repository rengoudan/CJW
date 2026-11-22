using System.Drawing;
using System.Windows.Forms;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月22日 画面フォントの統一（表示のみ。解析・出力には一切関係しない）。
    /// 设计器里各窗体写死的是「宋体」（中文字体，汉字按中文字形显示），首页用的是「微软雅黑」。
    /// 运行时把这些字体换成日文界面字体 Meiryo UI：只换字体名，字号、粗细、单位都不变。
    /// 本机没有安装 Meiryo UI 时（例如中文开发机）什么都不做，保持原样。
    /// 生效时机由 <see cref="UiHook"/> 决定：窗体激活之前（Load 之后、第一次绘制之前），之后追加的控件也会处理。
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

        public static bool Enabled => _target != null;

        /// <summary>起動時に一度だけ呼ぶ。対象フォントが無い環境では何もしない。</summary>
        public static void Install(string target = DefaultTarget, string monospace = DefaultMonospace)
        {
            if (_target != null) return;
            if (!IsInstalled(target)) return;
            _target = target;
            _mono = IsInstalled(monospace) ? monospace : null;
            UiHook.Register(FixControl);
        }

        /// <summary>指定したコントロール以下に適用する（以後追加される子コントロールにも適用）。</summary>
        public static void Apply(Control root) => UiHook.Apply(root);

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
            if (_target == null) return;
            // 親から継承しているフォントは親を直した時点で新しいフォントになっているので、ここでは一致しない
            if (IsTarget(c.Font))
            {
                if (IsMultilineText(c))
                {
                    if (_mono != null) c.Font = Get(c.Font, _mono);
                }
                else
                {
                    c.Font = Get(c.Font, _target);
                }
            }
            if (c.ContextMenuStrip != null) FixToolStrip(c.ContextMenuStrip);
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
    }
}
