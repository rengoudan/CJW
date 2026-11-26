using JwCore;
using JwLogging;
using JwServices;
using RGBControls.Classes;
using RGBJWMain.Forms;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WLabel = System.Windows.Forms.Label;

namespace RGBJWMain.Pages
{
    /// <summary>
    /// 2026年11月27日 顧客管理を再構成（ワークベンチと同じ見た目・操作の流れ）。
    /// ・上段：見出し・［＋ 顧客を追加］［更新］
    /// ・数字カード（登録数・解析設定あり／なし・工事で使用中）→ 押すと一覧を絞り込む（同じカードをもう一度押すと解除）
    /// ・左：顧客一覧（会社名・住所・連絡先・電話・解析設定・工事数）＋ 検索（空白で区切るとすべて含む行を表示）
    /// ・右：選んだ顧客の詳細・使っている工事・［解析設定］［基本情報を編集］［削除］
    /// 操作：行クリックで選択、ダブルクリックで基本情報の編集、右クリックで同じ操作のメニュー。
    /// 削除は工事に使われている顧客を拒否し、解析設定も一緒に消えることを確認してから行う。
    /// 以前は表で直接書き換えていたが保存されていなかったため、編集は専用のダイアログで行う。
    /// </summary>
    public partial class JwCustomerPage : BasePage
    {
        private enum CustomerFilter { All, WithSettings, WithoutSettings, InUse }

        private sealed class CustomerRow
        {
            public CustomerRow(JwCustomerData data, List<CustomerProjectItem> projects, bool hasSettings)
            {
                Data = data;
                Projects = projects;
                HasSettings = hasSettings;
            }

            public JwCustomerData Data { get; }
            public List<CustomerProjectItem> Projects { get; }
            public bool HasSettings { get; }
            public long Id => Data.Id;
            public string CompanyName => Data.CompanyName ?? "";
            public string CompanyAddress => Data.CompanyAddress ?? "";
            public string Contact => Data.Contact ?? "";
            public string Telephone => Data.Telephone ?? "";
            public int ProjectCount => Projects.Count;
            public string SearchText => $"{CompanyName} {CompanyAddress} {Contact} {Telephone}";
        }

        private const string FontName = WorkbenchTheme.FontName;
        private static readonly Color Ink = WorkbenchTheme.Ink;
        private static readonly Color Muted = WorkbenchTheme.Muted;
        private static readonly Color PageBack = WorkbenchTheme.PageBack;
        private static readonly Color Orange = WorkbenchTheme.Orange;

        private float _k = 1f;
        private bool _loading, _reloadAgain;
        private CustomerFilter _filter = CustomerFilter.All;
        private List<CustomerRow> _all = new();
        private List<CustomerRow> _shown = new();
        private CustomerRow? _selected;

        private TableLayoutPanel _root = null!;
        private WLabel _subtitle = null!, _listTitle = null!, _detailName = null!, _detailInfo = null!, _projectsTitle = null!;
        private readonly WLabel[] _kpiValues = new WLabel[4];
        private readonly WLabel[] _kpiSubs = new WLabel[4];
        private readonly Panel[] _kpiCards = new Panel[4];
        private AntdUI.Input _search = null!;
        private AntdUI.Table _table = null!, _projectTable = null!;
        private AntdUI.Button _btnEdit = null!, _btnSetting = null!, _btnDelete = null!;

        public JwCustomerPage()
        {
            InitializeComponent();
            base.InitData();
            _k = DeviceDpi / 96f;
            // 旧い表示（BasePage の見出し・保存ボタン・表）は使わない
            uiLine1.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;
            BuildPage();
            VisibleChanged += async (s, e) => { if (Visible) await ReloadAsync(); };
        }

        public JwqitaService jwqitaService => ServiceFactory.GetInstance().CreateJwqitaService();

        private int S(float v) => WorkbenchTheme.Px(v, _k);

        // ───────── 組み立て ─────────

