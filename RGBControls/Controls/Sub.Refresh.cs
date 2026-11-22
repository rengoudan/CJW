using JwCore;
using JwShapeCommon;

namespace RGBControls.Controls
{
    /// <summary>
    /// 2026年11月23日 梁画面で工区・CSV 出力を変えたとき、階画面の梁一覧にもすぐ反映する。
    /// 原来只重绘了图，左侧梁一览要重新打开才显示新的工区。
    /// 一覧は値を表示時に読み込むため、データソースを設定し直す（「要確認のみ」の絞り込みと選択中の梁は保つ）。
    /// </summary>
    public partial class Sub
    {
        private void RefreshBeamList()
        {
            if (_subdata == null) return;
            string? selectedId = liangtable.SelectedIndex > 0 && liangtable.DataSource is IList<JwBeamData> before
                                 && liangtable.SelectedIndex - 1 < before.Count
                ? before[liangtable.SelectedIndex - 1].Id
                : null;

            ApplyBeamFilter(); // 梁番号順（「要確認のみ」なら絞り込み）で設定し直す

            if (selectedId != null && liangtable.DataSource is IList<JwBeamData> after)
            {
                int i = after.ToList().FindIndex(b => b.Id == selectedId);
                if (i >= 0) liangtable.SelectedIndex = i + 1; // AntdUI の行番号は見出しが 0
            }
        }

        /// <summary>閉じたら全体通知の購読を解除する（原来从不解除）。</summary>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            GlobalEvent.GetGlobalEvent().RefreshShowBeamsEvent -= GlobalEvent_RefreshShowBeamsEvent;
            base.OnHandleDestroyed(e);
        }
    }
}
