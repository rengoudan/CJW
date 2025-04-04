namespace RGBJWMain.FormSet
{
    partial class SystemSetting
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
            uiLabel1 = new Sunny.UI.UILabel();
            uiTextBox1 = new Sunny.UI.UITextBox();
            pnlBtm.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBtm
            // 
            pnlBtm.Location = new Point(1, 236);
            pnlBtm.Size = new Size(540, 55);
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(412, 12);
            // 
            // btnOK
            // 
            btnOK.Location = new Point(297, 12);
            btnOK.Click += btnOK_Click;
            // 
            // uiLabel1
            // 
            uiLabel1.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel1.Location = new Point(4, 65);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(136, 23);
            uiLabel1.TabIndex = 2;
            uiLabel1.Text = "ServerAddress：";
            uiLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiTextBox1
            // 
            uiTextBox1.ButtonSymbolOffset = new Point(0, 0);
            uiTextBox1.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiTextBox1.Location = new Point(147, 59);
            uiTextBox1.Margin = new Padding(4, 5, 4, 5);
            uiTextBox1.MinimumSize = new Size(1, 16);
            uiTextBox1.Name = "uiTextBox1";
            uiTextBox1.Padding = new Padding(5);
            uiTextBox1.ShowText = false;
            uiTextBox1.Size = new Size(328, 29);
            uiTextBox1.TabIndex = 3;
            uiTextBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiTextBox1.Watermark = "";
            // 
            // SystemSetting
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(542, 294);
            Controls.Add(uiTextBox1);
            Controls.Add(uiLabel1);
            Name = "SystemSetting";
            Text = "SystemSetting";
            ZoomScaleRect = new Rectangle(15, 15, 800, 450);
            Load += SystemSetting_Load;
            Controls.SetChildIndex(pnlBtm, 0);
            Controls.SetChildIndex(uiLabel1, 0);
            Controls.SetChildIndex(uiTextBox1, 0);
            pnlBtm.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UITextBox uiTextBox1;
    }
}