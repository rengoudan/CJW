namespace JwShapeCommon
{
    public enum ParseStepStatus { Running, Done, Warning, Failed }

    /// <summary>
    /// 2026年11月22日 jww 解析的进度通知（每个步骤开始/结束各一次）。
    /// 通过 JwFileHandle.Progress（IProgress）发给发起解析的窗体，不经过全局事件，避免串到其他窗体。
    /// </summary>
    public sealed class ParseProgress
    {
        /// <summary>步骤序号（0 起）。与 JwFileHandle.ParseStepNames 的下标一致</summary>
        public int Index { get; init; }
        public int Total { get; init; }
        /// <summary>面向用户的步骤名（日文）</summary>
        public string StepName { get; init; } = "";
        public ParseStepStatus Status { get; init; }
        /// <summary>该步骤耗时（Running 时为 0）</summary>
        public long ElapsedMs { get; init; }
        /// <summary>结果摘要，如「梁 440本」。可为空</summary>
        public string? Summary { get; init; }
        /// <summary>失败时的异常</summary>
        public Exception? Error { get; init; }
    }
}
