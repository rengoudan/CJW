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
            panel1.Size = new Size(1422, 43);
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
            // 
            // jwProjectSubDatasBindingSource
            // 
            jwProjectSubDatasBindingSource.DataMember = "JwProjectSubDatas";
            jwProjectSubDatasBindingSource.DataSource = jwProjectMainDataBindingSource;
            // 
            // NewProjectMainPage
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1422, 865);
            Name = "NewProjectMainPage";
            Text = "NewProjectMainPage";
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
        private AntdUI.Table table1;
        private BindingSource jwProjectSubDatasBindingSource;
    }
}