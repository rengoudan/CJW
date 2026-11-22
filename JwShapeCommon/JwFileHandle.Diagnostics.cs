using System.Diagnostics;
using JwLogging;

namespace JwShapeCommon
{
    /// <summary>
    /// JwFileHandle 的诊断部分：解析步骤的执行与计时、被降级处理（吞掉）的异常的记录与汇总。
    /// </summary>
    public partial class JwFileHandle
    {
        /// <summary>
        /// 读取失败的原因（ReadJwFile 失败时设置）。
        /// </summary>
        public Exception? ReadError { get; private set; }

        /// <summary>
        /// 解析过程中被降级处理的异常数量，按类别（日文）统计。
        /// </summary>
        public Dictionary<string, int> SuppressedErrors { get; } = new();

        private string LogFileName => string.IsNullOrEmpty(_path) ? "(no file)" : Path.GetFileName(_path);

        /// <summary>
        /// 2026年11月22日 解析进度的接收方（通常是 FileParseForm）。为 null 时不通知。
        /// 由 UI 线程创建的 Progress&lt;T&gt; 会自动切回 UI 线程，因此解析可以在后台线程执行。
        /// </summary>
        public IProgress<ParseProgress>? Progress { get; set; }

        /// <summary>
        /// 全部解析步骤（显示顺序）。ReadJwFile → FollowTheStep 的各 Step → BuildCanvas。
        /// Step() 中使用的步骤名必须在此列表中（否则序号显示为末尾）。
        /// </summary>
        public static readonly IReadOnlyList<string> ParseStepNames = new[]
        {
            "ファイル読み込み", "梁線の変換", "梁の識別", "柱ソリッドの読み込み", "切断位置の識別", "柱の識別",
            "柱と梁の関係判定", "ブロック帰属の判定", "文字の識別(1)", "文字の識別(2)", "文字の識別(3)", "文字の識別(4)",
            "切断梁の生成", "B/BG数の集計", "追加穴の処理", "下階柱の識別", "連結の識別", "連結間隔の判定", "最終補正",
            "画面の生成",
        };

        private void Report(string stepName, ParseStepStatus status, long ms = 0, string? summary = null, Exception? error = null)
        {
            if (Progress == null) return;
            int idx = ParseStepNames is string[] arr ? Array.IndexOf(arr, stepName) : -1;
            Progress.Report(new ParseProgress
            {
                Index = idx < 0 ? ParseStepNames.Count - 1 : idx,
                Total = ParseStepNames.Count,
                StepName = stepName,
                Status = status,
                ElapsedMs = ms,
                Summary = summary,
                Error = error,
            });
        }

        /// <summary>
        /// 执行一个解析步骤：记录耗时、通知进度；失败时包装成 ParseStepException 抛出。
        /// summary 在步骤成功后调用，返回结果摘要（以「⚠」开头表示需要注意）。
        /// </summary>
        private void Step(string stepName, string methodName, Action action, Func<string?>? summary = null)
        {
            Report(stepName, ParseStepStatus.Running);
            var sw = Stopwatch.StartNew();
            try
            {
                action();
                AppLog.Debug("解析ステップ完了 {Step}({Method}) {Ms}ms file={File}",
                    stepName, methodName, sw.ElapsedMilliseconds, LogFileName);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "解析ステップ失敗 {Step}({Method}) {Ms}ms file={File}",
                    stepName, methodName, sw.ElapsedMilliseconds, LogFileName);
                SendMsg($"【エラー】{stepName} で失敗しました: {ex.Message}");
                Report(stepName, ParseStepStatus.Failed, sw.ElapsedMilliseconds, ex.Message, ex);
                throw new ParseStepException(stepName, methodName, ex);
            }

            string? text = null;
            try { text = summary?.Invoke(); }
            catch (Exception ex) { AppLog.Warn(ex, "解析ステップの集計に失敗 {Step}", stepName); }
            Report(stepName, text != null && text.StartsWith("⚠") ? ParseStepStatus.Warning : ParseStepStatus.Done,
                sw.ElapsedMilliseconds, text);
        }

