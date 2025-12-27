using JwCore;
using JwShapeCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBControls.Controls
{
    public partial class Sub : UserControl
    {

        private JwProjectSubData _subdata;

        public JwProjectSubData SubData
        {
            get { return _subdata; }
            set
            {
                _subdata = value;
                if (_subdata != null)
                {
                    this.pageHeader1.Text = _subdata.FloorName;
                    this.pageHeader1.Description= "解析されたデータからさまざまな処理図または関連する CSV ファイルをエクスポートします。";
                    this.pageHeader1.Refresh();
                }
            }
        }   

        public Sub()
        {
            InitializeComponent();
        }

        public Sub(JwProjectSubData subdata)
        {
            InitializeComponent();
            SubData= subdata;
            //this.Resize += Sub_Resize;
        }

        private void Sub_Resize(object? sender, EventArgs e)
        {
            this.Refresh();
            this.jwCanvasControl1.Invalidate();
        }

        private void Sub_Load(object sender, EventArgs e)
        {
            if (_subdata != null)
            {
                JwCanvas jwc = _subdata.DataToCanvas();
                JwCanvasDraw canvasDraw = new JwCanvasDraw(jwc);
                this.jwCanvasControl1.CanvasDraw = canvasDraw;

                this.liangtable.DataSource = _subdata.JwBeamDatas;

                this.zhutable.DataSource = _subdata.JwPillarDatas;

                this.lianjietable.DataSource = _subdata.JwLianjieDatas;

                //this.table1.DataSource = _subdata.JwLinkPartDatas;
            }
        }
    }
}
