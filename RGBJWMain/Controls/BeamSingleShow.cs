using JwCore;
using JwShapeCommon;
using Microsoft.VisualBasic;
using Sunny.UI;
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
using static System.Windows.Forms.LinkLabel;

namespace RGBJWMain.Controls
{
    public partial class BeamSingleShow : Control
    {
        public BeamSingleShow()
        {
            
            this.BackColor = Color.Black;
            
            InitializeComponent();
        }

        private JwSingleBeamDraw _jwDrawShape;
        public JwSingleBeamDraw JwDrawShape
        {
            get 
            { 
                return _jwDrawShape; 
            }
            set 
            { 
                _jwDrawShape = value;
                _jwDrawShape.Draw(Width, Height, 20,20);
                //_jwDrawShape.Draw();
                if(_jwDrawShape.controls != null)
                {
                    _bounds = _jwDrawShape.controls;
                    _lines = _jwDrawShape.Lines;
                    Invalidate();
                }
            }
        }

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

        private void drawBeams(PaintEventArgs pe)
        {
            initdraw();
            //if(_singleBeam!=null)
            //{
            //    pe.Graphics.DrawRectangle(pens, _singleBeam.BeamRectangle);
            //    foreach (var el in _singleBeam.Ellipselst)
            //    {

            //        //pe.Graphics.DrawRectangle(pens, (int)beam.TopLeft.X, (int)beam.TopLeft.Y, (int)beam.Width, (int)beam.Height);
            //        pe.Graphics.DrawEllipse(pens, el);
            //    }
            //}
            if (_bounds!=null)
            {
                foreach(var b in _bounds)
                {
                    if(b.ShapeType==DrawShapeType.Beam)
                    {
                        pe.Graphics.DrawRectangle(pens, b.DrawRectangleF.Location.X, b.DrawRectangleF.Location.Y, b.DrawRectangleF.Width, b.DrawRectangleF.Height);
                    }
                    if(b.ShapeType== JwCore.DrawShapeType.Hole)
                    {
                        pe.Graphics.DrawEllipse(penyl, b.DrawRectangleF.Location.X, b.DrawRectangleF.Location.Y, b.DrawRectangleF.Width, b.DrawRectangleF.Height);
                    }
                    
                }
            }

            if (_lines != null)
            {
                biaozhuFont = new Font(Control.DefaultFont.SystemFontName, fontSize, FontStyle.Regular);
                foreach (var line in _lines)
                {
                    pe.Graphics.DrawLine(penjt,line.DrawStart.X, line.DrawStart.Y, line.DrawEnd.X, line.DrawEnd.Y);//箭头 s 点和 pr点
                    StringFormat sf = new StringFormat();
                    sf.FormatFlags = StringFormatFlags.DirectionVertical;
                    var swz = new PointF((line.DrawStart.X+ line.DrawEnd.X)/2, line.DrawStart.Y - 10);
                    pe.Graphics.DrawString(line.Title, biaozhuFont, bushred, swz);
                }
            }
            if (JwDrawShape != null)
            {
                if (JwDrawShape.FuzhuXs.Count > 0)
                {
                    foreach(var x in JwDrawShape.FuzhuXs)
                    {
                        pe.Graphics.DrawLine(penx, x, 0, x, Height);//箭头 s 点和 pr点
                    }
                }
                if(JwDrawShape.FuzhuYs.Count > 0)
                {
                    foreach(var y in JwDrawShape.FuzhuYs)
                    {
                        pe.Graphics.DrawLine(penx, 0, y, Width, y);//箭头 s 点和 pr点
                    }
                }
            }
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            //if (_candraw)
            //{
            //    drawBeams(pe);
            //}
            drawBeams(pe);
        }
    }
}
