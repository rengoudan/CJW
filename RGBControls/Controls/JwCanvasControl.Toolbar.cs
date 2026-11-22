using System.Drawing;
using JwCore;
using JwServices;
using JwShapeCommon;
using JwSharedConfig.Config;

namespace RGBJWMain.Controls
{
    /// <summary>
    /// 2026年11月23日 図の表示の切り替えを「表示モード＋レイヤー」にまとめる（表示のみ。データ・出力は変えない）。
    /// 原来 11 个「标签＋On/Off」开关按固定坐标排两行，窄的窗口（解析画面）会溢出、名字被截断。
    ///   ・表示モード：標準／工区／金物／寸法（作業ごとの組み合わせ。手で変えると「カスタム」）
    ///   ・レイヤー▾：11＋要確認の項目を「部材／文字／補助」に分けたチェック一覧（件数付き）
    ///   ・梁番号の検索、要確認の梁へ ◀ ▶ で移動
    ///   ・図の右下に 全体／＋／－、下に状態バー（縮尺・表示倍率・カーソル位置・件数・選択中の梁）
    ///   ・画面の種類（ViewKey：parse／floor／canvas）ごとに、最後の表示を本機に記憶
    /// 旧スイッチは非表示にして残す（読み込み時に従来どおり全部オンにされた後、ここで表示モードを適用する）。
    /// </summary>
    public partial class JwCanvasControl
    {
        public enum Layer { Beam, Pillar, DownPillar, Cutting, BBG, DownB, BeamCode, BBGText, Fuzhu, Dimension, GongquColor, NeedCheck }

        private static readonly (string Group, (Layer Layer, string Name, string Tip)[] Items)[] LayerGroups =
        {
            ("部材", new[]
            {
                (Layer.Beam, "梁", "梁の外形"),
                (Layer.Pillar, "柱", "柱"),
                (Layer.DownPillar, "下階柱", "下階柱の記号"),
                (Layer.Cutting, "継手記号", "継手（切断）の記号"),
                (Layer.BBG, "B/BG", "B／BG 金物"),
                (Layer.DownB, "B(下のみ柱あり)", "下階にだけ柱がある B 金物"),
            }),
            ("文字", new[]
            {
                (Layer.BeamCode, "梁符号", "梁番号の文字"),
                (Layer.BBGText, "B/BG自動統計", "B／BG 金物の番号"),
            }),
            ("補助", new[]
            {
                (Layer.Fuzhu, "補助線", "梁の中心の補助線"),
                (Layer.Dimension, "寸法", "通り芯（梁の中心線）の寸法"),
                (Layer.GongquColor, "工区色", "梁を工区の色で表示し、凡例を出す"),
                (Layer.NeedCheck, "要確認", "穴の無い梁に赤い破線の枠"),
            }),
        };

        private static readonly (string Mode, Layer[] Layers)[] Modes =
        {
            ("標準", new[] { Layer.Beam, Layer.Pillar, Layer.DownPillar, Layer.Cutting, Layer.BBG, Layer.BeamCode, Layer.Dimension, Layer.NeedCheck }),
            ("工区", new[] { Layer.Beam, Layer.BeamCode, Layer.GongquColor }),
            ("金物", new[] { Layer.Beam, Layer.Pillar, Layer.BBG, Layer.BBGText, Layer.DownB }),
            ("寸法", new[] { Layer.Beam, Layer.Fuzhu, Layer.Dimension, Layer.BeamCode }),
        };
        private const string DefaultMode = "標準";

        private readonly HashSet<Layer> _layers = new();
        private string? _mode = DefaultMode;
        private string _viewKey = "canvas";
        private readonly Dictionary<string, RadioButton> _modeButtons = new();
        private Label? _customLabel, _needCheckLabel, _status;
        private Button? _needPrev, _needNext;
        private TextBox? _search;
        private ContextMenuStrip? _layerMenu;
        private readonly Dictionary<Layer, ToolStripMenuItem> _layerItems = new();
        private bool _applying;
        private List<JwBeam> _needCheckBeams = new();
        private int _needIndex = -1;
        private List<JwBeam> _searchHits = new();
        private int _searchIndex = -1;
        private string _lastSearch = "";
        private Dictionary<string, int> _savedHoles = new();

        private static readonly Color Primary = Color.FromArgb(80, 160, 255);

        /// <summary>画面の種類（parse＝解析画面、floor＝階画面、canvas＝その他）。表示モードの記憶に使う。</summary>
        public string ViewKey
        {
            get => _viewKey;
            set
            {
                _viewKey = string.IsNullOrEmpty(value) ? "canvas" : value;
                LoadViewState();
                ApplyLayers();
            }
        }

