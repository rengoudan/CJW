using AntdUI.Svg;
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
    public partial class SingleBeamShow : Control
    {
        public SingleBeamShow()
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


        public SingleBeamShow(double minx,double maxx,double miny,double maxy,List<JwwSen> sens,List<JwwSolid> solids,List<int> colors,List<JwBlock> blocks) 
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

        List<JwwSen> _senlst;
        List<JwwEnko> _enkolst;
        List<JwwSunpou> _sunpoulst;
        List<double> xlst;
        List<double> ylst;
        public SingleBeamShow(List<JwwData> datas)
        {
            xlst = new List<double>();
            ylst = new List<double>();
            _senlst = new List<JwwSen>();
            _enkolst = new List<JwwEnko>();
             _sunpoulst = new List<JwwSunpou>();

            foreach (var data in datas)
            {
                string typename = data.GetType().Name;
                if(typename == "JwwSen")
                {
                    if(_senlst == null)
                    {
                        _senlst = new List<JwwSen>();
                    }
                    _senlst.Add((JwwSen)data);
                    xlst.Add(((JwwSen)data).m_start_x);
                    xlst.Add(((JwwSen)data).m_end_x);
                    ylst.Add(((JwwSen)data).m_start_y);
                    ylst.Add(((JwwSen)data).m_end_y);
                }
                else if(typename == "JwwEnko")
                {
                    if(_enkolst == null)
                    {
                        _enkolst = new List<JwwEnko>();
                    }
                    _enkolst.Add((JwwEnko)data);
                }
                else if(typename== "JwwSunpou")
                {
                    var sunpou = (JwwSunpou)data;
                    _sunpoulst.Add(sunpou);
                    xlst.Add(sunpou.m_Sen.m_end_x);
                    ylst.Add(sunpou.m_Sen.m_end_y);
                    xlst.Add(sunpou.m_Sen.m_start_x);
                    ylst.Add(sunpou.m_Sen.m_start_y);

                }
            }
            _minx = xlst.Min() - 2;
            _maxx = xlst.Max() + 2;
            _miny = ylst.Min() - 2;
            _maxy = ylst.Max() + 2;
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
        System.Drawing.Font ftext = new System.Drawing.Font(FontFamily.GenericMonospace, 30F, FontStyle.Bold);
        private void drawBeams(PaintEventArgs pe)
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
                float flx = Convert.ToSingle(_minx) * scale;
                float fly = Convert.ToSingle(_maxy) * scale;
                z.Transform = myMatrix;
                z.TranslateTransform(-flx, fly, MatrixOrder.Append);

                //pe.Graphics.TranslateTransform(-origin.X,  origin.Y/fly);
                //pe.Graphics.ScaleTransform(sscale, sscale);


                Pen myPen = new Pen(Color.GreenYellow, 1 / scale);
                Pen myPen2 = new Pen(Color.Gray, 1 / scale);

                if (_sens != null)
                {
                    if (_sens.Count > 0)
                    {
                        foreach (var se in _sens)
                        {
                            if (se.m_nPenStyle == 2)
                            {
                                var pen = new Pen(System.Drawing.Color.Red, 1 / scale);
                                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                                z.DrawLine(pen, Convert.ToSingle(se.m_start_x), Convert.ToSingle(se.m_start_y), Convert.ToSingle(se.m_end_x), Convert.ToSingle(se.m_end_y));
                            }
                            else
                            {
                                var pen = new Pen(System.Drawing.Color.White, 1 / scale);
                                //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                                //myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                                z.DrawLine(pen, Convert.ToSingle(se.m_start_x), Convert.ToSingle(se.m_start_y), Convert.ToSingle(se.m_end_x), Convert.ToSingle(se.m_end_y));
                            }

                        }
                    }
                }

                if(_enkolst?.Count>0)
                {
                    foreach(var enko in _enkolst)
                    {
                        var pen = new Pen(System.Drawing.Color.Yellow, 1 / scale);
                        z.DrawEllipse(pen, Convert.ToSingle(enko.m_start_x - enko.m_dHankei), Convert.ToSingle(enko.m_start_y - enko.m_dHankei), Convert.ToSingle(enko.m_dHankei * 2), Convert.ToSingle(enko.m_dHankei * 2));
                    }
                }

                if(_sunpoulst?.Count>0)
                {
                    foreach(var sunpou in _sunpoulst)
                    {
                        var pen = new Pen(System.Drawing.Color.Cyan, 1 / scale);
                        z.DrawLine(pen, Convert.ToSingle(sunpou.m_Sen.m_start_x), Convert.ToSingle(sunpou.m_Sen.m_start_y), Convert.ToSingle(sunpou.m_Sen.m_end_x), Convert.ToSingle(sunpou.m_Sen.m_end_y));

                        var smsg = sunpou.m_Moji.m_string;
                        SolidBrush brushsolid = new SolidBrush(System.Drawing.Color.White);
                        z.DrawString(smsg, ftext, brushsolid,new System.Drawing.PointF(Convert.ToSingle(sunpou.m_Moji.m_start_x),Convert.ToSingle(sunpou.m_Moji.m_start_y)));

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
