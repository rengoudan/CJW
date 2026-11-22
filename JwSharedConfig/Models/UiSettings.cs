namespace JwSharedConfig.Models
{
    /// <summary>
    /// 2026年11月22日 画面操作に関する本機の設定（appconfig.json に保存。データベースとは無関係）。
    /// </summary>
    public class UiSettings
    {
        /// <summary>出力の種類ごとに最後に選んだ保存先フォルダ（キー：出力の種類）。</summary>
        public Dictionary<string, string> ExportFolders { get; set; } = new();

        /// <summary>2026年11月23日 図の表示モード・レイヤー（キー：画面の種類 parse／floor／canvas）。</summary>
        public Dictionary<string, string> CanvasViews { get; set; } = new();
    }
}
