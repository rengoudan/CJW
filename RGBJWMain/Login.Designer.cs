namespace RGBJWMain
{
    partial class Login
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
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Text = "JW-Extended Management";
            // 
            // lblSubText
            // 
            lblSubText.Location = new Point(376, 421);
            lblSubText.Text = "JW-Extended Management";
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(702, 12);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(36, 31);
            uiSymbolButton1.Symbol = 61459;
            uiSymbolButton1.TabIndex = 10;
            uiSymbolButton1.TipsFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // Login
            // 
            AutoScaleMode = AutoScaleMode.None;
            ButtonCancelText = "cancel";
            ButtonLoginText = "login";
            ClientSize = new Size(750, 450);
            Controls.Add(uiSymbolButton1);
            LoginText = "login";
            Name = "Login";
            PasswordWatermark = "Password";
            SubText = "JW-Extended Management";
            Text = "Login";
            Title = "JW-Extended Management";
            UserNameWatermark = "Account";
            ButtonLoginClick += Login_ButtonLoginClick;
            ButtonCancelClick += Login_ButtonCancelClick;
            FormClosed += Login_FormClosed;
            Controls.SetChildIndex(lblTitle, 0);
            Controls.SetChildIndex(lblSubText, 0);
            Controls.SetChildIndex(uiPanel1, 0);
            Controls.SetChildIndex(uiSymbolButton1, 0);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UISymbolButton uiSymbolButton1;
    }
}