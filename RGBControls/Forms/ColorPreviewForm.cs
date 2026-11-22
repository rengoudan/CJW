using System.Drawing;
using System.Drawing.Drawing2D;
using RGBControls.Classes;

namespace RGBControls.Forms
{
    /// <summary>
    /// 2026年11月22日 解析前の「色分布の確認」画面。
    /// 左上：解析設定の各色に、図面上のデータが何件あるか（0 件は ⚠）。
    /// 左下：線色ごとの件数と、その色を使っている設定項目。ここで選んだ線色を設定項目に割り当てられる。
    /// 右　：図面全体の縮図。選択中の線色を赤で強調表示する。
    /// 「設定に反映」で呼び出し元（解析設定画面）の選択だけを変える。保存はしない（保存は解析設定画面の「保存」）。
    /// 図面の読み取り・集計は読み取り専用で、解析処理には関与しない。
    /// </summary>
    public sealed class ColorPreviewForm : Form
    {
        public sealed record Assignment(string Setting, int Color);

        /// <summary>解析設定の線色コンボで選べる範囲（lc0〜lc14）。</summary>
        public const int MinAssignableColor = 0, MaxAssignableColor = 14;

        private readonly JwwColorStats _stats;
        private readonly List<Assignment> _assignments;
        private readonly Dictionary<string, int> _original;
        private readonly Dictionary<int, JwwColorStats.ColorRow> _rows;
        private readonly ListView _settingList;
        private readonly ListView _colorList;
        private readonly PreviewPanel _preview;
        private readonly ComboBox _targetBox;
        private readonly Label _pickedLabel;
        private readonly Button _assignButton;
        private readonly Button _applyButton;
        private readonly Label _status;
        private readonly Font _boldFont;

        /// <summary>元の値から変わった割り当て（「設定に反映」で閉じたときに呼び出し元が使う）。</summary>
        public IReadOnlyList<Assignment> Changes =>
            _assignments.Where(a => _original.TryGetValue(a.Setting, out var o) && o != a.Color).ToList();

