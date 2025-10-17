namespace RGBJWMain.Pages
{
    partial class JwBudgePage
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            uiSymbolButton2 = new Sunny.UI.UISymbolButton();
            uiSplitContainer1 = new Sunny.UI.UISplitContainer();
            uiDataGridView1 = new Sunny.UI.UIDataGridView();
            projectNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            amountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            creationTimeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            jwBudgetMainDataBindingSource = new BindingSource(components);
            panel4 = new Panel();
            uiDataGridView2 = new Sunny.UI.UIDataGridView();
            jwBudgetSubDatasBindingSource = new BindingSource(components);
            panel3 = new Panel();
            uiSymbolButton3 = new Sunny.UI.UISymbolButton();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            uilbprojectname = new Sunny.UI.UIMarkLabel();
            contextMenuStrip1 = new ContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            budgetItemNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            ModelParm = new DataGridViewTextBoxColumn();
            unitNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            unitPriceDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            numberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            amountDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            jwBudgetMainDataDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            creationTimeDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            uiSplitContainer1.BeginInit();
            uiSplitContainer1.Panel1.SuspendLayout();
            uiSplitContainer1.Panel2.SuspendLayout();
            uiSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiDataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwBudgetMainDataBindingSource).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiDataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwBudgetSubDatasBindingSource).BeginInit();
            panel3.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // uiLine1
            // 
            uiLine1.Text = "予算管理";
            // 
            // panel1
            // 
            panel1.Controls.Add(uiSymbolButton2);
            panel1.Controls.SetChildIndex(uiButton1, 0);
            panel1.Controls.SetChildIndex(uiSymbolButton2, 0);
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(uiSplitContainer1);
            // 
            // uiSymbolButton2
            // 
            uiSymbolButton2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton2.Location = new Point(136, 5);
            uiSymbolButton2.MinimumSize = new Size(1, 1);
            uiSymbolButton2.Name = "uiSymbolButton2";
            uiSymbolButton2.Size = new Size(163, 35);
            uiSymbolButton2.Symbol = 300043;
            uiSymbolButton2.TabIndex = 4;
            uiSymbolButton2.Text = "予算を作成する";
            uiSymbolButton2.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton2.Click += uiSymbolButton2_Click;
            // 
            // uiSplitContainer1
            // 
            uiSplitContainer1.Dock = DockStyle.Fill;
            uiSplitContainer1.Location = new Point(0, 0);
            uiSplitContainer1.MinimumSize = new Size(20, 20);
            uiSplitContainer1.Name = "uiSplitContainer1";
            // 
            // uiSplitContainer1.Panel1
            // 
            uiSplitContainer1.Panel1.Controls.Add(uiDataGridView1);
            uiSplitContainer1.Panel1.RightToLeft = RightToLeft.No;
            // 
            // uiSplitContainer1.Panel2
            // 
            uiSplitContainer1.Panel2.Controls.Add(panel4);
            uiSplitContainer1.Panel2.Controls.Add(panel3);
            uiSplitContainer1.Panel2.RightToLeft = RightToLeft.No;
            uiSplitContainer1.RightToLeft = RightToLeft.No;
            uiSplitContainer1.Size = new Size(798, 376);
            uiSplitContainer1.SplitterDistance = 231;
            uiSplitContainer1.SplitterWidth = 11;
            uiSplitContainer1.TabIndex = 0;
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(235, 243, 255);
            uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            uiDataGridView1.AutoGenerateColumns = false;
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
            uiDataGridView1.ColumnHeadersHeight = 32;
            uiDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            uiDataGridView1.Columns.AddRange(new DataGridViewColumn[] { projectNameDataGridViewTextBoxColumn, amountDataGridViewTextBoxColumn, idDataGridViewTextBoxColumn, creationTimeDataGridViewTextBoxColumn });
            uiDataGridView1.DataSource = jwBudgetMainDataBindingSource;
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
            uiDataGridView1.MultiSelect = false;
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
            uiDataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            uiDataGridView1.Size = new Size(231, 376);
            uiDataGridView1.StripeOddColor = Color.FromArgb(235, 243, 255);
            uiDataGridView1.TabIndex = 0;
            uiDataGridView1.CellMouseDown += uiDataGridView1_CellMouseDown;
            uiDataGridView1.RowHeaderMouseClick += uiDataGridView1_RowHeaderMouseClick;
            // 
            // projectNameDataGridViewTextBoxColumn
            // 
            projectNameDataGridViewTextBoxColumn.DataPropertyName = "ProjectName";
            projectNameDataGridViewTextBoxColumn.HeaderText = "ProjectName";
            projectNameDataGridViewTextBoxColumn.Name = "projectNameDataGridViewTextBoxColumn";
            projectNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            // 
            // idDataGridViewTextBoxColumn
            // 
            idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn.HeaderText = "Id";
            idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            idDataGridViewTextBoxColumn.Visible = false;
            // 
            // creationTimeDataGridViewTextBoxColumn
            // 
            creationTimeDataGridViewTextBoxColumn.DataPropertyName = "CreationTime";
            creationTimeDataGridViewTextBoxColumn.HeaderText = "CreationTime";
            creationTimeDataGridViewTextBoxColumn.Name = "creationTimeDataGridViewTextBoxColumn";
            // 
            // jwBudgetMainDataBindingSource
            // 
            jwBudgetMainDataBindingSource.DataSource = typeof(JwCore.JwBudgetMainData);
            // 
            // panel4
            // 
            panel4.Controls.Add(uiDataGridView2);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 49);
            panel4.Name = "panel4";
            panel4.Size = new Size(556, 327);
            panel4.TabIndex = 2;
            // 
            // uiDataGridView2
            // 
            dataGridViewCellStyle6.BackColor = Color.FromArgb(235, 243, 255);
            uiDataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            uiDataGridView2.AutoGenerateColumns = false;
            uiDataGridView2.BackgroundColor = Color.White;
            uiDataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle7.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            uiDataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            uiDataGridView2.ColumnHeadersHeight = 32;
            uiDataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            uiDataGridView2.Columns.AddRange(new DataGridViewColumn[] { budgetItemNameDataGridViewTextBoxColumn, ModelParm, unitNameDataGridViewTextBoxColumn, unitPriceDataGridViewTextBoxColumn, numberDataGridViewTextBoxColumn, amountDataGridViewTextBoxColumn1, jwBudgetMainDataDataGridViewTextBoxColumn, idDataGridViewTextBoxColumn1, creationTimeDataGridViewTextBoxColumn1 });
            uiDataGridView2.DataSource = jwBudgetSubDatasBindingSource;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            uiDataGridView2.DefaultCellStyle = dataGridViewCellStyle8;
            uiDataGridView2.Dock = DockStyle.Fill;
            uiDataGridView2.EnableHeadersVisualStyles = false;
            uiDataGridView2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiDataGridView2.GridColor = Color.FromArgb(80, 160, 255);
            uiDataGridView2.Location = new Point(0, 0);
            uiDataGridView2.Name = "uiDataGridView2";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle9.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle9.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle9.SelectionForeColor = Color.White;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            uiDataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.BackColor = Color.White;
            dataGridViewCellStyle10.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiDataGridView2.RowsDefaultCellStyle = dataGridViewCellStyle10;
            uiDataGridView2.RowTemplate.Height = 25;
            uiDataGridView2.SelectedIndex = -1;
            uiDataGridView2.Size = new Size(556, 327);
            uiDataGridView2.StripeOddColor = Color.FromArgb(235, 243, 255);
            uiDataGridView2.TabIndex = 0;
            // 
            // jwBudgetSubDatasBindingSource
            // 
            jwBudgetSubDatasBindingSource.DataMember = "JwBudgetSubDatas";
            jwBudgetSubDatasBindingSource.DataSource = jwBudgetMainDataBindingSource;
            // 
            // panel3
            // 
            panel3.Controls.Add(uiSymbolButton3);
            panel3.Controls.Add(uiSymbolButton1);
            panel3.Controls.Add(uilbprojectname);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(556, 49);
            panel3.TabIndex = 1;
            // 
            // uiSymbolButton3
            // 
            uiSymbolButton3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton3.Location = new Point(272, 8);
            uiSymbolButton3.MinimumSize = new Size(1, 1);
            uiSymbolButton3.Name = "uiSymbolButton3";
            uiSymbolButton3.Size = new Size(130, 35);
            uiSymbolButton3.Symbol = 559771;
            uiSymbolButton3.TabIndex = 16;
            uiSymbolButton3.Text = "新しい予算";
            uiSymbolButton3.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton3.Click += uiSymbolButton3_Click;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(419, 8);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(134, 35);
            uiSymbolButton1.Symbol = 261891;
            uiSymbolButton1.TabIndex = 5;
            uiSymbolButton1.Text = "輸出EXCEL";
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // uilbprojectname
            // 
            uilbprojectname.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uilbprojectname.ForeColor = Color.FromArgb(48, 48, 48);
            uilbprojectname.Location = new Point(14, 12);
            uilbprojectname.Name = "uilbprojectname";
            uilbprojectname.Padding = new Padding(5, 0, 0, 0);
            uilbprojectname.Size = new Size(168, 23);
            uilbprojectname.TabIndex = 15;
            uilbprojectname.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(161, 26);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(160, 22);
            toolStripMenuItem1.Text = "輸出予算EXCEL";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // budgetItemNameDataGridViewTextBoxColumn
            // 
            budgetItemNameDataGridViewTextBoxColumn.DataPropertyName = "BudgetItemName";
            budgetItemNameDataGridViewTextBoxColumn.HeaderText = "予算項目";
            budgetItemNameDataGridViewTextBoxColumn.Name = "budgetItemNameDataGridViewTextBoxColumn";
            budgetItemNameDataGridViewTextBoxColumn.ReadOnly = true;
            budgetItemNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // ModelParm
            // 
            ModelParm.DataPropertyName = "ModelParm";
            ModelParm.HeaderText = "モデル";
            ModelParm.Name = "ModelParm";
            ModelParm.ReadOnly = true;
            ModelParm.Width = 250;
            // 
            // unitNameDataGridViewTextBoxColumn
            // 
            unitNameDataGridViewTextBoxColumn.DataPropertyName = "UnitName";
            unitNameDataGridViewTextBoxColumn.HeaderText = "ユニット名";
            unitNameDataGridViewTextBoxColumn.Name = "unitNameDataGridViewTextBoxColumn";
            unitNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // unitPriceDataGridViewTextBoxColumn
            // 
            unitPriceDataGridViewTextBoxColumn.DataPropertyName = "UnitPrice";
            unitPriceDataGridViewTextBoxColumn.HeaderText = "単価";
            unitPriceDataGridViewTextBoxColumn.Name = "unitPriceDataGridViewTextBoxColumn";
            // 
            // numberDataGridViewTextBoxColumn
            // 
            numberDataGridViewTextBoxColumn.DataPropertyName = "Number";
            numberDataGridViewTextBoxColumn.HeaderText = "量";
            numberDataGridViewTextBoxColumn.Name = "numberDataGridViewTextBoxColumn";
            // 
            // amountDataGridViewTextBoxColumn1
            // 
            amountDataGridViewTextBoxColumn1.DataPropertyName = "Amount";
            amountDataGridViewTextBoxColumn1.HeaderText = "合計金額";
            amountDataGridViewTextBoxColumn1.Name = "amountDataGridViewTextBoxColumn1";
            // 
            // jwBudgetMainDataDataGridViewTextBoxColumn
            // 
            jwBudgetMainDataDataGridViewTextBoxColumn.DataPropertyName = "JwBudgetMainData";
            jwBudgetMainDataDataGridViewTextBoxColumn.HeaderText = "JwBudgetMainData";
            jwBudgetMainDataDataGridViewTextBoxColumn.Name = "jwBudgetMainDataDataGridViewTextBoxColumn";
            jwBudgetMainDataDataGridViewTextBoxColumn.Visible = false;
            // 
            // idDataGridViewTextBoxColumn1
            // 
            idDataGridViewTextBoxColumn1.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn1.HeaderText = "Id";
            idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            idDataGridViewTextBoxColumn1.Visible = false;
            // 
            // creationTimeDataGridViewTextBoxColumn1
            // 
            creationTimeDataGridViewTextBoxColumn1.DataPropertyName = "CreationTime";
            creationTimeDataGridViewTextBoxColumn1.HeaderText = "日付";
            creationTimeDataGridViewTextBoxColumn1.Name = "creationTimeDataGridViewTextBoxColumn1";
            // 
            // JwBudgePage
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(800, 450);
            Name = "JwBudgePage";
            Text = "予算管理";
            Load += JwBudgePage_Load;
            Controls.SetChildIndex(uiLine1, 0);
            Controls.SetChildIndex(panel1, 0);
            Controls.SetChildIndex(panel2, 0);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            uiSplitContainer1.Panel1.ResumeLayout(false);
            uiSplitContainer1.Panel2.ResumeLayout(false);
            uiSplitContainer1.EndInit();
            uiSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiDataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwBudgetMainDataBindingSource).EndInit();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiDataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwBudgetSubDatasBindingSource).EndInit();
            panel3.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UISymbolButton uiSymbolButton2;
        private Sunny.UI.UISplitContainer uiSplitContainer1;
        private Sunny.UI.UIDataGridView uiDataGridView1;
        private BindingSource jwBudgetMainDataBindingSource;
        private DataGridViewTextBoxColumn projectMainIdDataGridViewTextBoxColumn;
        private Sunny.UI.UIDataGridView uiDataGridView2;
        private BindingSource jwBudgetSubDatasBindingSource;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private Panel panel3;
        private Sunny.UI.UIMarkLabel uilbprojectname;
        private Panel panel4;
        private DataGridViewTextBoxColumn projectNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn creationTimeDataGridViewTextBoxColumn;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Sunny.UI.UISymbolButton uiSymbolButton3;
        private DataGridViewTextBoxColumn budgetItemNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn ModelParm;
        private DataGridViewTextBoxColumn unitNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn unitPriceDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn jwBudgetMainDataDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn creationTimeDataGridViewTextBoxColumn1;
    }
}