        // ───────── 構築 ─────────

        private void InitViewToolbar()
        {
            // 旧スイッチ（標籤＋On/Off 共 11 組）は隠して、1 行のツールバーに置き換える
            // 旧スイッチは配置の計算に入らない入れ物へ移す（処理は残す）
            var legacy = new Panel { Visible = false };
            foreach (Control c in panel2.Controls.Cast<Control>().ToList()) legacy.Controls.Add(c);
            Controls.Add(legacy);
            // 幅が足りないときは折り返し、高さは中身に合わせる（解析画面の狭い図でもはみ出さない）。
            // FlowLayoutPanel の AutoSize は 1 行ぶんの高さしか計算しないため、並べた結果から高さを決める
            var bar = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8, 6, 8, 4), WrapContents = true };
            bar.Layout += (s, e) =>
            {
                var visible = bar.Controls.Cast<Control>().Where(c => c.Visible).ToList();
                if (visible.Count == 0) return;
                int h = visible.Max(c => c.Bottom + c.Margin.Bottom) + bar.Padding.Bottom;
                if (panel2.Height != h) panel2.Height = h;
            };
            bar.Controls.Add(MakeLabel("表示", bold: true));
            foreach (var (mode, _) in Modes)
            {
                var rb = MakeSegment(mode);
                rb.CheckedChanged += (s, e) =>
                {
                    StyleSegment(rb);
                    if (rb.Checked && !_applying) SetMode(mode);
                };
                _modeButtons[mode] = rb;
                bar.Controls.Add(rb);
            }
            _customLabel = MakeLabel("カスタム", bold: false);
            _customLabel.ForeColor = Color.DimGray;
            _customLabel.Visible = false;
            bar.Controls.Add(_customLabel);

            var layerButton = MakeButton("レイヤー ▾", "表示する項目を個別に選ぶ");
            _layerMenu = BuildLayerMenu();
            layerButton.Click += (s, e) => _layerMenu.Show(layerButton, new Point(0, layerButton.Height));
            bar.Controls.Add(Separator());
            bar.Controls.Add(layerButton);

