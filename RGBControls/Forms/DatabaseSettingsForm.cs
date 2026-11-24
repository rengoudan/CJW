using JwData;
using JwSharedConfig.Config;
using JwSharedConfig.Models;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBControls.Forms
{
    /// <summary>
    /// 2026年11月24日 データベース設定（ローカル SQLite／リモート SQL Server）。
    /// 起動時（MainForm より前）にも使うため単独の Form。保存すると appconfig.json に書き、DialogResult.OK で閉じる。
    /// </summary>
    public class DatabaseSettingsForm : Form
    {
        private readonly AntdUI.Radio _rbLocal = new() { Text = "ローカル（本機の SQLite）", AutoSizeMode = AntdUI.TAutoSize.Width };
        private readonly AntdUI.Radio _rbRemote = new() { Text = "リモート（SQL Server）", AutoSizeMode = AntdUI.TAutoSize.Width };
        private readonly AntdUI.Input _server = new() { PlaceholderText = "例：192.168.1.10 / 192.168.1.10,1433 / 192.168.1.10\\SQLEXPRESS" };
        private readonly AntdUI.Input _database = new() { PlaceholderText = "例：JwData" };
        private readonly AntdUI.Input _user = new() { PlaceholderText = "例：sa" };
        private readonly AntdUI.Input _password = new() { UseSystemPasswordChar = true };
        private readonly AntdUI.Checkbox _trustCert = new() { Text = "サーバー証明書を検証しない（社内サーバー向け）", AutoSizeMode = AntdUI.TAutoSize.Width };
        private readonly AntdUI.Label _status = new() { Text = "" };
        private readonly AntdUI.Button _btnTest = new() { Text = "接続テスト", IconSvg = "ApiOutlined" };
        private readonly AntdUI.Button _btnSave = new() { Text = "保存", Type = AntdUI.TTypeMini.Primary };
        private readonly AntdUI.Button _btnCancel = new() { Text = "キャンセル" };

        private readonly int _timeout;

        /// <summary>保存された設定（DialogResult.OK のとき）。</summary>
        public DatabaseSettings? Saved { get; private set; }

        /// <param name="message">起動時に接続できなかったときの説明（上部に赤字で表示）。</param>
        public DatabaseSettingsForm(string? message = null)
        {
            Text = "データベース設定";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = message != null; // 起動時はタスクバーに出す（MainForm がまだないため）
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Meiryo UI", 10F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(620, message != null ? 470 : 420);

            var s = AppConfigManager.Current.Database;
            _timeout = s.ConnectTimeout;
            _rbLocal.Checked = !s.IsRemote;
            _rbRemote.Checked = s.IsRemote;
            _server.Text = s.Server;
            _database.Text = s.Database;
            _user.Text = s.UserName;
            _password.Text = s.GetPassword();
            _trustCert.Checked = s.TrustServerCertificate;

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(16, 12, 16, 8),
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            if (message != null)
            {
                var warn = new AntdUI.Label { Text = message, ForeColor = Color.FromArgb(207, 19, 34), Dock = DockStyle.Fill };
                AddRow(grid, null, warn, 54);
            }

            var modes = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Margin = new Padding(0) };
            _rbLocal.Height = _rbRemote.Height = 34;
            modes.Controls.Add(_rbLocal);
            modes.Controls.Add(_rbRemote);
            AddRow(grid, "保存先", modes, 40);
            AddRow(grid, "サーバー", _server, 42);
            AddRow(grid, "データベース名", _database, 42);
            AddRow(grid, "ユーザー名", _user, 42);
            AddRow(grid, "パスワード", _password, 42);
            AddRow(grid, null, _trustCert, 34);
            _status.Dock = DockStyle.Fill;
            AddRow(grid, null, _status, 54);
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            grid.RowCount++;

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 52,
                Padding = new Padding(12, 6, 12, 6),
            };
            foreach (var b in new[] { _btnCancel, _btnSave, _btnTest })
            {
                b.Size = new Size(b == _btnTest ? 130 : 100, 38);
                buttons.Controls.Add(b);
            }

            Controls.Add(grid);
            Controls.Add(buttons);

            _rbLocal.CheckedChanged += (_, _) => UpdateEnabled();
            _rbRemote.CheckedChanged += (_, _) => UpdateEnabled();
            _btnTest.Click += async (_, _) => await TestAsync();
            _btnSave.Click += async (_, _) => await SaveAsync();
            _btnCancel.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
            UpdateEnabled();
        }

        private static void AddRow(TableLayoutPanel grid, string? caption, Control control, int height)
        {
            int row = grid.RowCount++;
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
            if (caption != null)
                grid.Controls.Add(new Label { Text = caption, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, row);
            control.Dock = DockStyle.Fill;
            grid.Controls.Add(control, 1, row);
        }

        private bool IsRemote => _rbRemote.Checked;

        private void UpdateEnabled()
        {
            foreach (Control c in new Control[] { _server, _database, _user, _password, _trustCert, _btnTest })
                c.Enabled = IsRemote;
            if (!IsRemote) SetStatus("", false);
        }

        private void SetStatus(string text, bool error)
        {
            _status.Text = text;
            _status.ForeColor = error ? Color.FromArgb(207, 19, 34) : Color.FromArgb(56, 158, 13);
        }

        private string? ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(_server.Text)) return "サーバーを入力してください。";
            if (string.IsNullOrWhiteSpace(_database.Text)) return "データベース名を入力してください。";
            if (string.IsNullOrWhiteSpace(_user.Text)) return "ユーザー名を入力してください。";
            return null;
        }

        private string BuildConnectionString() =>
            SqlServerDatabase.BuildConnectionString(_server.Text, _database.Text, _user.Text, _password.Text, _trustCert.Checked, _timeout);

        /// <summary>接続テストを実行し、成功（データベース未作成・更新待ちを含む）なら true。</summary>
        private async Task<bool> TestAsync()
        {
            var invalid = ValidateInput();
            if (invalid != null)
            {
                SetStatus(invalid, true);
                return false;
            }

            var conn = BuildConnectionString();
            _btnTest.Loading = true;
            _btnSave.Enabled = false;
            SetStatus("接続中…", false);
            try
            {
                var r = await Task.Run(() => SqlServerDatabase.Check(conn));
                switch (r.Status)
                {
                    case SqlServerDatabase.Status.Ready:
                        SetStatus("接続できました。", false);
                        return true;
                    case SqlServerDatabase.Status.DatabaseMissing:
                        SetStatus("接続できました。データベースがまだないため、再起動時に作成します（作成権限が必要です）。", false);
                        return true;
                    case SqlServerDatabase.Status.NeedsUpgrade:
                        SetStatus($"接続できました。再起動時にテーブルを作成・更新します（{r.PendingCount} 件）。", false);
                        return true;
                    case SqlServerDatabase.Status.NewerThanApp:
                        SetStatus("このデータベースは新しいバージョンのプログラムで更新されています。プログラムを更新してください。", true);
                        return false;
                    default:
                        SetStatus("接続できません：" + r.Error, true);
                        return false;
                }
            }
            finally
            {
                _btnTest.Loading = false;
                _btnSave.Enabled = true;
            }
        }

        private async Task SaveAsync()
        {
            if (IsRemote)
            {
                var invalid = ValidateInput();
                if (invalid != null)
                {
                    SetStatus(invalid, true);
                    return;
                }
                if (!await TestAsync()
                    && MessageBox.Show(this, "接続を確認できませんでした。このまま保存しますか？", Text,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
            }

            var s = AppConfigManager.Current.Database;
            s.Mode = IsRemote ? DatabaseSettings.ModeRemote : DatabaseSettings.ModeLocal;
            // ローカルにしてもリモートの入力内容は残す（戻すときに再入力しなくてよい）
            if (IsRemote || !string.IsNullOrWhiteSpace(_server.Text))
            {
                s.Server = _server.Text.Trim();
                s.Database = _database.Text.Trim();
                s.UserName = _user.Text.Trim();
                s.SetPassword(_password.Text);
                s.TrustServerCertificate = _trustCert.Checked;
            }
            AppConfigManager.Save();
            Saved = s.Clone();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
