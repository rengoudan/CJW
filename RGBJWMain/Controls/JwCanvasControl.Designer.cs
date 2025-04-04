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
            uiSDown = new Sunny.UI.UISwitch();
            uiLabel6 = new Sunny.UI.UILabel();
            uiSwitch2 = new Sunny.UI.UISwitch();
            uiLabel5 = new Sunny.UI.UILabel();
            uiLabel4 = new Sunny.UI.UILabel();
            uiLabel3 = new Sunny.UI.UILabel();
            uiLabel2 = new Sunny.UI.UILabel();
            uiLabel1 = new Sunny.UI.UILabel();
            uiGoujian = new Sunny.UI.UISwitch();
            uiShowfuzhu = new Sunny.UI.UISwitch();
            uiShowpillar = new Sunny.UI.UISwitch();
            uiSwitch1 = new Sunny.UI.UISwitch();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1415, 777);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(jwShowBeams1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 53);
            panel3.Name = "panel3";
            panel3.Size = new Size(1415, 724);
            panel3.TabIndex = 1;
            // 
            // jwShowBeams1
            // 
            jwShowBeams1.BackColor = Color.Black;
            jwShowBeams1.CanvasDraw = null;
            jwShowBeams1.Dock = DockStyle.Fill;
            jwShowBeams1.HasItems = false;
            jwShowBeams1.Location = new Point(0, 0);
            jwShowBeams1.Name = "jwShowBeams1";
            jwShowBeams1.SelectedBeam = null;
            jwShowBeams1.SelectedSquare = null;
            jwShowBeams1.ShowBeams = false;
            jwShowBeams1.ShowFuzhu = false;
            jwShowBeams1.ShowGoujian = false;
            jwShowBeams1.Showmsg = false;
            jwShowBeams1.ShowPillar = false;
            jwShowBeams1.Size = new Size(1415, 724);
            jwShowBeams1.TabIndex = 0;
            jwShowBeams1.Text = "jwShowBeams1";
            jwShowBeams1.Click += jwShowBeams1_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(uiSDown);
            panel2.Controls.Add(uiLabel6);
            panel2.Controls.Add(uiSwitch2);
            panel2.Controls.Add(uiLabel5);
            panel2.Controls.Add(uiLabel4);
            panel2.Controls.Add(uiLabel3);
            panel2.Controls.Add(uiLabel2);
            panel2.Controls.Add(uiLabel1);
            panel2.Controls.Add(uiGoujian);
            panel2.Controls.Add(uiShowfuzhu);
            panel2.Controls.Add(uiShowpillar);
            panel2.Controls.Add(uiSwitch1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1415, 53);
            panel2.TabIndex = 0;
            // 
            // uiSDown
            // 
            uiSDown.ActiveText = "On";
            uiSDown.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSDown.InActiveText = "Off";
            uiSDown.Location = new Point(1023, 14);
            uiSDown.MinimumSize = new Size(1, 1);
            uiSDown.Name = "uiSDown";
            uiSDown.Size = new Size(75, 29);
            uiSDown.TabIndex = 11;
            uiSDown.Text = "uiSwitch2";
            uiSDown.ValueChanged += uiSDown_ValueChanged;
            // 
            // uiLabel6
            // 
            uiLabel6.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel6.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel6.Location = new Point(880, 20);
            uiLabel6.Name = "uiLabel6";
            uiLabel6.Size = new Size(137, 23);
            uiLabel6.TabIndex = 10;
            uiLabel6.Text = "B(下のみ柱あり):";
            uiLabel6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiSwitch2
            // 
            uiSwitch2.ActiveText = "On";
            uiSwitch2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSwitch2.InActiveText = "Off";
            uiSwitch2.Location = new Point(1231, 14);
            uiSwitch2.MinimumSize = new Size(1, 1);
            uiSwitch2.Name = "uiSwitch2";
            uiSwitch2.Size = new Size(75, 29);
            uiSwitch2.TabIndex = 9;
            uiSwitch2.Text = "uiSwitch2";
            uiSwitch2.ValueChanged += uiSwitch2_ValueChanged;
            // 
            // uiLabel5
            // 
            uiLabel5.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel5.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel5.Location = new Point(1159, 20);
            uiLabel5.Name = "uiLabel5";
            uiLabel5.Size = new Size(78, 23);
            uiLabel5.TabIndex = 8;
            uiLabel5.Text = "梁符号:";
            uiLabel5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel4
            // 
            uiLabel4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel4.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel4.Location = new Point(697, 20);
            uiLabel4.Name = "uiLabel4";
            uiLabel4.Size = new Size(78, 23);
            uiLabel4.TabIndex = 7;
            uiLabel4.Text = "B/BG:";
            uiLabel4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel3
            // 
            uiLabel3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel3.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel3.Location = new Point(496, 20);
            uiLabel3.Name = "uiLabel3";
            uiLabel3.Size = new Size(78, 23);
            uiLabel3.TabIndex = 6;
            uiLabel3.Text = "補助線:";
            uiLabel3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel2
            // 
            uiLabel2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel2.Location = new Point(305, 20);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(50, 23);
            uiLabel2.TabIndex = 5;
            uiLabel2.Text = "柱:";
            uiLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel1
            // 
            uiLabel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel1.Location = new Point(51, 20);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(50, 23);
            uiLabel1.TabIndex = 4;
            uiLabel1.Text = "梁:";
            uiLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiGoujian
            // 
            uiGoujian.ActiveText = "On";
            uiGoujian.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiGoujian.InActiveText = "Off";
            uiGoujian.Location = new Point(781, 14);
            uiGoujian.MinimumSize = new Size(1, 1);
            uiGoujian.Name = "uiGoujian";
            uiGoujian.Size = new Size(75, 29);
            uiGoujian.TabIndex = 3;
            uiGoujian.Text = "uiSwitch2";
            uiGoujian.ValueChanged += uiGoujian_ValueChanged;
            // 
            // uiShowfuzhu
            // 
            uiShowfuzhu.ActiveText = "On";
            uiShowfuzhu.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiShowfuzhu.InActiveText = "Off";
            uiShowfuzhu.Location = new Point(580, 14);
            uiShowfuzhu.MinimumSize = new Size(1, 1);
            uiShowfuzhu.Name = "uiShowfuzhu";
            uiShowfuzhu.Size = new Size(75, 29);
            uiShowfuzhu.TabIndex = 2;
            uiShowfuzhu.Text = "uiSwitch2";
            uiShowfuzhu.ValueChanged += uiShowfuzhu_ValueChanged;
            // 
            // uiShowpillar
            // 
            uiShowpillar.ActiveText = "On";
            uiShowpillar.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiShowpillar.InActiveText = "Off";
            uiShowpillar.Location = new Point(361, 14);
            uiShowpillar.MinimumSize = new Size(1, 1);
            uiShowpillar.Name = "uiShowpillar";
            uiShowpillar.Size = new Size(75, 29);
            uiShowpillar.TabIndex = 1;
            uiShowpillar.Text = "uiSwitch2";
            uiShowpillar.ValueChanged += uiShowpillar_ValueChanged;
            // 
            // uiSwitch1
            // 
            uiSwitch1.ActiveText = "On";
            uiSwitch1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSwitch1.InActiveText = "Off";
            uiSwitch1.Location = new Point(107, 14);
            uiSwitch1.MinimumSize = new Size(1, 1);
            uiSwitch1.Name = "uiSwitch1";
            uiSwitch1.Size = new Size(75, 29);
            uiSwitch1.TabIndex = 0;
            uiSwitch1.Text = "uiSwitch1";
            uiSwitch1.ValueChanged += uiSwitch1_ValueChanged;
            // 
            // JwCanvasControl
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "JwCanvasControl";
            Size = new Size(1415, 777);
            AutoSizeChanged += JwCanvasControl_AutoSizeChanged;
            Click += JwCanvasControl_Click;
            MouseMove += JwCanvasControl_MouseMove;
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
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
    }
}
