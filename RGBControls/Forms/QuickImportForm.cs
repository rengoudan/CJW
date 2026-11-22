using System.Drawing;
using JwCore;
using RGBControls.Classes;

namespace RGBControls.Forms
{
    /// <summary>
    /// 2026年11月22日 取り込み時の簡易確認（顧客の解析設定が保存済みのときだけ出す）。
    /// 階名・材料と主な色の設定を確認し、「このまま解析」なら解析設定画面を見せずに確定する（設定の反映は解析設定画面の既存処理）。
    /// 「設定を変更…」で従来の解析設定画面を開く。
    /// </summary>
    public sealed class QuickImportForm : Form
    {
        /// <summary>このまま解析＝OK、設定を変更＝Retry、やめる＝Cancel。</summary>
        public string FloorName => _floor.Text;
        public string? MaterialId => (_material.SelectedItem as JwMaterialData)?.Id;

        private readonly TextBox _floor;
        private readonly ComboBox _material;
        private readonly Func<string, Task<string?>> _checkFloorName;

        public QuickImportForm(string projectName, string fileName, string progress, string defaultFloorName,
            IList<JwMaterialData> materials, string settingSummary, Func<string, Task<string?>> checkFloorName)
        {
            _checkFloorName = checkFloorName;
            float k = DeviceDpi / 96f;
            Text = $"図面の取り込み{progress}　{fileName}";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ShowInTaskbar = false;
            Font = new Font("Microsoft Sans Serif", 10.5f); // UiFont が日本語 UI フォントへ置き換える
            ClientSize = new Size((int)(560 * k), (int)(300 * k));

            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(14, 12, 14, 0) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90 * k));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            Label L(string t) => new() { Text = t, AutoSize = true, Margin = new Padding(0, 8, 0, 4) };

            _floor = new TextBox { Text = defaultFloorName, Dock = DockStyle.Fill, Margin = new Padding(0, 4, 0, 4) };
            _material = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill, Margin = new Padding(0, 4, 0, 4), DisplayMember = "MaterialName" };
            foreach (var m in materials) _material.Items.Add(m);
            if (_material.Items.Count > 0) _material.SelectedIndex = 0; // 解析設定画面と同じく先頭が既定

            grid.Controls.Add(L("工事"), 0, 0); grid.Controls.Add(new Label { Text = projectName, AutoSize = true, Margin = new Padding(0, 8, 0, 4) }, 1, 0);
            grid.Controls.Add(L("ファイル"), 0, 1); grid.Controls.Add(new Label { Text = fileName, AutoSize = true, Margin = new Padding(0, 8, 0, 4) }, 1, 1);
            grid.Controls.Add(L("階名"), 0, 2); grid.Controls.Add(_floor, 1, 2);
            grid.Controls.Add(L("材料"), 0, 3); grid.Controls.Add(_material, 1, 3);
            grid.Controls.Add(L("解析設定"), 0, 4);
            grid.Controls.Add(new Label { Text = settingSummary, AutoSize = true, MaximumSize = new Size((int)(430 * k), 0), Margin = new Padding(0, 8, 0, 4), ForeColor = Color.DimGray }, 1, 4);

            var go = new Button { Text = "このまま解析", AutoSize = true, Padding = new Padding(14, 2, 14, 2) };
            var change = new Button { Text = "設定を変更…", AutoSize = true, Padding = new Padding(10, 2, 10, 2), DialogResult = DialogResult.Retry };
            var cancel = new Button { Text = "キャンセル", AutoSize = true, Padding = new Padding(10, 2, 10, 2), DialogResult = DialogResult.Cancel };
            go.Click += async (s, e) =>
            {
                go.Enabled = false;
                try
                {
                    var err = string.IsNullOrWhiteSpace(_floor.Text) ? "階名を入力してください。" : await _checkFloorName(_floor.Text);
                    if (err != null)
                    {
                        MessageBox.Show(this, err, "図面の取り込み", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _floor.Focus();
                        _floor.SelectAll();
                        return;
                    }
                    if (_material.SelectedItem == null)
                    {
                        MessageBox.Show(this, "材料を選んでください。", "図面の取り込み", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DialogResult = DialogResult.OK;
                }
                finally { if (!IsDisposed) go.Enabled = true; }
            };
            var bottom = new Panel { Dock = DockStyle.Bottom, Height = (int)(50 * k) };
            bottom.Controls.AddRange(new Control[] { go, change, cancel });
            bottom.Layout += (s, e) =>
            {
                cancel.Location = new Point(bottom.ClientSize.Width - cancel.Width - 12, (int)(10 * k));
                change.Location = new Point(cancel.Left - change.Width - 8, cancel.Top);
                go.Location = new Point(change.Left - go.Width - 8, cancel.Top);
            };
            AcceptButton = go;
            CancelButton = cancel;

            Controls.Add(grid);
            Controls.Add(bottom);
        }
    }
}
