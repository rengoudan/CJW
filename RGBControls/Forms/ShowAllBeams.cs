using JwCore;
using JwServices;
using JwShapeCommon;
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

namespace RGBControls.Forms
{
    /// <summary>
    /// 梁統計（工事全体の梁を梁番号ごとに集計）。
    /// 2026年11月23日 作り直し（表示・確認のみ。データは書き換えない。梁画面で編集した内容は閉じたあと再集計）：
    ///   ・上：工事全体の集計（梁種類・総本数・総長・CSV 対象・要確認）
    ///   ・左：梁番号ごとの一覧（階ごとの本数の列、1 本の長さ・芯長・総長、端部、穴数、状態）。検索・階・要確認で絞り込み、Excel 出力
    ///   ・右上：選んだ梁の 3D（穴間隔の寸法付き。2D 図は廃止）
    ///   ・右下：その梁番号に含まれる梁（階ごと）。クリックで 3D を切り替え、ダブルクリックで梁画面
    ///   状態：形状差（同じ梁番号で穴の配置＝BeamSignature が異なる梁がある）／穴なし／CSV対象外
    /// 原来的问题：「梁長さ」实际是 1 根的长度、没有总长；预览只显示组内第一根的 2D；同一梁番号下孔位不同也看不出来。
    /// </summary>
    public partial class ShowAllBeams : AntdUI.Window
    {
        private JwProjectMainService JwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();

        private readonly JwProjectMainData _selectedMainData = null!;
        private List<BeamTypeRow> _rows = new();
        private List<string> _floors = new();
        private Dictionary<string, JwProjectSubData> _subs = new();
        private Dictionary<string, int> _holeCounts = new();

        // 画面（コードで構築）
        private System.Windows.Forms.Label _summary = null!;
        private System.Windows.Forms.Label _footer = null!;
        private TextBox _search = null!;
        private ComboBox _floorFilter = null!;
        private CheckBox _needCheckOnly = null!;
        private System.Windows.Forms.Label _beamTitle = null!;
        private JwBeam3D.Beam3DControl _beam3D = null!;
        private bool _building;

        public ShowAllBeams()
        {
            InitializeComponent();
        }

        public ShowAllBeams(JwProjectMainData mainData)
        {
            _selectedMainData = mainData;
            InitializeComponent();
            BuildLayout();
            // 2026年11月21日 原来在构造函数里"触发不等待"地加载数据，表格可能在数据到达前就已绑定空列表。
            // 改为在 ShowAllBeams_Load 中 await 加载完成后再绑定。
        }

        // ───────────────────────── 集計 ─────────────────────────

        /// <summary>梁番号ごとの 1 行。</summary>
        public sealed class BeamTypeRow
        {
            public string BeamCode { get; set; } = "";
            public string InitialBeamCode { get; set; } = "";
            public int BeamCount { get; set; }
            public double Length { get; set; }
            public double XXLength { get; set; }
            /// <summary>総長（m）＝ 1 本の長さ × 本数</summary>
            public double TotalMeters { get; set; }
            public string Ends { get; set; } = "";
            public string Holes { get; set; } = "";
            public bool ShapeMismatch { get; set; }
            public int NoHole { get; set; }
            public int NoCsv { get; set; }
            public bool NeedCheck => ShapeMismatch || NoHole > 0 || NoCsv > 0;
            public string Status { get; set; } = "";
            public Dictionary<string, int> FloorCounts { get; } = new();
            public List<JwBeamData> Beams { get; set; } = new();
            public string MajoritySignature { get; set; } = "";

