namespace RGBJWMain.Forms
{
    partial class NewJwBeamForm
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
            newSingleBeamControl1 = new Controls.NewSingleBeamControl();
            SuspendLayout();
            // 
            // newSingleBeamControl1
            // 
            newSingleBeamControl1.BackColor = Color.Black;
            newSingleBeamControl1.Dock = DockStyle.Fill;
            newSingleBeamControl1.Location = new Point(0, 0);
            newSingleBeamControl1.Name = "newSingleBeamControl1";
            newSingleBeamControl1.ShowBeam = null;
            newSingleBeamControl1.Size = new Size(1679, 669);
            newSingleBeamControl1.TabIndex = 0;
            newSingleBeamControl1.Text = "newSingleBeamControl1";
            // 
            // NewJwBeamForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1679, 669);
            Controls.Add(newSingleBeamControl1);
            Name = "NewJwBeamForm";
            Text = "NewJwBeamForm";
            Shown += NewJwBeamForm_Shown;
            ResumeLayout(false);
        }

        #endregion

        private Controls.NewSingleBeamControl newSingleBeamControl1;
    }
}