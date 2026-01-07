namespace RGBControls.Pages
{
    partial class NewBaseDataPage
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
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            table1 = new AntdUI.Table();
            divider1 = new AntdUI.Divider();
            button1 = new AntdUI.Button();
            panel2 = new Panel();
            table2 = new AntdUI.Table();
            divider2 = new AntdUI.Divider();
            button2 = new AntdUI.Button();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            divider1.SuspendLayout();
            panel2.SuspendLayout();
            divider2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1317, 993);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(table1);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(454, 987);
            panel1.TabIndex = 0;
            // 
            // table1
            // 
            table1.Dock = DockStyle.Fill;
            table1.Gap = 12;
            table1.Location = new Point(0, 52);
            table1.Name = "table1";
            table1.Size = new Size(452, 933);
            table1.TabIndex = 1;
            table1.Text = "table1";
            table1.SelectIndexChanged += table1_SelectIndexChanged;
            // 
            // divider1
            // 
            divider1.Controls.Add(button1);
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(452, 52);
            divider1.TabIndex = 0;
            divider1.Text = "データの種類";
            divider1.Thickness = 2F;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.Ghost = true;
            button1.IconSize = new Size(40, 40);
            button1.IconSvg = "PlusOutlined";
            button1.Location = new Point(340, 0);
            button1.Name = "button1";
            button1.Size = new Size(112, 52);
            button1.TabIndex = 0;
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(table2);
            panel2.Controls.Add(divider2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(463, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(851, 987);
            panel2.TabIndex = 1;
            // 
            // table2
            // 
            table2.Dock = DockStyle.Fill;
            table2.Gap = 12;
            table2.Location = new Point(0, 52);
            table2.Name = "table2";
            table2.Size = new Size(849, 933);
            table2.TabIndex = 1;
            table2.Text = "table2";
            // 
            // divider2
            // 
            divider2.Controls.Add(button2);
            divider2.Dock = DockStyle.Top;
            divider2.Location = new Point(0, 0);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(849, 52);
            divider2.TabIndex = 0;
            divider2.Text = "データ";
            divider2.Thickness = 2F;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Right;
            button2.Ghost = true;
            button2.IconSize = new Size(40, 40);
            button2.IconSvg = "PlusOutlined";
            button2.Location = new Point(737, 0);
            button2.Name = "button2";
            button2.Size = new Size(112, 52);
            button2.TabIndex = 0;
            button2.Type = AntdUI.TTypeMini.Success;
            button2.Click += button2_Click;
            // 
            // NewBaseDataPage
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1317, 993);
            Controls.Add(tableLayoutPanel1);
            Name = "NewBaseDataPage";
            Text = "データ管理";
            Load += NewBaseDataPage_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            divider1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            divider2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private AntdUI.Divider divider1;
        private AntdUI.Table table1;
        private Panel panel2;
        private AntdUI.Table table2;
        private AntdUI.Divider divider2;
        private AntdUI.Button button1;
        private AntdUI.Button button2;
    }
}