namespace RGBJWMain.Pages
{
    partial class SubDetail
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
            uiLine1 = new Sunny.UI.UILine();
            panel1 = new Panel();
            panel2 = new Panel();
            jwCanvasControl1 = new RGBJWMain.Controls.JwCanvasControl();
            panel3 = new Panel();
            uiTabControl1 = new Sunny.UI.UITabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            uiTabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // uiLine1
            // 
            uiLine1.BackColor = Color.Transparent;
            uiLine1.Dock = DockStyle.Top;
            uiLine1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLine1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLine1.Location = new Point(0, 0);
            uiLine1.MinimumSize = new Size(1, 1);
            uiLine1.Name = "uiLine1";
            uiLine1.Size = new Size(1704, 44);
            uiLine1.TabIndex = 0;
            uiLine1.Text = "uiLine1";
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 44);
            panel1.Name = "panel1";
            panel1.Size = new Size(1704, 959);
            panel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(jwCanvasControl1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1704, 578);
            panel2.TabIndex = 0;
            // 
            // jwCanvasControl1
            // 
            jwCanvasControl1.BeamSelected = false;
            jwCanvasControl1.CanvasDraw = null;
            jwCanvasControl1.Dock = DockStyle.Fill;
            jwCanvasControl1.Location = new Point(0, 0);
            jwCanvasControl1.Margin = new Padding(5, 4, 5, 4);
            jwCanvasControl1.Name = "jwCanvasControl1";
            jwCanvasControl1.SelectBeamEvent = null;
            jwCanvasControl1.SelectedBeam = null;
            jwCanvasControl1.Size = new Size(1704, 578);
            jwCanvasControl1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(uiTabControl1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 578);
            panel3.Name = "panel3";
            panel3.Size = new Size(1704, 381);
            panel3.TabIndex = 1;
            // 
            // uiTabControl1
            // 
            uiTabControl1.Controls.Add(tabPage1);
            uiTabControl1.Controls.Add(tabPage2);
            uiTabControl1.Dock = DockStyle.Fill;
            uiTabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            uiTabControl1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiTabControl1.ItemSize = new Size(150, 40);
            uiTabControl1.Location = new Point(0, 0);
            uiTabControl1.MainPage = "";
            uiTabControl1.Name = "uiTabControl1";
            uiTabControl1.SelectedIndex = 0;
            uiTabControl1.Size = new Size(1704, 381);
            uiTabControl1.SizeMode = TabSizeMode.Fixed;
            uiTabControl1.TabIndex = 0;
            uiTabControl1.TabUnSelectedForeColor = Color.FromArgb(240, 240, 240);
            uiTabControl1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(0, 40);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(1704, 341);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(0, 40);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(300, 110);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // SubDetail
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1704, 1003);
            Controls.Add(panel1);
            Controls.Add(uiLine1);
            Name = "SubDetail";
            Text = "SubDetail";
            Load += SubDetail_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            uiTabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UILine uiLine1;
        private Panel panel1;
        private Panel panel2;
        private Controls.JwCanvasControl jwCanvasControl1;
        private Panel panel3;
        private Sunny.UI.UITabControl uiTabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
    }
}