        private void BuildPage()
        {
            BackColor = PageBack;
            _root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, BackColor = PageBack,
                Padding = new Padding(S(18), S(12), S(18), S(14)), AutoScroll = true,
            };
            _root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, S(58)));   // 見出し＋操作
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, S(104)));  // 数字カード
            _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));      // 一覧／詳細
            _root.Controls.Add(BuildHeader(), 0, 0);
            _root.Controls.Add(BuildKpis(), 0, 1);
            _root.Controls.Add(BuildBody(), 0, 2);
            Controls.Add(_root);
            _root.BringToFront();
        }

        private Control BuildHeader()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };
            var title = new WLabel { Text = "顧客管理", AutoSize = true, ForeColor = Ink, Font = new Font(FontName, 17f, FontStyle.Bold), Location = new Point(0, S(6)) };
            _subtitle = new WLabel { AutoSize = true, ForeColor = Muted, Font = new Font(FontName, 10f) };
            panel.Controls.Add(title);
            panel.Controls.Add(_subtitle);

            var actions = new FlowLayoutPanel { AutoSize = true, WrapContents = false, FlowDirection = FlowDirection.LeftToRight, BackColor = Color.Transparent };
            actions.Controls.Add(WorkbenchTheme.ActionButton("＋ 顧客を追加", AntdUI.TTypeMini.Primary, () => { _ = RunAsync(AddAsync); }, _k));
            actions.Controls.Add(WorkbenchTheme.ActionButton("更新", AntdUI.TTypeMini.Default, () => { _ = ReloadAsync(); }, _k, ghost: true));
            panel.Controls.Add(actions);

            panel.Layout += (s, e) =>
            {
                _subtitle.Location = new Point(title.Right + S(12), title.Bottom - _subtitle.Height - S(3));
                actions.Location = new Point(Math.Max(0, panel.ClientSize.Width - actions.Width), Math.Max(0, (panel.ClientSize.Height - actions.Height) / 2));
            };
            return panel;
        }

        private Control BuildKpis()
        {
            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1, Margin = new Padding(0, S(4), 0, S(8)) };
            for (int i = 0; i < 4; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            string[] titles = { "登録している顧客", "解析設定あり", "解析設定なし", "工事で使用中" };
            CustomerFilter[] filters = { CustomerFilter.All, CustomerFilter.WithSettings, CustomerFilter.WithoutSettings, CustomerFilter.InUse };
            for (int i = 0; i < titles.Length; i++)
            {
                var f = filters[i];
                var card = WorkbenchTheme.Card(new Padding(i == 0 ? 0 : S(6), 0, i == 3 ? 0 : S(6), 0), () => _filter == f);
                var t = new WLabel { Text = titles[i], AutoSize = true, ForeColor = Muted, Location = new Point(S(14), S(10)), Font = new Font(FontName, 10f) };
                var v = new WLabel { Text = "-", AutoSize = true, ForeColor = Ink, Location = new Point(S(12), S(30)), Font = new Font(FontName, 22f, FontStyle.Bold) };
                var sub = new WLabel { Text = "", AutoSize = true, ForeColor = Muted, Font = new Font(FontName, 9f) };
                card.Controls.AddRange(new Control[] { t, v, sub });
                card.Layout += (s, e) => sub.Location = new Point(v.Right + S(4), v.Bottom - sub.Height - S(8));
                // 押すと絞り込み、同じカードをもう一度押すと全件に戻す
                void Toggle(object? s, EventArgs e) { _filter = _filter == f ? CustomerFilter.All : f; RefreshView(); }
                foreach (Control c in new Control[] { card, t, v, sub })
                {
                    c.Cursor = Cursors.Hand;
                    c.Click += Toggle;
                }
                _kpiValues[i] = v;
                _kpiSubs[i] = sub;
                _kpiCards[i] = card;
                grid.Controls.Add(card, i, 0);
            }
            return grid;
        }

        private Control BuildBody()
        {
            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Margin = new Padding(0, 0, 0, S(8)) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
            grid.Controls.Add(BuildList(), 0, 0);
            grid.Controls.Add(BuildDetail(), 1, 0);
            return grid;
        }

        private Control BuildList()
        {
            var card = WorkbenchTheme.Card(new Padding(0, 0, S(6), 0));
            _listTitle = WorkbenchTheme.SectionTitle("顧客一覧", _k);

            var searchBar = new Panel { Dock = DockStyle.Top, Height = S(50), Padding = new Padding(S(10), S(8), S(10), S(8)) };
            _search = new AntdUI.Input
            {
                Dock = DockStyle.Fill, PlaceholderText = "会社名・住所・連絡先・電話で検索（空白で区切ると、すべて含む顧客を表示）",
                Font = new Font(FontName, 10f),
            };
            _search.TextChanged += (s, e) => RefreshView();
            searchBar.Controls.Add(_search);

            _table = new AntdUI.Table
            {
                Dock = DockStyle.Fill, Bordered = false, EmptyHeader = true,
                AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill, Font = new Font(FontName, 10f),
            };
            _table.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("CompanyName", "会社名") { Width = "200" },
                new AntdUI.Column("CompanyAddress", "住所"),
                new AntdUI.Column("Contact", "連絡先") { Width = "130" },
                new AntdUI.Column("Telephone", "電話") { Width = "120" },
                new AntdUI.Column("HasSettings", "解析設定")
                {
                    Width = "90", Align = AntdUI.ColumnAlign.Center,
                    Render = (v, r, i) => r is CustomerRow x
                        ? new AntdUI.CellTag(x.HasSettings ? "設定済" : "未設定", x.HasSettings ? AntdUI.TTypeMini.Success : AntdUI.TTypeMini.Warn)
                        : null,
                },
                new AntdUI.Column("ProjectCount", "工事") { Width = "60", Align = AntdUI.ColumnAlign.Center },
            };
            _table.CellClick += (s, e) =>
            {
                if (e.Record is not CustomerRow row) return;
                SelectCustomer(row);
                if (e.Button == MouseButtons.Right) contextMenuStrip1.Show(MousePosition);
            };
            _table.CellDoubleClick += (s, e) =>
            {
                if (e.Record is CustomerRow row)
                {
                    SelectCustomer(row);
                    _ = RunAsync(() => EditAsync(row));
                }
            };

            // 表は Fill を先に、上端の見出し・検索を後から（ワークベンチのカードと同じ積み方）
            card.Controls.Add(_table);
            card.Controls.Add(searchBar);
            card.Controls.Add(_listTitle);
            return card;
        }

        private Control BuildDetail()
        {
            var card = WorkbenchTheme.Card(new Padding(S(6), 0, 0, 0));
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, BackColor = Color.White,
                Padding = new Padding(S(14), S(10), S(14), S(10)),
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, S(36)));  // 会社名
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, S(110))); // 詳細
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, S(30)));  // 使っている工事の見出し
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));     // 使っている工事
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, S(50)));  // 操作

            _detailName = new WLabel { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Ink, Font = new Font(FontName, 13f, FontStyle.Bold), AutoEllipsis = true };
            _detailInfo = new WLabel { Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopLeft, ForeColor = Ink, Font = new Font(FontName, 10f) };
            _projectsTitle = new WLabel { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Ink, Font = new Font(FontName, 10.5f, FontStyle.Bold) };

            _projectTable = new AntdUI.Table
            {
                Dock = DockStyle.Fill, Bordered = false, EmptyHeader = true, EmptyText = "この顧客を使っている工事はありません",
                AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill, Font = new Font(FontName, 10f),
            };
            _projectTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("ProjectName", "工事名"),
                new AntdUI.Column("FloorQuantity", "階数") { Width = "70", Align = AntdUI.ColumnAlign.Center },
            };

            _btnSetting = WorkbenchTheme.ActionButton("解析設定", AntdUI.TTypeMini.Primary, () => RunSelectedAsync(OpenSettingsAsync), _k);
            _btnEdit = WorkbenchTheme.ActionButton("基本情報を編集", AntdUI.TTypeMini.Default, () => RunSelectedAsync(EditAsync), _k);
            _btnDelete = WorkbenchTheme.ActionButton("削除", AntdUI.TTypeMini.Error, () => RunSelectedAsync(DeleteAsync), _k, ghost: true);
            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, S(6), 0, 0) };
            buttons.Controls.Add(_btnSetting);
            buttons.Controls.Add(_btnEdit);
            buttons.Controls.Add(_btnDelete);

            layout.Controls.Add(_detailName, 0, 0);
            layout.Controls.Add(_detailInfo, 0, 1);
            layout.Controls.Add(_projectsTitle, 0, 2);
            layout.Controls.Add(_projectTable, 0, 3);
            layout.Controls.Add(buttons, 0, 4);
            card.Controls.Add(layout);

            UpdateButtons();
            return card;
        }

        // ───────── 表示 ─────────

        /// <summary>集計と一覧を読み直す（表示中・操作の後・設定画面を閉じた後に呼ぶ）。</summary>
        private async Task ReloadAsync()
        {
            if (_loading) { _reloadAgain = true; return; }
            _loading = true;
            try
            {
                do
                {
                    _reloadAgain = false;
                    var customers = await jwqitaService.GetAllJwCustomerDatasAsync(null);
                    var projects = await jwqitaService.GetCustomerProjectItemsAsync();
                    var withSettings = await jwqitaService.GetCustomerIdsWithSettingsAsync();
                    var byCustomer = projects.GroupBy(p => p.CustomerId)
                        .ToDictionary(g => g.Key, g => g.OrderBy(p => p.ProjectName).ToList());
                    _all = customers
                        .OrderBy(c => c.CompanyName)
                        .Select(c => new CustomerRow(
                            c,
                            byCustomer.TryGetValue(c.Id, out var ps) ? ps : new List<CustomerProjectItem>(),
                            withSettings.Contains(c.Id)))
                        .ToList();
                    RefreshView();
                } while (_reloadAgain);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "顧客一覧の読み込みに失敗しました");
            }
            finally { _loading = false; }
        }

        /// <summary>読み込み済みの顧客を、絞り込み・検索に従って表示する（読み取りのみ）。</summary>
        private void RefreshView()
        {
            int withSettings = _all.Count(r => r.HasSettings);
            int without = _all.Count - withSettings;
            int inUse = _all.Count(r => r.ProjectCount > 0);
            SetKpi(0, _all.Count, "社", null);
            SetKpi(1, withSettings, "社", null);
            SetKpi(2, without, without > 0 ? "社（要設定）" : "社", without > 0 ? (Color?)Orange : null);
            SetKpi(3, inUse, "社", null);
            foreach (var c in _kpiCards) c.Invalidate();

            _subtitle.Text = "行を選ぶと右に詳細が出ます。ダブルクリックで基本情報を編集";

            var tokens = (_search.Text ?? "").Split(new[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries);
            _shown = _all
                .Where(MatchFilter)
                .Where(r => tokens.All(t => r.SearchText.Contains(t, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            _listTitle.Text = $"顧客一覧（{_shown.Count} 件）";
            _table.EmptyText = _all.Count == 0
                ? "顧客が登録されていません。\r\n「＋ 顧客を追加」から登録してください。"
                : "条件に合う顧客がありません";
            _table.DataSource = _shown;

            // 再読込で行の実体が変わるので、同じ顧客を指し直す。一覧から消えた顧客は選択を外す
            if (_selected != null) _selected = _all.FirstOrDefault(r => r.Id == _selected.Id);
            if (_selected != null && !_shown.Contains(_selected)) _selected = null;
            if (_selected != null) _table.SelectedIndex = _shown.IndexOf(_selected) + 1;

            RenderDetail();
            UpdateButtons();
        }

        private bool MatchFilter(CustomerRow r) => _filter switch
        {
            CustomerFilter.WithSettings => r.HasSettings,
            CustomerFilter.WithoutSettings => !r.HasSettings,
            CustomerFilter.InUse => r.ProjectCount > 0,
            _ => true,
        };

        private void SetKpi(int i, int value, string unit, Color? color)
        {
            _kpiValues[i].Text = value.ToString();
            _kpiValues[i].ForeColor = color ?? Ink;
            _kpiSubs[i].Text = unit;
            _kpiCards[i].PerformLayout();
        }

        private void RenderDetail()
        {
            var r = _selected;
            if (r == null)
            {
                _detailName.Text = "顧客を選択してください";
                _detailName.ForeColor = Muted;
                _detailInfo.Text = "一覧の行をクリックすると、住所・連絡先・解析設定の状態と、\r\nその顧客を使っている工事がここに表示されます。";
                _detailInfo.ForeColor = Muted;
                _projectsTitle.Text = "使っている工事";
                _projectTable.DataSource = new List<CustomerProjectItem>();
                return;
            }

            _detailName.Text = r.CompanyName;
            _detailName.ForeColor = Ink;
            _detailInfo.ForeColor = Ink;
            _detailInfo.Text =
                $"住所　　{OrDash(r.CompanyAddress)}\r\n" +
                $"連絡先　{OrDash(r.Contact)}\r\n" +
                $"電話　　{OrDash(r.Telephone)}\r\n" +
                $"解析設定　{(r.HasSettings ? "設定済み" : "未設定（工事の解析の前に設定してください）")}";
            _projectsTitle.Text = $"使っている工事（{r.ProjectCount} 件）";
            _projectTable.DataSource = r.Projects;
        }

        private void UpdateButtons()
        {
            bool has = _selected != null;
            if (_btnSetting != null) _btnSetting.Enabled = has;
            if (_btnEdit != null) _btnEdit.Enabled = has;
            if (_btnDelete != null) _btnDelete.Enabled = has;
        }

        private void SelectCustomer(CustomerRow row)
        {
            _selected = row;
            RenderDetail();
            UpdateButtons();
        }

        private static string OrDash(string s) => string.IsNullOrWhiteSpace(s) ? "－" : s;

        // ───────── 操作 ─────────

        /// <summary>操作の例外をメッセージにし、最後に一覧を読み直す。</summary>
        private async Task RunAsync(Func<Task> action)
        {
            try { await action(); }
            catch (Exception ex)
            {
                AppLog.Error(ex, "顧客管理の操作に失敗しました");
                UIMessageBox.ShowError(ex.Message);
            }
            await ReloadAsync();
        }

        /// <summary>選択中の顧客に対して操作する（選択が無ければ何もしない）。</summary>
        private Task RunSelectedAsync(Func<CustomerRow, Task> action)
        {
            var row = _selected;
            return row == null ? Task.CompletedTask : RunAsync(() => action(row));
        }

        private async Task AddAsync()
        {
            var option = new UIEditOption { AutoLabelWidth = true, Text = "追加" };
            option.AddText("CompanyName", "会社名", null, true);
            option.AddText("CompanyAddress", "会社住所", null, false);
            option.AddText("Contact", "会社の連絡先", null, false);
            option.AddText("Telephone", "電話", null, false);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += (s, e) => CheckCustomerForm(e, 0);
            frm.ShowDialog();
            if (!frm.IsOK) return;

            var data = new JwCustomerData
            {
                CompanyName = Text2(frm, "CompanyName"),
                CompanyAddress = Text2(frm, "CompanyAddress"),
                Contact = Text2(frm, "Contact"),
                Telephone = Text2(frm, "Telephone"),
            };
            await jwqitaService.AddJwCustomerDataAsync(data);
        }

        private async Task EditAsync(CustomerRow row)
        {
            var c = row.Data;
            var option = new UIEditOption { AutoLabelWidth = true, Text = "編集" };
            option.AddText("CompanyName", "会社名", c.CompanyName, true);
            option.AddText("CompanyAddress", "会社住所", c.CompanyAddress, false);
            option.AddText("Contact", "会社の連絡先", c.Contact, false);
            option.AddText("Telephone", "電話", c.Telephone, false);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += (s, e) => CheckCustomerForm(e, row.Id);
            frm.ShowDialog();
            if (!frm.IsOK) return;

            c.CompanyName = Text2(frm, "CompanyName");
            c.CompanyAddress = Text2(frm, "CompanyAddress");
            c.Contact = Text2(frm, "Contact");
            c.Telephone = Text2(frm, "Telephone");
            await jwqitaService.UpdateJwCustomerDataAsync(c);
        }

        private async Task DeleteAsync(CustomerRow row)
        {
            if (row.ProjectCount > 0)
            {
                UIMessageBox.ShowWarning($"「{row.CompanyName}」は {row.ProjectCount} 件の工事で使われているため削除できません。\n先に工事の顧客を変更してから削除してください。");
                return;
            }

            string extra = row.HasSettings ? "\n解析設定も一緒に削除されます。" : "";
            if (!UIMessageBox.ShowAsk($"顧客「{row.CompanyName}」を削除します。{extra}\nこの操作は元に戻せません。よろしいですか？")) return;

            bool ok = await jwqitaService.DeleteJwCustomerDataAsync(row.Id);
            if (!ok) UIMessageBox.ShowWarning("工事で使われているため削除できませんでした。一覧を更新します。");
            _selected = null;
        }

        private Task OpenSettingsAsync(CustomerRow row)
        {
            // 解析設定の画面（既存）。閉じた後に RunAsync が一覧を読み直し、「解析設定」の表示を更新する
            var setting = new CustomerDesignSetting(row.Data);
            setting.ShowDialog();
            return Task.CompletedTask;
        }

        /// <summary>追加・編集ダイアログの入力チェック。excludeId は編集中の顧客（重複判定から外す）。</summary>
        private bool CheckCustomerForm(UIEditForm.EditFormEventArgs e, long excludeId)
        {
            string name = Text2(e.Form, "CompanyName");
            if (name.Length == 0)
            {
                e.Form.SetEditorFocus("CompanyName");
                ShowWarningTip("会社名を空にすることはできません");
                return false;
            }

            var dup = _all.FirstOrDefault(r => r.Id != excludeId &&
                string.Equals(r.CompanyName.Trim(), name, StringComparison.OrdinalIgnoreCase));
            if (dup != null)
            {
                e.Form.SetEditorFocus("CompanyName");
                ShowWarningTip($"同じ会社名の顧客が既にあります：{dup.CompanyName}");
                return false;
            }
            return true;
        }

        private static string Text2(UIEditForm form, string key) => (Convert.ToString(form[key]) ?? "").Trim();

        // ───────── 右クリックメニュー（Designer の contextMenuStrip1） ─────────

        private void menuEdit_Click(object sender, EventArgs e) => _ = RunSelectedAsync(EditAsync);

        private void menuSetting_Click(object sender, EventArgs e) => _ = RunSelectedAsync(OpenSettingsAsync);

        private void menuDelete_Click(object sender, EventArgs e) => _ = RunSelectedAsync(DeleteAsync);
    }
}
