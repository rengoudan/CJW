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
            panel1 = new Panel();
            switch1 = new AntdUI.Switch();
            uiComboBox1 = new Sunny.UI.UIComboBox();
            panel4 = new Panel();
            button1 = new AntdUI.Button();
            uiSymbolButton2 = new Sunny.UI.UISymbolButton();
            pageHeader1 = new AntdUI.PageHeader();
            panel5 = new Panel();
            select7 = new AntdUI.Select();
            button2 = new AntdUI.Button();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            panel2 = new Panel();
            panel6 = new Panel();
            panel3 = new Panel();
            uiTextBox1 = new Sunny.UI.UITextBox();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(switch1);
            panel1.Controls.Add(uiComboBox1);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(uiSymbolButton2);
            panel1.Controls.Add(pageHeader1);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(uiSymbolButton1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1281, 92);
            panel1.TabIndex = 1;
            // 
            // switch1
            // 
            switch1.CheckedText = "CSV出力する";
            switch1.Location = new Point(866, 55);
            switch1.Name = "switch1";
            switch1.Size = new Size(142, 32);
            switch1.TabIndex = 33;
            switch1.Text = "switch1";
            switch1.UnCheckedText = "出力なし";
            
            // 
            // uiComboBox1
            // 
            uiComboBox1.DataSource = null;
            uiComboBox1.FillColor = Color.White;
            uiComboBox1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox1.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiComboBox1.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiComboBox1.Location = new Point(599, 58);
            uiComboBox1.Margin = new Padding(3, 4, 3, 4);
            uiComboBox1.MinimumSize = new Size(52, 0);
            uiComboBox1.Name = "uiComboBox1";
            uiComboBox1.Padding = new Padding(0, 0, 30, 2);
            uiComboBox1.Size = new Size(166, 25);
            uiComboBox1.SymbolSize = 24;
            uiComboBox1.TabIndex = 2;
            uiComboBox1.Text = "uiComboBox1";
            uiComboBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox1.Watermark = "";
            // 
            // panel4
            // 
            panel4.Controls.Add(button1);
            panel4.Location = new Point(599, 53);
            panel4.Margin = new Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new Size(204, 33);
            panel4.TabIndex = 31;
            panel4.Text = "panel4";
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.IconSvg = "CheckOutlined";
            button1.JoinMode = AntdUI.TJoinMode.Right;
            button1.Location = new Point(163, 0);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(41, 33);
            button1.TabIndex = 1;
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // uiSymbolButton2
            // 
            uiSymbolButton2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton2.Location = new Point(419, 57);
            uiSymbolButton2.Margin = new Padding(2);
            uiSymbolButton2.MinimumSize = new Size(1, 1);
            uiSymbolButton2.Name = "uiSymbolButton2";
            uiSymbolButton2.Size = new Size(162, 30);
            uiSymbolButton2.Symbol = 261891;
            uiSymbolButton2.TabIndex = 32;
            uiSymbolButton2.Text = "梁レシピCSV";
            uiSymbolButton2.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton2.Click += uiSymbolButton2_Click;
            // 
            // pageHeader1
            // 
            pageHeader1.Dock = DockStyle.Top;
            pageHeader1.Location = new Point(0, 0);
            pageHeader1.Margin = new Padding(2);
            pageHeader1.Name = "pageHeader1";
            pageHeader1.ShowButton = true;
            pageHeader1.Size = new Size(1281, 44);
            pageHeader1.TabIndex = 31;
            pageHeader1.Text = "梁预览";
            // 
            // panel5
            // 
            panel5.Controls.Add(select7);
            panel5.Controls.Add(button2);
            panel5.Location = new Point(195, 57);
            panel5.Margin = new Padding(2);
            panel5.Name = "panel5";
            panel5.Size = new Size(204, 33);
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
            select7.Margin = new Padding(2);
            select7.Name = "select7";
            select7.PlaceholderText = "工区選択";
            select7.Size = new Size(163, 33);
            select7.TabIndex = 0;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Right;
            button2.IconSvg = "CheckOutlined";
            button2.JoinMode = AntdUI.TJoinMode.Right;
            button2.Location = new Point(163, 0);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(41, 33);
            button2.TabIndex = 1;
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += button2_Click;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(11, 57);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(165, 30);
            uiSymbolButton1.Symbol = 558052;
            uiSymbolButton1.TabIndex = 0;
            uiSymbolButton1.Text = "梁図JW";
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel6);
            panel2.Controls.Add(panel3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 92);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(1281, 672);
            panel2.TabIndex = 2;
            // 
            // panel6
            // 
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(0, 0);
            panel6.Margin = new Padding(2);
            panel6.Name = "panel6";
            panel6.Size = new Size(1281, 440);
            panel6.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(uiTextBox1);
            panel3.Controls.Add(divider1);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 440);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(1281, 232);
            panel3.TabIndex = 1;
            // 
            // uiTextBox1
            // 
            uiTextBox1.Dock = DockStyle.Fill;
            uiTextBox1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiTextBox1.Location = new Point(0, 28);
            uiTextBox1.Margin = new Padding(3, 4, 3, 4);
            uiTextBox1.MinimumSize = new Size(1, 13);
            uiTextBox1.Multiline = true;
            uiTextBox1.Name = "uiTextBox1";
            uiTextBox1.Padding = new Padding(4);
            uiTextBox1.ShowScrollBar = true;
            uiTextBox1.ShowText = false;
            uiTextBox1.Size = new Size(1281, 204);
            uiTextBox1.TabIndex = 1;
            uiTextBox1.Text = "uiTextBox1";
            uiTextBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiTextBox1.Watermark = "";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(0, 0);
            divider1.Margin = new Padding(2);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(1281, 28);
            divider1.TabIndex = 0;
            divider1.Text = "プレビューCSV";
            // 
            // NewJwBeamForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1281, 764);
            Controls.Add(panel2);
            Controls.Add(panel1);
            HelpButton = true;
            Margin = new Padding(2);
            Name = "NewJwBeamForm";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "NewJwBeamForm";
            Load += NewJwBeamForm_Load;
            Shown += NewJwBeamForm_Shown;
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
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
        private Sunny.UI.UISymbolButton uiSymbolButton2;
        private Panel panel4;
        private AntdUI.Button button1;
        private Sunny.UI.UIComboBox uiComboBox1;
        private Panel panel6;
        private AntdUI.Switch switch1;
    }
}