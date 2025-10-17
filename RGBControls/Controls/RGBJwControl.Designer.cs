namespace RGBJWMain.Controls
{
    partial class RGBJwControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            uiPanel1 = new Sunny.UI.UIPanel();
            uiLabel2 = new Sunny.UI.UILabel();
            uiLabel1 = new Sunny.UI.UILabel();
            uiSwitch2 = new Sunny.UI.UISwitch();
            uiSwitch1 = new Sunny.UI.UISwitch();
            rgbJwwShow1 = new RGBJwwShow();
            uiPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // uiPanel1
            // 
            uiPanel1.Controls.Add(uiLabel2);
            uiPanel1.Controls.Add(uiLabel1);
            uiPanel1.Controls.Add(uiSwitch2);
            uiPanel1.Controls.Add(uiSwitch1);
            uiPanel1.Dock = DockStyle.Top;
            uiPanel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiPanel1.Location = new Point(0, 0);
            uiPanel1.Margin = new Padding(4, 5, 4, 5);
            uiPanel1.MinimumSize = new Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.Size = new Size(1248, 54);
            uiPanel1.TabIndex = 0;
            uiPanel1.Text = null;
            uiPanel1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiLabel2
            // 
            uiLabel2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel2.Location = new Point(280, 19);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(100, 23);
            uiLabel2.TabIndex = 4;
            uiLabel2.Text = "形";
            uiLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel1
            // 
            uiLabel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel1.Location = new Point(34, 19);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(100, 23);
            uiLabel1.TabIndex = 3;
            uiLabel1.Text = "ライン";
            uiLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiSwitch2
            // 
            uiSwitch2.ActiveText = "On";
            uiSwitch2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSwitch2.InActiveText = "Off";
            uiSwitch2.Location = new Point(386, 13);
            uiSwitch2.MinimumSize = new Size(1, 1);
            uiSwitch2.Name = "uiSwitch2";
            uiSwitch2.Size = new Size(75, 29);
            uiSwitch2.TabIndex = 2;
            uiSwitch2.Text = "uiSwitch2";
            uiSwitch2.ValueChanged += uiSwitch2_ValueChanged;
            // 
            // uiSwitch1
            // 
            uiSwitch1.ActiveText = "On";
            uiSwitch1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSwitch1.InActiveText = "Off";
            uiSwitch1.Location = new Point(140, 13);
            uiSwitch1.MinimumSize = new Size(1, 1);
            uiSwitch1.Name = "uiSwitch1";
            uiSwitch1.Size = new Size(75, 29);
            uiSwitch1.TabIndex = 1;
            uiSwitch1.Text = "uiSwitch1";
            uiSwitch1.ValueChanged += uiSwitch1_ValueChanged;
            // 
            // rgbJwwShow1
            // 
            rgbJwwShow1.BackColor = Color.Black;
            rgbJwwShow1.Blocks = null;
            rgbJwwShow1.Colors = null;
            rgbJwwShow1.Dock = DockStyle.Fill;
            rgbJwwShow1.IsDraw = false;
            rgbJwwShow1.Location = new Point(0, 54);
            rgbJwwShow1.Maxx = 0D;
            rgbJwwShow1.Maxy = 0D;
            rgbJwwShow1.Minx = 0D;
            rgbJwwShow1.Miny = 0D;
            rgbJwwShow1.Name = "rgbJwwShow1";
            rgbJwwShow1.S = null;
            rgbJwwShow1.Sens = null;
            rgbJwwShow1.ShowSen = false;
            rgbJwwShow1.ShowShape = false;
            rgbJwwShow1.Size = new Size(1248, 691);
            rgbJwwShow1.Solid = null;
            rgbJwwShow1.TabIndex = 1;
            rgbJwwShow1.Text = "rgbJwwShow1";
            // 
            // RGBJwControl
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(rgbJwwShow1);
            Controls.Add(uiPanel1);
            Name = "RGBJwControl";
            Size = new Size(1248, 745);
            uiPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UISwitch uiSwitch1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UISwitch uiSwitch2;
        private RGBJwwShow rgbJwwShow1;
    }
}
