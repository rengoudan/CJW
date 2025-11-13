namespace RGBControls
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            pageHeader1 = new AntdUI.PageHeader();
            button1 = new AntdUI.Button();
            panel1 = new Panel();
            uiTabControl1 = new Sunny.UI.UITabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            uiNavBar1 = new Sunny.UI.UINavBar();
            pictureBox1 = new PictureBox();
            pageHeader1.SuspendLayout();
            panel1.SuspendLayout();
            uiTabControl1.SuspendLayout();
            uiNavBar1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pageHeader1
            // 
            pageHeader1.Controls.Add(button1);
            pageHeader1.DividerThickness = 3F;
            pageHeader1.Dock = DockStyle.Top;
            pageHeader1.Icon = (Image)resources.GetObject("pageHeader1.Icon");
            pageHeader1.Location = new Point(0, 0);
            pageHeader1.Name = "pageHeader1";
            pageHeader1.ShowButton = true;
            pageHeader1.ShowIcon = true;
            pageHeader1.Size = new Size(1261, 40);
            pageHeader1.TabIndex = 0;
            pageHeader1.Text = "JW設計図の自動認識-予算を自動生成";
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.Ghost = true;
            button1.IconSvg = "SettingOutlined";
            button1.Location = new Point(933, 0);
            button1.Name = "button1";
            button1.Size = new Size(112, 40);
            button1.TabIndex = 0;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(uiTabControl1);
            panel1.Controls.Add(uiNavBar1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 40);
            panel1.Name = "panel1";
            panel1.Size = new Size(1261, 794);
            panel1.TabIndex = 1;
            // 
            // uiTabControl1
            // 
            uiTabControl1.Controls.Add(tabPage1);
            uiTabControl1.Controls.Add(tabPage2);
            uiTabControl1.Dock = DockStyle.Fill;
            uiTabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            uiTabControl1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiTabControl1.ItemSize = new Size(0, 1);
            uiTabControl1.Location = new Point(0, 78);
            uiTabControl1.MainPage = "";
            uiTabControl1.Name = "uiTabControl1";
            uiTabControl1.SelectedIndex = 0;
            uiTabControl1.Size = new Size(1261, 716);
            uiTabControl1.SizeMode = TabSizeMode.Fixed;
            uiTabControl1.TabIndex = 1;
            uiTabControl1.TabUnSelectedForeColor = Color.FromArgb(240, 240, 240);
            uiTabControl1.TabVisible = false;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(0, 0);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(1261, 716);
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
            // uiNavBar1
            // 
            uiNavBar1.BackColor = SystemColors.ButtonFace;
            uiNavBar1.Controls.Add(pictureBox1);
            uiNavBar1.Dock = DockStyle.Top;
            uiNavBar1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiNavBar1.ForeColor = Color.FromArgb(48, 48, 48);
            uiNavBar1.Location = new Point(0, 0);
            uiNavBar1.MenuStyle = Sunny.UI.UIMenuStyle.Custom;
            uiNavBar1.Name = "uiNavBar1";
            uiNavBar1.Size = new Size(1261, 78);
            uiNavBar1.TabControl = uiTabControl1;
            uiNavBar1.TabIndex = 0;
            uiNavBar1.Text = "uiNavBar1";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 19);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(252, 53);
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1261, 834);
            Controls.Add(panel1);
            Controls.Add(pageHeader1);
            Name = "MainForm";
            Text = "MainForm";
            WindowState = FormWindowState.Maximized;
            pageHeader1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            uiTabControl1.ResumeLayout(false);
            uiNavBar1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader pageHeader1;
        private AntdUI.Button button1;
        private Panel panel1;
        private Sunny.UI.UINavBar uiNavBar1;
        private Sunny.UI.UITabControl uiTabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private PictureBox pictureBox1;
    }
}