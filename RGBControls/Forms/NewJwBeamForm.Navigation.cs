using JwShapeCommon;

namespace RGBJWMain.Forms
{
    /// <summary>
    /// 2026年11月22日 梁画面で「前へ／次へ」（← → キーでも可）。表示のみ（データは変えない）。
    /// 原来确认一根梁要关掉窗口再去图上点下一根。同じ階の梁を梁番号順に切り替える。
    /// 「要確認のみ」なら穴の無い梁（解析直後は終端穴なしも）だけを順に見る。
    /// 階の梁一覧が渡されたときだけ表示する（図から開いた場合）。
    /// </summary>
    public partial class NewJwBeamForm
    {
        private List<JwBeam>? _navAll;
        private Button? _prevButton, _nextButton;
        private CheckBox? _navNeedCheckOnly;
        private Label? _navPosition;
        private Label? _drawError;

        private Dictionary<string, int> _savedHoleCounts = new();

        /// <summary>
        /// 要確認の判定（工事一覧の状態列・階画面と同じ基準）：
        /// ・保存済みの梁：DB の穴が 0 個。
        /// ・解析直後で未保存の梁：解析で「終端穴なし」になった梁（進捗カードの ⚠ と同じ）。
        /// 図のデータの Holes は描画処理の途中で空になることがあり、1 本ごとの CSV は端部の穴を形状から作るため、どちらも基準にしない。
        /// </summary>
        private bool BeamNeedsCheck(JwBeam b) =>
            _savedHoleCounts.TryGetValue(b.Id ?? "", out var n) ? n == 0 : b.MissingEndHole;

        /// <summary>
        /// 要確認の理由。この画面の 2D 図・1 本ごとの CSV は端部の穴を形状から作るので穴が見えることがある。
        /// 何が「穴なし」なのかをはっきり書く（工事一覧の状態列・階の梁製造 CSV と同じ基準）。
        /// </summary>
        private string NeedCheckReason(JwBeam b) =>
            _savedHoleCounts.ContainsKey(b.Id ?? "")
                ? "保存データに穴がありません（階の梁製造CSVに穴が出ません）"
                : "解析時に終端穴が見つかりませんでした";

        private void LoadSavedHoleCounts()
        {
            if (_navAll == null) return;
            try { _savedHoleCounts = _jwProjectMainService.GetSavedHoleCounts(_navAll.Select(b => b.Id)); }
            catch (Exception ex) { JwLogging.AppLog.Warn(ex, "穴の数を読み込めませんでした"); }
        }

        /// <summary>切り替えに使う梁（同じ階の全梁）。図から開いたときに渡す。</summary>
        public NewJwBeamForm(JwBeam jwbeam, JwCanvas canvas, IEnumerable<JwBeam> floorBeams) : this(jwbeam, canvas)
        {
            _navAll = floorBeams.Where(b => b != null).OrderBy(b => b.BeamCode, StringComparer.Ordinal).ToList();
            LoadSavedHoleCounts();
            KeyPreview = true;
            KeyDown += OnNavKeyDown;
            Shown += (s, e) => InitNavigation();
        }

        private List<JwBeam> NavList() =>
            _navAll == null ? new List<JwBeam>()
            : _navNeedCheckOnly?.Checked == true ? _navAll.Where(BeamNeedsCheck).ToList() : _navAll;

        private void InitNavigation()
        {
            if (_navAll == null || _prevButton != null) return;
            _prevButton = new Button { Text = "◀ 前へ", AutoSize = true, Margin = new Padding(0, 2, 4, 0) };
            _nextButton = new Button { Text = "次へ ▶", AutoSize = true, Margin = new Padding(0, 2, 12, 0) };
            _prevButton.Click += (s, e) => Move(-1);
            _nextButton.Click += (s, e) => Move(+1);
            _navNeedCheckOnly = new CheckBox { Text = "要確認のみ", AutoSize = true, Margin = new Padding(0, 6, 8, 0) };
            _navNeedCheckOnly.CheckedChanged += (s, e) =>
            {
                // 絞り込みで今の梁が外れたら、最初の要確認の梁へ
                var list = NavList();
                if (list.Count > 0 && !list.Contains(_jwbeam)) ShowBeam(list[0]);
                else UpdateNavState();
            };
            _navPosition = new Label { AutoSize = true, Margin = new Padding(0, 8, 0, 0) };
            var tip = new ToolTip();
            tip.SetToolTip(_prevButton, "前の梁（← キー）");
            tip.SetToolTip(_nextButton, "次の梁（→ キー）");

            var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 36, Padding = new Padding(8, 2, 8, 0), WrapContents = false };
            bar.Controls.AddRange(new Control[] { _prevButton, _nextButton, _navNeedCheckOnly, _navPosition });
            panel6.Controls.Add(bar);
            bar.SendToBack(); // 図より先に Dock=Top を確定させる
            UpdateNavState();
        }

