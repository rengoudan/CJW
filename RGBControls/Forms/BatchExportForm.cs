using System.Drawing;
using JwCore;
using RGBControls.Classes;

namespace RGBControls.Forms
{
    /// <summary>
    /// 2026年11月22日 工事の「一括出力」の指定画面（出力の種類・階・出力先）。出力そのものは各出力の既存処理を使う。
    /// </summary>
    public sealed class BatchExportForm : Form
    {
        public enum Kind { BeamJw, BanzukeTop, BanzukeBottom, Brace, Csv }

        public static readonly (Kind Kind, string Name)[] Kinds =
        {
            (Kind.BeamJw, "梁JW（梁ごとの図）"),
            (Kind.BanzukeTop, "番付図上JW"),
            (Kind.BanzukeBottom, "番付図下JW"),
            (Kind.Brace, "ブレース施工図"),
            (Kind.Csv, "梁製造CSV"),
        };

        public const string FolderKey = "Batch";

        private readonly CheckedListBox _kinds;
        private readonly CheckedListBox _floors;
        private readonly TextBox _folder;
        private readonly List<JwProjectSubData> _subs;

        public List<Kind> SelectedKinds => _kinds.CheckedIndices.Cast<int>().Select(i => Kinds[i].Kind).ToList();
        public List<JwProjectSubData> SelectedFloors => _floors.CheckedIndices.Cast<int>().Select(i => _subs[i]).ToList();
        public string Folder => _folder.Text;

        public BatchExportForm(JwProjectMainData main, IEnumerable<JwProjectSubData> floors)
        {
            float k = DeviceDpi / 96f;
            _subs = floors.OrderBy(s => s.FloorName, StringComparer.Ordinal).ToList();
            Text = $"一括出力　{main.ProjectName}";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ShowInTaskbar = false;
            Font = new Font("Microsoft Sans Serif", 10.5f); // UiFont が日本語 UI フォントへ置き換える
            ClientSize = new Size((int)(620 * k), (int)(470 * k));

            _kinds = new CheckedListBox { CheckOnClick = true, IntegralHeight = false, Dock = DockStyle.Fill };
            foreach (var (_, name) in Kinds) _kinds.Items.Add(name, true);
            var kindBox = new GroupBox { Text = "出力する種類", Dock = DockStyle.Left, Width = (int)(260 * k), Padding = new Padding(6) };
            kindBox.Controls.Add(_kinds);

            _floors = new CheckedListBox { CheckOnClick = true, IntegralHeight = false, Dock = DockStyle.Fill };
            foreach (var s in _subs) _floors.Items.Add(s.FloorName ?? "", true);
            var allOn = new LinkLabel { Text = "すべて選択", AutoSize = true };
            var allOff = new LinkLabel { Text = "すべて解除", AutoSize = true, Margin = new Padding(12, 3, 3, 3) };
            allOn.LinkClicked += (s, e) => { for (int i = 0; i < _floors.Items.Count; i++) _floors.SetItemChecked(i, true); };
            allOff.LinkClicked += (s, e) => { for (int i = 0; i < _floors.Items.Count; i++) _floors.SetItemChecked(i, false); };
            var floorLinks = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = (int)(26 * k) };
            floorLinks.Controls.AddRange(new Control[] { allOn, allOff });
            var floorBox = new GroupBox { Text = "出力する階", Dock = DockStyle.Fill, Padding = new Padding(6) };
            floorBox.Controls.Add(_floors);
            floorBox.Controls.Add(floorLinks);

            var lists = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 8, 10, 0) };
            lists.Controls.Add(floorBox);
            lists.Controls.Add(new Panel { Dock = DockStyle.Left, Width = 8 });
            lists.Controls.Add(kindBox);

            _folder = new TextBox { ReadOnly = true, Width = (int)(450 * k), Text = ExportFolders.Get(FolderKey) ?? "" };
            var browse = new Button { Text = "参照…", AutoSize = true };
            browse.Click += (s, e) =>
            {
                using var d = new FolderBrowserDialog();
                if (ExportFolders.Show(d, FolderKey) == DialogResult.OK) _folder.Text = d.SelectedPath;
            };
            var folderRow = new FlowLayoutPanel { Dock = DockStyle.Top, Height = (int)(34 * k), WrapContents = false };
            folderRow.Controls.AddRange(new Control[] { new Label { Text = "出力先", AutoSize = true, Margin = new Padding(3, 7, 6, 0) }, _folder, browse });
            var note = new Label
            {
                Dock = DockStyle.Top,
                Height = (int)(44 * k),
                ForeColor = Color.DimGray,
                Text = $"出力先の下に「{main.ProjectName}」フォルダを作り、階ごとに出力します（同名のファイルは上書き、それ以外のファイルは消しません）。",
            };
            var folderPanel = new Panel { Dock = DockStyle.Bottom, Height = (int)(84 * k), Padding = new Padding(10, 6, 10, 0) };
            folderPanel.Controls.Add(note);
            folderPanel.Controls.Add(folderRow);

            var ok = new Button { Text = "出力", AutoSize = true, Padding = new Padding(16, 2, 16, 2) };
            var cancel = new Button { Text = "キャンセル", AutoSize = true, Padding = new Padding(16, 2, 16, 2), DialogResult = DialogResult.Cancel };
            ok.Click += (s, e) =>
            {
                string? err = SelectedKinds.Count == 0 ? "出力する種類を選んでください。"
                            : SelectedFloors.Count == 0 ? "出力する階を選んでください。"
                            : string.IsNullOrEmpty(Folder) || !Directory.Exists(Folder) ? "出力先のフォルダを選んでください。"
                            : null;
                if (err != null) { MessageBox.Show(this, err, "一括出力", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                DialogResult = DialogResult.OK;
            };
            var bottom = new Panel { Dock = DockStyle.Bottom, Height = (int)(48 * k) };
            bottom.Controls.AddRange(new Control[] { ok, cancel });
            bottom.Layout += (s, e) =>
            {
                cancel.Location = new Point(bottom.ClientSize.Width - cancel.Width - 12, (int)(8 * k));
                ok.Location = new Point(cancel.Left - ok.Width - 8, cancel.Top);
            };
            CancelButton = cancel;

            Controls.Add(lists);
            Controls.Add(folderPanel);
            Controls.Add(bottom);
        }
    }
}
