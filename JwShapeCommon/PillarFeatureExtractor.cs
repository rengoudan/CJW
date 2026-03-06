using JwShapeCommon;
using NetTopologySuite.Geometries;

using NetTopologySuite.Noding.Snapround;
using NetTopologySuite.Operation.Linemerge;

using NetTopologySuite.Operation.Polygonize;
using NetTopologySuite.LinearReferencing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JwShapeCommon
{
    public class PillarFeatureExtractor
    {
        private readonly GeometryFactory _gf;
        private readonly double _squareSideLength;
        private readonly int _nodeRoundDigits;
        private readonly double _minSegmentLength;

        public PillarFeatureExtractor(double squareSideLength, GeometryFactory geometryFactory = null, int nodeRoundDigits = 6, double minSegmentLength = 1e-6)
        {
            _squareSideLength = squareSideLength;
            _gf = geometryFactory ?? new GeometryFactory();
            _nodeRoundDigits = nodeRoundDigits;
            _minSegmentLength = minSegmentLength;
        }

        public List<PillarFeature> Extract(List<JwXian> inputSegments, bool debug = false)
        {
            if (inputSegments == null || inputSegments.Count == 0) return new List<PillarFeature>();

            var clean = PreprocessSegments(inputSegments, debug);
            if (clean.Count == 0) return new List<PillarFeature>();

            var mls = BuildMLS(clean);

            var (polygons, remainingLines, mergedLines) = PolygonizeAndMergeLines(mls, debug);

            var squares = FindSquaresFromEdges(mergedLines, debug);

            var pillars = GroupSquaresByLongLines(squares, mergedLines, remainingLines, debug);

            return pillars;
        }

        // ------------------ 预处理 ------------------
        private List<JwXian> PreprocessSegments(List<JwXian> segments, bool debug = false)
        {
            var unique = segments
                .Distinct(new JwXianComparint())
                .Where(x => x != null)
                .ToList();

            // 简单合并重叠段（如果你的 JwXian 支持 Merge/OverlapsWith）
            bool mergedAny = true;
            while (mergedAny)
            {
                mergedAny = false;
                for (int i = 0; i < unique.Count; i++)
                {
                    for (int j = i + 1; j < unique.Count; j++)
                    {
                        var a = unique[i];
                        var b = unique[j];
                        if (a == null || b == null) continue;
                        try
                        {
                            if (a.OverlapsWith(b))
                            {
                                var merged = a.Merge(b);
                                unique.RemoveAt(j);
                                unique.RemoveAt(i);
                                unique.Add(merged);
                                mergedAny = true;
                                break;
                            }
                        }
                        catch
                        {
                            // 忽略无法合并的情况
                        }
                    }
                    if (mergedAny) break;
                }
            }

            unique = unique.Where(x => x != null && x.Distance() > _minSegmentLength).ToList();
            return unique;
        }

        // ------------------ 构建 MultiLineString ------------------
        private MultiLineString BuildMLS(List<JwXian> segments)
        {
            var lines = segments.Select(s => s.ToLineString()).Where(l => l != null).ToArray();
            return _gf.CreateMultiLineString(lines);
        }

        // ------------------ Polygonize + LineMerge ------------------
        private (List<Polygon> polygons, Geometry remainingLines, List<LineString> mergedLines) PolygonizeAndMergeLines(MultiLineString mls, bool debug = false)
        {
            var lineGeoms = new List<LineString>();
            for (int i = 0; i < mls.NumGeometries; i++)
            {
                var g = mls.GetGeometryN(i);
                if (g is LineString ls) lineGeoms.Add(ls);
                else if (g is MultiLineString m)
                {
                    for (int k = 0; k < m.NumGeometries; k++)
                        if (m.GetGeometryN(k) is LineString sub) lineGeoms.Add(sub);
                }
            }

            // 合并连续段
            var merger = new LineMerger();
            merger.Add(lineGeoms.Cast<Geometry>().ToList());
            var merged = merger.GetMergedLineStrings().Cast<LineString>().ToList();

            // polygonize
            var polygonizer = new Polygonizer();
            foreach (var ls in merged) polygonizer.Add(ls);

            var polysObj = polygonizer.GetPolygons();
            var polygons = new List<Polygon>();
            foreach (var p in polysObj) if (p is Polygon poly) polygons.Add(poly);

            // 剩余边（dangles/cutEdges/invalidRingLines + merged 中不属于 polygon 边的）
            var remainingList = new List<LineString>();
            foreach (var obj in polygonizer.GetDangles()) if (obj is LineString l) remainingList.Add(l);
            foreach (var obj in polygonizer.GetCutEdges()) if (obj is LineString l) remainingList.Add(l);
            foreach (var obj in polygonizer.GetInvalidRingLines()) if (obj is LineString l) remainingList.Add(l);

            // merged 中不完全属于 polygon 边的也算 remaining
            var polyEdges = new HashSet<string>();
            foreach (var poly in polygons)
            {
                var ring = poly.ExteriorRing;
                for (int i = 0; i < ring.NumPoints - 1; i++)
                {
                    var c1 = ring.GetCoordinateN(i);
                    var c2 = ring.GetCoordinateN(i + 1);
                    polyEdges.Add(EdgeKey(c1, c2));
                }
            }

            foreach (var ml in merged)
            {
                var coords = ml.Coordinates;
                bool allInPolys = true;
                for (int i = 0; i < coords.Length - 1; i++)
                {
                    if (!polyEdges.Contains(EdgeKey(coords[i], coords[i + 1])))
                    {
                        allInPolys = false;
                        break;
                    }
                }
                if (!allInPolys) remainingList.Add(ml);
            }

            Geometry remaining;
            if (remainingList.Count == 0) remaining = _gf.CreateGeometryCollection(null);
            else if (remainingList.Count == 1) remaining = remainingList[0];
            else remaining = _gf.CreateMultiLineString(remainingList.ToArray());

            return (polygons, remaining, merged);
        }

        private string EdgeKey(Coordinate a, Coordinate b)
        {
            if (a == null || b == null) return string.Empty;
            if (a.X < b.X || (a.X == b.X && a.Y <= b.Y))
                return $"{Math.Round(a.X, _nodeRoundDigits):F6},{Math.Round(a.Y, _nodeRoundDigits):F6}-{Math.Round(b.X, _nodeRoundDigits):F6},{Math.Round(b.Y, _nodeRoundDigits):F6}";
            else
                return $"{Math.Round(b.X, _nodeRoundDigits):F6},{Math.Round(b.Y, _nodeRoundDigits):F6}-{Math.Round(a.X, _nodeRoundDigits):F6},{Math.Round(a.Y, _nodeRoundDigits):F6}";
        }

        // ------------------ 从边集合中寻找正方形 ------------------
        private List<Polygon> FindSquaresFromEdges(List<LineString> mergedLines, bool debug = false)
        {
            var squares = new List<Polygon>();
            if (mergedLines == null || mergedLines.Count == 0) return squares;

            // 先检测 mergedLines 中本身就是闭合环的多边形（CSV 情况）
            foreach (var ls in mergedLines)
            {
                if (ls == null) continue;
                bool isClosed = ls.IsClosed || (ls.NumPoints >= 2 && ls.GetCoordinateN(0).Equals2D(ls.GetCoordinateN(ls.NumPoints - 1)));
                if (!isClosed) continue;
                if (ls.NumPoints == 5)
                {
                    try
                    {
                        var ring = new LinearRing(ls.Coordinates);
                        if (!ring.IsValid) continue;
                        var poly = _gf.CreatePolygon(ring);
                        if (IsSquareCandidate(poly)) squares.Add(poly);
                    }
                    catch { }
                }
            }

            // 基于图遍历寻找 4-cycle（保底）
            Func<Coordinate, string> coordKey = c => $"{Math.Round(c.X, _nodeRoundDigits):F6},{Math.Round(c.Y, _nodeRoundDigits):F6}";
            var adj = new Dictionary<string, HashSet<string>>();
            var coordMap = new Dictionary<string, Coordinate>();

            void AddEdge(Coordinate a, Coordinate b)
            {
                var ka = coordKey(a);
                var kb = coordKey(b);
                if (!coordMap.ContainsKey(ka)) coordMap[ka] = a;
                if (!coordMap.ContainsKey(kb)) coordMap[kb] = b;
                if (!adj.ContainsKey(ka)) adj[ka] = new HashSet<string>();
                if (!adj.ContainsKey(kb)) adj[kb] = new HashSet<string>();
                adj[ka].Add(kb);
                adj[kb].Add(ka);
            }

            foreach (var ls in mergedLines)
            {
                if (ls == null) continue;
                var coords = ls.Coordinates;
                for (int i = 0; i < coords.Length - 1; i++) AddEdge(coords[i], coords[i + 1]);
            }

            var cycles = new HashSet<string>();
            foreach (var a in adj.Keys)
            {
                foreach (var b in adj[a])
                {
                    foreach (var c in adj[b].Where(x => x != a))
                    {
                        foreach (var d in adj[c].Where(x => x != b && x != a))
                        {
                            if (adj[d].Contains(a))
                            {
                                var seq = new[] { a, b, c, d };
                                var norm = NormalizeCycle(seq);
                                if (!cycles.Contains(norm))
                                {
                                    cycles.Add(norm);
                                    var coordsList = seq.Select(k => coordMap[k]).ToList();
                                    coordsList.Add(coordMap[a]);
                                    try
                                    {
                                        var ring = new LinearRing(coordsList.ToArray());
                                        if (!ring.IsValid) continue;
                                        var poly = _gf.CreatePolygon(ring);
                                        if (IsSquareCandidate(poly))
                                        {
                                            bool dup = squares.Any(s => s.Centroid.Coordinate.Equals2D(poly.Centroid.Coordinate));
                                            if (!dup) squares.Add(poly);
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
            }

            return squares;
        }

        private string NormalizeCycle(string[] seq)
        {
            var rotations = new List<string>();
            for (int r = 0; r < seq.Length; r++)
            {
                var rot = seq.Skip(r).Concat(seq.Take(r)).ToArray();
                rotations.Add(string.Join(";", rot));
            }
            var rev = seq.Reverse().ToArray();
            for (int r = 0; r < seq.Length; r++)
            {
                var rot = rev.Skip(r).Concat(rev.Take(r)).ToArray();
                rotations.Add(string.Join(";", rot));
            }
            rotations.Sort(StringComparer.Ordinal);
            return rotations.First();
        }

        private bool IsSquareCandidate(Polygon poly)
        {
            if (poly == null) return false;
            var ring = poly.ExteriorRing;
            if (ring == null) return false;
            var coords = ring.Coordinates;
            if (coords.Length < 5) return false;
            var verts = coords.Take(coords.Length - 1).ToArray();
            if (verts.Length != 4) return false;

            double[] lens = new double[4];
            for (int i = 0; i < 4; i++) lens[i] = verts[i].Distance(verts[(i + 1) % 4]);
            double avg = lens.Average();
            if (avg <= 1e-12) return false;

            // 宽松长度容差
            double relTol = 0.15;
            if (!lens.All(l => Math.Abs(l - avg) / avg <= relTol)) return false;

            // 角度接近 90°
            for (int i = 0; i < 4; i++)
            {
                var p = verts[i];
                var prev = verts[(i + 3) % 4];
                var next = verts[(i + 1) % 4];
                var v1x = prev.X - p.X; var v1y = prev.Y - p.Y;
                var v2x = next.X - p.X; var v2y = next.Y - p.Y;
                var dot = v1x * v2x + v1y * v2y;
                var m1 = Math.Sqrt(v1x * v1x + v1y * v1y);
                var m2 = Math.Sqrt(v2x * v2x + v2y * v2y);
                if (m1 < 1e-12 || m2 < 1e-12) return false;
                var cos = dot / (m1 * m2);
                cos = Math.Max(-1.0, Math.Min(1.0, cos));
                var ang = Math.Acos(cos) * 180.0 / Math.PI;
                if (Math.Abs(ang - 90.0) > 20.0) return false;
            }

            return true;
        }

        // ------------------ 简化的分组逻辑（仅两种情况） ------------------
        private List<PillarFeature> GroupSquaresByLongLines(List<Polygon> squares, List<LineString> mergedLines, Geometry remainingLines, bool debug = false)
        {
            var result = new List<PillarFeature>();
            if (squares == null || squares.Count == 0) return result;
            if (mergedLines == null) mergedLines = new List<LineString>();

            // 预计算每个正方的边线集合与边线段（方便判定点是否落在边上）
            var squareInfos = squares.Select(p => new
            {
                Poly = p,
                Center = p.Centroid.Coordinate,
                Envelope = p.EnvelopeInternal,
                Edges = GetPolygonEdges(p)
            }).ToList();

            // 遍历两两组合
            for (int i = 0; i < squareInfos.Count; i++)
            {
                for (int j = i + 1; j < squareInfos.Count; j++)
                {
                    var s1 = squareInfos[i];
                    var s2 = squareInfos[j];

                    // 忽略重叠或距离过近的（避免重复）
                    if (s1.Poly.Centroid.Coordinate.Equals2D(s2.Poly.Centroid.Coordinate)) continue;

                    // 1) 检查是否存在两条平行短线，且两条线的端点分别落在两个正方的边上
                    var candidateShortLines = new List<LineString>();
                    foreach (var ls in mergedLines)
                    {
                        if (ls == null) continue;
                       

                        var coords = ls.Coordinates;
                        
                        var a = coords.First();
                        var b = coords.Last();

                        // 端点分别落在两个正方的边上（任意一对）
                        bool aOnS1 = PointOnAnyEdge(a, s1.Edges);
                        bool bOnS2 = PointOnAnyEdge(b, s2.Edges);
                        bool aOnS2 = PointOnAnyEdge(a, s2.Edges);
                        bool bOnS1 = PointOnAnyEdge(b, s1.Edges);

                        if ((aOnS1 && bOnS2) || (aOnS2 && bOnS1))
                        {
                            candidateShortLines.Add(ls);
                        }
                    }

                    // 需要两条平行线（方向相近）
                    if (candidateShortLines.Count >= 2)
                    {
                        // 找到两条方向接近且分别连接两正方的线
                        bool foundPair = false;
                        for (int a = 0; a < candidateShortLines.Count && !foundPair; a++)
                        {
                            for (int b = a + 1; b < candidateShortLines.Count && !foundPair; b++)
                            {
                                var l1 = candidateShortLines[a];
                                var l2 = candidateShortLines[b];
                                if (AreLinesParallel(l1, l2, angleToleranceDeg: 15.0))
                                {
                                    // 确保两条线分别连接两个正方（端点分布）
                                    if (ConnectsTwoSquaresByEndpoints(l1, l2, s1.Edges, s2.Edges))
                                    {
                                        var pf = new PillarFeature
                                        {
                                            Center1 = s1.Center,
                                            Center2 = s2.Center,
                                            SquareSideLength = _squareSideLength
                                        };
                                        pf.ComputeAngleAndOrientation();
                                        result.Add(pf);
                                        foundPair = true;
                                    }
                                }
                            }
                        }
                        if (foundPair) continue;
                    }

                    // 2) 检查中间为矩形：在两个正方之间存在一个矩形（或长条多边形），且矩形两端边被包含在两个正方的边上
                    // 我们在 mergedLines + remainingLines 中查找可能的矩形（多段合并成闭合环或长矩形）
                    var candidateRects = new List<Polygon>();

                    // 从 polygons（如果有）寻找位于两正方中心连线中间且与两正方边对齐的矩形
                    // 这里使用 mergedLines 构造闭合环的简单检测：若某条 mergedLine 是闭合或点数较多则视为候选
                    foreach (var ls in mergedLines)
                    {
                        if (ls == null) continue;
                        if (ls.IsClosed || ls.NumPoints >= 4)
                        {
                            try
                            {
                                var ring = new LinearRing(ls.Coordinates);
                                if (!ring.IsValid) continue;
                                var poly = _gf.CreatePolygon(ring);
                                if (poly == null) continue;
                                // 判断 poly 是否位于 s1 与 s2 的中间（包络相交且中心在线段附近）
                                if (IsBetweenSquares(poly, s1.Poly, s2.Poly))
                                {
                                    candidateRects.Add(poly);
                                }
                            }
                            catch { }
                        }
                    }

                    // 也检查 remainingLines（如果是 MultiLineString）
                    if (remainingLines != null && !remainingLines.IsEmpty)
                    {
                        var remLines = new List<LineString>();
                        if (remainingLines is LineString rl) remLines.Add(rl);
                        else if (remainingLines is MultiLineString mls)
                        {
                            for (int k = 0; k < mls.NumGeometries; k++)
                                if (mls.GetGeometryN(k) is LineString sub) remLines.Add(sub);
                        }

                        // 简单尝试：若多条 remainingLines 可以组成一个矩形（端点在两正方边上），则接受
                        // 这里用非常简单的启发式：寻找四条边的集合，其端点落在两个正方的边上或在两正方之间
                        var grouped = GroupLinesToRectangleCandidates(remLines, s1.Poly, s2.Poly);
                        candidateRects.AddRange(grouped);
                    }

                    // 对候选矩形进一步判定：矩形的两端边被包含在两个正方的边上
                    foreach (var rect in candidateRects)
                    {
                        if (RectEndsContainedInSquares(rect, s1.Poly, s2.Poly))
                        {
                            var pf = new PillarFeature
                            {
                                Center1 = s1.Center,
                                Center2 = s2.Center,
                                SquareSideLength = _squareSideLength
                            };
                            pf.ComputeAngleAndOrientation();
                            result.Add(pf);
                            break;
                        }
                    }
                }
            }

            return result;
        }

        // ------------------ 辅助方法 ------------------

        // 将多边形的每条边作为 LineString 返回
        private List<LineString> GetPolygonEdges(Polygon poly)
        {
            var edges = new List<LineString>();
            if (poly == null) return edges;
            var ring = poly.ExteriorRing;
            var coords = ring.Coordinates;
            for (int i = 0; i < coords.Length - 1; i++)
            {
                var a = coords[i];
                var b = coords[i + 1];
                edges.Add(_gf.CreateLineString(new[] { a, b }));
            }
            return edges;
        }

        // 判定点是否落在任意边上（允许小容差）
        private bool PointOnAnyEdge(Coordinate pt, List<LineString> edges, double tol = 1e-6)
        {
            if (pt == null || edges == null) return false;
            var p = _gf.CreatePoint(pt);
            foreach (var e in edges)
            {
                if (e == null) continue;
                if (e.Distance(p) <= tol) return true;
            }
            return false;
        }

        // 判定两条线是否平行（角度接近）
        private bool AreLinesParallel(LineString l1, LineString l2, double angleToleranceDeg = 10.0)
        {
            if (l1 == null || l2 == null) return false;
            var a1 = l1.GetCoordinateN(0);
            var b1 = l1.GetCoordinateN(l1.NumPoints - 1);
            var a2 = l2.GetCoordinateN(0);
            var b2 = l2.GetCoordinateN(l2.NumPoints - 1);

            var dx1 = b1.X - a1.X; var dy1 = b1.Y - a1.Y;
            var dx2 = b2.X - a2.X; var dy2 = b2.Y - a2.Y;

            var ang1 = Math.Atan2(dy1, dx1) * 180.0 / Math.PI;
            var ang2 = Math.Atan2(dy2, dx2) * 180.0 / Math.PI;

            double diff = Math.Abs(NormalizeAngleDeg(ang1 - ang2));
            if (diff > 90) diff = 180 - diff;
            return diff <= angleToleranceDeg;
        }

        private double NormalizeAngleDeg(double a)
        {
            while (a <= -180) a += 360;
            while (a > 180) a -= 360;
            return a;
        }

        // 判定两条线的端点是否分别连接两个正方（任意组合）
        private bool ConnectsTwoSquaresByEndpoints(LineString l1, LineString l2, List<LineString> edges1, List<LineString> edges2, double tol = 1e-6)
        {
            var ends = new List<Coordinate> {
                l1.GetCoordinateN(0), l1.GetCoordinateN(l1.NumPoints-1),
                l2.GetCoordinateN(0), l2.GetCoordinateN(l2.NumPoints-1)
            };

            // 需要至少一条线的一个端点在 s1，另一个端点在 s2；另一条线同理（或交叉）
            int countS1 = ends.Count(c => PointOnAnyEdge(c, edges1, tol));
            int countS2 = ends.Count(c => PointOnAnyEdge(c, edges2, tol));
            return countS1 >= 1 && countS2 >= 1;
        }

        // 判定多边形是否位于两正方之间（简单包络/中心判断）
        private bool IsBetweenSquares(Polygon candidate, Polygon s1, Polygon s2)
        {
            if (candidate == null || s1 == null || s2 == null) return false;
            var env = candidate.EnvelopeInternal;
            var env1 = s1.EnvelopeInternal;
            var env2 = s2.EnvelopeInternal;

            // 中心点应在两正方中心连线附近
            var c1 = s1.Centroid.Coordinate;
            var c2 = s2.Centroid.Coordinate;
            var midX = (c1.X + c2.X) / 2.0;
            var midY = (c1.Y + c2.Y) / 2.0;
            var cc = candidate.Centroid.Coordinate;
            double dx = cc.X - midX;
            double dy = cc.Y - midY;
            double dist2 = dx * dx + dy * dy;
            double maxDist = Math.Max(s1.EnvelopeInternal.Width, s1.EnvelopeInternal.Height) + Math.Max(s2.EnvelopeInternal.Width, s2.EnvelopeInternal.Height);
            return dist2 <= (maxDist * maxDist * 0.6);
        }

        // 将若干条线尝试组合成矩形候选（非常启发式）
        private List<Polygon> GroupLinesToRectangleCandidates(List<LineString> lines, Polygon s1, Polygon s2)
        {
            var res = new List<Polygon>();
            if (lines == null || lines.Count == 0) return res;

            // 简单策略：寻找能组成闭合环的线集合（端点匹配）
            // 构造端点字典
            var dict = new Dictionary<string, List<LineString>>();
            Func<Coordinate, string> key = c => $"{Math.Round(c.X, 6):F6},{Math.Round(c.Y, 6):F6}";
            foreach (var l in lines)
            {
                var a = key(l.GetCoordinateN(0));
                var b = key(l.GetCoordinateN(l.NumPoints - 1));
                if (!dict.ContainsKey(a)) dict[a] = new List<LineString>();
                if (!dict.ContainsKey(b)) dict[b] = new List<LineString>();
                dict[a].Add(l);
                dict[b].Add(l);
            }

            // 朴素尝试：任取一条线，沿端点追踪最多 4 条边形成闭合
            foreach (var start in lines)
            {
                var path = new List<Coordinate> { start.GetCoordinateN(0), start.GetCoordinateN(start.NumPoints - 1) };
                var used = new HashSet<LineString> { start };
                TryExtend(path, used, lines, res, s1, s2);
            }

            return res;
        }

        private void TryExtend(List<Coordinate> path, HashSet<LineString> used, List<LineString> pool, List<Polygon> outPolys, Polygon s1, Polygon s2)
        {
            if (path.Count > 10) return;
            var last = path.Last();
            // 找与 last 相连且未使用的线
            foreach (var l in pool)
            {
                if (used.Contains(l)) continue;
                var a = l.GetCoordinateN(0);
                var b = l.GetCoordinateN(l.NumPoints - 1);
                Coordinate next = null;
                if (a.Equals2D(last)) next = b;
                else if (b.Equals2D(last)) next = a;
                if (next == null) continue;

                var newPath = new List<Coordinate>(path) { next };
                var newUsed = new HashSet<LineString>(used) { l };

                // 若闭合（next == path[0]）且点数 >= 4，尝试构造 polygon
                if (next.Equals2D(path[0]) && newPath.Count >= 4)
                {
                    try
                    {
                        var coords = newPath.ToArray();
                        // 确保首尾重复
                        if (!coords[0].Equals2D(coords[coords.Length - 1]))
                        {
                            var arr = new Coordinate[coords.Length + 1];
                            Array.Copy(coords, arr, coords.Length);
                            arr[arr.Length - 1] = coords[0];
                            coords = arr;
                        }
                        var ring = new LinearRing(coords);
                        if (!ring.IsValid) continue;
                        var poly = _gf.CreatePolygon(ring);
                        if (poly != null && poly.Area > 1e-8)
                        {
                            // 仅接受矩形形状（4 边）或近似矩形
                            if (poly.NumPoints >= 5)
                            {
                                outPolys.Add(poly);
                                return;
                            }
                        }
                    }
                    catch { }
                }
                else
                {
                    TryExtend(newPath, newUsed, pool, outPolys, s1, s2);
                }
            }
        }

        // 判定矩形两端边是否被包含在两个正方的边上（任意顺序）
        private bool RectEndsContainedInSquares(Polygon rect, Polygon s1, Polygon s2, double tol = 1e-6)
        {
            if (rect == null || s1 == null || s2 == null) return false;
            var rectEdges = GetPolygonEdges(rect);
            // 取 rect 的两端边：按与两正方中心连线方向最接近的两条边
            var c1 = s1.Centroid.Coordinate;
            var c2 = s2.Centroid.Coordinate;
            var dirX = c2.X - c1.X; var dirY = c2.Y - c1.Y;
            var dirAng = Math.Atan2(dirY, dirX) * 180.0 / Math.PI;

            // 计算每条 rect 边与方向的夹角，选择最接近方向的两条作为端边
            var edgeAngles = rectEdges.Select(e =>
            {
                var a = e.GetCoordinateN(0);
                var b = e.GetCoordinateN(e.NumPoints - 1);
                var ang = Math.Atan2(b.Y - a.Y, b.X - a.X) * 180.0 / Math.PI;
                var diff = Math.Abs(NormalizeAngleDeg(Math.Abs(ang - dirAng)));
                return new { Edge = e, AngleDiff = diff };
            }).OrderBy(x => x.AngleDiff).ToList();

            if (edgeAngles.Count < 2) return false;
            var end1 = edgeAngles[0].Edge;
            var end2 = edgeAngles[1].Edge;

            // 端边必须分别被包含在两个正方的某条边上（端边的两个端点都落在对应正方的边上）
            var e1a = end1.GetCoordinateN(0);
            var e1b = end1.GetCoordinateN(end1.NumPoints - 1);
            var e2a = end2.GetCoordinateN(0);
            var e2b = end2.GetCoordinateN(end2.NumPoints - 1);

            bool e1OnS1 = PointOnAnyEdge(e1a, GetPolygonEdges(s1)) && PointOnAnyEdge(e1b, GetPolygonEdges(s1));
            bool e1OnS2 = PointOnAnyEdge(e1a, GetPolygonEdges(s2)) && PointOnAnyEdge(e1b, GetPolygonEdges(s2));
            bool e2OnS1 = PointOnAnyEdge(e2a, GetPolygonEdges(s1)) && PointOnAnyEdge(e2b, GetPolygonEdges(s1));
            bool e2OnS2 = PointOnAnyEdge(e2a, GetPolygonEdges(s2)) && PointOnAnyEdge(e2b, GetPolygonEdges(s2));

            // 需要一端在 s1，另一端在 s2（任意顺序）
            if ((e1OnS1 && e2OnS2) || (e1OnS2 && e2OnS1)) return true;
            return false;
        }
    }
}