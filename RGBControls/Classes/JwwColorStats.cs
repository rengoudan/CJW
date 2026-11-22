using System.Drawing;
using JwwHelper;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月22日 解析前の「色分布の確認」用。jww を読み、線・ソリッド・文字を線色ごとに集計する（読み取りのみ）。
    /// 解析処理（JwFileHandle）とは独立しており、解析結果・設定・出力には一切影響しない。
    /// ブロックは解析と同じ式（回転 → 倍率 → 基準点へ平行移動）で配置位置に展開する。
    /// </summary>
    public sealed class JwwColorStats
    {
        public sealed record Line(int Color, PointF A, PointF B, bool InBlock);
        public sealed record Solid(int Color, PointF[] Points, bool InBlock);
        public sealed record Text(int Color, PointF At, string Value);

        public sealed class ColorRow
        {
            public int Color { get; init; }
            public int Lines { get; set; }
            public int Solids { get; set; }
            public int Texts { get; set; }
            public int BlockItems { get; set; }
            public int Total => Lines + Solids + Texts;
        }

        public List<Line> Lines { get; } = new();
        public List<Solid> Solids { get; } = new();
        public List<Text> Texts { get; } = new();
        public double Scale { get; private set; }
        public int BlockCount { get; private set; }

        /// <summary>線色ごとの集計（件数の多い順）。</summary>
        public List<ColorRow> ByColor()
        {
            var map = new Dictionary<int, ColorRow>();
            ColorRow Row(int c) => map.TryGetValue(c, out var r) ? r : (map[c] = new ColorRow { Color = c });
            foreach (var l in Lines) { var r = Row(l.Color); r.Lines++; if (l.InBlock) r.BlockItems++; }
            foreach (var s in Solids) { var r = Row(s.Color); r.Solids++; if (s.InBlock) r.BlockItems++; }
            foreach (var t in Texts) Row(t.Color).Texts++;
            return map.Values.OrderByDescending(r => r.Total).ThenBy(r => r.Color).ToList();
        }

        public RectangleF Bounds()
        {
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            void Add(PointF p)
            {
                if (float.IsNaN(p.X) || float.IsNaN(p.Y) || float.IsInfinity(p.X) || float.IsInfinity(p.Y)) return;
                if (p.X < minX) minX = p.X; if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X; if (p.Y > maxY) maxY = p.Y;
            }
            foreach (var l in Lines) { Add(l.A); Add(l.B); }
            foreach (var s in Solids) foreach (var p in s.Points) Add(p);
            foreach (var t in Texts) Add(t.At);
            return minX > maxX ? RectangleF.Empty : RectangleF.FromLTRB(minX, minY, maxX, maxY);
        }

        // ── 読み込み ──

        private sealed class Def
        {
            public readonly List<Line> Lines = new();
            public readonly List<Solid> Solids = new();
            public readonly List<(int Number, Func<PointF, PointF> Tf)> Refs = new();
        }

        public static JwwColorStats Read(string path)
        {
            var stats = new JwwColorStats();
            using var reader = new JwwReader();
            reader.Read(path, r => stats.Collect(r));
            return stats;
        }

        private static Func<PointF, PointF> Transform(JwwBlock b)
        {
            double a = b.m_radKaitenKaku, sx = b.m_dBairitsuX, sy = b.m_dBairitsuY, ox = b.m_DPKijunTen_x, oy = b.m_DPKijunTen_y;
            double cos = Math.Cos(a), sin = Math.Sin(a);
            return p => new PointF(
                (float)((p.X * cos - p.Y * sin) * sx + ox),
                (float)((p.X * sin + p.Y * cos) * sy + oy));
        }

        private static PointF[] SolidPoints(JwwSolid s)
        {
            var pts = new List<PointF>
            {
                new((float)s.m_start_x, (float)s.m_start_y),
                new((float)s.m_end_x, (float)s.m_end_y),
                new((float)s.m_DPoint2_x, (float)s.m_DPoint2_y),
            };
            if (s.m_DPoint2_x != s.m_DPoint3_x || s.m_DPoint2_y != s.m_DPoint3_y)
                pts.Add(new((float)s.m_DPoint3_x, (float)s.m_DPoint3_y));
            return pts.ToArray();
        }

        private void Collect(JwwReader r)
        {
            Scale = r.Header.m_adScale[0];

            // ブロック定義（ネイティブのメモリはコールバックの間だけ有効なので、ここで値をコピーする）
            var defs = new Dictionary<int, Def>();
            foreach (var dl in r.DataListList)
            {
                var def = new Def();
                defs[dl.m_nNumber] = def;
                dl.EnumerateDataList(d =>
                {
                    switch (d)
                    {
                        case JwwSen s:
                            def.Lines.Add(new Line(s.m_nPenColor, new((float)s.m_start_x, (float)s.m_start_y), new((float)s.m_end_x, (float)s.m_end_y), true));
                            break;
                        case JwwSolid so:
                            def.Solids.Add(new Solid(so.m_nPenColor, SolidPoints(so), true));
                            break;
                        case JwwBlock b:
                            def.Refs.Add((b.m_nNumber, Transform(b)));
                            break;
                    }
                    return true;
                });
            }

            foreach (var d in r.DataList)
            {
                switch (d)
                {
                    case JwwSen s:
                        Lines.Add(new Line(s.m_nPenColor, new((float)s.m_start_x, (float)s.m_start_y), new((float)s.m_end_x, (float)s.m_end_y), false));
                        break;
                    case JwwSolid so:
                        Solids.Add(new Solid(so.m_nPenColor, SolidPoints(so), false));
                        break;
                    case JwwMoji m:
                        Texts.Add(new Text(m.m_nPenColor, new((float)m.m_start_x, (float)m.m_start_y), m.m_string ?? ""));
                        break;
                    case JwwBlock b:
                        BlockCount++;
                        Expand(defs, b.m_nNumber, Transform(b), 0);
                        break;
                }
            }
        }

        private void Expand(Dictionary<int, Def> defs, int number, Func<PointF, PointF> tf, int depth)
        {
            if (depth > 8 || !defs.TryGetValue(number, out var def)) return; // 循環参照の保険
            foreach (var l in def.Lines) Lines.Add(l with { A = tf(l.A), B = tf(l.B) });
            foreach (var s in def.Solids) Solids.Add(s with { Points = s.Points.Select(tf).ToArray() });
            foreach (var (n, inner) in def.Refs) Expand(defs, n, p => tf(inner(p)), depth + 1);
        }
    }
}
