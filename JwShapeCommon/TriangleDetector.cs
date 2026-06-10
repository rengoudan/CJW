using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JwShapeCommon
{
    public class TriangleDetector
    {
        private readonly GeometryFactory _gf;
        private readonly int _roundDigits;

        public TriangleDetector(int roundDigits = 6, GeometryFactory gf = null)
        {
            _roundDigits = roundDigits;
            _gf = gf ?? new GeometryFactory();
        }

        /// <summary>
        /// 从 JwXian 集合中识别所有三角形（返回三个 JwPoint）
        /// </summary>
        public List<JwDirected> FindTriangles(List<JwXian> segments)
        {
            if (segments == null || segments.Count == 0)
                return new List<JwDirected>();

            // 转换为 LineString
            var lines = segments
                .Select(s => s.ToLineString())
                .Where(ls => ls != null && ls.NumPoints >= 2)
                .ToList();

            return FindTrianglesFromLineStrings(lines);
        }

        /// <summary>
        /// 从 LineString 集合中识别三角形（内部使用）
        /// </summary>
        private List<JwDirected> FindTrianglesFromLineStrings(List<LineString> lines)
        {
            var triangles = new List<JwDirected>();
            if (lines == null || lines.Count == 0) return triangles;

            // ============================
            // 1. 构建图结构（无向图）
            // ============================
            Func<Coordinate, string> key = c =>
                $"{Math.Round(c.X, _roundDigits):F6},{Math.Round(c.Y, _roundDigits):F6}";

            var adj = new Dictionary<string, HashSet<string>>();
            var coordMap = new Dictionary<string, Coordinate>();

            void AddEdge(Coordinate a, Coordinate b)
            {
                var ka = key(a);
                var kb = key(b);

                if (!coordMap.ContainsKey(ka)) coordMap[ka] = a;
                if (!coordMap.ContainsKey(kb)) coordMap[kb] = b;

                if (!adj.ContainsKey(ka)) adj[ka] = new HashSet<string>();
                if (!adj.ContainsKey(kb)) adj[kb] = new HashSet<string>();

                adj[ka].Add(kb);
                adj[kb].Add(ka);
            }

            foreach (var ls in lines)
            {
                var coords = ls.Coordinates;
                for (int i = 0; i < coords.Length - 1; i++)
                    AddEdge(coords[i], coords[i + 1]);
            }

            // ============================
            // 2. 查找所有 3-cycle（三角形）
            // ============================
            var visited = new HashSet<string>();

            foreach (var a in adj.Keys)
            {
                foreach (var b in adj[a])
                {
                    if (b == a) continue;

                    foreach (var c in adj[b])
                    {
                        if (c == a || c == b) continue;

                        // 闭合：c → a
                        if (adj[c].Contains(a))
                        {
                            var tri = new[] { a, b, c };
                            var norm = NormalizeTriangle(tri);

                            if (!visited.Contains(norm))
                            {
                                visited.Add(norm);

                                triangles.Add(new JwDirected(
                                    coordMap[a].ToJwPoint(),
                                    coordMap[b].ToJwPoint(),
                                    coordMap[c].ToJwPoint()
                                ));
                            }
                        }
                    }
                }
            }

            return triangles;
        }

        /// <summary>
        /// 规范化三角形顶点顺序，避免重复
        /// </summary>
        private string NormalizeTriangle(string[] seq)
        {
            var list = new List<string>();

            // 正向旋转
            for (int i = 0; i < 3; i++)
            {
                var rot = seq.Skip(i).Concat(seq.Take(i));
                list.Add(string.Join(";", rot));
            }

            // 反向旋转
            var rev = seq.Reverse().ToArray();
            for (int i = 0; i < 3; i++)
            {
                var rot = rev.Skip(i).Concat(rev.Take(i));
                list.Add(string.Join(";", rot));
            }

            list.Sort(StringComparer.Ordinal);
            return list.First();
        }
    }
}