            // 階ごとの本数の列（AntdUI の列はプロパティ名で結び付けるため、階の順に F0, F1, … を使う）
            public int[] FloorValues { get; set; } = Array.Empty<int>();
            private int Fv(int i) => i < FloorValues.Length ? FloorValues[i] : 0;
            public int F0 => Fv(0); public int F1 => Fv(1); public int F2 => Fv(2); public int F3 => Fv(3); public int F4 => Fv(4);
            public int F5 => Fv(5); public int F6 => Fv(6); public int F7 => Fv(7); public int F8 => Fv(8); public int F9 => Fv(9);
            public int F10 => Fv(10); public int F11 => Fv(11); public int F12 => Fv(12); public int F13 => Fv(13); public int F14 => Fv(14);
            public int F15 => Fv(15); public int F16 => Fv(16); public int F17 => Fv(17); public int F18 => Fv(18); public int F19 => Fv(19);
        }
        private const int MaxFloorColumns = 20;

        /// <summary>右下の一覧の 1 行（梁 1 本）。</summary>
        public sealed class MemberRow
        {
            public JwBeamData Beam { get; set; } = null!;
            public string FloorName => Beam.FloorName;
            public string BeamCode => Beam.BeamCode;
            public string InitialBeamCode => Beam.InitialBeamCode;
            public string GongQu => Beam.GongQu;
            public string Csv => Beam.HasCsv ? "○" : "－";
            public int Holes { get; set; }
            public string Check { get; set; } = "";
        }

        private static string EndText(JwBeamData b) =>
            $"{(b.HasStartSide ? b.StartTelosType.ToString() : "B")}–{(b.HasEndSide ? b.EndTelosType.ToString() : "B")}";

        private async Task createData()
        {
            _rows = new List<BeamTypeRow>();
            if (_selectedMainData == null) return;

            var subs = await JwProjectMainService.GetSubDatasAsync(s => s.JwProjectMainDataId == _selectedMainData.Id);
            _subs = subs.ToDictionary(s => s.Id);
            _floors = subs.Select(s => s.FloorName ?? "").Distinct().OrderBy(f => f, StringComparer.Ordinal).ToList();

            var beams = await JwProjectMainService.GetBeamDatasByMainAsync(_selectedMainData.Id);
            _holeCounts = JwProjectMainService.GetSavedHoleCounts(beams.Select(b => b.Id));
            int Holes(JwBeamData b) => _holeCounts.TryGetValue(b.Id, out var n) ? n : 0;

            foreach (var group in beams.GroupBy(t => t.BeamCode))
            {
                var list = group.OrderBy(b => b.FloorName, StringComparer.Ordinal).ToList();
                var first = list[0];
                var signatures = list.Select(b => b.BeamSignature ?? "").Where(s => s.Length > 0).ToList();
                string majority = signatures.GroupBy(s => s).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault() ?? "";
                var holeCounts = list.Select(Holes).Distinct().OrderBy(n => n).ToList();
                var row = new BeamTypeRow
                {
                    BeamCode = group.Key,
                    InitialBeamCode = first.InitialBeamCode,
                    BeamCount = list.Count,
                    Length = first.Length,
                    XXLength = first.XXLength,
                    TotalMeters = list.Sum(b => b.Length) / 1000.0,
                    Ends = string.Join("／", list.Select(EndText).Distinct()),
                    Holes = holeCounts.Count == 1 ? holeCounts[0].ToString() : $"{holeCounts.First()}～{holeCounts.Last()}",
                    ShapeMismatch = signatures.Distinct().Count() > 1,
                    NoHole = list.Count(b => Holes(b) == 0),
                    NoCsv = list.Count(b => !b.HasCsv),
                    Beams = list,
                    MajoritySignature = majority,
                };
                foreach (var b in list)
                    row.FloorCounts[b.FloorName ?? ""] = row.FloorCounts.GetValueOrDefault(b.FloorName ?? "") + 1;
                row.FloorValues = _floors.Select(f => row.FloorCounts.GetValueOrDefault(f)).ToArray();
                var parts = new List<string>();
                if (row.ShapeMismatch) parts.Add("形状差");
                if (row.NoHole > 0) parts.Add($"穴なし {row.NoHole}");
                if (row.NoCsv > 0) parts.Add($"CSV対象外 {row.NoCsv}");
                row.Status = parts.Count == 0 ? "OK" : string.Join("・", parts);
                _rows.Add(row);
            }
            _rows = _rows.OrderBy(r => r.BeamCode, StringComparer.Ordinal).ToList();
        }