        /// <summary>
        /// 2026年11月22日 画面（JwCanvas）の生成も 1 ステップとして進度に含める。
        /// </summary>
        public JwCanvas BuildCanvas()
        {
            JwCanvas canvas = null!;
            Step("画面の生成", nameof(CreateCanvas), () => canvas = CreateCanvas(),
                () => HasCanvas ? null : "⚠ 現在の解析設定では有効なデータが検出されません");
            return canvas;
        }

        /// <summary>读取阶段的进度通知（ReadJwFile 中调用）</summary>
        private void ReportReadStart() => Report("ファイル読み込み", ParseStepStatus.Running);

        private void ReportReadEnd(long ms)
        {
            if (!IsSuccessRead)
            {
                var msg = ReadError?.Message ?? "jwファイルを読み込めませんでした";
                Report("ファイル読み込み", ParseStepStatus.Failed, ms, msg, ReadError);
                return;
            }
            var summary = $"線 {SenLst?.Count ?? 0}・ソリッド {SolidLst?.Count ?? 0}・文字 {MojiLst?.Count ?? 0}・ブロック {JWWBlockLst?.Count ?? 0}　縮尺 1/{_scale}";
            AppLog.Info("jww読み込み完了 {Summary} {Ms}ms file={File}", summary, ms, LogFileName);
            Report("ファイル読み込み", ParseStepStatus.Done, ms, summary);
        }

        private string? PillarSummary() => (Pillars?.Count ?? 0) == 0
            ? $"⚠ {JwFileConsts.BeamPillarParseColor?.ColorNumber}番の色では柱を識別できません"
            : $"柱 {Pillars!.Count}本";

        /// <summary>最終補正の結果摘要：終端穴の無い梁・回避された例外があれば注意表示</summary>
        private string? RevisionSummary()
        {
            int missing = Beams?.Count(b => b.MissingEndHole) ?? 0;
            int suppressed = SuppressedErrors.Values.Sum();
            if (missing == 0 && suppressed == 0) return null;
            var parts = new List<string>();
            if (missing > 0) parts.Add($"終端穴なし {missing}本");
            if (suppressed > 0) parts.Add($"回避した例外 {suppressed}件");
            return "⚠ " + string.Join("・", parts);
        }

        /// <summary>
        /// 记录一次被降级处理的异常（原有的"失败则返回默认值"行为保持不变）。
        /// 同一类别只有前 5 次写完整堆栈，之后只计数，避免日志被刷屏。
        /// </summary>
        private void Suppressed(string category, Exception ex, string? detail = null)
        {
            SuppressedErrors.TryGetValue(category, out var n);
            SuppressedErrors[category] = ++n;
            if (n <= 5)
                AppLog.Warn(ex, "解析中の例外を回避 [{Category}] #{Count} {Detail} file={File}",
                    category, n, detail ?? "", LogFileName);
        }

        /// <summary>
        /// 解析结束时输出汇总到解析日志窗口和日志文件。
        /// </summary>
        private void ReportSuppressedSummary()
        {
            // 终点孔缺失的梁：逐根记录位置，便于在图上找到并删除/修正
            var missing = Beams?.Where(b => b.MissingEndHole).ToList() ?? new List<JwBeam>();
            if (missing.Count > 0)
            {
                foreach (var b in missing.Take(20))
                    AppLog.Warn("終端穴が見つからない梁 dir={Dir} TL=({X},{Y}) end={End} file={File}",
                        b.DirectionType, b.TopLeft?.X, b.TopLeft?.Y, b.EndTelosType, LogFileName);
                SendMsg($"【注意】終端穴が見つからない梁が {missing.Count} 本あります（終端の加工は出力されません）。図面の該当箇所を確認してください。");
            }

            if (SuppressedErrors.Count == 0) return;
            var summary = string.Join("、", SuppressedErrors.Select(kv => $"{kv.Key} {kv.Value}件"));
            AppLog.Warn("解析中に回避された例外の集計: {Summary} file={File}", summary, LogFileName);
            SendMsg($"【注意】解析中に一部の処理が失敗しました（{summary}）。結果を確認してください。");
        }
    }
}
