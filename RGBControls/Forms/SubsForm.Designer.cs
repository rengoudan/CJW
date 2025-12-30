namespace RGBControls.Forms
{
    partial class SubsForm
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubsForm));
            tabs1 = new AntdUI.Tabs();
            pageHeader1 = new AntdUI.PageHeader();
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            panel1 = new Panel();
            pageHeader1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tabs1
            // 
            tabs1.Dock = DockStyle.Fill;
            tabs1.Location = new Point(0, 0);
            tabs1.Name = "tabs1";
            tabs1.Size = new Size(800, 410);
            tabs1.Style = styleLine1;
            tabs1.TabIndex = 0;
            tabs1.Text = "tabs1";
            // 
            // pageHeader1
            // 
            pageHeader1.Controls.Add(button2);
            pageHeader1.Controls.Add(button1);
            pageHeader1.Dock = DockStyle.Top;
            pageHeader1.Icon = (Image)resources.GetObject("pageHeader1.Icon");
            pageHeader1.Location = new Point(0, 0);
            pageHeader1.Name = "pageHeader1";
            pageHeader1.ShowButton = true;
            pageHeader1.ShowIcon = true;
            pageHeader1.Size = new Size(800, 40);
            pageHeader1.TabIndex = 1;
            pageHeader1.Text = "pageHeader1";
            // 
            // button2
            // 
            button2.Dock = DockStyle.Right;
            button2.Ghost = true;
            button2.IconSvg = "PlusOutlined";
            button2.Location = new Point(492, 0);
            button2.Name = "button2";
            button2.Radius = 0;
            button2.Size = new Size(46, 40);
            button2.TabIndex = 1;
            button2.WaveSize = 0;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.Ghost = true;
            button1.IconSvg = "SettingOutlined";
            button1.Location = new Point(538, 0);
            button1.Name = "button1";
            button1.Radius = 0;
            button1.Size = new Size(46, 40);
            button1.TabIndex = 0;
            button1.WaveSize = 0;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(tabs1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 40);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 410);
            panel1.TabIndex = 2;
            // 
            // SubsForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(pageHeader1);
            Name = "SubsForm";
            Text = "SubsForm";
            Load += SubsForm_Load;
            pageHeader1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Tabs tabs1;
        private AntdUI.PageHeader pageHeader1;
        private Panel panel1;
        private AntdUI.Button button1;
        private AntdUI.Button button2;
    }
}