using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace JwBeam3D
{
    /// <summary>CSV V2.2 の穴キーワード（座標値入力）に対応する面。</summary>
    public enum CsvFace { Top, Bottom, Right, Left }

    /// <summary>
    /// 1 つの穴（横穴数・縦穴数は展開済み）。X は先端からの距離 mm、Y は面ごとの座標系での値 mm。
    /// </summary>
    public sealed class CsvHole
    {
        public CsvFace Face { get; init; }
        public double Diameter { get; init; }
        public double X { get; init; }
        public double Y { get; init; }
        /// <summary>この穴を生成した CSV 行の縦穴数（表示側で対の穴に展開するかの判定に使う）</summary>
        public int VerticalCount { get; init; } = 1;
        /// <summary>元の CSV 行（ツールチップ表示用）</summary>
        public string SourceLine { get; init; } = "";
        public int LineNumber { get; init; }
    }

    /// <summary>鋼材サイズ "H-200x100x5.5x8" → 高さ H × 幅 B × ウェブ厚 tw × フランジ厚 tf。</summary>
    public sealed class SteelSize
    {
        public string Kind { get; init; } = "";
        public double[] Values { get; init; } = Array.Empty<double>();
        public string Raw { get; init; } = "";

        public bool IsH => Kind == "H" && Values.Length >= 4;
        public double H => Values[0];
        public double B => Values[1];
        public double Tw => Values[2];
        public double Tf => Values[3];

        public static SteelSize? Parse(string raw)
        {
            var m = Regex.Match(raw.Trim(), @"^([A-Z\[]+)-([0-9.]+(?:x[0-9.]+)*)$");
            if (!m.Success) return null;
            var vals = m.Groups[2].Value.Split('x')
                .Select(s => double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var v) ? v : double.NaN)
                .ToArray();
            if (vals.Any(double.IsNaN)) return null;
            return new SteelSize { Kind = m.Groups[1].Value, Values = vals, Raw = raw.Trim() };
        }
    }

    public sealed class CsvProduct
    {
        public string ProductNo { get; set; } = "";
        public string[] Names { get; set; } = Array.Empty<string>();
        public SteelSize? Size { get; set; }
        public string SizeRaw { get; set; } = "";
        public double Length { get; set; }
        public List<CsvHole> Holes { get; } = new();
        /// <summary>未対応のキーワード・解釈できない行など</summary>
        public List<string> Warnings { get; } = new();
    }

    /// <summary>
    /// タケダ機械 CSV フォーマット V2.2 のうち、本アプリが出力する範囲（座標値入力の穴）を解析する。
    /// マーク・ピッチ入力・センター振分・タップ・長穴などのキーワードは読み飛ばして警告に記録する。
    /// </summary>
    public static class ProcessCsvParser
    {
        private static readonly Dictionary<string, CsvFace> HoleKeywords = new()
        {
            ["上面穴"] = CsvFace.Top,
            ["下面穴"] = CsvFace.Bottom,
            ["右面穴"] = CsvFace.Right,
            ["左面穴"] = CsvFace.Left,
        };

        // 仕様書に記載のその他キーワード（読み飛ばす）
        private static readonly Regex OtherKeyword = new(
            @"^(上面|下面|右面|左面)(マーク|タップ)$|^ピッチ|^センター振分|^直線長穴|^トラック|^円|^任意軌跡|^フランジ右面|^ウェブ左面",
            RegexOptions.Compiled);

        public static List<CsvProduct> Parse(string csv)
        {
            var products = new List<CsvProduct>();
            CsvProduct? cur = null;
            int headerLinesRead = 0;
            CsvFace? face = null;
            string? skipping = null;
            double prevX = 0, prevY = 0;

            var lines = csv.Replace("\r\n", "\n").Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                int lineNo = i + 1;
                var line = lines[i].Trim();
                if (line.Length == 0 || line.StartsWith(";")) continue;
                if (Regex.IsMatch(line, @"^V\d+(\.\d+)?$")) continue;

                if (line == "START")
                {
                    cur = new CsvProduct();
                    products.Add(cur);
                    headerLinesRead = 0; face = null; skipping = null; prevX = prevY = 0;
                    continue;
                }
                if (line == "END") { cur = null; continue; }
                if (cur == null) continue;

                if (HoleKeywords.TryGetValue(line, out var f)) { face = f; skipping = null; prevX = prevY = 0; continue; }
                if (OtherKeyword.IsMatch(line) && !line.Contains(','))
                {
                    face = null; skipping = line; continue;
                }

                var fields = line.Split(',').Select(s => s.Trim()).ToArray();

                if (headerLinesRead == 0)
                {
                    // 製品情報1行目: 製品番号,名称1..5,鋼材サイズ,単重,製品長,本数,...
                    cur.ProductNo = Field(fields, 0);
                    cur.Names = Enumerable.Range(1, 5).Select(k => Field(fields, k)).ToArray();
                    cur.SizeRaw = Field(fields, 6);
                    cur.Size = SteelSize.Parse(cur.SizeRaw);
                    if (cur.Size == null) cur.Warnings.Add($"{lineNo}行: 鋼材サイズを解釈できません「{cur.SizeRaw}」");
                    cur.Length = Num(Field(fields, 8)) ?? 0;
                    headerLinesRead = 1;
                    continue;
                }
                if (headerLinesRead == 1 && face == null && skipping == null)
                {
                    // 製品情報2行目（省略可）: 方向,製品名印字,... → 表示には不要
                    headerLinesRead = 2;
                    continue;
                }

                if (skipping != null)
                {
                    cur.Warnings.Add($"{lineNo}行: 未対応キーワード「{skipping}」のデータは 3D に表示していません");
                    continue;
                }
                if (face == null)
                {
                    cur.Warnings.Add($"{lineNo}行: キーワードの無いデータ行を無視しました");
                    continue;
                }

                // 座標値入力: 穴径,絶相,先尾両端,X座,Y座,横穴数,横ピッチ,縦穴数,縦ピッチ,マーク番号,フランジ
                var dia = Num(Field(fields, 0).Split('/')[0]);
                var mode = Field(fields, 1);
                var end = Field(fields, 2);
                var x0 = Num(Field(fields, 3));
                var y0 = Num(Field(fields, 4)) ?? 0;
                int nx = (int)(Num(Field(fields, 5)) ?? 1);
                double px = Num(Field(fields, 6)) ?? 0;
                int ny = (int)(Num(Field(fields, 7)) ?? 1);
                double py = Num(Field(fields, 8)) ?? 0;
                if (dia == null || x0 == null)
                {
                    cur.Warnings.Add($"{lineNo}行: 穴径またはX座を解釈できません「{line}」");
                    continue;
                }

                double bx = x0.Value, by = y0;
                switch (mode)
                {
                    case "相対X": bx = prevX + x0.Value; break;
                    case "相対XY": bx = prevX + x0.Value; by = prevY + y0; break;
                    case "絶対": case "": break;
                    default: cur.Warnings.Add($"{lineNo}行: 不明な絶相「{mode}」を絶対として扱いました"); break;
                }
                prevX = bx; prevY = by;

                bool head = end is "先端" or "両端" or "";
                bool tail = end is "尾端" or "両端";
                if (!head && !tail)
                {
                    cur.Warnings.Add($"{lineNo}行: 不明な先尾両端「{end}」を先端として扱いました");
                    head = true;
                }

                for (int ix = 0; ix < Math.Max(1, nx); ix++)
                    for (int iy = 0; iy < Math.Max(1, ny); iy++)
                    {
                        double dx = bx + ix * px, y = by + iy * py;
                        if (head) cur.Holes.Add(new CsvHole { Face = face.Value, Diameter = dia.Value, X = dx, Y = y, VerticalCount = ny, SourceLine = line, LineNumber = lineNo });
                        if (tail) cur.Holes.Add(new CsvHole { Face = face.Value, Diameter = dia.Value, X = cur.Length - dx, Y = y, VerticalCount = ny, SourceLine = line, LineNumber = lineNo });
                    }
            }
            return products;
        }

        private static string Field(string[] f, int i) => i < f.Length ? f[i] : "";

        private static double? Num(string s) =>
            double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var v) ? v : null;
    }
}
