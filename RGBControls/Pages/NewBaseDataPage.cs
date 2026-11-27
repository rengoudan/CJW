using JwCore;
using JwLogging;
using JwServices;
using JwShapeCommon;
using RGBControls.Classes;
using RGBJWMain.Pages;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WLabel = System.Windows.Forms.Label;

namespace RGBControls.Pages
{
    /// <summary>
    /// 2026年11月27日 データ管理（基礎データ）を再構成（ワークベンチ・顧客管理と同じ見た目・操作の流れ）。
    /// ・上段：見出し・［＋ 種類を追加］［＋ データを追加］［更新］
    /// ・数字カード（データの種類・選択中の種類のデータ・単価未入力）
    /// ・左：データの種類の一覧　右：選んだ種類のデータ（材料名称・仕様・識別子・単価・単位・摘要）
    /// 操作：左の行をクリックで種類を選ぶ。「データを追加」は種類を選んでいるときだけ使える。
    /// 行をダブルクリックすると、その種類（左）・データ（右）を編集できる。
    /// 追加ダイアログは顧客管理の追加と同じ（必須項目・重複の確認を行い、保存後に一覧を読み直す）。
    /// </summary>
    public partial class NewBaseDataPage : BasePage
    {
        private const string FontName = WorkbenchTheme.FontName;
        private static readonly Color Ink = WorkbenchTheme.Ink;
        private static readonly Color Muted = WorkbenchTheme.Muted;
        private static readonly Color PageBack = WorkbenchTheme.PageBack;
        private static readonly Color Orange = WorkbenchTheme.Orange;

        private float _k = 1f;
        private bool _loading, _reloadAgain;
        private List<JwMaterialTypeData> _types = new();
        private JwMaterialTypeData? _selected;

        private TableLayoutPanel _root = null!;
        private WLabel _subtitle = null!, _typeTitle = null!, _materialTitle = null!;
        private readonly WLabel[] _kpiValues = new WLabel[3];
        private readonly WLabel[] _kpiSubs = new WLabel[3];
        private readonly Panel[] _kpiCards = new Panel[3];
        private AntdUI.Table _typeTable = null!, _materialTable = null!;
        private AntdUI.Button _btnAddMaterial = null!;

        public NewBaseDataPage()
        {
            InitializeComponent();
            _k = DeviceDpi / 96f;
            // 旧い表示（BasePage の見出し・保存ボタン・表）は使わない
            uiLine1.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;
            BuildPage();
            VisibleChanged += async (s, e) => { if (Visible) await ReloadAsync(); };
        }

