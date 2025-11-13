namespace RGBControls.Forms
{
    partial class CsvSettings
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            label3 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            label1 = new AntdUI.Label();
            label4 = new AntdUI.Label();
            divider1 = new AntdUI.Divider();
            input1 = new AntdUI.Input();
            input2 = new AntdUI.Input();
            input3 = new AntdUI.Input();
            input4 = new AntdUI.Input();
            SuspendLayout();
            // 
            // label3
            // 
            label3.Location = new Point(0, 177);
            label3.Name = "label3";
            label3.Size = new Size(167, 35);
            label3.TabIndex = 2;
            label3.Text = "縦穴数";
            // 
            // label2
            // 
            label2.Location = new Point(0, 115);
            label2.Name = "label2";
            label2.Size = new Size(167, 33);
            label2.TabIndex = 1;
            label2.Text = "横ピッチ";
            // 
            // label1
            // 
            label1.Location = new Point(3, 54);
            label1.Name = "label1";
            label1.Size = new Size(167, 35);
            label1.TabIndex = 0;
            label1.Text = "横穴数";
            // 
            // label4
            // 
            label4.Location = new Point(0, 244);
            label4.Name = "label4";
            label4.Size = new Size(167, 35);
            label4.TabIndex = 3;
            label4.Text = "縦穴数";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Size = new Size(424, 34);
            divider1.TabIndex = 4;
            divider1.Text = "CSVSETTINGS";
            // 
            // input1
            // 
            input1.Location = new Point(173, 40);
            input1.Name = "input1";
            input1.Size = new Size(241, 49);
            input1.TabIndex = 5;
            input1.Text = "1";
            // 
            // input2
            // 
            input2.Location = new Point(173, 99);
            input2.Name = "input2";
            input2.Size = new Size(241, 49);
            input2.TabIndex = 6;
            input2.Text = "0.0";
            // 
            // input3
            // 
            input3.Location = new Point(173, 163);
            input3.Name = "input3";
            input3.Size = new Size(241, 49);
            input3.TabIndex = 7;
            input3.Text = "1";
            // 
            // input4
            // 
            input4.Location = new Point(173, 230);
            input4.Name = "input4";
            input4.Size = new Size(241, 49);
            input4.TabIndex = 8;
            input4.Text = "0.0";
            // 
            // CsvSettings
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(input4);
            Controls.Add(input3);
            Controls.Add(input2);
            Controls.Add(input1);
            Controls.Add(divider1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label4);
            Controls.Add(label1);
            Name = "CsvSettings";
            Size = new Size(424, 542);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Label label3;
        private AntdUI.Label label2;
        private AntdUI.Label label1;
        private AntdUI.Label label4;
        private AntdUI.Divider divider1;
        private AntdUI.Input input1;
        private AntdUI.Input input2;
        private AntdUI.Input input3;
        private AntdUI.Input input4;
    }
}