        // ───────────────────────── 画面 ─────────────────────────

        private void BuildLayout()
        {
            _building = true;
            pageHeader1.Text = $"梁統計　{_selectedMainData?.ProjectName}";
            Text = pageHeader1.Text; // 窓のタイトル（原来是类名「ShowAllBeams」）

            // 設計器の配置（上下 2 段＋2D 図）を外し、表は新しい配置で使う
            panel1.Controls.Clear();
            tableLayoutPanel1.Controls.Clear();
            panel2.Controls.Clear();
            table1.Dock = DockStyle.Fill;
            table2.Dock = DockStyle.Fill;

            _summary = new System.Windows.Forms.Label
            {
                Dock = DockStyle.Top, Height = 40, Padding = new Padding(12, 10, 0, 0),
                Font = new Font(Font.FontFamily, 12f, FontStyle.Bold), Text = "集計中…",
            };

            // 左：絞り込み・梁番号ごとの一覧・階別合計
            _search = new TextBox { Width = 160, Margin = new Padding(0, 6, 8, 0), PlaceholderText = "梁番号で検索" };
            _search.TextChanged += (s, e) => ApplyFilter();
            _floorFilter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 140, Margin = new Padding(0, 5, 8, 0) };
            _floorFilter.SelectedIndexChanged += (s, e) => ApplyFilter();
            _needCheckOnly = new CheckBox { Text = "要確認のみ", AutoSize = true, Margin = new Padding(0, 8, 16, 0) };
            _needCheckOnly.CheckedChanged += (s, e) => ApplyFilter();
            var excel = new AntdUI.Button { Text = "梁リスト Excel出力", AutoSizeMode = AntdUI.TAutoSize.Width, Height = 32, Ghost = true, Type = AntdUI.TTypeMini.Primary };
            excel.Click += (s, e) => ExportExcel();
            var filterBar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 42, Padding = new Padding(8, 4, 8, 0), WrapContents = false };
            filterBar.Controls.AddRange(new Control[]
            {
                new System.Windows.Forms.Label { Text = "絞り込み", AutoSize = true, Margin = new Padding(0, 10, 6, 0) },
                _search, new System.Windows.Forms.Label { Text = "階", AutoSize = true, Margin = new Padding(0, 10, 4, 0) }, _floorFilter, _needCheckOnly, excel,
            });
            _footer = new System.Windows.Forms.Label { Dock = DockStyle.Bottom, Height = 30, Padding = new Padding(12, 7, 0, 0), ForeColor = Color.DimGray };
            var left = new Panel { Dock = DockStyle.Fill };
            left.Controls.Add(table1);
            left.Controls.Add(filterBar);
            left.Controls.Add(_footer);
            filterBar.BringToFront(); _footer.BringToFront(); table1.BringToFront();

            // 右上：3D　右下：梁番号に含まれる梁
            _beamTitle = new System.Windows.Forms.Label
            {
                Dock = DockStyle.Top, Height = 32, Padding = new Padding(8, 8, 0, 0),
                Font = new Font(Font.FontFamily, 10.5f, FontStyle.Bold), Text = "梁を選ぶと 3D で表示します",
            };
            _beam3D = new JwBeam3D.Beam3DControl { Dock = DockStyle.Fill, PairPitch = JwFileConsts.EllipseSpacing };
            var viewPanel = new Panel { Dock = DockStyle.Fill };
            viewPanel.Controls.Add(_beam3D);
            viewPanel.Controls.Add(_beamTitle);

