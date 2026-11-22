using JwServices;
using Sunny.UI;

namespace RGBControls.Pages
{
    /// <summary>
    /// 2026年11月22日 工事の「階数を変更」（右クリック）。
    /// 階数は取り込める階の上限。取り込み済みの階数より小さくはできない（1 以上）。
    /// </summary>
    public partial class NewProjectMainPage
    {
        private async Task ChangeFloorQuantityAsync()
        {
            if (_selectedMainData == null) return;
            var (parsed, planned) = await JwProjectMainService.GetFloorCapacityAsync(_selectedMainData.Id);
            int min = Math.Max(1, parsed);

            var option = new UIEditOption { AutoLabelWidth = true, Text = $"階数を変更　{_selectedMainData.ProjectName}" };
            option.AddInteger("FloorQuantity", $"階数（{min} 以上）", Math.Max(planned, min));
            var frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += (s, e) =>
            {
                if (!int.TryParse(e.Form["FloorQuantity"]?.ToString(), out var v) || v < min)
                {
                    e.Form.SetEditorFocus("FloorQuantity");
                    ShowWarningTip($"階数は {min} 以上にしてください（取り込み済み {parsed} 階）");
                    return false;
                }
                return true;
            };
            frm.ShowDialog();
            if (!frm.IsOK) return;

            int value = Convert.ToInt32(frm["FloorQuantity"].ToString());
            if (value == planned) return;
            try
            {
                await JwProjectMainService.ChangeFloorQuantityAsync(_selectedMainData.Id, value);
                JwLogging.AppLog.Info("階数を変更 {Project}: {Old} → {New}", _selectedMainData.ProjectName, planned, value);
                await ReloadData();
                ShowSuccessTip($"階数を {value} 階に変更しました");
            }
            catch (FloorLimitException ex)
            {
                UIMessageBox.ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                RGBControls.Errors.ErrorReporter.Report(ex, "階数の変更に失敗しました。");
            }
        }
    }
}
