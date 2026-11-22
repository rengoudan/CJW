using AntdUI;
using JwCore;
using JwServices;

namespace RGBControls.Pages
{
    /// <summary>
    /// 2026年11月22日 工事・階の「状態」列（表示のみ）。
    /// 工事：未解析 ／ 解析済 n/m階 ／ 全階解析済（要確認の階があれば併記）。解析数列（ParsedQuantity）ではなく実際の階数で数える。
    /// 階：要確認（柱なし・梁なし・穴なし梁） ＞ CSV出力済（n回） ＞ 解析済。
    /// </summary>
    public partial class NewProjectMainPage
    {
        private Dictionary<string, FloorCheck> _floorChecks = new();

        private async Task RefreshFloorChecksAsync()
        {
            try
            {
                var list = await JwProjectMainService.GetFloorChecksAsync();
                _floorChecks = list.ToDictionary(c => c.SubId);
            }
            catch (Exception ex)
            {
                // 集計に失敗しても一覧の表示は続ける（状態列だけ空になる）
                JwLogging.AppLog.Warn(ex, "階の状態集計に失敗");
                _floorChecks = new();
            }
        }

        private object? RenderFloorStatus(object? value, object record, int rowIndex)
        {
            if (record is not JwProjectSubData sub) return null;
            if (_floorChecks.TryGetValue(sub.Id, out var c) && c.NeedsCheck)
                return new CellTag("要確認：" + c.Reason, TTypeMini.Error);
            if (sub.ExportCount > 0)
                return new CellTag($"CSV出力済（{sub.ExportCount}回）", TTypeMini.Success);
            return new CellTag("解析済", TTypeMini.Primary);
        }

        private object? RenderProjectStatus(object? value, object record, int rowIndex)
        {
            if (record is not JwProjectMainData main) return null;
            var floors = _floorChecks.Values.Where(c => c.MainId == main.Id).ToList();
            int parsed = floors.Count;
            int needCheck = floors.Count(c => c.NeedsCheck);
            if (parsed == 0) return new CellTag("未解析", TTypeMini.Default);

            bool complete = main.FloorQuantity > 0 && parsed >= main.FloorQuantity;
            string text = main.FloorQuantity <= 0 ? $"解析済 {parsed}階"
                        : complete ? "全階解析済"
                        : $"解析済 {parsed}/{main.FloorQuantity}階";
            if (needCheck > 0) return new CellTag($"{text}・要確認 {needCheck}階", TTypeMini.Error);
            return new CellTag(text, complete ? TTypeMini.Success : main.FloorQuantity <= 0 ? TTypeMini.Primary : TTypeMini.Warn);
        }
    }
}
