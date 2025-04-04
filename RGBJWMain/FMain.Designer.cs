namespace RGBJWMain
{
    partial class FMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            uiNavBar1 = new Sunny.UI.UINavBar();
            pictureBox1 = new PictureBox();
            uiTabControl1 = new Sunny.UI.UITabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            uiNavBar1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            uiTabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // uiNavBar1
            // 
            uiNavBar1.BackColor = Color.FromArgb(240, 240, 240);
            uiNavBar1.Controls.Add(pictureBox1);
            uiNavBar1.Dock = DockStyle.Top;
            uiNavBar1.DropMenuFont = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiNavBar1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiNavBar1.ForeColor = Color.FromArgb(48, 48, 48);
            uiNavBar1.Location = new Point(0, 35);
            uiNavBar1.MenuHoverColor = Color.FromArgb(230, 230, 230);
            uiNavBar1.MenuSelectedColor = Color.FromArgb(250, 250, 250);
            uiNavBar1.MenuStyle = Sunny.UI.UIMenuStyle.White;
            uiNavBar1.Name = "uiNavBar1";
            uiNavBar1.Size = new Size(1276, 56);
            uiNavBar1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(252, 53);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // uiTabControl1
            // 
            uiTabControl1.Controls.Add(tabPage1);
            uiTabControl1.Controls.Add(tabPage2);
            uiTabControl1.Dock = DockStyle.Fill;
            uiTabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            uiTabControl1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiTabControl1.ItemSize = new Size(0, 1);
            uiTabControl1.Location = new Point(0, 91);
            uiTabControl1.MainPage = "";
            uiTabControl1.Name = "uiTabControl1";
            uiTabControl1.SelectedIndex = 0;
            uiTabControl1.Size = new Size(1276, 941);
            uiTabControl1.SizeMode = TabSizeMode.Fixed;
            uiTabControl1.TabIndex = 2;
            uiTabControl1.TabUnSelectedForeColor = Color.FromArgb(240, 240, 240);
            uiTabControl1.TabVisible = false;
            uiTabControl1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(0, 0);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(1276, 941);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(0, 40);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(200, 60);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // FMain
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1276, 1032);
            Controls.Add(uiTabControl1);
            Controls.Add(uiNavBar1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(1920, 1032);
            Name = "FMain";
            ShowTitleIcon = true;
            Text = "JW設計図の自動認識-予算を自動生成";
            WindowState = FormWindowState.Maximized;
            uiNavBar1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            uiTabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UINavBar uiNavBar1;
        private Sunny.UI.UITabControl uiTabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private PictureBox pictureBox1;
    }
}