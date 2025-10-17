namespace RGBJWMain.Forms
{
    partial class AddBudget
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
            uiComboBox1 = new Sunny.UI.UIComboBox();
            bindingSource1 = new BindingSource(components);
            uiMarkLabel6 = new Sunny.UI.UIMarkLabel();
            uiMarkLabel1 = new Sunny.UI.UIMarkLabel();
            uiMarkLabel2 = new Sunny.UI.UIMarkLabel();
            uiLabel1 = new Sunny.UI.UILabel();
            uiLabel2 = new Sunny.UI.UILabel();
            uiMarkLabel3 = new Sunny.UI.UIMarkLabel();
            uiTextBox1 = new Sunny.UI.UITextBox();
            panel1 = new Panel();
            uiSymbolButton1 = new Sunny.UI.UISymbolButton();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // uiComboBox1
            // 
            uiComboBox1.DataBindings.Add(new Binding("SelectedItem", bindingSource1, "MaterialDescription", true));
            uiComboBox1.DataBindings.Add(new Binding("SelectedValue", bindingSource1, "Id", true));
            uiComboBox1.DataSource = null;
            uiComboBox1.FillColor = Color.White;
            uiComboBox1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiComboBox1.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiComboBox1.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiComboBox1.Location = new Point(201, 82);
            uiComboBox1.Margin = new Padding(4, 5, 4, 5);
            uiComboBox1.MinimumSize = new Size(63, 0);
            uiComboBox1.Name = "uiComboBox1";
            uiComboBox1.Padding = new Padding(0, 0, 30, 2);
            uiComboBox1.Size = new Size(327, 29);
            uiComboBox1.SymbolSize = 24;
            uiComboBox1.TabIndex = 0;
            uiComboBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBox1.Watermark = "";
            // 
            // bindingSource1
            // 
            bindingSource1.DataSource = typeof(JwCore.JwMaterialData);
            // 
            // uiMarkLabel6
            // 
            uiMarkLabel6.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel6.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel6.Location = new Point(57, 82);
            uiMarkLabel6.Name = "uiMarkLabel6";
            uiMarkLabel6.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel6.Size = new Size(119, 23);
            uiMarkLabel6.TabIndex = 24;
            uiMarkLabel6.Text = "予算項目";
            uiMarkLabel6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel1
            // 
            uiMarkLabel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel1.Location = new Point(57, 146);
            uiMarkLabel1.Name = "uiMarkLabel1";
            uiMarkLabel1.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel1.Size = new Size(119, 23);
            uiMarkLabel1.TabIndex = 25;
            uiMarkLabel1.Text = "単 位";
            uiMarkLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel2
            // 
            uiMarkLabel2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel2.Location = new Point(57, 208);
            uiMarkLabel2.Name = "uiMarkLabel2";
            uiMarkLabel2.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel2.Size = new Size(119, 23);
            uiMarkLabel2.TabIndex = 26;
            uiMarkLabel2.Text = "単 価";
            uiMarkLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel1
            // 
            uiLabel1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel1.Location = new Point(201, 146);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(100, 23);
            uiLabel1.TabIndex = 27;
            uiLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel2
            // 
            uiLabel2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel2.Location = new Point(201, 208);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(100, 23);
            uiLabel2.TabIndex = 28;
            uiLabel2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiMarkLabel3
            // 
            uiMarkLabel3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiMarkLabel3.ForeColor = Color.FromArgb(48, 48, 48);
            uiMarkLabel3.Location = new Point(57, 266);
            uiMarkLabel3.Name = "uiMarkLabel3";
            uiMarkLabel3.Padding = new Padding(5, 0, 0, 0);
            uiMarkLabel3.Size = new Size(119, 23);
            uiMarkLabel3.TabIndex = 29;
            uiMarkLabel3.Text = "量";
            uiMarkLabel3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiTextBox1
            // 
            uiTextBox1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiTextBox1.Location = new Point(201, 266);
            uiTextBox1.Margin = new Padding(4, 5, 4, 5);
            uiTextBox1.MinimumSize = new Size(1, 16);
            uiTextBox1.Name = "uiTextBox1";
            uiTextBox1.Padding = new Padding(5);
            uiTextBox1.ShowText = false;
            uiTextBox1.Size = new Size(119, 29);
            uiTextBox1.TabIndex = 30;
            uiTextBox1.TextAlignment = ContentAlignment.MiddleLeft;
            uiTextBox1.Watermark = "";
            // 
            // panel1
            // 
            panel1.Controls.Add(uiSymbolButton1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 384);
            panel1.Name = "panel1";
            panel1.Size = new Size(558, 47);
            panel1.TabIndex = 31;
            // 
            // uiSymbolButton1
            // 
            uiSymbolButton1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Location = new Point(443, 5);
            uiSymbolButton1.MinimumSize = new Size(1, 1);
            uiSymbolButton1.Name = "uiSymbolButton1";
            uiSymbolButton1.Size = new Size(112, 39);
            uiSymbolButton1.TabIndex = 0;
            uiSymbolButton1.Text = "もちろん";
            uiSymbolButton1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            uiSymbolButton1.Click += uiSymbolButton1_Click;
            // 
            // AddBudget
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(558, 431);
            Controls.Add(panel1);
            Controls.Add(uiTextBox1);
            Controls.Add(uiMarkLabel3);
            Controls.Add(uiLabel2);
            Controls.Add(uiLabel1);
            Controls.Add(uiMarkLabel2);
            Controls.Add(uiMarkLabel1);
            Controls.Add(uiMarkLabel6);
            Controls.Add(uiComboBox1);
            Name = "AddBudget";
            Text = "カスタム予算を追加する";
            Load += AddBudget_Load;
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIComboBox uiComboBox1;
        private Sunny.UI.UIMarkLabel uiMarkLabel6;
        private BindingSource bindingSource1;
        private Sunny.UI.UIMarkLabel uiMarkLabel1;
        private Sunny.UI.UIMarkLabel uiMarkLabel2;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UIMarkLabel uiMarkLabel3;
        private Sunny.UI.UITextBox uiTextBox1;
        private Panel panel1;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
    }
}