            bar.Controls.Add(Separator());
            _search = new TextBox { Width = 150, PlaceholderText = "梁番号で検索（Enter）", Margin = new Padding(0, 4, 6, 0) };
            _search.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; SearchNext(); } };
            bar.Controls.Add(_search);

            bar.Controls.Add(Separator());
            _needCheckLabel = MakeLabel("要確認 －", bold: false);
            _needPrev = MakeButton("◀", "前の要確認の梁へ");
            _needNext = MakeButton("▶", "次の要確認の梁へ");
            _needPrev.Click += (s, e) => MoveNeedCheck(-1);
            _needNext.Click += (s, e) => MoveNeedCheck(+1);
            bar.Controls.Add(_needCheckLabel);
            bar.Controls.Add(_needPrev);
            bar.Controls.Add(_needNext);
            panel2.Controls.Add(bar);
            bar.BringToFront();

            // 図の右下：全体／＋／－
            var zoom = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, BackColor = Color.FromArgb(40, 40, 40), Padding = new Padding(3), WrapContents = false };
            var fit = MakeButton("全体", "全体表示（ダブルクリック・Home キーでも可）", dark: true);
            var plus = MakeButton("＋", "拡大（ホイールでも可）", dark: true);
            var minus = MakeButton("－", "縮小（ホイールでも可）", dark: true);
            fit.Click += (s, e) => jwShowBeams1.FitAll();
            plus.Click += (s, e) => jwShowBeams1.ZoomBy(1.25f);
            minus.Click += (s, e) => jwShowBeams1.ZoomBy(1 / 1.25f);
            zoom.Controls.AddRange(new Control[] { fit, plus, minus });
            jwShowBeams1.Controls.Add(zoom);
            void PlaceZoom() => zoom.Location = new Point(jwShowBeams1.Width - zoom.Width - 12, jwShowBeams1.Height - zoom.Height - 12);
            jwShowBeams1.Resize += (s, e) => PlaceZoom();
            zoom.SizeChanged += (s, e) => PlaceZoom();

            // 下：状態バー
            _status = new Label { Dock = DockStyle.Bottom, Height = 26, Padding = new Padding(10, 5, 0, 0), BackColor = Color.FromArgb(243, 249, 255), ForeColor = Color.FromArgb(48, 48, 48) };
            panel1.Controls.Add(_status);
            jwShowBeams1.BringToFront(); // Fill は最後に配置
            jwShowBeams1.ViewStateChanged += (s, e) => UpdateStatus();

            LoadViewState();
            SyncToolbar();
        }

        private static Label MakeLabel(string text, bool bold) => new()
        {
            Text = text, AutoSize = true, Margin = new Padding(0, 8, 6, 0),
            Font = bold ? new Font(SystemFonts.MessageBoxFont ?? Control.DefaultFont, FontStyle.Bold) : null,
        };

        private static Control Separator() => new Label { AutoSize = false, Width = 1, Height = 22, BackColor = Color.FromArgb(200, 210, 225), Margin = new Padding(8, 5, 8, 0) };

        private static RadioButton MakeSegment(string text)
        {
            var rb = new RadioButton
            {
                Text = text, Appearance = Appearance.Button, AutoSize = true, FlatStyle = FlatStyle.Flat,
                Padding = new Padding(10, 1, 10, 1), Margin = new Padding(0, 1, 2, 0), TextAlign = ContentAlignment.MiddleCenter, Cursor = Cursors.Hand,
            };
            rb.FlatAppearance.BorderColor = Primary;
            rb.FlatAppearance.CheckedBackColor = Primary;
            rb.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 238, 255);
            StyleSegment(rb);
            return rb;
        }

        private static void StyleSegment(RadioButton rb)
        {
            rb.ForeColor = rb.Checked ? Color.White : Primary;
            rb.BackColor = rb.Checked ? Primary : Color.White;
        }

        private static Button MakeButton(string text, string tip, bool dark = false)
        {
            var b = new Button
            {
                Text = text, AutoSize = true, FlatStyle = FlatStyle.Flat, Margin = new Padding(dark ? 2 : 0, dark ? 0 : 1, 4, 0),
                Padding = new Padding(6, 0, 6, 0), Cursor = Cursors.Hand,
                BackColor = dark ? Color.FromArgb(60, 60, 60) : Color.White, ForeColor = dark ? Color.White : Color.FromArgb(48, 48, 48),
            };
            b.FlatAppearance.BorderColor = dark ? Color.FromArgb(90, 90, 90) : Color.FromArgb(200, 210, 225);
            new ToolTip().SetToolTip(b, tip);
            return b;
        }

        private ContextMenuStrip BuildLayerMenu()
        {
            var menu = new ContextMenuStrip { ShowCheckMargin = true, ShowImageMargin = false };
            bool first = true;
            foreach (var (group, items) in LayerGroups)
            {
                if (!first) menu.Items.Add(new ToolStripSeparator());
                first = false;
                menu.Items.Add(new ToolStripMenuItem(group) { Enabled = false, Font = new Font(menu.Font, FontStyle.Bold) });
                foreach (var (layer, name, tip) in items)
                {
                    var item = new ToolStripMenuItem(name) { CheckOnClick = true, ToolTipText = tip, Tag = name };
                    item.CheckedChanged += (s, e) =>
                    {
                        if (_applying) return;
                        if (item.Checked) _layers.Add(layer); else _layers.Remove(layer);
                        _mode = MatchMode();
                        ApplyLayers();
                        SaveViewState();
                    };
                    _layerItems[layer] = item;
                    menu.Items.Add(item);
                }
            }
            menu.Items.Add(new ToolStripSeparator());
            var reset = new ToolStripMenuItem("標準に戻す");
            reset.Click += (s, e) => SetMode(DefaultMode);
            menu.Items.Add(reset);
            // 項目をクリックしても閉じない（続けて選べる）
            menu.Closing += (s, e) => { if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked) e.Cancel = true; };
            return menu;
        }

        // ───────── モード・レイヤー ─────────

        private void SetMode(string mode)
        {
            var def = Modes.FirstOrDefault(m => m.Mode == mode);
            if (def.Layers == null) return;
            _layers.Clear();
            foreach (var l in def.Layers) _layers.Add(l);
            _mode = mode;
            ApplyLayers();
            SaveViewState();
        }

        private string? MatchMode() =>
            Modes.Where(m => m.Layers.Length == _layers.Count && m.Layers.All(_layers.Contains)).Select(m => m.Mode).FirstOrDefault();

        /// <summary>今のレイヤーを図に反映し、ツールバーの表示を合わせる。</summary>
        private void ApplyLayers()
        {
            var v = jwShowBeams1;
            v.ShowBeams = _layers.Contains(Layer.Beam);
            v.ShowPillar = _layers.Contains(Layer.Pillar);
            v.ShowDownPillar = _layers.Contains(Layer.DownPillar);
            v.ShowCutting = _layers.Contains(Layer.Cutting);
            v.ShowGoujian = _layers.Contains(Layer.BBG);
            v.ShowDownB = _layers.Contains(Layer.DownB);
            v.Showmsg = _layers.Contains(Layer.BeamCode);
            v.ShowGoujiantext = _layers.Contains(Layer.BBGText);
            v.ShowFuzhu = _layers.Contains(Layer.Fuzhu);
            v.ShowDimension = _layers.Contains(Layer.Dimension);
            v.ShowColor = _layers.Contains(Layer.GongquColor);
            v.ShowNeedCheck = _layers.Contains(Layer.NeedCheck);
            SyncToolbar();
        }

        private void SyncToolbar()
        {
            _applying = true;
            try
            {
                foreach (var (mode, rb) in _modeButtons) rb.Checked = mode == _mode;
                if (_customLabel != null) _customLabel.Visible = _mode == null;
                foreach (var (layer, item) in _layerItems) item.Checked = _layers.Contains(layer);
            }
            finally { _applying = false; }
        }

        // ───────── 記憶（本機の appconfig.json） ─────────

        private void LoadViewState()
        {
            _layers.Clear();
            string? saved = null;
            try { AppConfigManager.Current.Ui.CanvasViews.TryGetValue(_viewKey, out saved); } catch { }
            // 形式：「標準」などのモード名、または「custom:Beam,Pillar,…」
            if (!string.IsNullOrEmpty(saved) && saved.StartsWith("custom:"))
            {
                foreach (var name in saved.Substring(7).Split(',', StringSplitOptions.RemoveEmptyEntries))
                    if (Enum.TryParse<Layer>(name, out var l)) _layers.Add(l);
                _mode = MatchMode();
                if (_layers.Count > 0) return;
            }
            var mode = Modes.Any(m => m.Mode == saved) ? saved! : DefaultMode;
            foreach (var l in Modes.First(m => m.Mode == mode).Layers) _layers.Add(l);
            _mode = mode;
        }

        private void SaveViewState()
        {
            try
            {
                var map = AppConfigManager.Current.Ui.CanvasViews;
                string value = _mode ?? "custom:" + string.Join(",", _layers.OrderBy(l => l));
                if (map.TryGetValue(_viewKey, out var old) && old == value) return;
                map[_viewKey] = value;
                AppConfigManager.Save();
            }
            catch (Exception ex)
            {
                JwLogging.AppLog.Warn(ex, "図の表示設定を保存できませんでした");
            }
        }

        // ───────── 図の読み込み時 ─────────

        /// <summary>CanvasDraw の設定（旧スイッチが全部オンにされた）後に呼ぶ：表示モードの適用・要確認・件数。</summary>
        private void OnCanvasLoaded()
        {
            var canvas = _canvasDraw?.jwCanvas;
            _savedHoles = new Dictionary<string, int>();
            if (canvas?.Beams != null)
            {
                try
                {
                    // 保存済みの梁は DB の穴の数で判定（工事一覧の状態列と同じ）。解析直後の梁は DB に無い
                    _savedHoles = ServiceFactory.GetInstance().CreateJwProjectMainService().GetSavedHoleCounts(canvas.Beams.Select(b => b.Id));
                }
                catch (Exception ex) { JwLogging.AppLog.Warn(ex, "穴の数を読み込めませんでした"); }
            }
            jwShowBeams1.IsNeedCheck = b => _savedHoles.TryGetValue(b.Id ?? "", out var n) ? n == 0 : b.MissingEndHole;
            jwShowBeams1.HoleCount = b => _savedHoles.TryGetValue(b.Id ?? "", out var n) ? n : b.Holes.Count;
            _needCheckBeams = canvas?.Beams?.Where(b => jwShowBeams1.IsNeedCheck(b)).OrderBy(b => b.BeamCode, StringComparer.Ordinal).ToList() ?? new();
            _needIndex = -1;
            _searchHits.Clear(); _searchIndex = -1; _lastSearch = "";
            if (_needCheckLabel != null)
            {
                _needCheckLabel.Text = _needCheckBeams.Count > 0 ? $"要確認 {_needCheckBeams.Count}" : "要確認 なし";
                _needCheckLabel.ForeColor = _needCheckBeams.Count > 0 ? Color.Firebrick : Color.SeaGreen;
            }
            if (_needPrev != null) _needPrev.Enabled = _needCheckBeams.Count > 0;
            if (_needNext != null) _needNext.Enabled = _needCheckBeams.Count > 0;
            UpdateLayerCounts();
            ApplyLayers();
            UpdateStatus();
        }

        private void UpdateLayerCounts()
        {
            var cd = _canvasDraw;
            var canvas = cd?.jwCanvas;
            int? Count(Layer l) => l switch
            {
                Layer.Beam => canvas?.Beams?.Count,
                Layer.Pillar => canvas?.Pillars?.Count,
                Layer.DownPillar => cd?.DownPillars?.Count,
                Layer.Cutting => cd?.CuttingDraws?.Count,
                Layer.BBG => cd?.links?.Count(k => !k.LinkPart.IsDownPillar),
                Layer.DownB => cd?.links?.Count(k => k.LinkPart.IsDownPillar),
                Layer.NeedCheck => _needCheckBeams.Count,
                _ => null,
            };
            foreach (var (layer, item) in _layerItems)
            {
                var n = Count(layer);
                item.Text = n.HasValue ? $"{item.Tag}（{n}）" : (string)item.Tag!;
            }
        }

        // ───────── 検索・要確認 ─────────

        private void SearchNext()
        {
            var canvas = _canvasDraw?.jwCanvas;
            var q = _search?.Text.Trim() ?? "";
            if (canvas?.Beams == null || q.Length == 0) return;
            if (!string.Equals(q, _lastSearch, StringComparison.OrdinalIgnoreCase))
            {
                _lastSearch = q;
                _searchHits = canvas.Beams.Where(b => (b.BeamCode ?? "").Contains(q, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(b => string.Equals(b.BeamCode, q, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                    .ThenBy(b => b.BeamCode, StringComparer.Ordinal).ToList();
                _searchIndex = -1;
            }
            if (_searchHits.Count == 0)
            {
                SetStatusMessage($"「{q}」に一致する梁はありません");
                return;
            }
            _searchIndex = (_searchIndex + 1) % _searchHits.Count;
            var beam = _searchHits[_searchIndex];
            if (!_layers.Contains(Layer.Beam)) { _layers.Add(Layer.Beam); _mode = MatchMode(); ApplyLayers(); }
            jwShowBeams1.FocusBeam(beam);
            SetStatusMessage($"{beam.BeamCode}（{_searchIndex + 1}/{_searchHits.Count}）　Enter で次へ");
        }

        private void MoveNeedCheck(int delta)
        {
            if (_needCheckBeams.Count == 0) return;
            _needIndex = ((_needIndex + delta) % _needCheckBeams.Count + _needCheckBeams.Count) % _needCheckBeams.Count;
            var beam = _needCheckBeams[_needIndex];
            if (!_layers.Contains(Layer.NeedCheck) || !_layers.Contains(Layer.Beam))
            {
                _layers.Add(Layer.NeedCheck); _layers.Add(Layer.Beam);
                _mode = MatchMode();
                ApplyLayers();
            }
            jwShowBeams1.FocusBeam(beam);
            SetStatusMessage($"要確認 {_needIndex + 1}/{_needCheckBeams.Count}：{beam.BeamCode}（穴なし）");
        }

        // ───────── 状態バー ─────────

        private string? _statusMessage;
        private DateTime _statusMessageUntil;

        private void SetStatusMessage(string text)
        {
            _statusMessage = text;
            _statusMessageUntil = DateTime.Now.AddSeconds(4);
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (_status == null) return;
            var v = jwShowBeams1;
            var canvas = _canvasDraw?.jwCanvas;
            if (canvas == null) { _status.Text = "図面がありません"; return; }
            var parts = new List<string>
            {
                $"縮尺 1/{JwFileConsts.JwScale:0.#}",
                $"表示 {v.ZoomScale * 100:0}%",
            };
            var real = v.RealAt(v.CursorPosition);
            if (real.HasValue) parts.Add($"X {real.Value.X:#,0}　Y {real.Value.Y:#,0}");
            parts.Add($"梁 {canvas.Beams?.Count ?? 0}・柱 {canvas.Pillars?.Count ?? 0}・B/BG {_canvasDraw?.links?.Count ?? 0}");
            var sel = v.HoverBeam ?? (v.IsBeamSelected ? v.SelectedBeam : null);
            if (sel != null) parts.Add($"{(v.HoverBeam != null ? "カーソル" : "選択")}：{sel.BeamCode}");
            if (_statusMessage != null && DateTime.Now < _statusMessageUntil) parts.Add(_statusMessage);
            _status.Text = string.Join("　│　", parts);
        }
    }
}
