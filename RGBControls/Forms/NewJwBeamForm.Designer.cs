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
            newSingleBeamControl1 = new RGBJWMain.Controls.NewSingleBeamControl();
            panel1 = new Panel();
            pageHeader1 = new AntdUI.PageHeader();
            panel5 = new Panel();
            select7 = new AntdUI.Select();
            button2 = new AntdUI.Button();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            panel2 = new Panel();
            panel3 = new Panel();
            uiTextBox1 = new Sunny.UI.UITextBox();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // newSingleBeamControl1
            // 
            newSingleBeamControl1.BackColor = Color.Black;
            newSingleBeamControl1.Dock = DockStyle.Top;
            newSingleBeamControl1.Location = new Point(0, 0);
            newSingleBeamControl1.Margin = new Padding(2);
            newSingleBeamControl1.Name = "newSingleBeamControl1";
            newSingleBeamControl1.ShowBeam = null;
            newSingleBeamControl1.Size = new Size(1562, 563);
            newSingleBeamControl1.TabIndex = 0;
            newSingleBeamControl1.Text = "newSingleBeamControl1";
            // 
            // panel1
            // 
            panel1.Controls.Add(pageHeader1);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(uiSymbolButton1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1562, 111);
            panel1.TabIndex = 1;
            // 
            // pageHeader1
            // 
            pageHeader1.Dock = DockStyle.Top;
            pageHeader1.Location = new Point(0, 0);
            pageHeader1.Name = "pageHeader1";
            pageHeader1.ShowButton = true;
            pageHeader1.Size = new Size(1562, 53);
            pageHeader1.TabIndex = 31;
            pageHeader1.Text = "梁预览";
            // 
            // panel5
            // 
            panel5.Controls.Add(select7);
            panel5.Controls.Add(button2);
            panel5.Location = new Point(238, 68);
            panel5.Name = "panel5";
            panel5.Size = new Size(249, 40);
            panel5.TabIndex = 30;
            panel5.Text = "panel4";
            // 
            // select7
            // 
            select7.AllowClear = true;
            select7.Dock = DockStyle.Fill;
            select7.Items.AddRange(new object[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" });
            select7.JoinMode = AntdUI.TJoinMode.Left;
            select7.LocalizationPlaceholderText = "Select.{id}";
            select7.Location = new Point(0, 0);
            select7.Name = "select7";
            select7.PlaceholderText = "工区選択";
            select7.Size = new Size(199, 40);
            select7.TabIndex = 0;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Right;
            button2.IconSvg = "CheckOutlined";
            button2.JoinMode = AntdUI.TJoinMode.Right;
            button2.Location = new Point(199, 0);
            button2.Name = "button2";
            button2.Size = new Size(50, 40);
            button2.TabIndex = 1;
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += button2_Click;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(13, 68);
            uiSymbolButton1.Margin = new Padding(4);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(202, 36);
            uiSymbolButton1.Symbol = 558052;
            uiSymbolButton1.TabIndex = 0;
            uiSymbolButton1.Text = "輸出JWW";
            uiSymbolButton1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(newSingleBeamControl1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 111);
            panel2.Name = "panel2";
            panel2.Size = new Size(1562, 798);
            panel2.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(uiTextBox1);
            panel3.Controls.Add(divider1);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 519);
            panel3.Name = "panel3";
            panel3.Size = new Size(1562, 279);
            panel3.TabIndex = 1;
            // 
            // uiTextBox1
            // 
            uiTextBox1.Dock = DockStyle.Fill;
            uiTextBox1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiTextBox1.Location = new Point(0, 34);
            uiTextBox1.Margin = new Padding(4, 5, 4, 5);
            uiTextBox1.MinimumSize = new Size(1, 16);
            uiTextBox1.Multiline = true;
            uiTextBox1.Name = "uiTextBox1";
            uiTextBox1.Padding = new Padding(5);
            uiTextBox1.ShowScrollBar = true;
            uiTextBox1.ShowText = false;
            uiTextBox1.Size = new Size(1562, 245);
            uiTextBox1.TabIndex = 1;
            uiTextBox1.Text = "uiTextBox1";
            uiTextBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiTextBox1.Watermark = "";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(1562, 34);
            divider1.TabIndex = 0;
            divider1.Text = "プレビューCSV";
            // 
            // NewJwBeamForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1562, 909);
            Controls.Add(panel2);
            Controls.Add(panel1);
            HelpButton = true;
            Margin = new Padding(2);
            Name = "NewJwBeamForm";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "NewJwBeamForm";
            Shown += NewJwBeamForm_Shown;
            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Controls.NewSingleBeamControl newSingleBeamControl1;
        private Panel panel1;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Panel panel2;
        private Panel panel3;
        private AntdUI.Divider divider1;
        private Sunny.UI.UITextBox uiTextBox1;
        private Panel panel5;
        private AntdUI.Select select7;
        private AntdUI.Button button2;
        private AntdUI.PageHeader pageHeader1;
    }
}