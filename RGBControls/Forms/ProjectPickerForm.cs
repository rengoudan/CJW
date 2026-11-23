using System.Drawing;
using JwServices;

namespace RGBControls.Forms
{
    /// <summary>
    /// 2026年11月24日 ワークベンチの操作（取り込み・梁統計・一括出力）で工事を選ぶ小さな画面。
    /// 最近取り込んだ工事を上に並べ、先頭を選んだ状態で開く。ダブルクリック・Enter で決定。
    /// </summary>
    public sealed class ProjectPickerForm : Form
    {
        private readonly ListView _list;
        public long? SelectedId { get; private set; }

        /// <param name="rows">表示順の工事（工事・顧客・階の進み具合の文字）。</param>
        public ProjectPickerForm(string title, IReadOnlyList<(long Id, string Name, string Customer, string Progress)> rows)
        {
            float k = DeviceDpi / 96f;
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ShowInTaskbar = false;
            Font = new Font("Microsoft Sans Serif", 10.5f); // UiFont が日本語 UI フォントへ置き換える
            ClientSize = new Size((int)(520 * k), (int)(380 * k));

            _list = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, MultiSelect = false, HideSelection = false, GridLines = true };
            _list.Columns.Add("工事", (int)(200 * k));
            _list.Columns.Add("顧客", (int)(170 * k));
            _list.Columns.Add("階", (int)(110 * k));
            foreach (var r in rows) _list.Items.Add(new ListViewItem(new[] { r.Name, r.Customer, r.Progress }) { Tag = r.Id });
            if (_list.Items.Count > 0) _list.Items[0].Selected = true;
            _list.DoubleClick += (s, e) => Accept();

            var ok = new Button { Text = "OK", AutoSize = true, Padding = new Padding(16, 2, 16, 2) };
            var cancel = new Button { Text = "キャンセル", AutoSize = true, Padding = new Padding(10, 2, 10, 2), DialogResult = DialogResult.Cancel };
            ok.Click += (s, e) => Accept();
            var bottom = new Panel { Dock = DockStyle.Bottom, Height = (int)(48 * k) };
            bottom.Controls.AddRange(new Control[] { ok, cancel });
            bottom.Layout += (s, e) =>
            {
                cancel.Location = new Point(bottom.ClientSize.Width - cancel.Width - 12, (int)(8 * k));
                ok.Location = new Point(cancel.Left - ok.Width - 8, cancel.Top);
            };
            AcceptButton = ok;
            CancelButton = cancel;
            var pad = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 10, 10, 0) };
            pad.Controls.Add(_list);
            Controls.Add(pad);
            Controls.Add(bottom);
        }

        private void Accept()
        {
            if (_list.SelectedItems.Count == 0) return;
            SelectedId = (long)_list.SelectedItems[0].Tag!;
            DialogResult = DialogResult.OK;
        }
    }
}