        /// <param name="allowAssign">false なら参照のみ（割り当て・反映の操作を出さない。設定画面以外から開くとき）</param>
        public ColorPreviewForm(JwwColorStats stats, string fileName, IEnumerable<Assignment> assignments, bool allowAssign = true)
        {
            _stats = stats;
            _assignments = assignments.ToList();
            _original = _assignments.ToDictionary(a => a.Setting, a => a.Color);
            var byColor = stats.ByColor();
            _rows = byColor.ToDictionary(r => r.Color);

            Text = $"色分布の確認　{fileName}";
            StartPosition = FormStartPosition.CenterParent;
            float k = DeviceDpi / 96f;
            Size = new Size((int)(1180 * k), (int)(780 * k)); // 100% では 1180×780（1080p に収まる）
            MinimumSize = new Size((int)(900 * k), (int)(600 * k));
            ShowInTaskbar = false;
            MinimizeBox = false;
            Font = new Font("Microsoft Sans Serif", 10.5f); // UiFont が日本語 UI フォントへ置き換える
            _boldFont = new Font(Font, FontStyle.Bold);

            var header = new Label
            {
                Dock = DockStyle.Top,
                Height = (int)(56 * k),
                Padding = new Padding(10, 8, 0, 0),
                Text = $"縮尺 1/{stats.Scale}　線 {stats.Lines.Count:N0}・ソリッド {stats.Solids.Count:N0}・文字 {stats.Texts.Count:N0}（ブロック {stats.BlockCount:N0} 個を展開済み）"
                       + Environment.NewLine + (allowAssign
                           ? "上の一覧で設定項目を選び、下の一覧で線色を選んで「割り当て」（ダブルクリック・右クリックでも可）。"
                           : "一覧で色を選ぶと、右の図で赤く表示します（参照のみ。色の変更は解析設定画面で行います）。"),
            };

            // ── 設定項目の一覧 ──
            _settingList = MakeList(("設定項目", 150), ("線色", 110), ("図面上の件数", 220));
            for (int i = 0; i < _assignments.Count; i++)
            {
                var item = new ListViewItem(new[] { "", "", "" }) { Tag = i };
                _settingList.Items.Add(item);
            }
            var settingMenu = new ContextMenuStrip();
            var revert = settingMenu.Items.Add("元に戻す", null, (s, e) => RevertSelected());
            settingMenu.Opening += (s, e) =>
            {
                int i = SelectedSettingIndex();
                revert.Enabled = i >= 0 && _assignments[i].Color != _original[_assignments[i].Setting];
            };
            _settingList.ContextMenuStrip = settingMenu;

            // ── 線色の一覧 ──
            _colorList = MakeList(("線色", 60), ("線", 64), ("ソリッド", 84), ("文字", 60), ("ブロック内", 96), ("設定での用途", 200));
            foreach (var r in byColor)
            {
                var item = new ListViewItem(new[]
                {
                    ColorName(r.Color), r.Lines.ToString("N0"), r.Solids.ToString("N0"), r.Texts.ToString("N0"),
                    r.BlockItems == 0 ? "" : r.BlockItems.ToString("N0"), "",
                }) { Tag = r.Color };
                if (!IsAssignable(r.Color)) item.ForeColor = Color.Gray;
                _colorList.Items.Add(item);
            }
            for (int i = 1; i <= 4; i++) _colorList.Columns[i].TextAlign = HorizontalAlignment.Right;
            var colorMenu = new ContextMenuStrip();
            colorMenu.Opening += (s, e) =>
            {
                colorMenu.Items.Clear();
                int c = SelectedColor();
                if (c == int.MinValue) { e.Cancel = true; return; }
                if (!IsAssignable(c))
                {
                    colorMenu.Items.Add($"{ColorName(c)} は解析設定で選べない線色です").Enabled = false;
                    return;
                }
                for (int i = 0; i < _assignments.Count; i++)
                {
                    int idx = i;
                    var mi = colorMenu.Items.Add($"{ColorName(c)} を「{_assignments[i].Setting}」に割り当て", null, (s2, e2) => Assign(idx, c));
                    mi.Enabled = _assignments[i].Color != c;
                }
            };
            if (allowAssign)
            {
                _colorList.ContextMenuStrip = colorMenu;
                _colorList.DoubleClick += (s, e) => AssignToTarget();
            }

            _settingList.SelectedIndexChanged += (s, e) => OnSettingSelected();
            _colorList.SelectedIndexChanged += (s, e) => OnColorSelected();

            foreach (var lv in new[] { _settingList, _colorList })
                foreach (ColumnHeader col in lv.Columns) col.Width = (int)(col.Width * k);

            // ── 割り当てバー（線色一覧の下）──
            _pickedLabel = new Label { AutoSize = true, Margin = new Padding(0, 7, 4, 0), Font = _boldFont, Text = "線色：－" };
            var mid = new Label { AutoSize = true, Margin = new Padding(0, 7, 4, 0), Text = "を" };
            _targetBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = (int)(150 * k), Margin = new Padding(0, 3, 4, 0) };
            foreach (var a in _assignments) _targetBox.Items.Add(a.Setting);
            var tail = new Label { AutoSize = true, Margin = new Padding(0, 7, 8, 0), Text = "に" };
            _assignButton = new Button { Text = "割り当て", AutoSize = true, Padding = new Padding(10, 0, 10, 0), Margin = new Padding(0, 2, 0, 0) };
            _assignButton.Click += (s, e) => AssignToTarget();
            _targetBox.SelectedIndexChanged += (s, e) => UpdateAssignButton();
            var bar = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = (int)(40 * k), Padding = new Padding(4, 4, 0, 0), WrapContents = false };
            bar.Controls.AddRange(new Control[] { _pickedLabel, mid, _targetBox, tail, _assignButton });

