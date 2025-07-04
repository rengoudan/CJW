using JwCore;
using JwData;
using JwShapeCommon;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Pages
{
    public partial class SubDetail : UIPage
    {
        public JwDataContext? dbContext;
        public SubDetail()
        {
            InitializeComponent();
        }

        private JwProjectSubData _subData;

        public SubDetail(JwProjectSubData subData)
        {
            InitializeComponent();
            _subData = subData;
            uiLine1.Text = _subData.FloorName;
            dbContext = ContextFactory.GetContext();
        }

        private void SubDetail_Load(object sender, EventArgs e)
        {
            if(_subData != null)
            {
                this.Name = _subData.FloorName;
                this.Text = _subData.FloorName;
                //jwCanvasControl1. = _subData;

                JwCanvas canvas = _subData.DataToCanvas();
                ObservableCollectionListSource<JwProjectSubData> subDatas = new ObservableCollectionListSource<JwProjectSubData> { _subData };

                JwCanvasDraw canvasDraw = new JwCanvasDraw(canvas);
                jwCanvasControl1.CanvasDraw = canvasDraw;

                //this.dbContext?.Database.EnsureCreated();

                //this.dbContext?.JwProjectMainDatas.Load();

                //this.jwLianjieDatasBindingSource.DataSource = dbContext?.JwProjectSubDatas.Local.ToBindingList(); ;
                this.bindingSource1.DataSource = subDatas;

            }
        }
    }
}
