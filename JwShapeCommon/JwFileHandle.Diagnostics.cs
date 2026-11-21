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
        /// 执行一个解析步骤：记录耗时；失败时包装成 ParseStepException 抛出。
        /// </summary>
        private void Step(string stepName, string methodName, Action action)
        {
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
                throw new ParseStepException(stepName, methodName, ex);
            }
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
            if (SuppressedErrors.Count == 0) return;
            var summary = string.Join("、", SuppressedErrors.Select(kv => $"{kv.Key} {kv.Value}件"));
            AppLog.Warn("解析中に回避された例外の集計: {Summary} file={File}", summary, LogFileName);
            SendMsg($"【注意】解析中に一部の処理が失敗しました（{summary}）。結果を確認してください。");
        }
    }
}
