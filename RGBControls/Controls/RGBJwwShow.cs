using JwCore;
using JwShapeCommon;
using JwwHelper;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using NPOI.SS.Formula.Functions;
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
using static System.Net.Mime.MediaTypeNames;

namespace RGBJWMain.Controls
{
    public partial class RGBJwwShow : Control
    {
        public RGBJwwShow()
        {
            this.BackColor = Color.Black;

            InitializeComponent();
            
            
            //if(sscale)

            this.MouseWheel += RGBJwwShow_MouseClick;
            isdraw = false;
        }

        private float sscale=1.0f;
        private float jscale = 1.0f;

        private PointF origin = new PointF(0, 0);

        private void RGBJwwShow_MouseClick(object? sender, MouseEventArgs e)
        {
            // 可以根据需要实现缩放或其他功能，这里暂时留空
            // 例如：Invalidate();
            float oldScale = sscale;
            sscale += e.Delta > 0 ? 0.1f : -0.1f;
            sscale = Math.Max(jscale, sscale); // 防止缩得太小
                                           // 以鼠标位置为中心进行缩放
            origin.X = e.X - (e.X - origin.X) * (sscale / oldScale);
            origin.Y = e.Y - (e.Y - origin.Y) * (sscale / oldScale);
            //Invalidate();
        }

        double _minx;
        public double Minx
        {
            get { return _minx; }
            set { _minx = value; }
        }

        double _maxx;
        public double Maxx
        {
            get { return  _maxx; }
            set
            {
                _maxx = value;
            }
        }
        double _miny;
        public double Miny
        {
            get => _miny;
            set
            {
                _miny = value;
            }
        }
        double _maxy;
        public double Maxy
        {
            get => _maxy;
            set { 
                _maxy = value;
            }
        }
        List<JwwSen> _sens;
        public List<JwwSen> Sens
        {
            get => _sens;
            set { 
                _sens = value;
            }
        }
        List<JwwSolid> _solid;
        public List<JwwSolid> Solid
        {
            get=> _solid;
            set => _solid = value;
        }

        bool isdraw = false;
        public bool IsDraw
        {
            get { return isdraw; }
            set  { isdraw = value;
                Invalidate();
            }
        }
        List<int> _colors;
        public List<int> Colors
        {
            get=> _colors;
            set => _colors = value;
        }
        Dictionary<int,System.Drawing.Color> _colors2 = new Dictionary<int,Color>();
        List<JwBlock> _blocks;
        public List<JwBlock> Blocks
        {
            get
            {
                return _blocks;
            }
            set
            {
                _blocks = value;
            }
        }

        private bool _showSen;
        public bool ShowSen
        {
            get
            {
                return _showSen;
            }
            set
            {
                _showSen = value;
                Invalidate();
            }
        }


        private bool _showShape;
        public bool ShowShape
        {
            get
            {
                return _showShape;
            }
            set
            {
                _showShape = value;
                Invalidate();
            }
        }


        public RGBJwwShow(double minx,double maxx,double miny,double maxy,List<JwwSen> sens,List<JwwSolid> solids,List<int> colors,List<JwBlock> blocks) 
        {
            this.BackColor = System.Drawing.Color.Black;
            InitializeComponent();
            _minx = minx;
            _maxx = maxx;
            _miny = miny;
            _maxy = maxy;
            _sens = sens;
            _solid = solids;

            isdraw = true;
            _blocks = blocks;
            _colors = colors;
            createcolors();
        }


        public void init()
        {
            _showSen = true;
            _showShape = true;
        }

        public void createcolors()
        {
            if (_colors != null)
            {
                if(_colors.Count> 0)
                {
                    if (_colors.Count < JwFileConsts.JwFileColors.Length)
                    {
                        for (int i = 0; i < _colors.Count; i++)
                        {
                            _colors2.Add(_colors[i], JwFileConsts.JwFileColors[i]);
                        }
                    }
                }
            }
        }



        private string _s;
        public string S
        {
            get { return _s; }
            set { _s = value; 
            Invalidate();
            }
        }

        public void draw()
        {
            Invalidate();
        }

