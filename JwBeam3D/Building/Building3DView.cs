using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace JwBeam3D.Building
{
    /// <summary>
    /// 2026年11月27日 建物全体（全階の梁＋単柱・K 柱）の 3D 表示。
    /// 座標：X・Y＝平面（図面の向きそのまま、実寸 mm）、Z＝鉛直。
    /// 各階の梁天端（H 形鋼の右面）を Z＝階番号 ×（階高＋分解距離）に置き、
    /// 柱は下端＝その階の梁天端、上端＝上の階の梁の左面（下フランジ下面）。
    /// 操作は単梁の 3D と同じ（左ドラッグ＝回転、右ドラッグ＝移動、ホイール＝拡大縮小）。
    /// </summary>
    public sealed class Building3DView : Grid
    {
        private static readonly Color SteelA = Color.FromRgb(0x2F, 0x55, 0x97);
        private static readonly Color SteelB = Color.FromRgb(0x4F, 0x7F, 0xC4);
        private static readonly Color PostColor = Color.FromRgb(0x0F, 0x6E, 0x56);
        private static readonly Color BraceColor = Color.FromRgb(0x1D, 0x9E, 0x75);
        private static readonly Color UnmatchedColor = Color.FromRgb(0xEF, 0x9F, 0x27);
        private static readonly Color NoUpperColor = Color.FromArgb(0x90, 0x88, 0x87, 0x80);
        private static readonly Color MarkMissColor = Color.FromRgb(0xE2, 0x4B, 0x4A);
        // 単梁の 3D と同じ：穴＝明るい黄、問題のある穴＝赤
        private static readonly Color HoleColor = Color.FromRgb(0xFF, 0xD6, 0x00);
        private static readonly Color HoleProblemColor = Color.FromRgb(0xFF, 0x2D, 0x2D);
        private static readonly Color LinkColor = Color.FromRgb(0x53, 0x4A, 0xB7); // 連接線＝紫（鋼材の青・柱の緑と区別）

        private readonly HelixViewport3D _viewport;
        private readonly ModelVisual3D _root = new();
        private readonly TextBlock _info;
        private readonly TextBlock _hover;
        private readonly Dictionary<GeometryModel3D, string> _tips = new();
        private bool _hasContent;

        public Building3DView()
        {
            Background = Brushes.White;
            _viewport = new HelixViewport3D
            {
                ShowViewCube = true,
                ShowCoordinateSystem = true,
                ModelUpDirection = new Vector3D(0, 0, 1),
                ZoomExtentsWhenLoaded = false,
                IsHeadLightEnabled = true,
                Background = Brushes.White,
            };
            _viewport.RotateGesture = new MouseGesture(MouseAction.LeftClick);
            _viewport.PanGesture = new MouseGesture(MouseAction.RightClick);
            _viewport.CameraRotationMode = CameraRotationMode.Turntable;
            _viewport.Children.Add(new DefaultLights());
            _viewport.Children.Add(_root);
            _viewport.MouseMove += OnMouseMove;
            Children.Add(_viewport);

            _info = new TextBlock
            {
                Margin = new Thickness(10),
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Background = new SolidColorBrush(Color.FromArgb(0xD8, 0xFF, 0xFF, 0xFF)),
                Padding = new Thickness(6, 4, 6, 4),
            };
            Children.Add(_info);

            _hover = new TextBlock
            {
                Margin = new Thickness(10),
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Background = new SolidColorBrush(Color.FromArgb(0xE8, 0xFF, 0xFF, 0xE0)),
                Padding = new Thickness(6, 4, 6, 4),
                Visibility = Visibility.Collapsed,
            };
            Children.Add(_hover);
        }

        public void ShowMessage(string message)
        {
            _root.Content = null;
            _root.Children.Clear();
            _tips.Clear();
            _hasContent = false;
            _info.Text = message;
            _hover.Visibility = Visibility.Collapsed;
        }

        /// <param name="floors">下の階から順。</param>
        /// <param name="offsets">floors と同じ順の平面移動量。</param>
        /// <param name="resetCamera">視点を初期位置に戻すか（階高などの変更では視点を保つ）。</param>
        public void Show(IReadOnlyList<BuildingFloor> floors, IReadOnlyList<FloorOffset> offsets, BuildingViewOptions opt, bool resetCamera)
        {
            _tips.Clear();
            _root.Children.Clear();
            var group = new Model3DGroup();
            double step = opt.FloorHeight + Math.Max(0, opt.Explode);
            double Ztop(int i) => i * step;

            int pillarTotal = 0, pillarMiss = 0, markMiss = 0, beamTotal = 0, holeTotal = 0, holeBad = 0;
            var holeMat = Freeze(MaterialHelper.CreateMaterial(HoleColor));
            var holeBadMat = Freeze(MaterialHelper.CreateMaterial(HoleProblemColor));
            var linkMat = Freeze(MaterialHelper.CreateMaterial(LinkColor));
            int linkTotal = 0;
            var labels = new List<Visual3D>();

            for (int i = 0; i < floors.Count; i++)
            {
                var f = floors[i];
                var o = offsets[i];
                bool visible = opt.VisibleFloors == null || opt.VisibleFloors.Contains(f.Id);
                if (!visible) continue;
                double zt = Ztop(i);

                // 梁
                var steelMat = Freeze(MaterialHelper.CreateMaterial(i % 2 == 0 ? SteelA : SteelB));
                foreach (var b in f.Beams)
                {
                    var gm = BeamModel(b, o, zt, steelMat);
                    if (gm == null) continue;
                    group.Children.Add(gm);
                    int bad = b.Holes.Count(h => h.Problems.Count > 0);
                    string tip = $"{f.Name}　梁 {b.Code}　{b.Size.Raw}　長さ {b.Length:0}　穴 {b.Holes.Count}"
                                 + (bad > 0 ? $"（⚠ 位置に問題 {bad}）" : "")
                                 + (b.HoleError != null ? $"\n⚠ 穴を表示できません：{b.HoleError}" : "");
                    _tips[gm] = tip;
                    beamTotal++;
                    holeTotal += b.Holes.Count;
                    holeBad += bad;
                    if (opt.ShowHoles)
                    {
                        foreach (var hm in HoleModels(b, o, zt, holeMat, holeBadMat))
                        {
                            group.Children.Add(hm);
                            _tips[hm] = tip;
                        }
                    }
                }

                // 2026年11月27日 連接線：始点・終点ともこの階の梁の左面（下フランジ下面）の高さ。棒が下フランジに接するよう半径分下げる
                if (opt.ShowLinks && f.Links.Count > 0)
                {
                    double zLeft = zt - (f.Beams.FirstOrDefault()?.Size.H ?? BuildingDefaults.BeamSize.H);
                    double r = BuildingDefaults.LinkRadius, z = zLeft - r;
                    foreach (var l in f.Links)
                    {
                        var p0 = new Point3D(l.X0 + o.Dx, l.Y0 + o.Dy, z);
                        var p1 = new Point3D(l.X1 + o.Dx, l.Y1 + o.Dy, z);
                        if ((p1 - p0).Length < 1) continue;
                        var mb = new MeshBuilder(false, false);
                        mb.AddCylinder(p0, p1, r, 12, true, true);
                        mb.AddSphere(p0, r * 2.5, 12, 8);
                        mb.AddSphere(p1, r * 2.5, 12, 8);
                        var gm = new GeometryModel3D(Freeze(mb.ToMesh()), linkMat) { BackMaterial = linkMat };
                        group.Children.Add(gm);
                        double len3d = (p1 - p0).Length;
                        _tips[gm] = $"{f.Name}　連接線　長さ {(l.Length > 0 ? l.Length : len3d):0}"
                                    + $"\n始点 ({l.X0:0}, {l.Y0:0})　終点 ({l.X1:0}, {l.Y1:0})　Z={zLeft:0}（梁左面）";
                        linkTotal++;
                    }
                }

                var bb = FloorAligner.Bbox(f);
                if (bb != null)
                    labels.Add(Label(f.Name, new Point3D(bb.Value.MinX + o.Dx - 600, bb.Value.MinY + o.Dy - 600, zt), Color.FromRgb(0x21, 0x21, 0x21)));

                // 柱（この階の梁天端 → 上の階の梁の左面）
                BuildingFloor? up = i + 1 < floors.Count ? floors[i + 1] : null;
                FloorOffset? uo = up != null ? offsets[i + 1] : null;
                double upperH = up?.Beams.FirstOrDefault()?.Size.H ?? BuildingDefaults.BeamSize.H;
                double zb = zt, ztp = Ztop(i + 1) - upperH;
                var upMarks = up != null && uo != null ? up.DownMarks.Select(m => (X: m.X + uo.Dx, Y: m.Y + uo.Dy)).ToList() : new List<(double X, double Y)>();
                double tol2 = opt.MatchTolerance * opt.MatchTolerance;
                bool Near((double X, double Y) p) => upMarks.Any(m => (m.X - p.X) * (m.X - p.X) + (m.Y - p.Y) * (m.Y - p.Y) <= tol2);

                foreach (var p in f.Pillars)
                {
                    pillarTotal++;
                    var a = (X: p.AX + o.Dx, Y: p.AY + o.Dy);
                    var c = (X: p.BX + o.Dx, Y: p.BY + o.Dy);
                    string state;
                    Color postColor, braceColor;
                    if (up == null)
                    {
                        state = "上の階なし（階高で仮表示）";
                        postColor = braceColor = NoUpperColor;
                    }
                    else if (upMarks.Count == 0 || Near(a) || Near(c) || Near(((a.X + c.X) / 2, (a.Y + c.Y) / 2)))
                    {
                        state = upMarks.Count == 0 ? "上の階に叉線なし" : "上の階の叉線と一致";
                        postColor = PostColor; braceColor = BraceColor;
                    }
                    else
                    {
                        state = "⚠ 上の階の叉線と一致しません";
                        postColor = braceColor = UnmatchedColor;
                        pillarMiss++;
                    }
                    string tip = $"{f.Name}　{(p.IsK ? "K 柱" : "単柱")} {p.Code}　{state}\n下端 Z={zb:0}（梁右面）　上端 Z={ztp:0}（上階梁左面）　柱長 {ztp - zb:0}";
                    foreach (var gm in PillarModels(p.IsK, a, c, zb, ztp, Freeze(MaterialHelper.CreateMaterial(postColor)), Freeze(MaterialHelper.CreateMaterial(braceColor))))
                    {
                        group.Children.Add(gm);
                        _tips[gm] = tip;
                    }
                }

                // 上の階の叉線のうち、この階の柱が無いもの（赤い印）
                if (up != null && (opt.VisibleFloors == null || opt.VisibleFloors.Contains(up.Id)))
                {
                    var pts = f.Pillars.SelectMany(p => new[] { (X: p.AX + o.Dx, Y: p.AY + o.Dy), (X: p.BX + o.Dx, Y: p.BY + o.Dy), (X: p.CX + o.Dx, Y: p.CY + o.Dy) }).ToList();
                    var missMat = Freeze(MaterialHelper.CreateMaterial(MarkMissColor));
                    foreach (var m in upMarks)
                    {
                        if (pts.Any(q => (q.X - m.X) * (q.X - m.X) + (q.Y - m.Y) * (q.Y - m.Y) <= tol2)) continue;
                        markMiss++;
                        var mb = new MeshBuilder(false, false);
                        mb.AddBox(new Point3D(m.X, m.Y, ztp - 40), 160, 160, 80);
                        var gm = new GeometryModel3D(Freeze(mb.ToMesh()), missMat);
                        group.Children.Add(gm);
                        _tips[gm] = $"{up.Name}　叉線（下の階 {f.Name} に対応する柱がありません）";
                    }
                }
            }

            _root.Content = group;
            foreach (var l in labels) _root.Children.Add(l);
            _hasContent = group.Children.Count > 0;

            var sb = new StringBuilder();
            sb.AppendLine($"{floors.Count} 階　梁 {beamTotal} 本　柱 {pillarTotal} 本　階高 {opt.FloorHeight:0} mm" + (opt.Explode > 0 ? $"（分解 +{opt.Explode:0}）" : ""));
            sb.AppendLine($"連接線 {linkTotal} 本" + (opt.ShowLinks ? "" : "（非表示）"));
            sb.AppendLine($"穴 {holeTotal} 個" + (opt.ShowHoles ? "" : "（非表示）") + (holeBad > 0 ? $"　⚠ 位置に問題のある穴 {holeBad} 個（赤）" : ""));
            if (pillarMiss > 0) sb.AppendLine($"⚠ 上の階の叉線と一致しない柱 {pillarMiss} 本（橙）");
            if (markMiss > 0) sb.AppendLine($"⚠ 下の階に柱が無い叉線 {markMiss} 箇所（赤）");
            sb.Append("操作：左ドラッグ＝回転　右ドラッグ＝移動　ホイール＝拡大縮小");
            _info.Text = sb.ToString();

            if (resetCamera) ResetCamera();
        }

        public void ResetCamera()
        {
            if (!_hasContent) return;
            var dir = new Vector3D(-1, 1.4, -0.9);
            _viewport.Camera = new PerspectiveCamera
            {
                Position = new Point3D(-dir.X, -dir.Y, -dir.Z),
                LookDirection = dir,
                UpDirection = new Vector3D(0, 0, 1),
                FieldOfView = 45,
            };
            _viewport.ZoomExtents(0);
        }

        /// <summary>H 形鋼（ウェブ鉛直、上フランジ上面＝zTop）。</summary>
        private static GeometryModel3D? BeamModel(BuildingBeam b, FloorOffset o, double zTop, Material mat)
        {
            var s = b.Size;
            double L = b.Length;
            if (L < 1) return null;
            double H = s.H, B = s.B, tw = s.Tw, tf = s.Tf;
            double cx = (b.X0 + b.X1) / 2 + o.Dx, cy = (b.Y0 + b.Y1) / 2 + o.Dy;
            double zb = zTop - H;
            var mb = new MeshBuilder(false, false);
            if (b.Horizontal)
            {
                mb.AddBox(new Point3D(cx, cy, zb + tf / 2), L, B, tf);
                mb.AddBox(new Point3D(cx, cy, zTop - tf / 2), L, B, tf);
                mb.AddBox(new Point3D(cx, cy, zb + H / 2), L, tw, H - 2 * tf);
            }
            else
            {
                mb.AddBox(new Point3D(cx, cy, zb + tf / 2), B, L, tf);
                mb.AddBox(new Point3D(cx, cy, zTop - tf / 2), B, L, tf);
                mb.AddBox(new Point3D(cx, cy, zb + H / 2), tw, L, H - 2 * tf);
            }
            return new GeometryModel3D(Freeze(mb.ToMesh()), mat) { BackMaterial = mat };
        }

        /// <summary>
        /// 梁の穴（局所座標 → 全体座標）。局所 X＝先端（X0,Y0）からの材軸方向、局所 Y＝横方向、局所 Z＝梁下端から。
        /// 横方向の正の向きは材軸の左側（水平梁＝+Y、垂直梁＝-X）とする。フランジの穴は中心に対して左右対称の対なので、
        /// どちら向きでも見た目は変わらない。ウェブの穴は貫通なので横方向の向きに依らない。
        /// 梁ごとに 1 つ（問題のある穴は別の 1 つ）にまとめて描く（穴の数が多いので）。
        /// </summary>
        private static IEnumerable<GeometryModel3D> HoleModels(BuildingBeam b, FloorOffset o, double zTop, Material ok, Material bad)
        {
            if (b.Holes.Count == 0) yield break;
            double sx = b.X0 + o.Dx, sy = b.Y0 + o.Dy, zb = zTop - b.Size.H;
            double ux = b.Horizontal ? 1 : 0, uy = b.Horizontal ? 0 : 1;   // 材軸
            double nx = b.Horizontal ? 0 : -1, ny = b.Horizontal ? 1 : 0;  // 横方向
            Point3D G((double X, double Y, double Z) p) => new(sx + ux * p.X + nx * p.Y, sy + uy * p.X + ny * p.Y, zb + p.Z);

            var good = new MeshBuilder(false, false);
            var wrong = new MeshBuilder(false, false);
            int ng = 0, nw = 0;
            foreach (var h in b.Holes)
            {
                var mb = h.Problems.Count > 0 ? wrong : good;
                // cap 付きの AddCylinder の第 3 引数は半径（Beam3DView と同じ）
                mb.AddCylinder(G(h.P1), G(h.P2), h.Source.Diameter / 2, 16, true, true);
                if (h.Problems.Count > 0) nw++; else ng++;
            }
            if (ng > 0) yield return new GeometryModel3D(Freeze(good.ToMesh()), ok);
            if (nw > 0) yield return new GeometryModel3D(Freeze(wrong.ToMesh()), bad);
        }

        /// <summary>
        /// 単柱：竖柱 1 本。K 柱：竖柱 2 本（A・B）＋斜撑 2 本（B の上端 → A の中間 → B の下端）。
        /// 向きは当面 A／B の順のまま（左右の入れ替えは考慮しない）。
        /// </summary>
        private static List<GeometryModel3D> PillarModels(bool isK, (double X, double Y) a, (double X, double Y) b, double zb, double zt, Material postMat, Material braceMat)
        {
            double ps = BuildingDefaults.PostSize, len = Math.Max(zt - zb, 1);
            var posts = new MeshBuilder(false, false);
            posts.AddBox(new Point3D(a.X, a.Y, zb + len / 2), ps, ps, len);
            var result = new List<GeometryModel3D>();
            if (isK)
            {
                posts.AddBox(new Point3D(b.X, b.Y, zb + len / 2), ps, ps, len);
                double dx = b.X - a.X, dy = b.Y - a.Y, d = Math.Sqrt(dx * dx + dy * dy);
                if (d > ps)
                {
                    double ux = dx / d, uy = dy / d, h = ps / 2, bs = BuildingDefaults.BraceSize;
                    var pa = new Point3D(a.X + ux * h, a.Y + uy * h, zb + len / 2);
                    var pTop = new Point3D(b.X - ux * h, b.Y - uy * h, zt - bs);
                    var pBot = new Point3D(b.X - ux * h, b.Y - uy * h, zb + bs);
                    var br = new MeshBuilder(false, false);
                    double r = bs / Math.Sqrt(2); // 4 分割の円柱＝一辺 bs の角管
                    br.AddCylinder(pTop, pa, r, 4, true, true);
                    br.AddCylinder(pa, pBot, r, 4, true, true);
                    result.Add(new GeometryModel3D(Freeze(br.ToMesh()), braceMat) { BackMaterial = braceMat });
                }
            }
            result.Insert(0, new GeometryModel3D(Freeze(posts.ToMesh()), postMat) { BackMaterial = postMat });
            return result;
        }

        private static T Freeze<T>(T f) where T : Freezable
        {
            if (f.CanFreeze) f.Freeze();
            return f;
        }

        private static BillboardTextVisual3D Label(string text, Point3D pos, Color color) => new()
        {
            Text = text,
            Position = pos,
            Foreground = new SolidColorBrush(color),
            Background = new SolidColorBrush(Color.FromArgb(0xC8, 0xFF, 0xFF, 0xFF)),
            FontSize = 14,
            FontWeight = FontWeights.Bold,
        };

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var hits = Viewport3DHelper.FindHits(_viewport.Viewport, e.GetPosition(_viewport.Viewport));
            var tip = hits.Select(h => h.Model as GeometryModel3D)
                          .Where(m => m != null && _tips.ContainsKey(m))
                          .Select(m => _tips[m!])
                          .FirstOrDefault();
            if (tip == null) { _hover.Visibility = Visibility.Collapsed; return; }
            _hover.Text = tip;
            _hover.Visibility = Visibility.Visible;
        }
    }
}
