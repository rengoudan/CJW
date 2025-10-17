namespace RGBJWMain
{
    partial class JwMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JwMainForm));
            uiAvatar1 = new Sunny.UI.UIAvatar();
            uiLabel1 = new Sunny.UI.UILabel();
            pictureBox1 = new PictureBox();
            uiPanel1 = new Sunny.UI.UIPanel();
            uiPanel2 = new Sunny.UI.UIPanel();
            timer1 = new System.Windows.Forms.Timer(components);
            Footer.SuspendLayout();
            Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            uiPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // Footer
            // 
            Footer.Controls.Add(uiPanel1);
            Footer.Location = new Point(0, 687);
            Footer.Size = new Size(1144, 40);
            // 
            // Header
            // 
            Header.Controls.Add(pictureBox1);
            Header.Controls.Add(uiLabel1);
            Header.Controls.Add(uiAvatar1);
            Header.Size = new Size(1144, 67);
            // 
            // uiAvatar1
            // 
            uiAvatar1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            uiAvatar1.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiAvatar1.Location = new Point(1071, 0);
            uiAvatar1.MinimumSize = new Size(1, 1);
            uiAvatar1.Name = "uiAvatar1";
            uiAvatar1.Size = new Size(70, 65);
            uiAvatar1.TabIndex = 0;
            uiAvatar1.Text = "uiAvatar1";
            uiAvatar1.Click += uiAvatar1_Click;
            // 
            // uiLabel1
            // 
            uiLabel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            uiLabel1.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel1.Location = new Point(965, 23);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(100, 23);
            uiLabel1.TabIndex = 1;
            uiLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(252, 53);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // uiPanel1
            // 
            uiPanel1.Controls.Add(uiPanel2);
            uiPanel1.Dock = DockStyle.Bottom;
            uiPanel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiPanel1.Location = new Point(0, 0);
            uiPanel1.Margin = new Padding(4, 5, 4, 5);
            uiPanel1.MinimumSize = new Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.Size = new Size(1144, 40);
            uiPanel1.TabIndex = 0;
            uiPanel1.Text = null;
            uiPanel1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiPanel2
            // 
            uiPanel2.Dock = DockStyle.Right;
            uiPanel2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiPanel2.Location = new Point(910, 0);
            uiPanel2.Margin = new Padding(4, 5, 4, 5);
            uiPanel2.MinimumSize = new Size(1, 1);
            uiPanel2.Name = "uiPanel2";
            uiPanel2.Size = new Size(234, 40);
            uiPanel2.TabIndex = 0;
            uiPanel2.Text = null;
            uiPanel2.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // JwMainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1144, 727);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "JwMainForm";
            ShowTitleIcon = true;
            Text = "設計と加工の支援";
            ZoomScaleRect = new Rectangle(15, 15, 800, 450);
            Load += JwMainForm_Load;
            Shown += JwMainForm_Shown;
            Footer.ResumeLayout(false);
            Header.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            uiPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIAvatar uiAvatar1;
        private Sunny.UI.UILabel uiLabel1;
        private PictureBox pictureBox1;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UIPanel uiPanel2;
        private System.Windows.Forms.Timer timer1;
    }
}