            int rowH = TextRenderer.MeasureText("梁", _settingList.Font).Height + (int)(8 * k);
            int boxH = (int)(40 * k) + rowH * (Math.Max(_settingList.Items.Count, 1) + 1);
            var settingBox = new GroupBox { Text = "解析設定の色（現在の選択）", Dock = DockStyle.Top, Height = boxH, Padding = new Padding(6) };
            settingBox.Controls.Add(_settingList);
            var colorBox = new GroupBox { Text = "線色ごとの件数", Dock = DockStyle.Fill, Padding = new Padding(6) };
            colorBox.Controls.Add(_colorList);
            if (allowAssign) colorBox.Controls.Add(bar);
            else _settingList.ContextMenuStrip = null;
            foreach (var lv in new[] { _settingList, _colorList })
                lv.ClientSizeChanged += (s, e) => FillLastColumn((ListView)s!);
            var left = new Panel { Dock = DockStyle.Left, Width = (int)(600 * k), Padding = new Padding(8, 0, 4, 0) };
            left.Controls.Add(colorBox);
            left.Controls.Add(settingBox);

            _preview = new PreviewPanel(stats) { Dock = DockStyle.Fill };
            var legend = new Label { Dock = DockStyle.Bottom, Height = (int)(26 * k), Text = "赤：選択中の線色　灰：その他の線色", Padding = new Padding(4, 4, 0, 0) };
            new ToolTip().SetToolTip(legend, "ソリッドは塗りつぶし、文字は点で表示します");
            var right = new Panel { Dock = DockStyle.Fill, Padding = new Padding(4, 0, 8, 0) };
            right.Controls.Add(_preview);
            right.Controls.Add(legend);

