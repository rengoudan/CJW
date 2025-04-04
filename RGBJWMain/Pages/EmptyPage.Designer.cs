namespace RGBJWMain.Pages
{
    partial class EmptyPage
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
            uiLabel2 = new Sunny.UI.UILabel();
            uiLabel3 = new Sunny.UI.UILabel();
            uiLabel4 = new Sunny.UI.UILabel();
            uiLabel5 = new Sunny.UI.UILabel();
            SuspendLayout();
            // 
            // uiLabel2
            // 
            uiLabel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            uiLabel2.Font = new Font("微软雅黑", 24F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel2.Location = new Point(0, 0);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(800, 96);
            uiLabel2.TabIndex = 1;
            uiLabel2.Text = "1.設計図はフロアごとに別々のファイルが必要";
            uiLabel2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel3
            // 
            uiLabel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            uiLabel3.Font = new Font("微软雅黑", 24F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel3.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel3.Location = new Point(0, 81);
            uiLabel3.Name = "uiLabel3";
            uiLabel3.Size = new Size(800, 96);
            uiLabel3.TabIndex = 2;
            uiLabel3.Text = "2.一度に1つのフロアプランを特定する";
            uiLabel3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel4
            // 
            uiLabel4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            uiLabel4.Font = new Font("微软雅黑", 24F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel4.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel4.Location = new Point(0, 162);
            uiLabel4.Name = "uiLabel4";
            uiLabel4.Size = new Size(800, 96);
            uiLabel4.TabIndex = 3;
            uiLabel4.Text = "3.統一された設計基準";
            uiLabel4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel5
            // 
            uiLabel5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            uiLabel5.Font = new Font("微软雅黑", 24F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel5.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel5.Location = new Point(0, 238);
            uiLabel5.Name = "uiLabel5";
            uiLabel5.Size = new Size(800, 124);
            uiLabel5.TabIndex = 4;
            uiLabel5.Text = "4.自動予算機能の前提は、見積資料情報を構成する予算項目情報を維持することです。";
            uiLabel5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // EmptyPage
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(800, 450);
            Controls.Add(uiLabel5);
            Controls.Add(uiLabel4);
            Controls.Add(uiLabel3);
            Controls.Add(uiLabel2);
            Name = "EmptyPage";
            PageIndex = 1000;
            Text = "ワークベンチ";
            ResumeLayout(false);
        }

        #endregion
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UILabel uiLabel5;
    }
}