using RGBControls.Classes;

namespace RGBJWMain.Forms
{
    public partial class NewJwBeamForm
    {
        /// <summary>
        /// 2026年11月22日 設計サイズ 1561×906（AutoScaleMode=Font）は表示スケール 125% 以上だと 1080p の画面からはみ出すため、
        /// 読み込み後に作業領域へ収める（既存の Load 処理は base.OnLoad の中で先に実行される）。
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FormLayoutHelper.FitToWorkingArea(this);
        }
    }
}
