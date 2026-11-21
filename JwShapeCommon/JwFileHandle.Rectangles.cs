using NetTopologySuite.Geometries;
using NetTopologySuite.Index.Strtree;

namespace JwShapeCommon
{
    /// <summary>
    /// 2026年11月21日 FindRectangles 的性能重写（结果与原实现等价）。
    ///
    /// 原实现的问题：
    ///   1) 三重循环中每一步都重新做 R 树查询、重新生成 LineString 并计算 Intersects，同一线段的邻居被反复计算；
    ///   2) 每找到一个候选矩形，都在锁内用 AreRectanglesEqual 与全部已找到矩形逐个比较（O(R²)，
    ///      且 JwXianComparint 的哈希是常量，Except 本身也退化为两两比较）；
    ///   3) 线段相等判断（seg.Equals）每次都分配列表并执行 Except。
    ///   梁线较多且相互交叉时（如梁色与长网格线同色）会呈组合爆炸，表现为"卡死"。
    ///
    /// 等价性说明：
    ///   - 相交关系：与原实现完全相同的判定（R 树包络查询 + LineString.Intersects），只是每条线段只算一次并缓存。
    ///   - 跳过条件：XianEquals(a, b) 精确复刻 a.Equals(b)（即 a 的端点集合 ⊆ b 的端点集合，端点按 JwPointComparint 比较）。
    ///   - 去重：
    ///       · 4 个顶点互不相同的矩形（非退化）：可证明原 AreRectanglesEqual 等价于"4 条边的集合相等"，
    ///         且非退化矩形不可能与退化矩形相等，因此用边集合的精确键做哈希判重。
    ///       · 有重复顶点的矩形（退化，H=0）：仍用原 AreRectanglesEqual 精确比较，只是先按"至少共享一个顶点"
    ///         （相等的必要条件）筛选候选，避免与全部矩形比较。
    ///     注：原实现对退化矩形的去重结果本身随并行顺序而变化（同一图纸多次运行 434/435/437 个），
    ///     非退化矩形则每次一致；本实现对非退化矩形与原实现逐一相同。
    /// </summary>
    public partial class JwFileHandle
    {
        private void FindRectangles(List<JwXian> segments)
        {
            Mians = new List<JWMian>();
            int n = segments.Count;

            // R 树与 LineString 只构建一次
            var rtree = new STRtree<JwXian>();
            var lines = new LineString[n];
            var indexOf = new Dictionary<JwXian, int>(ReferenceEqualityComparer.Instance);
            for (int i = 0; i < n; i++)
            {
                lines[i] = segments[i].ToLineString();
                rtree.Insert(lines[i].EnvelopeInternal, segments[i]);
                indexOf.TryAdd(segments[i], i);
            }
            rtree.Build(); // 并行查询前先构建，避免惰性构建的并发问题

            // 相交邻接表：判定与原实现相同（包络查询 + Intersects，结果包含自身）
            var adj = new JwXian[n][];
            Parallel.For(0, n, i =>
            {
                adj[i] = rtree.Query(lines[i].EnvelopeInternal)
                              .Where(seg => lines[indexOf[seg]].Intersects(lines[i]))
                              .ToArray();
            });

            var sync = new object();
            var nonDegenerateKeys = new HashSet<string>(StringComparer.Ordinal);
            var byVertex = new Dictionary<(double, double), List<List<JwXian>>>();

            Parallel.For(0, n, i =>
            {
                var seg1 = segments[i];
                foreach (var seg2 in adj[i])
                {
                    if (XianEquals(seg2, seg1)) continue; // 跳过自身
                    foreach (var seg3 in adj[indexOf[seg2]])
                    {
                        if (XianEquals(seg3, seg1) || XianEquals(seg3, seg2)) continue;
                        foreach (var seg4 in adj[indexOf[seg3]])
                        {
                            if (XianEquals(seg4, seg1) || XianEquals(seg4, seg2) || XianEquals(seg4, seg3)) continue;
                            if (!IsRectangle(seg1, seg2, seg3, seg4, out var points)) continue;

                            var s1 = new JwXian(points[0], points[1]);
                            var s2 = new JwXian(points[1], points[2]);
                            var s3 = new JwXian(points[2], points[3]);
                            var s4 = new JwXian(points[3], points[0]);
                            var rectangle = new List<JwXian> { s1, s2, s3, s4 };
                            bool degenerate = HasRepeatedVertex(points);
                            string? key = degenerate ? null : EdgeSetKey(points);

                            lock (sync)
                            {
                                bool exists = degenerate
                                    ? CandidatesSharingVertex(byVertex, points).Any(r => AreRectanglesEqual(r, rectangle))
                                    : nonDegenerateKeys.Contains(key!);
                                if (exists) continue;

                                // 以下与原实现相同
                                JWMian m = new JWMian();
                                var storlst = rectangle.OrderByDescending(t => t.Distance());
                                var maxd = rectangle.OrderByDescending(t => t.Distance()).First();
                                var mind = storlst.Last();
                                m.Width = maxd.Distance();
                                m.Length = m.Width;
                                m.Height = mind.Distance();
                                bool isbiancha = false;
                                m.Jiaodu = maxd.Jiaodu(out isbiancha);
                                m.IsJiaoduPiancha = isbiancha;
                                m.Xians = new List<JwXian>(rectangle);
                                m.Points = points;
                                Mians.Add(m);

                                if (!degenerate) nonDegenerateKeys.Add(key!);
                                foreach (var v in points.Select(VertexKey).Distinct())
                                {
                                    if (!byVertex.TryGetValue(v, out var list)) byVertex[v] = list = new List<List<JwXian>>();
                                    list.Add(rectangle);
                                }
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 精确复刻 a.Equals(b)：a 的两个端点都能在 b 的端点中找到（按 JwPointComparint：字符串相同且坐标 ==）。
        /// </summary>
        private static bool XianEquals(JwXian a, JwXian b)
        {
            if (ReferenceEquals(a, b)) return true;
            return PointIn(a.Pone, b) && PointIn(a.Ptwo, b);

            static bool PointIn(JWPoint p, JwXian x) => SamePoint(p, x.Pone) || SamePoint(p, x.Ptwo);
        }

        /// <summary>
        /// 与 JwPointComparint 一致：X、Y 用 == 比较，且 GetPointString 相同（仅 ±0 会使两者不一致）。
        /// </summary>
        private static bool SamePoint(JWPoint p, JWPoint q) => SameCoord(p.X, q.X) && SameCoord(p.Y, q.Y);

        private static bool SameCoord(double a, double b) =>
            a == b && (a != 0 || double.IsNegative(a) == double.IsNegative(b));

        /// <summary>任意两个顶点坐标相等（== 判定，偏保守：疑似退化即走精确比较路径）。</summary>
        private static bool HasRepeatedVertex(List<JWPoint> pts)
        {
            for (int i = 0; i < pts.Count; i++)
                for (int j = i + 1; j < pts.Count; j++)
                    if (pts[i].X == pts[j].X && pts[i].Y == pts[j].Y) return true;
            return false;
        }

        /// <summary>
        /// 非退化矩形的边集合键：每条边两端点按位排序，4 条边再排序。按位比较以区分 ±0（与 SamePoint 一致）。
        /// </summary>
        private static string EdgeSetKey(List<JWPoint> pts)
        {
            string P(JWPoint p) => BitConverter.DoubleToInt64Bits(p.X).ToString("X16") + BitConverter.DoubleToInt64Bits(p.Y).ToString("X16");
            var edges = new string[4];
            for (int i = 0; i < 4; i++)
            {
                string a = P(pts[i]), b = P(pts[(i + 1) % 4]);
                edges[i] = string.CompareOrdinal(a, b) <= 0 ? a + b : b + a;
            }
            Array.Sort(edges, StringComparer.Ordinal);
            return string.Concat(edges);
        }

        /// <summary>顶点索引键：±0 归一，作为候选筛选（超集即可，最终由 AreRectanglesEqual 精确判定）。</summary>
        private static (double, double) VertexKey(JWPoint p) => (p.X + 0.0, p.Y + 0.0);

        private static IEnumerable<List<JwXian>> CandidatesSharingVertex(
            Dictionary<(double, double), List<List<JwXian>>> byVertex, List<JWPoint> pts)
        {
            var seen = new HashSet<List<JwXian>>(ReferenceEqualityComparer.Instance);
            foreach (var v in pts.Select(VertexKey).Distinct())
                if (byVertex.TryGetValue(v, out var list))
                    foreach (var r in list)
                        if (seen.Add(r)) yield return r;
        }
    }
}
