using JwCore;

namespace RGBJWMain.Forms
{
    /// <summary>
    /// 2026年11月23日 G 端の上下フランジ穴の位置（29／35）を、選んだ時点で反映する（✓ ボタン不要）。
    /// 原来：选择后要按 ✓（button1）；而且从解析窗口打开时（IsNewBeam＝true）✓ 的处理是空的，切换完全不生效。
    /// 現在：切り替えると 穴の再計算（holeorder）→ 2D 図・CSV・3D を描き直す。
    ///   ・保存済みの梁：DB にも保存（従来の ✓ と同じ UpdateBeamBFDistance）。
    ///   ・解析直後の梁：画面上の梁に反映し、解析画面の「保存」で一緒に保存される（ToDbData が値を写す）。
    /// 穴の計算そのもの（JwBeam.holeorder の 29/35 の扱い）は変えていない。
    /// </summary>
    public partial class NewJwBeamForm
    {
        private bool _gDistanceSyncing;
        private bool _gDistanceHooked;

        /// <summary>NewJwBeamForm_Load の最後に呼ぶ（初期値を設定した後でイベントを付ける）。</summary>
        private void InitGDistanceSwitch()
        {
            button1.Visible = false; // 選んだ時点で反映するので ✓ は不要
            new ToolTip().SetToolTip(uiComboBox1, "G 端の上下フランジ穴の位置（先端からの距離）。選ぶとすぐに 2D 図・CSV・3D に反映します");
            if (_gDistanceHooked) return;
            _gDistanceHooked = true;
            uiComboBox1.SelectedIndexChanged += async (s, e) => await OnGDistanceChangedAsync();
        }

        /// <summary>表示中の梁に合わせて 29/35 の表示を切り替える（梁の切り替え時。反映処理は走らせない）。</summary>
        private void SyncGDistanceUi()
        {
            _gDistanceSyncing = true;
            try
            {
                bool show = _jwbeam != null && _jwbeam.HasBFG;
                uiComboBox1.Visible = show;
                button1.Visible = false;
                if (!show) return;
                if (uiComboBox1.DataSource == null) uiComboBox1.DataSource = Enum.GetValues(typeof(BaiFangGTBDistanceType));
                uiComboBox1.SelectedItem = _jwbeam!.BaiFangGTBDistance;
            }
            finally { _gDistanceSyncing = false; }
        }

        private async Task OnGDistanceChangedAsync()
        {
            if (_gDistanceSyncing || _jwbeam == null || !_jwbeam.HasBFG) return;
            if (uiComboBox1.SelectedItem is not BaiFangGTBDistanceType value || value == _jwbeam.BaiFangGTBDistance) return;

            _jwbeam.BaiFangGTBDistance = value;
            _jwbeam.holeorder();   // 穴の再計算（29 なら G 端の上下フランジ穴を 6mm 端寄りに）
            RefreshBeamViews();    // 2D 図・CSV・3D
            JwLogging.AppLog.Info("G 端の穴位置を変更 {Beam} → {Value}（{Mode}）", _jwbeam.BeamCode, value, IsNewBeam ? "解析中・保存時に反映" : "保存済み");

            if (!IsNewBeam)
            {
                try
                {
                    await _jwProjectMainService.UpdateBeamBFDistance(_jwbeam.Id, value);
                }
                catch (Exception ex)
                {
                    RGBControls.Errors.ErrorReporter.Report(ex, "G 端の穴位置を保存できませんでした。");
                }
            }
        }
    }
}
