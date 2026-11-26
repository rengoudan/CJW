using System;
using System.Collections.Generic;
using System.Linq;

namespace JwBeam3D.Building
{
    /// <summary>
    /// 2026年11月27日 上下の階の図面原点のずれを自動で求める（上の階に加える平行移動）。
    /// 優先順：
    ///   1) 下の階の柱の位置 ↔ 上の階の叉線（下階柱の記号）を重ねる（2 点以上一致したとき）
    ///   2) 通り芯（垂直梁の中心 X・水平梁の中心 Y）を軸ごとに重ねる（2 本以上一致したとき）
    ///   3) 梁全体の外形の左下を揃える
    /// 同点の候補が複数あるときは、外形の左下を揃えた位置に近いものを選ぶ。
    /// </summary>
    public static class FloorAligner
    {
        public static FloorOffset Align(BuildingFloor lower, FloorOffset lowerOffset, BuildingFloor upper)
        {
            var bbox = BboxOffset(lower, lowerOffset, upper);

            // 1) 柱 ↔ 叉線
            var pillarPts = PillarPoints(lower).Select(p => (X: p.X + lowerOffset.Dx, Y: p.Y + lowerOffset.Dy)).ToList();
            if (pillarPts.Count > 0 && upper.DownMarks.Count > 0)
            {
                var r = MatchPoints(pillarPts, upper.DownMarks, BuildingDefaults.MatchTolerance, bbox);
                if (r.Score >= Math.Min(2, upper.DownMarks.Count))
                    return new FloorOffset { Dx = r.Dx, Dy = r.Dy, Method = $"柱と叉線 {r.Score}/{upper.DownMarks.Count} 箇所一致" };
            }

            // 2) 通り芯（軸ごと）
            var lx = GridLines(lower, vertical: true).Select(v => v + lowerOffset.Dx).ToList();
            var ux = GridLines(upper, vertical: true);
            var ly = GridLines(lower, vertical: false).Select(v => v + lowerOffset.Dy).ToList();
            var uy = GridLines(upper, vertical: false);
            var gx = Match1D(lx, ux, BuildingDefaults.GridTolerance, bbox.Dx);
            var gy = Match1D(ly, uy, BuildingDefaults.GridTolerance, bbox.Dy);
            bool okx = gx.Score >= 2, oky = gy.Score >= 2;
            if (okx || oky)
            {
                return new FloorOffset
                {
                    Dx = okx ? gx.D : bbox.Dx,
                    Dy = oky ? gy.D : bbox.Dy,
                    Method = $"通り芯 X {(okx ? gx.Score + " 本" : "外形")}・Y {(oky ? gy.Score + " 本" : "外形")}",
                };
            }

            // 3) 外形
            return new FloorOffset { Dx = bbox.Dx, Dy = bbox.Dy, Method = "梁の外形（左下）" };
        }

        /// <summary>柱の竖柱の中心（K 柱は 2 本＋中心）。叉線が柱ごと・竖柱ごとどちらで描かれても当たるようにする。</summary>
        public static IEnumerable<(double X, double Y)> PillarPoints(BuildingFloor f)
        {
            foreach (var p in f.Pillars)
            {
                yield return (p.AX, p.AY);
                if (p.IsK)
                {
                    yield return (p.BX, p.BY);
                    yield return (p.CX, p.CY);
                }
            }
        }

        /// <summary>通り芯：vertical＝垂直梁の中心 X、false＝水平梁の中心 Y。近い値はまとめる。</summary>
        public static List<double> GridLines(BuildingFloor f, bool vertical)
        {
            var vals = f.Beams.Where(b => b.Horizontal != vertical)
                              .Select(b => vertical ? (b.X0 + b.X1) / 2 : (b.Y0 + b.Y1) / 2)
                              .OrderBy(v => v).ToList();
            var merged = new List<double>();
            var group = new List<double>();
            foreach (var v in vals)
            {
                if (group.Count > 0 && v - group[^1] > BuildingDefaults.GridTolerance)
                {
                    merged.Add(group.Average());
                    group.Clear();
                }
                group.Add(v);
            }
            if (group.Count > 0) merged.Add(group.Average());
            return merged;
        }

