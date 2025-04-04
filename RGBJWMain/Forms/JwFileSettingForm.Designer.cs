namespace RGBJWMain.Forms
{
    partial class JwFileSettingForm
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
            uiPanel1 = new Sunny.UI.UIPanel();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            uiCbBeamcolor = new Sunny.UI.UIComboBox();
            uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            uiCbTextColor = new Sunny.UI.UIComboBox();
            uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            uiCbpillarcolor = new Sunny.UI.UIComboBox();
            uiMarkLabel4 = new Sunny.UI.UIMarkLabel();
            uiCbSplitcolor = new Sunny.UI.UIComboBox();
            uiComboBox1 = new Sunny.UI.UIComboBox();
            uiComboBox2 = new Sunny.UI.UIComboBox();
            uiMarkLabel5 = new Sunny.UI.UIMarkLabel();
            uiNearSpliteMax = new Sunny.UI.UIDoubleUpDown();
            uiPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // uiPanel1
            // 
            uiPanel1.Controls.Add(uiSymbolButton1);
            uiPanel1.Dock = DockStyle.Bottom;
            uiPanel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiPanel1.Location = new Point(0, 564);
            uiPanel1.Margin = new Padding(4, 5, 4, 5);
            uiPanel1.MinimumSize = new Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.Size = new Size(557, 63);
            uiPanel1.TabIndex = 0;
            uiPanel1.Text = null;
            uiPanel1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(369, 14);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(166, 35);
            uiSymbolButton1.TabIndex = 0;
            uiSymbolButton1.Text = "セーブ";
            uiSymbolButton1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // uiMarkLabel1
            // 
            uiMarkLabel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel1.Location = new Point(33, 87);
            uiMarkLabel1.Name = "uiMarkLabel1";
            uiMarkLabel1.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel1.Size = new Size(168, 23);
            uiMarkLabel1.TabIndex = 1;
            uiMarkLabel1.Text = "梁ペイントカラー";
            uiMarkLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiCbBeamcolor
            // 
            uiCbBeamcolor.DataSource = null;
            uiCbBeamcolor.FillColor = Color.White;
            uiCbBeamcolor.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiCbBeamcolor.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiCbBeamcolor.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiCbBeamcolor.Location = new Point(369, 81);
            uiCbBeamcolor.Margin = new Padding(4, 5, 4, 5);
            uiCbBeamcolor.MinimumSize = new Size(63, 0);
            uiCbBeamcolor.Name = "uiCbBeamcolor";
            uiCbBeamcolor.Padding = new Padding(0, 0, 30, 2);
            uiCbBeamcolor.Size = new Size(119, 29);
            uiCbBeamcolor.SymbolSize = 24;
            uiCbBeamcolor.TabIndex = 2;
            uiCbBeamcolor.TextAlignment = ContentAlignment.MiddleLeft;
            uiCbBeamcolor.Watermark = "";
            // 
            // uiMarkLabel2
            // 
            uiMarkLabel2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel2.Location = new Point(33, 154);
            uiMarkLabel2.Name = "uiMarkLabel2";
            uiMarkLabel2.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel2.Size = new Size(168, 23);
            uiMarkLabel2.TabIndex = 3;
            uiMarkLabel2.Text = "色に注釈を付ける";
            uiMarkLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiCbTextColor
            // 
            uiCbTextColor.DataSource = null;
            uiCbTextColor.FillColor = Color.White;
            uiCbTextColor.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiCbTextColor.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiCbTextColor.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiCbTextColor.Location = new Point(369, 148);
            uiCbTextColor.Margin = new Padding(4, 5, 4, 5);
            uiCbTextColor.MinimumSize = new Size(63, 0);
            uiCbTextColor.Name = "uiCbTextColor";
            uiCbTextColor.Padding = new Padding(0, 0, 30, 2);
            uiCbTextColor.Size = new Size(119, 29);
            uiCbTextColor.SymbolSize = 24;
            uiCbTextColor.TabIndex = 3;
            uiCbTextColor.TextAlignment = ContentAlignment.MiddleLeft;
            uiCbTextColor.Watermark = "";
            // 
            // uiMarkLabel3
            // 
            uiMarkLabel3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel3.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel3.Location = new Point(33, 216);
            uiMarkLabel3.Name = "uiMarkLabel3";
            uiMarkLabel3.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel3.Size = new Size(168, 23);
            uiMarkLabel3.TabIndex = 4;
            uiMarkLabel3.Text = "柱識別";
            uiMarkLabel3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiCbpillarcolor
            // 
            uiCbpillarcolor.DataSource = null;
            uiCbpillarcolor.FillColor = Color.White;
            uiCbpillarcolor.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiCbpillarcolor.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiCbpillarcolor.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiCbpillarcolor.Location = new Point(369, 216);
            uiCbpillarcolor.Margin = new Padding(4, 5, 4, 5);
            uiCbpillarcolor.MinimumSize = new Size(63, 0);
            uiCbpillarcolor.Name = "uiCbpillarcolor";
            uiCbpillarcolor.Padding = new Padding(0, 0, 30, 2);
            uiCbpillarcolor.Size = new Size(119, 29);
            uiCbpillarcolor.SymbolSize = 24;
            uiCbpillarcolor.TabIndex = 4;
            uiCbpillarcolor.Text = "線⾊番号";
            uiCbpillarcolor.TextAlignment = ContentAlignment.MiddleLeft;
            uiCbpillarcolor.Watermark = "";
            // 
            // uiMarkLabel4
            // 
            uiMarkLabel4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel4.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel4.Location = new Point(33, 277);
            uiMarkLabel4.Name = "uiMarkLabel4";
            uiMarkLabel4.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel4.Size = new Size(168, 23);
            uiMarkLabel4.TabIndex = 5;
            uiMarkLabel4.Text = "分割点";
            uiMarkLabel4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiCbSplitcolor
            // 
            uiCbSplitcolor.DataSource = null;
            uiCbSplitcolor.FillColor = Color.White;
            uiCbSplitcolor.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiCbSplitcolor.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiCbSplitcolor.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiCbSplitcolor.Location = new Point(369, 277);
            uiCbSplitcolor.Margin = new Padding(4, 5, 4, 5);
            uiCbSplitcolor.MinimumSize = new Size(63, 0);
            uiCbSplitcolor.Name = "uiCbSplitcolor";
            uiCbSplitcolor.Padding = new Padding(0, 0, 30, 2);
            uiCbSplitcolor.Size = new Size(119, 29);
            uiCbSplitcolor.SymbolSize = 24;
            uiCbSplitcolor.TabIndex = 6;
            uiCbSplitcolor.Text = "線⾊番号";
            uiCbSplitcolor.TextAlignment = ContentAlignment.MiddleLeft;
            uiCbSplitcolor.Watermark = "";
            // 
            // uiComboBox1
            // 
            uiComboBox1.DataSource = null;
            uiComboBox1.FillColor = Color.White;
            uiComboBox1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox1.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiComboBox1.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiComboBox1.Location = new Point(239, 277);
            uiComboBox1.Margin = new Padding(4, 5, 4, 5);
            uiComboBox1.MinimumSize = new Size(63, 0);
            uiComboBox1.Name = "uiComboBox1";
            uiComboBox1.Padding = new Padding(0, 0, 30, 2);
            uiComboBox1.Size = new Size(111, 29);
            uiComboBox1.SymbolSize = 24;
            uiComboBox1.TabIndex = 7;
            uiComboBox1.Text = "線種番号";
            uiComboBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox1.Watermark = "";
            // 
            // uiComboBox2
            // 
            uiComboBox2.DataSource = null;
            uiComboBox2.FillColor = Color.White;
            uiComboBox2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox2.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiComboBox2.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiComboBox2.Location = new Point(239, 216);
            uiComboBox2.Margin = new Padding(4, 5, 4, 5);
            uiComboBox2.MinimumSize = new Size(63, 0);
            uiComboBox2.Name = "uiComboBox2";
            uiComboBox2.Padding = new Padding(0, 0, 30, 2);
            uiComboBox2.Size = new Size(111, 29);
            uiComboBox2.SymbolSize = 24;
            uiComboBox2.TabIndex = 8;
            uiComboBox2.Text = "線種番号";
            uiComboBox2.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox2.Watermark = "";
            // 
            // uiMarkLabel5
            // 
            uiMarkLabel5.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel5.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel5.Location = new Point(33, 341);
            uiMarkLabel5.Name = "uiMarkLabel5";
            uiMarkLabel5.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel5.Size = new Size(168, 23);
            uiMarkLabel5.TabIndex = 9;
            uiMarkLabel5.Text = "分割点";
            uiMarkLabel5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiNearSpliteMax
            // 
            uiNearSpliteMax.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiNearSpliteMax.Location = new Point(369, 335);
            uiNearSpliteMax.Margin = new Padding(4, 5, 4, 5);
            uiNearSpliteMax.Maximum = 100D;
            uiNearSpliteMax.Minimum = 0D;
            uiNearSpliteMax.MinimumSize = new Size(100, 0);
            uiNearSpliteMax.Name = "uiNearSpliteMax";
            uiNearSpliteMax.ShowText = false;
            uiNearSpliteMax.Size = new Size(119, 29);
            uiNearSpliteMax.Step = 5D;
            uiNearSpliteMax.TabIndex = 10;
            uiNearSpliteMax.Text = "uiDoubleUpDown1";
            uiNearSpliteMax.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // JwFileSettingForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(557, 627);
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
            Controls.Add(uiPanel1);
            Name = "JwFileSettingForm";
            Text = "JwFileSettingForm";
            ZoomScaleRect = new Rectangle(15, 15, 800, 450);
            Load += JwFileSettingForm_Load;
            uiPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIComboBox uiCbBeamcolor;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UIComboBox uiCbTextColor;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UIComboBox uiCbpillarcolor;
        private Sunny.UI.UIMarkLabel uiMarkLabel4;
        private Sunny.UI.UIComboBox uiCbSplitcolor;
        private Sunny.UI.UIComboBox uiComboBox1;
        private Sunny.UI.UIComboBox uiComboBox2;
        private Sunny.UI.UIMarkLabel uiMarkLabel5;
        private Sunny.UI.UIDoubleUpDown uiNearSpliteMax;
    }
}