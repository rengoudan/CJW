using System.Diagnostics;
using JwShapeCommon;
using RGBControls.Errors;

namespace RGBJWMain.Forms
{
    /// <summary>
    /// 2026年11月22日 jww 読み込み・解析の進捗カード（画面領域に表示、完了後は 1 行の要約に折りたたむ）。
    /// コードのみで構築し、Designer は変更しない。
    /// </summary>
    public partial class FileParseForm
    {
        private static readonly Color RunningColor = Color.FromArgb(0x15, 0x65, 0xC0);
        private static readonly Color DoneColor = Color.FromArgb(0x2E, 0x7D, 0x32);
        private static readonly Color WarnColor = Color.FromArgb(0xE6, 0x7E, 0x00);
        private static readonly Color FailColor = Color.FromArgb(0xC6, 0x28, 0x28);

        private Panel? _progressHost;
        private Label? _progressTitle;
        private Label? _progressElapsed;
        private Label? _progressCurrent;
        private ProgressBar? _progressBar;
        private ListView? _progressList;
        private FlowLayoutPanel? _progressButtons;
        private readonly List<Control> _leftResultControls = new(); // 左側に移した結果（再解析時に片付ける）
        private readonly Stopwatch _parseWatch = new();
        private System.Windows.Forms.Timer? _elapsedTimer;
        private Exception? _parseError;

