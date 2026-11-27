using JwCore;
using JwLogging;
using JwServices;
using JwShapeCommon;
using Microsoft.EntityFrameworkCore;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RGBControls.Classes;
using RGBJWMain.Forms;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WLabel = System.Windows.Forms.Label;

namespace RGBJWMain.Pages
{
    /// <summary>
    /// 2026年11月27日 予算管理を再構成（顧客管理・ワークベンチと同じ見た目・操作の流れ）。
    /// ・上段：見出し・［＋ 予算を作成］［更新］
    /// ・数字カード（予算の件数・合計金額・工事数・最新の作成日）
    /// ・左：予算一覧（工事名・合計金額・作成日）＋ 検索（空白で区切るとすべて含む行を表示）
    /// ・右：選んだ予算の明細（予算項目・モデル・単位・単価・量・金額）・［EXCEL出力］［新しい予算を追加］
    /// 操作：行クリックで明細を表示（明細は選んだ時だけ読む）、右クリックで同じ操作のメニュー。
    /// 明細は読み取り専用（以前の表の直接編集は保存されていなかったため）。
    /// </summary>
    public partial class JwBudgePage : BasePage
    {
        private sealed class BudgetRow
        {
            public BudgetRow(JwBudgetMainData data)
            {
                Data = data;
            }

            public JwBudgetMainData Data { get; }
            /// <summary>明細（JwBudgetSubDatas）を読み込み済みか。再読込で行の実体が変わるとfalse に戻る。</summary>
            public bool DetailLoaded { get; set; }
            public string ProjectName => Data.ProjectName ?? "";
            public string AmountText => Data.Amount.ToString("N0");
            public string DateText => Data.CreationTime.ToString("yyyy/MM/dd");
            public string SearchText => $"{ProjectName} {DateText}";
        }

        private sealed class BudgetSubRow
        {
            public BudgetSubRow(JwBudgetSubData data)
            {
                Data = data;
            }

            public JwBudgetSubData Data { get; }
            public string BudgetItemName => Data.BudgetItemName ?? "";
            public string ModelParm => Data.ModelParm ?? "";
            public string UnitName => Data.UnitName ?? "";
            public string UnitPriceText => Data.UnitPrice.ToString("N0");
            public string NumberText => Data.Number.ToString("0.##");
            public string AmountText => Data.Amount.ToString("N0");
            public string SourceText => Data.BudgetType == BudgetFrom.自動識別 ? "自動識別" : "手動追加";
        }

        private const string FontName = WorkbenchTheme.FontName;
        private static readonly Color Ink = WorkbenchTheme.Ink;
        private static readonly Color Muted = WorkbenchTheme.Muted;
        private static readonly Color PageBack = WorkbenchTheme.PageBack;

        private float _k = 1f;
        private bool _loading, _reloadAgain;
        private List<BudgetRow> _all = new();
        private List<BudgetRow> _shown = new();
        private BudgetRow? _selected;

        private TableLayoutPanel _root = null!;
        private WLabel _subtitle = null!, _listTitle = null!, _detailName = null!, _detailInfo = null!, _detailTitle = null!;
        private readonly WLabel[] _kpiValues = new WLabel[4];
        private readonly WLabel[] _kpiSubs = new WLabel[4];
        private readonly Panel[] _kpiCards = new Panel[4];
        private AntdUI.Input _search = null!;
        private AntdUI.Table _table = null!, _subTable = null!;
        private AntdUI.Button _btnExcel = null!, _btnAdd = null!;

        public JwBudgePage()
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

