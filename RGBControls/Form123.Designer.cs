namespace RGBControls
{
    partial class Form123
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
            ttest1 = new RGBControls.Controls.ttest();
            SuspendLayout();
            // 
            // ttest1
            // 
            ttest1.BackColor = Color.Black;
            ttest1.Dock = DockStyle.Fill;
            ttest1.Location = new Point(0, 0);
            ttest1.Name = "ttest1";
            ttest1.Size = new Size(1188, 739);
            ttest1.TabIndex = 0;
            ttest1.Text = "ttest1";
            // 
            // Form123
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1188, 739);
            Controls.Add(ttest1);
            Name = "Form123";
            Text = "Form123";
            ResumeLayout(false);
        }

        #endregion

        private Controls.ttest ttest1;
    }
}