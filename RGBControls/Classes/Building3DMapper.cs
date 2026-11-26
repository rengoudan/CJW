using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using JwBeam3D;
using JwBeam3D.Building;
using JwCore;
using JwServices;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月27日 建物全体の 3D 表示用に、工事の各階のデータを実寸の表示データへ変換する（読み取りのみ。データは書き換えない）。
    /// ・図面座標 × 縮尺 ＝ 実寸 mm（JwExtend.DataToJw と同じ解釈：Location＝左上、Y は上向き）
    /// ・梁の鋼材は BeamXHName → 階の既定 → CSV と同じ H-200x100x5.5x8
    /// ・階の順：階名（取り込み時の既定はファイル名、例「xxx1階」）の数字。全階に数字が無ければ取り込んだ順
    /// </summary>
    internal static class Building3DMapper
    {
        public static async Task<List<(JwProjectSubData Sub, BuildingFloor Floor)>> LoadAsync(JwProjectMainService service, JwProjectMainData main)
        {
            long mainId = main.Id;
            var subs = await service.GetSubDatasAsync(s => s.JwProjectMainDataId == mainId); // 梁統計と同じ読み方
            var result = new List<(JwProjectSubData, BuildingFloor)>();
            foreach (var sub in subs)
            {
                await service.LoadSubCollectionAsync(sub);
                foreach (var b in sub.JwBeamDatas) await service.LoadBeamCollectionAsync(b); // 穴（CSV の元）
                result.Add((sub, ToFloor(sub)));
            }
            return OrderFloors(result);
        }

        public static BuildingFloor ToFloor(JwProjectSubData sub)
        {
            double s = sub.Scale > 0 ? sub.Scale
                     : double.TryParse(sub.Biaochi, NumberStyles.Float, CultureInfo.InvariantCulture, out var bs) && bs > 0 ? bs : 1;
            var floor = new BuildingFloor { Id = sub.Id, Name = string.IsNullOrWhiteSpace(sub.FloorName) ? "（階名なし）" : sub.FloorName! };
            var defaultSize = ParseSize(sub.DefaultBeamXHName) ?? BuildingDefaults.BeamSize;

            foreach (var b in sub.JwBeamDatas)
            {
                if (b.Location == null || b.IsParentBeam) continue;
                double x = b.Location.X, y = b.Location.Y, w = b.Width, h = b.Height;
                bool horizontal = b.DirectionType == BeamDirectionType.Horizontal;
                var (holes, holeError) = Holes(b, s);
                floor.Beams.Add(new BuildingBeam
                {
                    Id = b.Id,
                    Code = b.BeamCode,
                    Horizontal = horizontal,
                    X0 = (horizontal ? x : x + w / 2) * s,
                    X1 = (horizontal ? x + w : x + w / 2) * s,
                    Y0 = (horizontal ? y - h / 2 : y - h) * s,
                    Y1 = (horizontal ? y - h / 2 : y) * s,
                    Size = ParseSize(b.BeamXHName) ?? defaultSize,
                    Holes = holes,
                    HoleError = holeError,
                });
            }

            foreach (var p in sub.JwPillarDatas)
            {
                bool isK = p.BaseType == PillarBaseType.KPillar;
                if (p.BaseType == PillarBaseType.None) continue;
                var a = p.FirstLocation ?? p.Location;
                var c = isK ? p.LastLocation ?? a : a;
                if (a == null || c == null) continue;
                floor.Pillars.Add(new BuildingPillar
                {
                    Id = p.Id,
                    Code = string.IsNullOrEmpty(p.TaggTitle) ? p.PillarCode ?? "" : p.TaggTitle,
                    IsK = isK,
                    AX = a.X * s, AY = a.Y * s,
                    BX = c.X * s, BY = c.Y * s,
                });
            }

            foreach (var m in sub.JwDownPillarDatas)
            {
                if (m.Location == null) continue;
                floor.DownMarks.Add((m.Location.X * s, m.Location.Y * s));
            }

            // 2026年11月27日 連接線（調整後の Start／End。ブレース施工図と同じ点）
            foreach (var l in sub.JwLianjieDatas)
            {
                if (l.Start == null || l.End == null) continue;
                floor.Links.Add(new BuildingLink
                {
                    Id = l.Id ?? "",
                    X0 = l.Start.X * s, Y0 = l.Start.Y * s,
                    X1 = l.End.X * s, Y1 = l.End.Y * s,
                    Length = l.Length,
                });
            }
            return floor;
        }

        /// <summary>
        /// 梁の穴：梁統計の 3D と同じく、データ → JwBeam（穴の再計算）→ CSV → BeamGeometry.Place（2D と同じ対の穴）。
        /// メモリ上で作るだけで保存はしない。縮尺は全体設定なので一時的に階の値にして戻す。
        /// </summary>
        private static (IReadOnlyList<PlacedHole> Holes, string? Error) Holes(JwBeamData data, double scale)
        {
            double old = JwShapeCommon.JwFileConsts.JwScale;
            try
            {
                JwShapeCommon.JwFileConsts.JwScale = scale;
                var csv = JwShapeCommon.JwExtend.BeamDataToJwBeam(data).ToProcessCsv();
                var product = ProcessCsvParser.Parse(csv).FirstOrDefault();
                if (product?.Size == null || !product.Size.IsH) return (Array.Empty<PlacedHole>(), "CSV を読めません");
                var placed = BeamGeometry.Place(product, JwShapeCommon.JwFileConsts.EllipseSpacing)
                                         .Where(p => p.Source.Face != CsvFace.Bottom).ToList();
                return (placed, null);
            }
            catch (Exception ex)
            {
                JwLogging.AppLog.Warn(ex, "建物 3D：穴を作れません {Beam}", data.BeamCode);
                return (Array.Empty<PlacedHole>(), ex.Message);
            }
            finally
            {
                JwShapeCommon.JwFileConsts.JwScale = old;
            }
        }

        private static SteelSize? ParseSize(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            var t = raw.Replace('×', 'x').Replace('Ｘ', 'x').Replace('X', 'x').Replace('ｘ', 'x').Replace(" ", "").Replace("　", "");
            var size = SteelSize.Parse(t);
            return size != null && size.IsH ? size : null;
        }

        /// <summary>階名の数字で並べる（全階に数字があるときだけ）。それ以外は取り込んだ順。</summary>
        public static List<(JwProjectSubData Sub, BuildingFloor Floor)> OrderFloors(List<(JwProjectSubData Sub, BuildingFloor Floor)> floors)
        {
            var numbered = floors.Select(f => (Item: f, No: FloorNumber(f.Sub.FloorName))).ToList();
            if (numbered.All(x => x.No.HasValue))
                return numbered.OrderBy(x => x.No!.Value).ThenBy(x => x.Item.Sub.CreationTime).Select(x => x.Item).ToList();
            return floors.OrderBy(f => f.Sub.CreationTime).ToList();
        }

        private static readonly Regex Basement = new(@"(?:地下|(?<![A-Za-z])B)\s*(\d+)\s*(?:階|F)?", RegexOptions.IgnoreCase);
        private static readonly Regex Above = new(@"(\d+)\s*(?:階|F)", RegexOptions.IgnoreCase);
        private static readonly Regex Roof = new(@"(?:^|[^A-Z])(?:RF|R階|PH)|屋上|屋根", RegexOptions.IgnoreCase);

        /// <summary>「xxx1階」「2F」→ 1・2、「B1」「地下1階」→ -1、「RF」「屋上」→ 最上。読めなければ null。</summary>
        public static int? FloorNumber(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var n = ToHalfWidth(name);
            if (Roof.IsMatch(n)) return 10000;
            var m = Basement.Match(n);
            if (m.Success && int.TryParse(m.Groups[1].Value, out var b)) return -b;
            var all = Above.Matches(n);
            if (all.Count > 0 && int.TryParse(all[^1].Groups[1].Value, out var a)) return a; // 末尾に近い方を採用（工事番号などの数字を避ける）
            return null;
        }

        private static string ToHalfWidth(string s)
        {
            var sb = new StringBuilder(s.Length);
            foreach (var ch in s)
            {
                if (ch >= '０' && ch <= '９') sb.Append((char)('0' + (ch - '０')));
                else if (ch >= 'Ａ' && ch <= 'Ｚ') sb.Append((char)('A' + (ch - 'Ａ')));
                else if (ch >= 'ａ' && ch <= 'ｚ') sb.Append((char)('a' + (ch - 'ａ')));
                else sb.Append(ch);
            }
            return sb.ToString();
        }
    }
}
