namespace RGBJWMain.Controls
{
    partial class JwCanvasControl
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
            panel1 = new Panel();
            jwShowBeams1 = new JwShowBeams();
            panel2 = new Panel();
            uiLabel1 = new Sunny.UI.UILabel();
            uiSwitch1 = new Sunny.UI.UISwitch();
            uiSwitch2 = new Sunny.UI.UISwitch();
            uiLabel2 = new Sunny.UI.UILabel();
            uiLabel5 = new Sunny.UI.UILabel();
            uiShowpillar = new Sunny.UI.UISwitch();
            uiSwitch3 = new Sunny.UI.UISwitch();
            uiLabel3 = new Sunny.UI.UILabel();
            uiLabel7 = new Sunny.UI.UILabel();
            uiShowfuzhu = new Sunny.UI.UISwitch();
            uiSDown = new Sunny.UI.UISwitch();
            uiLabel4 = new Sunny.UI.UILabel();
            uiLabel6 = new Sunny.UI.UILabel();
            uiGoujian = new Sunny.UI.UISwitch();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(jwShowBeams1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(5, 4, 5, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(2224, 1097);
            panel1.TabIndex = 0;
            // 
            // jwShowBeams1
            // 
            jwShowBeams1.BackColor = Color.Black;
            jwShowBeams1.CanvasDraw = null;
            jwShowBeams1.Dock = DockStyle.Fill;
            jwShowBeams1.HasItems = false;
            jwShowBeams1.Location = new Point(0, 66);
            jwShowBeams1.Margin = new Padding(5, 4, 5, 4);
            jwShowBeams1.Name = "jwShowBeams1";
            jwShowBeams1.SelectedBeam = null;
            jwShowBeams1.SelectedSquare = null;
            jwShowBeams1.SelectPillar = null;
            jwShowBeams1.ShowBeams = false;
            jwShowBeams1.ShowDownB = false;
            jwShowBeams1.ShowFuzhu = false;
            jwShowBeams1.ShowGoujian = false;
            jwShowBeams1.ShowGoujiantext = false;
            jwShowBeams1.Showmsg = false;
            jwShowBeams1.ShowPillar = false;
            jwShowBeams1.Size = new Size(2224, 1031);
            jwShowBeams1.TabIndex = 2;
            jwShowBeams1.Text = "jwShowBeams1";
            // 
            // panel2
            // 
            panel2.Controls.Add(uiLabel1);
            panel2.Controls.Add(uiSwitch1);
            panel2.Controls.Add(uiSwitch2);
            panel2.Controls.Add(uiLabel2);
            panel2.Controls.Add(uiLabel5);
            panel2.Controls.Add(uiShowpillar);
            panel2.Controls.Add(uiSwitch3);
            panel2.Controls.Add(uiLabel3);
            panel2.Controls.Add(uiLabel7);
            panel2.Controls.Add(uiShowfuzhu);
            panel2.Controls.Add(uiSDown);
            panel2.Controls.Add(uiLabel4);
            panel2.Controls.Add(uiLabel6);
            panel2.Controls.Add(uiGoujian);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(5, 4, 5, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(2224, 66);
            panel2.TabIndex = 1;
            panel2.MouseDown += panel2_MouseDown;
            // 
            // uiLabel1
            // 
            uiLabel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel1.Location = new Point(5, 5);
            uiLabel1.Margin = new Padding(5, 0, 5, 0);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(54, 40);
            uiLabel1.TabIndex = 4;
            uiLabel1.Text = "梁:";
            uiLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiSwitch1
            // 
            uiSwitch1.ActiveText = "On";
            uiSwitch1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSwitch1.InActiveText = "Off";
            uiSwitch1.Location = new Point(69, 9);
            uiSwitch1.Margin = new Padding(5, 4, 5, 4);
            uiSwitch1.MinimumSize = new Size(2, 1);
            uiSwitch1.Name = "uiSwitch1";
            uiSwitch1.Size = new Size(80, 41);
            uiSwitch1.TabIndex = 5;
            uiSwitch1.Text = "uiSwitch1";
            uiSwitch1.ValueChanged += uiSwitch1_ValueChanged;
            // 
            // uiSwitch2
            // 
            uiSwitch2.ActiveText = "On";
            uiSwitch2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSwitch2.InActiveText = "Off";
            uiSwitch2.Location = new Point(1399, 9);
            uiSwitch2.Margin = new Padding(5, 4, 5, 4);
            uiSwitch2.MinimumSize = new Size(2, 1);
            uiSwitch2.Name = "uiSwitch2";
            uiSwitch2.Size = new Size(90, 41);
            uiSwitch2.TabIndex = 17;
            uiSwitch2.Text = "uiSwitch2";
            uiSwitch2.ValueChanged += uiSwitch2_ValueChanged;
            // 
            // uiLabel2
            // 
            uiLabel2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel2.Location = new Point(159, 5);
            uiLabel2.Margin = new Padding(5, 0, 5, 0);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(47, 32);
            uiLabel2.TabIndex = 6;
            uiLabel2.Text = "柱:";
            uiLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel5
            // 
            uiLabel5.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel5.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel5.Location = new Point(1294, 5);
            uiLabel5.Margin = new Padding(5, 0, 5, 0);
            uiLabel5.Name = "uiLabel5";
            uiLabel5.Size = new Size(95, 32);
            uiLabel5.TabIndex = 16;
            uiLabel5.Text = "梁符号:";
            uiLabel5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiShowpillar
            // 
            uiShowpillar.ActiveText = "On";
            uiShowpillar.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiShowpillar.InActiveText = "Off";
            uiShowpillar.Location = new Point(216, 9);
            uiShowpillar.Margin = new Padding(5, 4, 5, 4);
            uiShowpillar.MinimumSize = new Size(2, 1);
            uiShowpillar.Name = "uiShowpillar";
            uiShowpillar.Size = new Size(89, 41);
            uiShowpillar.TabIndex = 7;
            uiShowpillar.Text = "uiSwitch2";
            uiShowpillar.ValueChanged += uiShowpillar_ValueChanged;
            // 
            // uiSwitch3
            // 
            uiSwitch3.ActiveText = "On";
            uiSwitch3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSwitch3.InActiveText = "Off";
            uiSwitch3.Location = new Point(1194, 9);
            uiSwitch3.Margin = new Padding(5, 4, 5, 4);
            uiSwitch3.MinimumSize = new Size(2, 1);
            uiSwitch3.Name = "uiSwitch3";
            uiSwitch3.Size = new Size(90, 41);
            uiSwitch3.TabIndex = 15;
            uiSwitch3.Text = "uiSwitch2";
            uiSwitch3.ValueChanged += uiSwitch3_ValueChanged;
            // 
            // uiLabel3
            // 
            uiLabel3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel3.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel3.Location = new Point(315, 5);
            uiLabel3.Margin = new Padding(5, 0, 5, 0);
            uiLabel3.Name = "uiLabel3";
            uiLabel3.Size = new Size(95, 32);
            uiLabel3.TabIndex = 8;
            uiLabel3.Text = "補助線:";
            uiLabel3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel7
            // 
            uiLabel7.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel7.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel7.Location = new Point(1019, 5);
            uiLabel7.Name = "uiLabel7";
            uiLabel7.Size = new Size(167, 34);
            uiLabel7.TabIndex = 14;
            uiLabel7.Text = "B/BG自動統計:";
            uiLabel7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiShowfuzhu
            // 
            uiShowfuzhu.ActiveText = "On";
            uiShowfuzhu.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiShowfuzhu.InActiveText = "Off";
            uiShowfuzhu.Location = new Point(420, 9);
            uiShowfuzhu.Margin = new Padding(5, 4, 5, 4);
            uiShowfuzhu.MinimumSize = new Size(2, 1);
            uiShowfuzhu.Name = "uiShowfuzhu";
            uiShowfuzhu.Size = new Size(91, 41);
            uiShowfuzhu.TabIndex = 9;
            uiShowfuzhu.Text = "uiSwitch2";
            uiShowfuzhu.ValueChanged += uiShowfuzhu_ValueChanged;
            // 
            // uiSDown
            // 
            uiSDown.ActiveText = "On";
            uiSDown.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSDown.InActiveText = "Off";
            uiSDown.Location = new Point(921, 9);
            uiSDown.Margin = new Padding(5, 4, 5, 4);
            uiSDown.MinimumSize = new Size(2, 1);
            uiSDown.Name = "uiSDown";
            uiSDown.Size = new Size(90, 41);
            uiSDown.TabIndex = 13;
            uiSDown.Text = "uiSwitch2";
            uiSDown.ValueChanged += uiSDown_ValueChanged;
            // 
            // uiLabel4
            // 
            uiLabel4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel4.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel4.Location = new Point(521, 5);
            uiLabel4.Margin = new Padding(5, 0, 5, 0);
            uiLabel4.Name = "uiLabel4";
            uiLabel4.Size = new Size(70, 32);
            uiLabel4.TabIndex = 10;
            uiLabel4.Text = "B/BG:";
            uiLabel4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel6
            // 
            uiLabel6.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel6.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel6.Location = new Point(700, 5);
            uiLabel6.Margin = new Padding(5, 0, 5, 0);
            uiLabel6.Name = "uiLabel6";
            uiLabel6.Size = new Size(211, 32);
            uiLabel6.TabIndex = 12;
            uiLabel6.Text = "B(下のみ柱あり):";
            uiLabel6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiGoujian
            // 
            uiGoujian.ActiveText = "On";
            uiGoujian.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiGoujian.InActiveText = "Off";
            uiGoujian.Location = new Point(601, 9);
            uiGoujian.Margin = new Padding(5, 4, 5, 4);
            uiGoujian.MinimumSize = new Size(2, 1);
            uiGoujian.Name = "uiGoujian";
            uiGoujian.Size = new Size(89, 41);
            uiGoujian.TabIndex = 11;
            uiGoujian.Text = "uiSwitch2";
            uiGoujian.ValueChanged += uiGoujian_ValueChanged;
            // 
            // JwCanvasControl
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Margin = new Padding(5, 4, 5, 4);
            Name = "JwCanvasControl";
            Size = new Size(2224, 1097);
            AutoSizeChanged += JwCanvasControl_AutoSizeChanged;
            Click += JwCanvasControl_Click;
            MouseMove += JwCanvasControl_MouseMove;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Sunny.UI.UISwitch uiSwitch1;
        private Sunny.UI.UISwitch uiShowpillar;
        private Sunny.UI.UISwitch uiShowfuzhu;
        private Sunny.UI.UISwitch uiGoujian;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UISwitch uiSwitch2;
        private Sunny.UI.UILabel uiLabel5;
        private Sunny.UI.UILabel uiLabel6;
        private Sunny.UI.UISwitch uiSDown;
        private Sunny.UI.UISwitch uiSwitch3;
        private Sunny.UI.UILabel uiLabel7;
        private JwShowBeams jwShowBeams1;
    }
}
