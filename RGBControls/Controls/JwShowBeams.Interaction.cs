using System.Drawing;
using System.Drawing.Drawing2D;
using JwCore;
using JwShapeCommon;

namespace RGBJWMain.Controls
{
    /// <summary>
    /// 2026年11月23日 図の操作と表示の追加（表示のみ。データ・出力は変えない）。
    /// ・ホイール＝拡大縮小（Ctrl 不要、カーソル位置中心）、左ドラッグ＝移動、ダブルクリック／Home＝全体表示
    /// ・梁にカーソルを合わせると強調し、梁番号・長さ・端部・穴数・工区・CSV を情報カードで表示
    /// ・要確認の梁に赤い破線の枠（ShowNeedCheck）、梁へ移動して点滅（FocusBeam：検索・要確認の移動）
    /// ・工区の凡例は画面座標で、使っている工区だけ本数付きで表示
    /// ・縮小（全体表示より小さく）したときは、ブレース長さ・B/BG の番号などの細かい文字を省く
    /// </summary>
    public partial class JwShowBeams
    {
        private const float MinZoom = 0.4f, MaxZoom = 20f;
        private const int DragThreshold = 4;

        private bool _dragging, _suppressClick, _leftDown;
        private Point _dragStart;
        private PointF _originAtDragStart;
        private Point _cursor;
        private ControlDraw? _hover;
        private ControlDraw? _focus;
        private bool _focusBlink = true;
        private System.Windows.Forms.Timer? _focusTimer;
        private int _focusTicks;
        private bool _showNeedCheck = true;

        /// <summary>表示・操作の状態が変わった（拡大率・カーソル位置・選択・ホバー）。状態バーの更新用。</summary>
        public event EventHandler? ViewStateChanged;

        /// <summary>要確認の梁か（JwCanvasControl が DB の穴の数などから設定）。</summary>
        public Func<JwBeam, bool>? IsNeedCheck { get; set; }

        /// <summary>梁の穴の数（保存済みは DB の値）。null なら図のデータの穴の数。</summary>
        public Func<JwBeam, int>? HoleCount { get; set; }

        /// <summary>要確認の梁の枠を表示するか。</summary>
        public bool ShowNeedCheck
        {
            get => _showNeedCheck;
            set { _showNeedCheck = value; Invalidate(); }
        }

        /// <summary>現在の拡大率（全体表示＝1）。</summary>
        public float ZoomScale => scale;

        /// <summary>縮小したときは細かい文字を省く（全体表示以上の大きさでは従来どおり表示）。</summary>
        public bool ShowDetailText => scale >= 0.8f;

        /// <summary>カーソル位置（画面座標）。</summary>
        public Point CursorPosition => _cursor;

        /// <summary>カーソルの下の梁（無ければ null）。</summary>
        public JwBeam? HoverBeam => _hover?.JwSquareBase as JwBeam;

