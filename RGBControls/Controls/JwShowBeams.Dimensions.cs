using System.Drawing;
using System.Drawing.Drawing2D;
using JwCore;
using JwShapeCommon;

namespace RGBJWMain.Controls
{
    /// <summary>
    /// 2026年11月23日 図の周りに通り芯（梁の中心線）の寸法を表示する（jww 出力の JwCanvas.DrawGridDimensions と同じ内容）。
    /// 左右：水平梁の中心線の間隔、上下：垂直梁の中心線の間隔。内側＝連続寸法、外側＝全長（通り芯 3 本以上）。
    /// 数値は梁の Center（図面座標）から実寸で計算し、位置は画面上の梁の位置に合わせる。
    /// 寸法線・文字の大きさは画面のピクセルで一定（拡大縮小しても読みやすい）。
    /// </summary>
    public partial class JwShowBeams
    {
        private bool _showDimension = true;

        /// <summary>寸法を表示するときの、図の自動フィットの余白（左右・上下それぞれ合計の px。片側 約 85px）。</summary>
        private const int DimensionFitMargin = 170;

        /// <summary>通り芯の寸法を表示するか（画面の「寸法」スイッチ）。切り替えると余白を変えて図を合わせ直す。</summary>
        public bool ShowDimension
        {
            get => _showDimension;
            set
            {
                if (_showDimension == value) return;
                _showDimension = value;
                setcanvas(_canvasDraw);
                Invalidate();
            }
        }

        private static readonly Color DimColor = Color.FromArgb(185, 185, 185);

        /// <summary>OnPaint から呼ぶ（座標変換は適用済み）。</summary>
        private void DrawGridDimensions(Graphics g)
        {
            if (!_showDimension || _bounds == null) return;
            var beams = _bounds.Where(b => b.ShapeType == DrawShapeType.Beam && b.JwSquareBase is JwBeam && b.DrawPoints?.Count > 0).ToList();
            if (beams.Count == 0) return;

            // 図の外形（梁・柱の描画位置）
            var all = _bounds.Where(b => (b.ShapeType == DrawShapeType.Beam || b.ShapeType == DrawShapeType.Pillar)).ToList();
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            foreach (var d in all)
            {
                var r = Box(d);
                if (r.IsEmpty) continue;
                minX = Math.Min(minX, r.Left); maxX = Math.Max(maxX, r.Right);
                minY = Math.Min(minY, r.Top); maxY = Math.Max(maxY, r.Bottom);
            }
            if (minX > maxX) return;

            double tol = GridDimension.Tolerance;
            // 水平梁：Center は Y、画面では縦位置（矩形の中央 Y）
            var hLines = GridDimension.Merge(beams.Where(b => ((JwBeam)b.JwSquareBase).DirectionType == BeamDirectionType.Horizontal)
                .Select(b => (((JwBeam)b.JwSquareBase).Center, (double)(Box(b).Top + Box(b).Height / 2))), tol);
            var vLines = GridDimension.Merge(beams.Where(b => ((JwBeam)b.JwSquareBase).DirectionType == BeamDirectionType.Vertical)
                .Select(b => (((JwBeam)b.JwSquareBase).Center, (double)(Box(b).Left + Box(b).Width / 2))), tol);

            float px = 1f / Math.Max(scale, 0.01f);     // 画面 1 ピクセル分の論理長
            float inner = 22 * px, outer = 44 * px, gap = 3 * px, dot = 2.5f * px;
            using var pen = new Pen(DimColor, px);
            using var brush = new SolidBrush(DimColor);
            using var font = new Font(Font.FontFamily, 12 * px, FontStyle.Regular, GraphicsUnit.Pixel);
            var state = g.Save();
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 上下（画面の上＝minY）：垂直梁の中心線の間隔
            Chain(g, pen, brush, font, vLines, horizontalDim: true, at: minY - inner, outward: -1, outerShift: outer - inner, gap, dot);
            Chain(g, pen, brush, font, vLines, horizontalDim: true, at: maxY + inner, outward: +1, outerShift: outer - inner, gap, dot);
            // 左右：水平梁の中心線の間隔
            Chain(g, pen, brush, font, hLines, horizontalDim: false, at: minX - inner, outward: -1, outerShift: outer - inner, gap, dot);
            Chain(g, pen, brush, font, hLines, horizontalDim: false, at: maxX + inner, outward: +1, outerShift: outer - inner, gap, dot);
            g.Restore(state);
        }

        private static RectangleF Box(ControlDraw d)
        {
            if (d.DrawPoints != null && d.DrawPoints.Count > 0)
            {
                float x1 = d.DrawPoints.Min(p => p.X), x2 = d.DrawPoints.Max(p => p.X);
                float y1 = d.DrawPoints.Min(p => p.Y), y2 = d.DrawPoints.Max(p => p.Y);
                return RectangleF.FromLTRB(x1, y1, x2, y2);
            }
            return d.DrawRectangleF;
        }

        private static void Chain(Graphics g, Pen pen, Brush brush, Font font, List<GridDimension.Line> lines, bool horizontalDim, float at, int outward, float outerShift, float gap, float dot)
        {
            if (lines.Count < 2) return;
            for (int i = 0; i + 1 < lines.Count; i++)
                Dim(g, pen, brush, font, (float)lines[i].Pos, (float)lines[i + 1].Pos, at, horizontalDim, outward, GridDimension.Text(lines[i].Value, lines[i + 1].Value), gap, dot);
            if (lines.Count >= 3)
                Dim(g, pen, brush, font, (float)lines[0].Pos, (float)lines[^1].Pos, at + outward * outerShift, horizontalDim, outward, GridDimension.Text(lines[0].Value, lines[^1].Value), gap, dot);
        }

        /// <summary>寸法 1 つ（線・両端の点・外側の数字）。</summary>
        private static void Dim(Graphics g, Pen pen, Brush brush, Font font, float a, float b, float at, bool horizontalDim, int outward, string text, float gap, float dot)
        {
            float mid = (a + b) / 2;
            var size = g.MeasureString(text, font);
            if (horizontalDim)
            {
                g.DrawLine(pen, a, at, b, at);
                g.FillEllipse(brush, a - dot, at - dot, dot * 2, dot * 2);
                g.FillEllipse(brush, b - dot, at - dot, dot * 2, dot * 2);
                float ty = outward < 0 ? at - gap - size.Height : at + gap;
                g.DrawString(text, font, brush, mid - size.Width / 2, ty);
            }
            else
            {
                g.DrawLine(pen, at, a, at, b);
                g.FillEllipse(brush, at - dot, a - dot, dot * 2, dot * 2);
                g.FillEllipse(brush, at - dot, b - dot, dot * 2, dot * 2);
                // 縦の数字は 90° 回して（下から上へ読む）寸法線の外側に置く
                var st = g.Save();
                float cx = outward < 0 ? at - gap - size.Height / 2 : at + gap + size.Height / 2;
                g.TranslateTransform(cx, mid);
                g.RotateTransform(-90);
                g.DrawString(text, font, brush, -size.Width / 2, -size.Height / 2);
                g.Restore(st);
            }
        }
    }
}
