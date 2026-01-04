
namespace RGBControls.Controls
{
    partial class Sub
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
            pageHeader1 = new AntdUI.PageHeader();
            panel1 = new Panel();
            panel3 = new Panel();
            panel5 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel9 = new Panel();
            panel4 = new Panel();
            bbgtable = new AntdUI.Table();
            divider4 = new AntdUI.Divider();
            leftpane = new Panel();
            panel8 = new Panel();
            lianjietable = new AntdUI.Table();
            divider3 = new AntdUI.Divider();
            button6 = new AntdUI.Button();
            button5 = new AntdUI.Button();
            panel7 = new Panel();
            zhutable = new AntdUI.Table();
            divider2 = new AntdUI.Divider();
            panel6 = new Panel();
            liangtable = new AntdUI.Table();
            divider1 = new AntdUI.Divider();
            jwCanvasControl1 = new RGBJWMain.Controls.JwCanvasControl();
            panel2 = new Panel();
            button4 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            button7 = new AntdUI.Button();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel9.SuspendLayout();
            panel4.SuspendLayout();
            leftpane.SuspendLayout();
            panel8.SuspendLayout();
            divider3.SuspendLayout();
            panel7.SuspendLayout();
            panel6.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // pageHeader1
            // 
            pageHeader1.Description = "floorcaozuo";
            pageHeader1.DividerShow = true;
            pageHeader1.Dock = DockStyle.Top;
            pageHeader1.Location = new Point(0, 0);
            pageHeader1.Name = "pageHeader1";
            pageHeader1.Size = new Size(1359, 80);
            pageHeader1.SubGap = 4;
            pageHeader1.TabIndex = 8;
            pageHeader1.TabStop = false;
            pageHeader1.Text = "subname";
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 80);
            panel1.Name = "panel1";
            panel1.Size = new Size(1359, 1294);
            panel1.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(panel5);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 55);
            panel3.Name = "panel3";
            panel3.Size = new Size(1359, 1239);
            panel3.TabIndex = 2;
            // 
            // panel5
            // 
            panel5.BorderStyle = BorderStyle.FixedSingle;
            panel5.Controls.Add(tableLayoutPanel1);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(1359, 1239);
            panel5.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(panel9, 0, 0);
            tableLayoutPanel1.Controls.Add(leftpane, 2, 0);
            tableLayoutPanel1.Controls.Add(jwCanvasControl1, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1357, 1237);
            tableLayoutPanel1.TabIndex = 20;
            // 
            // panel9
            // 
            panel9.BorderStyle = BorderStyle.FixedSingle;
            panel9.Controls.Add(panel4);
            panel9.Controls.Add(divider4);
            panel9.Dock = DockStyle.Fill;
            panel9.Location = new Point(3, 3);
            panel9.Name = "panel9";
            panel9.Size = new Size(265, 1231);
            panel9.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(bbgtable);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 34);
            panel4.Name = "panel4";
            panel4.Size = new Size(263, 300);
            panel4.TabIndex = 1;
            // 
            // bbgtable
            // 
            bbgtable.Dock = DockStyle.Fill;
            bbgtable.Gap = 12;
            bbgtable.Location = new Point(0, 0);
            bbgtable.Name = "bbgtable";
            bbgtable.Size = new Size(263, 300);
            bbgtable.TabIndex = 0;
            bbgtable.Text = "table1";
            bbgtable.CellDoubleClick += bbgtable_CellDoubleClick;
            // 
            // divider4
            // 
            divider4.Dock = DockStyle.Top;
            divider4.Location = new Point(0, 0);
            divider4.Name = "divider4";
            divider4.Orientation = AntdUI.TOrientation.Left;
            divider4.Size = new Size(263, 34);
            divider4.TabIndex = 0;
            divider4.Text = "B/BG";
            // 
            // leftpane
            // 
            leftpane.BorderStyle = BorderStyle.FixedSingle;
            leftpane.Controls.Add(panel8);
            leftpane.Controls.Add(divider3);
            leftpane.Controls.Add(panel7);
            leftpane.Controls.Add(divider2);
            leftpane.Controls.Add(panel6);
            leftpane.Controls.Add(divider1);
            leftpane.Dock = DockStyle.Fill;
            leftpane.Location = new Point(1088, 3);
            leftpane.Name = "leftpane";
            leftpane.Size = new Size(266, 1231);
            leftpane.TabIndex = 3;
            leftpane.TabStop = true;
            // 
            // panel8
            // 
            panel8.Controls.Add(lianjietable);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(0, 708);
            panel8.Name = "panel8";
            panel8.Size = new Size(264, 300);
            panel8.TabIndex = 15;
            // 
            // lianjietable
            // 
            lianjietable.Dock = DockStyle.Fill;
            lianjietable.Gap = 12;
            lianjietable.Location = new Point(0, 0);
            lianjietable.Name = "lianjietable";
            lianjietable.Size = new Size(264, 300);
            lianjietable.TabIndex = 14;
            lianjietable.Text = "table1";
            lianjietable.CellDoubleClick += lianjietable_CellDoubleClick;
            // 
            // divider3
            // 
            divider3.BadgeAlign = AntdUI.TAlign.Left;
            divider3.BadgeMode = true;
            divider3.BadgeSize = 0.8F;
            divider3.BadgeSvg = "ArrowsAltOutlined";
            divider3.Controls.Add(button6);
            divider3.Controls.Add(button5);
            divider3.Dock = DockStyle.Top;
            divider3.Location = new Point(0, 668);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(264, 40);
            divider3.TabIndex = 13;
            divider3.Text = "接続部品";
            // 
            // button6
            // 
            button6.Dock = DockStyle.Right;
            button6.Ghost = true;
            button6.IconSvg = "FileExcelOutlined";
            button6.Location = new Point(179, 0);
            button6.Name = "button6";
            button6.Size = new Size(39, 40);
            button6.TabIndex = 1;
            button6.Type = AntdUI.TTypeMini.Error;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.Dock = DockStyle.Right;
            button5.Ghost = true;
            button5.IconSvg = "PlusOutlined";
            button5.Location = new Point(218, 0);
            button5.Name = "button5";
            button5.Shape = AntdUI.TShape.Round;
            button5.Size = new Size(46, 40);
            button5.TabIndex = 0;
            button5.Type = AntdUI.TTypeMini.Success;
            button5.Click += button5_Click;
            // 
            // panel7
            // 
            panel7.Controls.Add(zhutable);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(0, 368);
            panel7.Name = "panel7";
            panel7.Size = new Size(264, 300);
            panel7.TabIndex = 16;
            // 
            // zhutable
            // 
            zhutable.Dock = DockStyle.Fill;
            zhutable.Gap = 12;
            zhutable.Location = new Point(0, 0);
            zhutable.Name = "zhutable";
            zhutable.Size = new Size(264, 300);
            zhutable.TabIndex = 12;
            zhutable.Text = "table1";
            zhutable.CellDoubleClick += zhutable_CellDoubleClick;
            // 
            // divider2
            // 
            divider2.Badge = "";
            divider2.BadgeAlign = AntdUI.TAlign.Left;
            divider2.BadgeSize = 0.8F;
            divider2.BadgeSvg = "BorderOuterOutlined";
            divider2.Dock = DockStyle.Top;
            divider2.Location = new Point(0, 334);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(264, 34);
            divider2.TabIndex = 11;
            divider2.Text = "柱";
            // 
            // panel6
            // 
            panel6.Controls.Add(liangtable);
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(0, 34);
            panel6.Name = "panel6";
            panel6.Size = new Size(264, 300);
            panel6.TabIndex = 17;
            // 
            // liangtable
            // 
            liangtable.Dock = DockStyle.Fill;
            liangtable.Gap = 12;
            liangtable.Location = new Point(0, 0);
            liangtable.Name = "liangtable";
            liangtable.Size = new Size(264, 300);
            liangtable.TabIndex = 10;
            liangtable.Text = "table1";
            liangtable.CellDoubleClick += liangtable_CellDoubleClick;
            // 
            // divider1
            // 
            divider1.Badge = "";
            divider1.BadgeAlign = AntdUI.TAlign.Left;
            divider1.BadgeMode = true;
            divider1.BadgeSize = 0.8F;
            divider1.BadgeSvg = "PicCenterOutlined";
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(264, 34);
            divider1.TabIndex = 9;
            divider1.Text = "梁";
            // 
            // jwCanvasControl1
            // 
            jwCanvasControl1.BeamSelected = false;
            jwCanvasControl1.CanvasDraw = null;
            jwCanvasControl1.Dock = DockStyle.Fill;
            jwCanvasControl1.Location = new Point(276, 4);
            jwCanvasControl1.Margin = new Padding(5, 4, 5, 4);
            jwCanvasControl1.Name = "jwCanvasControl1";
            jwCanvasControl1.SelectBeamEvent = null;
            jwCanvasControl1.SelectedBeam = null;
            jwCanvasControl1.Size = new Size(804, 1229);
            jwCanvasControl1.TabIndex = 4;
            // 
            // panel2
            // 
            panel2.Controls.Add(button7);
            panel2.Controls.Add(button4);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1359, 55);
            panel2.TabIndex = 7;
            // 
            // button4
            // 
            button4.IconSvg = "ForkOutlined";
            button4.Location = new Point(447, 6);
            button4.Name = "button4";
            button4.Size = new Size(182, 46);
            button4.TabIndex = 6;
            button4.Text = "ブレース施工図";
            button4.Type = AntdUI.TTypeMini.Primary;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.IconSvg = "InsertRowBelowOutlined";
            button3.Location = new Point(299, 6);
            button3.Name = "button3";
            button3.Size = new Size(142, 46);
            button3.TabIndex = 5;
            button3.Text = "番付図下JW";
            button3.Type = AntdUI.TTypeMini.Primary;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.IconSvg = "InsertRowAboveOutlined";
            button2.Location = new Point(151, 6);
            button2.Name = "button2";
            button2.Size = new Size(142, 46);
            button2.TabIndex = 4;
            button2.Text = "番付図上JW";
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.IconSvg = "DownloadOutlined";
            button1.Location = new Point(3, 6);
            button1.Name = "button1";
            button1.Size = new Size(142, 46);
            button1.TabIndex = 3;
            button1.Text = "輸出梁JW";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // button7
            // 
            button7.IconSvg = "FileExcelOutlined";
            button7.Location = new Point(635, 6);
            button7.Name = "button7";
            button7.Size = new Size(198, 43);
            button7.TabIndex = 7;
            button7.Text = "輸出接続";
            button7.Type = AntdUI.TTypeMini.Error;
            button7.Click += button7_Click;
            // 
            // Sub
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(pageHeader1);
            Name = "Sub";
            Size = new Size(1359, 1374);
            Load += Sub_Load;
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel5.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel4.ResumeLayout(false);
            leftpane.ResumeLayout(false);
            panel8.ResumeLayout(false);
            divider3.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }



        #endregion

        private AntdUI.PageHeader pageHeader1;
        private Panel panel1;
        private Panel panel2;
        private AntdUI.Button button1;
        private AntdUI.Button button2;
        private AntdUI.Button button3;
        private AntdUI.Button button4;
        private Panel panel3;
        private Panel leftpane;
        private Panel panel5;
        private AntdUI.Divider divider1;
        private Panel panel6;
        private AntdUI.Table liangtable;
        private AntdUI.Divider divider2;
        private Panel panel7;
        private AntdUI.Table zhutable;
        private AntdUI.Divider divider3;
        private Panel panel8;
        private AntdUI.Table lianjietable;
        private Panel panel9;
        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Divider divider4;
        private Panel panel4;
        private AntdUI.Table bbgtable;
        private RGBJWMain.Controls.JwCanvasControl jwCanvasControl1;
        private AntdUI.Button button5;
        private AntdUI.Button button6;
        private AntdUI.Button button7;
    }
}
