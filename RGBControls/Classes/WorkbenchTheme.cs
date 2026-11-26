using System.Drawing;
using System.Windows.Forms;
using WLabel = System.Windows.Forms.Label;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月27日 ワークベンチ（ホーム）と顧客管理で共通の配色・部品（表示のみ。解析・出力には関与しない）。
    /// 白いカード＋薄い枠線、薄灰の背景、見出し（太字）、AntdUI のボタンを両画面で同じにそろえる。
    /// </summary>
    public static class WorkbenchTheme
    {
        public static readonly Color Ink = Color.FromArgb(38, 38, 38);
        public static readonly Color Muted = Color.FromArgb(110, 110, 110);
        public static readonly Color CardBorder = Color.FromArgb(222, 226, 232);
        public static readonly Color PageBack = Color.FromArgb(244, 246, 249);
        public static readonly Color Blue = Color.FromArgb(22, 119, 255);
        public static readonly Color BlueTint = Color.FromArgb(240, 246, 255);
        public static readonly Color Red = Color.FromArgb(220, 53, 69);
        public static readonly Color Orange = Color.FromArgb(230, 140, 20);
        public static readonly Color Green = Color.FromArgb(40, 160, 80);

        public const string FontName = "Microsoft Sans Serif";

        /// <summary>白いカード。active を返すと青い枠（絞り込み中の数字カードなど）。</summary>
        public static Panel Card(Padding margin, Func<bool>? active = null)
        {
            var p = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = margin, Padding = new Padding(1) };
            p.Paint += (s, e) =>
            {
                bool on = active?.Invoke() ?? false;
                using var pen = new Pen(on ? Blue : CardBorder, on ? 2 : 1);
                e.Graphics.DrawRectangle(pen, 1, 1, p.Width - 3, p.Height - 3);
            };
            p.Resize += (s, e) => p.Invalidate();
            return p;
        }

        /// <summary>カード内の見出し（上端に寄せる）。</summary>
        public static WLabel SectionTitle(string text, float scale) => new()
        {
            Text = text, Dock = DockStyle.Top, Height = Px(34, scale), ForeColor = Ink, TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font(FontName, 11.5f, FontStyle.Bold), Padding = new Padding(Px(10, scale), 0, 0, 0),
        };

        /// <summary>見出し行の操作ボタン（ワークベンチと同じ角丸・高さ）。</summary>
        public static AntdUI.Button ActionButton(string text, AntdUI.TTypeMini type, Action onClick, float scale, bool ghost = false)
        {
            var b = new AntdUI.Button
            {
                Text = text, Type = type, Ghost = ghost, BorderWidth = type == AntdUI.TTypeMini.Default ? 1 : 0,
                AutoSizeMode = AntdUI.TAutoSize.Width, Height = Px(40, scale), Margin = new Padding(Px(6, scale), 0, 0, 0),
                Font = new Font(FontName, 10.5f), Radius = 6,
            };
            b.Click += (s, e) => onClick();
            return b;
        }

        /// <summary>DPI に合わせた寸法（96 DPI 基準の値）。</summary>
        public static int Px(float v, float scale) => (int)Math.Round(v * scale);
    }
}
