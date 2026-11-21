using System;
using System.Collections.Generic;

namespace JwBeam3D
{
    /// <summary>
    /// CSV の面座標 → 3D 座標（建築上の取付け姿勢）への変換と、穴位置の妥当性チェック。
    ///
    /// 座標系（単位 mm）:
    ///   X = 材軸方向（先端 = 0 → 尾端 = 製品長）
    ///   Y = 横方向（+ は加工機で「上面」＝ウェブ上側が向いていた側。加工機の「天」方向）
    ///   Z = 鉛直（0 = 梁下端, H = 梁上端）
    ///
    /// 面の対応（CSV フォーマット V2.2 第6章 H型鋼材 ＋ 本アプリの定義）:
    ///   上面穴 = ウェブ（HasTop ← hole.HasCenter）。Y座は原点②（右面＝上フランジ外面側）から左面側へ ＋。負にならない。
    ///          → 3D: Z = H − Y座（梁上端から下向きに測る）
    ///   右面穴 = 上フランジ（HasRight ← hole.HasTop）。Y座は面の中心が原点、＋は「天」側（負になり得る）。
    ///   左面穴 = 下フランジ（HasLeft ← hole.HasBottom）。同上。
    ///   ※ 2026/11/22 ユーザー訂正：JwHole.HasTop/HasBottom は実際の上下と一致している。
    ///     誤っていたのは JwHoleMachining のコメント（HasLeft=上面, HasRight=下面 と書かれていた）の方。
    /// </summary>
    public sealed class PlacedHole
    {
        public CsvHole Source { get; init; } = null!;
        /// <summary>この穴の面座標 Y（対の 2 個目では Source.Y + ピッチ）</summary>
        public double Y { get; init; }
        /// <summary>CSV 1 行から 2D 図と同じく対で展開した 2 個目の穴か</summary>
        public bool IsPairPartner { get; init; }
        /// <summary>穴の軸の両端（板厚方向に少し突き出す）</summary>
        public (double X, double Y, double Z) P1 { get; init; }
        public (double X, double Y, double Z) P2 { get; init; }
        /// <summary>空なら問題なし。材料外・ウェブ干渉などの理由。</summary>
        public List<string> Problems { get; } = new();
        public string FaceName => Source.Face switch
        {
            CsvFace.Top => "上面穴（ウェブ）",
            CsvFace.Right => "右面穴（上フランジ）",
            CsvFace.Left => "左面穴（下フランジ）",
            CsvFace.Bottom => "下面穴",
            _ => Source.Face.ToString(),
        };
    }

    public static class BeamGeometry
    {
        private const double Protrude = 1.0; // 穴の円柱を板から少しはみ出させて見やすくする

        /// <param name="pairPitch">
        /// 0 より大きい場合、縦穴数 1 の CSV 行を「Y と Y+pairPitch の 2 穴」として表示する。
        /// 2D 図（JwHoleMachining.DrawToJww）が 1 加工記録を面中心 ±EllipseSpacing/2 の 2 穴で描くのと同じ扱い
        /// （CSV の Y は対の下側＝中心 −28、もう 1 個は +56 の位置）。0 なら CSV の記述どおり 1 穴。
        /// </param>
        public static List<PlacedHole> Place(CsvProduct p, double pairPitch = 0)
        {
            var result = new List<PlacedHole>();
            foreach (var h in p.Holes)
            {
                result.Add(PlaceOne(p, h, h.Y, false));
                if (pairPitch > 0 && h.VerticalCount == 1)
                    result.Add(PlaceOne(p, h, h.Y + pairPitch, true));
            }
            return result;
        }

        private static PlacedHole PlaceOne(CsvProduct p, CsvHole h, double y, bool partner)
        {
            var s = p.Size!;
            double H = s.H, B = s.B, tw = s.Tw, tf = s.Tf, L = p.Length;
            double r = h.Diameter / 2;
            PlacedHole ph;
            switch (h.Face)
            {
                case CsvFace.Top: // ウェブ：横方向に貫通。Y座＝梁上端（右面＝上フランジ外面、原点②）から下向きの距離
                    double zw = H - y;
                    ph = new PlacedHole
                    {
                        Source = h, Y = y, IsPairPartner = partner,
                        P1 = (h.X, -tw / 2 - Protrude, zw),
                        P2 = (h.X, tw / 2 + Protrude, zw),
                    };
                    if (y - r < tf || y + r > H - tf)
                        ph.Problems.Add($"ウェブ範囲外（フランジに掛かる）: Y={y}, 有効範囲 {tf + r:0.#}～{H - tf - r:0.#}");
                    break;
                case CsvFace.Right: // 上フランジ：鉛直に貫通。Y座＝フランジ中心からの距離
                case CsvFace.Left:  // 下フランジ
                    double zc = h.Face == CsvFace.Right ? H - tf / 2 : tf / 2;
                    ph = new PlacedHole
                    {
                        Source = h, Y = y, IsPairPartner = partner,
                        P1 = (h.X, y, zc - tf / 2 - Protrude),
                        P2 = (h.X, y, zc + tf / 2 + Protrude),
                    };
                    if (Math.Abs(y) + r > B / 2)
                        ph.Problems.Add($"フランジ幅の外: Y={y}, 有効範囲 ±{B / 2 - r:0.#}");
                    else if (Math.Abs(y) - r < tw / 2)
                        ph.Problems.Add($"ウェブと干渉: Y={y}（ウェブ厚 {tw}）");
                    break;
                default:
                    ph = new PlacedHole { Source = h, Y = y, IsPairPartner = partner, P1 = (h.X, 0, 0), P2 = (h.X, 0, 0) };
                    ph.Problems.Add("H形鋼に下面穴はありません（表示対象外）");
                    break;
            }
            if (h.X - r < 0 || h.X + r > L)
                ph.Problems.Add($"製品長の外: X={h.X}, 製品長 {L}");
            return ph;
        }
    }
}
