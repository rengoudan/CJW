namespace RGBJWMain
{
    partial class SettingForm
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
            uiLabel1 = new Sunny.UI.UILabel();
            uiComboBox1 = new Sunny.UI.UIComboBox();
            uiComboBox2 = new Sunny.UI.UIComboBox();
            uiLabel2 = new Sunny.UI.UILabel();
            uiLine1 = new Sunny.UI.UILine();
            uiDataGridView1 = new Sunny.UI.UIDataGridView();
            uiLabel3 = new Sunny.UI.UILabel();
            pnlBtm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiDataGridView1).BeginInit();
            SuspendLayout();
            // 
            // pnlBtm
            // 
            pnlBtm.Location = new Point(1, 389);
            pnlBtm.Size = new Size(517, 55);
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(389, 12);
            // 
            // btnOK
            // 
            btnOK.Location = new Point(274, 12);
            // 
            // uiLabel1
            // 
            uiLabel1.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel1.Location = new Point(138, 86);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(156, 23);
            uiLabel1.TabIndex = 2;
            uiLabel1.Text = "beamParseColor：";
            uiLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiComboBox1
            // 
            uiComboBox1.DataSource = null;
            uiComboBox1.FillColor = Color.White;
            uiComboBox1.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox1.Items.AddRange(new object[] { "lc105", "lc2", "lc3", "lc4", "lc5", "lc6", "lc7", "lc8", "lc9", "lc10", "lc11" });
            uiComboBox1.Location = new Point(301, 80);
            uiComboBox1.Margin = new Padding(4, 5, 4, 5);
            uiComboBox1.MinimumSize = new Size(63, 0);
            uiComboBox1.Name = "uiComboBox1";
            uiComboBox1.Padding = new Padding(0, 0, 30, 2);
            uiComboBox1.Size = new Size(151, 29);
            uiComboBox1.TabIndex = 3;
            uiComboBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox1.Watermark = "";
            // 
            // uiComboBox2
            // 
            uiComboBox2.DataSource = null;
            uiComboBox2.FillColor = Color.White;
            uiComboBox2.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox2.Items.AddRange(new object[] { "Triangular", "Square" });
            uiComboBox2.Location = new Point(301, 140);
            uiComboBox2.Margin = new Padding(4, 5, 4, 5);
            uiComboBox2.MinimumSize = new Size(63, 0);
            uiComboBox2.Name = "uiComboBox2";
            uiComboBox2.Padding = new Padding(0, 0, 30, 2);
            uiComboBox2.Size = new Size(151, 29);
            uiComboBox2.TabIndex = 4;
            uiComboBox2.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox2.Watermark = "";
            // 
            // uiLabel2
            // 
            uiLabel2.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel2.Location = new Point(24, 140);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(270, 23);
            uiLabel2.TabIndex = 5;
            uiLabel2.Text = "beamOverLengthReinforcement：";
            uiLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLine1
            // 
            uiLine1.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLine1.Location = new Point(24, 196);
            uiLine1.MinimumSize = new Size(1, 1);
            uiLine1.Name = "uiLine1";
            uiLine1.Size = new Size(466, 29);
            uiLine1.TabIndex = 6;
            uiLine1.Text = "ラベル付けの習慣";
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(243, 249, 255);
            uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            uiDataGridView1.BackgroundColor = Color.FromArgb(243, 249, 255);
            uiDataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            uiDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(220, 236, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            uiDataGridView1.EnableHeadersVisualStyles = false;
            uiDataGridView1.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiDataGridView1.GridColor = Color.FromArgb(104, 173, 255);
            uiDataGridView1.Location = new Point(24, 231);
            uiDataGridView1.Name = "uiDataGridView1";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(243, 249, 255);
            dataGridViewCellStyle4.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(220, 236, 255);
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(48, 48, 48);
            uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            uiDataGridView1.RowTemplate.Height = 25;
            uiDataGridView1.ScrollBarRectColor = Color.FromArgb(80, 160, 255);
            uiDataGridView1.SelectedIndex = -1;
            uiDataGridView1.Size = new Size(466, 150);
            uiDataGridView1.TabIndex = 7;
            // 
            // uiLabel3
            // 
            uiLabel3.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel3.Location = new Point(24, 287);
            uiLabel3.Name = "uiLabel3";
            uiLabel3.Size = new Size(466, 23);
            uiLabel3.TabIndex = 8;
            uiLabel3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(519, 447);
            Controls.Add(uiLabel3);
            Controls.Add(uiDataGridView1);
            Controls.Add(uiLine1);
            Controls.Add(uiLabel2);
            Controls.Add(uiComboBox2);
            Controls.Add(uiComboBox1);
            Controls.Add(uiLabel1);
            Name = "SettingForm";
            Text = "SettingForm";
            ZoomScaleRect = new Rectangle(15, 15, 800, 450);
            Load += SettingForm_Load;
            Controls.SetChildIndex(pnlBtm, 0);
            Controls.SetChildIndex(uiLabel1, 0);
            Controls.SetChildIndex(uiComboBox1, 0);
            Controls.SetChildIndex(uiComboBox2, 0);
            Controls.SetChildIndex(uiLabel2, 0);
            Controls.SetChildIndex(uiLine1, 0);
            Controls.SetChildIndex(uiDataGridView1, 0);
            Controls.SetChildIndex(uiLabel3, 0);
            pnlBtm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiDataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIComboBox uiComboBox1;
        private Sunny.UI.UIComboBox uiComboBox2;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILine uiLine1;
        private Sunny.UI.UIDataGridView uiDataGridView1;
        private Sunny.UI.UILabel uiLabel3;
    }
}