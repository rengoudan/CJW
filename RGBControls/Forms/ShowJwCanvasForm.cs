using JwCore;
using JwData;
using JwServices;
using JwShapeCommon;
using RGBControls.Classes;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Forms
{
    public partial class ShowJwCanvasForm : UIForm2
    {
        private JwProjectMainService JwProjectMainService => ServiceFactory.GetInstance().CreateJwProjectMainService();
        public ShowJwCanvasForm()
        {
            InitializeComponent();
            GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent += DeleteSelectedSquare;
            GlobalEvent.GetGlobalEvent().WarningEvent += GlobalEvent_WarningEvent;
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
                var z =await JwProjectMainService.DeleteSquare(e.Id,e.SubId, e.DrawShapeType);
                if(z)
                {
                    this.SuccessModal("指定されたコンテンツは削除されました!");
                    if(GlobalEvent.GetGlobalEvent().RefreshDataEvent!=null)
                    {
                        GlobalEvent.GetGlobalEvent().RefreshDataEvent(this, EventArgs.Empty);
                    }
                }  
            }
        }

        public JwCanvas jwCanvas { get; set; }

        private void ShowJwCanvasForm_Load(object sender, EventArgs e)
        {
            
            if (jwCanvas != null)
            {
                JwCanvasDraw canvasDraw = new JwCanvasDraw(jwCanvas);
                jwCanvasControl1.IsNewCanvas = false;
                jwCanvasControl1.CanvasDraw = canvasDraw;
                //jwCanvasControl1.Click += JwCanvasControl1_Click;
                jwCanvasControl1.SelectBeamEvent += JwCanvas_Click;
                this.jwProjectSubDataBindingSource.DataSource = jwCanvas.JwProjectSubData;
            }


        }
        private void JwCanvas_Click(object? sender, EventArgs e)
        {
            if (jwCanvas != null)
            {
                if (jwCanvasControl1.BeamSelected)
                {
                    var z = jwCanvasControl1.SelectedBeam;

                    if (z != null)
                    {
                        if (z.DirectionType == BeamDirectionType.Horizontal)
                        {
                            z.AbsolutePD = z.TopLeft.X;
                        }
                        if (z.DirectionType == BeamDirectionType.Vertical)
                        {
                            z.AbsolutePD = z.BottomLeft.Y;
                        }
                        var q = z.jwBeamMarks;
                        //JwSingleBeamForm jsForm = new JwSingleBeamForm(z);

                        ////jsForm.ShowBeam = js;
                        ////jsForm.sha
                        //jsForm.ShowDialog();

                        NewJwBeamForm jsForm = new NewJwBeamForm(z);
                        jsForm.Show();

                    }
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                var beam = dataGridView1.Rows[e.RowIndex].DataBoundItem as JwBeamData;
                if (beam != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = beam.Id,
                            DrawShapeType = JwCore.DrawShapeType.Beam
                        });
                    }
                }
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                var pillar = dataGridView2.Rows[e.RowIndex].DataBoundItem as JwPillarData;
                if (pillar != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = pillar.Id,
                            DrawShapeType = JwCore.DrawShapeType.Pillar
                        });
                    }
                }
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (jwCanvas != null)
            {
                saveFileDialog1.Filter = "*.png|png file";
                saveFileDialog1.DefaultExt = ".png";
                saveFileDialog1.FileName = jwCanvas.JwProjectSubData.FloorName + ".png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    jwCanvasControl1.jwToPng(saveFileDialog1.FileName);
                }
            }
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                var beam = dataGridView3.Rows[e.RowIndex].DataBoundItem as JwLinkPartData;
                if (beam != null)
                {
                    if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                    {
                        GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this, new ControlSelectedSquareArgs
                        {
                            Id = beam.Id,
                            DrawShapeType = JwCore.DrawShapeType.LinkPart
                        });
                    }
                }
            }
        }

        private void ShowJwCanvasForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent-= DeleteSelectedSquare;
            GlobalEvent.GetGlobalEvent().WarningEvent -= GlobalEvent_WarningEvent;
        }
    }
}
