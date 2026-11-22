using AntdUI;
using JwCore;
using RGBControls.Controls;
using RGBControls.Forms;

namespace RGBControls.Pages
{
    /// <summary>
    /// 2026年11月22日 工事・階一覧の操作の追加（表示・画面遷移のみ。データは変えない）。
    /// </summary>
    public partial class NewProjectMainPage
    {
        /// <summary>
        /// 階の画面（梁・柱・連結の一覧と図）を開く。needCheck なら要確認の梁だけを表示し、最初の 1 本を図上で強調する。
        /// 原来的双击处理：先加载楼层的梁和孔，再用 Sub + ShowSubForm 最大化显示（不变）。
        /// </summary>
        private async Task OpenFloorViewAsync(JwProjectSubData sub, bool needCheck)
        {
            await JwProjectMainService.LoadSubCollectionAsync(sub);
            foreach (var bd in sub.JwBeamDatas)
                await JwProjectMainService.LoadBeamCollectionAsync(bd);

            var view = new Sub(sub) { StartWithNeedCheck = needCheck };
            using var form = new ShowSubForm(sub, view) { WindowState = FormWindowState.Maximized };
            form.ShowDialog();
        }

        /// <summary>状態列の「要確認」をクリック → その階の要確認の梁を表示。</summary>
        private async Task<bool> TryOpenNeedCheckFromStatusCellAsync(TableClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left || e.RowIndex <= 0) return false;
            if (e.Column?.Key != "ExportCount") return false; // 状態列（NewProjectMainPage.Status.cs）
            if (table1[e.RowIndex - 1].record is not JwProjectSubData sub) return false;
            if (!_floorChecks.TryGetValue(sub.Id, out var c) || !c.NeedsCheck) return false;
            await OpenFloorViewAsync(sub, needCheck: true);
            return true;
        }
    }
}
