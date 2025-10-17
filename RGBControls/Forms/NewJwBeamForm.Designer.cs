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
            panel1 = new Panel();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // newSingleBeamControl1
            // 
            newSingleBeamControl1.BackColor = Color.Black;
            newSingleBeamControl1.Dock = DockStyle.Bottom;
            newSingleBeamControl1.Location = new Point(0, 57);
            newSingleBeamControl1.Margin = new Padding(2);
            newSingleBeamControl1.Name = "newSingleBeamControl1";
            newSingleBeamControl1.ShowBeam = null;
            newSingleBeamControl1.Size = new Size(1374, 501);
            newSingleBeamControl1.TabIndex = 0;
            newSingleBeamControl1.Text = "newSingleBeamControl1";
            // 
            // panel1
            // 
            panel1.Controls.Add(uiSymbolButton1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1374, 52);
            panel1.TabIndex = 1;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(12, 12);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(165, 35);
            uiSymbolButton1.Symbol = 558052;
            uiSymbolButton1.TabIndex = 0;
            uiSymbolButton1.Text = "輸出JWW";
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // NewJwBeamForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1374, 558);
            Controls.Add(panel1);
            Controls.Add(newSingleBeamControl1);
            Margin = new Padding(2);
            Name = "NewJwBeamForm";
            Text = "NewJwBeamForm";
            Shown += NewJwBeamForm_Shown;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Controls.NewSingleBeamControl newSingleBeamControl1;
        private Panel panel1;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
    }
}