        private void drawBeams(PaintEventArgs pe)
        {
            if(isdraw)
            {
                var z = pe.Graphics;
                using (z)
                {
                    var wth = _maxx - _minx;
                    var hth = _maxy - _miny;
                    var wscale = Convert.ToSingle((this.Width - 20) / wth);
                    var hscale = Convert.ToSingle((this.Height - 20) / hth);
                    var scale = Math.Min(wscale, hscale);
                    Matrix myMatrix = new Matrix(1, 0, 0, -1, 0, 0);
                    myMatrix.Scale(scale, scale);
                    float flx = Convert.ToSingle(_minx) * scale - 20;
                    float fly = Convert.ToSingle(_maxy) * scale + 20;
                    z.Transform = myMatrix;
                    z.TranslateTransform(-flx, fly, MatrixOrder.Append);

                    //pe.Graphics.TranslateTransform(-origin.X,  origin.Y/fly);
                    //pe.Graphics.ScaleTransform(sscale, sscale);


                    Pen myPen = new Pen(Color.GreenYellow, 1 / scale);
                    Pen myPen2 = new Pen(Color.Gray, 1 / scale);

                    if (_showSen)
                    {
                        if (_sens != null)
                        {
                            if (_sens.Count > 0)
                            {
                                foreach (var se in _sens)
                                {
                                    if (se.m_nPenStyle == 2)
                                    {
                                        if (!isselectcolor)
                                        {
                                            var pen = new Pen(_colors2[se.m_nPenColor], 1 / scale);
                                            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                                            z.DrawLine(pen, Convert.ToSingle(se.m_start_x), Convert.ToSingle(se.m_start_y), Convert.ToSingle(se.m_end_x), Convert.ToSingle(se.m_end_y));
                                        }
                                        else
                                        {
                                            if (se.m_nPenColor == selectedcolorint)
                                            {
                                                var pen = new Pen(_colors2[se.m_nPenColor], 1 / scale);
                                                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                                                z.DrawLine(pen, Convert.ToSingle(se.m_start_x), Convert.ToSingle(se.m_start_y), Convert.ToSingle(se.m_end_x), Convert.ToSingle(se.m_end_y));
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        if (!isselectcolor)
                                        {
                                            var pen = new Pen(_colors2[se.m_nPenColor], 1 / scale);
                                            //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                                            //myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                                            z.DrawLine(pen, Convert.ToSingle(se.m_start_x), Convert.ToSingle(se.m_start_y), Convert.ToSingle(se.m_end_x), Convert.ToSingle(se.m_end_y));
                                        }
                                        else
                                        {
                                            if (se.m_nPenColor == selectedcolorint)
                                            {
                                                var pen = new Pen(_colors2[se.m_nPenColor], 1 / scale);
                                                //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                                                //myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                                                z.DrawLine(pen, Convert.ToSingle(se.m_start_x), Convert.ToSingle(se.m_start_y), Convert.ToSingle(se.m_end_x), Convert.ToSingle(se.m_end_y));
                                            }
                                        }
                                        
                                    }

                                }
                            }
                        }
                    }

                    if (_showShape)
                    {
                        if (_solid != null)
                        {
                            if (_solid.Count > 0)
                            {
                                foreach (var so in _solid)
                                {
                                    if (!isselectcolor)
                                    {
                                        PointF[] points = new PointF[] { new PointF((float)so.m_start_x, (float)so.m_start_y), new PointF((float)so.m_DPoint2_x, (float)so.m_DPoint2_y), new PointF((float)so.m_DPoint3_x, (float)so.m_DPoint3_y), new PointF((float)so.m_end_x, (float)so.m_end_y) };
                                        SolidBrush brushsolid = new SolidBrush(_colors2[so.m_nPenColor]);
                                        //g.DrawPolygon(pen, points);
                                        z.FillPolygon(brushsolid, points);
                                    }
                                    else
                                    {
                                        if (so.m_nPenColor == selectedcolorint)
                                        {
                                            PointF[] points = new PointF[] { new PointF((float)so.m_start_x, (float)so.m_start_y), new PointF((float)so.m_DPoint2_x, (float)so.m_DPoint2_y), new PointF((float)so.m_DPoint3_x, (float)so.m_DPoint3_y), new PointF((float)so.m_end_x, (float)so.m_end_y) };
                                            SolidBrush brushsolid = new SolidBrush(_colors2[so.m_nPenColor]);
                                            //g.DrawPolygon(pen, points);
                                            z.FillPolygon(brushsolid, points);
                                        }
                                    }
                                }
                            }
                        }
                        if (_blocks != null)
                        {
                            if (_blocks.Count > 0)
                            {
                                foreach (var block in _blocks)
                                {
                                    if (!isselectcolor)
                                    {
                                        PointF[] points = new PointF[block.BlockPoint.Count];
                                        var i = 0;
                                        foreach (var p in block.BlockPoint)
                                        {
                                            points[i] = new PointF((float)p.X, (float)p.Y);
                                            i++;
                                        }
                                        SolidBrush brushsolid = new SolidBrush(_colors2[block.ColorInt]);
                                        //g.DrawPolygon(pen, points);
                                        z.FillPolygon(brushsolid, points);
                                    }
                                    else
                                    {
                                        if (block.ColorInt == selectedcolorint)
                                        {
                                            PointF[] points = new PointF[block.BlockPoint.Count];
                                            var i = 0;
                                            foreach (var p in block.BlockPoint)
                                            {
                                                points[i] = new PointF((float)p.X, (float)p.Y);
                                                i++;
                                            }
                                            SolidBrush brushsolid = new SolidBrush(_colors2[block.ColorInt]);
                                            //g.DrawPolygon(pen, points);
                                            z.FillPolygon(brushsolid, points);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (_colors2.Count > 0)
                    {
                        z.ResetTransform();
                        //float starty = (float)_maxy;
                        float starty = 20;
                        

                        foreach (var color in _colors2)
                        {
                            //Font ftext = new Font(FontFamily.GenericMonospace,10F, FontStyle.Bold);
                            //var stext = pe.Graphics.MeasureString(color.Key.ToString(), ftext);
                            //var nl = new PointF((float)_maxx+stext.Width/2, starty);
                            //SolidBrush brushsolid = new SolidBrush(color.Value);
                            //z.DrawString(color.Key.ToString(), ftext, brushsolid, nl);
                            //starty -= 50;
                            if (!isselectcolor)
                            {
                                System.Drawing.Font ftext = new System.Drawing.Font(FontFamily.GenericMonospace, 30F, FontStyle.Bold);
                                var stext = pe.Graphics.MeasureString(color.Key.ToString(), ftext);
                                var nl = new PointF(this.Width - 100, starty);
                                RectangleF trf = new RectangleF(nl, new SizeF(stext.Width, stext.Height));
                                if (!colorrectangles.ContainsKey(color.Key))
                                {
                                    colorrectangles.Add(color.Key, trf);
                                }

                                SolidBrush brushsolid = new SolidBrush(color.Value);
                                z.DrawString(color.Key.ToString(), ftext, brushsolid, nl);
                                starty += 80;
                            }
                            else
                            {
                                System.Drawing.Font ftext;
                                if (color.Key == selectedcolorint)
                                {
                                    ftext = new System.Drawing.Font(FontFamily.GenericMonospace, 38F, FontStyle.Underline);
                                    //現在選択されている色
                                    //Font ftext = new Font(FontFamily.GenericMonospace, 30F, FontStyle.Bold);
                                    var stext = pe.Graphics.MeasureString("現在選択されている色番号は :" + color.Key.ToString(), ftext);
                                    var stext1 = pe.Graphics.MeasureString("現在選択されている色番号は :", ftext);
                                    var nl = new PointF(this.Width - 100-stext1.Width, starty);
                                    RectangleF trf = new RectangleF(nl, new SizeF(stext.Width, stext.Height));
                                    if (!colorrectangles.ContainsKey(color.Key))
                                    {
                                        colorrectangles.Add(color.Key, trf);
                                    }

                                    SolidBrush brushsolid = new SolidBrush(color.Value);
                                    z.DrawString("現在選択されている色番号は :" + color.Key.ToString(), ftext, brushsolid, nl);
                                    starty += 80;
                                }
                                else
                                {
                                    ftext = new System.Drawing.Font(FontFamily.GenericMonospace, 30F, FontStyle.Bold);
                                    //Font ftext = new Font(FontFamily.GenericMonospace, 30F, FontStyle.Bold);
                                    var stext = pe.Graphics.MeasureString(color.Key.ToString(), ftext);
                                    var nl = new PointF(this.Width - 100, starty);
                                    RectangleF trf = new RectangleF(nl, new SizeF(stext.Width, stext.Height));
                                    if (!colorrectangles.ContainsKey(color.Key))
                                    {
                                        colorrectangles.Add(color.Key, trf);
                                    }

                                    SolidBrush brushsolid = new SolidBrush(color.Value);
                                    z.DrawString(color.Key.ToString(), ftext, brushsolid, nl);
                                    starty += 80;
                                }
                               
                            }
                            
                        }
                    }
                }
            }
        }

        Dictionary<int, RectangleF> colorrectangles=new Dictionary<int, RectangleF>();

        private bool HasMouseDownPoint = false;

        private Point MouseDownPoint;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            MouseDownPoint = new Point(e.X, e.Y);
            HasMouseDownPoint = true;
        }

        bool isselectcolor = false;
        int selectedcolorint = -1;
        protected override void OnClick(EventArgs e)
        {
           
                var z = MouseDownPoint;
                bool re = false;
                isselectcolor = false;
                
                foreach (var beam in colorrectangles)
                {
                    if (beam.Value.Contains(z))
                    {
                    //beam.NeedReDraw = true;
                    isselectcolor = true;
                        re = true;
                    selectedcolorint = beam.Key;
                        break;
                    }

                }
                if (re)
                {
                    Invalidate();

                }
            else
            {
                isselectcolor = false;
                selectedcolorint = -1;
                Invalidate();
            }
            
            base.OnClick(e);
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            
            base.OnPaint(pe);
            drawBeams(pe);

        }
    }
}
