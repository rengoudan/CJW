using JwCore;
using JwwHelper;

namespace JwShapeCommon
{
    /// <summary>
    /// 2026年11月23日 設計図の jww 出力に通り芯の寸法を追加する（番付図上・番付図下・ブレース施工図）。
    /// ・左右：水平梁の中心線（Y）の間隔　上下：垂直梁の中心線（X）の間隔
    /// ・内側＝隣り合う通り芯の連続寸法、外側＝全長（通り芯が 3 本以上のとき。2 本なら内側と同じなので省く）
    /// ・図（梁・柱の外形）のすぐ外側に置き、数字は寸法線の外側（図に重ならない）
    /// ・色はブレース長さの寸法（JwLianjieSingle）と同じ灰色の設定：線色 2、文字 2×3、端点 線色 9
    /// 梁・穴・CSV など他の出力内容は変えない（追加のみ）。
    /// </summary>
    public partial class JwCanvas
    {
        // 図面（用紙）上の寸法。jww の座標は用紙 mm（実寸＝座標×縮尺）
        private const double DimInner = 6;      // 図の外形から内側の寸法線まで
        private const double DimOuter = 12;     // 図の外形から外側（全長）の寸法線まで
        private const double DimTextGap = 0.8;  // 寸法線と数字の間
        private const double DimTextW = 2;      // 文字幅
        private const double DimTextH = 3;      // 文字高さ
        private const short DimPenColor = 2;    // 灰色（ブレース長さの寸法と同じ）
        private const short DimTenColor = 9;

        /// <summary>通り芯の寸法（jww データ）。梁が無ければ空。</summary>
        public List<JwwData> DrawGridDimensions()
        {
            var jd = new List<JwwData>();
            if (Beams == null || Beams.Count == 0) return jd;

            // 図の外形（梁・柱）
            var pts = new List<JWPoint>();
            void AddCorners(JwSquareBase s)
            {
                foreach (var p in new[] { s.TopLeft, s.TopRight, s.BottomLeft, s.BottomRight })
                    if (p != null) pts.Add(p);
            }
            foreach (var b in Beams) AddCorners(b);
            if (Pillars != null) foreach (var p in Pillars) AddCorners(p);
            if (pts.Count == 0) return jd;
            double minX = pts.Min(p => p.X), maxX = pts.Max(p => p.X), minY = pts.Min(p => p.Y), maxY = pts.Max(p => p.Y);

            double tol = GridDimension.Tolerance;
            var hLines = GridDimension.Merge(Beams.Where(b => b.DirectionType == BeamDirectionType.Horizontal).Select(b => (b.Center, b.Center)), tol);
            var vLines = GridDimension.Merge(Beams.Where(b => b.DirectionType == BeamDirectionType.Vertical).Select(b => (b.Center, b.Center)), tol);

            // 上下：垂直梁の中心線（X）の間隔
            Chain(jd, vLines, horizontalDim: true, at: minY - DimInner, outward: -1);
            Chain(jd, vLines, horizontalDim: true, at: maxY + DimInner, outward: +1);
            // 左右：水平梁の中心線（Y）の間隔
            Chain(jd, hLines, horizontalDim: false, at: minX - DimInner, outward: -1);
            Chain(jd, hLines, horizontalDim: false, at: maxX + DimInner, outward: +1);
            return jd;
        }

        /// <summary>1 辺ぶん：内側の連続寸法と、外側の全長。</summary>
        private static void Chain(List<JwwData> jd, List<GridDimension.Line> lines, bool horizontalDim, double at, int outward)
        {
            if (lines.Count < 2) return;
            for (int i = 0; i + 1 < lines.Count; i++)
                jd.AddRange(Dim(lines[i].Pos, lines[i + 1].Pos, at, horizontalDim, outward, GridDimension.Text(lines[i].Value, lines[i + 1].Value)));
            if (lines.Count >= 3)
            {
                double outer = at + outward * (DimOuter - DimInner);
                jd.AddRange(Dim(lines[0].Pos, lines[^1].Pos, outer, horizontalDim, outward, GridDimension.Text(lines[0].Value, lines[^1].Value)));
            }
        }

        /// <summary>
        /// 寸法 1 つ（寸法線・数字・両端の点）。horizontalDim＝X 方向の寸法（線は y＝at）。
        /// 数字は寸法線の外側（outward 方向）に置く。縦の寸法は 90° 回転（下から上へ読む。文字の上は −X 方向）。
        /// </summary>
        private static List<JwwData> Dim(double a, double b, double at, bool horizontalDim, int outward, string text)
        {
            var result = new List<JwwData>();
            var s = new JwwSunpou();
            s.m_Sen.m_nPenColor = DimPenColor;
            s.m_Sen.m_nPenStyle = 1;
            double textLen = text.Length * DimTextW;
            double mid = (a + b) / 2;
            if (horizontalDim)
            {
                s.m_Sen.m_start_x = a; s.m_Sen.m_start_y = at;
                s.m_Sen.m_end_x = b; s.m_Sen.m_end_y = at;
                s.m_Moji.m_start_x = mid - textLen / 2;
                s.m_Moji.m_start_y = outward > 0 ? at + DimTextGap : at - DimTextGap - DimTextH;
                s.m_Moji.m_degKakudo = 0;
            }
            else
            {
                s.m_Sen.m_start_x = at; s.m_Sen.m_start_y = a;
                s.m_Sen.m_end_x = at; s.m_Sen.m_end_y = b;
                // 90° 回転：基準線から −X 側に文字が立つ
                s.m_Moji.m_start_x = outward > 0 ? at + DimTextGap + DimTextH : at - DimTextGap;
                s.m_Moji.m_start_y = mid - textLen / 2;
                s.m_Moji.m_degKakudo = 90;
            }
            s.m_Moji.m_string = text;
            s.m_Moji.m_dSizeX = DimTextW;
            s.m_Moji.m_dSizeY = DimTextH;
            result.Add(s);

            foreach (var (x, y) in horizontalDim ? new[] { (a, at), (b, at) } : new[] { (at, a), (at, b) })
                result.Add(new JwwTen { m_start_x = x, m_start_y = y, m_nPenColor = DimTenColor, m_nPenStyle = 1 });
            return result;
        }
    }
}
