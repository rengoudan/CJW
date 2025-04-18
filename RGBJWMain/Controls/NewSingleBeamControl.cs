﻿using JwShapeCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBJWMain.Controls
{
    public partial class NewSingleBeamControl : Control
    {
        public NewSingleBeamControl()
        {
            this.BackColor = Color.Black;
            InitializeComponent();
        }

        private JwBeam _showBeam;
        public JwBeam ShowBeam
        {
            get { return _showBeam; }
            set
            {
                _showBeam = value;
                if (_showBeam != null) 
                {
                    _jwDrawShape = new NewJwBeamJwDraw(_showBeam);

                    if (_jwDrawShape != null)
                    {
                        Invalidate();
                    }
                }
                
                //_jwDrawShape.
            }
        }


        private NewJwBeamJwDraw _jwDrawShape;
       
        private List<ControlDraw> _bounds;

        private List<ControlLine> _lines;

        private bool _candraw = false;

        Pen pens = new Pen(new SolidBrush(Color.White), 1);//线条的粗细

        Pen penyl = new Pen(new SolidBrush(Color.Yellow), 1);//线条的粗细

        Pen pensr = new Pen(new SolidBrush(Color.Red), 1);//线条的粗细

        Pen penx = new Pen(new SolidBrush(Color.Gray), 0.5f);//线条的粗细

        Pen penjt = new Pen(new SolidBrush(Color.Green), 0.5f);//线条的粗细

        int fontSize = 8;
        Font biaozhuFont;
        Font subdataFont;
        Font labelFont;
        Brush bushred = new SolidBrush(Color.Red);
        Brush bushwhite = new SolidBrush(Color.White);
        private void initdraw()
        {
            biaozhuFont = new Font(Control.DefaultFont.SystemFontName, fontSize, FontStyle.Regular);
            subdataFont = new Font(Control.DefaultFont.SystemFontName, 10, FontStyle.Regular);
            labelFont = new Font(Control.DefaultFont.SystemFontName, 13, FontStyle.Bold | FontStyle.Underline);
            pens.DashStyle = DashStyle.Solid;//线条的线型
            penx.DashStyle = DashStyle.Dot;//线条的线型
            penjt.DashStyle = DashStyle.Dot;//线条的线型
            penjt.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            penjt.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
        }

        private void _drawall(PaintEventArgs pe)
        {
            int w = this.Width;
            int h=this.Height;
            if (this._jwDrawShape != null)
            {
                if (this._jwDrawShape.CanDraw) 
                {
                    this._jwDrawShape.CreateControlDraw(w,h);
                    if(this._jwDrawShape.ControlDraws?.Count> 0)
                    {
                        var yw=this._jwDrawShape.Maxx-_jwDrawShape.Minx;

                        var yh=this._jwDrawShape.Maxy-_jwDrawShape.Miny;
                        var wscale = Convert.ToSingle((this.Width - 20) / yw);
                        var hscale = Convert.ToSingle((this.Height - 20) / yh);
                        var scale = Math.Min(wscale, hscale);
                        Matrix myMatrix = new Matrix(1, 0, 0, -1, 0, 0);
                        myMatrix.Scale(scale, scale);
                        //float flx = Convert.ToSingle(_jwDrawShape.Minx) * scale - 20;
                        //float fly = Convert.ToSingle(_jwDrawShape.Maxy) * scale + 50;
                        float flx = Convert.ToSingle(_jwDrawShape.Minx) * scale ;
                        float fly = Convert.ToSingle(_jwDrawShape.Maxy) * scale ;
                        var z = pe.Graphics;
                        using (z)
                        {
                            z.Transform = myMatrix;
                            z.TranslateTransform(-flx, fly, MatrixOrder.Append);
                            foreach (var cd in _jwDrawShape.ControlDraws) 
                            {
                                
                                Pen pnn = new Pen(new SolidBrush(Color.White), 1/scale);//线条的粗细
                                if (cd.ShapeType == JwCore.DrawShapeType.Beam)
                                {
                                    pe.Graphics.DrawRectangle(pnn, cd.DrawRectangleF.Location.X, cd.DrawRectangleF.Location.Y, cd.DrawRectangleF.Width, cd.DrawRectangleF.Height);
                                }
                                if (cd.ShapeType == JwCore.DrawShapeType.Hole){
                                    pe.Graphics.DrawEllipse(pnn, cd.DrawRectangleF.Location.X, cd.DrawRectangleF.Location.Y, cd.DrawRectangleF.Width, cd.DrawRectangleF.Height);
                                }
                                
                             }
                        }
                            
                    }
                }
            }

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            this._drawall(pe);
        }
    }
}
