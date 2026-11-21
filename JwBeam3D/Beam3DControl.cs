using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace JwBeam3D
{
    /// <summary>
    /// WinForms から使う 3D 表示コントロール。CSV（V2.2）文字列を渡すと、その内容どおりに描画する。
    /// 画面の CSV と 3D が常に同じデータになるよう、CSV を唯一の入力にしている。
    /// </summary>
    public sealed class Beam3DControl : UserControl
    {
        private readonly Beam3DView _view = new();

        public Beam3DControl()
        {
            Controls.Add(new ElementHost { Dock = DockStyle.Fill, Child = _view });
        }

        /// <summary>
        /// 縦穴数 1 の CSV 行を 2 穴（Y と Y+PairPitch）で表示するときのピッチ mm。0 なら CSV どおり 1 穴。
        /// 2D 図と合わせるため、呼び出し側から JwFileConsts.EllipseSpacing を渡す。
        /// </summary>
        public double PairPitch { get; set; }

        /// <summary>CSV を解析して表示する。複数製品が含まれる場合は先頭の 1 本を表示。</summary>
        public void ShowCsv(string? csv)
        {
            if (string.IsNullOrWhiteSpace(csv))
            {
                _view.ShowMessage("CSV データがありません。");
                return;
            }
            try
            {
                var product = ProcessCsvParser.Parse(csv).FirstOrDefault();
                if (product == null)
                {
                    _view.ShowMessage("CSV に製品データ（START～END）がありません。");
                    return;
                }
                if (product.Size == null || !product.Size.IsH)
                {
                    _view.ShowMessage($"3D 表示は H形鋼のみ対応しています（鋼材サイズ「{product.SizeRaw}」）。");
                    return;
                }
                _view.Show(product, BeamGeometry.Place(product, PairPitch));
            }
            catch (Exception ex)
            {
                // 3D 表示の失敗で画面全体を止めない（ログには残す）
                JwLogging.AppLog.Warn(ex, "3D表示に失敗");
                _view.ShowMessage("3D 表示に失敗しました: " + ex.Message);
            }
        }
    }
}