            var memberTitle = new System.Windows.Forms.Label
            {
                Dock = DockStyle.Top, Height = 28, Padding = new Padding(8, 6, 0, 0),
                Text = "この梁番号の梁（クリックで 3D 切り替え、ダブルクリックで梁画面）",
            };
            var memberPanel = new Panel { Dock = DockStyle.Fill };
            memberPanel.Controls.Add(table2);
            memberPanel.Controls.Add(memberTitle);
            table2.BringToFront();

            var rightSplit = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterWidth = 6 };
            rightSplit.Panel1.Controls.Add(viewPanel);
            rightSplit.Panel2.Controls.Add(memberPanel);

            var mainSplit = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Vertical, SplitterWidth = 6 };
            mainSplit.Panel1.Controls.Add(left);
            mainSplit.Panel2.Controls.Add(rightSplit);

            panel1.Controls.Add(mainSplit);
            panel1.Controls.Add(_summary);
            mainSplit.BringToFront();

            // 画面の大きさが決まってから分割位置を決める（左 55%、右は 3D を大きめに）
            Shown += (s, e) =>
            {
                try
                {
                    mainSplit.SplitterDistance = (int)(mainSplit.Width * 0.55);
                    rightSplit.SplitterDistance = (int)(rightSplit.Height * 0.62);
                }
                catch { /* 小さすぎる画面では既定のまま */ }
            };

            table2.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("FloorName", "階"),
                new AntdUI.Column("BeamCode", "梁番号"),
                new AntdUI.Column("InitialBeamCode", "初期梁番号"),
                new AntdUI.Column("GongQu", "工区"),
                new AntdUI.Column("Csv", "CSV"),
                new AntdUI.Column("Holes", "穴数"),
                new AntdUI.Column("Check", "確認")
                {
                    Render = (v, r, i) => r is MemberRow m && m.Check.Length > 0 ? new AntdUI.CellTag(m.Check, AntdUI.TTypeMini.Error) : null,
                },
            };
            table2.SelectIndexChanged += async (s, e) => await OnMemberSelectedAsync();
            table2.CellDoubleClick += async (s, e) =>
            {
                if (e.RowIndex > 0 && table2[e.RowIndex - 1].record is MemberRow m) await OpenBeamDetailAsync(m);
            };
            _building = false;
        }

        private void BuildTypeColumns()
        {
            var cols = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("BeamCode", "梁番号") { SortOrder = true },
                new AntdUI.Column("InitialBeamCode", "初期梁番号"),
                new AntdUI.Column("BeamCount", "本数") { SortOrder = true },
            };
            for (int i = 0; i < Math.Min(_floors.Count, MaxFloorColumns); i++)
            {
                int idx = i;
                cols.Add(new AntdUI.Column($"F{i}", _floors[i])
                {
                    Render = (v, r, row) => r is BeamTypeRow t && idx < t.FloorValues.Length && t.FloorValues[idx] > 0 ? t.FloorValues[idx].ToString() : "",
                });
            }
            cols.Add(new AntdUI.Column("Length", "長さ(mm)") { SortOrder = true, Render = (v, r, i) => r is BeamTypeRow t ? t.Length.ToString("0.#") : "" });
            cols.Add(new AntdUI.Column("XXLength", "芯長(mm)") { Render = (v, r, i) => r is BeamTypeRow t ? t.XXLength.ToString("0.#") : "" });
            cols.Add(new AntdUI.Column("TotalMeters", "総長(m)") { SortOrder = true, Render = (v, r, i) => r is BeamTypeRow t ? t.TotalMeters.ToString("0.00") : "" });
            cols.Add(new AntdUI.Column("Ends", "端部"));
            cols.Add(new AntdUI.Column("Holes", "穴数"));
            cols.Add(new AntdUI.Column("Status", "状態")
            {
                Render = (v, r, i) => r is BeamTypeRow t
                    ? new AntdUI.CellTag(t.Status, t.ShapeMismatch || t.NoHole > 0 ? AntdUI.TTypeMini.Error : t.NoCsv > 0 ? AntdUI.TTypeMini.Warn : AntdUI.TTypeMini.Success)
                    : null,
            });
            table1.Columns = cols;
        }

        private async void ShowAllBeams_Load(object sender, EventArgs e)
        {
            try
            {
                await ReloadAsync(null);
            }
            catch (Exception ex)
            {
                RGBControls.Errors.ErrorReporter.Report(ex, "梁統計を集計できませんでした。");
            }
        }

        /// <summary>集計し直して表示する（keepCode の梁番号を選び直す）。</summary>
        private async Task ReloadAsync(string? keepCode)
        {
            await createData();
            BuildTypeColumns();

            _building = true;
            _floorFilter.Items.Clear();
            _floorFilter.Items.Add("すべて");
            foreach (var f in _floors) _floorFilter.Items.Add(f);
            _floorFilter.SelectedIndex = 0;
            _building = false;

            int kinds = _rows.Count, total = _rows.Sum(r => r.BeamCount);
            double meters = _rows.Sum(r => r.TotalMeters);
            int csv = _rows.Sum(r => r.BeamCount - r.NoCsv);
            int need = _rows.Count(r => r.NeedCheck);
            _summary.Text = $"梁種類 {kinds}・総本数 {total} 本・総長 {meters:0.00} m・CSV対象 {csv} 本" + (need > 0 ? $"・要確認 {need} 種類" : "");
            _summary.ForeColor = need > 0 ? Color.Firebrick : SystemColors.ControlText;
            _footer.Text = "階別本数：" + string.Join("　", _floors.Select(f => $"{f} {_rows.Sum(r => r.FloorCounts.GetValueOrDefault(f))}本"))
                           + (_floors.Count > MaxFloorColumns ? $"（表の階の列は先頭 {MaxFloorColumns} 階まで）" : "");

            ApplyFilter(keepCode);
        }

        private List<BeamTypeRow> FilteredRows()
        {
            IEnumerable<BeamTypeRow> q = _rows;
            var text = _search?.Text?.Trim();
            if (!string.IsNullOrEmpty(text)) q = q.Where(r => r.BeamCode.Contains(text, StringComparison.OrdinalIgnoreCase)
                                                          || (r.InitialBeamCode ?? "").Contains(text, StringComparison.OrdinalIgnoreCase));
            if (_floorFilter?.SelectedIndex > 0) { var f = _floorFilter.SelectedItem as string ?? ""; q = q.Where(r => r.FloorCounts.GetValueOrDefault(f) > 0); }
            if (_needCheckOnly?.Checked == true) q = q.Where(r => r.NeedCheck);
            return q.ToList();
        }

        private void ApplyFilter(string? keepCode = null)
        {
            if (_building) return;
            keepCode ??= SelectedRow()?.BeamCode;
            var list = FilteredRows();
            table1.DataSource = list;
            int i = keepCode == null ? -1 : list.FindIndex(r => r.BeamCode == keepCode);
            if (i < 0 && list.Count > 0) i = 0;
            if (i >= 0)
            {
                table1.SelectedIndex = i + 1; // AntdUI の行番号は見出しが 0
                // AntdUI の SelectIndexChanged は行番号が変わったときだけ起きる（データを差し替えても行番号は残る）。
                // 絞り込みで同じ行番号のままだと右下の一覧・3D が前の梁番号のままになるので、直接呼ぶ
                ShowSelectedType();
            }
            else { _shownType = null; _shownMember = null; table2.DataSource = null; _beam3D.ShowCsv(null); _beamTitle.Text = "該当する梁がありません"; }
        }

        private BeamTypeRow? SelectedRow() =>
            table1.SelectedIndex > 0 && table1.DataSource is List<BeamTypeRow> l && table1.SelectedIndex - 1 < l.Count ? l[table1.SelectedIndex - 1] : null;

        private void table1_SelectIndexChanged(object sender, EventArgs e) => ShowSelectedType();

        private BeamTypeRow? _shownType;

        /// <summary>左で選んでいる梁番号の梁を右下に並べ、1 本を選んで 3D を表示する（同じ梁番号なら何もしない）。</summary>
        private void ShowSelectedType()
        {
            var row = SelectedRow();
            if (row == null) { _shownType = null; table2.DataSource = null; return; }
            if (ReferenceEquals(row, _shownType)) return; // イベントと直接呼び出しの二重実行を防ぐ
            _shownType = row;
            var members = row.Beams.Select(b => new MemberRow
            {
                Beam = b,
                Holes = _holeCounts.GetValueOrDefault(b.Id),
                Check = string.Join("・", new[]
                {
                    row.ShapeMismatch && (b.BeamSignature ?? "") != row.MajoritySignature ? "形状差" : null,
                    _holeCounts.GetValueOrDefault(b.Id) == 0 ? "穴なし" : null,
                }.Where(x => x != null)),
            }).ToList();
            table2.DataSource = members;
            // 形状差があれば、ほかと異なる梁を最初に表示する
            int pick = members.FindIndex(m => m.Check.Contains("形状差"));
            table2.SelectedIndex = (pick >= 0 ? pick : 0) + 1;
            // 右下の行番号が前と同じだと SelectIndexChanged が起きず 3D が切り替わらなかったため、直接呼ぶ
            _ = OnMemberSelectedAsync();
        }

        private MemberRow? SelectedMember() =>
            table2.SelectedIndex > 0 && table2.DataSource is List<MemberRow> l && table2.SelectedIndex - 1 < l.Count ? l[table2.SelectedIndex - 1] : null;

        private int _showVersion;

        /// <summary>選んだ梁を 3D で表示（CSV から描く。梁画面の 3D と同じ）。</summary>
        private MemberRow? _shownMember;

        private async Task OnMemberSelectedAsync()
        {
            var m = SelectedMember();
            if (m == null) return;
            if (ReferenceEquals(m, _shownMember)) return; // イベントと直接呼び出しの二重実行を防ぐ
            _shownMember = m;
            int ver = ++_showVersion;
            try
            {
                await JwProjectMainService.LoadBeamCollectionAsync(m.Beam);
                if (ver != _showVersion || IsDisposed) return; // 速く切り替えたときは最後の選択だけ表示
                string csv = BeamCsv(m.Beam, out var jb);
                _beam3D.ShowCsv(csv);
                var b = m.Beam;
                _beamTitle.Text = $"{b.BeamCode}　{b.FloorName}　長さ {b.Length:0.#}　端部 {EndText(b)}　穴 {m.Holes}"
                                  + (jb.HasBFG ? $"　G端 {(int)jb.BaiFangGTBDistance}" : "")
                                  + (b.HasCsv ? "" : "　（CSV対象外）");
            }
            catch (Exception ex)
            {
                JwLogging.AppLog.Warn(ex, "梁統計：3D を表示できません {Beam}", m.Beam.BeamCode);
                _beam3D.ShowCsv(null);
                _beamTitle.Text = $"{m.Beam.BeamCode}：3D を表示できませんでした";
                _shownMember = null; // もう一度選べば再試行できるように
            }
        }

        /// <summary>保存済みの梁から CSV を作る（その階の縮尺で計算。計算後は縮尺を元に戻す）。</summary>
        private string BeamCsv(JwBeamData data, out JwBeam beam)
        {
            double old = JwFileConsts.JwScale;
            try
            {
                if (_subs.TryGetValue(data.JwProjectSubDataId, out var sub) && sub.Scale > 0) JwFileConsts.JwScale = sub.Scale;
                beam = data.BeamDataToJwBeam(); // 穴の再計算（holeorder）まで行われる
                return beam.ToProcessCsv();
            }
            finally
            {
                JwFileConsts.JwScale = old;
            }
        }

        /// <summary>ダブルクリック：その階の図データを作り、梁画面（前へ／次へ・工区・29/35 も使える）を開く。閉じたら再集計。</summary>
        private async Task OpenBeamDetailAsync(MemberRow m)
        {
            if (!_subs.TryGetValue(m.Beam.JwProjectSubDataId, out var sub)) return;
            JwCanvas? canvas = null;
            await SpinRunner.RunAsync(this, async () =>
            {
                await JwProjectMainService.LoadSubCollectionAsync(sub);
                foreach (var bd in sub.JwBeamDatas) await JwProjectMainService.LoadBeamCollectionAsync(bd);
                JwFileConsts.JwScale = sub.Scale; // 階画面を開くときと同じ
                canvas = sub.DataToCanvas();
            });
            var beam = canvas?.Beams.FirstOrDefault(b => b.Id == m.Beam.Id);
            if (canvas == null || beam == null)
            {
                UIMessageBox.ShowWarning("この梁の図データを作成できませんでした。");
                return;
            }
            // 図で梁をクリックしたときと同じく、描画の基準位置を設定する
            if (beam.DirectionType == BeamDirectionType.Horizontal) beam.AbsolutePD = beam.TopLeft.X;
            if (beam.DirectionType == BeamDirectionType.Vertical) beam.AbsolutePD = beam.BottomLeft.Y;
            using (var f = new NewJwBeamForm(beam, canvas, canvas.Beams))
            {
                f.ShowDialog(this);
            }
            await ReloadAsync(m.Beam.BeamCode); // 工区・CSV 出力・29/35 の変更を反映
        }

        // ───────────────────────── Excel ─────────────────────────

        /// <summary>梁リスト（梁番号ごとの本数・階別本数・長さ・総長・端部・穴数・状態）を Excel に出力（絞り込みに関係なく全件）。</summary>
        private void ExportExcel()
        {
            if (_rows.Count == 0) { UIMessageBox.ShowWarning("出力する梁がありません。"); return; }
            using var dlg = new SaveFileDialog
            {
                Filter = "Excel ファイル(*.xlsx)|*.xlsx",
                FileName = $"{_selectedMainData.ProjectName}_梁リスト.xlsx",
            };
            if (ExportFolders.Show(dlg, ExportFolders.BeamListExcel) != DialogResult.OK) return;
            try
            {
                WriteBeamListExcel(dlg.FileName);
                JwLogging.AppLog.Info("梁リストを Excel 出力 {Project} → {File}", _selectedMainData.ProjectName, dlg.FileName);
                UIMessageBox.ShowSuccess("梁リストを出力しました");
            }
            catch (Exception ex)
            {
                RGBControls.Errors.ErrorReporter.Report(ex, "梁リストを出力できませんでした（ファイルが開かれていないか確認してください）。");
            }
        }

        /// <summary>梁リストの Excel を書き出す。</summary>
        private void WriteBeamListExcel(string path)
        {
            {
                var wb = new XSSFWorkbook();
                var sh = wb.CreateSheet("梁リスト");
                var bold = wb.CreateFont(); bold.IsBold = true;
                var head = wb.CreateCellStyle(); head.SetFont(bold);
                head.FillForegroundColor = IndexedColors.PaleBlue.Index; head.FillPattern = FillPattern.SolidForeground;
                head.BorderBottom = head.BorderTop = head.BorderLeft = head.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                var cell = wb.CreateCellStyle(); cell.BorderBottom = cell.BorderTop = cell.BorderLeft = cell.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                var num2 = wb.CreateCellStyle(); num2.CloneStyleFrom(cell); num2.DataFormat = wb.CreateDataFormat().GetFormat("0.00");
                var num1 = wb.CreateCellStyle(); num1.CloneStyleFrom(cell); num1.DataFormat = wb.CreateDataFormat().GetFormat("0.#");
                var totalStyle = wb.CreateCellStyle(); totalStyle.CloneStyleFrom(num2); totalStyle.SetFont(bold);
                var totalInt = wb.CreateCellStyle(); totalInt.CloneStyleFrom(cell); totalInt.SetFont(bold);

                var titleFont = wb.CreateFont(); titleFont.IsBold = true; titleFont.FontHeightInPoints = 14;
                var title = wb.CreateCellStyle(); title.SetFont(titleFont);
                var r0 = sh.CreateRow(0); r0.CreateCell(0).SetCellValue($"梁リスト　{_selectedMainData.ProjectName}"); r0.GetCell(0).CellStyle = title;
                sh.CreateRow(1).CreateCell(0).SetCellValue($"出力日 {DateTime.Now:yyyy/MM/dd}　梁種類 {_rows.Count}・総本数 {_rows.Sum(r => r.BeamCount)} 本・総長 {_rows.Sum(r => r.TotalMeters):0.00} m");

                var headers = new List<string> { "梁番号", "初期梁番号", "本数" };
                headers.AddRange(_floors);
                headers.AddRange(new[] { "長さ(mm)", "芯長(mm)", "総長(m)", "端部", "穴数", "状態" });
                var hr = sh.CreateRow(3);
                for (int c = 0; c < headers.Count; c++) { var x = hr.CreateCell(c); x.SetCellValue(headers[c]); x.CellStyle = head; }

                int rowIndex = 4;
                foreach (var r in _rows)
                {
                    var row = sh.CreateRow(rowIndex++);
                    int c = 0;
                    void S(string v) { var x = row.CreateCell(c++); x.SetCellValue(v); x.CellStyle = cell; }
                    void N(double v, ICellStyle st) { var x = row.CreateCell(c++); x.SetCellValue(v); x.CellStyle = st; }
                    S(r.BeamCode); S(r.InitialBeamCode ?? ""); N(r.BeamCount, cell);
                    foreach (var f in _floors) { int n = r.FloorCounts.GetValueOrDefault(f); if (n > 0) N(n, cell); else S(""); }
                    N(r.Length, num1); N(r.XXLength, num1); N(Math.Round(r.TotalMeters, 3), num2);
                    S(r.Ends); S(r.Holes); S(r.Status);
                }
                // 合計
                var tr = sh.CreateRow(rowIndex);
                int tc = 0;
                var t0 = tr.CreateCell(tc++); t0.SetCellValue("合計"); t0.CellStyle = totalInt;
                tr.CreateCell(tc++).CellStyle = totalInt;
                var tn = tr.CreateCell(tc++); tn.SetCellValue(_rows.Sum(r => r.BeamCount)); tn.CellStyle = totalInt;
                foreach (var f in _floors) { var x = tr.CreateCell(tc++); x.SetCellValue(_rows.Sum(r => r.FloorCounts.GetValueOrDefault(f))); x.CellStyle = totalInt; }
                tr.CreateCell(tc++).CellStyle = totalInt; tr.CreateCell(tc++).CellStyle = totalInt;
                var tm = tr.CreateCell(tc++); tm.SetCellValue(Math.Round(_rows.Sum(r => r.TotalMeters), 3)); tm.CellStyle = totalStyle;
                for (int i = 0; i < 3; i++) tr.CreateCell(tc++).CellStyle = totalInt;

                for (int c = 0; c < headers.Count; c++) sh.SetColumnWidth(c, (c < 2 ? 14 : headers[c].Length > 4 ? 11 : 8) * 256);
                sh.SetColumnWidth(headers.Count - 1, 24 * 256);
                sh.CreateFreezePane(1, 4);

                using (var fs = new FileStream(path, FileMode.Create)) wb.Write(fs);
            }
        }
    }
}