        private void InitInteraction()
        {
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
            MouseDown += (s, e) =>
            {
                Focus(); // Home キーを受けるため
                if (e.Button != MouseButtons.Left) return;
                _leftDown = true;
                _dragging = false;
                _dragStart = e.Location;
                _originAtDragStart = origin;
            };
            MouseMove += OnCanvasMouseMove;
            MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (_dragging) Cursor = Cursors.Default;
                    _leftDown = false;
                    _dragging = false;
                }
                RaiseViewChanged(); // クリックでの選択も反映
            };
            MouseLeave += (s, e) =>
            {
                if (_hover != null) { _hover = null; Invalidate(); RaiseViewChanged(); }
            };
            DoubleClick += (s, e) =>
            {
                // 空いている所のダブルクリック＝全体表示（梁の上は従来どおりクリック＝梁画面）
                if (HitBeam(PointToClient(MousePosition)) == null) FitAll();
            };
            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Home) { FitAll(); e.Handled = true; }
                else if (e.KeyCode == Keys.Escape) { ClearFocus(); e.Handled = true; }
            };
        }

        private void RaiseViewChanged() => ViewStateChanged?.Invoke(this, EventArgs.Empty);

        private void OnCanvasMouseMove(object? sender, MouseEventArgs e)
        {
            _cursor = e.Location;
            if (_leftDown && e.Button == MouseButtons.Left)
            {
                if (!_dragging && (Math.Abs(e.X - _dragStart.X) > DragThreshold || Math.Abs(e.Y - _dragStart.Y) > DragThreshold))
                {
                    _dragging = true;
                    _suppressClick = true; // ドラッグ後のクリックで梁を選ばない
                    Cursor = Cursors.SizeAll;
                }
                if (_dragging)
                {
                    origin = new PointF(_originAtDragStart.X + e.X - _dragStart.X, _originAtDragStart.Y + e.Y - _dragStart.Y);
                    Invalidate();
                    RaiseViewChanged();
                    return;
                }
            }
            var hit = HitBeam(e.Location);
            if (!ReferenceEquals(hit, _hover))
            {
                _hover = hit;
                Invalidate();
            }
            else if (_hover != null)
            {
                Invalidate(); // 情報カードをカーソルに追従させる
            }
            RaiseViewChanged();
        }

        /// <summary>画面座標の位置にある梁（クリックと同じ判定）。</summary>
        private ControlDraw? HitBeam(Point p)
        {
            if (_bounds == null || !_showBeams) return null;
            var logic = new PointF((p.X - origin.X) / scale, (p.Y - origin.Y) / scale);
            return _bounds.FirstOrDefault(t => t.ShapeType == DrawShapeType.Beam && t.JwSquareBase is JwBeam && t.DrawRectangleF.Contains(logic));
        }

        // ───────── 拡大縮小・移動 ─────────

        /// <summary>画面座標 p を中心に factor 倍する。</summary>
        public void ZoomAt(Point p, float factor)
        {
            float old = scale;
            float next = Math.Clamp(scale * factor, MinZoom, MaxZoom);
            if (Math.Abs(next - old) < 1e-4) return;
            scale = next;
            origin = new PointF(p.X - (p.X - origin.X) * (scale / old), p.Y - (p.Y - origin.Y) * (scale / old));
            Invalidate();
            RaiseViewChanged();
        }

        /// <summary>画面の中央を中心に拡大縮小（＋／－ボタン）。</summary>
        public void ZoomBy(float factor) => ZoomAt(new Point(Width / 2, Height / 2), factor);

        /// <summary>全体表示（読み込み直後と同じ表示）。</summary>
        public void FitAll()
        {
            ClearFocus();
            scale = 1f;
            origin = new PointF(0, 0);
            Invalidate();
            RaiseViewChanged();
        }

        /// <summary>梁へ移動・拡大し、しばらく点滅させる（検索・要確認の移動）。</summary>
        public bool FocusBeam(JwBeam beam)
        {
            var d = _bounds?.FirstOrDefault(t => t.ShapeType == DrawShapeType.Beam && ReferenceEquals(t.JwSquareBase, beam));
            if (d == null) return false;
            var r = Box(d);
            float span = Math.Max(Math.Max(r.Width, r.Height), 1f);
            float s = Math.Clamp(Math.Min(Width, Height) / (span * 1.8f), 1f, 8f);
            scale = s;
            origin = new PointF(Width / 2f - (r.Left + r.Width / 2) * s, Height / 2f - (r.Top + r.Height / 2) * s);
            _focus = d;
            _focusTicks = 0;
            _focusBlink = true;
            _focusTimer ??= new System.Windows.Forms.Timer { Interval = 250 };
            _focusTimer.Tick -= OnFocusTick;
            _focusTimer.Tick += OnFocusTick;
            _focusTimer.Start();
            Invalidate();
            RaiseViewChanged();
            return true;
        }

        /// <summary>検索・要確認の移動で付けた強調枠を消す（全体表示・Esc）。</summary>
        public void ClearFocus()
        {
            _focusTimer?.Stop();
            if (_focus == null) return;
            _focus = null;
            Invalidate();
        }

        private void OnFocusTick(object? sender, EventArgs e)
        {
            _focusTicks++;
            _focusBlink = !_focusBlink;
            if (_focusTicks >= 10) { _focusTimer?.Stop(); _focusBlink = true; } // 点滅後も枠は残す（次の移動まで）
            Invalidate();
        }

        /// <summary>画面座標 → 図面の実寸座標（mm）。図が無ければ null。</summary>
        public (double X, double Y)? RealAt(Point p)
        {
            var cd = CanvasDraw;
            if (cd == null || cd.Beilv == 0) return null;
            double lx = (p.X - origin.X) / scale, ly = (p.Y - origin.Y) / scale;
            double x = (lx - cd.AxisX) / cd.Beilv, y = (cd.AxisY - ly) / cd.Beilv;
            return (x * JwFileConsts.JwScale, y * JwFileConsts.JwScale);
        }

        // ───────── 描画 ─────────

        /// <summary>図の座標系で描く重ね表示（要確認の枠・ホバー・強調）。</summary>
        private void DrawWorldOverlays(Graphics g)
        {
            if (_bounds == null || !_showBeams) return;
            float px = 1f / Math.Max(scale, 0.01f);
            var st = g.Save();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (_showNeedCheck && IsNeedCheck != null)
            {
                using var pen = new Pen(Color.FromArgb(255, 70, 70), 1.6f * px) { DashStyle = DashStyle.Dash };
                foreach (var d in _bounds)
                    if (d.ShapeType == DrawShapeType.Beam && d.JwSquareBase is JwBeam b && IsNeedCheck(b))
                    {
                        var r = RectangleF.Inflate(Box(d), 4 * px, 4 * px);
                        g.DrawRectangle(pen, r.X, r.Y, r.Width, r.Height);
                    }
            }
            if (_hover?.DrawPoints?.Count > 2)
            {
                using var pen = new Pen(Color.FromArgb(120, 200, 255), 3f * px);
                g.DrawPolygon(pen, _hover.DrawPoints.ToArray());
            }
            if (_focus?.DrawPoints?.Count > 2 && _focusBlink)
            {
                using var pen = new Pen(Color.FromArgb(255, 170, 0), 3.5f * px);
                var r = RectangleF.Inflate(Box(_focus), 7 * px, 7 * px);
                g.DrawRectangle(pen, r.X, r.Y, r.Width, r.Height);
            }
            g.Restore(st);
        }

        /// <summary>画面座標で描く重ね表示（工区の凡例・情報カード）。</summary>
        private void DrawScreenOverlays(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (_showcolor) DrawGongquLegend(g);
            if (_hover?.JwSquareBase is JwBeam beam && !_dragging) DrawInfoCard(g, beam, _cursor);
        }

        /// <summary>使っている工区だけ、色と本数を右上に表示する（原来固定显示 A～Z 全部）。</summary>
        private void DrawGongquLegend(Graphics g)
        {
            if (_bounds == null) return;
            var used = _bounds.Where(d => d.ShapeType == DrawShapeType.Beam && d.JwSquareBase is JwBeam)
                              .Select(d => (JwBeam)d.JwSquareBase)
                              .Distinct()
                              .GroupBy(b => string.IsNullOrEmpty(b.GongQu) ? "" : b.GongQu)
                              .OrderBy(g2 => g2.Key == "" ? "~" : g2.Key, StringComparer.Ordinal)
                              .ToList();
            if (used.Count == 0) return;
            using var font = new Font(Font.FontFamily, 10f, FontStyle.Regular);
            using var bold = new Font(Font.FontFamily, 10f, FontStyle.Bold);
            int rowH = (int)Math.Ceiling(g.MeasureString("工区", font).Height) + 2, box = rowH - 6;
            string Label(IGrouping<string, JwBeam> grp) => $"{(grp.Key == "" ? "未設定" : grp.Key)}　{grp.Count()}本";
            // 幅は文字に合わせる（表示倍率が高い環境でも切れない）
            int w = (int)Math.Ceiling(used.Max(grp => g.MeasureString(Label(grp), font).Width)) + box + 28, h = 10 + rowH * (used.Count + 1);
            var card = new Rectangle(Width - w - 12, 12, w, h);
            using (var bg = new SolidBrush(Color.FromArgb(190, 30, 30, 30))) g.FillRectangle(bg, card);
            using (var border = new Pen(Color.FromArgb(90, 255, 255, 255))) g.DrawRectangle(border, card);
            g.DrawString("工区", bold, Brushes.White, card.X + 8, card.Y + 5);
            int y = card.Y + 5 + rowH;
            foreach (var grp in used)
            {
                var c = grp.Key == "" ? Color.White : JwShapeHelper.GetColor(grp.Key);
                using (var b = new SolidBrush(c)) g.FillRectangle(b, card.X + 8, y + 3, box, box);
                g.DrawString(Label(grp), font, Brushes.White, card.X + 8 + box + 8, y);
                y += rowH;
            }
        }

        /// <summary>梁の情報カード（カーソルの右下。画面からはみ出す場合は反対側）。</summary>
        private void DrawInfoCard(Graphics g, JwBeam b, Point at)
        {
            int holes = HoleCount?.Invoke(b) ?? b.Holes.Count;
            string ends = $"{(b.HasStartSide ? b.StartTelosType.ToString() : "B")}–{(b.HasEndSide ? b.EndTelosType.ToString() : "B")}";
            var lines = new List<(string Text, Color Color)>
            {
                (b.BeamCode, Color.White),
                ($"長さ {b.Length:#,0}　端部 {ends}", Color.Gainsboro),
                ($"穴 {holes}　工区 {(string.IsNullOrEmpty(b.GongQu) ? "未設定" : b.GongQu)}　CSV {(b.HasCsv ? "○" : "対象外")}", Color.Gainsboro),
            };
            if (IsNeedCheck?.Invoke(b) == true) lines.Add(("要確認（穴なし）", Color.FromArgb(255, 110, 110)));
            lines.Add(("クリックで梁画面", Color.Gray));

            using var font = new Font(Font.FontFamily, 10f, FontStyle.Regular);
            using var bold = new Font(Font.FontFamily, 11f, FontStyle.Bold);
            float w = 0, h = 8;
            var sizes = lines.Select((l, i) => g.MeasureString(l.Text, i == 0 ? bold : font)).ToList();
            foreach (var s in sizes) { w = Math.Max(w, s.Width); h += s.Height; }
            w += 16;
            float x = at.X + 16, y = at.Y + 18;
            if (x + w > Width - 4) x = at.X - w - 12;
            if (y + h > Height - 4) y = at.Y - h - 12;
            var rect = new RectangleF(x, y, w, h);
            using (var bg = new SolidBrush(Color.FromArgb(225, 25, 25, 25))) g.FillRectangle(bg, rect);
            using (var border = new Pen(Color.FromArgb(120, 200, 255))) g.DrawRectangle(border, rect.X, rect.Y, rect.Width, rect.Height);
            float ty = y + 4;
            for (int i = 0; i < lines.Count; i++)
            {
                using var brush = new SolidBrush(lines[i].Color);
                g.DrawString(lines[i].Text, i == 0 ? bold : font, brush, x + 8, ty);
                ty += sizes[i].Height;
            }
        }
    }
}
