using JwServices;
using Sunny.UI;

namespace RGBJWMain.Forms
{
    /// <summary>
    /// 2026年11月22日 取り込みの流れに関する追加（設定値・解析処理は変えない）。
    /// ・階名の重複を、解析を始める前（この画面の「保存」）で確認する。
    ///   原来要等整张图解析完、在解析窗口按「保存」时才提示「このフロアは識別されアップロードされました」。
    ///   判定条件は保存時の確認（同じ工事・同じ階名）と同じ。保存時の確認もそのまま残す。
    /// </summary>
    public partial class CustomerDesignSetting
    {
        /// <summary>
        /// 簡易確認（QuickImportForm）の「このまま解析」から開くとき true。画面を見せずに、読み込み完了後に「保存」と同じ処理を行う
        /// （＝画面を開いて何も変えずに「保存」を押すのと同じ。設定の反映は既存の uiButton1_Click がそのまま行う）。
        /// 確認で止まった場合（階名の重複など）は画面を表示して利用者に任せる。
        /// </summary>
        public bool AutoConfirm { get; set; }

        /// <summary>階名（簡易確認で入力した値）。null なら従来どおりファイル名。</summary>
        public string? FloorNameOverride { get; set; }

        /// <summary>材料の Id（簡易確認で選んだ値）。null なら従来どおり一覧の先頭。</summary>
        public string? MaterialIdOverride { get; set; }

        private void PrepareAutoConfirm()
        {
            if (!AutoConfirm) return;
            Opacity = 0;            // 自動確定の間は見せない
        }

        private void RevealForUser()
        {
            if (!AutoConfirm) return;
            AutoConfirm = false;
            Opacity = 1;
            Activate();
        }

        /// <summary>CustomerDesignSetting_Load の最後（データの読み込み完了後）に呼ばれる。</summary>
        private void AfterSettingsLoaded()
        {
            if (!isloadbyparse) return;
            if (!string.IsNullOrEmpty(FloorNameOverride))
            {
                uiTextBox1.Text = FloorNameOverride;
                if (propn != null) propn.FloorName = FloorNameOverride;
            }
            if (!string.IsNullOrEmpty(MaterialIdOverride) && uiComboBox3.DataSource is IList<JwCore.JwMaterialData> mats)
            {
                int i = mats.ToList().FindIndex(m => m.Id == MaterialIdOverride);
                if (i >= 0) uiComboBox3.SelectedIndex = i;
            }
            if (AutoConfirm)
                BeginInvoke(new Action(() => uiButton1_Click(uiButton1, EventArgs.Empty)));
        }

        /// <summary>階名が空・既に取り込み済みなら知らせて false。</summary>
        private async Task<bool> CheckFloorNameAsync()
        {
            if (!isloadbyparse || propn?.MainData == null) return true;
            string name = uiTextBox1.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                RevealForUser();
                UIMessageBox.ShowWarning("階名を入力してください。");
                uiTextBox1.Focus();
                return false;
            }
            long mainId = propn.MainData.Id;
            var service = ServiceFactory.GetInstance().CreateJwProjectMainService();
            var existing = await service.FindSubData(t => t.FloorName == name && t.JwProjectMainDataId == mainId);
            if (existing != null)
            {
                RevealForUser();
                UIMessageBox.ShowWarning($"階「{name}」は、この工事に既に取り込まれています。\n別の階名を入力してください。");
                uiTextBox1.Focus();
                uiTextBox1.SelectAll();
                return false;
            }
            return true;
        }
    }
}
