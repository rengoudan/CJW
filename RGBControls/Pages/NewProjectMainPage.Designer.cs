namespace RGBControls.Pages
{
    partial class NewProjectMainPage
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
            projectmaintable = new AntdUI.Table();
            jwProjectMainDataBindingSource = new BindingSource(components);
            panel3 = new Panel();
            table1 = new AntdUI.Table();
            jwProjectSubDatasBindingSource = new BindingSource(components);
            button1 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)jwProjectMainDataBindingSource).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)jwProjectSubDatasBindingSource).BeginInit();
            SuspendLayout();
            // 
            // uiLine1
            // 
            uiLine1.Size = new Size(1422, 29);
            // 
            // panel1
            // 
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Size = new Size(1422, 43);
            panel1.Controls.SetChildIndex(uiButton1, 0);
            panel1.Controls.SetChildIndex(button1, 0);
            panel1.Controls.SetChildIndex(button2, 0);
            panel1.Controls.SetChildIndex(button3, 0);
            // 
            // panel2
            // 
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(projectmaintable);
            panel2.Size = new Size(1422, 793);
            // 
            // projectmaintable
            // 
            projectmaintable.Dock = DockStyle.Top;
            projectmaintable.Gap = 12;
            projectmaintable.Location = new Point(0, 0);
            projectmaintable.Name = "projectmaintable";
            projectmaintable.Size = new Size(1422, 381);
            projectmaintable.TabIndex = 0;
            projectmaintable.CellDoubleClick += projectmaintable_CellDoubleClick;
            projectmaintable.SelectIndexChanged += projectmaintable_SelectIndexChanged;
            // 
            // jwProjectMainDataBindingSource
            // 
            jwProjectMainDataBindingSource.DataSource = typeof(JwCore.JwProjectMainData);
            // 
            // panel3
            // 
            panel3.Controls.Add(table1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 381);
            panel3.Name = "panel3";
            panel3.Size = new Size(1422, 412);
            panel3.TabIndex = 1;
            // 
            // table1
            // 
            table1.Dock = DockStyle.Fill;
            table1.Gap = 12;
            table1.Location = new Point(0, 0);
            table1.Name = "table1";
            table1.Size = new Size(1422, 412);
            table1.TabIndex = 0;
            table1.Text = "table1";
            table1.CellClick += table1_CellClick;
            table1.CellHover += table1_CellHover;
            table1.CellDoubleClick += table1_CellDoubleClick;
            // 
            // jwProjectSubDatasBindingSource
            // 
            jwProjectSubDatasBindingSource.DataMember = "JwProjectSubDatas";
            jwProjectSubDatasBindingSource.DataSource = jwProjectMainDataBindingSource;
            // 
            // button1
            // 
            button1.IconSvg = "PlusOutlined";
            button1.Location = new Point(131, 6);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 1;
            button1.Text = "增";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.IconSvg = "UploadOutlined";
            button2.Location = new Point(249, 6);
            button2.Name = "button2";
            button2.Size = new Size(188, 34);
            button2.TabIndex = 2;
            button2.Text = "アップロード";
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.IconSvg = "BgColorsOutlined";
            button3.Location = new Point(447, 4);
            button3.Name = "button3";
            button3.Size = new Size(188, 34);
            button3.TabIndex = 3;
            button3.Text = "テストカラー";
            button3.Type = AntdUI.TTypeMini.Primary;
            button3.Click += button3_Click;
            // 
            // NewProjectMainPage
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1422, 865);
            Name = "NewProjectMainPage";
            Text = "プロジェクト";
            FormClosed += NewProjectMainPage_FormClosed;
            Load += NewProjectMainPage_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)jwProjectMainDataBindingSource).EndInit();
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)jwProjectSubDatasBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Table projectmaintable;
        private BindingSource jwProjectMainDataBindingSource;
        private Panel panel3;
        private BindingSource jwProjectSubDatasBindingSource;
        private AntdUI.Button button1;
        private AntdUI.Button button2;
        private AntdUI.Table table1;
        private AntdUI.Button button3;
    }
}