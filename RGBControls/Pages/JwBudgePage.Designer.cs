using System.Drawing;
using System.Windows.Forms;

namespace RGBJWMain.Pages
{
    partial class JwBudgePage
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
        /// 2026年11月27日 画面の部品は JwBudgePage.cs（BuildPage）で組み立てる。ここは右クリックメニューだけ。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            menuExcel = new ToolStripMenuItem();
            menuAddBudget = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            //
            // contextMenuStrip1
            //
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { menuExcel, menuAddBudget });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(161, 48);
            //
            // menuExcel
            //
            menuExcel.Name = "menuExcel";
            menuExcel.Text = "EXCEL出力";
            menuExcel.Click += menuExcel_Click;
            //
            // menuAddBudget
            //
            menuAddBudget.Name = "menuAddBudget";
            menuAddBudget.Text = "新しい予算を追加";
            menuAddBudget.Click += menuAddBudget_Click;
            //
            // JwBudgePage
            //
            AutoScaleMode = AutoScaleMode.None;
            Name = "JwBudgePage";
            Text = "予算管理";
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem menuExcel;
        private ToolStripMenuItem menuAddBudget;
    }
}
