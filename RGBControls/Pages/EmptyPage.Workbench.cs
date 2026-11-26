using JwCore;
using JwLogging;
using JwServices;
using RGBControls.Forms;
using RGBControls.Pages;
using WLabel = System.Windows.Forms.Label;

namespace RGBJWMain.Pages
{
    /// <summary>
    /// 2026年11月24日 ワークベンチ（ホーム）。
    /// ・よく使う操作（工事を追加・図面を取り込む・梁統計・一括出力）
    /// ・数字カード（進行中の工事・未取り込み階・要確認の階・CSV未出力の階・今月の取り込み）→ 押すと工事一覧を絞り込む
    /// ・対応が必要なもの（要確認／未取り込み／CSV未出力）をその場で処理
    /// ・最近の工事（取り込みの進み具合）→ ダブルクリックで階の作業台
    /// ・作業の流れと準備状況（基礎データ・顧客の解析設定・今日のエラー）
    /// 読み取りのみ。解析・出力の中身は工事一覧の既存の処理をそのまま使う。
    /// </summary>
    public partial class EmptyPage
    {
        private IWorkbenchHost? _host;
        private HomeSummary? _summary;
        private bool _loading, _reloadAgain;
        private float _k = 1f;

        private WLabel _dateLabel = null!;
        private readonly List<(WLabel Value, WLabel Sub)> _kpis = new();
        private readonly List<Action> _kpiActions = new();
        private AntdUI.Table _todoTable = null!, _recentTable = null!;
        private WLabel _todoTitle = null!, _emptyHint = null!;
        private FlowLayoutPanel _readyPanel = null!;
        private TableLayoutPanel _root = null!;

        private static readonly Color Ink = Color.FromArgb(38, 38, 38);
        private static readonly Color Muted = Color.FromArgb(110, 110, 110);
        private static readonly Color CardBorder = Color.FromArgb(222, 226, 232);
        private static readonly Color PageBack = Color.FromArgb(244, 246, 249);
        private static readonly Color Blue = Color.FromArgb(22, 119, 255);
        private static readonly Color Red = Color.FromArgb(220, 53, 69);
        private static readonly Color Orange = Color.FromArgb(230, 140, 20);
        private static readonly Color Green = Color.FromArgb(40, 160, 80);

        private JwProjectMainService Service => ServiceFactory.GetInstance().CreateJwProjectMainService();

        private int S(float v) => (int)Math.Round(v * _k);

