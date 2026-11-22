namespace RGBControls.Classes
{
    /// <summary>
    /// 2026年11月22日 画面サイズへの収まり調整（1920×1080、表示スケール 125%/150% でも窓が画面外にはみ出さないように）。
    /// 最大化表示の窓は対象外。業務ロジックには関与しない。
    /// </summary>
    internal static class FormLayoutHelper
    {
        /// <summary>
        /// 窓が作業領域（タスクバーを除く画面）より大きい場合は縮め、作業領域の中に収まる位置へ移動する。
        /// 縮めたときに中身が絶対配置で収まらない窓は、allowScroll=true でスクロール可能にする。
        /// </summary>
        public static void FitToWorkingArea(Form form, bool allowScroll = false)
        {
            if (form.WindowState == FormWindowState.Maximized) return;
            var screen = form.Owner != null ? Screen.FromControl(form.Owner) : Screen.FromPoint(Cursor.Position);
            var wa = screen.WorkingArea;

            bool shrunk = false;
            int w = form.Width, h = form.Height;
            if (w > wa.Width) { w = wa.Width; shrunk = true; }
            if (h > wa.Height) { h = wa.Height; shrunk = true; }
            if (shrunk)
            {
                form.Size = new Size(w, h);
                if (allowScroll) form.AutoScroll = true;
                // 縮めた場合は作業領域の中央に置き直す
                form.Location = new Point(wa.Left + (wa.Width - w) / 2, wa.Top + (wa.Height - h) / 2);
                JwLogging.AppLog.Debug("窓サイズを画面に合わせて調整 {Form} → {W}x{H}（作業領域 {WA}）", form.GetType().Name, w, h, wa);
                return;
            }

            // サイズは収まっているが位置がはみ出している場合は内側へ寄せる
            int x = Math.Min(Math.Max(form.Left, wa.Left), wa.Right - w);
            int y = Math.Min(Math.Max(form.Top, wa.Top), wa.Bottom - h);
            if (x != form.Left || y != form.Top) form.Location = new Point(x, y);
        }
    }
}
