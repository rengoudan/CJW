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

namespace JwBeam3D
{
    /// <summary>
    /// H形鋼 1 本を 3D 表示する WPF ビュー（コードのみで構築）。
    /// 回転：右ドラッグ / パン：中ドラッグ / ズーム：ホイール（HelixToolkit 既定）。
    /// </summary>
    public sealed class Beam3DView : Grid
    {
        // 2026年11月21日 鋼材と穴は高コントラストに（鋼材＝濃い青、穴＝明るい黄、問題のある穴＝赤）
        private static readonly Color SteelColor = Color.FromRgb(0x2F, 0x55, 0x97);
        private static readonly Color HoleColor = Color.FromRgb(0xFF, 0xD6, 0x00);
        private static readonly Color ProblemColor = Color.FromRgb(0xFF, 0x2D, 0x2D);
        private static readonly Color HeadColor = Color.FromRgb(0x2E, 0x7D, 0x32);

        private readonly HelixViewport3D _viewport;
        private readonly ModelVisual3D _root = new();
        private readonly TextBlock _info;
        private readonly TextBlock _hover;
        private readonly CheckBox _machineView;
        private readonly Dictionary<GeometryModel3D, PlacedHole> _holeModels = new();

        public Beam3DView()
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
            // 2026年11月21日 操作：左ドラッグ＝回転（左下の座標軸も連動）、右ドラッグ＝パン、ホイール＝ズーム
            _viewport.RotateGesture = new MouseGesture(MouseAction.LeftClick);
            _viewport.PanGesture = new MouseGesture(MouseAction.RightClick);
            _viewport.CameraRotationMode = CameraRotationMode.Turntable; // Z 軸（鉛直）を保ったまま回す
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

            _machineView = new CheckBox
            {
                Content = "加工機の向き（ウェブ水平）",
                Margin = new Thickness(10),
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Padding = new Thickness(4, 0, 0, 0),
            };
            // ViewCube と重ならないよう少し下げる
            _machineView.Margin = new Thickness(10, 10, 140, 10);
            _machineView.Checked += (s, e) => ApplyOrientation(true);
            _machineView.Unchecked += (s, e) => ApplyOrientation(false);
            Children.Add(_machineView);
        }

        /// <summary>加工機の向き（ウェブ水平）で表示するか。</summary>
        public bool MachineOrientation
        {
            get => _machineView.IsChecked == true;
            set => _machineView.IsChecked = value;
        }

        /// <summary>メッセージのみ表示（解析できない場合など）。</summary>
        public void ShowMessage(string message)
        {
            _root.Content = null;
            _holeModels.Clear();
            _info.Text = message;
            _hover.Visibility = Visibility.Collapsed;
        }

        public void Show(CsvProduct product, IReadOnlyList<PlacedHole> holes)
        {
            var s = product.Size!;
            double H = s.H, B = s.B, tw = s.Tw, tf = s.Tf, L = Math.Max(product.Length, 1);

            var group = new Model3DGroup();
            _holeModels.Clear();

            // 鋼材：下フランジ・上フランジ・ウェブの 3 つの直方体
            var steel = new MeshBuilder(false, false);
            steel.AddBox(new Point3D(L / 2, 0, tf / 2), L, B, tf);
            steel.AddBox(new Point3D(L / 2, 0, H - tf / 2), L, B, tf);
            steel.AddBox(new Point3D(L / 2, 0, H / 2), L, tw, H - 2 * tf);
            group.Children.Add(new GeometryModel3D(steel.ToMesh(), MaterialHelper.CreateMaterial(SteelColor))
            {
                BackMaterial = MaterialHelper.CreateMaterial(SteelColor),
            });

            // 先端の目印（材端に薄い緑の板）
            var head = new MeshBuilder(false, false);
            head.AddBox(new Point3D(-0.5, 0, H / 2), 1, B + 6, H + 6);
            group.Children.Add(new GeometryModel3D(head.ToMesh(), MaterialHelper.CreateMaterial(Color.FromArgb(0x80, HeadColor.R, HeadColor.G, HeadColor.B))));

            // 穴：1 穴ずつ別モデル（ホバーで情報表示するため）
            foreach (var h in holes.Where(h => h.Source.Face != CsvFace.Bottom))
            {
                var mb = new MeshBuilder(false, false);
                // 両端にフタを付けて、板面上で塗りつぶした円として見えるようにする。
                // 注意：このオーバーロード（cap 付き）の第 3 引数は「半径」（XML ドキュメントの説明文は "diameters" だが引数名は radius）。
                // 以前は直径をそのまま渡していたため穴が 2 倍の大きさで表示されていた（2026/11/21 修正）。
                mb.AddCylinder(new Point3D(h.P1.X, h.P1.Y, h.P1.Z), new Point3D(h.P2.X, h.P2.Y, h.P2.Z), h.Source.Diameter / 2, 32, true, true);
                var color = h.Problems.Count > 0 ? ProblemColor : HoleColor;
                var gm = new GeometryModel3D(mb.ToMesh(), MaterialHelper.CreateMaterial(color));
                group.Children.Add(gm);
                _holeModels[gm] = h;
            }

            _root.Content = group;
            _root.Children.Clear();
            _root.Children.Add(Label("先端", new Point3D(-90, -B / 2 - 60, H + 60), HeadColor));
            _root.Children.Add(Label("尾端", new Point3D(L + 90, 0, H + 70), Colors.DimGray));

            // 面の名称（CSV キーワード）。先端の断面の外側に置き、梁本体と重ならないようにする。
            // 姿勢を切り替えても面に付いて回る
            var faceColor = Color.FromRgb(0x21, 0x21, 0x21); // 鋼材（青）と区別できる濃いグレー
            _root.Children.Add(Label("上面（ウェブ）", new Point3D(-90, B / 2 + 40, H / 2), faceColor));
            _root.Children.Add(Label("右面（上フランジ）", new Point3D(-90, 0, H + 40), faceColor));
            _root.Children.Add(Label("左面（下フランジ）", new Point3D(-90, 0, -40), faceColor));

            // 両端の I 形断面の輪郭線
            var outline = new LinesVisual3D { Color = Colors.Black, Thickness = 1.5 };
            foreach (var x in new[] { 0.0, L })
            {
                var p = SectionOutline(x, H, B, tw, tf);
                for (int i = 0; i < p.Count; i++) { outline.Points.Add(p[i]); outline.Points.Add(p[(i + 1) % p.Count]); }
            }
            _root.Children.Add(outline);

            // 情報パネル
            var sb = new StringBuilder();
            sb.AppendLine($"{s.Raw}　製品長 {product.Length:0.#} mm");
            int top = holes.Count(h => h.Source.Face == CsvFace.Top);
            int right = holes.Count(h => h.Source.Face == CsvFace.Right);
            int left = holes.Count(h => h.Source.Face == CsvFace.Left);
            sb.AppendLine($"上面(ウェブ) {top}　右面(上フランジ) {right}　左面(下フランジ) {left}");
            if (holes.Any(h => h.IsPairPartner))
                sb.AppendLine($"※ 2D 図と同じく、CSV 1 行を Y 方向ピッチ {holes.First(h => h.IsPairPartner).Y - holes.First(h => h.IsPairPartner).Source.Y:0.#} の 2 穴で表示（計 {holes.Count} 穴）");
            int bad = holes.Count(h => h.Problems.Count > 0);
            if (bad > 0) sb.AppendLine($"⚠ 位置に問題のある穴 {bad} 個（赤表示）");
            foreach (var w in product.Warnings.Take(3)) sb.AppendLine("⚠ " + w);
            if (product.Warnings.Count > 3) sb.AppendLine($"⚠ ほか {product.Warnings.Count - 3} 件");
            sb.AppendLine("操作：左ドラッグ＝回転　右ドラッグ＝移動　ホイール＝拡大縮小");
            _info.Text = sb.ToString().TrimEnd();

            ApplyOrientation(_machineView.IsChecked == true);
        }