        private void BuildWorkbench(IWorkbenchHost host)
        {
            _host = host;
            _k = DeviceDpi / 96f;
            foreach (var l in new Control[] { uiLabel2, uiLabel3, uiLabel4, uiLabel5 }) l.Visible = false; // 旧い案内文
            BackColor = PageBack;

            // 1920×1080（タスクバーを除く約1040px）より窓が低いときは、下段を潰す代わりに縦スクロールさせる
            var root = _root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, BackColor = PageBack,
                Padding = new Padding(S(18), S(12), S(18), S(14)), AutoScroll = true,
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, S(58)));   // 見出し＋操作（操作が折り返すとき BuildHeader で高さを増やす）
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, S(104)));  // 数字カード
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));      // 対応が必要なもの／最近の工事
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, S(150)));  // 作業の流れ／準備状況
            root.Controls.Add(BuildHeader(), 0, 0);
            root.Controls.Add(BuildKpis(), 0, 1);
            root.Controls.Add(BuildLists(), 0, 2);
            root.Controls.Add(BuildBottom(), 0, 3);
            Controls.Add(root);
            root.BringToFront();

            VisibleChanged += async (s, e) => { if (Visible) await ReloadHomeAsync(); };
        }

        // ───────── 見出し・操作 ─────────

        private Control BuildHeader()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };
            var title = new WLabel { Text = "ワークベンチ", AutoSize = true, ForeColor = Ink, Font = new Font("Microsoft Sans Serif", 17f, FontStyle.Bold), Location = new Point(0, S(6)) };
            _dateLabel = new WLabel { AutoSize = true, ForeColor = Muted, Font = new Font("Microsoft Sans Serif", 10f) };
            panel.Controls.Add(title);
            panel.Controls.Add(_dateLabel);

            // 操作ボタンは幅が足りなければ折り返す（右端から左へはみ出して先頭のボタンが見えなくならないように）
            var actions = new FlowLayoutPanel { AutoSize = true, WrapContents = true, FlowDirection = FlowDirection.LeftToRight, BackColor = Color.Transparent };
            actions.Controls.Add(ActionButton("＋ 工事を追加", AntdUI.TTypeMini.Primary, async () => await RunAsync(() => _host!.AddProjectAsync())));
            actions.Controls.Add(ActionButton("図面を取り込む", AntdUI.TTypeMini.Default, async () => await PickAndRunAsync("図面を取り込む工事を選択", id => _host!.ImportAsync(id), importable: true)));
            actions.Controls.Add(ActionButton("梁統計", AntdUI.TTypeMini.Default, async () => await PickAndRunAsync("梁統計を表示する工事を選択", id => _host!.BeamStatsAsync(id), parsedOnly: true)));
            actions.Controls.Add(ActionButton("一括出力", AntdUI.TTypeMini.Default, async () => await PickAndRunAsync("一括出力する工事を選択", id => _host!.BatchExportAsync(id), parsedOnly: true)));
            actions.Controls.Add(ActionButton("更新", AntdUI.TTypeMini.Default, async () => await ReloadHomeAsync(), ghost: true));
            panel.Controls.Add(actions);

            panel.Layout += (s, e) =>
            {
                _dateLabel.Location = new Point(title.Right + S(12), title.Bottom - _dateLabel.Height - S(3));
                // 日付の右側に収まる幅で折り返す
                int left = _dateLabel.Right + S(24);
                int maxW = Math.Max(S(220), panel.ClientSize.Width - left);
                if (actions.MaximumSize.Width != maxW) actions.MaximumSize = new Size(maxW, 0);

                // 折り返した分だけ見出し行を高くする（既定 58 を下限にする）
                if (_root != null && _root.RowStyles.Count > 0)
                {
                    int rowH = Math.Max(S(58), actions.Height + S(12));
                    if (_root.RowStyles[0].Height != rowH) _root.RowStyles[0].Height = rowH;
                }

                actions.Location = new Point(Math.Max(0, panel.ClientSize.Width - actions.Width), Math.Max(0, (panel.ClientSize.Height - actions.Height) / 2));
            };
            return panel;
        }

        private AntdUI.Button ActionButton(string text, AntdUI.TTypeMini type, Action onClick, bool ghost = false)
        {
            var b = new AntdUI.Button
            {
                Text = text, Type = type, Ghost = ghost, BorderWidth = type == AntdUI.TTypeMini.Default ? 1 : 0,
                AutoSizeMode = AntdUI.TAutoSize.Width, Height = S(40), Margin = new Padding(S(6), 0, 0, 0),
                Font = new Font("Microsoft Sans Serif", 10.5f), Radius = 6,
            };
            b.Click += (s, e) => onClick();
            return b;
        }

        // ───────── 数字カード ─────────

        private Control BuildKpis()
        {
            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 5, RowCount = 1, Margin = new Padding(0, S(4), 0, S(8)) };
            for (int i = 0; i < 5; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            string[] titles = { "進行中の工事", "未取り込みの階", "要確認の階", "CSV未出力の階", "今月の取り込み" };
            for (int i = 0; i < titles.Length; i++)
            {
                int idx = i;
                var card = Card(new Padding(i == 0 ? 0 : S(6), 0, i == 4 ? 0 : S(6), 0));
                card.Cursor = Cursors.Hand;
                var t = new WLabel { Text = titles[i], AutoSize = true, ForeColor = Muted, Location = new Point(S(14), S(10)), Font = new Font("Microsoft Sans Serif", 10f) };
                var v = new WLabel { Text = "-", AutoSize = true, ForeColor = Ink, Location = new Point(S(12), S(30)), Font = new Font("Microsoft Sans Serif", 22f, FontStyle.Bold) };
                var sub = new WLabel { Text = "", AutoSize = true, ForeColor = Muted, Font = new Font("Microsoft Sans Serif", 9f) };
                card.Controls.AddRange(new Control[] { t, v, sub });
                card.Layout += (s, e) => sub.Location = new Point(v.Right + S(4), v.Bottom - sub.Height - S(8));
                foreach (Control c in new Control[] { card, t, v, sub })
                {
                    c.Cursor = Cursors.Hand;
                    c.Click += (s, e) => { if (idx < _kpiActions.Count) _kpiActions[idx](); };
                }
                _kpis.Add((v, sub));
                grid.Controls.Add(card, i, 0);
            }
            return grid;
        }

        private static Panel Card(Padding margin)
        {
            var p = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = margin };
            p.Paint += (s, e) =>
            {
                using var pen = new Pen(CardBorder);
                e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            };
            p.Resize += (s, e) => p.Invalidate();
            return p;
        }

        private WLabel SectionTitle(string text) => new()
        {
            Text = text, Dock = DockStyle.Top, Height = S(34), ForeColor = Ink, TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Microsoft Sans Serif", 11.5f, FontStyle.Bold), Padding = new Padding(S(10), 0, 0, 0),
        };

        // ───────── 対応が必要なもの／最近の工事 ─────────

        private Control BuildLists()
        {
            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Margin = new Padding(0, 0, 0, S(8)) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

            // 対応が必要なもの
            var todo = Card(new Padding(0, 0, S(6), 0));
            todo.Padding = new Padding(1);
            _todoTitle = SectionTitle("対応が必要なもの");
            _todoTable = new AntdUI.Table { Dock = DockStyle.Fill, Bordered = false, EmptyHeader = true, AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill, Font = new Font("Microsoft Sans Serif", 10f) };
            _todoTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("Kind", "種類") { Width = "90", Render = (v, r, i) => r is TodoRow t ? new AntdUI.CellTag(t.Kind, t.KindType) : null },
                new AntdUI.Column("Project", "工事"),
                new AntdUI.Column("Floor", "階") { Width = "80" },
                new AntdUI.Column("Detail", "内容"),
                new AntdUI.Column("Action", "操作")
                {
                    Width = "100", Align = AntdUI.ColumnAlign.Center,
                    Render = (v, r, i) => r is TodoRow t ? new AntdUI.CellButton(t.Kind, t.ActionText, t.KindType) { Ghost = true, BorderWidth = 1 } : null,
                },
            };
            _todoTable.CellButtonClick += async (s, e) => { if (e.Record is TodoRow t) await RunAsync(t.Run); };
            _todoTable.CellDoubleClick += async (s, e) => { if (e.Record is TodoRow t) await RunAsync(t.Run); };
            _emptyHint = new WLabel
            {
                Dock = DockStyle.Fill, Visible = false, ForeColor = Muted, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 11f),
                Text = "まだ工事がありません。\r\n\r\n① 顧客ごとの解析設定を確認し、\r\n② 上の「＋ 工事を追加」で工事（階数）を登録してから、\r\n③ 設計図（.jww）を取り込んでください。",
            };
            todo.Controls.Add(_todoTable);
            todo.Controls.Add(_emptyHint);
            todo.Controls.Add(_todoTitle);
            grid.Controls.Add(todo, 0, 0);

            // 最近の工事
            var recent = Card(new Padding(S(6), 0, 0, 0));
            recent.Padding = new Padding(1);
            var recentHeader = SectionTitle("最近の工事");
            var all = new LinkLabel { Text = "工事一覧へ", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right, LinkColor = Blue };
            all.LinkClicked += async (s, e) => await _host!.ShowProjectsAsync(null, null);
            recentHeader.Controls.Add(all);
            recentHeader.Layout += (s, e) => all.Location = new Point(recentHeader.Width - all.Width - S(12), (recentHeader.Height - all.Height) / 2);
            _recentTable = new AntdUI.Table { Dock = DockStyle.Fill, Bordered = false, EmptyHeader = true, AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill, Font = new Font("Microsoft Sans Serif", 10f) };
            _recentTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("Name", "工事"),
                new AntdUI.Column("Customer", "顧客"),
                new AntdUI.Column("Progress", "取り込み") { Width = "110", Render = (v, r, i) => r is RecentRow x ? new AntdUI.CellProgress(x.Ratio) { Fill = x.Ratio >= 1f ? Green : Blue } : null },
                new AntdUI.Column("FloorText", "階") { Width = "60", Align = AntdUI.ColumnAlign.Center },
                new AntdUI.Column("Check", "要確認")
                {
                    Width = "70", Align = AntdUI.ColumnAlign.Center,
                    Render = (v, r, i) => r is RecentRow x && x.NeedCheck > 0 ? new AntdUI.CellTag(x.NeedCheck.ToString(), AntdUI.TTypeMini.Error) : null,
                },
                new AntdUI.Column("LastText", "最終取り込み") { Width = "110" },
            };
            _recentTable.CellDoubleClick += async (s, e) => { if (e.Record is RecentRow x) await RunAsync(() => _host!.OpenWorkbenchAsync(x.Id)); };
            var tip = new WLabel { Text = "ダブルクリックで階の作業台を開きます", Dock = DockStyle.Bottom, Height = S(26), ForeColor = Muted, Padding = new Padding(S(10), 0, 0, 0), TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Microsoft Sans Serif", 9f) };
            recent.Controls.Add(_recentTable);
            recent.Controls.Add(tip);
            recent.Controls.Add(recentHeader);
            grid.Controls.Add(recent, 1, 0);
            return grid;
        }

        // ───────── 作業の流れ／準備状況 ─────────

        private Control BuildBottom()
        {
            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Margin = new Padding(0) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));

            var flowCard = Card(new Padding(0, 0, S(6), 0));
            flowCard.Padding = new Padding(1);
            // 5 つの手順を幅いっぱいに等分（間に矢印）
            var steps = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 9, RowCount = 1, Padding = new Padding(S(10), S(4), S(10), S(12)) };
            for (int c = 0; c < 9; c++)
                steps.ColumnStyles.Add(c % 2 == 0 ? new ColumnStyle(SizeType.Percent, 20) : new ColumnStyle(SizeType.Absolute, S(26)));
            (string Text, string Hint, Func<Task> Run)[] defs =
            {
                ("① 顧客・解析設定", "顧客ごとの解析設定", () => { _host!.ShowNav(WorkbenchNav.Customers); return Task.CompletedTask; }),
                ("② 工事登録", "工事名・顧客・階数", () => RunAsync(() => _host!.AddProjectAsync())),
                ("③ 図面取り込み・解析", "階ごとの .jww", () => PickAndRunAsync("図面を取り込む工事を選択", id => _host!.ImportAsync(id), importable: true)),
                ("④ 確認", "要確認の梁・工区", () => _host!.ShowProjectsAsync(NeedCheckMainIds(), "要確認の階がある工事")),
                ("⑤ 出力", "CSV・Excel・施工図", () => PickAndRunAsync("一括出力する工事を選択", id => _host!.BatchExportAsync(id), parsedOnly: true)),
            };
            for (int i = 0; i < defs.Length; i++)
            {
                var d = defs[i];
                var box = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0), BackColor = Color.FromArgb(240, 246, 255), Cursor = Cursors.Hand };
                var t = new WLabel { Text = d.Text, AutoSize = false, Dock = DockStyle.Top, Height = S(36), TextAlign = ContentAlignment.BottomCenter, ForeColor = Blue, Font = new Font("Microsoft Sans Serif", 10.5f, FontStyle.Bold), Cursor = Cursors.Hand };
                var h = new WLabel { Text = d.Hint, AutoSize = false, Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopCenter, ForeColor = Muted, Font = new Font("Microsoft Sans Serif", 9f), Cursor = Cursors.Hand };
                box.Controls.Add(h);
                box.Controls.Add(t);
                foreach (Control c in new Control[] { box, t, h }) c.Click += async (s, e) => await d.Run();
                steps.Controls.Add(box, i * 2, 0);
                if (i < defs.Length - 1)
                    steps.Controls.Add(new WLabel { Text = "→", AutoSize = false, Dock = DockStyle.Fill, Margin = new Padding(0), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Muted, Font = new Font("Microsoft Sans Serif", 13f) }, i * 2 + 1, 0);
            }
            flowCard.Controls.Add(steps);
            flowCard.Controls.Add(SectionTitle("作業の流れ"));
            grid.Controls.Add(flowCard, 0, 0);

            var readyCard = Card(new Padding(S(6), 0, 0, 0));
            readyCard.Padding = new Padding(1);
            _readyPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(S(12), S(2), 0, 0) };
            readyCard.Controls.Add(_readyPanel);
            readyCard.Controls.Add(SectionTitle("準備状況"));
            grid.Controls.Add(readyCard, 1, 0);
            return grid;
        }

        // ───────── データ ─────────

        private sealed class TodoRow
        {
            public string Kind { get; init; } = "";
            public AntdUI.TTypeMini KindType { get; init; }
            public string Project { get; init; } = "";
            public string Floor { get; init; } = "";
            public string Detail { get; init; } = "";
            public string ActionText { get; init; } = "";
            public Func<Task> Run { get; init; } = () => Task.CompletedTask;
        }

        private sealed class RecentRow
        {
            public long Id { get; init; }
            public string Name { get; init; } = "";
            public string Customer { get; init; } = "";
            public float Ratio { get; init; }
            public string FloorText { get; init; } = "";
            public int NeedCheck { get; init; }
            public DateTime Last { get; init; }
            public string LastText => Last.ToString("yyyy/MM/dd");
        }

        private List<long> NeedCheckMainIds() =>
            _summary?.Checks.Where(c => c.NeedsCheck).Select(c => c.MainId).Distinct().ToList() ?? new List<long>();

        /// <summary>集計を読み直して表示を更新する。表示中・操作の後に呼ぶ。</summary>
        public async Task ReloadHomeAsync()
        {
            if (_host == null) return;
            if (_loading) { _reloadAgain = true; return; }
            _loading = true;
            try
            {
                do
                {
                    _reloadAgain = false;
                    _summary = await Service.GetHomeSummaryAsync();
                    Render(_summary);
                } while (_reloadAgain);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "ワークベンチの集計に失敗しました");
            }
            finally { _loading = false; }
        }

        private void Render(HomeSummary s)
        {
            var now = DateTime.Now;
            _dateLabel.Text = now.ToString("yyyy年M月d日（ddd）", new System.Globalization.CultureInfo("ja-JP"));

            var projects = s.Projects.ToDictionary(p => p.Id);
            var floorsByMain = s.Floors.GroupBy(f => f.MainId).ToDictionary(g => g.Key, g => g.ToList());
            var floorById = s.Floors.ToDictionary(f => f.Id);
            int Imported(long id) => floorsByMain.TryGetValue(id, out var l) ? l.Count : 0;
            var needCheck = s.Checks.Where(c => c.NeedsCheck && floorById.ContainsKey(c.SubId)).ToList();
            var needCheckIds = needCheck.Select(c => c.SubId).ToHashSet();

            var active = s.Projects.Where(p => p.Status != ProjectStatus.Budget).ToList();
            var missing = active.Where(p => Imported(p.Id) < p.FloorQuantity).ToList();
            int missingFloors = missing.Sum(p => p.FloorQuantity - Imported(p.Id));
            var noCsv = s.Floors.Where(f => f.ExportCount == 0 && !needCheckIds.Contains(f.Id)).ToList();
            var thisMonth = s.Floors.Where(f => f.Created.Year == now.Year && f.Created.Month == now.Month).ToList();

            SetKpi(0, active.Count, "件", null, () => _host!.ShowProjectsAsync(active.Select(p => p.Id), "進行中の工事"));
            SetKpi(1, missingFloors, "階", missingFloors > 0 ? Orange : null, () => _host!.ShowProjectsAsync(missing.Select(p => p.Id), "未取り込みの階がある工事"), $"{missing.Count} 工事");
            SetKpi(2, needCheck.Count, "階", needCheck.Count > 0 ? Red : null, () => _host!.ShowProjectsAsync(needCheck.Select(c => c.MainId).Distinct(), "要確認の階がある工事"));
            SetKpi(3, noCsv.Count, "階", noCsv.Count > 0 ? Orange : null, () => _host!.ShowProjectsAsync(noCsv.Select(f => f.MainId).Distinct(), "CSV未出力の階がある工事"));
            SetKpi(4, thisMonth.Count, "階", null, () => _host!.ShowProjectsAsync(thisMonth.Select(f => f.MainId).Distinct(), "今月取り込んだ工事"));

            // 対応が必要なもの：要確認 → 未取り込み → CSV未出力（それぞれ新しい順）
            string PName(long id) => projects.TryGetValue(id, out var p) ? p.Name : "";
            var todo = new List<TodoRow>();
            foreach (var c in needCheck.OrderByDescending(c => floorById[c.SubId].Created))
            {
                string subId = c.SubId;
                todo.Add(new TodoRow
                {
                    Kind = "要確認", KindType = AntdUI.TTypeMini.Error, Project = PName(c.MainId), Floor = floorById[subId].FloorName,
                    Detail = c.Reason, ActionText = "確認", Run = () => _host!.OpenNeedCheckAsync(subId),
                });
            }
            foreach (var p in missing.OrderByDescending(p => p.Created))
            {
                long id = p.Id;
                todo.Add(new TodoRow
                {
                    Kind = "未取り込み", KindType = AntdUI.TTypeMini.Warn, Project = p.Name, Floor = "",
                    Detail = $"{Imported(id)} / {p.FloorQuantity} 階 取り込み済み（残り {p.FloorQuantity - Imported(id)} 階）",
                    ActionText = "取り込む", Run = () => _host!.ImportAsync(id),
                });
            }
            foreach (var f in noCsv.OrderByDescending(f => f.Created))
            {
                string subId = f.Id;
                todo.Add(new TodoRow
                {
                    Kind = "CSV未出力", KindType = AntdUI.TTypeMini.Primary, Project = PName(f.MainId), Floor = f.FloorName,
                    Detail = $"梁製造CSVをまだ出力していません（{f.Created:M/d} 取り込み）", ActionText = "CSV出力", Run = () => _host!.ExportCsvAsync(subId),
                });
            }
            _todoTitle.Text = todo.Count > 0 ? $"対応が必要なもの（{todo.Count} 件）" : "対応が必要なもの";
            _todoTable.DataSource = todo;
            _todoTable.EmptyText = "対応が必要なものはありません";
            bool noProjects = s.Projects.Count == 0;
            _emptyHint.Visible = noProjects;
            _todoTable.Visible = !noProjects;

            // 最近の工事：最後に階を取り込んだ（無ければ登録した）日の新しい順
            var needByMain = needCheck.GroupBy(c => c.MainId).ToDictionary(g => g.Key, g => g.Count());
            _recentTable.DataSource = s.Projects
                .Select(p =>
                {
                    int n = Imported(p.Id);
                    var last = floorsByMain.TryGetValue(p.Id, out var l) ? l.Max(f => f.Created) : p.Created;
                    return new RecentRow
                    {
                        Id = p.Id, Name = p.Name, Customer = p.Customer, Last = last,
                        Ratio = p.FloorQuantity > 0 ? Math.Min(1f, n / (float)p.FloorQuantity) : (n > 0 ? 1f : 0f),
                        FloorText = $"{n}/{p.FloorQuantity}", NeedCheck = needByMain.GetValueOrDefault(p.Id),
                    };
                })
                .OrderByDescending(r => r.Last).Take(30).ToList();

            RenderReady(s);
        }

        private void SetKpi(int idx, int value, string unit, Color? color, Func<Task> open, string? extra = null)
        {
            var (v, sub) = _kpis[idx];
            v.Text = value.ToString();
            v.ForeColor = color ?? Ink;
            sub.Text = extra == null ? unit : $"{unit}（{extra}）";
            while (_kpiActions.Count <= idx) _kpiActions.Add(() => { });
            _kpiActions[idx] = async () => await open();
            v.Parent?.PerformLayout();
        }

        private void RenderReady(HomeSummary s)
        {
            _readyPanel.SuspendLayout();
            foreach (Control c in _readyPanel.Controls.Cast<Control>().ToList()) c.Dispose();
            _readyPanel.Controls.Clear();

            AddReady(s.BeamMaterialCount > 0, s.BeamMaterialCount > 0 ? $"梁の鋼材（基礎データ）：{s.BeamMaterialCount} 件" : "基礎データに梁の鋼材がありません", WorkbenchNav.BaseData);
            bool allSet = s.CustomerCount > 0 && s.CustomersWithSettings >= s.CustomerCount;
            AddReady(allSet, s.CustomerCount == 0 ? "顧客が登録されていません" : $"解析設定のある顧客：{s.CustomersWithSettings} / {s.CustomerCount}", WorkbenchNav.Customers);
            int errors = TodayErrorCount();
            AddReady(errors <= 0, errors <= 0 ? "今日のエラー：なし" : $"今日のエラー：{errors} 件", WorkbenchNav.Logs, warnColor: Red);
            _readyPanel.ResumeLayout();
        }

        private void AddReady(bool ok, string text, WorkbenchNav nav, Color? warnColor = null)
        {
            var row = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0, S(3), 0, S(3)) };
            row.Controls.Add(new WLabel { Text = ok ? "✔" : "⚠", AutoSize = true, ForeColor = ok ? Green : (warnColor ?? Orange), Font = new Font("Microsoft Sans Serif", 10.5f, FontStyle.Bold), Margin = new Padding(0, 0, S(4), 0) });
            var link = new LinkLabel { Text = text, AutoSize = true, LinkColor = ok ? Ink : (warnColor ?? Orange), LinkBehavior = LinkBehavior.HoverUnderline, Font = new Font("Microsoft Sans Serif", 10.5f) };
            link.LinkClicked += (s, e) => _host!.ShowNav(nav);
            row.Controls.Add(link);
            _readyPanel.Controls.Add(row);
        }

        /// <summary>今日のログのエラー件数（読み取りのみ）。ログが無ければ -1。</summary>
        private static int TodayErrorCount()
        {
            try
            {
                var path = Path.Combine(AppLog.LogDir, $"rgbjw-{DateTime.Today:yyyyMMdd}.log");
                if (!File.Exists(path)) return -1;
                return LogReader.Read(path).Count(e => e.Level >= LogLevelKind.Error);
            }
            catch { return 0; }
        }

        // ───────── 操作 ─────────

        private async Task RunAsync(Func<Task> action)
        {
            try { await action(); }
            catch (Exception ex)
            {
                AppLog.Error(ex, "ワークベンチの操作に失敗しました");
                Sunny.UI.UIMessageBox.ShowError(ex.Message);
            }
            await ReloadHomeAsync();
        }

        /// <summary>工事を選んでから操作する（最近取り込んだ順）。</summary>
        private async Task PickAndRunAsync(string title, Func<long, Task> action, bool importable = false, bool parsedOnly = false)
        {
            if (_summary == null) await ReloadHomeAsync();
            var s = _summary;
            if (s == null) return;
            var counts = s.Floors.GroupBy(f => f.MainId).ToDictionary(g => g.Key, g => (Count: g.Count(), Last: g.Max(f => f.Created)));
            var rows = s.Projects
                .Where(p => !importable || p.Status != ProjectStatus.Budget)
                .Where(p => !parsedOnly || counts.ContainsKey(p.Id))
                .OrderByDescending(p => counts.TryGetValue(p.Id, out var c) ? c.Last : p.Created)
                .Select(p => (p.Id, p.Name, p.Customer, Progress: $"{(counts.TryGetValue(p.Id, out var c) ? c.Count : 0)} / {p.FloorQuantity} 階"))
                .ToList();
            if (rows.Count == 0)
            {
                Sunny.UI.UIMessageBox.ShowWarning(parsedOnly ? "図面を取り込んだ工事がありません。" : "工事がありません。先に「工事を追加」で登録してください。");
                return;
            }
            long id;
            using (var f = new ProjectPickerForm(title, rows))
            {
                if (f.ShowDialog(FindForm()) != DialogResult.OK || f.SelectedId == null) return;
                id = f.SelectedId.Value;
            }
            await RunAsync(() => action(id));
        }
    }
}
