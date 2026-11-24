using RGBControls.Classes;
using RGBControls.Forms;
using System;
using System.Windows.Forms;

namespace RGBControls
{
    /// <summary>2026年11月24日 ヘッダーのデータベース設定ボタンと、現在の接続先の表示。</summary>
    public partial class MainForm
    {
        private void InstallDatabaseButton()
        {
            var button = new AntdUI.Button
            {
                Dock = DockStyle.Right,
                Ghost = true,
                IconSvg = "DatabaseOutlined",
                Size = new System.Drawing.Size(48, 40),
            };
            new AntdUI.TooltipComponent().SetTip(button, "データベース設定");
            button.Click += DatabaseButton_Click;
            pageHeader1.Controls.Add(button);
            button.BringToFront(); // 設定ボタンの左に並べる
            pageHeader1.SubText = DatabaseStartup.Description;
        }

        private void DatabaseButton_Click(object? sender, EventArgs e)
        {
            var before = JwSharedConfig.Config.AppConfigManager.Current.Database.Clone();
            using var form = new DatabaseSettingsForm();
            if (form.ShowDialog(this) != DialogResult.OK || form.Saved == null) return;
            if (before.SameTarget(form.Saved)) return;

            if (MessageBox.Show(this, "データベースの設定を保存しました。再起動すると反映されます。\n今すぐ再起動しますか？",
                    "データベース設定", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Restart();
        }
    }
}
