using JwShapeCommon.Jwbase;
using RGBControls.Classes;
using RGBControls.Forms;

namespace RGBJWMain.Forms
{
    /// <summary>
    /// 2026年11月22日 「色分布を確認」ボタン（読み取りのみ。設定値・保存方法・解析処理は変えない）。
    /// 図面を開いて解析する流れ（アップロード・再解析）ではその図面を、顧客管理から開いたときは選んだ図面を集計する。
    /// 色の割り当ては、画面で今選んでいる値（未保存でも）で照合する。
    /// </summary>
    public partial class CustomerDesignSetting
    {
        private Sunny.UI.UIButton? _colorPreviewButton;

        private void AddColorPreviewButton()
        {
            if (_colorPreviewButton != null) return;
            _colorPreviewButton = new Sunny.UI.UIButton
            {
                Text = "色分布を確認",
                Font = uiButton1.Font,
                Size = new Size(150, uiButton1.Height),
                Location = new Point(24, uiButton1.Top),
                MinimumSize = uiButton1.MinimumSize,
            };
            new ToolTip().SetToolTip(_colorPreviewButton,
                "図面の線・ソリッド・文字を線色ごとに数え、いま選んでいる色にデータがあるかを確認します（設定は変わりません）");
            _colorPreviewButton.Click += async (s, e) => await ShowColorPreviewAsync();
            uiButton1.Parent.Controls.Add(_colorPreviewButton);
        }

        /// <summary>設定項目名と線色コンボの対応（色分布画面での表示名）。</summary>
        private (string Name, Sunny.UI.UIComboBox Box)[] ColorSettingBoxes() => new[]
        {
            ("梁輪郭線", uiCbBeamcolor),
            ("梁符号の文字", uiCbTextColor),
            ("柱", uiCbpillarcolor),
            ("継手記号", uiCbSplitcolor),
            ("下階柱記号", uidownpillarcolor),
            ("ブレース", uiComboBox4),
            ("追加の穴", uiComboBox8),
        };

        /// <summary>いま画面で選んでいる色（未選択・「線⾊番号」は -1 ＝未設定）。</summary>
        private List<ColorPreviewForm.Assignment> CurrentColorAssignments() =>
            ColorSettingBoxes()
                .Select(x => new ColorPreviewForm.Assignment(x.Name, x.Box.SelectedItem is JwColor c ? c.ColorNumber : -1))
                .ToList();

        /// <summary>色分布画面で割り当てた色を各コンボの選択に反映する（保存はしない。手でコンボを選び直すのと同じ）。</summary>
        private int ApplyColorAssignments(IEnumerable<ColorPreviewForm.Assignment> changes)
        {
            var boxes = ColorSettingBoxes().ToDictionary(x => x.Name, x => x.Box);
            int applied = 0;
            foreach (var ch in changes)
            {
                if (!boxes.TryGetValue(ch.Setting, out var box)) continue;
                box.SelectedValue = ch.Color;
                if (box.SelectedItem is JwColor c && c.ColorNumber == ch.Color)
                {
                    applied++;
                    JwLogging.AppLog.Info("色分布から設定に反映 {Setting} = lc{Color}", ch.Setting, ch.Color);
                }
                else
                {
                    JwLogging.AppLog.Warn("色分布からの反映に失敗 {Setting} = lc{Color}", ch.Setting, ch.Color);
                }
            }
            return applied;
        }

        private async Task ShowColorPreviewAsync()
        {
            string path = _path;
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                using var ofd = new OpenFileDialog { Filter = "Jw_cad 図面 (*.jww)|*.jww", Title = "色分布を確認する図面を選択" };
                if (ofd.ShowDialog(this) != DialogResult.OK) return;
                path = ofd.FileName;
            }

            var button = _colorPreviewButton!;
            button.Enabled = false;
            UseWaitCursor = true;
            JwwColorStats stats;
            try
            {
                stats = await Task.Run(() => JwwColorStats.Read(path));
            }
            catch (Exception ex)
            {
                JwLogging.AppLog.Warn(ex, "色分布の読み込みに失敗 {Path}", path);
                RGBControls.Errors.ErrorReporter.Report(ex, "図面を読み込めませんでした。");
                return;
            }
            finally
            {
                UseWaitCursor = false;
                if (!IsDisposed) button.Enabled = true;
            }
            if (IsDisposed) return;

            using var dlg = new ColorPreviewForm(stats, Path.GetFileName(path), CurrentColorAssignments());
            if (dlg.ShowDialog(this) != DialogResult.OK || dlg.Changes.Count == 0) return;

            int n = ApplyColorAssignments(dlg.Changes);
            if (n > 0)
                this.ShowSuccessTip($"{n} 件の色を反映しました。「保存」を押すと確定します。");
        }
    }
}
