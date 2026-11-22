using JwCore;
using JwServices;
using JwShapeCommon;

namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月22日 梁製造 CSV を出力できたときに、その階の ExportCount を +1 して一覧を更新する。
    /// CSV の内容・出力処理には触れない（ファイル書き込みが成功した後に呼ぶだけ）。
    /// 記録に失敗しても CSV 出力自体は成功しているので、ログに残すだけで利用者には出さない。
    /// </summary>
    internal static class CsvExportRecorder
    {
        public static async Task RecordAsync(JwProjectSubData? sub)
        {
            if (sub == null || string.IsNullOrEmpty(sub.Id)) return;
            try
            {
                var service = ServiceFactory.GetInstance().CreateJwProjectMainService();
                int n = await service.IncrementExportCountAsync(sub.Id);
                if (n >= 0) sub.ExportCount = n;
                JwLogging.AppLog.Info("CSV出力を記録 floor={Floor} count={Count}", sub.FloorName, n);
                GlobalEvent.GetGlobalEvent().RefreshDataEvent?.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                JwLogging.AppLog.Warn(ex, "CSV出力回数の記録に失敗 floor={Floor}", sub.FloorName);
            }
        }
    }
}
