using AntdUI;
using JwCore;

namespace RGBControls.Pages
{
    /// <summary>
    /// 2026年11月22日 階一覧の上に、よく使う出力のボタンを並べる（右クリックメニューと同じ処理を呼ぶだけ）。
    /// 原来所有导出都只在右键菜单里，新用户不容易发现。右键菜单保持不变。
    /// </summary>
    public partial class NewProjectMainPage
    {
        private System.Windows.Forms.Label? _exportBarFloor;
        private readonly List<AntdUI.Button> _exportBarButtons = new();

        private static readonly string[] ExportBarItems =
        {
            "要確認の梁を表示", "梁製造CSV", "梁JW出力", "番付図上JW出力", "番付図下JW出力", "ブレース施工図",
        };

        private void InitExportBar()
        {
            if (_exportBarFloor != null) return;
            var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(6, 6, 6, 0), WrapContents = false };
            _exportBarFloor = new System.Windows.Forms.Label { AutoSize = true, Margin = new Padding(0, 9, 12, 0), Text = "階を選択してください" };
            bar.Controls.Add(_exportBarFloor);
            foreach (var text in ExportBarItems)
            {
                var b = new AntdUI.Button
                {
                    Text = text,
                    AutoSizeMode = TAutoSize.Width,
                    Height = 32,
                    Margin = new Padding(0, 0, 6, 0),
                    Enabled = false,
                    Type = text == "梁製造CSV" ? TTypeMini.Primary : TTypeMini.Default,
                    Ghost = text != "梁製造CSV",
                };
                b.Click += (s, e) =>
                {
                    if (selectedsubData != null) contextMenuStrip1_Opening(new ContextMenuStripItem(text));
                };
                _exportBarButtons.Add(b);
                bar.Controls.Add(b);
            }
            panel3.Controls.Add(bar);
            bar.SendToBack(); // 表（Fill）より先に Dock=Top を確定させる
            table1.SelectIndexChanged += (s, e) => OnFloorSelected();
        }

        /// <summary>左クリックで選んだ階も、右クリックと同じく「選択中の階」にする。</summary>
        private void OnFloorSelected()
        {
            if (table1.SelectedIndex > 0 && table1[table1.SelectedIndex - 1].record is JwProjectSubData sub)
                selectedsubData = sub;
            UpdateExportBar();
        }

        private void UpdateExportBar()
        {
            if (_exportBarFloor == null) return;
            bool has = selectedsubData != null && table1.DataSource != null && table1.SelectedIndex > 0;
            _exportBarFloor.Text = has ? $"選択中の階：{selectedsubData!.FloorName}" : "階を選択してください";
            foreach (var b in _exportBarButtons) b.Enabled = has;
        }
    }
}
