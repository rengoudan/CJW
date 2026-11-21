using JwLogging;
using RGBControls.Errors;
using Sunny.UI;

namespace RGBControls.Pages
{
    /// <summary>
    /// 日志查看页（导航栏「ログ」）。读取 %LocalAppData%\RGBJW\logs 下的日志文件，
    /// 可按日期、级别、关键字筛选，选中一行在下方显示完整内容（含堆栈）。纯代码构建。
    /// </summary>
    public class LogPage : UIPage
    {
        private const int MaxRows = 5000;

        private readonly ComboBox _fileBox;
        private readonly ComboBox _levelBox;
        private readonly TextBox _keywordBox;
        private readonly ListView _list;
        private readonly TextBox _detail;
        private readonly Label _status;
        private List<LogEntry> _entries = new();

        private static readonly (string Text, LogLevelKind Min)[] Levels =
        {
            ("すべて", LogLevelKind.Debug),
            ("情報以上", LogLevelKind.Info),
            ("警告以上", LogLevelKind.Warning),
            ("エラーのみ", LogLevelKind.Error),
        };

        public LogPage()
        {
            Text = "ログ";
            Font = SystemFonts.MessageBoxFont ?? Font;

            _fileBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 170, Margin = new Padding(4, 6, 4, 4) };
            _levelBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 110, Margin = new Padding(4, 6, 4, 4) };
            _levelBox.Items.AddRange(Levels.Select(l => (object)l.Text).ToArray());
            _levelBox.SelectedIndex = 2; // 默认「警告以上」
            _keywordBox = new TextBox { Width = 200, Margin = new Padding(4, 6, 4, 4), PlaceholderText = "キーワード" };