        private JwqitaService jwqitaService => ServiceFactory.GetInstance().CreateJwqitaService();

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
            _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));      // 種類の一覧／選んだ種類のデータ
            _root.Controls.Add(BuildHeader(), 0, 0);
            _root.Controls.Add(BuildKpis(), 0, 1);
            _root.Controls.Add(BuildBody(), 0, 2);
            Controls.Add(_root);
            _root.BringToFront();
        }

        private Control BuildHeader()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };
            var title = new WLabel { Text = "データ管理", AutoSize = true, ForeColor = Ink, Font = new Font(FontName, 17f, FontStyle.Bold), Location = new Point(0, S(6)) };
            _subtitle = new WLabel { AutoSize = true, ForeColor = Muted, Font = new Font(FontName, 10f) };
            panel.Controls.Add(title);
            panel.Controls.Add(_subtitle);

            var actions = new FlowLayoutPanel { AutoSize = true, WrapContents = false, FlowDirection = FlowDirection.LeftToRight, BackColor = Color.Transparent };
            actions.Controls.Add(WorkbenchTheme.ActionButton("＋ 種類を追加", AntdUI.TTypeMini.Primary, () => { _ = RunAsync(AddTypeAsync); }, _k));
            _btnAddMaterial = WorkbenchTheme.ActionButton("＋ データを追加", AntdUI.TTypeMini.Success, () => { _ = RunAsync(AddMaterialAsync); }, _k);
            actions.Controls.Add(_btnAddMaterial);
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
            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, Margin = new Padding(0, S(4), 0, S(8)) };
            for (int i = 0; i < 3; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 3));
            string[] titles = { "データの種類", "選択中の種類のデータ", "単価未入力（選択中）" };
            for (int i = 0; i < titles.Length; i++)
            {
                var card = WorkbenchTheme.Card(new Padding(i == 0 ? 0 : S(6), 0, i == 2 ? 0 : S(6), 0));
                var t = new WLabel { Text = titles[i], AutoSize = true, ForeColor = Muted, Location = new Point(S(14), S(10)), Font = new Font(FontName, 10f) };
                var v = new WLabel { Text = "-", AutoSize = true, ForeColor = Ink, Location = new Point(S(12), S(30)), Font = new Font(FontName, 22f, FontStyle.Bold) };
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
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            grid.Controls.Add(BuildTypeList(), 0, 0);
            grid.Controls.Add(BuildMaterialList(), 1, 0);
            return grid;
        }

        private Control BuildTypeList()
        {
            var card = WorkbenchTheme.Card(new Padding(0, 0, S(6), 0));
            _typeTitle = WorkbenchTheme.SectionTitle("データの種類", _k);

            _typeTable = new AntdUI.Table
            {
                Dock = DockStyle.Fill, Bordered = false, EmptyHeader = true,
                EmptyText = "データの種類が登録されていません。\r\n「＋ 種類を追加」から登録してください。",
                AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill, Font = new Font(FontName, 10f),
            };
            _typeTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("MaterialTypeName", "データの種類"),
                new AntdUI.Column("MaterialType", "カテゴリー") { Width = "110", Align = AntdUI.ColumnAlign.Center },
                new AntdUI.Column("MaterialCount", "量") { Width = "70", Align = AntdUI.ColumnAlign.Center },
            };
            _typeTable.CellClick += async (s, e) =>
            {
                if (e.Record is JwMaterialTypeData t) await SelectTypeAsync(t);
            };
            _typeTable.CellDoubleClick += (s, e) =>
            {
                if (e.Record is JwMaterialTypeData t) _ = RunAsync(() => EditTypeAsync(t));
            };

            // 表は Fill を先に、上端の見出しを後から（ワークベンチ・顧客管理と同じ積み方）
            card.Controls.Add(_typeTable);
            card.Controls.Add(_typeTitle);
            return card;
        }

        private Control BuildMaterialList()
        {
            var card = WorkbenchTheme.Card(new Padding(S(6), 0, 0, 0));
            _materialTitle = WorkbenchTheme.SectionTitle("データ", _k);

            _materialTable = new AntdUI.Table
            {
                Dock = DockStyle.Fill, Bordered = false, EmptyHeader = true,
                EmptyText = "左の一覧からデータの種類を選んでください",
                AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill, Font = new Font(FontName, 10f),
            };
            _materialTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("MaterialName", "材料名称"),
                new AntdUI.Column("MaterialParameter", "仕　様"),
                new AntdUI.Column("GeneralTitle", "識別子") { Width = "100" },
                new AntdUI.Column("UnitPrice", "単価") { Width = "90" },
                new AntdUI.Column("UnitName", "単位") { Width = "70", Align = AntdUI.ColumnAlign.Center },
                new AntdUI.Column("Remark", "摘要"),
            };
            _materialTable.CellDoubleClick += (s, e) =>
            {
                if (e.Record is JwMaterialData m) _ = RunAsync(() => EditMaterialAsync(m));
            };

            card.Controls.Add(_materialTable);
            card.Controls.Add(_materialTitle);
            return card;
        }

        // ───────── 表示 ─────────

        /// <summary>種類の一覧と数字カードを読み直す。選択中の種類は同じ顧客（種類）を指し直す。</summary>
        private async Task ReloadAsync()
        {
            if (_loading) { _reloadAgain = true; return; }
            _loading = true;
            try
            {
                do
                {
                    _reloadAgain = false;
                    _types = await jwqitaService.GetJwMaterialTypeDatasAsync();
                    if (_selected != null)
                    {
                        var id = _selected.Id;
                        _selected = _types.FirstOrDefault(t => t.Id == id);
                        if (_selected != null) await jwqitaService.LoadSubDataAsync(_selected);
                    }
                    RenderView(rebindTypes: true);
                } while (_reloadAgain);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "データ管理の読み込みに失敗しました");
            }
            finally { _loading = false; }
        }

        /// <summary>画面の表示を現在の状態にそろえる（読み取りのみ）。</summary>
        private void RenderView(bool rebindTypes)
        {
            int noPrice = _selected == null ? 0 : _selected.JwMaterialDatas.Count(m => m.UnitPrice == 0);
            int count = _selected == null ? 0 : _selected.JwMaterialDatas.Count();
            SetKpi(0, _types.Count, "種類", null);
            SetKpi(1, count, "件", null);
            SetKpi(2, noPrice, "件", noPrice > 0 ? Orange : null);

            _subtitle.Text = "左でデータの種類を選ぶと、右にそのデータが表示されます。";
            _typeTitle.Text = $"データの種類（{_types.Count} 件）";
            if (rebindTypes)
            {
                _typeTable.DataSource = _types;
            }
            if (_selected != null && _types.Contains(_selected))
            {
                _typeTable.SelectedIndex = _types.IndexOf(_selected) + 1;
            }

            _materialTitle.Text = _selected == null
                ? "データ"
                : $"データ：{_selected.MaterialTypeName}（{count} 件）";
            _materialTable.DataSource = _selected == null
                ? new List<JwMaterialData>()
                : _selected.JwMaterialDatas;

            _btnAddMaterial.Enabled = _selected != null;
        }

        private void SetKpi(int i, int value, string unit, Color? color)
        {
            _kpiValues[i].Text = value.ToString();
            _kpiValues[i].ForeColor = color ?? Ink;
            _kpiSubs[i].Text = unit;
            _kpiCards[i].PerformLayout();
        }

        // ───────── 操作 ─────────

        /// <summary>操作の例外をメッセージにし、最後に一覧を読み直す。</summary>
        private async Task RunAsync(Func<Task> action)
        {
            try { await action(); }
            catch (Exception ex)
            {
                AppLog.Error(ex, "データ管理の操作に失敗しました");
                UIMessageBox.ShowError(ex.Message);
            }
            await ReloadAsync();
        }

        /// <summary>行を選んだとき、その種類のデータを読み込んで右側に出す。</summary>
        private async Task SelectTypeAsync(JwMaterialTypeData t)
        {
            try
            {
                _selected = t;
                await jwqitaService.LoadSubDataAsync(t);
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "データの読み込みに失敗しました");
                UIMessageBox.ShowError(ex.Message);
            }
            RenderView(rebindTypes: false);
        }

        private async Task AddTypeAsync()
        {
            var matype = JwExtend.CreateBindList<MaterialType>();
            var option = new UIEditOption { AutoLabelWidth = true, Text = "追加" };
            option.AddText("MaterialTypeName", "タイプ名", null, true);
            option.AddCombobox("MaterialType", "カテゴリー", matype, "Name", "Value", 0);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += (s, e) => CheckTypeForm(e, null);
            frm.ShowDialog();
            if (!frm.IsOK) return;

            var data = new JwMaterialTypeData
            {
                MaterialTypeName = Text2(frm, "MaterialTypeName"),
            };
            data.MaterialType = (MaterialType)(frm["MaterialType"]);
            await jwqitaService.AddJwMaterialTypeDataAsync(data);
        }

        private async Task AddMaterialAsync()
        {
            var parent = _selected;
            if (parent == null)
            {
                ShowWarningTip("データの種類を選択してください");
                return;
            }

            var option = new UIEditOption { AutoLabelWidth = true, Text = "追加" };
            option.AddText("MaterialName", "材料名称", null, true);
            option.AddText("MaterialParameter", "仕     様", null, true);
            option.AddText("GeneralTitle", "共通識別子", null, true);
            option.AddText("UnitName", "単位", null, true);
            option.AddDouble("UnitPrice", "単価", 0);
            option.AddDouble("NumParameter", "数値パラメータ", 0);
            option.AddText("Remark", "摘  要", null, true);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.CheckedData += (s, e) => CheckMaterialForm(e, null);
            frm.ShowDialog();
            if (!frm.IsOK) return;

            var data = new JwMaterialData
            {
                MaterialName = Text2(frm, "MaterialName"),
                MaterialParameter = Text2(frm, "MaterialParameter"),
                GeneralTitle = Text2(frm, "GeneralTitle"),
                UnitName = Text2(frm, "UnitName"),
                UnitPrice = Convert.ToDecimal(frm["UnitPrice"]),
                NumParameter = Convert.ToDouble(frm["NumParameter"]),
                Remark = Text2(frm, "Remark"),
                MaterialType = parent.MaterialType,
                JwMaterialTypeDataId = parent.Id,
            };
            await jwqitaService.AddJwMaterialDataAsync(data);
        }

        /// <summary>ダブルクリックしたデータの内容を編集する。種類（親）は変えない。</summary>
        private async Task EditMaterialAsync(JwMaterialData m)
        {
            var option = new UIEditOption { AutoLabelWidth = true, Text = "編集" };
            option.AddText("MaterialName", "材料名称", m.MaterialName, true);
            option.AddText("MaterialParameter", "仕     様", m.MaterialParameter, true);
            option.AddText("GeneralTitle", "共通識別子", m.GeneralTitle, true);
            option.AddText("UnitName", "単位", m.UnitName, true);
            option.AddDouble("UnitPrice", "単価", (double)m.UnitPrice);
            option.AddDouble("NumParameter", "数値パラメータ", m.NumParameter);
            option.AddText("Remark", "摘  要", m.Remark, true);

            var frm = new UIEditForm(option);
            frm.Render();
            string id = m.Id;
            frm.CheckedData += (s, e) => CheckMaterialForm(e, id);
            frm.ShowDialog();
            if (!frm.IsOK) return;

            m.MaterialName = Text2(frm, "MaterialName");
            m.MaterialParameter = Text2(frm, "MaterialParameter");
            m.GeneralTitle = Text2(frm, "GeneralTitle");
            m.UnitName = Text2(frm, "UnitName");
            m.UnitPrice = Convert.ToDecimal(frm["UnitPrice"]);
            m.NumParameter = Convert.ToDouble(frm["NumParameter"]);
            m.Remark = Text2(frm, "Remark");
            await jwqitaService.UpdateJwMaterialDataAsync(m);
        }

        /// <summary>ダブルクリックした種類の名前・カテゴリーを編集する。</summary>
        private async Task EditTypeAsync(JwMaterialTypeData t)
        {
            var matype = JwExtend.CreateBindList<MaterialType>();
            var option = new UIEditOption { AutoLabelWidth = true, Text = "編集" };
            option.AddText("MaterialTypeName", "タイプ名", t.MaterialTypeName, true);
            option.AddCombobox("MaterialType", "カテゴリー", matype, "Name", "Value", (int)t.MaterialType);

            var frm = new UIEditForm(option);
            frm.Render();
            string id = t.Id;
            frm.CheckedData += (s, e) => CheckTypeForm(e, id);
            frm.ShowDialog();
            if (!frm.IsOK) return;

            await jwqitaService.UpdateJwMaterialTypeDataAsync(
                id, Text2(frm, "MaterialTypeName"), (MaterialType)(frm["MaterialType"]));
        }

        /// <summary>種類の追加・編集ダイアログの入力チェック。同じ名前の種類は登録させない（excludeId は編集中の種類）。</summary>
        private bool CheckTypeForm(UIEditForm.EditFormEventArgs e, string? excludeId)
        {
            string name = Text2(e.Form, "MaterialTypeName");
            if (name.Length == 0)
            {
                e.Form.SetEditorFocus("MaterialTypeName");
                ShowWarningTip("タイプ名を空にすることはできません");
                return false;
            }

            var dup = _types.FirstOrDefault(t => t.Id != excludeId &&
                string.Equals((t.MaterialTypeName ?? "").Trim(), name, StringComparison.OrdinalIgnoreCase));
            if (dup != null)
            {
                e.Form.SetEditorFocus("MaterialTypeName");
                ShowWarningTip($"同じ名前のデータの種類が既にあります：{dup.MaterialTypeName}");
                return false;
            }
            return true;
        }

        /// <summary>データの追加・編集ダイアログの入力チェック。同じ材料名称・仕様は登録させない（excludeId は編集中のデータ）。</summary>
        private bool CheckMaterialForm(UIEditForm.EditFormEventArgs e, string? excludeId)
        {
            string name = Text2(e.Form, "MaterialName");
            string param = Text2(e.Form, "MaterialParameter");
            if (name.Length == 0)
            {
                e.Form.SetEditorFocus("MaterialName");
                ShowWarningTip("材料名称を空にすることはできません");
                return false;
            }
            if (Convert.ToDouble(e.Form["UnitPrice"]) == 0)
            {
                e.Form.SetEditorFocus("UnitPrice");
                ShowWarningTip("単価をゼロにすることはできません");
                return false;
            }

            if (jwqitaService.GetMaterialData(t => t.MaterialName == name && t.MaterialParameter == param && t.Id != excludeId).Any())
            {
                e.Form.SetEditorFocus("MaterialName");
                ShowWarningTip($"同じ材料名称・仕様が既に存在します：{name}");
                return false;
            }
            return true;
        }

        private static string Text2(UIEditForm form, string key) => (Convert.ToString(form[key]) ?? "").Trim();
    }
}
