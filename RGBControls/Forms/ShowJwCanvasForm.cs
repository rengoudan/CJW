using JwCore;
using JwData;
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
        public JwDataContext? dbContext;
        public ShowJwCanvasForm()
        {
            InitializeComponent();
            GlobalEvent.GetGlobalEvent().DeleteSelectedSquareEvent += DeleteSelectedSquare;
        }

        private void DeleteSelectedSquare(object? sender, ControlSelectedSquareArgs e)
        {
            //throw new NotImplementedException();
            if (!string.IsNullOrEmpty(e.Id))
            {
                if (e.DrawShapeType == JwCore.DrawShapeType.Beam)
                {
                    var z = this.dbContext?.JwBeamDatas.Find(e.Id);
                    this.dbContext?.JwBeamDatas.Remove(z);
                }
                if(e.DrawShapeType== JwCore.DrawShapeType.Pillar)
                {
                    var p = this.dbContext?.JwPillarDatas.Find(e.Id);
                    this.dbContext?.JwPillarDatas.Remove(p);
                }
                if(e.DrawShapeType== JwCore.DrawShapeType.LinkPart)
                {
                    var lp = this.dbContext?.JwLinkPartDatas.Find(e.Id);
                    this.dbContext?.JwLinkPartDatas.Remove(lp);
                }
                this.SuccessModal("指定されたコンテンツは削除されました!");
                this.dbContext?.SaveChanges();
            }


        }

        public JwCanvas jwCanvas { get; set; }

        private void ShowJwCanvasForm_Load(object sender, EventArgs e)
        {
            dbContext = ContextFactory.GetContext();
            this.dbContext?.Database.EnsureCreated();

            this.dbContext?.JwBeamDatas.Load();
            this.dbContext?.JwPillarDatas.Load();
            if (jwCanvas != null)
            {
                JwCanvasDraw canvasDraw = new JwCanvasDraw(jwCanvas);
                jwCanvasControl1.CanvasDraw = canvasDraw;
                //jwCanvasControl1.Click += JwCanvasControl1_Click;
                jwCanvasControl1.SelectBeamEvent += JwCanvas_Click;
                this.jwProjectSubDataBindingSource.DataSource = jwCanvas.JwProjectSubData;
                //if(jwCanvas.JwProjectSubData.JwBeamDatas != null )
                //{
                //    this.jwBeamDatasBindingSource.DataSource = jwCanvas.JwProjectSubData.JwBeamDatas;
                //}

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
        }
    }
}
