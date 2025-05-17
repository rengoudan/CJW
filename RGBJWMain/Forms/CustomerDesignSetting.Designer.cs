namespace RGBJWMain.Forms
{
    partial class CustomerDesignSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            uiButton1 = new Sunny.UI.UIButton();
            uiNearSpliteMax = new Sunny.UI.UIDoubleUpDown();
            uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            uiComboBox2 = new Sunny.UI.UIComboBox();
            uiComboBox1 = new Sunny.UI.UIComboBox();
            uiCbSplitcolor = new Sunny.UI.UIComboBox();
            uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            uiCbpillarcolor = new Sunny.UI.UIComboBox();
            uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            uiCbTextColor = new Sunny.UI.UIComboBox();
            uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            uiCbBeamcolor = new Sunny.UI.UIComboBox();
            uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            uiMarkLabel6 = new Sunny.UI.UIMarkLabel();
            uiTextBox1 = new Sunny.UI.UITextBox();
            uiMarkLabel7 = new Sunny.UI.UIMarkLabel();
            uiComboBox3 = new Sunny.UI.UIComboBox();
            uiMarkLabel8 = new Sunny.UI.UIMarkLabel();
            uidownpillarcolor = new Sunny.UI.UIComboBox();
            uiMarkLabel9 = new Sunny.UI.UIMarkLabel();
            uiDoubleUpDown1 = new Sunny.UI.UIDoubleUpDown();
            uiMarkLabel10 = new Sunny.UI.UIMarkLabel();
            uiComboBox4 = new Sunny.UI.UIComboBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(uiButton1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 603);
            panel1.Name = "panel1";
            panel1.Size = new Size(622, 53);
            panel1.TabIndex = 0;
            // 
            // uiButton1
            // 
            uiButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiButton1.Location = new Point(506, 10);
            uiButton1.MinimumSize = new Size(1, 1);
            uiButton1.Name = "uiButton1";
            uiButton1.Size = new Size(100, 35);
            uiButton1.TabIndex = 0;
            uiButton1.Text = "セーブ";
            uiButton1.Click += uiButton1_Click;
            // 
            // uiNearSpliteMax
            // 
            uiNearSpliteMax.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiNearSpliteMax.Location = new Point(420, 357);
            uiNearSpliteMax.Margin = new Padding(4, 5, 4, 5);
            uiNearSpliteMax.Maximum = 100D;
            uiNearSpliteMax.Minimum = 0D;
            uiNearSpliteMax.MinimumSize = new Size(100, 0);
            uiNearSpliteMax.Name = "uiNearSpliteMax";
            uiNearSpliteMax.ShowText = false;
            uiNearSpliteMax.Size = new Size(119, 29);
            uiNearSpliteMax.Step = 5D;
            uiNearSpliteMax.TabIndex = 22;
            uiNearSpliteMax.Text = "uiDoubleUpDown1";
            uiNearSpliteMax.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel5
            // 
            uiMarkLabel5.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel5.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel5.Location = new Point(84, 363);
            uiMarkLabel5.Name = "uiMarkLabel5";
            uiMarkLabel5.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel5.Size = new Size(168, 23);
            uiMarkLabel5.TabIndex = 21;
            uiMarkLabel5.Text = "分割点";
            uiMarkLabel5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiComboBox2
            // 
            uiComboBox2.DataSource = null;
            uiComboBox2.FillColor = Color.White;
            uiComboBox2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox2.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiComboBox2.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiComboBox2.Location = new Point(290, 238);
            uiComboBox2.Margin = new Padding(4, 5, 4, 5);
            uiComboBox2.MinimumSize = new Size(63, 0);
            uiComboBox2.Name = "uiComboBox2";
            uiComboBox2.Padding = new Padding(0, 0, 30, 2);
            uiComboBox2.Size = new Size(111, 29);
            uiComboBox2.SymbolSize = 24;
            uiComboBox2.TabIndex = 20;
            uiComboBox2.Text = "線種番号";
            uiComboBox2.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox2.Visible = false;
            uiComboBox2.Watermark = "";
            // 
            // uiComboBox1
            // 
            uiComboBox1.DataSource = null;
            uiComboBox1.FillColor = Color.White;
            uiComboBox1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox1.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiComboBox1.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiComboBox1.Location = new Point(290, 299);
            uiComboBox1.Margin = new Padding(4, 5, 4, 5);
            uiComboBox1.MinimumSize = new Size(63, 0);
            uiComboBox1.Name = "uiComboBox1";
            uiComboBox1.Padding = new Padding(0, 0, 30, 2);
            uiComboBox1.Size = new Size(111, 29);
            uiComboBox1.SymbolSize = 24;
            uiComboBox1.TabIndex = 19;
            uiComboBox1.Text = "線種番号";
            uiComboBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox1.Visible = false;
            uiComboBox1.Watermark = "";
            // 
            // uiCbSplitcolor
            // 
            uiCbSplitcolor.DataSource = null;
            uiCbSplitcolor.FillColor = Color.White;
            uiCbSplitcolor.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiCbSplitcolor.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiCbSplitcolor.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiCbSplitcolor.Location = new Point(420, 299);
            uiCbSplitcolor.Margin = new Padding(4, 5, 4, 5);
            uiCbSplitcolor.MinimumSize = new Size(63, 0);
            uiCbSplitcolor.Name = "uiCbSplitcolor";
            uiCbSplitcolor.Padding = new Padding(0, 0, 30, 2);
            uiCbSplitcolor.Size = new Size(119, 29);
            uiCbSplitcolor.SymbolSize = 24;
            uiCbSplitcolor.TabIndex = 18;
            uiCbSplitcolor.Text = "線⾊番号";
            uiCbSplitcolor.TextAlignment = ContentAlignment.MiddleLeft;
            uiCbSplitcolor.Watermark = "";
            // 
            // uiMarkLabel4
            // 
            uiMarkLabel4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel4.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel4.Location = new Point(84, 299);
            uiMarkLabel4.Name = "uiMarkLabel4";
            uiMarkLabel4.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel4.Size = new Size(168, 23);
            uiMarkLabel4.TabIndex = 17;
            uiMarkLabel4.Text = "分割点";
            uiMarkLabel4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiCbpillarcolor
            // 
            uiCbpillarcolor.DataSource = null;
            uiCbpillarcolor.FillColor = Color.White;
            uiCbpillarcolor.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiCbpillarcolor.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiCbpillarcolor.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiCbpillarcolor.Location = new Point(420, 238);
            uiCbpillarcolor.Margin = new Padding(4, 5, 4, 5);
            uiCbpillarcolor.MinimumSize = new Size(63, 0);
            uiCbpillarcolor.Name = "uiCbpillarcolor";
            uiCbpillarcolor.Padding = new Padding(0, 0, 30, 2);
            uiCbpillarcolor.Size = new Size(119, 29);
            uiCbpillarcolor.SymbolSize = 24;
            uiCbpillarcolor.TabIndex = 15;
            uiCbpillarcolor.Text = "線⾊番号";
            uiCbpillarcolor.TextAlignment = ContentAlignment.MiddleLeft;
            uiCbpillarcolor.Watermark = "";
            // 
            // uiMarkLabel3
            // 
            uiMarkLabel3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel3.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel3.Location = new Point(84, 238);
            uiMarkLabel3.Name = "uiMarkLabel3";
            uiMarkLabel3.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel3.Size = new Size(168, 23);
            uiMarkLabel3.TabIndex = 16;
            uiMarkLabel3.Text = "柱識別";
            uiMarkLabel3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiCbTextColor
            // 
            uiCbTextColor.DataSource = null;
            uiCbTextColor.FillColor = Color.White;
            uiCbTextColor.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiCbTextColor.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiCbTextColor.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiCbTextColor.Location = new Point(420, 170);
            uiCbTextColor.Margin = new Padding(4, 5, 4, 5);
            uiCbTextColor.MinimumSize = new Size(63, 0);
            uiCbTextColor.Name = "uiCbTextColor";
            uiCbTextColor.Padding = new Padding(0, 0, 30, 2);
            uiCbTextColor.Size = new Size(119, 29);
            uiCbTextColor.SymbolSize = 24;
            uiCbTextColor.TabIndex = 13;
            uiCbTextColor.TextAlignment = ContentAlignment.MiddleLeft;
            uiCbTextColor.Watermark = "";
            // 
            // uiMarkLabel2
            // 
            uiMarkLabel2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel2.Location = new Point(84, 176);
            uiMarkLabel2.Name = "uiMarkLabel2";
            uiMarkLabel2.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel2.Size = new Size(168, 23);
            uiMarkLabel2.TabIndex = 14;
            uiMarkLabel2.Text = "色に注釈を付ける";
            uiMarkLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiCbBeamcolor
            // 
            uiCbBeamcolor.DataSource = null;
            uiCbBeamcolor.FillColor = Color.White;
            uiCbBeamcolor.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiCbBeamcolor.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiCbBeamcolor.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiCbBeamcolor.Location = new Point(420, 103);
            uiCbBeamcolor.Margin = new Padding(4, 5, 4, 5);
            uiCbBeamcolor.MinimumSize = new Size(63, 0);
            uiCbBeamcolor.Name = "uiCbBeamcolor";
            uiCbBeamcolor.Padding = new Padding(0, 0, 30, 2);
            uiCbBeamcolor.Size = new Size(119, 29);
            uiCbBeamcolor.SymbolSize = 24;
            uiCbBeamcolor.TabIndex = 12;
            uiCbBeamcolor.TextAlignment = ContentAlignment.MiddleLeft;
            uiCbBeamcolor.Watermark = "";
            // 
            // uiMarkLabel1
            // 
            uiMarkLabel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel1.Location = new Point(84, 109);
            uiMarkLabel1.Name = "uiMarkLabel1";
            uiMarkLabel1.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel1.Size = new Size(168, 23);
            uiMarkLabel1.TabIndex = 11;
            uiMarkLabel1.Text = "梁ペイントカラー";
            uiMarkLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel6
            // 
            uiMarkLabel6.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel6.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel6.Location = new Point(84, 53);
            uiMarkLabel6.Name = "uiMarkLabel6";
            uiMarkLabel6.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel6.Size = new Size(168, 23);
            uiMarkLabel6.TabIndex = 23;
            uiMarkLabel6.Text = "階";
            uiMarkLabel6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiTextBox1
            // 
            uiTextBox1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiTextBox1.Location = new Point(290, 47);
            uiTextBox1.Margin = new Padding(4, 5, 4, 5);
            uiTextBox1.MinimumSize = new Size(1, 16);
            uiTextBox1.Name = "uiTextBox1";
            uiTextBox1.Padding = new Padding(5);
            uiTextBox1.ShowText = false;
            uiTextBox1.Size = new Size(249, 29);
            uiTextBox1.TabIndex = 24;
            uiTextBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiTextBox1.Watermark = "";
            // 
            // uiMarkLabel7
            // 
            uiMarkLabel7.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel7.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel7.Location = new Point(84, 420);
            uiMarkLabel7.Name = "uiMarkLabel7";
            uiMarkLabel7.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel7.Size = new Size(168, 23);
            uiMarkLabel7.TabIndex = 25;
            uiMarkLabel7.Text = "梁モデル";
            uiMarkLabel7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiComboBox3
            // 
            uiComboBox3.DataSource = null;
            uiComboBox3.FillColor = Color.White;
            uiComboBox3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox3.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiComboBox3.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiComboBox3.Location = new Point(367, 414);
            uiComboBox3.Margin = new Padding(4, 5, 4, 5);
            uiComboBox3.MinimumSize = new Size(63, 0);
            uiComboBox3.Name = "uiComboBox3";
            uiComboBox3.Padding = new Padding(0, 0, 30, 2);
            uiComboBox3.Size = new Size(172, 29);
            uiComboBox3.SymbolSize = 24;
            uiComboBox3.TabIndex = 14;
            uiComboBox3.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox3.Watermark = "";
            // 
            // uiMarkLabel8
            // 
            uiMarkLabel8.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel8.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel8.Location = new Point(84, 470);
            uiMarkLabel8.Name = "uiMarkLabel8";
            uiMarkLabel8.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel8.Size = new Size(168, 23);
            uiMarkLabel8.TabIndex = 26;
            uiMarkLabel8.Text = "下段柱";
            uiMarkLabel8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uidownpillarcolor
            // 
            uidownpillarcolor.DataSource = null;
            uidownpillarcolor.FillColor = Color.White;
            uidownpillarcolor.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uidownpillarcolor.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uidownpillarcolor.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uidownpillarcolor.Location = new Point(420, 464);
            uidownpillarcolor.Margin = new Padding(4, 5, 4, 5);
            uidownpillarcolor.MinimumSize = new Size(63, 0);
            uidownpillarcolor.Name = "uidownpillarcolor";
            uidownpillarcolor.Padding = new Padding(0, 0, 30, 2);
            uidownpillarcolor.Size = new Size(119, 29);
            uidownpillarcolor.SymbolSize = 24;
            uidownpillarcolor.TabIndex = 13;
            uidownpillarcolor.TextAlignment = ContentAlignment.MiddleLeft;
            uidownpillarcolor.Watermark = "";
            // 
            // uiMarkLabel9
            // 
            uiMarkLabel9.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel9.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel9.Location = new Point(84, 516);
            uiMarkLabel9.Name = "uiMarkLabel9";
            uiMarkLabel9.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel9.Size = new Size(168, 23);
            uiMarkLabel9.TabIndex = 27;
            uiMarkLabel9.Text = "識別範囲エラー";
            uiMarkLabel9.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiDoubleUpDown1
            // 
            uiDoubleUpDown1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiDoubleUpDown1.Location = new Point(420, 510);
            uiDoubleUpDown1.Margin = new Padding(4, 5, 4, 5);
            uiDoubleUpDown1.Maximum = 150D;
            uiDoubleUpDown1.Minimum = -100D;
            uiDoubleUpDown1.MinimumSize = new Size(100, 0);
            uiDoubleUpDown1.Name = "uiDoubleUpDown1";
            uiDoubleUpDown1.ShowText = false;
            uiDoubleUpDown1.Size = new Size(119, 29);
            uiDoubleUpDown1.Step = 50D;
            uiDoubleUpDown1.TabIndex = 28;
            uiDoubleUpDown1.Text = "uiDoubleUpDown1";
            uiDoubleUpDown1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiMarkLabel10
            // 
            uiMarkLabel10.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel10.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel10.Location = new Point(84, 557);
            uiMarkLabel10.Name = "uiMarkLabel10";
            uiMarkLabel10.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel10.Size = new Size(168, 23);
            uiMarkLabel10.TabIndex = 29;
            uiMarkLabel10.Text = "リンク";
            uiMarkLabel10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiComboBox4
            // 
            uiComboBox4.DataSource = null;
            uiComboBox4.FillColor = Color.White;
            uiComboBox4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox4.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiComboBox4.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiComboBox4.Location = new Point(420, 557);
            uiComboBox4.Margin = new Padding(4, 5, 4, 5);
            uiComboBox4.MinimumSize = new Size(63, 0);
            uiComboBox4.Name = "uiComboBox4";
            uiComboBox4.Padding = new Padding(0, 0, 30, 2);
            uiComboBox4.Size = new Size(119, 29);
            uiComboBox4.SymbolSize = 24;
            uiComboBox4.TabIndex = 13;
            uiComboBox4.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox4.Watermark = "";
            // 
            // CustomerDesignSetting
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(622, 656);
            Controls.Add(uiComboBox4);
            Controls.Add(uiMarkLabel10);
            Controls.Add(uiDoubleUpDown1);
            Controls.Add(uiMarkLabel9);
            Controls.Add(uidownpillarcolor);
            Controls.Add(uiMarkLabel8);
            Controls.Add(uiComboBox3);
            Controls.Add(uiMarkLabel7);
            Controls.Add(uiTextBox1);
            Controls.Add(uiMarkLabel6);
            Controls.Add(uiNearSpliteMax);
            Controls.Add(uiMarkLabel5);
            Controls.Add(uiComboBox2);
            Controls.Add(uiComboBox1);
            Controls.Add(uiCbSplitcolor);
            Controls.Add(uiMarkLabel4);
            Controls.Add(uiCbpillarcolor);
            Controls.Add(uiMarkLabel3);
            Controls.Add(uiCbTextColor);
            Controls.Add(uiMarkLabel2);
            Controls.Add(uiCbBeamcolor);
            Controls.Add(uiMarkLabel1);
            Controls.Add(panel1);
            Name = "CustomerDesignSetting";
            Text = "構成を解析する";
            Load += CustomerDesignSetting_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UIDoubleUpDown uiNearSpliteMax;
        private Sunny.UI.UIMarkLabel uiMarkLabel5;
        private Sunny.UI.UIComboBox uiComboBox2;
        private Sunny.UI.UIComboBox uiComboBox1;
        private Sunny.UI.UIComboBox uiCbSplitcolor;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UIComboBox uiCbpillarcolor;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIComboBox uiCbTextColor;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIComboBox uiCbBeamcolor;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIMarkLabel uiMarkLabel6;
        private Sunny.UI.UITextBox uiTextBox1;
        private Sunny.UI.UIMarkLabel uiMarkLabel7;
        private Sunny.UI.UIComboBox uiComboBox3;
        private Sunny.UI.UIMarkLabel uiMarkLabel8;
        private Sunny.UI.UIComboBox uidownpillarcolor;
        private Sunny.UI.UIMarkLabel uiMarkLabel9;
        private Sunny.UI.UIDoubleUpDown uiDoubleUpDown1;
        private Sunny.UI.UIMarkLabel uiMarkLabel10;
        private Sunny.UI.UIComboBox uiComboBox4;
    }
}