        private void OnNavKeyDown(object? sender, KeyEventArgs e)
        {
            // 文字入力中（工区の入力など）は矢印キーを奪わない
            if (ActiveControl is TextBoxBase || ActiveControl is Sunny.UI.UITextBox || ActiveControl is ComboBox) return;
            if (e.KeyCode == Keys.Left) { Move(-1); e.Handled = true; }
            else if (e.KeyCode == Keys.Right) { Move(+1); e.Handled = true; }
        }

        private void Move(int delta)
        {
            var list = NavList();
            if (list.Count == 0) return;
            int i = list.IndexOf(_jwbeam);
            int next = i < 0 ? 0 : i + delta;
            if (next < 0 || next >= list.Count) return;
            ShowBeam(list[next]);
        }

        private void UpdateNavState()
        {
            if (_prevButton == null || _nextButton == null || _navPosition == null) return;
            var list = NavList();
            int i = list.IndexOf(_jwbeam);
            _prevButton.Enabled = i > 0;
            _nextButton.Enabled = i >= 0 ? i < list.Count - 1 : list.Count > 0;
            int need = _navAll?.Count(BeamNeedsCheck) ?? 0;
            _navPosition.Text = (i >= 0 ? $"{i + 1} / {list.Count} 本" : $"－ / {list.Count} 本")
                                + (need > 0 ? $"（要確認 {need} 本）" : "")
                                + (BeamNeedsCheck(_jwbeam) ? "　要確認：" + NeedCheckReason(_jwbeam) : "");
            _navPosition.ForeColor = BeamNeedsCheck(_jwbeam) ? Color.Firebrick : SystemColors.ControlText;
        }

        /// <summary>表示中の梁を切り替える（開いたときと同じ処理：2D 図・CSV・3D・工区）。</summary>
        private void ShowBeam(JwBeam beam)
        {
            if (beam == null || ReferenceEquals(beam, _jwbeam)) { UpdateNavState(); return; }
            // 図で梁をクリックしたときと同じく、描画の基準位置を設定する（JwShowBeams の選択処理と同じ）
            if (beam.DirectionType == JwCore.BeamDirectionType.Horizontal) beam.AbsolutePD = beam.TopLeft.X;
            if (beam.DirectionType == JwCore.BeamDirectionType.Vertical) beam.AbsolutePD = beam.BottomLeft.Y;
            _jwbeam = beam;
            beamData = _jwProjectMainService.FindBeamDataById(beam.Id);
            try
            {
                beamShow();
                _singleBeamShow?.BringToFront(); // Dock=Fill は最後に配置（上部の帯の下を埋める）
                if (_drawError != null) { panel6.Controls.Remove(_drawError); _drawError.Dispose(); _drawError = null; }
            }
            catch (Exception ex)
            {
                // 既知の問題：一部の短い梁は 2D 図の作成（DrawToJwwwSingle）で例外になる（図からクリックしても同じ）。
                // 切り替えでは止めずに知らせ、CSV・3D は表示する
                JwLogging.AppLog.Warn(ex, "梁の 2D 図を作成できません {Beam}", beam.BeamCode);
                if (_singleBeamShow != null) { panel6.Controls.Remove(_singleBeamShow); _singleBeamShow = null!; }
                _drawError ??= new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.Firebrick };
                _drawError.Text = $"梁 {beam.BeamCode} の 2D 図を作成できませんでした（CSV・3D 表示は使えます）。";
                if (!panel6.Controls.Contains(_drawError)) panel6.Controls.Add(_drawError);
                _drawError.BringToFront();
            }
            uiTextBox1.Text = _jwbeam.ToProcessCsv(); // 3D はこの CSV から描く
            ApplyViewMode();
            if (!string.IsNullOrEmpty(_jwbeam.GongQu)) select7.SelectedValue = _jwbeam.GongQu;
            UpdateNavState();
        }
    }
}
