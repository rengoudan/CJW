namespace RGBJWMain.Pages
{
    partial class JwBaseDataPage
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
            uiDataGridView1 = new Sunny.UI.UIDataGridView();
            MaterialTypeName = new DataGridViewTextBoxColumn();
            DefaultDataId = new DataGridViewTextBoxColumn();
            materialCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            CreationTime = new DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            jwMaterialTypeDataBindingSource = new BindingSource(components);
            jwMaterialDataBindingSource = new BindingSource(components);
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            uiSplitContainer1 = new Sunny.UI.UISplitContainer();
            uiDataGridView2 = new Sunny.UI.UIDataGridView();
            jwMaterialDatasBindingSource = new BindingSource(components);
            panel3 = new Panel();
            uiSymbolButton2 = new Sunny.UI.UISymbolButton();
            materialNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            materialParameterDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            generalTitleDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            unitPriceDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            unitNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            materialTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            remarkDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            jwMaterialTypeDataIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            jwMaterialTypeDataDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            creationTimeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiDataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwMaterialTypeDataBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwMaterialDataBindingSource).BeginInit();
            uiSplitContainer1.BeginInit();
            uiSplitContainer1.Panel1.SuspendLayout();
            uiSplitContainer1.Panel2.SuspendLayout();
            uiSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiDataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwMaterialDatasBindingSource).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // uiLine1
            // 
            uiLine1.Text = "基本的なデータ管理";
            // 
            // panel1
            // 
            panel1.Controls.Add(uiSymbolButton1);
            panel1.Controls.SetChildIndex(uiButton1, 0);
            panel1.Controls.SetChildIndex(uiSymbolButton1, 0);
            // 
            // panel2
            // 
            panel2.Controls.Add(uiSplitContainer1);
            // 
            // uiDataGridView1
            // 
            uiDataGridView1.AllowUserToAddRows = false;
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
            uiDataGridView1.Columns.AddRange(new DataGridViewColumn[] { MaterialTypeName, DefaultDataId, materialCountDataGridViewTextBoxColumn, CreationTime, idDataGridViewTextBoxColumn1 });
            uiDataGridView1.DataSource = jwMaterialTypeDataBindingSource;
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
            uiDataGridView1.Size = new Size(282, 378);
            uiDataGridView1.StripeOddColor = Color.FromArgb(235, 243, 255);
            uiDataGridView1.TabIndex = 0;
            uiDataGridView1.RowHeaderMouseClick += uiDataGridView1_RowHeaderMouseClick;
            // 
            // MaterialTypeName
            // 
            MaterialTypeName.DataPropertyName = "MaterialTypeName";
            MaterialTypeName.HeaderText = "データの種類";
            MaterialTypeName.Name = "MaterialTypeName";
            MaterialTypeName.Width = 200;
            // 
            // DefaultDataId
            // 
            DefaultDataId.DataPropertyName = "DefaultDataId";
            DefaultDataId.HeaderText = "DefaultDataId";
            DefaultDataId.Name = "DefaultDataId";
            DefaultDataId.Visible = false;
            // 
            // materialCountDataGridViewTextBoxColumn
            // 
            materialCountDataGridViewTextBoxColumn.DataPropertyName = "MaterialCount";
            materialCountDataGridViewTextBoxColumn.HeaderText = "量";
            materialCountDataGridViewTextBoxColumn.Name = "materialCountDataGridViewTextBoxColumn";
            materialCountDataGridViewTextBoxColumn.Width = 150;
            // 
            // CreationTime
            // 
            CreationTime.DataPropertyName = "CreationTime";
            CreationTime.HeaderText = "CreationTime";
            CreationTime.Name = "CreationTime";
            // 
            // idDataGridViewTextBoxColumn1
            // 
            idDataGridViewTextBoxColumn1.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn1.HeaderText = "Id";
            idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            idDataGridViewTextBoxColumn1.Visible = false;
            // 
            // jwMaterialTypeDataBindingSource
            // 
            jwMaterialTypeDataBindingSource.DataSource = typeof(JwCore.JwMaterialTypeData);
            // 
            // jwMaterialDataBindingSource
            // 
            jwMaterialDataBindingSource.DataSource = typeof(JwCore.JwMaterialData);
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(131, 5);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(77, 35);
            uiSymbolButton1.Symbol = 300043;
            uiSymbolButton1.TabIndex = 1;
            uiSymbolButton1.Text = "增";
            uiSymbolButton1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
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
            // 
            // uiSplitContainer1.Panel2
            // 
            uiSplitContainer1.Panel2.Controls.Add(uiDataGridView2);
            uiSplitContainer1.Panel2.Controls.Add(panel3);
            uiSplitContainer1.Size = new Size(800, 378);
            uiSplitContainer1.SplitterDistance = 282;
            uiSplitContainer1.SplitterWidth = 11;
            uiSplitContainer1.TabIndex = 1;
            // 
            // uiDataGridView2
            // 
            uiDataGridView2.AllowUserToAddRows = false;
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
            uiDataGridView2.Columns.AddRange(new DataGridViewColumn[] { materialNameDataGridViewTextBoxColumn, materialParameterDataGridViewTextBoxColumn, generalTitleDataGridViewTextBoxColumn, unitPriceDataGridViewTextBoxColumn, unitNameDataGridViewTextBoxColumn, materialTypeDataGridViewTextBoxColumn, remarkDataGridViewTextBoxColumn, jwMaterialTypeDataIdDataGridViewTextBoxColumn, jwMaterialTypeDataDataGridViewTextBoxColumn, idDataGridViewTextBoxColumn, creationTimeDataGridViewTextBoxColumn });
            uiDataGridView2.DataSource = jwMaterialDatasBindingSource;
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
            uiDataGridView2.Location = new Point(0, 47);
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
            uiDataGridView2.Size = new Size(507, 331);
            uiDataGridView2.StripeOddColor = Color.FromArgb(235, 243, 255);
            uiDataGridView2.TabIndex = 1;
            // 
            // jwMaterialDatasBindingSource
            // 
            jwMaterialDatasBindingSource.DataMember = "JwMaterialDatas";
            jwMaterialDatasBindingSource.DataSource = jwMaterialTypeDataBindingSource;
            // 
            // panel3
            // 
            panel3.Controls.Add(uiSymbolButton2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(507, 47);
            panel3.TabIndex = 0;
            // 
            // uiSymbolButton2
            // 
            uiSymbolButton2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton2.Location = new Point(18, 6);
            uiSymbolButton2.MinimumSize = new Size(1, 1);
            uiSymbolButton2.Name = "uiSymbolButton2";
            uiSymbolButton2.Size = new Size(77, 35);
            uiSymbolButton2.Symbol = 300043;
            uiSymbolButton2.TabIndex = 2;
            uiSymbolButton2.Text = "增";
            uiSymbolButton2.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton2.Click += uiSymbolButton2_Click;
            // 
            // materialNameDataGridViewTextBoxColumn
            // 
            materialNameDataGridViewTextBoxColumn.DataPropertyName = "MaterialName";
            materialNameDataGridViewTextBoxColumn.HeaderText = "材料名称";
            materialNameDataGridViewTextBoxColumn.Name = "materialNameDataGridViewTextBoxColumn";
            materialNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // materialParameterDataGridViewTextBoxColumn
            // 
            materialParameterDataGridViewTextBoxColumn.DataPropertyName = "MaterialParameter";
            materialParameterDataGridViewTextBoxColumn.HeaderText = "仕       様";
            materialParameterDataGridViewTextBoxColumn.Name = "materialParameterDataGridViewTextBoxColumn";
            materialParameterDataGridViewTextBoxColumn.Width = 250;
            // 
            // generalTitleDataGridViewTextBoxColumn
            // 
            generalTitleDataGridViewTextBoxColumn.DataPropertyName = "GeneralTitle";
            generalTitleDataGridViewTextBoxColumn.HeaderText = "識別子";
            generalTitleDataGridViewTextBoxColumn.Name = "generalTitleDataGridViewTextBoxColumn";
            // 
            // unitPriceDataGridViewTextBoxColumn
            // 
            unitPriceDataGridViewTextBoxColumn.DataPropertyName = "UnitPrice";
            unitPriceDataGridViewTextBoxColumn.HeaderText = "単価";
            unitPriceDataGridViewTextBoxColumn.Name = "unitPriceDataGridViewTextBoxColumn";
            // 
            // unitNameDataGridViewTextBoxColumn
            // 
            unitNameDataGridViewTextBoxColumn.DataPropertyName = "UnitName";
            unitNameDataGridViewTextBoxColumn.HeaderText = "単 位";
            unitNameDataGridViewTextBoxColumn.Name = "unitNameDataGridViewTextBoxColumn";
            // 
            // materialTypeDataGridViewTextBoxColumn
            // 
            materialTypeDataGridViewTextBoxColumn.DataPropertyName = "MaterialType";
            materialTypeDataGridViewTextBoxColumn.HeaderText = "MaterialType";
            materialTypeDataGridViewTextBoxColumn.Name = "materialTypeDataGridViewTextBoxColumn";
            materialTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // remarkDataGridViewTextBoxColumn
            // 
            remarkDataGridViewTextBoxColumn.DataPropertyName = "Remark";
            remarkDataGridViewTextBoxColumn.HeaderText = "摘  要";
            remarkDataGridViewTextBoxColumn.Name = "remarkDataGridViewTextBoxColumn";
            remarkDataGridViewTextBoxColumn.Width = 200;
            // 
            // jwMaterialTypeDataIdDataGridViewTextBoxColumn
            // 
            jwMaterialTypeDataIdDataGridViewTextBoxColumn.DataPropertyName = "JwMaterialTypeDataId";
            jwMaterialTypeDataIdDataGridViewTextBoxColumn.HeaderText = "JwMaterialTypeDataId";
            jwMaterialTypeDataIdDataGridViewTextBoxColumn.Name = "jwMaterialTypeDataIdDataGridViewTextBoxColumn";
            jwMaterialTypeDataIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // jwMaterialTypeDataDataGridViewTextBoxColumn
            // 
            jwMaterialTypeDataDataGridViewTextBoxColumn.DataPropertyName = "JwMaterialTypeData";
            jwMaterialTypeDataDataGridViewTextBoxColumn.HeaderText = "JwMaterialTypeData";
            jwMaterialTypeDataDataGridViewTextBoxColumn.Name = "jwMaterialTypeDataDataGridViewTextBoxColumn";
            jwMaterialTypeDataDataGridViewTextBoxColumn.Visible = false;
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
            creationTimeDataGridViewTextBoxColumn.Width = 200;
            // 
            // JwBaseDataPage
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(800, 450);
            Name = "JwBaseDataPage";
            Text = "データ管理";
            Load += JwBaseDataPage_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiDataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwMaterialTypeDataBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwMaterialDataBindingSource).EndInit();
            uiSplitContainer1.Panel1.ResumeLayout(false);
            uiSplitContainer1.Panel2.ResumeLayout(false);
            uiSplitContainer1.EndInit();
            uiSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiDataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwMaterialDatasBindingSource).EndInit();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIDataGridView uiDataGridView1;
        private BindingSource jwMaterialDataBindingSource;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Sunny.UI.UISplitContainer uiSplitContainer1;
        private BindingSource jwMaterialTypeDataBindingSource;
        private Panel panel3;
        private Sunny.UI.UISymbolButton uiSymbolButton2;
        private Sunny.UI.UIDataGridView uiDataGridView2;
        private BindingSource jwMaterialDatasBindingSource;
        private DataGridViewTextBoxColumn MaterialTypeName;
        private DataGridViewTextBoxColumn DefaultDataId;
        private DataGridViewTextBoxColumn materialCountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn CreationTime;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn materialNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn materialParameterDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn generalTitleDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn unitPriceDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn unitNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn materialTypeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn remarkDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn jwMaterialTypeDataIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn jwMaterialTypeDataDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn creationTimeDataGridViewTextBoxColumn;
    }
}