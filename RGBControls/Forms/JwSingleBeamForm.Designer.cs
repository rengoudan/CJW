namespace RGBJWMain.Forms
{
    partial class JwSingleBeamForm
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
            panel3 = new Panel();
            beamSingleShow1 = new RGBJWMain.Controls.BeamSingleShow();
            panel2 = new Panel();
            uiComboBox3 = new Sunny.UI.UIComboBox();
            uiMarkLabel7 = new Sunny.UI.UIMarkLabel();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            select1 = new AntdUI.Select();
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
            panel1.Location = new Point(0, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(980, 415);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(beamSingleShow1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 57);
            panel3.Name = "panel3";
            panel3.Size = new Size(980, 358);
            panel3.TabIndex = 1;
            // 
            // beamSingleShow1
            // 
            beamSingleShow1.BackColor = Color.Black;
            beamSingleShow1.Dock = DockStyle.Fill;
            beamSingleShow1.JwDrawShape = null;
            beamSingleShow1.Location = new Point(0, 0);
            beamSingleShow1.Name = "beamSingleShow1";
            beamSingleShow1.Size = new Size(980, 358);
            beamSingleShow1.TabIndex = 0;
            beamSingleShow1.Text = "beamSingleShow1";
            // 
            // panel2
            // 
            panel2.Controls.Add(select1);
            panel2.Controls.Add(uiComboBox3);
            panel2.Controls.Add(uiMarkLabel7);
            panel2.Controls.Add(uiSymbolButton1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(980, 57);
            panel2.TabIndex = 0;
            // 
            // uiComboBox3
            // 
            uiComboBox3.DataSource = null;
            uiComboBox3.FillColor = Color.White;
            uiComboBox3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox3.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiComboBox3.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiComboBox3.Location = new Point(273, 9);
            uiComboBox3.Margin = new Padding(4, 5, 4, 5);
            uiComboBox3.MinimumSize = new Size(63, 0);
            uiComboBox3.Name = "uiComboBox3";
            uiComboBox3.Padding = new Padding(0, 0, 30, 2);
            uiComboBox3.Size = new Size(172, 29);
            uiComboBox3.SymbolSize = 24;
            uiComboBox3.TabIndex = 15;
            uiComboBox3.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox3.Watermark = "";
            // 
            // uiMarkLabel7
            // 
            uiMarkLabel7.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel7.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel7.Location = new Point(158, 10);
            uiMarkLabel7.Name = "uiMarkLabel7";
            uiMarkLabel7.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel7.Size = new Size(108, 23);
            uiMarkLabel7.TabIndex = 26;
            uiMarkLabel7.Text = "梁モデル:";
            uiMarkLabel7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(3, 3);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(107, 35);
            uiSymbolButton1.Symbol = 558052;
            uiSymbolButton1.TabIndex = 0;
            uiSymbolButton1.Text = "輸出JWW";
            uiSymbolButton1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // select1
            // 
            select1.Items.AddRange(new object[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" });
            select1.Location = new Point(477, 0);
            select1.Name = "select1";
            select1.Size = new Size(160, 51);
            select1.TabIndex = 27;
            select1.Text = "工区選択";
            // 
            // JwSingleBeamForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(980, 450);
            Controls.Add(panel1);
            Name = "JwSingleBeamForm";
            Text = "JwSingleBeamForm";
            ZoomScaleRect = new Rectangle(15, 15, 800, 450);
            Load += JwSingleBeamForm_Load;
            Shown += JwSingleBeamForm_Shown;
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private Panel panel2;
        private Controls.BeamSingleShow beamSingleShow1;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Sunny.UI.UIMarkLabel uiMarkLabel7;
        private Sunny.UI.UIComboBox uiComboBox3;
        private AntdUI.Select select1;
    }
}