// ============================================================================================
// JwRegressionCheck — 解析結果・CSV 出力が変わっていないことを確認する開発用ツール
//
//   JwRegressionCheck --db <jwdata.db> --project <工事名> --floor <階名>
//                     [--jww <図面.jww>] [--csv <既知の正しい CSV>] [--appconfig <appconfig.json>]
//
//   ・DB はコピーしてから開く（元の DB は変更しない）
//   ・[1] DB の階データ → CSV（画面の「梁製造CSV」と同じ処理）→ 既知の CSV とバイト比較
//   ・[2] jww を現在のコードで解析（解析設定は DB の顧客設定・階データから復元）→ DB の階データと内容比較
//   ・[3] [2] の解析結果から CSV を生成 → 既知の CSV とバイト比較（jww → CSV の通し確認）
//   すべて一致で終了コード 0、差分があれば 1。
// ============================================================================================
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using JwCore;
using JwData;
using JwServices;
using JwShapeCommon;
using JwShapeCommon.Jwbase;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Point = NetTopologySuite.Geometries.Point;

static class Program
{
    static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var a = ParseArgs(args);
        if (!a.ContainsKey("db") || !a.ContainsKey("project") || !a.ContainsKey("floor"))
        {
            Console.WriteLine("usage: JwRegressionCheck --db <jwdata.db> --project <工事名> --floor <階名> [--jww <file>] [--csv <expected.csv>] [--appconfig <appconfig.json>]");
            return 2;
        }
        return Run(a).GetAwaiter().GetResult();
    }

    static async Task<int> Run(Dictionary<string, string> a)
    {
        int failures = 0;
        void Result(string name, bool ok, string detail = "") { Console.WriteLine($"[{(ok ? "一致" : "差分")}] {name}{(detail == "" ? "" : "  " + detail)}"); if (!ok) failures++; }

        // DB はコピーを開く
        var dbCopy = Path.Combine(Path.GetTempPath(), $"jwreg_{Guid.NewGuid():N}.db");
        File.Copy(a["db"], dbCopy);
        try
        {
            using var ctx = new JwDataContext(new DbContextOptionsBuilder<JwDataContext>()
                .UseSqlite($"Data Source={dbCopy}", x => x.UseNetTopologySuite()).Options);
            var svc = new JwProjectMainService(ctx);

            var main = ctx.JwProjectMainDatas.Include(m => m.JwCustomerData).SingleOrDefault(m => m.ProjectName == a["project"]);
            if (main == null) { Console.WriteLine($"工事「{a["project"]}」が DB にありません"); return 2; }
            var sub = await svc.FindSubData(s => s.FloorName == a["floor"] && s.JwProjectMainDataId == main.Id);
            if (sub == null) { Console.WriteLine($"階「{a["floor"]}」が工事「{a["project"]}」にありません"); return 2; }
            await svc.LoadSubCollectionAsync(sub);
            foreach (var b in sub.JwBeamDatas) await svc.LoadBeamCollectionAsync(b);
            Console.WriteLine($"DB: 工事「{main.ProjectName}」 階「{sub.FloorName}」 梁 {sub.JwBeamDatas.Count} 本（解析日時 {sub.CreationTime:yyyy-MM-dd HH:mm}）");

            ApplyCsvSettings(a.GetValueOrDefault("appconfig"));
            JwFileConsts.JwScale = sub.Scale; // CSV の穴位置計算は静的な JwScale を使う

            byte[]? expected = a.ContainsKey("csv") ? File.ReadAllBytes(a["csv"]) : null;

            // [1] DB → CSV
            var csvFromDb = EncodeLikeExport(sub.DataToCanvas().ToProcessCsv());
            if (expected != null) CompareCsv("[1] DB の階データ → CSV  vs  既知の CSV", expected, csvFromDb, Result);

            if (a.ContainsKey("jww"))
            {
                // [2] jww → 解析 → DB と比較
                var cust = main.JwCustomerDataId == null ? null
                    : ctx.JwCustDesignConstDatas.SingleOrDefault(c => c.JwCustomerDataId == main.JwCustomerDataId);
                if (cust == null) { Console.WriteLine("顧客の解析設定が DB にありません"); return 2; }
                ApplyParseSettings(cust, sub);
                ApplyParseConfig(a.GetValueOrDefault("appconfig"));

                int repeat = a.TryGetValue("repeat", out var rs) ? int.Parse(rs) : 1;
                for (int run = 1; run <= repeat; run++)
                {
                if (repeat > 1) Console.WriteLine($"--- 解析 {run} / {repeat} 回目");
                var model = new JwProjectPathModel { Path = a["jww"], MainData = main, FloorName = sub.FloorName };
                var h = new JwFileHandle(model);
                h.ReadJwFile();
                h.FollowTheStep();
                h.CreateCanvas();   // 画面と同じ順序：CreateData は CreateCanvas で求めた範囲（TopLeft 等）を使う
                h.CreateData();
                Console.WriteLine($"解析: 梁 {h._beamdatas.Count} 本 / 穴 {h._holeDatas.Count} / 柱 {h._beampillarDatas.Count} / 部材 {h._linkPartDatas.Count} / 連結 {h._lianjieDatas.Count}");

                // まず完全一致で比較し、差分があれば「梁番号の英字サフィックスを伏せた」比較もする。
                // 同一形状の梁グループ内での A/B/C… の割当ては、梁の並び順（並列処理の結果）に依存し、元のコードでも実行ごとに変わる。
                var exact = CompareEntities(sub, h, normalizeSuffix: false);
                if (exact.All(r => r.ok)) foreach (var r in exact) Result(r.name, true, "");
                else
                {
                    var norm = CompareEntities(sub, h, normalizeSuffix: true);
                    foreach (var (r, n) in exact.Zip(norm))
                    {
                        if (r.ok) Result(r.name, true, "");
                        else if (n.ok) { Console.WriteLine($"[参考] {r.name}：{r.detail}"); Result(r.name + "（梁番号の英字サフィックスを除く）", true, "← 同一形状グループ内のサフィックス割当てのみ相違"); }
                        else { Result(n.name, false, n.detail); foreach (var s in n.samples) Console.WriteLine("      " + s); }
                    }
                    if (!norm[0].ok) PrintBeamFieldDiffs(sub, h);
                }

                // [3] 解析結果 → CSV（メモリ上で DB と同じ形に組み立てる）
                if (expected != null)
                {
                    var s2 = h._subData!;
                    s2.DefaultBeamXHName = sub.DefaultBeamXHName; // 保存時に材料から設定される値（解析とは無関係）
                    foreach (var b in h._beamdatas)
                    {
                        foreach (var x in h._holeDatas.Where(x => x.JwBeamDataId == b.Id)) b.JwHoles.Add(x);
                        foreach (var x in h._jwbvdatas.Where(x => x.JwBeamDataId == b.Id)) b.JwBeamVerticalDatas.Add(x);
                        s2.JwBeamDatas.Add(b);
                    }
                    foreach (var x in h._beampillarDatas) s2.JwPillarDatas.Add(x);
                    foreach (var x in h._linkPartDatas) s2.JwLinkPartDatas.Add(x);
                    foreach (var x in h._lianjieDatas) s2.JwLianjieDatas.Add(x);
                    foreach (var x in h._downPillarDatas) s2.JwDownPillarDatas.Add(x);
                    foreach (var x in h._cuttingDatas) s2.JwCuttings.Add(x);
                    foreach (var x in h._addMarkDatas) s2.JwAddedHoles.Add(x);
                    JwFileConsts.JwScale = sub.Scale;
                    var csvFromParse = EncodeLikeExport(s2.DataToCanvas().ToProcessCsv());
                    CompareCsv("[3] jww → 解析 → CSV  vs  既知の CSV", expected, csvFromParse, Result);
                }
                } // repeat
            }
        }
        finally
        {
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            try { File.Delete(dbCopy); } catch { }
        }
        Console.WriteLine(failures == 0 ? "結果: すべて一致" : $"結果: 差分 {failures} 件");
        return failures == 0 ? 0 : 1;
    }

    // ---------- 設定の復元 ----------

    /// <summary>CustomerDesignSetting の「確定」と同じ値を JwFileConsts に設定する（保存されていない項目は画面の既定値）。</summary>
    static void ApplyParseSettings(JwCustDesignConstData c, JwProjectSubData sub)
    {
        var colors = JwFileConsts.GetJwColors();
        var styles = JwFileConsts.GetJwPenStyles();
        JwColor Col(int n) => colors.Single(x => x.ColorNumber == n);
        JwFileConsts.BeamParseColor = Col(c.BeamParseColorNumber);
        JwFileConsts.BeamSymbolTextColor = Col(c.BeamSymbolTextColorNumber);
        JwFileConsts.BeamSplitParseColor = Col(c.BeamSplitParseColor);
        JwFileConsts.BeamPillarParseColor = Col(c.BeamPillarParseColor);
        JwFileConsts.PillarPenStyle = styles.Single(x => x.StyleNumber == c.PillarPenStyle);
        JwFileConsts.SplitPenStyle = styles.Single(x => x.StyleNumber == c.SplitPenStyle);
        JwFileConsts.BRParseColore = c.DownPillarColorNumber;          // 画面では「下階柱」の色を BR 色に使う
        JwFileConsts.LianjieParseColor = Col(c.LianjieColorNumber);
        JwFileConsts.NearSpliteMax = c.NearSpliteMax;
        JwFileConsts.LianjieParsingMethod = c.LianjieParsingMethod;
        JwFileConsts.PillarDrawingMethod = c.PillarDrawingMethod;
        JwFileConsts.SpliteDrawingMethod = c.SplitDrawingMethod;
        JwFileConsts.LjCompentType = sub.LianjieCompentType;          // 解析時の値は階データに保存されている
        JwFileConsts.Ktype = sub.KPillarType;
        JwFileConsts.MinimalStrategy = sub.MinimalStrategy;
        JwFileConsts.EWaiKongColor = colors[0];                         // 画面の既定値（一覧の先頭）
    }

    /// <summary>
    /// 「退化矩形を除外」は appconfig.json の Parse 設定に従う（指定が無ければ既定＝無効）。
    /// 旧バージョンのコードにはこの設定が無いため、存在する場合のみ反射で設定する。
    /// </summary>
    static void ApplyParseConfig(string? appconfig)
    {
        bool exclude = false;
        if (appconfig != null && File.Exists(appconfig))
        {
            using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(appconfig));
            if (doc.RootElement.TryGetProperty("Parse", out var p) && p.TryGetProperty("ExcludeDegenerateRectangles", out var v))
                exclude = v.GetBoolean();
        }
        var f = typeof(JwFileConsts).GetField("ExcludeDegenerateRectangles", BindingFlags.Public | BindingFlags.Static);
        if (f != null) f.SetValue(null, exclude);
        Console.WriteLine($"解析設定: 退化矩形を除外 = {(f == null ? "（このバージョンには無い設定）" : exclude.ToString())}");
    }

    static void ApplyCsvSettings(string? appconfig)
    {
        if (appconfig == null || !File.Exists(appconfig)) { Console.WriteLine("（appconfig 未指定：CSV 設定は既定値）"); return; }
        using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(appconfig));
        if (!doc.RootElement.TryGetProperty("Csv", out var csv)) return;
        double D(string n) => csv.TryGetProperty(n, out var v) ? v.GetDouble() : 0;
        JwFileConsts.CsvHxNum = (int)D("Hxnum"); JwFileConsts.CsvHxJianju = D("Hxjianju");
        JwFileConsts.CsvZxNum = (int)D("Zxnum"); JwFileConsts.CsvZxJianju = D("Zxjianju");
        JwFileConsts.EllipseDiameter = D("Kongjing"); JwFileConsts.CsvYTiaozheng = D("Ytiaozheng");
        Console.WriteLine($"CSV 設定: 穴径 {JwFileConsts.EllipseDiameter}  Y調整 {JwFileConsts.CsvYTiaozheng}");
    }

    /// <summary>NewProjectMainPage.ExportCsv と同じ書き出し（Shift-JIS）。</summary>
    static byte[] EncodeLikeExport(string csv) => Encoding.GetEncoding("Shift-JIS").GetBytes(csv);

    // ---------- 内容比較 ----------

    static readonly HashSet<string> SubIgnore = new() { "FloorName", "DefaultBeamXHId", "DefaultBeamXHName", "ExportCount" };
    /// <summary>保存時（SaveNewParseResult）に材料から設定される値。解析結果ではないので比較しない</summary>
    static readonly HashSet<string> BeamIgnore = new() { "BeamXHId", "BeamXHName" };

    /// <summary>
    /// エンティティの内容を表す文字列（Id・作成日時・外部キーは除外、梁への参照は参照先の梁の内容に置き換える）。
    /// </summary>
    static string Key(object e, Dictionary<string, string>? beamKeys, HashSet<string>? ignore = null)
    {
        var sb = new StringBuilder(e.GetType().Name);
        foreach (var p in e.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name, StringComparer.Ordinal))
        {
            if (!p.CanRead || p.GetIndexParameters().Length > 0) continue;
            if (p.Name is "Id" or "CreationTime" || ignore?.Contains(p.Name) == true) continue;
            var t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
            var v = p.GetValue(e);
            string? s;
            if (p.Name == "BeamCode" && NormalizeSuffix)
                s = NormCode(v as string);
            else if (p.Name is "JwBeamDataId" or "BeamId")
                s = v is string id && beamKeys != null && beamKeys.TryGetValue(id, out var bk) ? "beam#" + Hash(bk) : (v == null || (string)v == "" ? "" : "beam#?");
            else if (p.Name.EndsWith("Id")) continue;                         // その他の外部キー
            else if (typeof(Geometry).IsAssignableFrom(t)) s = v is Point pt ? $"{pt.X.ToString("R", CultureInfo.InvariantCulture)},{pt.Y.ToString("R", CultureInfo.InvariantCulture)}" : v?.ToString();
            else if (t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(decimal))
                s = v is IFormattable f ? f.ToString(t == typeof(double) || t == typeof(float) ? "R" : null, CultureInfo.InvariantCulture) : v?.ToString();
            else continue;                                                      // ナビゲーション・コレクション
            sb.Append('|').Append(p.Name).Append('=').Append(s);
        }
        return sb.ToString();
    }

    static string Hash(string s) => Convert.ToHexString(System.Security.Cryptography.SHA1.HashData(Encoding.UTF8.GetBytes(s)))[..10];

    /// <summary>
    /// 梁を位置（中心点・方向）で対応付け、値の違う項目を表示する（差分の原因調査用）。
    /// </summary>
    static void PrintBeamFieldDiffs(JwProjectSubData sub, JwFileHandle h)
    {
        string Pos(JwBeamData b) => $"{b.DirectionType}@{b.CenterPoint?.X:0.###},{b.CenterPoint?.Y:0.###}";
        var dbByPos = sub.JwBeamDatas.GroupBy(Pos).ToDictionary(g => g.Key, g => g.ToList());
        int shown = 0;
        foreach (var nb in h._beamdatas)
        {
            if (!dbByPos.TryGetValue(Pos(nb), out var cands) || cands.Count != 1) continue;
            var db = cands[0];
            var diffs = new List<string>();
            foreach (var p in typeof(JwBeamData).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (p.Name is "Id" or "CreationTime" || p.Name.EndsWith("Id") || BeamIgnore.Contains(p.Name)) continue;
                var t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                if (!(t.IsPrimitive || t.IsEnum || t == typeof(string) || typeof(Geometry).IsAssignableFrom(t))) continue;
                string F(object? v) => v is Point pt ? $"{pt.X:R},{pt.Y:R}" : v is IFormattable f ? f.ToString(v is double ? "R" : null, CultureInfo.InvariantCulture) : v?.ToString() ?? "";
                var (x, y) = (F(p.GetValue(db)), F(p.GetValue(nb)));
                if (x != y) diffs.Add($"{p.Name}: DB={x} / 今回={y}");
            }
            int dh = db.JwHoles.Count, nh = h._holeDatas.Count(x => x.JwBeamDataId == nb.Id);
            if (dh != nh) diffs.Add($"穴の数: DB={dh} / 今回={nh}");
            if (diffs.Count == 0) continue;
            if (shown++ >= 6) break;
            Console.WriteLine($"    梁 {Pos(nb)}（DB {db.BeamCode} / 今回 {nb.BeamCode}）");
            foreach (var d in diffs) Console.WriteLine("        " + (d.Length > 220 ? d[..220] + "…" : d));
        }
    }

    static bool NormalizeSuffix;

    /// <summary>梁番号末尾の英字（同一グループ内のサフィックス）を伏せる。InitialBeamCode は別項目として比較される。</summary>
    static string NormCode(string? code) => code == null ? "" : code.TrimEnd("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()) + "*";

    static List<(string name, bool ok, string detail, List<string> samples)> CompareEntities(JwProjectSubData sub, JwFileHandle h, bool normalizeSuffix)
    {
        NormalizeSuffix = normalizeSuffix;
        var res = new List<(string, bool, string, List<string>)>();
        var beamKeyNew = h._beamdatas.ToDictionary(b => b.Id, b => Key(b, null, BeamIgnore));
        var beamKeyDb = sub.JwBeamDatas.ToDictionary(b => b.Id, b => Key(b, null, BeamIgnore));
        void C(string name, IEnumerable<string> db, IEnumerable<string> now) => res.Add(Compare(name, db, now));
        C("[2] 梁", sub.JwBeamDatas.Select(b => beamKeyDb[b.Id]), h._beamdatas.Select(b => beamKeyNew[b.Id]));
        C("[2] 穴", sub.JwBeamDatas.SelectMany(b => b.JwHoles).Select(x => Key(x, beamKeyDb)), h._holeDatas.Select(x => Key(x, beamKeyNew)));
        C("[2] 梁の接触情報", sub.JwBeamDatas.SelectMany(b => b.JwBeamVerticalDatas).Select(x => Key(x, beamKeyDb)), h._jwbvdatas.Select(x => Key(x, beamKeyNew)));
        C("[2] 柱", sub.JwPillarDatas.Select(x => Key(x, beamKeyDb)), h._beampillarDatas.Select(x => Key(x, beamKeyNew)));
        C("[2] 部材(B/BG)", sub.JwLinkPartDatas.Select(x => Key(x, beamKeyDb)), h._linkPartDatas.Select(x => Key(x, beamKeyNew)));
        C("[2] 連結", sub.JwLianjieDatas.Select(x => Key(x, beamKeyDb)), h._lianjieDatas.Select(x => Key(x, beamKeyNew)));
        C("[2] 下階柱", sub.JwDownPillarDatas.Select(x => Key(x, beamKeyDb)), h._downPillarDatas.Select(x => Key(x, beamKeyNew)));
        C("[2] 切断", sub.JwCuttings.Select(x => Key(x, beamKeyDb)), h._cuttingDatas.Select(x => Key(x, beamKeyNew)));
        C("[2] 追加穴マーク", sub.JwAddedHoles.Select(x => Key(x, beamKeyDb)), h._addMarkDatas.Select(x => Key(x, beamKeyNew)));
        C("[2] 階の集計値", new[] { Key(sub, null, SubIgnore) }, new[] { Key(h._subData!, null, SubIgnore) });
        NormalizeSuffix = false;
        return res;
    }

    static (string name, bool ok, string detail, List<string> samples) Compare(string name, IEnumerable<string> db, IEnumerable<string> now)
    {
        var a = db.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        var b = now.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        var onlyDb = a.SelectMany(kv => Enumerable.Repeat(kv.Key, Math.Max(0, kv.Value - b.GetValueOrDefault(kv.Key)))).ToList();
        var onlyNow = b.SelectMany(kv => Enumerable.Repeat(kv.Key, Math.Max(0, kv.Value - a.GetValueOrDefault(kv.Key)))).ToList();
        var samples = onlyDb.Take(3).Select(x => "DB のみ : " + Shorten(x)).Concat(onlyNow.Take(3).Select(x => "今回のみ: " + Shorten(x))).ToList();
        return ($"{name}（DB {a.Values.Sum()} 件 / 今回 {b.Values.Sum()} 件）", onlyDb.Count == 0 && onlyNow.Count == 0,
                onlyDb.Count + onlyNow.Count == 0 ? "" : $"DB のみ {onlyDb.Count} 件・今回のみ {onlyNow.Count} 件", samples);
    }

    static string Shorten(string s) => s.Length > 300 ? s[..300] + "…" : s;

    /// <summary>
    /// CSV の比較。バイト完全一致を第一とし、一致しない場合は「製品（START～END）単位で、並び順と製品番号を無視した一致」も判定する。
    /// 解析時の梁の並び順（＝製品番号の割り当て）は元のコードでも並列処理のため実行ごとに変わり得るので、
    /// 後者が一致していれば「内容は同じで順序だけ違う」と判断できる。
    /// </summary>
    static void CompareCsv(string name, byte[] expected, byte[] actual, Action<string, bool, string> result)
    {
        if (expected.SequenceEqual(actual)) { result(name, true, $"（バイト完全一致 {expected.Length} バイト）"); return; }
        var sj = Encoding.GetEncoding("Shift-JIS");
        var be = Blocks(sj.GetString(expected)); var ba = Blocks(sj.GetString(actual));
        bool sameSet = be.OrderBy(x => x, StringComparer.Ordinal).SequenceEqual(ba.OrderBy(x => x, StringComparer.Ordinal));
        if (sameSet)
        {
            Console.WriteLine($"[参考] {name}：バイト不一致だが、製品 {be.Count} 件の内容は並び順を除き完全一致（梁の並び順・製品番号のみ相違）");
            result(name + "（並び順を無視）", true, "");
            return;
        }
        // 梁番号（3 列目）の英字サフィックスも伏せて比較
        static string MaskCode(string block)
        {
            var nl = block.IndexOf("\r\n", StringComparison.Ordinal);
            var head = nl < 0 ? block : block[..nl];
            var f = head.Split(',');
            if (f.Length > 2) f[2] = NormCode(f[2]);
            return string.Join(",", f) + (nl < 0 ? "" : block[nl..]);
        }
        var me = be.Select(MaskCode).OrderBy(x => x, StringComparer.Ordinal).ToList();
        var ma = ba.Select(MaskCode).OrderBy(x => x, StringComparer.Ordinal).ToList();
        if (me.SequenceEqual(ma))
        {
            Console.WriteLine($"[参考] {name}：製品 {be.Count} 件の形状・穴はすべて一致。同一形状グループ内の梁番号サフィックス（A/B/C…）の割当てのみ相違");
            result(name + "（梁番号の英字サフィックスを除く）", true, "");
            return;
        }
        var onlyE = be.Except(ba).ToList(); var onlyA = ba.Except(be).ToList();
        result(name, false, $"製品 {be.Count} / {ba.Count} 件、既知のみ {onlyE.Count} 件・今回のみ {onlyA.Count} 件\n      " + CsvDiff(expected, actual));
        foreach (var x in onlyE.Take(2)) Console.WriteLine("      既知のみ: " + Shorten(x.Replace("\r\n", " ⏎ ")));
        foreach (var x in onlyA.Take(2)) Console.WriteLine("      今回のみ: " + Shorten(x.Replace("\r\n", " ⏎ ")));
    }

    /// <summary>CSV を製品ブロックに分割し、製品番号（1 列目）を伏せる。</summary>
    static List<string> Blocks(string csv)
    {
        var res = new List<string>();
        foreach (var part in csv.Split("START\r\n").Skip(1))
        {
            var lines = part.Split("\r\n").ToList();
            if (lines.Count > 0 && lines[0].Contains(',')) lines[0] = "#" + lines[0][lines[0].IndexOf(',')..];
            res.Add(string.Join("\r\n", lines));
        }
        return res;
    }

    static string CsvDiff(byte[] expected, byte[] actual)
    {
        if (expected.SequenceEqual(actual)) return $"（{expected.Length} バイト）";
        var sj = Encoding.GetEncoding("Shift-JIS");
        var e = sj.GetString(expected).Split("\r\n"); var a = sj.GetString(actual).Split("\r\n");
        int n = Math.Min(e.Length, a.Length), i = 0;
        while (i < n && e[i] == a[i]) i++;
        return $"行数 {e.Length} / {a.Length}、最初の相違 {i + 1} 行目:\n      既知: {(i < e.Length ? e[i] : "(なし)")}\n      今回: {(i < a.Length ? a[i] : "(なし)")}";
    }

    static Dictionary<string, string> ParseArgs(string[] args)
    {
        var d = new Dictionary<string, string>();
        for (int i = 0; i + 1 < args.Length; i += 2) d[args[i].TrimStart('-')] = args[i + 1];
        return d;
    }
}
