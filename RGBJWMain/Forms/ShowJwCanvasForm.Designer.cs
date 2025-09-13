namespace RGBJWMain.Forms
{
    partial class ShowJwCanvasForm
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
            panel1 = new Panel();
            splitContainer1 = new SplitContainer();
            panel4 = new Panel();
            dataGridView3 = new DataGridView();
            bujianNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            beamIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            directedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            gouJianTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            isLianjieDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            isNoBeamDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            jwProjectSubDataIdDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            jwProjectSubDataDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            locationDataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            widthDataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            heightDataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            scaleDataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            directionTypeDataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            creationTimeDataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            jwLinkPartDatasBindingSource = new BindingSource(components);
            jwProjectSubDataBindingSource = new BindingSource(components);
            panel5 = new Panel();
            dataGridView2 = new DataGridView();
            pillarCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            TaggTitle = new DataGridViewTextBoxColumn();
            blocksCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            baseTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            firstLocationDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            firstWidthDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            firstHeightDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            centerLocationDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            centerWidthDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            centerHeightDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            lastLocationDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            lastWidthDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            lastHeightDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            jwProjectSubDataIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            jwProjectSubDataDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            locationDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            widthDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            heightDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            scaleDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            directionTypeDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            creationTimeDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            jwPillarDatasBindingSource = new BindingSource(components);
            panel3 = new Panel();
            dataGridView1 = new DataGridView();
            BeamCode = new DataGridViewTextBoxColumn();
            locationDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            IsQiegeBeam = new DataGridViewCheckBoxColumn();
            Width = new DataGridViewTextBoxColumn();
            heightDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            scaleDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            directionTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            creationTimeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            jwBeamDatasBindingSource = new BindingSource(components);
            jwCanvasControl1 = new Controls.JwCanvasControl();
            panel2 = new Panel();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            saveFileDialog1 = new SaveFileDialog();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwLinkPartDatasBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwProjectSubDataBindingSource).BeginInit();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwPillarDatasBindingSource).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)jwBeamDatasBindingSource).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(splitContainer1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 41);
            panel1.Name = "panel1";
            panel1.Size = new Size(1436, 1059);
            panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 46);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel4);
            splitContainer1.Panel1.Controls.Add(panel3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(jwCanvasControl1);
            splitContainer1.Size = new Size(1436, 1013);
            splitContainer1.SplitterDistance = 384;
            splitContainer1.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(dataGridView3);
            panel4.Controls.Add(panel5);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 234);
            panel4.Name = "panel4";
            panel4.Size = new Size(384, 779);
            panel4.TabIndex = 1;
            // 
            // dataGridView3
            // 
            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { bujianNameDataGridViewTextBoxColumn, beamIdDataGridViewTextBoxColumn, directedDataGridViewTextBoxColumn, gouJianTypeDataGridViewTextBoxColumn, isLianjieDataGridViewCheckBoxColumn, isNoBeamDataGridViewCheckBoxColumn, jwProjectSubDataIdDataGridViewTextBoxColumn1, jwProjectSubDataDataGridViewTextBoxColumn1, locationDataGridViewTextBoxColumn2, widthDataGridViewTextBoxColumn2, heightDataGridViewTextBoxColumn2, scaleDataGridViewTextBoxColumn2, directionTypeDataGridViewTextBoxColumn2, idDataGridViewTextBoxColumn2, creationTimeDataGridViewTextBoxColumn2 });
            dataGridView3.DataSource = jwLinkPartDatasBindingSource;
            dataGridView3.Dock = DockStyle.Fill;
            dataGridView3.Location = new Point(0, 217);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowTemplate.Height = 25;
            dataGridView3.Size = new Size(384, 562);
            dataGridView3.TabIndex = 1;
            dataGridView3.CellDoubleClick += dataGridView3_CellDoubleClick;
            // 
            // bujianNameDataGridViewTextBoxColumn
            // 
            bujianNameDataGridViewTextBoxColumn.DataPropertyName = "BujianName";
            bujianNameDataGridViewTextBoxColumn.HeaderText = "BujianName";
            bujianNameDataGridViewTextBoxColumn.Name = "bujianNameDataGridViewTextBoxColumn";
            // 
            // beamIdDataGridViewTextBoxColumn
            // 
            beamIdDataGridViewTextBoxColumn.DataPropertyName = "BeamId";
            beamIdDataGridViewTextBoxColumn.HeaderText = "BeamId";
            beamIdDataGridViewTextBoxColumn.Name = "beamIdDataGridViewTextBoxColumn";
            beamIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // directedDataGridViewTextBoxColumn
            // 
            directedDataGridViewTextBoxColumn.DataPropertyName = "Directed";
            directedDataGridViewTextBoxColumn.HeaderText = "Directed";
            directedDataGridViewTextBoxColumn.Name = "directedDataGridViewTextBoxColumn";
            // 
            // gouJianTypeDataGridViewTextBoxColumn
            // 
            gouJianTypeDataGridViewTextBoxColumn.DataPropertyName = "GouJianType";
            gouJianTypeDataGridViewTextBoxColumn.HeaderText = "GouJianType";
            gouJianTypeDataGridViewTextBoxColumn.Name = "gouJianTypeDataGridViewTextBoxColumn";
            // 
            // isLianjieDataGridViewCheckBoxColumn
            // 
            isLianjieDataGridViewCheckBoxColumn.DataPropertyName = "IsLianjie";
            isLianjieDataGridViewCheckBoxColumn.HeaderText = "IsLianjie";
            isLianjieDataGridViewCheckBoxColumn.Name = "isLianjieDataGridViewCheckBoxColumn";
            isLianjieDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // isNoBeamDataGridViewCheckBoxColumn
            // 
            isNoBeamDataGridViewCheckBoxColumn.DataPropertyName = "IsNoBeam";
            isNoBeamDataGridViewCheckBoxColumn.HeaderText = "IsNoBeam";
            isNoBeamDataGridViewCheckBoxColumn.Name = "isNoBeamDataGridViewCheckBoxColumn";
            isNoBeamDataGridViewCheckBoxColumn.Visible = false;
            // 
            // jwProjectSubDataIdDataGridViewTextBoxColumn1
            // 
            jwProjectSubDataIdDataGridViewTextBoxColumn1.DataPropertyName = "JwProjectSubDataId";
            jwProjectSubDataIdDataGridViewTextBoxColumn1.HeaderText = "JwProjectSubDataId";
            jwProjectSubDataIdDataGridViewTextBoxColumn1.Name = "jwProjectSubDataIdDataGridViewTextBoxColumn1";
            jwProjectSubDataIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // jwProjectSubDataDataGridViewTextBoxColumn1
            // 
            jwProjectSubDataDataGridViewTextBoxColumn1.DataPropertyName = "JwProjectSubData";
            jwProjectSubDataDataGridViewTextBoxColumn1.HeaderText = "JwProjectSubData";
            jwProjectSubDataDataGridViewTextBoxColumn1.Name = "jwProjectSubDataDataGridViewTextBoxColumn1";
            jwProjectSubDataDataGridViewTextBoxColumn1.Visible = false;
            // 
            // locationDataGridViewTextBoxColumn2
            // 
            locationDataGridViewTextBoxColumn2.DataPropertyName = "Location";
            locationDataGridViewTextBoxColumn2.HeaderText = "Location";
            locationDataGridViewTextBoxColumn2.Name = "locationDataGridViewTextBoxColumn2";
            locationDataGridViewTextBoxColumn2.Visible = false;
            // 
            // widthDataGridViewTextBoxColumn2
            // 
            widthDataGridViewTextBoxColumn2.DataPropertyName = "Width";
            widthDataGridViewTextBoxColumn2.HeaderText = "Width";
            widthDataGridViewTextBoxColumn2.Name = "widthDataGridViewTextBoxColumn2";
            widthDataGridViewTextBoxColumn2.Visible = false;
            // 
            // heightDataGridViewTextBoxColumn2
            // 
            heightDataGridViewTextBoxColumn2.DataPropertyName = "Height";
            heightDataGridViewTextBoxColumn2.HeaderText = "Height";
            heightDataGridViewTextBoxColumn2.Name = "heightDataGridViewTextBoxColumn2";
            heightDataGridViewTextBoxColumn2.Visible = false;
            // 
            // scaleDataGridViewTextBoxColumn2
            // 
            scaleDataGridViewTextBoxColumn2.DataPropertyName = "Scale";
            scaleDataGridViewTextBoxColumn2.HeaderText = "Scale";
            scaleDataGridViewTextBoxColumn2.Name = "scaleDataGridViewTextBoxColumn2";
            scaleDataGridViewTextBoxColumn2.Visible = false;
            // 
            // directionTypeDataGridViewTextBoxColumn2
            // 
            directionTypeDataGridViewTextBoxColumn2.DataPropertyName = "DirectionType";
            directionTypeDataGridViewTextBoxColumn2.HeaderText = "DirectionType";
            directionTypeDataGridViewTextBoxColumn2.Name = "directionTypeDataGridViewTextBoxColumn2";
            directionTypeDataGridViewTextBoxColumn2.Visible = false;
            // 
            // idDataGridViewTextBoxColumn2
            // 
            idDataGridViewTextBoxColumn2.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn2.HeaderText = "Id";
            idDataGridViewTextBoxColumn2.Name = "idDataGridViewTextBoxColumn2";
            idDataGridViewTextBoxColumn2.Visible = false;
            // 
            // creationTimeDataGridViewTextBoxColumn2
            // 
            creationTimeDataGridViewTextBoxColumn2.DataPropertyName = "CreationTime";
            creationTimeDataGridViewTextBoxColumn2.HeaderText = "CreationTime";
            creationTimeDataGridViewTextBoxColumn2.Name = "creationTimeDataGridViewTextBoxColumn2";
            creationTimeDataGridViewTextBoxColumn2.Visible = false;
            // 
            // jwLinkPartDatasBindingSource
            // 
            jwLinkPartDatasBindingSource.DataMember = "JwLinkPartDatas";
            jwLinkPartDatasBindingSource.DataSource = jwProjectSubDataBindingSource;
            // 
            // jwProjectSubDataBindingSource
            // 
            jwProjectSubDataBindingSource.DataSource = typeof(JwCore.JwProjectSubData);
            // 
            // panel5
            // 
            panel5.Controls.Add(dataGridView2);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(384, 217);
            panel5.TabIndex = 0;
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { pillarCodeDataGridViewTextBoxColumn, TaggTitle, blocksCountDataGridViewTextBoxColumn, baseTypeDataGridViewTextBoxColumn, firstLocationDataGridViewTextBoxColumn, firstWidthDataGridViewTextBoxColumn, firstHeightDataGridViewTextBoxColumn, centerLocationDataGridViewTextBoxColumn, centerWidthDataGridViewTextBoxColumn, centerHeightDataGridViewTextBoxColumn, lastLocationDataGridViewTextBoxColumn, lastWidthDataGridViewTextBoxColumn, lastHeightDataGridViewTextBoxColumn, jwProjectSubDataIdDataGridViewTextBoxColumn, jwProjectSubDataDataGridViewTextBoxColumn, locationDataGridViewTextBoxColumn1, widthDataGridViewTextBoxColumn1, heightDataGridViewTextBoxColumn1, scaleDataGridViewTextBoxColumn1, directionTypeDataGridViewTextBoxColumn1, idDataGridViewTextBoxColumn1, creationTimeDataGridViewTextBoxColumn1 });
            dataGridView2.DataSource = jwPillarDatasBindingSource;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.ReadOnly = true;
            dataGridView2.RowTemplate.Height = 25;
            dataGridView2.Size = new Size(384, 217);
            dataGridView2.TabIndex = 0;
            dataGridView2.CellDoubleClick += dataGridView2_CellDoubleClick;
            // 
            // pillarCodeDataGridViewTextBoxColumn
            // 
            pillarCodeDataGridViewTextBoxColumn.DataPropertyName = "PillarCode";
            pillarCodeDataGridViewTextBoxColumn.HeaderText = "柱";
            pillarCodeDataGridViewTextBoxColumn.Name = "pillarCodeDataGridViewTextBoxColumn";
            pillarCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // TaggTitle
            // 
            TaggTitle.DataPropertyName = "TaggTitle";
            TaggTitle.HeaderText = "TaggTitle";
            TaggTitle.Name = "TaggTitle";
            TaggTitle.ReadOnly = true;
            // 
            // blocksCountDataGridViewTextBoxColumn
            // 
            blocksCountDataGridViewTextBoxColumn.DataPropertyName = "BlocksCount";
            blocksCountDataGridViewTextBoxColumn.HeaderText = "BlocksCount";
            blocksCountDataGridViewTextBoxColumn.Name = "blocksCountDataGridViewTextBoxColumn";
            blocksCountDataGridViewTextBoxColumn.ReadOnly = true;
            blocksCountDataGridViewTextBoxColumn.Visible = false;
            // 
            // baseTypeDataGridViewTextBoxColumn
            // 
            baseTypeDataGridViewTextBoxColumn.DataPropertyName = "BaseType";
            baseTypeDataGridViewTextBoxColumn.HeaderText = "タイプ";
            baseTypeDataGridViewTextBoxColumn.Name = "baseTypeDataGridViewTextBoxColumn";
            baseTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // firstLocationDataGridViewTextBoxColumn
            // 
            firstLocationDataGridViewTextBoxColumn.DataPropertyName = "FirstLocation";
            firstLocationDataGridViewTextBoxColumn.HeaderText = "FirstLocation";
            firstLocationDataGridViewTextBoxColumn.Name = "firstLocationDataGridViewTextBoxColumn";
            firstLocationDataGridViewTextBoxColumn.ReadOnly = true;
            firstLocationDataGridViewTextBoxColumn.Visible = false;
            // 
            // firstWidthDataGridViewTextBoxColumn
            // 
            firstWidthDataGridViewTextBoxColumn.DataPropertyName = "FirstWidth";
            firstWidthDataGridViewTextBoxColumn.HeaderText = "FirstWidth";
            firstWidthDataGridViewTextBoxColumn.Name = "firstWidthDataGridViewTextBoxColumn";
            firstWidthDataGridViewTextBoxColumn.ReadOnly = true;
            firstWidthDataGridViewTextBoxColumn.Visible = false;
            // 
            // firstHeightDataGridViewTextBoxColumn
            // 
            firstHeightDataGridViewTextBoxColumn.DataPropertyName = "FirstHeight";
            firstHeightDataGridViewTextBoxColumn.HeaderText = "FirstHeight";
            firstHeightDataGridViewTextBoxColumn.Name = "firstHeightDataGridViewTextBoxColumn";
            firstHeightDataGridViewTextBoxColumn.ReadOnly = true;
            firstHeightDataGridViewTextBoxColumn.Visible = false;
            // 
            // centerLocationDataGridViewTextBoxColumn
            // 
            centerLocationDataGridViewTextBoxColumn.DataPropertyName = "CenterLocation";
            centerLocationDataGridViewTextBoxColumn.HeaderText = "CenterLocation";
            centerLocationDataGridViewTextBoxColumn.Name = "centerLocationDataGridViewTextBoxColumn";
            centerLocationDataGridViewTextBoxColumn.ReadOnly = true;
            centerLocationDataGridViewTextBoxColumn.Visible = false;
            // 
            // centerWidthDataGridViewTextBoxColumn
            // 
            centerWidthDataGridViewTextBoxColumn.DataPropertyName = "CenterWidth";
            centerWidthDataGridViewTextBoxColumn.HeaderText = "CenterWidth";
            centerWidthDataGridViewTextBoxColumn.Name = "centerWidthDataGridViewTextBoxColumn";
            centerWidthDataGridViewTextBoxColumn.ReadOnly = true;
            centerWidthDataGridViewTextBoxColumn.Visible = false;
            // 
            // centerHeightDataGridViewTextBoxColumn
            // 
            centerHeightDataGridViewTextBoxColumn.DataPropertyName = "CenterHeight";
            centerHeightDataGridViewTextBoxColumn.HeaderText = "CenterHeight";
            centerHeightDataGridViewTextBoxColumn.Name = "centerHeightDataGridViewTextBoxColumn";
            centerHeightDataGridViewTextBoxColumn.ReadOnly = true;
            centerHeightDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastLocationDataGridViewTextBoxColumn
            // 
            lastLocationDataGridViewTextBoxColumn.DataPropertyName = "LastLocation";
            lastLocationDataGridViewTextBoxColumn.HeaderText = "LastLocation";
            lastLocationDataGridViewTextBoxColumn.Name = "lastLocationDataGridViewTextBoxColumn";
            lastLocationDataGridViewTextBoxColumn.ReadOnly = true;
            lastLocationDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastWidthDataGridViewTextBoxColumn
            // 
            lastWidthDataGridViewTextBoxColumn.DataPropertyName = "LastWidth";
            lastWidthDataGridViewTextBoxColumn.HeaderText = "LastWidth";
            lastWidthDataGridViewTextBoxColumn.Name = "lastWidthDataGridViewTextBoxColumn";
            lastWidthDataGridViewTextBoxColumn.ReadOnly = true;
            lastWidthDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastHeightDataGridViewTextBoxColumn
            // 
            lastHeightDataGridViewTextBoxColumn.DataPropertyName = "LastHeight";
            lastHeightDataGridViewTextBoxColumn.HeaderText = "LastHeight";
            lastHeightDataGridViewTextBoxColumn.Name = "lastHeightDataGridViewTextBoxColumn";
            lastHeightDataGridViewTextBoxColumn.ReadOnly = true;
            lastHeightDataGridViewTextBoxColumn.Visible = false;
            // 
            // jwProjectSubDataIdDataGridViewTextBoxColumn
            // 
            jwProjectSubDataIdDataGridViewTextBoxColumn.DataPropertyName = "JwProjectSubDataId";
            jwProjectSubDataIdDataGridViewTextBoxColumn.HeaderText = "JwProjectSubDataId";
            jwProjectSubDataIdDataGridViewTextBoxColumn.Name = "jwProjectSubDataIdDataGridViewTextBoxColumn";
            jwProjectSubDataIdDataGridViewTextBoxColumn.ReadOnly = true;
            jwProjectSubDataIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // jwProjectSubDataDataGridViewTextBoxColumn
            // 
            jwProjectSubDataDataGridViewTextBoxColumn.DataPropertyName = "JwProjectSubData";
            jwProjectSubDataDataGridViewTextBoxColumn.HeaderText = "JwProjectSubData";
            jwProjectSubDataDataGridViewTextBoxColumn.Name = "jwProjectSubDataDataGridViewTextBoxColumn";
            jwProjectSubDataDataGridViewTextBoxColumn.ReadOnly = true;
            jwProjectSubDataDataGridViewTextBoxColumn.Visible = false;
            // 
            // locationDataGridViewTextBoxColumn1
            // 
            locationDataGridViewTextBoxColumn1.DataPropertyName = "Location";
            locationDataGridViewTextBoxColumn1.HeaderText = "Location";
            locationDataGridViewTextBoxColumn1.Name = "locationDataGridViewTextBoxColumn1";
            locationDataGridViewTextBoxColumn1.ReadOnly = true;
            locationDataGridViewTextBoxColumn1.Visible = false;
            // 
            // widthDataGridViewTextBoxColumn1
            // 
            widthDataGridViewTextBoxColumn1.DataPropertyName = "Width";
            widthDataGridViewTextBoxColumn1.HeaderText = "Width";
            widthDataGridViewTextBoxColumn1.Name = "widthDataGridViewTextBoxColumn1";
            widthDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // heightDataGridViewTextBoxColumn1
            // 
            heightDataGridViewTextBoxColumn1.DataPropertyName = "Height";
            heightDataGridViewTextBoxColumn1.HeaderText = "Height";
            heightDataGridViewTextBoxColumn1.Name = "heightDataGridViewTextBoxColumn1";
            heightDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // scaleDataGridViewTextBoxColumn1
            // 
            scaleDataGridViewTextBoxColumn1.DataPropertyName = "Scale";
            scaleDataGridViewTextBoxColumn1.HeaderText = "Scale";
            scaleDataGridViewTextBoxColumn1.Name = "scaleDataGridViewTextBoxColumn1";
            scaleDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // directionTypeDataGridViewTextBoxColumn1
            // 
            directionTypeDataGridViewTextBoxColumn1.DataPropertyName = "DirectionType";
            directionTypeDataGridViewTextBoxColumn1.HeaderText = "DirectionType";
            directionTypeDataGridViewTextBoxColumn1.Name = "directionTypeDataGridViewTextBoxColumn1";
            directionTypeDataGridViewTextBoxColumn1.ReadOnly = true;
            directionTypeDataGridViewTextBoxColumn1.Visible = false;
            // 
            // idDataGridViewTextBoxColumn1
            // 
            idDataGridViewTextBoxColumn1.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn1.HeaderText = "Id";
            idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            idDataGridViewTextBoxColumn1.ReadOnly = true;
            idDataGridViewTextBoxColumn1.Visible = false;
            // 
            // creationTimeDataGridViewTextBoxColumn1
            // 
            creationTimeDataGridViewTextBoxColumn1.DataPropertyName = "CreationTime";
            creationTimeDataGridViewTextBoxColumn1.HeaderText = "CreationTime";
            creationTimeDataGridViewTextBoxColumn1.Name = "creationTimeDataGridViewTextBoxColumn1";
            creationTimeDataGridViewTextBoxColumn1.ReadOnly = true;
            creationTimeDataGridViewTextBoxColumn1.Visible = false;
            // 
            // jwPillarDatasBindingSource
            // 
            jwPillarDatasBindingSource.DataMember = "JwPillarDatas";
            jwPillarDatasBindingSource.DataSource = jwProjectSubDataBindingSource;
            // 
            // panel3
            // 
            panel3.Controls.Add(dataGridView1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(384, 234);
            panel3.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { BeamCode, locationDataGridViewTextBoxColumn, IsQiegeBeam, Width, heightDataGridViewTextBoxColumn, scaleDataGridViewTextBoxColumn, directionTypeDataGridViewTextBoxColumn, idDataGridViewTextBoxColumn, creationTimeDataGridViewTextBoxColumn });
            dataGridView1.DataSource = jwBeamDatasBindingSource;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(384, 234);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            // 
            // BeamCode
            // 
            BeamCode.DataPropertyName = "BeamCode";
            BeamCode.HeaderText = "梁";
            BeamCode.Name = "BeamCode";
            BeamCode.ReadOnly = true;
            // 
            // locationDataGridViewTextBoxColumn
            // 
            locationDataGridViewTextBoxColumn.DataPropertyName = "Location";
            locationDataGridViewTextBoxColumn.HeaderText = "Location";
            locationDataGridViewTextBoxColumn.Name = "locationDataGridViewTextBoxColumn";
            locationDataGridViewTextBoxColumn.ReadOnly = true;
            locationDataGridViewTextBoxColumn.Visible = false;
            // 
            // IsQiegeBeam
            // 
            IsQiegeBeam.DataPropertyName = "IsQiegeBeam";
            IsQiegeBeam.HeaderText = "分割";
            IsQiegeBeam.Name = "IsQiegeBeam";
            IsQiegeBeam.ReadOnly = true;
            // 
            // Width
            // 
            Width.DataPropertyName = "Width";
            Width.HeaderText = "Width";
            Width.Name = "Width";
            Width.ReadOnly = true;
            // 
            // heightDataGridViewTextBoxColumn
            // 
            heightDataGridViewTextBoxColumn.DataPropertyName = "Height";
            heightDataGridViewTextBoxColumn.HeaderText = "Height";
            heightDataGridViewTextBoxColumn.Name = "heightDataGridViewTextBoxColumn";
            heightDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // scaleDataGridViewTextBoxColumn
            // 
            scaleDataGridViewTextBoxColumn.DataPropertyName = "Scale";
            scaleDataGridViewTextBoxColumn.HeaderText = "Scale";
            scaleDataGridViewTextBoxColumn.Name = "scaleDataGridViewTextBoxColumn";
            scaleDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // directionTypeDataGridViewTextBoxColumn
            // 
            directionTypeDataGridViewTextBoxColumn.DataPropertyName = "DirectionType";
            directionTypeDataGridViewTextBoxColumn.HeaderText = "DirectionType";
            directionTypeDataGridViewTextBoxColumn.Name = "directionTypeDataGridViewTextBoxColumn";
            directionTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // idDataGridViewTextBoxColumn
            // 
            idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn.HeaderText = "Id";
            idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            idDataGridViewTextBoxColumn.ReadOnly = true;
            idDataGridViewTextBoxColumn.Visible = false;
            // 
            // creationTimeDataGridViewTextBoxColumn
            // 
            creationTimeDataGridViewTextBoxColumn.DataPropertyName = "CreationTime";
            creationTimeDataGridViewTextBoxColumn.HeaderText = "CreationTime";
            creationTimeDataGridViewTextBoxColumn.Name = "creationTimeDataGridViewTextBoxColumn";
            creationTimeDataGridViewTextBoxColumn.ReadOnly = true;
            creationTimeDataGridViewTextBoxColumn.Visible = false;
            // 
            // jwBeamDatasBindingSource
            // 
            jwBeamDatasBindingSource.DataMember = "JwBeamDatas";
            jwBeamDatasBindingSource.DataSource = jwProjectSubDataBindingSource;
            // 
            // jwCanvasControl1
            // 
            jwCanvasControl1.BeamSelected = false;
            jwCanvasControl1.CanvasDraw = null;
            jwCanvasControl1.Dock = DockStyle.Fill;
            jwCanvasControl1.Location = new Point(0, 0);
            jwCanvasControl1.Margin = new Padding(4, 4, 4, 4);
            jwCanvasControl1.Name = "jwCanvasControl1";
            jwCanvasControl1.SelectBeamEvent = null;
            jwCanvasControl1.SelectedBeam = null;
            jwCanvasControl1.Size = new Size(1048, 1013);
            jwCanvasControl1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(uiSymbolButton1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1436, 46);
            panel2.TabIndex = 0;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(13, 5);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(205, 35);
            uiSymbolButton1.TabIndex = 0;
            uiSymbolButton1.Text = "設計図のエクスポート";
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // ShowJwCanvasForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1436, 1100);
            Controls.Add(panel1);
            Name = "ShowJwCanvasForm";
            Text = "設計図";
            WindowState = FormWindowState.Maximized;
            Load += ShowJwCanvasForm_Load;
            panel1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwLinkPartDatasBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwProjectSubDataBindingSource).EndInit();
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwPillarDatasBindingSource).EndInit();
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)jwBeamDatasBindingSource).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private SplitContainer splitContainer1;
        private Controls.JwCanvasControl jwCanvasControl1;
        private Panel panel2;
        private Panel panel4;
        private Panel panel5;
        private Panel panel3;
        private DataGridView dataGridView3;
        private BindingSource jwLinkPartDatasBindingSource;
        private BindingSource jwProjectSubDataBindingSource;
        private DataGridView dataGridView2;
        private BindingSource jwPillarDatasBindingSource;
        private DataGridView dataGridView1;
        private BindingSource jwBeamDatasBindingSource;
        private DataGridViewTextBoxColumn bujianNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn beamIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn directedDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn gouJianTypeDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn isLianjieDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn isNoBeamDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn jwProjectSubDataIdDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn jwProjectSubDataDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn locationDataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn widthDataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn heightDataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn scaleDataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn directionTypeDataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn creationTimeDataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn BeamCode;
        private DataGridViewTextBoxColumn locationDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn IsQiegeBeam;
        private DataGridViewTextBoxColumn Width;
        private DataGridViewTextBoxColumn heightDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn scaleDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn directionTypeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn creationTimeDataGridViewTextBoxColumn;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private SaveFileDialog saveFileDialog1;
        private DataGridViewTextBoxColumn pillarCodeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn TaggTitle;
        private DataGridViewTextBoxColumn blocksCountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn baseTypeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn firstLocationDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn firstWidthDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn firstHeightDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn centerLocationDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn centerWidthDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn centerHeightDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lastLocationDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lastWidthDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lastHeightDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn jwProjectSubDataIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn jwProjectSubDataDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn locationDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn widthDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn heightDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn scaleDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn directionTypeDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn creationTimeDataGridViewTextBoxColumn1;
    }
}