using JwCore;
using JwShapeCommon;

namespace RGBControls.Controls
{
    /// <summary>
    /// 2026年11月22日 「要確認」の梁を探しやすくする（表示・選択のみ。データは変えない）。
    /// ・梁の一覧に「確認」列：穴が 1 つも無い梁に「穴なし」（工事一覧の状態列と同じ判定）。
    /// ・「要確認のみ」で一覧を絞り込む。
    /// ・StartWithNeedCheck = true で開くと、絞り込んだうえで最初の要確認の梁を選択して図上で強調する。
    /// </summary>
    public partial class Sub
    {
        /// <summary>開いたときに要確認の梁だけを表示し、最初の 1 本を選択する。</summary>
        public bool StartWithNeedCheck { get; set; }

        private CheckBox? _needCheckOnly;
        private Label? _needCheckCount;

        private static bool NeedsCheck(JwBeamData b) => b.JwHoles.Count == 0;

        /// <summary>一覧の列定義の後に呼ぶ。</summary>
        private void SetupNeedCheckUi()
        {
            if (_needCheckOnly != null) return;
            liangtable.Columns.Insert(1, new AntdUI.Column("JwHoles", "確認")
            {
                Render = (value, record, row) => record is JwBeamData b && NeedsCheck(b)
                    ? new AntdUI.CellTag("穴なし", AntdUI.TTypeMini.Error)
                    : null,
            });

            _needCheckOnly = new CheckBox { Text = "要確認のみ", AutoSize = true, Margin = new Padding(6, 6, 6, 0) };
            _needCheckOnly.CheckedChanged += (s, e) => ApplyBeamFilter();
            _needCheckCount = new Label { AutoSize = true, Margin = new Padding(0, 9, 0, 0), ForeColor = Color.Firebrick };
            var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 30, WrapContents = false };
            bar.Controls.Add(_needCheckOnly);
            bar.Controls.Add(_needCheckCount);
            panel6.Controls.Add(bar);
            bar.BringToFront();
            liangtable.BringToFront(); // Fill は最後に配置（bar の下を埋める）
            panel6.Height += bar.Height;
        }

        /// <summary>梁の一覧を（必要なら要確認だけに）表示する。</summary>
        private void ApplyBeamFilter()
        {
            if (_subdata == null) return;
            var all = _subdata.JwBeamDatas.OrderBy(t => t.BeamCode).ToList();
            int n = all.Count(NeedsCheck);
            if (_needCheckCount != null) _needCheckCount.Text = n > 0 ? $"要確認 {n} 本" : "";
            liangtable.DataSource = _needCheckOnly?.Checked == true ? all.Where(NeedsCheck).ToList() : all;
        }

        /// <summary>要確認の梁だけにして、最初の 1 本を選択・図上で強調する。</summary>
        public void FocusFirstNeedCheck()
        {
            if (_needCheckOnly == null || _subdata == null) return;
            _needCheckOnly.Checked = true;
            ApplyBeamFilter();
            var first = _subdata.JwBeamDatas.Where(NeedsCheck).OrderBy(t => t.BeamCode).FirstOrDefault();
            if (first == null) return;
            liangtable.SelectedIndex = 1; // AntdUI の行番号は見出しが 0
            GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent?.Invoke(this, new ControlSelectedSquareArgs
            {
                Id = first.Id,
                DrawShapeType = DrawShapeType.Beam,
            });
        }
    }
}