        public JwProjectMainService jwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();

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
            _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));      // 一覧／明細
            _root.Controls.Add(BuildHeader(), 0, 0);
            _root.Controls.Add(BuildKpis(), 0, 1);
            _root.Controls.Add(BuildBody(), 0, 2);
            Controls.Add(_root);
            _root.BringToFront();
        }

        private Control BuildHeader()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };
            var title = new WLabel { Text = "予算管理", AutoSize = true, ForeColor = Ink, Font = new Font(FontName, 17f, FontStyle.Bold), Location = new Point(0, S(6)) };
            _subtitle = new WLabel { AutoSize = true, ForeColor = Muted, Font = new Font(FontName, 10f) };
            panel.Controls.Add(title);
            panel.Controls.Add(_subtitle);

            var actions = new FlowLayoutPanel { AutoSize = true, WrapContents = false, FlowDirection = FlowDirection.LeftToRight, BackColor = Color.Transparent };
            actions.Controls.Add(WorkbenchTheme.ActionButton("＋ 予算を作成", AntdUI.TTypeMini.Primary, () => { _ = RunAsync(CreateFromProjectAsync); }, _k));
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
            string[] titles = { "予算の件数", "予算の合計金額", "工事数", "最新の作成日" };
            for (int i = 0; i < titles.Length; i++)
            {
                var card = WorkbenchTheme.Card(new Padding(i == 0 ? 0 : S(6), 0, i == 3 ? 0 : S(6), 0));
                var t = new WLabel { Text = titles[i], AutoSize = true, ForeColor = Muted, Location = new Point(S(14), S(10)), Font = new Font(FontName, 10f) };
                var v = new WLabel { Text = "-", AutoSize = true, ForeColor = Ink, Location = new Point(S(12), S(30)), Font = new Font(FontName, 20f, FontStyle.Bold) };
                var sub = new WLabel { Text = "", AutoSize = true, ForeColor = Muted, Font = new Font(FontName, 9f) };
                card.Controls.AddRange(new Control[] { t, v, sub });
                card.Layout += (s, e) => sub.Location = new Point(v.Right + S(4), v.Bottom - sub.Height - S(8));
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
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.Controls.Add(BuildList(), 0, 0);
            grid.Controls.Add(BuildDetail(), 1, 0);
            return grid;
        }

        private Control BuildList()
        {
            var card = WorkbenchTheme.Card(new Padding(0, 0, S(6), 0));
            _listTitle = WorkbenchTheme.SectionTitle("予算一覧", _k);

            var searchBar = new Panel { Dock = DockStyle.Top, Height = S(50), Padding = new Padding(S(10), S(8), S(10), S(8)) };
            _search = new AntdUI.Input
            {
                Dock = DockStyle.Fill, PlaceholderText = "工事名・作成日で検索（空白で区切ると、すべて含む予算を表示）",
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
                new AntdUI.Column("ProjectName", "工事名"),
                new AntdUI.Column("AmountText", "合計金額") { Width = "130", Align = AntdUI.ColumnAlign.Right },
                new AntdUI.Column("DateText", "作成日") { Width = "110", Align = AntdUI.ColumnAlign.Center },
            };
            _table.CellClick += (s, e) =>
            {
                if (e.Record is not BudgetRow row) return;
                _ = SelectBudgetAsync(row);
                if (e.Button == MouseButtons.Right) contextMenuStrip1.Show(MousePosition);
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
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, S(36)));  // 工事名
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, S(90)));  // 詳細
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, S(30)));  // 明細の見出し
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));     // 明細
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, S(50)));  // 操作

            _detailName = new WLabel { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Ink, Font = new Font(FontName, 13f, FontStyle.Bold), AutoEllipsis = true };
            _detailInfo = new WLabel { Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopLeft, ForeColor = Ink, Font = new Font(FontName, 10f) };
            _detailTitle = new WLabel { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Ink, Font = new Font(FontName, 10.5f, FontStyle.Bold) };

            _subTable = new AntdUI.Table
            {
                Dock = DockStyle.Fill, Bordered = false, EmptyHeader = true, EmptyText = "明細はありません",
                AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill, Font = new Font(FontName, 10f),
            };
            _subTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("BudgetItemName", "予算項目"),
                new AntdUI.Column("ModelParm", "モデル"),
                new AntdUI.Column("UnitName", "単位") { Width = "70", Align = AntdUI.ColumnAlign.Center },
                new AntdUI.Column("UnitPriceText", "単価") { Width = "100", Align = AntdUI.ColumnAlign.Right },
                new AntdUI.Column("NumberText", "量") { Width = "70", Align = AntdUI.ColumnAlign.Right },
                new AntdUI.Column("AmountText", "金額") { Width = "120", Align = AntdUI.ColumnAlign.Right },
                new AntdUI.Column("SourceText", "区分") { Width = "90", Align = AntdUI.ColumnAlign.Center },
            };

            _btnExcel = WorkbenchTheme.ActionButton("EXCEL出力", AntdUI.TTypeMini.Primary, () => RunSelectedAsync(ExportExcelAsync, reload: false), _k);
            _btnAdd = WorkbenchTheme.ActionButton("新しい予算を追加", AntdUI.TTypeMini.Default, () => RunSelectedAsync(AddBudgetAsync), _k);
            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, BackColor = Color.White, Padding = new Padding(0, S(6), 0, 0) };
            buttons.Controls.Add(_btnExcel);
            buttons.Controls.Add(_btnAdd);

            layout.Controls.Add(_detailName, 0, 0);
            layout.Controls.Add(_detailInfo, 0, 1);
            layout.Controls.Add(_detailTitle, 0, 2);
            layout.Controls.Add(_subTable, 0, 3);
            layout.Controls.Add(buttons, 0, 4);
            card.Controls.Add(layout);

            UpdateButtons();
            return card;
        }

        // ───────── 表示 ─────────

        /// <summary>予算を読み直す（表示中・操作の後に呼ぶ）。選択中の予算は明細も読み直す。</summary>
        private async Task ReloadAsync()
        {
            if (_loading) { _reloadAgain = true; return; }
            _loading = true;
            try
            {
                do
                {
                    _reloadAgain = false;
                    var list = await jwqitaService.GetAllJwBudgetMainDataAsync(null);
                    _all = list.OrderByDescending(b => b.CreationTime).Select(b => new BudgetRow(b)).ToList();
                    // 再読込で行の実体が変わるので、同じ予算を指し直す。一覧から消えた予算は選択を外す
                    var keep = _selected == null ? null : _all.FirstOrDefault(r => r.Data.Id.Equals(_selected.Data.Id));
                    _selected = keep;
                    RefreshView();
                    if (keep != null) await SelectBudgetAsync(keep);
                } while (_reloadAgain);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "予算一覧の読み込みに失敗しました");
            }
            finally { _loading = false; }
        }

        /// <summary>読み込み済みの予算を、検索に従って表示する（読み取りのみ）。</summary>
        private void RefreshView()
        {
            decimal total = _all.Sum(r => r.Data.Amount);
            int projects = _all.Select(r => r.Data.JwProjectMainDataId).Distinct().Count();
            var latest = _all.Count > 0 ? _all.Max(r => r.Data.CreationTime) : (DateTime?)null;
            SetKpi(0, _all.Count.ToString(), "件");
            SetKpi(1, total.ToString("N0"), "円");
            SetKpi(2, projects.ToString(), "工事");
            SetKpi(3, latest?.ToString("yyyy/MM/dd") ?? "－", "");

            _subtitle.Text = "行を選ぶと明細が出ます。右クリックで EXCEL 出力・明細の追加";

            var tokens = (_search.Text ?? "").Split(new[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries);
            _shown = _all
                .Where(r => tokens.All(t => r.SearchText.Contains(t, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            _listTitle.Text = $"予算一覧（{_shown.Count} 件）";
            _table.EmptyText = _all.Count == 0
                ? "予算がありません。\r\n「＋ 予算を作成」から工事の予算を作成してください。"
                : "条件に合う予算がありません";
            _table.DataSource = _shown;

            if (_selected != null && !_shown.Contains(_selected)) _selected = null;
            if (_selected != null) _table.SelectedIndex = _shown.IndexOf(_selected) + 1;

            RenderDetail();
            UpdateButtons();
        }

        private void SetKpi(int i, string value, string unit)
        {
            _kpiValues[i].Text = value;
            _kpiSubs[i].Text = unit;
            _kpiCards[i].PerformLayout();
        }

        private void RenderDetail()
        {
            var r = _selected;
            if (r == null)
            {
                _detailName.Text = "予算を選択してください";
                _detailName.ForeColor = Muted;
                _detailInfo.Text = "一覧の行をクリックすると、工事の予算の明細がここに表示されます。";
                _detailInfo.ForeColor = Muted;
                _detailTitle.Text = "予算明細";
                _subTable.DataSource = new List<BudgetSubRow>();
                return;
            }

            _detailName.Text = r.ProjectName + " の予算";
            _detailName.ForeColor = Ink;
            _detailInfo.ForeColor = Ink;
            if (!r.DetailLoaded)
            {
                _detailInfo.Text = "明細を読み込んでいます…";
                _detailTitle.Text = "予算明細";
                _subTable.DataSource = new List<BudgetSubRow>();
                return;
            }

            var subs = r.Data.JwBudgetSubDatas.ToList();
            int auto = subs.Count(s => s.BudgetType == BudgetFrom.自動識別);
            _detailInfo.Text =
                $"合計金額　{r.Data.Amount:N0} 円\r\n" +
                $"作成日　　{r.DateText}\r\n" +
                $"明細　　　{subs.Count} 件（自動識別 {auto} ／ 手動追加 {subs.Count - auto}）";
            _detailTitle.Text = $"予算明細（{subs.Count} 件）";
            _subTable.DataSource = subs.Select(s => new BudgetSubRow(s)).ToList();
        }

        private void UpdateButtons()
        {
            bool has = _selected != null;
            if (_btnExcel != null) _btnExcel.Enabled = has;
            if (_btnAdd != null) _btnAdd.Enabled = has;
        }

        /// <summary>行を選んで明細を表示する。明細は初回の選択時だけ読む。</summary>
        private async Task SelectBudgetAsync(BudgetRow row)
        {
            _selected = row;
            RenderDetail();
            UpdateButtons();
            if (row.DetailLoaded) return;

            try
            {
                await jwqitaService.LoadBudgetSubCollectionAsync(row.Data);
                row.DetailLoaded = true;
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "予算明細の読み込みに失敗しました");
                UIMessageBox.ShowError(ex.Message);
                return;
            }
            if (ReferenceEquals(_selected, row)) RenderDetail();
        }

        // ───────── 操作 ─────────

        /// <summary>操作の例外をメッセージにする。</summary>
        private async Task GuardAsync(Func<Task> action)
        {
            try { await action(); }
            catch (Exception ex)
            {
                AppLog.Error(ex, "予算管理の操作に失敗しました");
                UIMessageBox.ShowError(ex.Message);
            }
        }

        /// <summary>操作の例外をメッセージにし、最後に一覧を読み直す。</summary>
        private async Task RunAsync(Func<Task> action)
        {
            await GuardAsync(action);
            await ReloadAsync();
        }

        /// <summary>選択中の予算に対して操作する（選択が無ければ何もしない）。reload=false は一覧を読み直さない（出力など）。</summary>
        private Task RunSelectedAsync(Func<BudgetRow, Task> action, bool reload = true)
        {
            var row = _selected;
            if (row == null) return Task.CompletedTask;
            return reload ? RunAsync(() => action(row)) : GuardAsync(() => action(row));
        }

        /// <summary>工事を選んで、図面の解析結果から予算を自動作成する。</summary>
        private async Task CreateFromProjectAsync()
        {
            var option = new UIEditOption();
            option.AutoLabelWidth = true;
            option.Text = "予算の作成";
            var cuslst = await jwProjectMainService.GetAllAsync();

            option.AddCombobox("ProjectMainId", "工事", cuslst, "ProjectName", "Id", 0);

            UIEditForm frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += Frm_CheckedData;//校验
            frm.ShowDialog();
            if (!frm.IsOK) return;

            var id = Convert.ToInt64(frm["ProjectMainId"]);
            var maindata = await jwProjectMainService.GetByIdAsync(id);
            var budgetlst = await jwqitaService.GetAllJwBudgetMainDataAsync(t => t.JwProjectMainDataId == id);
            if (maindata != null && budgetlst.Count == 0)
            {
                await jwProjectMainService.LoadSubDataAsync(maindata);
                if (maindata.JwProjectSubDatas.Count > 0)
                {
                    try
                    {
                        await CreateBudge(maindata);
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError("予算の作成に失敗しました: " + ex.Message);
                    }
                }
                else
                {
                    UIMessageBox.ShowError("図面がまだ解析されていません");
                }
            }
            else if (budgetlst.Count != 0)
            {
                if (UIMessageBox.ShowAsk("この工事の予算はすでに存在します。再生成しますか？"))
                {
                    UIMessageBox.ShowError("この機能は開発中です");
                }
            }
        }

        private bool Frm_CheckedData(object sender, UIEditForm.EditFormEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Form["ProjectMainId"].ToString()))
            {
                e.Form.SetEditorFocus("ProjectMainId");
                ShowWarningTip("会社名を空にすることはできません");
                return false;
            }
            //if (Convert.ToDouble(e.Form["UnitPrice"]) == 0)
            //{
            //    e.Form.SetEditorFocus("単価");
            //    ShowWarningTip("単価をゼロにすることはできません");
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 根据识别 自动生成预算项
        /// </summary>
        /// <param name="data"></param>
        private async Task CreateBudge(JwProjectMainData data)
        {
            //var materdatas = dbContext.JwMaterialDatas.Include(t => t.JwMaterialTypeData).ToList();
            //this.dbContext.Entry(data).Collection(e => e.JwProjectSubDatas).Load();
            var wls = await jwqitaService.GetMaterialDataAsync();
            var subs = await jwProjectMainService.GetSubDatasAsync(t=>t.JwProjectMainDataId == data.Id);
            var subids = subs.Select(t => t.Id).ToList();
            var beams = await jwProjectMainService.GetBeamDatasAsync(t => subids.Contains(t.JwProjectSubDataId));
            var pillars = await jwProjectMainService.GetPillarDatasAsync(t => subids.Contains(t.JwProjectSubDataId));
            var links = await jwProjectMainService.GetLinkPartDatasAsync(t => subids.Contains(t.JwProjectSubDataId));
            JwBudgetMainData budget = new JwBudgetMainData();
            budget.JwProjectMainDataId = data.Id;
            budget.ProjectName = data.ProjectName;
            await jwqitaService.AddJwBudgetMainDataAsync(budget);

            var ltlst = links.GroupBy(t => t.GouJianType).ToList();

            foreach (var link in ltlst)
            {
                var z = link.Key.ToString();
                var pwl = wls.Where(t => t.GeneralTitle.Equals(link.Key.ToString())).FirstOrDefault();
                if (pwl == null)
                {
                    UIMessageBox.ShowError(link.Key.ToString() + "のマテリアル情報は存在しません");
                    return;
                }
                JwBudgetSubData plbudget = new JwBudgetSubData();
                plbudget.BudgetItemName = pwl.MaterialName;
                plbudget.Number = link.Count();
                plbudget.UnitPrice = pwl.UnitPrice;
                plbudget.UnitName = pwl.UnitName;
                plbudget.Amount = plbudget.Number * plbudget.UnitPrice;
                plbudget.JwBudgetMainDataId = budget.Id;
                plbudget.BudgetType = BudgetFrom.自動識別;
                plbudget.JwMaterialDataId = pwl.Id;
                plbudget.ModelParm = pwl.MaterialParameter;
                plbudget.MaterialType = pwl.MaterialType;
                budget.Amount += plbudget.Amount;
                await jwqitaService.AddJwBudgetSubDataAsync(plbudget);
            }

            var beamgpnames = beams.GroupBy(t => t.BeamXHId).ToList();
            foreach (var beam in beamgpnames)
            {
                var beamlength = beam.Sum(t => t.Length);
                var wl = wls.Where(t => t.Id.Equals(beam.Key)).FirstOrDefault();
                if (wl == null)
                {
                    UIMessageBox.ShowError(beam.Key + "のマテリアル情報は存在しません");
                    return;
                }
                JwBudgetSubData beambudget = new JwBudgetSubData();
                beambudget.BudgetItemName = wl.MaterialName;
                beambudget.Number = Convert.ToDecimal(beamlength / 1000);
                //beambudget.UnitPrice
                beambudget.BudgetType = BudgetFrom.自動識別;
                beambudget.UnitPrice = wl.UnitPrice;
                beambudget.UnitName = wl.UnitName;
                beambudget.Amount = beambudget.Number * beambudget.UnitPrice;
                beambudget.JwBudgetMainDataId = budget.Id;
                beambudget.JwMaterialDataId = wl.Id;
                beambudget.ModelParm = wl.MaterialParameter;
                beambudget.MaterialType = wl.MaterialType;
                budget.Amount += beambudget.Amount;
                await jwqitaService.AddJwBudgetSubDataAsync(beambudget);
                //beambudget.UnitName=
            }

            var pillarnames = pillars.GroupBy(t => t.TaggTitle).ToList();
            foreach (var pillar in pillarnames)
            {
                if (string.IsNullOrEmpty(pillar.Key))
                {

                }
                else
                {
                    var pwl = wls.Where(t => t.GeneralTitle.Equals(pillar.Key)).FirstOrDefault();
                    if (pwl == null)
                    {
                        UIMessageBox.ShowError(pillar.Key + "のマテリアル情報は存在しません");
                        return;
                    }


                    JwBudgetSubData plbudget = new JwBudgetSubData();
                    plbudget.BudgetItemName = pwl.MaterialName;
                    plbudget.Number = pillar.Count();
                    plbudget.BudgetType = BudgetFrom.自動識別;
                    plbudget.UnitPrice = pwl.UnitPrice;
                    plbudget.UnitName = pwl.UnitName;
                    plbudget.Amount = plbudget.Number * plbudget.UnitPrice;
                    plbudget.JwBudgetMainDataId = budget.Id;
                    plbudget.JwMaterialDataId = pwl.Id;
                    plbudget.ModelParm = pwl.MaterialParameter;
                    plbudget.MaterialType = pwl.MaterialType;
                    //plbudget
                    budget.Amount += plbudget.Amount;
                    await jwqitaService.AddJwBudgetSubDataAsync(plbudget);
                }

            }
            await jwqitaService.UpdateJwBudgetMainDataAsync(budget);

        }

        /// <summary>選択中の予算を見積書（Excel）に出力する。明細が未読込なら先に読む。</summary>
        private async Task ExportExcelAsync(BudgetRow row)
        {
            if (!row.DetailLoaded)
            {
                await jwqitaService.LoadBudgetSubCollectionAsync(row.Data);
                row.DetailLoaded = true;
            }
            await ExcelExporter(row.Data);
        }

        private async Task ExcelExporter(JwBudgetMainData mainData)
        {
            var materdatas =await jwqitaService.GetMaterialDataAsync();
            FileStream file = new FileStream(@"template.xlsx", FileMode.Open, FileAccess.Read);
            XSSFWorkbook hssfworkbook = new XSSFWorkbook(file);
            ISheet XSSFSheet = hssfworkbook.GetSheetAt(0);
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "RGB COMPANY";
            //hssfworkbook.se.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Quotation";
            string filename = string.Format("{0}.xlsx", mainData.ProjectName);
            //FileDto files = new FileDto(filename, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //var workbook = new XSSFWorkbook();
            int i = 1;
            var subss = mainData.JwBudgetSubDatas.OrderBy(t => t.MaterialType).ToList();
            foreach (var z in subss)
            {
                IRow c = XSSFSheet.CopyRow(10, 10 + i);
                if (z.MaterialType == MaterialType.柱)
                {
                    var tdata = materdatas.Find(t => t.Id == z.JwMaterialDataId);
                    if (tdata != null)
                    {
                        c.GetCell(0).SetCellValue(tdata.JwMaterialTypeData.MaterialTypeName);
                    }

                    c.GetCell(1).SetCellValue(z.BudgetItemName);
                }
                else
                {
                    if (!string.IsNullOrEmpty(z.BudgetItemName))
                    {
                        c.GetCell(0).SetCellValue(z.BudgetItemName);
                        XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10 + i, 10 + i, 0, 3));
                    }
                }

                if (!string.IsNullOrEmpty(z.ModelParm))
                {
                    c.GetCell(4).SetCellValue(z.ModelParm);
                }
                if (!string.IsNullOrEmpty(z.Number.ToString()))
                {
                    c.GetCell(6).SetCellValue(z.Number.ToString());
                }
                if (!string.IsNullOrEmpty(z.UnitName))
                {
                    c.GetCell(8).SetCellValue(z.UnitName);
                }
                if (!string.IsNullOrEmpty(z.UnitPrice.ToString()))
                {
                    c.GetCell(9).SetCellValue(z.UnitPrice.ToString());
                }
                c.GetCell(10).SetCellValue(z.Amount.ToString());
                i++;
            }
            ICell cc = XSSFSheet.GetRow(0).GetCell(0);
            string yu = cc.StringCellValue;
            cc.SetCellValue(yu + mainData.ProjectName);
            //15
            ICell cdate = XSSFSheet.GetRow(1).GetCell(14);
            cdate.SetCellType(CellType.String);
            cdate.SetCellValue(mainData.CreationTime.ToShortDateString());

            IRow xiaoji = XSSFSheet.CopyRow(10, 10 + i);

            xiaoji.GetCell(0).SetCellValue("小  計");
            XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10 + i, 10 + i, 0, 3));
            double hj = Convert.ToDouble(mainData.JwBudgetSubDatas.Sum(t => t.Amount));
            xiaoji.GetCell(10).SetCellValue(hj);
            IRow heji = XSSFSheet.CopyRow(10, 11 + i);
            heji.GetCell(0).SetCellValue("合  計");
            XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(11 + i, 11 + i, 0, 3));

            heji.GetCell(10).SetCellValue(hj);

            IRow xiaofeishui = XSSFSheet.CopyRow(10, 12 + i);
            xiaofeishui.GetCell(0).SetCellValue("消費税");
            XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + i, 12 + i, 0, 3));
            xiaofeishui.GetCell(6).SetCellValue("10");
            xiaofeishui.GetCell(8).SetCellValue("%");

            double zhj = hj * 0.1;
            xiaofeishui.GetCell(10).SetCellValue(zhj);

            IRow zheji = XSSFSheet.CopyRow(10, 13 + i);
            zheji.GetCell(0).SetCellValue("総  合  計");
            XSSFSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(13 + i, 13 + i, 0, 3));
            zheji.GetCell(10).SetCellValue(hj + zhj);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel ファイル(*.xls)|*.xls|Excel ファイル(*.xlsx)|*.xlsx";
            saveFileDialog.FileName = string.Format("{0}見積書.xlsx", mainData.ProjectName);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    hssfworkbook.Write(stream);
                    //_tempFileCacheManager.SetFile(files.FileToken, stream.ToArray());
                }
                UIMessageBox.ShowSuccess("見積書を出力しました");
            }
        }

        /// <summary>選択中の予算に明細を手動で追加する（追加後に一覧と明細を読み直す）。</summary>
        private Task AddBudgetAsync(BudgetRow row)
        {
            AddBudget addBudget = new AddBudget(row.Data);
            if (addBudget.ShowDialog() == DialogResult.OK)
            {
                UIMessageTip.ShowOk("予算が正常に追加されました");
            }
            return Task.CompletedTask;
        }

        // ───────── 右クリックメニュー（Designer の contextMenuStrip1） ─────────

        private void menuExcel_Click(object sender, EventArgs e) => _ = RunSelectedAsync(ExportExcelAsync, reload: false);

        private void menuAddBudget_Click(object sender, EventArgs e) => _ = RunSelectedAsync(AddBudgetAsync);
    }
}
