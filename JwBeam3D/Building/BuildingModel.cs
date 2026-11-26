using System;
using System.Collections.Generic;

namespace JwBeam3D.Building
{
    /// <summary>
    /// 2026年11月27日 建物全体の 3D 表示に渡すデータ（業務データから変換済みの実寸 mm）。
    /// 平面座標は各階の図面座標 × 縮尺（＝実寸）。階どうしの原点は一致しないことがあるので、
    /// 表示時に階ごとの平行移動（FloorOffset）を加えて揃える。
    /// 高さ方向の約束：設計図に描かれた梁＝H 形鋼の右面（上フランジ上面）。
    ///   柱の下端＝その階の梁の右面、柱の上端＝上の階の梁の左面（下フランジ下面、上階の図の叉線の位置）。
    /// </summary>
    public sealed class BuildingBeam
    {
        public string Id { get; init; } = "";
        public string Code { get; init; } = "";
        /// <summary>材軸の中心線の両端（実寸 mm、階の図面座標系）。</summary>
        public double X0 { get; init; }
        public double Y0 { get; init; }
        public double X1 { get; init; }
        public double Y1 { get; init; }
        /// <summary>true＝水平梁（材軸が X 方向）、false＝垂直梁（材軸が Y 方向）。</summary>
        public bool Horizontal { get; init; }
        public SteelSize Size { get; init; } = BuildingDefaults.BeamSize;
        public double Length => Math.Sqrt((X1 - X0) * (X1 - X0) + (Y1 - Y0) * (Y1 - Y0));

        /// <summary>
        /// 2026年11月27日 穴（梁の CSV を BeamGeometry.Place で配置したもの＝単梁の 3D・加工機と同じ値）。
        /// 局所座標：X＝先端（X0,Y0 側＝図面で X／Y の小さい端）からの距離、Y＝横方向、Z＝梁下端からの高さ。
        /// </summary>
        public IReadOnlyList<PlacedHole> Holes { get; init; } = Array.Empty<PlacedHole>();

        /// <summary>穴を作れなかった理由（なければ null）。</summary>
        public string? HoleError { get; init; }
    }

    public sealed class BuildingPillar
    {
        public string Id { get; init; } = "";
        public string Code { get; init; } = "";
        public bool IsK { get; init; }
        /// <summary>竖柱 A の中心（単柱はこれのみ）。実寸 mm。</summary>
        public double AX { get; init; }
        public double AY { get; init; }
        /// <summary>竖柱 B の中心（K 柱のみ。単柱は A と同じ）。</summary>
        public double BX { get; init; }
        public double BY { get; init; }
        public double CX => (AX + BX) / 2;
        public double CY => (AY + BY) / 2;
    }

    public sealed class BuildingFloor
    {
        public string Id { get; init; } = "";
        public string Name { get; init; } = "";
        public List<BuildingBeam> Beams { get; } = new();
        public List<BuildingPillar> Pillars { get; } = new();
        /// <summary>この階の図にある叉線（下の階の柱が当たる位置）の中心。実寸 mm。</summary>
        public List<(double X, double Y)> DownMarks { get; } = new();
        /// <summary>2026年11月27日 連接線（ブレース）。始点・終点はこの階の梁の左面（下フランジ下面）の高さ。</summary>
        public List<BuildingLink> Links { get; } = new();
    }

    public sealed class BuildingLink
    {
        public string Id { get; init; } = "";
        /// <summary>始点・終点（実寸 mm、階の図面座標系）。</summary>
        public double X0 { get; init; }
        public double Y0 { get; init; }
        public double X1 { get; init; }
        public double Y1 { get; init; }
        /// <summary>保存されている長さ mm（表示用）。</summary>
        public double Length { get; init; }
    }

    /// <summary>階の図面座標 → 全体座標の平行移動。</summary>
    public sealed class FloorOffset
    {
        public double Dx { get; set; }
        public double Dy { get; set; }
        /// <summary>どう決めたか（画面表示用）。</summary>
        public string Method { get; set; } = "";
        /// <summary>手動で調整した値か。</summary>
        public bool IsManual { get; set; }
    }

    public sealed class BuildingViewOptions
    {
        /// <summary>階高：その階の梁天端（右面）から上の階の梁天端まで（mm）。</summary>
        public double FloorHeight { get; set; } = BuildingDefaults.FloorHeight;
        /// <summary>表示だけの層間の追加距離（分解表示）。柱もその分伸ばす。</summary>
        public double Explode { get; set; }
        /// <summary>表示する階の Id（null なら全階）。</summary>
        public ISet<string>? VisibleFloors { get; set; }
        /// <summary>柱と上階の叉線の一致判定の許容差（mm）。</summary>
        public double MatchTolerance { get; set; } = BuildingDefaults.MatchTolerance;
        /// <summary>梁の穴を表示するか。</summary>
        public bool ShowHoles { get; set; } = true;
        /// <summary>連接線を表示するか。</summary>
        public bool ShowLinks { get; set; } = true;
    }

    public static class BuildingDefaults
    {
        /// <summary>CSV 出力と同じ既定の鋼材（JwBeam.ToProcessCsv）。</summary>
        public static readonly SteelSize BeamSize = SteelSize.Parse("H-200x100x5.5x8")!;
        public const double FloorHeight = 2000;
        public const double PostSize = 100;   // □-100
        public const double BraceSize = 50;   // □-50
        public const double LinkRadius = 14;  // 連接線の表示太さ（半径。全体表示でも見えるよう実物より太め）
        public const double MatchTolerance = 150;
        public const double GridTolerance = 50;
    }
}
