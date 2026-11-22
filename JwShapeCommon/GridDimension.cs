using System.Globalization;

namespace JwShapeCommon
{
    /// <summary>
    /// 2026年11月23日 通り芯（梁の中心線）の寸法の共通処理。
    /// 水平梁は Center＝Y、垂直梁は Center＝X（JwSquareBase.Center：幅の狭い側の中央）。
    /// 同じ中心線上に分かれた梁（切断梁など）は 1 本の通り芯として扱う。
    /// </summary>
    public static class GridDimension
    {
        /// <summary>1 本の通り芯：Value＝図面座標（Center）、Pos＝描く位置（jww では Value と同じ、画面では画面座標）。</summary>
        public readonly record struct Line(double Value, double Pos);

        /// <summary>中心線をまとめて並べる（tol 以内は同じ線。並びは描く位置の順）。</summary>
        public static List<Line> Merge(IEnumerable<(double Value, double Pos)> items, double tol)
        {
            var result = new List<(double Value, double PosSum, int Count)>();
            foreach (var (v, p) in items.OrderBy(i => i.Value))
            {
                if (result.Count > 0 && Math.Abs(result[^1].Value - v) <= tol)
                {
                    var last = result[^1];
                    result[^1] = (last.Value, last.PosSum + p, last.Count + 1);
                }
                else result.Add((v, p, 1));
            }
            return result.Select(r => new Line(r.Value, r.PosSum / r.Count)).OrderBy(l => l.Pos).ToList();
        }

        /// <summary>2 本の通り芯の間隔（実寸 mm、整数、3 桁区切り）。</summary>
        public static string Text(double value1, double value2) =>
            Math.Round(Math.Abs(value2 - value1) * JwFileConsts.JwScale, 0).ToString("#,0", CultureInfo.InvariantCulture);

        /// <summary>同じ線とみなす許容差（図面座標。実寸 1mm）。</summary>
        public static double Tolerance => 1.0 / (JwFileConsts.JwScale > 0 ? JwFileConsts.JwScale : 1);
    }
}
