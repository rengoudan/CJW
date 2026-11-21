using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using JwLogging;

namespace RGBControls.Errors
{
    /// <summary>
    /// 错误对话框和日志页共用：打开日志目录、保存支持用 zip。
    /// </summary>
    internal static class SupportTools
    {
        public static void OpenLogFolder(IWin32Window owner)
        {
            try
            {
                Directory.CreateDirectory(AppLog.LogDir);
                Process.Start(new ProcessStartInfo("explorer.exe", $"\"{AppLog.LogDir}\"") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                AppLog.Warn(ex, "ログフォルダを開けませんでした");
                MessageBox.Show(owner, $"ログフォルダを開けませんでした。\n{AppLog.LogDir}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 打包最近 7 天日志（及可选的错误详情 error.txt）为 zip。日志文件正被写入，因此以共享方式读取。
        /// </summary>
        public static void SaveSupportPackage(IWin32Window owner, string? errorDetails = null)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "ZIPファイル (*.zip)|*.zip",
                FileName = $"rgbjw-log-{DateTime.Now:yyyyMMdd-HHmmss}.zip",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            };
            if (sfd.ShowDialog(owner) != DialogResult.OK) return;

            try
            {
                using (var zip = ZipFile.Open(sfd.FileName, ZipArchiveMode.Create))
                {
                    if (!string.IsNullOrEmpty(errorDetails))
                    {
                        var entry = zip.CreateEntry("error.txt");
                        using var w = new StreamWriter(entry.Open(), new UTF8Encoding(false));
                        w.Write(errorDetails);
                    }

                    foreach (var file in AppLog.GetRecentLogFiles(7))
                    {
                        var logEntry = zip.CreateEntry("logs/" + file.Name);
                        using var src = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                        using var dst = logEntry.Open();
                        src.CopyTo(dst);
                    }
                }
                AppLog.Info("サポート用ログを保存: {Path}", sfd.FileName);
                MessageBox.Show(owner, $"保存しました。\n{sfd.FileName}", "サポート用に保存",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppLog.Warn(ex, "サポート用ログの保存に失敗");
                MessageBox.Show(owner, $"保存に失敗しました。\n{ex.Message}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
