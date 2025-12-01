namespace RGBJWMain.Pages
{
    partial class TestsPage
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
            table1 = new AntdUI.Table();
            jwProjectMainDataBindingSource = new BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)jwProjectMainDataBindingSource).BeginInit();
            SuspendLayout();
            // 
            // table1
            // 
            table1.Dock = DockStyle.Fill;
            table1.Gap = 12;
            table1.Location = new Point(0, 0);
            table1.Name = "table1";
            table1.Size = new Size(800, 450);
            table1.TabIndex = 0;
            table1.Text = "table1";
            table1.CellClick += table1_CellClick;
            table1.SelectIndexChanged += table1_SelectIndexChanged;
            // 
            // jwProjectMainDataBindingSource
            // 
            jwProjectMainDataBindingSource.DataSource = typeof(JwCore.JwProjectMainData);
            // 
            // TestsPage
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(800, 450);
            Controls.Add(table1);
            Name = "TestsPage";
            Text = "TestsPage";
            Load += TestsPage_Load;
            ((System.ComponentModel.ISupportInitialize)jwProjectMainDataBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Table table1;
        private BindingSource jwProjectMainDataBindingSource;
    }
}