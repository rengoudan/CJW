namespace JwShapeCommon
{
    /// <summary>
    /// jww 解析流水线中某一步失败时抛出，携带步骤名，便于告诉用户"哪一步出错"。
    /// </summary>
    public class ParseStepException : Exception
    {
        /// <summary>面向用户的步骤名（日文），如「柱の識別」</summary>
        public string StepName { get; }

        /// <summary>内部方法名，如 ParseSquareCreatePillar</summary>
        public string MethodName { get; }

        public ParseStepException(string stepName, string methodName, Exception? inner)
            : base($"{stepName}（{methodName}）で失敗しました: {inner?.Message}", inner)
        {
            StepName = stepName;
            MethodName = methodName;
        }

        /// <summary>给用户看的一句话说明</summary>
        public string UserMessage => $"図面解析の「{StepName}」の段階でエラーが発生しました。";
    }
}