            _fileBox.SelectedIndexChanged += (s, e) => LoadSelectedFile();
            _levelBox.SelectedIndexChanged += (s, e) => ApplyFilter();
            _keywordBox.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { ApplyFilter(); e.SuppressKeyPress = true; } };

            var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8, 4, 8, 4), WrapContents = true };
            toolbar.Controls.Add(MakeLabel("日付"));
            toolbar.Controls.Add(_fileBox);
            toolbar.Controls.Add(MakeLabel("レベル"));
            toolbar.Controls.Add(_levelBox);
            toolbar.Controls.Add(_keywordBox);
            toolbar.Controls.Add(MakeButton("検索", (s, e) => ApplyFilter()));
            toolbar.Controls.Add(MakeButton("最新に更新", (s, e) => RefreshFiles()));
            toolbar.Controls.Add(MakeButton("ログフォルダを開く", (s, e) => SupportTools.OpenLogFolder(this)));
            toolbar.Controls.Add(MakeButton("サポート用に保存", (s, e) => SupportTools.SaveSupportPackage(this)));

            _list = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                HideSelection = false,
            };
            _list.Columns.Add("時刻", 150);
            _list.Columns.Add("レベル", 70);
            _list.Columns.Add("メッセージ", 900);
            _list.SelectedIndexChanged += (s, e) =>
                _detail!.Text = _list.SelectedItems.Count > 0 ? ((LogEntry)_list.SelectedItems[0].Tag).FullText : "";

            _detail = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                Font = new Font(FontFamily.GenericMonospace, 9f),
            };

            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
            split.Panel1.Controls.Add(_list);
            split.Panel2.Controls.Add(_detail);
            split.HandleCreated += (s, e) => split.SplitterDistance = Math.Max(100, split.Height * 6 / 10);

            _status = new Label { Dock = DockStyle.Bottom, AutoSize = false, Height = 24, Padding = new Padding(8, 4, 8, 0), ForeColor = SystemColors.GrayText };

            Controls.Add(split);
            Controls.Add(toolbar);
            Controls.Add(_status);

            // 每次切换到本页时自动刷新
            VisibleChanged += (s, e) => { if (Visible) RefreshFiles(); };
        }

        private static Label MakeLabel(string text) =>
            new() { Text = text, AutoSize = true, Margin = new Padding(8, 10, 0, 4) };

        private static Button MakeButton(string text, EventHandler onClick)
        {
            var b = new Button { Text = text, AutoSize = true, Margin = new Padding(4, 4, 4, 4) };
            b.Click += onClick;
            return b;
        }

        private void RefreshFiles()
        {
            var keep = _fileBox.SelectedItem as FileItem;
            var files = AppLog.GetRecentLogFiles(30).Select(f => new FileItem(f)).ToList();

            _fileBox.BeginUpdate();
            _fileBox.Items.Clear();
            _fileBox.Items.AddRange(files.Cast<object>().ToArray());
            _fileBox.EndUpdate();

            if (files.Count == 0)
            {
                _entries.Clear();
                ApplyFilter();
                return;
            }
            var idx = keep == null ? 0 : Math.Max(0, files.FindIndex(f => f.File.FullName == keep.File.FullName));
            if (_fileBox.SelectedIndex == idx) LoadSelectedFile(); // 同一项不会触发 SelectedIndexChanged
            else _fileBox.SelectedIndex = idx;
        }

        private void LoadSelectedFile()
        {
            if (_fileBox.SelectedItem is not FileItem item) return;
            try
            {
                _entries = LogReader.Read(item.File.FullName);
            }
            catch (Exception ex)
            {
                AppLog.Warn(ex, "ログファイルの読み込みに失敗 {Path}", item.File.FullName);
                _entries = new List<LogEntry>();
                _status.Text = $"ログファイルを読み込めませんでした: {ex.Message}";
                return;
            }
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var min = Levels[Math.Max(0, _levelBox.SelectedIndex)].Min;
            var kw = _keywordBox.Text.Trim();

            var rows = _entries
                .Where(e => e.Level >= min)
                .Where(e => kw.Length == 0 || e.FullText.Contains(kw, StringComparison.OrdinalIgnoreCase))
                .Reverse() // 新的在上
                .Take(MaxRows)
                .ToList();

            _list.BeginUpdate();
            _list.Items.Clear();
            foreach (var e in rows)
            {
                var it = new ListViewItem(new[] { e.Time.ToString("MM-dd HH:mm:ss.fff"), LevelText(e.Level), e.Message }) { Tag = e };
                if (e.Level >= LogLevelKind.Error) it.ForeColor = Color.Firebrick;
                else if (e.Level == LogLevelKind.Warning) it.ForeColor = Color.DarkOrange;
                _list.Items.Add(it);
            }
            _list.EndUpdate();
            _detail.Text = "";

            int errors = _entries.Count(e => e.Level >= LogLevelKind.Error);
            int warns = _entries.Count(e => e.Level == LogLevelKind.Warning);
            _status.Text = $"表示 {rows.Count} 件 / 全 {_entries.Count} 件（エラー {errors}・警告 {warns}）" +
                           (rows.Count >= MaxRows ? $"　※最新 {MaxRows} 件のみ表示" : "") +
                           $"　保存先: {AppLog.LogDir}";
        }

        private static string LevelText(LogLevelKind l) => l switch
        {
            LogLevelKind.Debug => "デバッグ",
            LogLevelKind.Info => "情報",
            LogLevelKind.Warning => "警告",
            LogLevelKind.Error => "エラー",
            LogLevelKind.Fatal => "致命的",
            _ => l.ToString(),
        };

        private sealed class FileItem
        {
            public FileInfo File { get; }
            public FileItem(FileInfo f) => File = f;
            // rgbjw-20261121.log → 2026-11-21（同日因超 50MB 分割的 rgbjw-20261121_001.log 也能显示）
            public override string ToString()
            {
                var name = Path.GetFileNameWithoutExtension(File.Name).Replace("rgbjw-", "");
                return name.Length >= 8 && DateTime.TryParseExact(name[..8], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var d)
                    ? d.ToString("yyyy-MM-dd") + name[8..]
                    : name;
            }
        }
    }
}