        /// <summary>X 位置での I 形断面の外形（12 点、フィレットは省略）。</summary>
        private static List<Point3D> SectionOutline(double x, double H, double B, double tw, double tf)
        {
            double b = B / 2, w = tw / 2;
            (double y, double z)[] yz =
            {
                (-b, 0), (b, 0), (b, tf), (w, tf), (w, H - tf), (b, H - tf),
                (b, H), (-b, H), (-b, H - tf), (-w, H - tf), (-w, tf), (-b, tf),
            };
            return yz.Select(p => new Point3D(x, p.y, p.z)).ToList();
        }

        private static BillboardTextVisual3D Label(string text, Point3D pos, Color color) => new()
        {
            Text = text,
            Position = pos,
            Foreground = new SolidColorBrush(color),
            Background = Brushes.Transparent,
            FontSize = 14,
            FontWeight = FontWeights.Bold,
        };

        /// <summary>
        /// 建築の取付け姿勢（ウェブ鉛直）／加工機の向き（ウェブ水平、上面が上）を切り替える。
        /// 加工機の向き = X 軸回りに +90°（+Y＝上面側 → 上）。
        /// </summary>
        private void ApplyOrientation(bool machine)
        {
            _root.Transform = machine
                ? new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90))
                : Transform3D.Identity;
            ResetView(machine);
        }

        private void ResetView(bool machine)
        {
            if (_root.Content == null) return;
            // 取付け姿勢：「上面」（ウェブの加工面、+Y 側）から斜め上に見下ろす。
            // 加工機の向き：先端側から材軸方向に見る（断面形状と 3 面の位置関係が分かりやすい）。
            var dir = machine ? new Vector3D(1.5, -0.6, -0.9) : new Vector3D(1, -1.6, -1.2);
            _viewport.Camera = new PerspectiveCamera
            {
                Position = new Point3D(-dir.X, -dir.Y, -dir.Z),
                LookDirection = dir,
                UpDirection = new Vector3D(0, 0, 1),
                FieldOfView = 45,
            };
            _viewport.ZoomExtents(0);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var hits = Viewport3DHelper.FindHits(_viewport.Viewport, e.GetPosition(_viewport.Viewport));
            var hole = hits.Select(h => h.Model as GeometryModel3D)
                           .Where(m => m != null && _holeModels.ContainsKey(m))
                           .Select(m => _holeModels[m!])
                           .FirstOrDefault();
            if (hole == null)
            {
                _hover.Visibility = Visibility.Collapsed;
                return;
            }
            var sb = new StringBuilder();
            sb.AppendLine($"{hole.FaceName}　φ{hole.Source.Diameter:0.#}");
            sb.AppendLine($"X={hole.Source.X:0.#}（先端から）　Y座={hole.Y:0.#}" +
                          (hole.IsPairPartner ? $"（対の穴：CSV の Y座 {hole.Source.Y:0.#} ＋ピッチ）" : ""));
            sb.Append($"CSV {hole.Source.LineNumber}行: {hole.Source.SourceLine}");
            foreach (var p in hole.Problems) sb.Append("\n⚠ " + p);
            _hover.Text = sb.ToString();
            _hover.Visibility = Visibility.Visible;
        }
    }
}
