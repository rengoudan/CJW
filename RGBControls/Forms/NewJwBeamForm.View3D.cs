using JwBeam3D;

namespace RGBJWMain.Forms
{
    /// <summary>
    /// 2026年11月21日 梁の 3D 表示（2D 図と切り替え）。
    /// 3D は画面下部の CSV プレビュー（uiTextBox1）をそのまま解析して描画するため、
    /// 表示中の CSV・3D・加工機へ渡すデータが常に一致する。
    /// 3D コントロールは初めて「3D」を選んだときに生成する（WPF の初期化を必要時まで遅らせる）。
    /// </summary>
    public partial class NewJwBeamForm
    {
        private Beam3DControl? _beam3D;
        private RadioButton? _radio2D;
        private RadioButton? _radio3D;

        private void InitViewSwitch()
        {
            if (_radio2D != null) return;

            _radio2D = MakeViewRadio("2D 図", true);
            _radio3D = MakeViewRadio("3D 表示", false);
            _radio3D.CheckedChanged += (s, e) => ApplyViewMode();

            var strip = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(8, 4, 8, 0),
                WrapContents = false,
            };
            strip.Controls.Add(_radio2D);
            strip.Controls.Add(_radio3D);
            panel6.Controls.Add(strip);
            strip.SendToBack(); // Dock=Top を先に確定させ、残りを図に使う

            uiTextBox1.TextChanged += (s, e) => { if (_radio3D.Checked) _beam3D?.ShowCsv(uiTextBox1.Text); };
        }

        private static RadioButton MakeViewRadio(string text, bool isChecked) => new()
        {
            Text = text,
            Checked = isChecked,
            Appearance = Appearance.Button,
            AutoSize = false,
            Size = new Size(110, 32),
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(0, 0, 6, 0),
            FlatStyle = FlatStyle.Flat,
        };

        private void ApplyViewMode()
        {
            bool show3D = _radio3D?.Checked == true;
            if (show3D && _beam3D == null)
            {
                // 2D 図と同じく、CSV 1 行（縦穴数1）を EllipseSpacing ピッチの 2 穴で表示する
                _beam3D = new Beam3DControl { Dock = DockStyle.Fill, PairPitch = JwShapeCommon.JwFileConsts.EllipseSpacing };
                panel6.Controls.Add(_beam3D);
            }
            if (_beam3D != null)
            {
                _beam3D.Visible = show3D;
                if (show3D)
                {
                    _beam3D.BringToFront();
                    _beam3D.ShowCsv(uiTextBox1.Text);
                }
            }
            if (_singleBeamShow != null) _singleBeamShow.Visible = !show3D;
        }
    }
}
