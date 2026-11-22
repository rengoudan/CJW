using JwSharedConfig.Config;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月22日 出力先フォルダを種類ごとに記憶する（本機の appconfig.json。データベースは使わない）。
    /// 原来每次导出都从头选文件夹、CSV 每次都从「ドキュメント」开始。
    /// 只影响对话框的初始位置，导出内容和文件名不变。
    /// </summary>
    internal static class ExportFolders
    {
        public const string BeamJw = "BeamJw";               // 梁JW（梁設計図）
        public const string BanzukeTop = "BanzukeTop";       // 番付図上JW
        public const string BanzukeBottom = "BanzukeBottom"; // 番付図下JW
        public const string Brace = "Brace";                 // ブレース施工図
        public const string BraceExcel = "BraceExcel";       // ブレース寸法 Excel
        public const string Csv = "Csv";                     // 梁製造 CSV
        public const string BeamListExcel = "BeamListExcel"; // 梁統計の梁リスト Excel

        public static string? Get(string key)
        {
            try
            {
                return AppConfigManager.Current.Ui.ExportFolders.TryGetValue(key, out var p) && Directory.Exists(p) ? p : null;
            }
            catch
            {
                return null; // 設定が読めなくても出力は続ける
            }
        }

        public static void Remember(string key, string? folder)
        {
            if (string.IsNullOrEmpty(folder)) return;
            try
            {
                var map = AppConfigManager.Current.Ui.ExportFolders;
                if (map.TryGetValue(key, out var old) && string.Equals(old, folder, StringComparison.OrdinalIgnoreCase)) return;
                map[key] = folder;
                AppConfigManager.Save();
            }
            catch (Exception ex)
            {
                JwLogging.AppLog.Warn(ex, "出力先フォルダの記憶に失敗 {Key}", key);
            }
        }

        /// <summary>前回のフォルダから開き、OK なら記憶する。</summary>
        public static DialogResult Show(FolderBrowserDialog dialog, string key)
        {
            var last = Get(key);
            if (last != null) dialog.SelectedPath = last;
            var r = dialog.ShowDialog();
            if (r == DialogResult.OK) Remember(key, dialog.SelectedPath);
            return r;
        }

        /// <summary>前回のフォルダから開き、OK なら保存先のフォルダを記憶する。</summary>
        public static DialogResult Show(SaveFileDialog dialog, string key)
        {
            var last = Get(key);
            if (last != null) dialog.InitialDirectory = last;
            var r = dialog.ShowDialog();
            if (r == DialogResult.OK) Remember(key, Path.GetDirectoryName(dialog.FileName));
            return r;
        }
    }
}
