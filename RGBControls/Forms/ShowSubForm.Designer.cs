namespace RGBControls.Forms
{
    partial class ShowSubForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowSubForm));
            pageHeader1 = new AntdUI.PageHeader();
            SuspendLayout();
            // 
            // pageHeader1
            // 
            pageHeader1.Dock = DockStyle.Top;
            pageHeader1.Icon = (Image)resources.GetObject("pageHeader1.Icon");
            pageHeader1.Location = new Point(0, 0);
            pageHeader1.Name = "pageHeader1";
            pageHeader1.ShowButton = true;
            pageHeader1.Size = new Size(800, 40);
            pageHeader1.TabIndex = 0;
            pageHeader1.Text = "pageHeader1";
            // 
            // ShowSubForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pageHeader1);
            Name = "ShowSubForm";
            Text = "ShowSubForm";
            Load += ShowSubForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader pageHeader1;
    }
}