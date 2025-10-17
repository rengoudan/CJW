namespace RGBJWMain.Forms
{
    partial class FileParseForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            panel1 = new Panel();
            uiSplitContainer1 = new Sunny.UI.UISplitContainer();
            panel3 = new Panel();
            uiDataGridView1 = new Sunny.UI.UIDataGridView();
            uiTextBox1 = new Sunny.UI.UITextBox();
            jwCanvasControl1 = new Controls.JwCanvasControl();
            panel2 = new Panel();
            uiSymbolButton2 = new Sunny.UI.UISymbolButton();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            panel1.SuspendLayout();
            uiSplitContainer1.BeginInit();
            uiSplitContainer1.Panel1.SuspendLayout();
            uiSplitContainer1.Panel2.SuspendLayout();
            uiSplitContainer1.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiDataGridView1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(uiSplitContainer1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1040, 1035);
            panel1.TabIndex = 0;
            // 
            // uiSplitContainer1
            // 
            uiSplitContainer1.BorderStyle = BorderStyle.FixedSingle;
            uiSplitContainer1.Dock = DockStyle.Fill;
            uiSplitContainer1.Location = new Point(0, 59);
            uiSplitContainer1.MinimumSize = new Size(20, 20);
            uiSplitContainer1.Name = "uiSplitContainer1";
            // 
            // uiSplitContainer1.Panel1
            // 
            uiSplitContainer1.Panel1.Controls.Add(panel3);
            uiSplitContainer1.Panel1.Controls.Add(uiTextBox1);
            // 
            // uiSplitContainer1.Panel2
            // 
            uiSplitContainer1.Panel2.Controls.Add(jwCanvasControl1);
            uiSplitContainer1.Size = new Size(1040, 976);
            uiSplitContainer1.SplitterDistance = 239;
            uiSplitContainer1.SplitterWidth = 11;
            uiSplitContainer1.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(uiDataGridView1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 650);
            panel3.Name = "panel3";
            panel3.Size = new Size(237, 324);
            panel3.TabIndex = 1;
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(235, 243, 255);
            uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            uiDataGridView1.BackgroundColor = Color.White;
            uiDataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            uiDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            uiDataGridView1.Dock = DockStyle.Fill;
            uiDataGridView1.EnableHeadersVisualStyles = false;
            uiDataGridView1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiDataGridView1.GridColor = Color.FromArgb(80, 160, 255);
            uiDataGridView1.Location = new Point(0, 0);
            uiDataGridView1.Name = "uiDataGridView1";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            uiDataGridView1.RowTemplate.Height = 25;
            uiDataGridView1.SelectedIndex = -1;
            uiDataGridView1.Size = new Size(237, 324);
            uiDataGridView1.StripeOddColor = Color.FromArgb(235, 243, 255);
            uiDataGridView1.TabIndex = 0;
            // 
            // uiTextBox1
            // 
            uiTextBox1.Dock = DockStyle.Top;
            uiTextBox1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiTextBox1.Location = new Point(0, 0);
            uiTextBox1.Margin = new Padding(4, 5, 4, 5);
            uiTextBox1.MinimumSize = new Size(1, 16);
            uiTextBox1.Multiline = true;
            uiTextBox1.Name = "uiTextBox1";
            uiTextBox1.Padding = new Padding(5);
            uiTextBox1.ShowScrollBar = true;
            uiTextBox1.ShowText = false;
            uiTextBox1.Size = new Size(237, 650);
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
            jwCanvasControl1.Size = new Size(788, 974);
            jwCanvasControl1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(uiSymbolButton2);
            panel2.Controls.Add(uiSymbolButton1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1040, 59);
            panel2.TabIndex = 0;
            // 
            // uiSymbolButton2
            // 
            uiSymbolButton2.FillColor = Color.Red;
            uiSymbolButton2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton2.Location = new Point(244, 12);
            uiSymbolButton2.MinimumSize = new Size(1, 1);
            uiSymbolButton2.Name = "uiSymbolButton2";
            uiSymbolButton2.Size = new Size(178, 35);
            uiSymbolButton2.Symbol = 61453;
            uiSymbolButton2.TabIndex = 1;
            uiSymbolButton2.Text = "キャンセル";
            uiSymbolButton2.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton2.Click += uiSymbolButton2_Click;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(12, 12);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(178, 35);
            uiSymbolButton1.Symbol = 557697;
            uiSymbolButton1.TabIndex = 0;
            uiSymbolButton1.Text = "セーブ";
            uiSymbolButton1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // FileParseForm
            // 
            AllowShowTitle = false;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1040, 1035);
            Controls.Add(panel1);
            Name = "FileParseForm";
            Padding = new Padding(0);
            ShowTitle = false;
            Text = "FileParseForm";
            WindowState = FormWindowState.Maximized;
            Load += FileParseForm_Load;
            panel1.ResumeLayout(false);
            uiSplitContainer1.Panel1.ResumeLayout(false);
            uiSplitContainer1.Panel2.ResumeLayout(false);
            uiSplitContainer1.EndInit();
            uiSplitContainer1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiDataGridView1).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Sunny.UI.UISplitContainer uiSplitContainer1;
        private Sunny.UI.UITextBox uiTextBox1;
        private Controls.JwCanvasControl jwCanvasControl1;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Panel panel3;
        private Sunny.UI.UIDataGridView uiDataGridView1;
        private Sunny.UI.UISymbolButton uiSymbolButton2;
    }
}