        /// <summary>進捗カードを表示し、全ステップを「待機」で並べる。</summary>
        private void BeginProgress(string fileName)
        {
            var font = SystemFonts.MessageBoxFont ?? Font;
            var host = uiSplitContainer1.Panel2;

            _progressTitle = new Label { Dock = DockStyle.Top, Height = 34, Font = new Font(font.FontFamily, 12f, FontStyle.Bold), Padding = new Padding(12, 8, 0, 0), Text = $"図面解析中　{fileName}" };
            _progressElapsed = new Label { Dock = DockStyle.Right, Width = 140, TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(0, 0, 12, 0), Font = font };
            _progressTitle.Controls.Add(_progressElapsed);
            _progressCurrent = new Label { Dock = DockStyle.Top, Height = 26, Padding = new Padding(12, 4, 0, 0), Font = font, ForeColor = RunningColor };
            _progressBar = new ProgressBar { Dock = DockStyle.Top, Height = 14, Maximum = JwFileHandle.ParseStepNames.Count, Style = ProgressBarStyle.Continuous };
            var barPad = new Panel { Dock = DockStyle.Top, Height = 22, Padding = new Padding(12, 0, 12, 8) };
            barPad.Controls.Add(_progressBar);

            _progressList = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                Font = font,
                BorderStyle = BorderStyle.None,
            };
            _progressList.Columns.Add("状態", 90);
            _progressList.Columns.Add("処理", 190);
            _progressList.Columns.Add("時間", 70, HorizontalAlignment.Right);
            _progressList.Columns.Add("結果", 420);
            foreach (var name in JwFileHandle.ParseStepNames)
                _progressList.Items.Add(new ListViewItem(new[] { "・待機", name, "", "" }) { ForeColor = SystemColors.GrayText });
            var listPad = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 0, 12, 0) };
            listPad.Controls.Add(_progressList);

            _progressButtons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 44, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8, 6, 8, 6), Visible = false };

            _progressHost = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            // Dock の順序：後から追加したもの（前面）が内側になるよう、Fill を最後に前面へ
            _progressHost.Controls.Add(listPad);
            _progressHost.Controls.Add(_progressButtons);
            _progressHost.Controls.Add(barPad);
            _progressHost.Controls.Add(_progressCurrent);
            _progressHost.Controls.Add(_progressTitle);
            listPad.BringToFront();
            host.Controls.Add(_progressHost);
            _progressHost.BringToFront(); // 空のキャンバスの上に重ねる

            _parseWatch.Restart();
            _elapsedTimer = new System.Windows.Forms.Timer { Interval = 100 };
            _elapsedTimer.Tick += (s, e) => UpdateElapsed();
            _elapsedTimer.Start();
            UpdateElapsed();
        }

        private void UpdateElapsed()
        {
            if (_progressElapsed != null) _progressElapsed.Text = $"経過 {_parseWatch.Elapsed:mm\\:ss\\.f}";
        }

        /// <summary>JwFileHandle からの進捗通知（UI スレッドで呼ばれる）。</summary>
        private void OnParseProgress(ParseProgress p)
        {
            if (_progressList == null || IsDisposed) return;
            if (p.Index < 0 || p.Index >= _progressList.Items.Count) return;
            var item = _progressList.Items[p.Index];
            (string text, Color color) = p.Status switch
            {
                ParseStepStatus.Running => ("▶ 実行中", RunningColor),
                ParseStepStatus.Done => ("✓ 完了", DoneColor),
                ParseStepStatus.Warning => ("⚠ 注意", WarnColor),
                _ => ("✗ 失敗", FailColor),
            };
            item.SubItems[0].Text = text;
            item.SubItems[2].Text = p.Status == ParseStepStatus.Running ? "" : FormatMs(p.ElapsedMs);
            item.SubItems[3].Text = p.Summary ?? "";
            item.ForeColor = color;
            item.EnsureVisible();

            int finished = _progressList.Items.Cast<ListViewItem>().Count(i => i.ForeColor != SystemColors.GrayText && i.ForeColor != RunningColor);
            if (_progressBar != null) _progressBar.Value = Math.Min(finished, _progressBar.Maximum);
            if (_progressCurrent != null && p.Status == ParseStepStatus.Running)
                _progressCurrent.Text = $"{p.Index + 1} / {p.Total}　{p.StepName}";
        }

        private static string FormatMs(long ms) => ms < 1000 ? $"{ms} ms" : $"{ms / 1000.0:0.0} s";

        private void StopElapsed()
        {
            _parseWatch.Stop();
            _elapsedTimer?.Stop();
            _elapsedTimer?.Dispose();
            _elapsedTimer = null;
            UpdateElapsed();
        }

        private int WarningCount() =>
            _progressList?.Items.Cast<ListViewItem>().Count(i => i.ForeColor == WarnColor) ?? 0;

        /// <summary>成功：カードを片付けてキャンバスを表示し、結果は左側に移す。</summary>
        private void EndProgressSucceeded(JwFileHandle h)
        {
            StopElapsed();
            int warn = WarningCount();
            var summary = $"解析完了 {FormatMs(_parseWatch.ElapsedMilliseconds)}　梁 {h.Beams?.Count ?? 0}本・柱 {h.Pillars?.Count ?? 0}本・連結 {h.LianjieSingles?.Count ?? 0}件" +
                          (warn > 0 ? $"　⚠ 注意 {warn}件" : "");
            MoveResultToLeft(summary.Replace("　梁", "\n梁"), warn > 0 ? WarnColor : DoneColor);
        }

        /// <summary>解析は終わったが有効なデータが無い：カードを開いたまま（保存不可）。</summary>
        private void EndProgressNoData()
        {
            StopElapsed();
            SetHeader("現在の解析設定では有効なデータが検出されません。色や線種の設定を確認してください。", WarnColor);
        }

        /// <summary>失敗：失敗したステップは赤表示、未実行のステップを明示し、カードを開いたまま。</summary>
        private void EndProgressFailed(Exception ex)
        {
            StopElapsed();
            _parseError = ex;
            if (_progressList != null)
                foreach (ListViewItem i in _progressList.Items)
                    if (i.ForeColor == SystemColors.GrayText || i.ForeColor == RunningColor)
                    {
                        if (i.ForeColor == RunningColor) { i.SubItems[0].Text = "✗ 失敗"; i.ForeColor = FailColor; }
                        else i.SubItems[0].Text = "－ 未実行";
                    }
            var step = (ex as ParseStepException)?.StepName;
            SetHeader(step != null ? $"解析に失敗しました（{step}）: {ex.InnerException?.Message ?? ex.Message}"
                                   : $"解析に失敗しました: {ex.Message}", FailColor);

            var detail = new Button { Text = "エラー詳細", AutoSize = true, Padding = new Padding(6, 2, 6, 2) };
            detail.Click += (s, e) =>
            {
                var msg = (_parseError as ParseStepException)?.UserMessage ?? "図面解析中にエラーが発生しました。";
                using var dlg = new ErrorDialog(_parseError!, msg + "\n" + Path.GetFileName(_path));
                dlg.ShowDialog(this);
            };
            _progressButtons!.Controls.Add(detail);
            _progressButtons.Visible = true;
        }

        private void SetHeader(string text, Color color)
        {
            if (_progressCurrent == null || _progressTitle == null) return;
            _progressTitle.Text = text.StartsWith("解析完了") ? "図面解析　完了" : "図面解析";
            _progressCurrent.Text = text;
            _progressCurrent.ForeColor = color;
        }

        /// <summary>
        /// 2026年11月22日 完了後：画面上のカードを片付け、解析結果（要約＋各ステップ）を左側の下段に移す。
        /// 左下の uiDataGridView1 はコードから使われておらず常に空だったため、その場所を使う（グリッドは非表示にするだけ）。
        /// 左側は幅が狭いので「状態（記号）・処理・結果」の 3 列にし、結果の全文と所要時間はツールチップで表示する。
        /// </summary>
        private void MoveResultToLeft(string summary, Color color)
        {
            if (_progressHost != null)
            {
                _progressHost.Visible = false;
                uiSplitContainer1.Panel2.Controls.Remove(_progressHost);
            }
            if (_progressList == null) return;

            var font = SystemFonts.MessageBoxFont ?? Font;
            var summaryLabel = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 44,
                Padding = new Padding(4, 4, 4, 0),
                Font = new Font(font.FontFamily, font.Size, FontStyle.Bold),
                ForeColor = color,
                Text = summary,
            };
            var list = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                ShowItemToolTips = true,
                Font = font,
                BorderStyle = BorderStyle.None,
            };
            list.Columns.Add("", 26);
            list.Columns.Add("処理", 112);
            list.Columns.Add("結果", 200);
            foreach (ListViewItem src in _progressList.Items)
            {
                string status = src.SubItems[0].Text;           // 「✓ 完了」など → 先頭の記号だけ
                string mark = status.Length > 0 ? status.Substring(0, 1) : "";
                string name = src.SubItems[1].Text, time = src.SubItems[2].Text, result = src.SubItems[3].Text;
                list.Items.Add(new ListViewItem(new[] { mark, name, result })
                {
                    ForeColor = src.ForeColor,
                    ToolTipText = $"{status}　{name}" + (time != "" ? $"　{time}" : "") + (result != "" ? $"\n{result}" : ""),
                });
            }

            uiDataGridView1.Visible = false;
            panel3.Controls.Add(list);
            panel3.Controls.Add(summaryLabel);
            _leftResultControls.Add(list);
            _leftResultControls.Add(summaryLabel);
            list.BringToFront(); // Dock=Top の要約を先に確定させ、一覧は残りに Fill

            // 1080p で最大化したとき、上のログ欄（設計時 650px 固定）が大きすぎて一覧がスクロールになるため、
            // 一覧が収まる高さまでログ欄を縮める（ログ欄は最低 150px 残す）
            int rowH = (list.Items.Count > 0 && list.IsHandleCreated) ? list.GetItemRect(0).Height : font.Height + 6;
            int wanted = summaryLabel.Height + rowH * (list.Items.Count + 1) + 12;
            int avail = uiSplitContainer1.Panel1.ClientSize.Height;
            int logH = Math.Max(150, Math.Min(uiTextBox1.Height, avail - wanted));
            if (logH < uiTextBox1.Height) uiTextBox1.Height = logH;

            // 「処理」列は内容に合わせ、「結果」列は残りの幅いっぱいにする（パネルの幅が変わったら追従）
            list.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
            void FitResultColumn() =>
                list.Columns[2].Width = Math.Max(60, list.ClientSize.Width - list.Columns[0].Width - list.Columns[1].Width - 4);
            FitResultColumn();
            list.ClientSizeChanged += (s, e) => FitResultColumn();
            _progressList = list; // 以後の参照（注意件数など）はこちらを使う
        }
    }
}
