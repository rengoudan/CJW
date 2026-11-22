using System.Drawing;
using JwCore;
using RGBControls.Classes;

namespace RGBControls.Forms
{
    /// <summary>
    /// 2026年11月22日 梁製造 CSV を出力する前の確認（表示のみ。CSV の内容・出力処理は変えない）。
    /// 原来点「梁製造CSV」直接弹出保存对话框，导出前看不到内容。
    /// 左：梁ごとの長さ・穴数・CSV 対象（穴が無い梁は ⚠）。右：書き出す CSV そのもの。
    /// </summary>
    public sealed class CsvExportConfirmForm : Form
    {
        /// <summary>確認画面を出し、「出力」なら true。</summary>
        public static bool Confirm(IWin32Window? owner, JwProjectSubData sub, string csv)
        {
            using var f = new CsvExportConfirmForm(sub, csv);
            return f.ShowDialog(owner) == DialogResult.OK;
        }

        private CsvExportConfirmForm(JwProjectSubData sub, string csv)
        {
            float k = DeviceDpi / 96f;
            Text = $"梁製造CSVの確認　{sub.FloorName}";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size((int)(1180 * k), (int)(760 * k));
            MinimumSize = new Size((int)(800 * k), (int)(500 * k));
            ShowInTaskbar = false;
            MinimizeBox = false;
            Font = new Font("Microsoft Sans Serif", 10.5f); // UiFont が日本語 UI フォントへ置き換える

            var beams = sub.JwBeamDatas.OrderBy(b => b.BeamCode, StringComparer.Ordinal).ToList();
            int target = beams.Count(b => b.HasCsv);
            int need = beams.Count(b => b.JwHoles.Count == 0);

            var header = new Label
            {
                Dock = DockStyle.Top,
                Height = (int)(56 * k),
                Padding = new Padding(10, 8, 0, 0),
                Text = $"梁 {beams.Count} 本・CSV 対象 {target} 本" + (need > 0 ? $"・要確認（穴なし）{need} 本" : "")
                       + Environment.NewLine
                       + (need > 0 ? "⚠ 穴の無い梁があります。内容を確認してから出力してください。" : "内容を確認して「出力」を押してください。"),
                ForeColor = need > 0 ? Color.Firebrick : SystemColors.ControlText,
            };

            var list = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
            };
            foreach (var (t, w, a) in new[] {
                ("梁番号", 110, HorizontalAlignment.Left), ("長さ", 90, HorizontalAlignment.Right), ("コア長", 90, HorizontalAlignment.Right),
                ("穴数", 60, HorizontalAlignment.Right), ("CSV", 50, HorizontalAlignment.Center), ("確認", 100, HorizontalAlignment.Left) })
                list.Columns.Add(t, (int)(w * k), a);
            foreach (var b in beams)
            {
                bool noHole = b.JwHoles.Count == 0;
                var item = new ListViewItem(new[]
                {
                    b.BeamCode, b.Length.ToString("0.#"), b.XXLength.ToString("0.#"), b.JwHoles.Count.ToString(),
                    b.HasCsv ? "○" : "－", noHole ? "⚠ 穴なし" : "",
                });
                if (noHole) item.ForeColor = Color.Firebrick;
                else if (!b.HasCsv) item.ForeColor = Color.Gray;
                list.Items.Add(item);
            }
            var left = new Panel { Dock = DockStyle.Left, Width = (int)(540 * k), Padding = new Padding(8, 0, 4, 0) };
            var listBox = new GroupBox { Text = "梁の一覧（灰色は CSV 対象外）", Dock = DockStyle.Fill, Padding = new Padding(6) };
            listBox.Controls.Add(list);
            left.Controls.Add(listBox);

            var text = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                Font = new Font(FontFamily.GenericMonospace, 10f),
                Text = csv.Replace("\r\n", "\n").Replace("\n", Environment.NewLine),
            };
            var csvBox = new GroupBox { Text = "書き出す CSV", Dock = DockStyle.Fill, Padding = new Padding(6) };
            csvBox.Controls.Add(text);
            var right = new Panel { Dock = DockStyle.Fill, Padding = new Padding(4, 0, 8, 0) };
            right.Controls.Add(csvBox);

            var ok = new Button { Text = "出力…", AutoSize = true, Padding = new Padding(16, 2, 16, 2), DialogResult = DialogResult.OK };
            var cancel = new Button { Text = "キャンセル", AutoSize = true, Padding = new Padding(16, 2, 16, 2), DialogResult = DialogResult.Cancel };
            var bottom = new Panel { Dock = DockStyle.Bottom, Height = (int)(48 * k) };
            bottom.Controls.AddRange(new Control[] { ok, cancel });
            bottom.Layout += (s, e) =>
            {
                cancel.Location = new Point(bottom.ClientSize.Width - cancel.Width - 12, (int)(8 * k));
                ok.Location = new Point(cancel.Left - ok.Width - 8, cancel.Top);
            };
            AcceptButton = ok;
            CancelButton = cancel;

            Controls.Add(right);
            Controls.Add(left);
            Controls.Add(bottom);
            Controls.Add(header);

            // 開いたときに CSV 全体が選択状態にならないよう、一覧にフォーカスを置く
            Shown += (s, e) => { text.SelectionStart = 0; text.SelectionLength = 0; list.Focus(); };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FormLayoutHelper.FitToWorkingArea(this);
        }
    }
}
