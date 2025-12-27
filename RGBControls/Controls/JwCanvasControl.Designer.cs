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
            panel3 = new Panel();
            jwShowBeams1 = new JwShowBeams();
            panel2 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            uiLabel1 = new Sunny.UI.UILabel();
            uiSwitch1 = new Sunny.UI.UISwitch();
            uiLabel2 = new Sunny.UI.UILabel();
            uiShowpillar = new Sunny.UI.UISwitch();
            uiLabel3 = new Sunny.UI.UILabel();
            uiShowfuzhu = new Sunny.UI.UISwitch();
            uiLabel4 = new Sunny.UI.UILabel();
            uiGoujian = new Sunny.UI.UISwitch();
            uiLabel6 = new Sunny.UI.UILabel();
            uiSDown = new Sunny.UI.UISwitch();
            uiLabel7 = new Sunny.UI.UILabel();
            uiSwitch3 = new Sunny.UI.UISwitch();
            uiLabel5 = new Sunny.UI.UILabel();
            uiSwitch2 = new Sunny.UI.UISwitch();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(5, 4, 5, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(2224, 1097);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(jwShowBeams1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 119);
            panel3.Margin = new Padding(5, 4, 5, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(2224, 978);
            panel3.TabIndex = 1;
            // 
            // jwShowBeams1
            // 
            jwShowBeams1.BackColor = Color.Black;
            jwShowBeams1.CanvasDraw = null;
            jwShowBeams1.Dock = DockStyle.Fill;
            jwShowBeams1.HasItems = false;
            jwShowBeams1.Location = new Point(0, 0);
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
            jwShowBeams1.Size = new Size(2224, 978);
            jwShowBeams1.TabIndex = 0;
            jwShowBeams1.Text = "jwShowBeams1";
            // 
            // panel2
            // 
            panel2.Controls.Add(flowLayoutPanel1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(5, 4, 5, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(2224, 119);
            panel2.TabIndex = 0;
            panel2.MouseDown += panel2_MouseDown;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(uiLabel1);
            flowLayoutPanel1.Controls.Add(uiSwitch1);
            flowLayoutPanel1.Controls.Add(uiLabel2);
            flowLayoutPanel1.Controls.Add(uiShowpillar);
            flowLayoutPanel1.Controls.Add(uiLabel3);
            flowLayoutPanel1.Controls.Add(uiShowfuzhu);
            flowLayoutPanel1.Controls.Add(uiLabel4);
            flowLayoutPanel1.Controls.Add(uiGoujian);
            flowLayoutPanel1.Controls.Add(uiLabel6);
            flowLayoutPanel1.Controls.Add(uiSDown);
            flowLayoutPanel1.Controls.Add(uiLabel7);
            flowLayoutPanel1.Controls.Add(uiSwitch3);
            flowLayoutPanel1.Controls.Add(uiLabel5);
            flowLayoutPanel1.Controls.Add(uiSwitch2);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(2224, 119);
            flowLayoutPanel1.TabIndex = 14;
            // 
            // uiLabel1
            // 
            uiLabel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel1.Location = new Point(5, 0);
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
            uiSwitch1.Location = new Point(69, 4);
            uiSwitch1.Margin = new Padding(5, 4, 5, 4);
            uiSwitch1.MinimumSize = new Size(2, 1);
            uiSwitch1.Name = "uiSwitch1";
            uiSwitch1.Size = new Size(80, 41);
            uiSwitch1.TabIndex = 0;
            uiSwitch1.Text = "uiSwitch1";
            uiSwitch1.ValueChanged += uiSwitch1_ValueChanged;
            // 
            // uiLabel2
            // 
            uiLabel2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel2.Location = new Point(159, 0);
            uiLabel2.Margin = new Padding(5, 0, 5, 0);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(47, 32);
            uiLabel2.TabIndex = 5;
            uiLabel2.Text = "柱:";
            uiLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiShowpillar
            // 
            uiShowpillar.ActiveText = "On";
            uiShowpillar.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiShowpillar.InActiveText = "Off";
            uiShowpillar.Location = new Point(216, 4);
            uiShowpillar.Margin = new Padding(5, 4, 5, 4);
            uiShowpillar.MinimumSize = new Size(2, 1);
            uiShowpillar.Name = "uiShowpillar";
            uiShowpillar.Size = new Size(89, 41);
            uiShowpillar.TabIndex = 1;
            uiShowpillar.Text = "uiSwitch2";
            uiShowpillar.ValueChanged += uiShowpillar_ValueChanged;
            // 
            // uiLabel3
            // 
            uiLabel3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel3.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel3.Location = new Point(315, 0);
            uiLabel3.Margin = new Padding(5, 0, 5, 0);
            uiLabel3.Name = "uiLabel3";
            uiLabel3.Size = new Size(95, 32);
            uiLabel3.TabIndex = 6;
            uiLabel3.Text = "補助線:";
            uiLabel3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiShowfuzhu
            // 
            uiShowfuzhu.ActiveText = "On";
            uiShowfuzhu.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiShowfuzhu.InActiveText = "Off";
            uiShowfuzhu.Location = new Point(420, 4);
            uiShowfuzhu.Margin = new Padding(5, 4, 5, 4);
            uiShowfuzhu.MinimumSize = new Size(2, 1);
            uiShowfuzhu.Name = "uiShowfuzhu";
            uiShowfuzhu.Size = new Size(91, 41);
            uiShowfuzhu.TabIndex = 2;
            uiShowfuzhu.Text = "uiSwitch2";
            uiShowfuzhu.ValueChanged += uiShowfuzhu_ValueChanged;
            // 
            // uiLabel4
            // 
            uiLabel4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel4.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel4.Location = new Point(521, 0);
            uiLabel4.Margin = new Padding(5, 0, 5, 0);
            uiLabel4.Name = "uiLabel4";
            uiLabel4.Size = new Size(70, 32);
            uiLabel4.TabIndex = 7;
            uiLabel4.Text = "B/BG:";
            uiLabel4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiGoujian
            // 
            uiGoujian.ActiveText = "On";
            uiGoujian.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiGoujian.InActiveText = "Off";
            uiGoujian.Location = new Point(601, 4);
            uiGoujian.Margin = new Padding(5, 4, 5, 4);
            uiGoujian.MinimumSize = new Size(2, 1);
            uiGoujian.Name = "uiGoujian";
            uiGoujian.Size = new Size(89, 41);
            uiGoujian.TabIndex = 3;
            uiGoujian.Text = "uiSwitch2";
            uiGoujian.ValueChanged += uiGoujian_ValueChanged;
            // 
            // uiLabel6
            // 
            uiLabel6.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel6.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel6.Location = new Point(700, 0);
            uiLabel6.Margin = new Padding(5, 0, 5, 0);
            uiLabel6.Name = "uiLabel6";
            uiLabel6.Size = new Size(211, 32);
            uiLabel6.TabIndex = 10;
            uiLabel6.Text = "B(下のみ柱あり):";
            uiLabel6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiSDown
            // 
            uiSDown.ActiveText = "On";
            uiSDown.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSDown.InActiveText = "Off";
            uiSDown.Location = new Point(921, 4);
            uiSDown.Margin = new Padding(5, 4, 5, 4);
            uiSDown.MinimumSize = new Size(2, 1);
            uiSDown.Name = "uiSDown";
            uiSDown.Size = new Size(90, 41);
            uiSDown.TabIndex = 11;
            uiSDown.Text = "uiSwitch2";
            uiSDown.ValueChanged += uiSDown_ValueChanged;
            // 
            // uiLabel7
            // 
            uiLabel7.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel7.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel7.Location = new Point(1019, 0);
            uiLabel7.Name = "uiLabel7";
            uiLabel7.Size = new Size(167, 34);
            uiLabel7.TabIndex = 12;
            uiLabel7.Text = "B/BG自動統計:";
            uiLabel7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiSwitch3
            // 
            uiSwitch3.ActiveText = "On";
            uiSwitch3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSwitch3.InActiveText = "Off";
            uiSwitch3.Location = new Point(1194, 4);
            uiSwitch3.Margin = new Padding(5, 4, 5, 4);
            uiSwitch3.MinimumSize = new Size(2, 1);
            uiSwitch3.Name = "uiSwitch3";
            uiSwitch3.Size = new Size(90, 41);
            uiSwitch3.TabIndex = 13;
            uiSwitch3.Text = "uiSwitch2";
            uiSwitch3.ValueChanged += uiSwitch3_ValueChanged;
            // 
            // uiLabel5
            // 
            uiLabel5.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel5.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel5.Location = new Point(1294, 0);
            uiLabel5.Margin = new Padding(5, 0, 5, 0);
            uiLabel5.Name = "uiLabel5";
            uiLabel5.Size = new Size(95, 32);
            uiLabel5.TabIndex = 8;
            uiLabel5.Text = "梁符号:";
            uiLabel5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiSwitch2
            // 
            uiSwitch2.ActiveText = "On";
            uiSwitch2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSwitch2.InActiveText = "Off";
            uiSwitch2.Location = new Point(1399, 4);
            uiSwitch2.Margin = new Padding(5, 4, 5, 4);
            uiSwitch2.MinimumSize = new Size(2, 1);
            uiSwitch2.Name = "uiSwitch2";
            uiSwitch2.Size = new Size(90, 41);
            uiSwitch2.TabIndex = 9;
            uiSwitch2.Text = "uiSwitch2";
            uiSwitch2.ValueChanged += uiSwitch2_ValueChanged;
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
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private Panel panel2;
        private Sunny.UI.UISwitch uiSwitch1;
        private JwShowBeams jwShowBeams1;
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
        private FlowLayoutPanel flowLayoutPanel1;
    }
}
