using JwShapeCommon;
using JwSharedConfig.Config;

namespace RGBJWMain.Forms
{
    /// <summary>
    /// 2026年11月21日 解析设置窗口中的试验性选项（代码添加，不改 Designer）。
    /// 目前只有「退化矩形を除外」：值保存在 appconfig.json（本机），不写数据库。
    /// </summary>
    public partial class CustomerDesignSetting
    {
        private Sunny.UI.UISwitch? _excludeDegenerateSwitch;

        /// <summary>在「穴処理方法」一行下方追加一行，并把窗口加高一行（底部按钮面板为 Dock=Bottom，会自动下移）。</summary>
        private void AddExperimentalOptions()
        {
            if (_excludeDegenerateSwitch != null) return;
            const int rowHeight = 50;

            var label = new Sunny.UI.UIMarkLabel
            {
                Font = uiMarkLabel15.Font,
                ForeColor = uiMarkLabel15.ForeColor,
                Location = new Point(uiMarkLabel15.Left, uiMarkLabel15.Top + rowHeight),
                Padding = uiMarkLabel15.Padding,
                Size = new Size(320, uiMarkLabel15.Height),
                Text = "退化矩形を除外（試験）",
                TextAlign = ContentAlignment.MiddleLeft,
            };
            _excludeDegenerateSwitch = new Sunny.UI.UISwitch
            {
                ActiveText = "除外",
                InActiveText = "しない",
                Font = uiSwitch1.Font,
                Location = new Point(uiSwitch1.Left, uiSwitch1.Top + rowHeight),
                MinimumSize = uiSwitch1.MinimumSize,
                Size = uiSwitch1.Size,
                Active = AppConfigManager.Current.Parse.ExcludeDegenerateRectangles,
            };
            var tip = new ToolTip();
            const string help = "梁色で描かれた小さな記号などから生じる、面積0の候補矩形（頂点が重複）を梁として扱いません。\n" +
                                "オンにすると解析結果が毎回同じになりますが、梁の本数が減ります。実際の図面で確認してから使用してください。";
            tip.SetToolTip(label, help);
            tip.SetToolTip(_excludeDegenerateSwitch, help);

            ClientSize = new Size(ClientSize.Width, ClientSize.Height + rowHeight);
            Controls.Add(label);
            Controls.Add(_excludeDegenerateSwitch);
        }

        /// <summary>
        /// 2026年11月22日 绝对布局、纵向约 983px：在较小屏幕或较高缩放比例下收进可用区域，放不下的部分可以滚动。
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e); // CustomerDesignSetting_Load（包括追加试验选项那一行）在这里先执行
            AddColorPreviewButton(); // 2026年11月22日 见 CustomerDesignSetting.ColorPreview.cs
            RGBControls.Classes.FormLayoutHelper.FitToWorkingArea(this, allowScroll: true);
        }

        /// <summary>确定时调用：写入解析常量并保存到本机配置。</summary>
        private void ApplyExperimentalOptions()
        {
            if (_excludeDegenerateSwitch == null) return;
            JwFileConsts.ExcludeDegenerateRectangles = _excludeDegenerateSwitch.Active;
            var cfg = AppConfigManager.Current;
            if (cfg.Parse.ExcludeDegenerateRectangles != _excludeDegenerateSwitch.Active)
            {
                cfg.Parse.ExcludeDegenerateRectangles = _excludeDegenerateSwitch.Active;
                AppConfigManager.Save();
            }
        }
    }
}
