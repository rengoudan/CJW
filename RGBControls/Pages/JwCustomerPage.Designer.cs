using System.Drawing;
using System.Windows.Forms;

namespace RGBJWMain.Pages
{
    partial class JwCustomerPage
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
        /// 2026年11月27日 画面の部品は JwCustomerPage.cs（BuildPage）で組み立てる。ここは右クリックメニューだけ。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            menuEdit = new ToolStripMenuItem();
            menuSetting = new ToolStripMenuItem();
            menuDelete = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            //
            // contextMenuStrip1
            //
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { menuEdit, menuSetting, menuDelete });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(161, 70);
            //
            // menuEdit
            //
            menuEdit.Name = "menuEdit";
            menuEdit.Text = "基本情報を編集";
            menuEdit.Click += menuEdit_Click;
            //
            // menuSetting
            //
            menuSetting.Name = "menuSetting";
            menuSetting.Text = "解析設定";
            menuSetting.Click += menuSetting_Click;
            //
            // menuDelete
            //
            menuDelete.Name = "menuDelete";
            menuDelete.Text = "削除";
            menuDelete.Click += menuDelete_Click;
            //
            // JwCustomerPage
            //
            AutoScaleMode = AutoScaleMode.None;
            Name = "JwCustomerPage";
            Text = "顧客管理";
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem menuEdit;
        private ToolStripMenuItem menuSetting;
        private ToolStripMenuItem menuDelete;
    }
}
