using AntdUI;
using JwCore;
using JwServices;
using JwShapeCommon;
using RGBControls.Classes;
using RGBControls.Controls;
using RGBJWMain.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBControls.Forms
{
    public partial class ShowSubForm : AntdUI.Window
    {
        private JwProjectSubData _subdata;

        private Sub _sub;

        private JwProjectMainService JwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();

        public JwProjectSubData Subdata
        {
            get { return _subdata; }
            set
            {
                _subdata = value;
                this.pageHeader1.Text = _subdata.FloorName;
                //this.pageHeader1.Refresh();
            }
        }


        public ShowSubForm(JwProjectSubData subdata,Sub sub)
        {
            InitializeComponent();
            Subdata = subdata;
            _sub = sub;
            GlobalEvent.GetGlobalEvent().WarningEvent += GlobalEvent_WarningEvent;
            GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent += DeleteSelectedSquare;
        }

        private void ShowSubForm_Load(object sender, EventArgs e)
        {
            if (_subdata != null)
            {
                this.Controls.Add(_sub);
                _sub.Dock= DockStyle.Fill;
                this.PerformLayout();
                this.BringToFront();
                this.Focus();
                this.PerformLayout(); this.Invalidate(); this.Update();
            }
        }


        /// <summary>
        /// 全局提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlobalEvent_WarningEvent(object? sender, WarningArgs e)
        {
            AntdUI.Message.warn(this, e.WarningMsg, Font);
        }

        /// <summary>
        /// 针对于数据的删除，需要同步更新主表里的合计字段的算法
        /// 增加dataoperate类，专门处理数据的增删改查和联动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteSelectedSquare(object? sender, ControlSelectedSquareArgs e)
        {
            //throw new NotImplementedException();
            if (!string.IsNullOrEmpty(e.Id))
            {
                var z = await JwProjectMainService.DeleteSquare(e.Id, e.SubId, e.DrawShapeType);
                if (z)
                {
                    this.SuccessModal("指定されたコンテンツは削除されました!");
                    if (GlobalEvent.GetGlobalEvent().RefreshDataEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().RefreshDataEvent(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
