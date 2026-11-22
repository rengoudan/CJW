namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月22日 AntdUI の組み込み文言を日本語にする（表示のみ）。
    /// 原来按系统语言显示：中文系统为「暂无数据」「确定／取消」，日文系统为英文「No data」「OK／Cancel」。
    /// 只处理下面列出的键；其他键返回 null，保持代码里 Localization.Get(key, 既定文言) 指定的文字。
    /// </summary>
    internal sealed class AntdJapanese : AntdUI.ILocalization
    {
        private static readonly Dictionary<string, string> Texts = new()
        {
            ["NoData"] = "データがありません",   // 空の表
            ["OK"] = "OK",                       // Modal の既定ボタン
            ["Cancel"] = "キャンセル",
        };

        public static void Install()
        {
            if (AntdUI.Localization.Provider == null) AntdUI.Localization.Provider = new AntdJapanese();
        }

        public string? GetLocalizedString(string key) => Texts.TryGetValue(key, out var s) ? s : null;
    }
}
