using JwCore;
using JwShapeCommon;

namespace RGBJWMain.Controls
{
    /// <summary>
    /// 2026年11月23日 梁の色（工区・CSV 出力の有無）を、梁画面で変更したらすぐ図に反映する。
    /// 原因：梁の色は図を作るとき（JwDrawShape.Draw）に 1 回だけ ControlDraw.PenColor に写し、その後は描き直しても使い回していた。
    ///       さらに「梁一覧の更新」通知（RefreshShowBeamsEvent）は階画面（Sub）しか受けておらず、解析画面などの図は描き直されなかった。
    /// 対応：描き直すときに各梁の現在の状態から色を取り直す（配置・拡大率は変えない）。図そのものが通知を受ける。
    /// </summary>
    public partial class JwShowBeams
    {
        /// <summary>梁の色を、梁の現在の工区・CSV 出力の設定から取り直す（JwDrawShape.Draw と同じ GetBeamColor）。</summary>
        private void UpdateBeamColors()
        {
            if (_bounds == null) return;
            foreach (var b in _bounds)
            {
                if (b.ShapeType == DrawShapeType.Beam && b.JwSquareBase is JwBeam beam)
                    // 2026年11月23日 工区の色は「工区」表示（ShowColor）のときだけ。それ以外は白（CSV 出力なしは灰色）
                    b.PenColor = _showcolor ? beam.GetBeamColor() : (beam.HasCsv ? Color.White : Color.Gray);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            GlobalEvent.GetGlobalEvent().RefreshShowBeamsEvent -= GlobalEvent_RefreshShowBeamsEvent; // 二重登録しない
            GlobalEvent.GetGlobalEvent().RefreshShowBeamsEvent += GlobalEvent_RefreshShowBeamsEvent;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            GlobalEvent.GetGlobalEvent().RefreshShowBeamsEvent -= GlobalEvent_RefreshShowBeamsEvent;
            base.OnHandleDestroyed(e);
        }
    }
}