        private static FloorOffset BboxOffset(BuildingFloor lower, FloorOffset lowerOffset, BuildingFloor upper)
        {
            var lb = Bbox(lower);
            var ub = Bbox(upper);
            if (lb == null || ub == null) return new FloorOffset { Dx = lowerOffset.Dx, Dy = lowerOffset.Dy };
            return new FloorOffset { Dx = lb.Value.MinX + lowerOffset.Dx - ub.Value.MinX, Dy = lb.Value.MinY + lowerOffset.Dy - ub.Value.MinY };
        }

        public static (double MinX, double MinY, double MaxX, double MaxY)? Bbox(BuildingFloor f)
        {
            if (f.Beams.Count == 0) return null;
            return (f.Beams.Min(b => Math.Min(b.X0, b.X1)), f.Beams.Min(b => Math.Min(b.Y0, b.Y1)),
                    f.Beams.Max(b => Math.Max(b.X0, b.X1)), f.Beams.Max(b => Math.Max(b.Y0, b.Y1)));
        }

        /// <summary>点群 target（上の階）を移動して fixedPts（下の階、全体座標）に重ねる移動量。</summary>
        private static (double Dx, double Dy, int Score) MatchPoints(
            List<(double X, double Y)> fixedPts, List<(double X, double Y)> target, double tol, FloorOffset prefer)
        {
            double tol2 = tol * tol;
            (double Dx, double Dy, int Score, double Dist) best = (prefer.Dx, prefer.Dy, 0, double.MaxValue);
            foreach (var f in fixedPts)
            {
                foreach (var t in target)
                {
                    double dx = f.X - t.X, dy = f.Y - t.Y;
                    int score = 0;
                    foreach (var q in target)
                    {
                        double qx = q.X + dx, qy = q.Y + dy;
                        foreach (var p in fixedPts)
                        {
                            double ex = p.X - qx, ey = p.Y - qy;
                            if (ex * ex + ey * ey <= tol2) { score++; break; }
                        }
                    }
                    double dist = (dx - prefer.Dx) * (dx - prefer.Dx) + (dy - prefer.Dy) * (dy - prefer.Dy);
                    if (score > best.Score || (score == best.Score && score > 0 && dist < best.Dist))
                        best = (dx, dy, score, dist);
                }
            }
            if (best.Score == 0) return (prefer.Dx, prefer.Dy, 0);

            // 一致した組のずれの平均で微調整
            double sx = 0, sy = 0; int n = 0;
            foreach (var q in target)
            {
                double qx = q.X + best.Dx, qy = q.Y + best.Dy;
                (double X, double Y)? near = null; double nd = tol2;
                foreach (var p in fixedPts)
                {
                    double ex = p.X - qx, ey = p.Y - qy, d = ex * ex + ey * ey;
                    if (d <= nd) { nd = d; near = p; }
                }
                if (near != null) { sx += near.Value.X - qx; sy += near.Value.Y - qy; n++; }
            }
            return n > 0 ? (best.Dx + sx / n, best.Dy + sy / n, best.Score) : (best.Dx, best.Dy, best.Score);
        }

        /// <summary>1 次元：target（上の階の通り芯）を d だけ動かして fixedVals に重ねる。</summary>
        private static (double D, int Score) Match1D(List<double> fixedVals, List<double> target, double tol, double prefer)
        {
            (double D, int Score, double Dist) best = (prefer, 0, double.MaxValue);
            foreach (var f in fixedVals)
            {
                foreach (var t in target)
                {
                    double d = f - t;
                    int score = target.Count(q => fixedVals.Any(p => Math.Abs(p - (q + d)) <= tol));
                    double dist = Math.Abs(d - prefer);
                    if (score > best.Score || (score == best.Score && score > 0 && dist < best.Dist))
                        best = (d, score, dist);
                }
            }
            if (best.Score == 0) return (prefer, 0);
            var res = target.Select(q => fixedVals.Select(p => p - (q + best.D)).Where(e => Math.Abs(e) <= tol).DefaultIfEmpty(double.NaN).OrderBy(e => Math.Abs(e)).First())
                            .Where(e => !double.IsNaN(e)).ToList();
            return (best.D + (res.Count > 0 ? res.Average() : 0), best.Score);
        }
    }
}
