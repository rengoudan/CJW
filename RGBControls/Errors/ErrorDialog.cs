using System.Runtime.InteropServices;
using System.Text;
using JwLogging;

namespace RGBControls.Errors
{
    /// <summary>
    /// 错误对话框：简短说明 + 可展开的详细信息 + 复制 / 打开日志目录 / 保存支持用日志。
    /// 纯代码构建，不依赖 Designer 文件。
    /// </summary>
    internal sealed class ErrorDialog : Form
    {
        private const int CollapsedHeight = 230;
        private const int ExpandedHeight = 520;

        private readonly string _details;
        private readonly TextBox _detailBox;
        private readonly Button _toggleButton;

        public ErrorDialog(Exception ex, string? userMessage)
        {
            _details = BuildDetails(ex, userMessage);

            Text = "エラー";
            Font = SystemFonts.MessageBoxFont ?? Font;
            AutoScaleMode = AutoScaleMode.Dpi;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(520, CollapsedHeight);
            Size = new Size(640, CollapsedHeight);

            var icon = new PictureBox
            {
                Image = SystemIcons.Error.ToBitmap(),
                SizeMode = PictureBoxSizeMode.AutoSize,
                Margin = new Padding(12, 14, 8, 0),
            };

            var message = new Label
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Margin = new Padding(4, 14, 12, 4),
                Text = (userMessage ?? "予期しないエラーが発生しました。") + "\n\n" +
                       "直前の操作は完了していない可能性があります。作業内容を確認してください。\n" +
                       "問題が続く場合は「サポート用に保存」でログを保存し、担当者へ送付してください。",
            };

            var summary = new Label
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                ForeColor = SystemColors.GrayText,
                Margin = new Padding(4, 0, 12, 4),
                Text = $"{ex.GetType().Name}: {Truncate(ex.Message, 200)}",
            };

            _detailBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericMonospace, 9f),
                Text = _details,
                Visible = false,
                Margin = new Padding(12, 4, 12, 4),
            };

            _toggleButton = MakeButton("詳細を表示 ▼", (s, e) => ToggleDetails());
            var copyButton = MakeButton("コピー", (s, e) => CopyDetails());
            var openButton = MakeButton("ログフォルダを開く", (s, e) => SupportTools.OpenLogFolder(this));
            var saveButton = MakeButton("サポート用に保存", (s, e) => SupportTools.SaveSupportPackage(this, _details));
            var closeButton = MakeButton("閉じる", (s, e) => Close());
            AcceptButton = closeButton;
            CancelButton = closeButton;

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(8, 4, 8, 8),
                WrapContents = false,
            };
            buttons.Controls.AddRange(new Control[] { closeButton, saveButton, openButton, copyButton, _toggleButton });

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            layout.Controls.Add(icon, 0, 0);
            layout.Controls.Add(message, 1, 0);
            layout.Controls.Add(summary, 1, 1);
            layout.Controls.Add(_detailBox, 0, 2);
            layout.SetColumnSpan(_detailBox, 2);
            layout.Controls.Add(buttons, 0, 3);
            layout.SetColumnSpan(buttons, 2);

            Controls.Add(layout);
        }

        private static Button MakeButton(string text, EventHandler onClick)
        {
            var b = new Button { Text = text, AutoSize = true, Padding = new Padding(6, 2, 6, 2), Margin = new Padding(4) };
            b.Click += onClick;
            return b;
        }

        private void ToggleDetails()
        {
            _detailBox.Visible = !_detailBox.Visible;
            _toggleButton.Text = _detailBox.Visible ? "詳細を隠す ▲" : "詳細を表示 ▼";
            Height = _detailBox.Visible ? Math.Max(Height, ExpandedHeight) : CollapsedHeight;
        }

        private void CopyDetails()
        {
            try
            {
                Clipboard.SetText(_details);
                MessageBox.Show(this, "エラー内容をクリップボードにコピーしました。", "コピー",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppLog.Warn(ex, "クリップボードへのコピーに失敗");
            }
        }

        private static string BuildDetails(Exception ex, string? userMessage)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"日時      : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"バージョン: {typeof(ErrorDialog).Assembly.GetName().Version}");
            sb.AppendLine($"OS        : {RuntimeInformation.OSDescription}");
            sb.AppendLine($"ランタイム: {RuntimeInformation.FrameworkDescription}");
            if (!string.IsNullOrEmpty(userMessage)) sb.AppendLine($"内容      : {userMessage}");
            sb.AppendLine($"ログ      : {AppLog.LogDir}");
            sb.AppendLine(new string('-', 60));
            sb.AppendLine(ex.ToString());
            return sb.ToString();
        }

        private static string Truncate(string s, int max) => s.Length <= max ? s : s[..max] + "…";
    }
}
