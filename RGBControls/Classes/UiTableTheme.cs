using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月22日 表の配色を窓の配色（SunnyUI 既定の青テーマ）にそろえる（表示のみ。解析・出力には関与しない）。
    /// 原来 SunnyUI 的表格是蓝色表头，AntdUI 的表格（工事・階一览、梁统计、楼层标签页、基础数据）和普通 DataGridView、ListView 是灰色表头；
    /// 选中行有的用系统高亮色（随 Windows 设置变化），有的用 AntdUI 默认色。这里统一：
    ///   表头：主色（标题栏同色）底 + 白字　隔行：浅蓝　选中行：浅蓝 + 深色字　悬停：更浅的蓝
    /// 生效时机同 <see cref="UiHook"/>（窗体显示前，之后追加的表格也会处理）。
    /// </summary>
    public static class UiTableTheme
    {
        /// <summary>主色：SunnyUI の標題バー・既存の表ヘッダーと同じ色。</summary>
        public static readonly Color Primary = Color.FromArgb(80, 160, 255);
        public static readonly Color HeaderFore = Color.White;
        public static readonly Color Stripe = Color.FromArgb(235, 243, 255);
        public static readonly Color Selected = Color.FromArgb(155, 200, 255);
        public static readonly Color Hover = Color.FromArgb(225, 238, 255);
        public static readonly Color TextDark = Color.FromArgb(48, 48, 48);
        public static readonly Color LightBorder = Color.FromArgb(200, 222, 255);

        private static bool _installed;
        private static readonly ConditionalWeakTable<ListView, object> OwnerDrawn = new();

        /// <summary>起動時に一度だけ呼ぶ。</summary>
        public static void Install()
        {
            if (_installed) return;
            _installed = true;
            UiHook.Register(Fix);
        }

        private static void Fix(Control c)
        {
            switch (c)
            {
                case AntdUI.Table t: FixAntdTable(t); break;
                case DataGridView g: FixGrid(g); break;
                case ListView lv: FixListView(lv); break;
            }
        }

        private static void FixAntdTable(AntdUI.Table t)
        {
            t.ColumnBack = Primary;
            t.ColumnFore = HeaderFore;
            t.RowSelectedBg = Selected;
            t.RowSelectedFore = TextDark;
            t.RowHoverBg = Hover;
            t.BorderColor = LightBorder;
            t.Invalidate();
        }

        private static void FixGrid(DataGridView g)
        {
            if (g is not Sunny.UI.UIDataGridView)
            {
                // SunnyUI の表は既に青ヘッダー・縞模様なので、普通の DataGridView だけ合わせる
                g.EnableHeadersVisualStyles = false;
                g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                g.ColumnHeadersDefaultCellStyle.BackColor = Primary;
                g.ColumnHeadersDefaultCellStyle.ForeColor = HeaderFore;
                g.AlternatingRowsDefaultCellStyle.BackColor = Stripe;
                g.BackgroundColor = Color.White;
                g.GridColor = LightBorder;
                // 行見出し：SunnyUI の表と同じく浅い青、選択時は主色
                g.RowHeadersDefaultCellStyle.BackColor = Stripe;
                g.RowHeadersDefaultCellStyle.SelectionBackColor = Primary;
                g.RowHeadersDefaultCellStyle.SelectionForeColor = HeaderFore;
            }
            // 見出しは選択されても色を変えない
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Primary;
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = HeaderFore;
            // 選択行：システムの強調色（Windows の設定で変わる）ではなく、テーマの浅い青
            SetSelection(g.DefaultCellStyle, force: true);
            SetSelection(g.RowsDefaultCellStyle, force: false);
            SetSelection(g.AlternatingRowsDefaultCellStyle, force: false);
        }

        /// <summary>明示的に選択色が指定されているスタイル（または既定スタイル）だけ置き換える。空なら既定から継承させる。</summary>
        private static void SetSelection(DataGridViewCellStyle s, bool force)
        {
            if (!force && s.SelectionBackColor.IsEmpty) return;
            s.SelectionBackColor = Selected;
            s.SelectionForeColor = TextDark;
        }

        private static void FixListView(ListView lv)
        {
            if (lv.View != View.Details || lv.HeaderStyle == ColumnHeaderStyle.None) return;
            if (OwnerDrawn.TryGetValue(lv, out _)) return;
            OwnerDrawn.Add(lv, null!);
            // 見出しと選択行だけ自前で描き、それ以外の行は既定の描画（行ごとの文字色はそのまま）
            lv.OwnerDraw = true;
            lv.DrawColumnHeader += DrawHeader;
            lv.DrawItem += (s, e) => e.DrawDefault = !e.Item.Selected;
            lv.DrawSubItem += DrawSubItem;
            lv.Invalidate();
        }

        private static void DrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
        {
            var lv = sender as ListView;
            bool selectedCell = e.Item != null && e.Item.Selected && (lv?.FullRowSelect == true || e.ColumnIndex == 0);
            if (!selectedCell || e.SubItem == null || lv == null)
            {
                e.DrawDefault = true;
                return;
            }
            // 選択行：システムの強調色ではなくテーマの浅い青。行の文字色（赤い警告など）は残す
            using (var back = new SolidBrush(Selected)) e.Graphics.FillRectangle(back, e.Bounds);
            var fore = e.Item!.ForeColor;
            if (fore.IsEmpty || fore == SystemColors.WindowText || fore == lv.ForeColor) fore = TextDark;
            var align = lv.Columns.Count > e.ColumnIndex ? lv.Columns[e.ColumnIndex].TextAlign : HorizontalAlignment.Left;
            var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | Align(align);
            var font = e.ColumnIndex == 0 || e.Item.UseItemStyleForSubItems ? e.Item.Font : e.SubItem.Font;
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, font ?? lv.Font, Rectangle.Inflate(e.Bounds, -3, 0), fore, flags);
        }

        private static TextFormatFlags Align(HorizontalAlignment a) => a switch
        {
            HorizontalAlignment.Right => TextFormatFlags.Right,
            HorizontalAlignment.Center => TextFormatFlags.HorizontalCenter,
            _ => TextFormatFlags.Left,
        };

        private static void DrawHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var back = new SolidBrush(Primary)) e.Graphics.FillRectangle(back, e.Bounds);
            using (var sep = new Pen(LightBorder))
                e.Graphics.DrawLine(sep, e.Bounds.Right - 1, e.Bounds.Top + 3, e.Bounds.Right - 1, e.Bounds.Bottom - 4);

            var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;
            flags |= e.Header?.TextAlign switch
            {
                HorizontalAlignment.Right => TextFormatFlags.Right,
                HorizontalAlignment.Center => TextFormatFlags.HorizontalCenter,
                _ => TextFormatFlags.Left,
            };
            var r = Rectangle.Inflate(e.Bounds, -4, 0);
            var font = (sender as ListView)?.Font ?? e.Font ?? SystemFonts.DefaultFont;
            TextRenderer.DrawText(e.Graphics, e.Header?.Text ?? "", font, r, HeaderFore, flags);
        }
    }
}