            // ── 下部：状態表示と「設定に反映」「キャンセル」──
            _status = new Label { AutoSize = false, Location = new Point(12, (int)(12 * k)), Height = (int)(24 * k), ForeColor = Color.RoyalBlue };
            _applyButton = new Button { Text = "設定に反映", AutoSize = true, Padding = new Padding(16, 2, 16, 2), Enabled = false };
            _applyButton.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };
            var cancel = new Button { Text = allowAssign ? "キャンセル" : "閉じる", AutoSize = true, Padding = new Padding(16, 2, 16, 2) };
            _applyButton.Visible = allowAssign;
            cancel.Click += (s, e) => Close();
            var bottom = new Panel { Dock = DockStyle.Bottom, Height = (int)(48 * k) };
            bottom.Controls.AddRange(new Control[] { _status, _applyButton, cancel });
            bottom.Layout += (s, e) =>
            {
                cancel.Location = new Point(bottom.ClientSize.Width - cancel.Width - 12, (int)(8 * k));
                _applyButton.Location = new Point(cancel.Left - _applyButton.Width - 8, cancel.Top);
                _status.Width = Math.Max(10, _applyButton.Left - _status.Left - 8);
            };
            CancelButton = cancel;

            Controls.Add(right);
            Controls.Add(left);
            Controls.Add(bottom);
            Controls.Add(header);

            RefreshAll();
            // 最初は先頭（梁輪郭線）を選択しておく
            if (_settingList.Items.Count > 0) _settingList.Items[0].Selected = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FormLayoutHelper.FitToWorkingArea(this);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK && Changes.Count > 0 &&
                MessageBox.Show(this, "割り当ての変更を破棄して閉じますか？", "確認",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _boldFont.Dispose();
            base.Dispose(disposing);
        }

        public static string ColorName(int c) => c < 0 ? "未設定" : $"lc{c}";

        public static bool IsAssignable(int c) => c >= MinAssignableColor && c <= MaxAssignableColor;

        private static string Describe(JwwColorStats.ColorRow r)
        {
            var parts = new List<string>();
            if (r.Lines > 0) parts.Add($"線 {r.Lines:N0}");
            if (r.Solids > 0) parts.Add($"ソリッド {r.Solids:N0}");
            if (r.Texts > 0) parts.Add($"文字 {r.Texts:N0}");
            return string.Join("・", parts);
        }

        // ── 表示の更新 ──

        private void RefreshAll()
        {
            for (int i = 0; i < _assignments.Count; i++)
            {
                var a = _assignments[i];
                var item = _settingList.Items[i];
                int orig = _original[a.Setting];
                bool changed = orig != a.Color;
                _rows.TryGetValue(a.Color, out var r);
                int total = r?.Total ?? 0;
                item.SubItems[0].Text = a.Setting;
                item.SubItems[1].Text = changed ? $"{ColorName(orig)} → {ColorName(a.Color)}" : ColorName(a.Color);
                item.SubItems[2].Text = a.Color < 0 ? "⚠ 未設定" : total == 0 ? "⚠ この色のデータがありません" : Describe(r!);
                item.ForeColor = changed ? Color.RoyalBlue : (a.Color < 0 || total == 0) ? Color.Firebrick : SystemColors.WindowText;
                item.Font = changed ? _boldFont : _settingList.Font;
            }
            foreach (ListViewItem item in _colorList.Items)
            {
                int c = (int)item.Tag!;
                string usage = string.Join("・", _assignments.Where(a => a.Color == c).Select(a => a.Setting));
                item.SubItems[5].Text = usage;
                item.Font = usage.Length > 0 ? _boldFont : _colorList.Font;
            }
            int n = Changes.Count;
            _applyButton.Enabled = n > 0;
            _status.Text = n == 0 ? "" : $"変更 {n} 件。「設定に反映」で解析設定画面に反映されます（保存は解析設定画面の「保存」で行います）。";
            UpdateAssignButton();
        }

        private int SelectedSettingIndex() => _settingList.SelectedItems.Count == 0 ? -1 : (int)_settingList.SelectedItems[0].Tag!;

        private int SelectedColor() => _colorList.SelectedItems.Count == 0 ? int.MinValue : (int)_colorList.SelectedItems[0].Tag!;

        private void UpdateAssignButton()
        {
            int c = SelectedColor();
            int t = _targetBox.SelectedIndex;
            _pickedLabel.Text = c == int.MinValue ? "線色：－" : $"線色 {ColorName(c)}";
            _assignButton.Enabled = c != int.MinValue && IsAssignable(c) && t >= 0 && _assignments[t].Color != c;
        }

        // ── 選択 ──

        private bool _syncing;

        private void OnSettingSelected()
        {
            if (_syncing) return;
            int i = SelectedSettingIndex();
            if (i < 0) return;
            _targetBox.SelectedIndex = i; // 割り当て先をこの設定項目にする
            int color = _assignments[i].Color;
            _preview.Highlight = color;
            // 線色の一覧も同じ色に合わせる
            _syncing = true;
            try
            {
                foreach (ListViewItem it in _colorList.Items) it.Selected = false;
                var match = _colorList.Items.Cast<ListViewItem>().FirstOrDefault(it => (int)it.Tag! == color);
                if (match != null) { match.Selected = true; match.EnsureVisible(); }
            }
            finally { _syncing = false; }
            UpdateAssignButton();
        }

        private void OnColorSelected()
        {
            if (_syncing) return;
            int c = SelectedColor();
            if (c != int.MinValue) _preview.Highlight = c;
            // 割り当て先（設定項目の選択）はそのまま
            UpdateAssignButton();
        }

        // ── 割り当て ──

        private void AssignToTarget()
        {
            int t = _targetBox.SelectedIndex;
            int c = SelectedColor();
            if (t < 0 || c == int.MinValue || !IsAssignable(c)) return;
            Assign(t, c);
        }

        private void Assign(int index, int color)
        {
            if (!IsAssignable(color) || _assignments[index].Color == color) return;
            _assignments[index] = _assignments[index] with { Color = color };
            RefreshAll();
        }

        private void RevertSelected()
        {
            int i = SelectedSettingIndex();
            if (i < 0) return;
            _assignments[i] = _assignments[i] with { Color = _original[_assignments[i].Setting] };
            RefreshAll();
            _preview.Highlight = _assignments[i].Color;
        }

        private static void FillLastColumn(ListView lv)
        {
            if (lv.Columns.Count == 0) return;
            int used = 0;
            for (int i = 0; i < lv.Columns.Count - 1; i++) used += lv.Columns[i].Width;
            int rest = lv.ClientSize.Width - used - 2;
            if (rest > 60) lv.Columns[lv.Columns.Count - 1].Width = rest;
        }

        private static ListView MakeList(params (string Text, int Width)[] cols)
        {
            var lv = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                HideSelection = false,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
            };
            foreach (var (t, w) in cols) lv.Columns.Add(t, w);
            return lv;
        }

        /// <summary>図面の縮図。灰色の全体図はサイズ変更時だけ描き直してキャッシュする。</summary>
        private sealed class PreviewPanel : Panel
        {
            private readonly JwwColorStats _stats;
            private readonly RectangleF _bounds;
            private Bitmap? _base;
            private int _highlight = int.MinValue;

            public PreviewPanel(JwwColorStats stats)
            {
                _stats = stats;
                _bounds = stats.Bounds();
                DoubleBuffered = true;
                BackColor = Color.White;
                BorderStyle = BorderStyle.FixedSingle;
                ResizeRedraw = true;
            }

            public int Highlight
            {
                get => _highlight;
                set { _highlight = value; Invalidate(); }
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                _base?.Dispose();
                _base = null;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing) _base?.Dispose();
                base.Dispose(disposing);
            }

            private Matrix? MakeMatrix()
            {
                if (_bounds.IsEmpty || ClientSize.Width < 20 || ClientSize.Height < 20) return null;
                const float margin = 12;
                float w = Math.Max(_bounds.Width, 1e-3f), h = Math.Max(_bounds.Height, 1e-3f);
                float s = Math.Min((ClientSize.Width - 2 * margin) / w, (ClientSize.Height - 2 * margin) / h);
                float ox = (ClientSize.Width - w * s) / 2, oy = (ClientSize.Height - h * s) / 2;
                // jww は Y 上向き → 画面は Y 下向き
                return new Matrix(s, 0, 0, -s, ox - _bounds.Left * s, oy + _bounds.Bottom * s);
            }

            private static PointF[] Tf(Matrix m, params PointF[] pts)
            {
                var copy = (PointF[])pts.Clone();
                m.TransformPoints(copy);
                return copy;
            }

            private void DrawItems(Graphics g, Matrix m, Func<int, bool> pick, Pen pen, Brush fill, float dot)
            {
                foreach (var l in _stats.Lines)
                {
                    if (!pick(l.Color)) continue;
                    var p = Tf(m, l.A, l.B);
                    g.DrawLine(pen, p[0], p[1]);
                }
                foreach (var so in _stats.Solids)
                {
                    if (!pick(so.Color) || so.Points.Length < 3) continue;
                    var p = Tf(m, so.Points);
                    g.FillPolygon(fill, p);
                }
                if (dot > 0)
                {
                    foreach (var t in _stats.Texts)
                    {
                        if (!pick(t.Color)) continue;
                        var p = Tf(m, t.At)[0];
                        g.FillRectangle(fill, p.X - dot / 2, p.Y - dot / 2, dot, dot);
                    }
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                using var m = MakeMatrix();
                if (m == null)
                {
                    TextRenderer.DrawText(e.Graphics, "表示できる図形がありません", Font, ClientRectangle, Color.Gray);
                    return;
                }
                if (_base == null)
                {
                    _base = new Bitmap(ClientSize.Width, ClientSize.Height);
                    using var g = Graphics.FromImage(_base);
                    g.Clear(Color.White);
                    using var pen = new Pen(Color.FromArgb(200, 200, 200), 1f);
                    using var fill = new SolidBrush(Color.FromArgb(215, 215, 215));
                    DrawItems(g, m, _ => true, pen, fill, 0);
                }
                e.Graphics.DrawImageUnscaled(_base, 0, 0);
                if (_highlight != int.MinValue)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using var pen = new Pen(Color.Red, 1.6f);
                    using var fill = new SolidBrush(Color.FromArgb(220, 230, 30, 30));
                    DrawItems(e.Graphics, m, c => c == _highlight, pen, fill, 4);
                }
            }
        }
    }
}
