namespace RGBJWMain.Pages
{
    partial class JwProjectMainPage
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
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            uiDataGridView1 = new Sunny.UI.UIDataGridView();
            projectNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            biaochiDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            beamsNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            kPillarCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            singlePillarCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            bCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            bGCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            floorQuantityDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            parsedQuantityDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            jwCustomerDataIdDataGridViewTextBoxColumn = new DataGridViewComboBoxColumn();
            jwCustomerDataBindingSource = new BindingSource(components);
            jwCustomerDataDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            creationTimeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            jwProjectMainDataBindingSource = new BindingSource(components);
            uiDataGridView2 = new Sunny.UI.UIDataGridView();
            floorNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            biaochiDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            MarkBeam = new DataGridViewTextBoxColumn();
            beamCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            horizontalBeamsCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            verticalBeamsCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            kPillarCountDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            singlePillarCountDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            bCountDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            bGCountDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            creationTimeDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            jwProjectSubDatasBindingSource = new BindingSource(components);
            jwProjectSubDataBindingSource = new BindingSource(components);
            panel3 = new Panel();
            uiMarkLabel7 = new Sunny.UI.UIMarkLabel();
            panel4 = new Panel();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            uiSymbolButton2 = new Sunny.UI.UISymbolButton();
            contextMenuStrip1 = new ContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            uiSymbolButton3 = new Sunny.UI.UISymbolButton();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uiDataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwCustomerDataBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwProjectMainDataBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)uiDataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwProjectSubDatasBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwProjectSubDataBindingSource).BeginInit();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // uiLine1
            // 
            uiLine1.Size = new Size(1176, 29);
            // 
            // panel1
            // 
            panel1.Controls.Add(uiSymbolButton3);
            panel1.Controls.Add(uiMarkLabel1);
            panel1.Controls.Add(uiSymbolButton2);
            panel1.Controls.Add(uiSymbolButton1);
            panel1.Size = new Size(1176, 43);
            panel1.Controls.SetChildIndex(uiButton1, 0);
            panel1.Controls.SetChildIndex(uiSymbolButton1, 0);
            panel1.Controls.SetChildIndex(uiSymbolButton2, 0);
            panel1.Controls.SetChildIndex(uiMarkLabel1, 0);
            panel1.Controls.SetChildIndex(uiSymbolButton3, 0);
            // 
            // uiButton1
            // 
            uiButton1.Click += uiButton1_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(uiDataGridView1);
            panel2.Size = new Size(1176, 671);
            // 
            // uiDataGridView1
            // 
            uiDataGridView1.AllowUserToAddRows = false;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(235, 243, 255);
            uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            uiDataGridView1.AutoGenerateColumns = false;
            uiDataGridView1.BackgroundColor = Color.White;
            uiDataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle7.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            uiDataGridView1.ColumnHeadersHeight = 32;
            uiDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            uiDataGridView1.Columns.AddRange(new DataGridViewColumn[] { projectNameDataGridViewTextBoxColumn, biaochiDataGridViewTextBoxColumn, beamsNumberDataGridViewTextBoxColumn, kPillarCountDataGridViewTextBoxColumn, singlePillarCountDataGridViewTextBoxColumn, bCountDataGridViewTextBoxColumn, bGCountDataGridViewTextBoxColumn, floorQuantityDataGridViewTextBoxColumn, parsedQuantityDataGridViewTextBoxColumn, jwCustomerDataIdDataGridViewTextBoxColumn, jwCustomerDataDataGridViewTextBoxColumn, idDataGridViewTextBoxColumn, creationTimeDataGridViewTextBoxColumn });
            uiDataGridView1.DataSource = jwProjectMainDataBindingSource;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle8;
            uiDataGridView1.Dock = DockStyle.Top;
            uiDataGridView1.EnableHeadersVisualStyles = false;
            uiDataGridView1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiDataGridView1.GridColor = Color.FromArgb(80, 160, 255);
            uiDataGridView1.Location = new Point(0, 0);
            uiDataGridView1.MultiSelect = false;
            uiDataGridView1.Name = "uiDataGridView1";
            uiDataGridView1.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle9.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle9.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle9.SelectionForeColor = Color.White;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            uiDataGridView1.RowHeadersWidth = 62;
            dataGridViewCellStyle10.BackColor = Color.White;
            dataGridViewCellStyle10.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle10;
            uiDataGridView1.RowTemplate.Height = 25;
            uiDataGridView1.SelectedIndex = -1;
            uiDataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            uiDataGridView1.Size = new Size(1176, 400);
            uiDataGridView1.StripeOddColor = Color.FromArgb(235, 243, 255);
            uiDataGridView1.TabIndex = 0;
            uiDataGridView1.CellDoubleClick += uiDataGridView1_CellDoubleClick;
            uiDataGridView1.CellMouseDown += uiDataGridView1_CellMouseDown;
            uiDataGridView1.RowHeaderMouseClick += uiDataGridView1_RowHeaderMouseClick;
            uiDataGridView1.RowStateChanged += uiDataGridView1_RowStateChanged;
            // 
            // projectNameDataGridViewTextBoxColumn
            // 
            projectNameDataGridViewTextBoxColumn.DataPropertyName = "ProjectName";
            projectNameDataGridViewTextBoxColumn.HeaderText = "工事名";
            projectNameDataGridViewTextBoxColumn.MinimumWidth = 8;
            projectNameDataGridViewTextBoxColumn.Name = "projectNameDataGridViewTextBoxColumn";
            projectNameDataGridViewTextBoxColumn.ReadOnly = true;
            projectNameDataGridViewTextBoxColumn.Width = 180;
            // 
            // biaochiDataGridViewTextBoxColumn
            // 
            biaochiDataGridViewTextBoxColumn.DataPropertyName = "Biaochi";
            biaochiDataGridViewTextBoxColumn.HeaderText = "縮尺";
            biaochiDataGridViewTextBoxColumn.MinimumWidth = 8;
            biaochiDataGridViewTextBoxColumn.Name = "biaochiDataGridViewTextBoxColumn";
            biaochiDataGridViewTextBoxColumn.ReadOnly = true;
            biaochiDataGridViewTextBoxColumn.Width = 150;
            // 
            // beamsNumberDataGridViewTextBoxColumn
            // 
            beamsNumberDataGridViewTextBoxColumn.DataPropertyName = "BeamsNumber";
            beamsNumberDataGridViewTextBoxColumn.HeaderText = "梁数";
            beamsNumberDataGridViewTextBoxColumn.MinimumWidth = 8;
            beamsNumberDataGridViewTextBoxColumn.Name = "beamsNumberDataGridViewTextBoxColumn";
            beamsNumberDataGridViewTextBoxColumn.ReadOnly = true;
            beamsNumberDataGridViewTextBoxColumn.Width = 150;
            // 
            // kPillarCountDataGridViewTextBoxColumn
            // 
            kPillarCountDataGridViewTextBoxColumn.DataPropertyName = "KPillarCount";
            kPillarCountDataGridViewTextBoxColumn.HeaderText = "K 柱 トータル";
            kPillarCountDataGridViewTextBoxColumn.MinimumWidth = 8;
            kPillarCountDataGridViewTextBoxColumn.Name = "kPillarCountDataGridViewTextBoxColumn";
            kPillarCountDataGridViewTextBoxColumn.ReadOnly = true;
            kPillarCountDataGridViewTextBoxColumn.Width = 150;
            // 
            // singlePillarCountDataGridViewTextBoxColumn
            // 
            singlePillarCountDataGridViewTextBoxColumn.DataPropertyName = "SinglePillarCount";
            singlePillarCountDataGridViewTextBoxColumn.HeaderText = "単柱";
            singlePillarCountDataGridViewTextBoxColumn.MinimumWidth = 8;
            singlePillarCountDataGridViewTextBoxColumn.Name = "singlePillarCountDataGridViewTextBoxColumn";
            singlePillarCountDataGridViewTextBoxColumn.ReadOnly = true;
            singlePillarCountDataGridViewTextBoxColumn.Width = 150;
            // 
            // bCountDataGridViewTextBoxColumn
            // 
            bCountDataGridViewTextBoxColumn.DataPropertyName = "BCount";
            bCountDataGridViewTextBoxColumn.HeaderText = "B";
            bCountDataGridViewTextBoxColumn.MinimumWidth = 8;
            bCountDataGridViewTextBoxColumn.Name = "bCountDataGridViewTextBoxColumn";
            bCountDataGridViewTextBoxColumn.ReadOnly = true;
            bCountDataGridViewTextBoxColumn.Width = 150;
            // 
            // bGCountDataGridViewTextBoxColumn
            // 
            bGCountDataGridViewTextBoxColumn.DataPropertyName = "BGCount";
            bGCountDataGridViewTextBoxColumn.HeaderText = "BG";
            bGCountDataGridViewTextBoxColumn.MinimumWidth = 8;
            bGCountDataGridViewTextBoxColumn.Name = "bGCountDataGridViewTextBoxColumn";
            bGCountDataGridViewTextBoxColumn.ReadOnly = true;
            bGCountDataGridViewTextBoxColumn.Width = 150;
            // 
            // floorQuantityDataGridViewTextBoxColumn
            // 
            floorQuantityDataGridViewTextBoxColumn.DataPropertyName = "FloorQuantity";
            floorQuantityDataGridViewTextBoxColumn.HeaderText = "階数";
            floorQuantityDataGridViewTextBoxColumn.MinimumWidth = 8;
            floorQuantityDataGridViewTextBoxColumn.Name = "floorQuantityDataGridViewTextBoxColumn";
            floorQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            floorQuantityDataGridViewTextBoxColumn.Width = 150;
            // 
            // parsedQuantityDataGridViewTextBoxColumn
            // 
            parsedQuantityDataGridViewTextBoxColumn.DataPropertyName = "ParsedQuantity";
            parsedQuantityDataGridViewTextBoxColumn.HeaderText = "解析数";
            parsedQuantityDataGridViewTextBoxColumn.MinimumWidth = 8;
            parsedQuantityDataGridViewTextBoxColumn.Name = "parsedQuantityDataGridViewTextBoxColumn";
            parsedQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            parsedQuantityDataGridViewTextBoxColumn.Width = 150;
            // 
            // jwCustomerDataIdDataGridViewTextBoxColumn
            // 
            jwCustomerDataIdDataGridViewTextBoxColumn.DataPropertyName = "JwCustomerDataId";
            jwCustomerDataIdDataGridViewTextBoxColumn.DataSource = jwCustomerDataBindingSource;
            jwCustomerDataIdDataGridViewTextBoxColumn.DisplayMember = "CompanyName";
            jwCustomerDataIdDataGridViewTextBoxColumn.HeaderText = "JwCustomerDataId";
            jwCustomerDataIdDataGridViewTextBoxColumn.MinimumWidth = 8;
            jwCustomerDataIdDataGridViewTextBoxColumn.Name = "jwCustomerDataIdDataGridViewTextBoxColumn";
            jwCustomerDataIdDataGridViewTextBoxColumn.ReadOnly = true;
            jwCustomerDataIdDataGridViewTextBoxColumn.Resizable = DataGridViewTriState.True;
            jwCustomerDataIdDataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            jwCustomerDataIdDataGridViewTextBoxColumn.ValueMember = "Id";
            jwCustomerDataIdDataGridViewTextBoxColumn.Visible = false;
            jwCustomerDataIdDataGridViewTextBoxColumn.Width = 150;
            // 
            // jwCustomerDataBindingSource
            // 
            jwCustomerDataBindingSource.DataSource = typeof(JwCore.JwCustomerData);
            // 
            // jwCustomerDataDataGridViewTextBoxColumn
            // 
            jwCustomerDataDataGridViewTextBoxColumn.DataPropertyName = "JwCustomerData";
            jwCustomerDataDataGridViewTextBoxColumn.HeaderText = "JwCustomerData";
            jwCustomerDataDataGridViewTextBoxColumn.MinimumWidth = 8;
            jwCustomerDataDataGridViewTextBoxColumn.Name = "jwCustomerDataDataGridViewTextBoxColumn";
            jwCustomerDataDataGridViewTextBoxColumn.ReadOnly = true;
            jwCustomerDataDataGridViewTextBoxColumn.Visible = false;
            jwCustomerDataDataGridViewTextBoxColumn.Width = 150;
            // 
            // idDataGridViewTextBoxColumn
            // 
            idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn.HeaderText = "Id";
            idDataGridViewTextBoxColumn.MinimumWidth = 8;
            idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            idDataGridViewTextBoxColumn.ReadOnly = true;
            idDataGridViewTextBoxColumn.Visible = false;
            idDataGridViewTextBoxColumn.Width = 150;
            // 
            // creationTimeDataGridViewTextBoxColumn
            // 
            creationTimeDataGridViewTextBoxColumn.DataPropertyName = "CreationTime";
            creationTimeDataGridViewTextBoxColumn.HeaderText = "作成時間";
            creationTimeDataGridViewTextBoxColumn.MinimumWidth = 8;
            creationTimeDataGridViewTextBoxColumn.Name = "creationTimeDataGridViewTextBoxColumn";
            creationTimeDataGridViewTextBoxColumn.ReadOnly = true;
            creationTimeDataGridViewTextBoxColumn.Width = 150;
            // 
            // jwProjectMainDataBindingSource
            // 
            jwProjectMainDataBindingSource.DataSource = typeof(JwCore.JwProjectMainData);
            // 
            // uiDataGridView2
            // 
            uiDataGridView2.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(235, 243, 255);
            uiDataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            uiDataGridView2.AutoGenerateColumns = false;
            uiDataGridView2.BackgroundColor = Color.White;
            uiDataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            uiDataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            uiDataGridView2.ColumnHeadersHeight = 32;
            uiDataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            uiDataGridView2.Columns.AddRange(new DataGridViewColumn[] { floorNameDataGridViewTextBoxColumn, biaochiDataGridViewTextBoxColumn1, MarkBeam, beamCountDataGridViewTextBoxColumn, horizontalBeamsCountDataGridViewTextBoxColumn, verticalBeamsCountDataGridViewTextBoxColumn, kPillarCountDataGridViewTextBoxColumn1, singlePillarCountDataGridViewTextBoxColumn1, bCountDataGridViewTextBoxColumn1, bGCountDataGridViewTextBoxColumn1, idDataGridViewTextBoxColumn1, creationTimeDataGridViewTextBoxColumn1 });
            uiDataGridView2.DataSource = jwProjectSubDatasBindingSource;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            uiDataGridView2.DefaultCellStyle = dataGridViewCellStyle3;
            uiDataGridView2.Dock = DockStyle.Fill;
            uiDataGridView2.EnableHeadersVisualStyles = false;
            uiDataGridView2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiDataGridView2.GridColor = Color.FromArgb(80, 160, 255);
            uiDataGridView2.Location = new Point(0, 0);
            uiDataGridView2.Name = "uiDataGridView2";
            uiDataGridView2.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle4.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            uiDataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            uiDataGridView2.RowHeadersWidth = 62;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiDataGridView2.RowsDefaultCellStyle = dataGridViewCellStyle5;
            uiDataGridView2.RowTemplate.Height = 25;
            uiDataGridView2.SelectedIndex = -1;
            uiDataGridView2.Size = new Size(1176, 222);
            uiDataGridView2.StripeOddColor = Color.FromArgb(235, 243, 255);
            uiDataGridView2.TabIndex = 1;
            uiDataGridView2.CellDoubleClick += uiDataGridView2_CellDoubleClick;
            uiDataGridView2.CellMouseDown += uiDataGridView2_CellMouseDown;
            uiDataGridView2.UserDeletedRow += uiDataGridView2_UserDeletedRow;
            uiDataGridView2.UserDeletingRow += uiDataGridView2_UserDeletingRow;
            // 
            // floorNameDataGridViewTextBoxColumn
            // 
            floorNameDataGridViewTextBoxColumn.DataPropertyName = "FloorName";
            floorNameDataGridViewTextBoxColumn.HeaderText = "階";
            floorNameDataGridViewTextBoxColumn.MinimumWidth = 8;
            floorNameDataGridViewTextBoxColumn.Name = "floorNameDataGridViewTextBoxColumn";
            floorNameDataGridViewTextBoxColumn.ReadOnly = true;
            floorNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // biaochiDataGridViewTextBoxColumn1
            // 
            biaochiDataGridViewTextBoxColumn1.DataPropertyName = "Biaochi";
            biaochiDataGridViewTextBoxColumn1.HeaderText = "縮尺";
            biaochiDataGridViewTextBoxColumn1.MinimumWidth = 8;
            biaochiDataGridViewTextBoxColumn1.Name = "biaochiDataGridViewTextBoxColumn1";
            biaochiDataGridViewTextBoxColumn1.ReadOnly = true;
            biaochiDataGridViewTextBoxColumn1.Width = 150;
            // 
            // MarkBeam
            // 
            MarkBeam.DataPropertyName = "MarkBeam";
            MarkBeam.HeaderText = "符号";
            MarkBeam.MinimumWidth = 8;
            MarkBeam.Name = "MarkBeam";
            MarkBeam.ReadOnly = true;
            MarkBeam.Width = 150;
            // 
            // beamCountDataGridViewTextBoxColumn
            // 
            beamCountDataGridViewTextBoxColumn.DataPropertyName = "BeamCount";
            beamCountDataGridViewTextBoxColumn.HeaderText = "梁数";
            beamCountDataGridViewTextBoxColumn.MinimumWidth = 8;
            beamCountDataGridViewTextBoxColumn.Name = "beamCountDataGridViewTextBoxColumn";
            beamCountDataGridViewTextBoxColumn.ReadOnly = true;
            beamCountDataGridViewTextBoxColumn.Width = 150;
            // 
            // horizontalBeamsCountDataGridViewTextBoxColumn
            // 
            horizontalBeamsCountDataGridViewTextBoxColumn.DataPropertyName = "HorizontalBeamsCount";
            horizontalBeamsCountDataGridViewTextBoxColumn.HeaderText = "水平梁数";
            horizontalBeamsCountDataGridViewTextBoxColumn.MinimumWidth = 8;
            horizontalBeamsCountDataGridViewTextBoxColumn.Name = "horizontalBeamsCountDataGridViewTextBoxColumn";
            horizontalBeamsCountDataGridViewTextBoxColumn.ReadOnly = true;
            horizontalBeamsCountDataGridViewTextBoxColumn.Width = 150;
            // 
            // verticalBeamsCountDataGridViewTextBoxColumn
            // 
            verticalBeamsCountDataGridViewTextBoxColumn.DataPropertyName = "VerticalBeamsCount";
            verticalBeamsCountDataGridViewTextBoxColumn.HeaderText = "垂直梁数";
            verticalBeamsCountDataGridViewTextBoxColumn.MinimumWidth = 8;
            verticalBeamsCountDataGridViewTextBoxColumn.Name = "verticalBeamsCountDataGridViewTextBoxColumn";
            verticalBeamsCountDataGridViewTextBoxColumn.ReadOnly = true;
            verticalBeamsCountDataGridViewTextBoxColumn.Width = 150;
            // 
            // kPillarCountDataGridViewTextBoxColumn1
            // 
            kPillarCountDataGridViewTextBoxColumn1.DataPropertyName = "KPillarCount";
            kPillarCountDataGridViewTextBoxColumn1.HeaderText = "K";
            kPillarCountDataGridViewTextBoxColumn1.MinimumWidth = 8;
            kPillarCountDataGridViewTextBoxColumn1.Name = "kPillarCountDataGridViewTextBoxColumn1";
            kPillarCountDataGridViewTextBoxColumn1.ReadOnly = true;
            kPillarCountDataGridViewTextBoxColumn1.Width = 150;
            // 
            // singlePillarCountDataGridViewTextBoxColumn1
            // 
            singlePillarCountDataGridViewTextBoxColumn1.DataPropertyName = "SinglePillarCount";
            singlePillarCountDataGridViewTextBoxColumn1.HeaderText = "単柱";
            singlePillarCountDataGridViewTextBoxColumn1.MinimumWidth = 8;
            singlePillarCountDataGridViewTextBoxColumn1.Name = "singlePillarCountDataGridViewTextBoxColumn1";
            singlePillarCountDataGridViewTextBoxColumn1.ReadOnly = true;
            singlePillarCountDataGridViewTextBoxColumn1.Width = 150;
            // 
            // bCountDataGridViewTextBoxColumn1
            // 
            bCountDataGridViewTextBoxColumn1.DataPropertyName = "BCount";
            bCountDataGridViewTextBoxColumn1.HeaderText = "Ｂ金物";
            bCountDataGridViewTextBoxColumn1.MinimumWidth = 8;
            bCountDataGridViewTextBoxColumn1.Name = "bCountDataGridViewTextBoxColumn1";
            bCountDataGridViewTextBoxColumn1.ReadOnly = true;
            bCountDataGridViewTextBoxColumn1.Width = 150;
            // 
            // bGCountDataGridViewTextBoxColumn1
            // 
            bGCountDataGridViewTextBoxColumn1.DataPropertyName = "BGCount";
            bGCountDataGridViewTextBoxColumn1.HeaderText = "ＢＧ金物";
            bGCountDataGridViewTextBoxColumn1.MinimumWidth = 8;
            bGCountDataGridViewTextBoxColumn1.Name = "bGCountDataGridViewTextBoxColumn1";
            bGCountDataGridViewTextBoxColumn1.ReadOnly = true;
            bGCountDataGridViewTextBoxColumn1.Width = 150;
            // 
            // idDataGridViewTextBoxColumn1
            // 
            idDataGridViewTextBoxColumn1.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn1.HeaderText = "Id";
            idDataGridViewTextBoxColumn1.MinimumWidth = 8;
            idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            idDataGridViewTextBoxColumn1.ReadOnly = true;
            idDataGridViewTextBoxColumn1.Width = 150;
            // 
            // creationTimeDataGridViewTextBoxColumn1
            // 
            creationTimeDataGridViewTextBoxColumn1.DataPropertyName = "CreationTime";
            creationTimeDataGridViewTextBoxColumn1.HeaderText = "CreationTime";
            creationTimeDataGridViewTextBoxColumn1.MinimumWidth = 8;
            creationTimeDataGridViewTextBoxColumn1.Name = "creationTimeDataGridViewTextBoxColumn1";
            creationTimeDataGridViewTextBoxColumn1.ReadOnly = true;
            creationTimeDataGridViewTextBoxColumn1.Width = 150;
            // 
            // jwProjectSubDatasBindingSource
            // 
            jwProjectSubDatasBindingSource.DataMember = "JwProjectSubDatas";
            jwProjectSubDatasBindingSource.DataSource = jwProjectMainDataBindingSource;
            // 
            // jwProjectSubDataBindingSource
            // 
            jwProjectSubDataBindingSource.DataSource = typeof(JwCore.JwProjectSubData);
            // 
            // panel3
            // 
            panel3.Controls.Add(uiMarkLabel7);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 400);
            panel3.Name = "panel3";
            panel3.Size = new Size(1176, 49);
            panel3.TabIndex = 2;
            // 
            // uiMarkLabel7
            // 
            uiMarkLabel7.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel7.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel7.Location = new Point(12, 14);
            uiMarkLabel7.Name = "uiMarkLabel7";
            uiMarkLabel7.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel7.Size = new Size(415, 23);
            uiMarkLabel7.TabIndex = 26;
            uiMarkLabel7.Text = "解析された設計図を表示するにはダブルクリックします";
            uiMarkLabel7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            panel4.Controls.Add(uiDataGridView2);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 449);
            panel4.Name = "panel4";
            panel4.Size = new Size(1176, 222);
            panel4.TabIndex = 3;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.FillColor = Color.Red;
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(261, 5);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(166, 35);
            uiSymbolButton1.Symbol = 61587;
            uiSymbolButton1.TabIndex = 1;
            uiSymbolButton1.Text = "アップロード";
            uiSymbolButton1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // uiSymbolButton2
            // 
            uiSymbolButton2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton2.Location = new Point(148, 5);
            uiSymbolButton2.MinimumSize = new Size(1, 1);
            uiSymbolButton2.Name = "uiSymbolButton2";
            uiSymbolButton2.Size = new Size(77, 35);
            uiSymbolButton2.Symbol = 300043;
            uiSymbolButton2.TabIndex = 3;
            uiSymbolButton2.Text = "增";
            uiSymbolButton2.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton2.Click += uiSymbolButton2_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(189, 34);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(188, 30);
            toolStripMenuItem1.Text = "輸出梁設計図";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // uiMarkLabel1
            // 
            uiMarkLabel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            uiMarkLabel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel1.Location = new Point(605, 5);
            uiMarkLabel1.Name = "uiMarkLabel1";
            uiMarkLabel1.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel1.Size = new Size(568, 35);
            uiMarkLabel1.TabIndex = 27;
            uiMarkLabel1.Text = "シングルクリックで選択してアップロード；エクスポートするには行データを右クリック";
            uiMarkLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiSymbolButton3
            // 
            uiSymbolButton3.FillColor = Color.Red;
            uiSymbolButton3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton3.Location = new Point(433, 6);
            uiSymbolButton3.MinimumSize = new Size(1, 1);
            uiSymbolButton3.Name = "uiSymbolButton3";
            uiSymbolButton3.Size = new Size(166, 35);
            uiSymbolButton3.Symbol = 61587;
            uiSymbolButton3.TabIndex = 28;
            uiSymbolButton3.Text = "テストカラー";
            uiSymbolButton3.Click += uiSymbolButton3_Click;
            // 
            // JwProjectMainPage
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1176, 743);
            Name = "JwProjectMainPage";
            NeedReload = true;
            Text = "プロジェクト";
            Shown += JwProjectMainPage_Shown;
            Controls.SetChildIndex(uiLine1, 0);
            Controls.SetChildIndex(panel1, 0);
            Controls.SetChildIndex(panel2, 0);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uiDataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwCustomerDataBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwProjectMainDataBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)uiDataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwProjectSubDatasBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwProjectSubDataBindingSource).EndInit();
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIDataGridView uiDataGridView1;
        private BindingSource jwProjectMainDataBindingSource;
        private BindingSource jwCustomerDataBindingSource;
        private Panel panel4;
        private Sunny.UI.UIDataGridView uiDataGridView2;
        private BindingSource jwProjectSubDataBindingSource;
        private Panel panel3;
        private BindingSource jwProjectSubDatasBindingSource;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Sunny.UI.UISymbolButton uiSymbolButton2;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private Sunny.UI.UIMarkLabel uiMarkLabel7;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UISymbolButton uiSymbolButton3;
        private DataGridViewTextBoxColumn projectNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn biaochiDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn beamsNumberDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn kPillarCountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn singlePillarCountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn bCountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn bGCountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn floorQuantityDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn parsedQuantityDataGridViewTextBoxColumn;
        private DataGridViewComboBoxColumn jwCustomerDataIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn jwCustomerDataDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn creationTimeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn floorNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn biaochiDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn MarkBeam;
        private DataGridViewTextBoxColumn beamCountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn horizontalBeamsCountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn verticalBeamsCountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn kPillarCountDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn singlePillarCountDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn bCountDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn bGCountDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn creationTimeDataGridViewTextBoxColumn1;
    }
}