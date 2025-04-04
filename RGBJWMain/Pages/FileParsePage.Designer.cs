namespace RGBJWMain.Pages
{
    partial class FileParsePage
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
            uiLine1 = new Sunny.UI.UILine();
            panel1 = new Panel();
            panel3 = new Panel();
            uiSplitContainer1 = new Sunny.UI.UISplitContainer();
            uiTextBox1 = new Sunny.UI.UITextBox();
            jwCanvasControl1 = new Controls.JwCanvasControl();
            panel2 = new Panel();
            uiButton1 = new Sunny.UI.UIButton();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            uiSplitContainer1.BeginInit();
            uiSplitContainer1.Panel1.SuspendLayout();
            uiSplitContainer1.Panel2.SuspendLayout();
            uiSplitContainer1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // uiLine1
            // 
            uiLine1.BackColor = Color.Transparent;
            uiLine1.Dock = DockStyle.Top;
            uiLine1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLine1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLine1.Location = new Point(0, 0);
            uiLine1.MinimumSize = new Size(1, 1);
            uiLine1.Name = "uiLine1";
            uiLine1.Size = new Size(800, 29);
            uiLine1.TabIndex = 0;
            uiLine1.Text = "JW-ファイル属性";
            uiLine1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 29);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 421);
            panel1.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(uiSplitContainer1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 50);
            panel3.Name = "panel3";
            panel3.Size = new Size(800, 371);
            panel3.TabIndex = 1;
            // 
            // uiSplitContainer1
            // 
            uiSplitContainer1.BorderStyle = BorderStyle.FixedSingle;
            uiSplitContainer1.Dock = DockStyle.Fill;
            uiSplitContainer1.Location = new Point(0, 0);
            uiSplitContainer1.MinimumSize = new Size(20, 20);
            uiSplitContainer1.Name = "uiSplitContainer1";
            // 
            // uiSplitContainer1.Panel1
            // 
            uiSplitContainer1.Panel1.Controls.Add(uiTextBox1);
            // 
            // uiSplitContainer1.Panel2
            // 
            uiSplitContainer1.Panel2.Controls.Add(jwCanvasControl1);
            uiSplitContainer1.Size = new Size(800, 371);
            uiSplitContainer1.SplitterDistance = 150;
            uiSplitContainer1.SplitterWidth = 11;
            uiSplitContainer1.TabIndex = 1;
            uiSplitContainer1.SplitterMoved += uiSplitContainer1_SplitterMoved;
            uiSplitContainer1.RightToLeftChanged += uiSplitContainer1_RightToLeftChanged;
            // 
            // uiTextBox1
            // 
            uiTextBox1.Dock = DockStyle.Fill;
            uiTextBox1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiTextBox1.Location = new Point(0, 0);
            uiTextBox1.Margin = new Padding(4, 5, 4, 5);
            uiTextBox1.MinimumSize = new Size(1, 16);
            uiTextBox1.Multiline = true;
            uiTextBox1.Name = "uiTextBox1";
            uiTextBox1.Padding = new Padding(5);
            uiTextBox1.ShowScrollBar = true;
            uiTextBox1.ShowText = false;
            uiTextBox1.Size = new Size(148, 369);
            uiTextBox1.TabIndex = 0;
            uiTextBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiTextBox1.Watermark = "";
            // 
            // jwCanvasControl1
            // 
            jwCanvasControl1.BeamSelected = false;
            jwCanvasControl1.CanvasDraw = null;
            jwCanvasControl1.Dock = DockStyle.Fill;
            jwCanvasControl1.Location = new Point(0, 0);
            jwCanvasControl1.Name = "jwCanvasControl1";
            jwCanvasControl1.SelectBeamEvent = null;
            jwCanvasControl1.SelectedBeam = null;
            jwCanvasControl1.Size = new Size(637, 369);
            jwCanvasControl1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(uiButton1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 50);
            panel2.TabIndex = 0;
            // 
            // uiButton1
            // 
            uiButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiButton1.Location = new Point(12, 6);
            uiButton1.MinimumSize = new Size(1, 1);
            uiButton1.Name = "uiButton1";
            uiButton1.Size = new Size(135, 35);
            uiButton1.TabIndex = 0;
            uiButton1.Text = "ファイルを開く";
            uiButton1.Click += uiButton1_Click;
            // 
            // FileParsePage
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(uiLine1);
            Name = "FileParsePage";
            Text = "JWW文件适配";
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            uiSplitContainer1.Panel1.ResumeLayout(false);
            uiSplitContainer1.Panel2.ResumeLayout(false);
            uiSplitContainer1.EndInit();
            uiSplitContainer1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UILine uiLine1;
        private Panel panel1;
        private Panel panel2;
        private Sunny.UI.UIButton uiButton1;
        private Panel panel3;
        private Controls.JwCanvasControl jwCanvasControl1;
        private Sunny.UI.UISplitContainer uiSplitContainer1;
        private Sunny.UI.UITextBox uiTextBox1;
    }
}