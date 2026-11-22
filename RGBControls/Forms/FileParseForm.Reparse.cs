namespace RGBJWMain.Forms
{
    /// <summary>
    /// 2026年11月22日 「再解析」：この窓のまま解析設定を開き、確定したら同じ図面を解析し直す。
    /// 解析は最初の読み込みと全く同じ処理（ParseAsync → JwFileHandle の ReadJwFile / FollowTheStep / BuildCanvas）で、
    /// 窓を閉じて同じファイルを開き直すのと結果は同じ。設定画面は同じ JwProjectPathModel に書き戻す。
    /// </summary>
    public partial class FileParseForm
    {
        private Sunny.UI.UISymbolButton? _reparseButton;
        private int _initialLogHeight = -1;

        /// <summary>ツールバー（キャンセルの右）に「再解析」ボタンを置く。</summary>
        private void InitReparseButton()
        {
            if (_reparseButton != null) return;
            _reparseButton = new Sunny.UI.UISymbolButton
            {
                Text = "再解析",
                Symbol = 61473, // 更新（循環矢印）
                Font = uiSymbolButton2.Font,
                Size = uiSymbolButton2.Size,
                Location = new Point(uiSymbolButton2.Right + (uiSymbolButton2.Left - uiSymbolButton1.Right), uiSymbolButton2.Top),
                MinimumSize = uiSymbolButton2.MinimumSize,
                Enabled = false,
            };
            new ToolTip().SetToolTip(_reparseButton, "解析設定を変更して、同じ図面をもう一度解析します");
            _reparseButton.Click += async (s, e) => await ReparseAsync();
            uiSymbolButton2.Parent.Controls.Add(_reparseButton);
        }

        private void SetReparseEnabled(bool enabled)
        {
            if (_reparseButton != null) _reparseButton.Enabled = enabled;
        }

        /// <summary>進捗カード（失敗・データなし時）に置くボタン。</summary>
        private Button MakeReparseCardButton()
        {
            var b = new Button { Text = "設定を変更して再解析", AutoSize = true, Padding = new Padding(6, 2, 6, 2) };
            b.Click += async (s, e) => await ReparseAsync();
            return b;
        }

        private async Task ReparseAsync()
        {
            if (_parsing || jwProjectPathModel == null) return;
            using (var dlg = new CustomerDesignSetting(jwProjectPathModel))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
            }
            JwLogging.AppLog.Info("再解析 {File}", Path.GetFileName(_path));
            ResetForReparse();
            uiTextBox1.AppendText(Environment.NewLine + "──────── 再解析 ────────" + Environment.NewLine);
            await ParseAsync();
        }

        /// <summary>前回の進捗カード・左側の結果を片付け、最初に開いたときの状態へ戻す。</summary>
        private void ResetForReparse()
        {
            if (_progressHost != null)
            {
                _progressHost.Parent?.Controls.Remove(_progressHost);
                _progressHost.Dispose();
                _progressHost = null;
            }
            foreach (var c in _leftResultControls)
            {
                c.Parent?.Controls.Remove(c);
                c.Dispose();
            }
            _leftResultControls.Clear();
            uiDataGridView1.Visible = true;
            if (_initialLogHeight > 0) uiTextBox1.Height = _initialLogHeight;
            _progressList = null;
            _parseError = null;
        }
    }
}
