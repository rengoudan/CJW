using Flurl.Http.Testing;
using JwCore;
using JwShapeCommon.Model;
using JwwHelper;
using Microsoft.EntityFrameworkCore;
using RGB.Jw.JW;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JwShapeCommon
{
    /// <summary>
    /// Y坐标从低到高
    /// </summary>
    public class NewJwBeamJwDraw
    {
        private JwBeam _beam;

        public bool CanDraw = false;

        public double XXLength = 0;

        public double BLength = 0;

        private double showlength = 0;

        private List<double> _xlst = new List<double>();

        private List<double> _ylst = new List<double>();

        private double _jiangeY = 0;

        private double _starty = 0;

        private double _fuzhuY = 0;

        /// <summary>
        /// 统一为水平，X 从零开始（通过累加 相对距离） Y 分配固定 上面 12 中间0 下面-12 
        /// </summary>
        /// <param name="beam"></param>
        public NewJwBeamJwDraw(JwBeam beam)
        {
            if (beam.DirectionType == BeamDirectionType.Horizontal)
            {
                _beam = beam;
                _beam.AbsolutePD = _beam.TopLeft.X;
            }
            else
            {
                _beam = JwShapeHelper.VerticalToHorizontal(beam);
                _beam.AbsolutePD = _beam.TopLeft.X;
                //    JwDrawShape beamsp = new JwDrawShape(verticalbeam);
                //    controls.AddRange(beamsp.Change(_minbeilv, axisX, axisY));
            }
            //_beam.AbsolutePD = Math.Round(beam.TopLeft.X, 6);//不用更改数据库从新生成间隔数据
            this._jiangeY = 4;
            this._fuzhuY = -this._jiangeY-0.4;
            this.banjing = JwFileConsts.EllipseDiameter / (2*JwFileConsts.JwScale);
            this.reset();
        }


        public double Minx
        {
            get
            {
                if (_xlst.Count > 0)
                {
                    return _xlst.Min();
                }
                else
                {
                    return 0;
                }
            }
        }

        public double Maxx
        {
            get
            {
                if (_xlst.Count > 0)
                {
                    return _xlst.Max();
                }
                else
                {
                    return 0;
                }
            }
        }

        public double Miny
        {
            get
            {
                if (_ylst.Count > 0)
                {
                    return _ylst.Min();
                }
                else
                {
                    return 0;
                }
            }
        }

        public double Maxy
        {
            get
            {
                if (_ylst.Count > 0)
                {
                    return _ylst.Max();
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 绘制核心方法
        /// </summary>
        /// <param name="wwidth"></param>
        /// <param name="wheight"></param>
        public void CreateControlDraw(int wwidth, int wheight)
        {
            double bl = 0;
            if (showlength != 0)
            {
                ControlDraws = new List<ControlDraw>();
                bl = Math.Min(Math.Round(wwidth / showlength, 2), Math.Round(wheight / 6d, 2));
                _xlst.Add(-2);
                _xlst.Add(showlength + 2);
                _ylst.Add(0);

                PointF pftop = new PointF((float)startx, (float)0);
                //var z = new RectangleF(pf, new SizeF((float)(bl * showlength), (float)(bl * 1)));
                var topbeam = new RectangleF(pftop, new SizeF((float)(BLength), (float)(1)));
                ControlDraw draw = new ControlDraw();
                draw.PenColor = Color.White;
                draw.DrawRectangleF = topbeam;
                draw.ShapeType = DrawShapeType.Beam;
                //draw.JwSquareBase = jwShape;
                ControlDraws.Add(draw);
                //宽度为1 2 ，间隔5试一下
                _ylst.Add(_jiangeY);
                PointF pfcenter = new PointF((float)startx, (float)_jiangeY);
                var centerbeam = new RectangleF(pfcenter, new SizeF((float)(BLength), (float)(2)));
                ControlDraw centerdraw = new ControlDraw();
                centerdraw.PenColor = Color.White;
                centerdraw.DrawRectangleF = centerbeam;
                centerdraw.ShapeType = DrawShapeType.Beam;
                ControlDraws.Add(centerdraw);

                PointF pfbottom = new PointF((float)startx, (float)_jiangeY * 2);
                _ylst.Add(_jiangeY * 2);
                var bottombeam = new RectangleF(pfbottom, new SizeF((float)(BLength), (float)(1)));
                ControlDraw bottomdraw = new ControlDraw();
                bottomdraw.PenColor = Color.White;
                bottomdraw.DrawRectangleF = bottombeam;
                bottomdraw.ShapeType = DrawShapeType.Beam;
                ControlDraws.Add(bottomdraw);



                //由小到大
                _beam.jwBeamMarks = _beam.jwBeamMarks.OrderBy(t => t.Coordinate).ToList();

                double prex = 0;

                Lines = new List<ControlLine>();


                float sx = 0, ex = 0;

                bool iadd = false;
                foreach (var m in _beam.jwBeamMarks)
                {
                    if (m.IsCenter)
                    {
                        if (m.IsCenterStart)
                        {

                            sx = (float)prex;
                            iadd = true;
                            if (m.HasAppend)
                            {
                                drawhole(m.AppendHole, m.IsBias, true);
                            }
                            drawfzline(sx, (float)_jiangeY);

                            drawline((float)startx, sx, (float)(2 * _jiangeY + 2));
                        }
                        else if (m.IsCenterEnd)
                        {
                            drawline(sx, (float)XXLength, (float)(2 * _jiangeY + 2));
                            if (m.HasAppend)
                            {
                                drawhole(m.AppendHole, m.IsBias, false, true);
                            }
                            drawfzline((float)XXLength, (float)_jiangeY);

                            drawline((float)endx, (float)XXLength, (float)(2 * _jiangeY + 2));
                        }
                        else
                        {
                            double zb = prex + (m.HasError ? m.PreCenterCorrect : m.PreCenterDistance);

                            prex = zb;
                            drawfzline((float)prex, (float)_jiangeY);
                            if (iadd)
                            {
                                ex = (float)prex;

                                drawline(sx, ex, (float)(2 * _jiangeY + 2));
                                sx = (float)prex;
                            }
                            else
                            {
                                iadd = true;
                                sx = (float)prex;
                            }
                            drawhole(m.AppendHole, zb);

                        }

                    }
                }
                //芯距离
                drawline(0, (float)XXLength, (float)(_jiangeY - 1), true);

                //梁长度
                drawline((float)startx, (float)endx, (float)(_jiangeY + 3), false, true);
            }
        }

        private void drawfzline(float x, float sy)
        {
            ControlLine l = new ControlLine();
            l.DrawStart = new PointF(x, sy);
            l.DrawEnd = new PointF(x, sy);
            l.HasMsg = false;
            this.Lines.Add(l);
        }

        /// <summary>
        /// 绘制标识线
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="ex"></param>
        /// <param name="y"></param>
        /// <param name="isxx"></param>
        /// <param name="isbeaml"></param>
        private void drawline(float sx, float ex, float y, bool isxx = false, bool isbeaml = false)
        {
            ControlLine l = new ControlLine();
            l.DrawStart = new PointF(sx, y);
            l.DrawEnd = new PointF(ex, y);
            l.Distance = Math.Round(Math.Abs(Math.Round(ex - sx, 2)) * JwFileConsts.JwScale, 0);
            l.Title = string.Format("{0}mm", l.Distance);
            l.HasMsg = true;
            l.IsXX = isxx;
            l.IsBeaml = isbeaml;
            if (l.IsXX)
            {
                l.Title = string.Format("コア距離:{0}mm", l.Distance);
            }
            if (isbeaml)
            {
                l.Title = string.Format("梁の長さ:{0}mm", l.Distance);
            }
            this.Lines.Add(l);
        }

        private void drawhole(JwHole h, double x)
        {
            double tcy = _starty + 0.5;

            double ccy = _jiangeY + 1;

            double bcy = _jiangeY * 2 + 0.5;
            double halfbj = JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale);
            double kzhijing = JwFileConsts.EllipseDiameter / JwFileConsts.JwScale;
            double locationx = Math.Round(h.Location.X, 6) + offsetX;
            var sz = new SizeF((float)kzhijing, (float)kzhijing);

            var chx = (float)(x - halfbj - kzhijing / 2);
            var chyx = (float)(x + halfbj - kzhijing / 2);
            if (h.HasTop)
            {
                _createsinglehole(chx, (float)tcy);
                _createsinglehole(chx, (float)tcy, true);
                _createsinglehole(chyx, (float)tcy);
                _createsinglehole(chyx, (float)tcy, true);
            }
            if (h.HasCenter)
            {
                _createsinglehole(chx, (float)ccy);
                _createsinglehole(chx, (float)ccy, true);
                _createsinglehole(chyx, (float)ccy);
                _createsinglehole(chyx, (float)ccy, true);
            }
            if (h.HasBottom)
            {
                _createsinglehole(chx, (float)bcy);
                _createsinglehole(chx, (float)bcy, true);
                _createsinglehole(chyx, (float)bcy);
                _createsinglehole(chyx, (float)bcy, true);

                if (h.HasBhLinkHole)
                {
                    double hxf = chyx + JwFileConsts.Kongjing / JwFileConsts.JwScale;
                    _createsinglehole((float)hxf, (float)bcy);
                    _createsinglehole((float)hxf, (float)bcy,true);
                    //createhole(hxf, bcy - halfbj);
                    //createhole(hxf, bcy + halfbj);
                }
                if (h.HasPreLinkHole)
                {
                    double hxf = chx - JwFileConsts.Kongjing / JwFileConsts.JwScale;
                    _createsinglehole((float)hxf, (float)bcy);
                    _createsinglehole((float)hxf, (float)bcy, true);
                }
            }


        }

        /// <summary>
        /// jwfileconsts kongjing 为孔组间距
        /// </summary>
        /// <param name="h"></param>
        /// <param name="isbis"></param>
        /// <param name="istart"></param>
        /// <param name="isend"></param>
        private void drawhole(JwHole h, bool isbis, bool istart = false, bool isend = false)
        {
            double tcy = _starty + 0.5;

            double ccy = _jiangeY + 1;

            double bcy = _jiangeY * 2 + 0.5;
            double halfbj = JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale);
            double kzhijing = JwFileConsts.EllipseDiameter / JwFileConsts.JwScale;
            double locationx = Math.Round(h.Location.X, 6) + offsetX;
            var sz = new SizeF((float)kzhijing, (float)kzhijing);
            if (istart)
            {
                if (_beam.StartTelosType == KongzuType.B)
                {
                    if (isbis)
                    {
                        var hx = (float)(locationx - halfbj - kzhijing / 2);
                        _createsinglehole(hx, (float)tcy);
                        _createsinglehole(hx, (float)tcy, true);
                        _createsinglehole(hx, (float)ccy);
                        _createsinglehole(hx, (float)ccy, true);
                        _createsinglehole(hx, (float)bcy);
                        _createsinglehole(hx, (float)bcy, true);
                    }
                    else
                    {
                        var hx = (float)(locationx - halfbj - kzhijing / 2);
                        var hyx = (float)(locationx + halfbj - kzhijing / 2);
                        _createsinglehole(hx, (float)tcy);
                        _createsinglehole(hx, (float)tcy, true);
                        _createsinglehole(hx, (float)ccy);
                        _createsinglehole(hx, (float)ccy, true);
                        _createsinglehole(hx, (float)bcy);
                        _createsinglehole(hx, (float)bcy, true);

                        _createsinglehole(hyx, (float)tcy);
                        _createsinglehole(hyx, (float)tcy, true);
                        _createsinglehole(hyx, (float)ccy);
                        _createsinglehole(hyx, (float)ccy, true);
                        _createsinglehole(hyx, (float)bcy);
                        _createsinglehole(hyx, (float)bcy, true);
                    }
                }
                else if (_beam.StartTelosType == KongzuType.G)
                {
                    var hx = (float)(90 / JwFileConsts.JwScale);
                    _createsinglehole(hx, (float)tcy);
                    _createsinglehole(hx, (float)tcy, true);
                    _createsinglehole(hx, (float)ccy);
                    _createsinglehole(hx, (float)ccy, true);
                    _createsinglehole(hx, (float)bcy);
                    _createsinglehole(hx, (float)bcy, true);
                }
                else
                {
                    var hx = (float)(28 / JwFileConsts.JwScale);
                    _createsinglehole(hx, (float)tcy);
                    _createsinglehole(hx, (float)tcy, true);
                    _createsinglehole(hx, (float)ccy);
                    _createsinglehole(hx, (float)ccy, true);
                    _createsinglehole(hx, (float)bcy);
                    _createsinglehole(hx, (float)bcy, true);

                    if (h.HasBhLinkHole)
                    {
                        double hxf = hx + JwFileConsts.Kongjing / JwFileConsts.JwScale;
                        _createsinglehole((float)hxf, (float)bcy);
                        _createsinglehole((float)hxf, (float)bcy, true);
                        //createhole(hxf, bcy - halfbj);
                        //createhole(hxf, bcy + halfbj);
                    }
                    //if (h.HasPreLinkHole)
                    //{
                    //    double hxf = hx - JwFileConsts.Kongjing / JwFileConsts.JwScale;
                    //    _createsinglehole((float)hxf, (float)bcy);
                    //    _createsinglehole((float)hxf, (float)bcy, true);
                    //}

                }


                //else
                //{
                //    var hx = (float)(locationx);
                //    _createsinglehole(hx, (float)tcy);
                //    _createsinglehole(hx, (float)tcy, true);
                //    _createsinglehole(hx, (float)ccy);
                //    _createsinglehole(hx, (float)ccy, true);
                //    _createsinglehole(hx, (float)bcy);
                //    _createsinglehole(hx, (float)bcy, true);

                //}
            }
            else if (isend)
            {
                double endzb = XXLength;
                if (_beam.EndTelosType == KongzuType.B)
                {
                    if (isbis)
                    {
                        //double endzb = XXLength;
                        var hx = (float)(endzb + halfbj - kzhijing / 2);
                        _createsinglehole(hx, (float)tcy);
                        _createsinglehole(hx, (float)tcy, true);
                        _createsinglehole(hx, (float)ccy);
                        _createsinglehole(hx, (float)ccy, true);
                        _createsinglehole(hx, (float)bcy);
                        _createsinglehole(hx, (float)bcy, true);
                    }
                    else
                    {
                        var hx = (float)(endzb - halfbj - kzhijing / 2);
                        var hyx = (float)(endzb + halfbj - kzhijing / 2);
                        _createsinglehole(hx, (float)tcy);
                        _createsinglehole(hx, (float)tcy, true);
                        _createsinglehole(hx, (float)ccy);
                        _createsinglehole(hx, (float)ccy, true);
                        _createsinglehole(hx, (float)bcy);
                        _createsinglehole(hx, (float)bcy, true);

                        _createsinglehole(hyx, (float)tcy);
                        _createsinglehole(hyx, (float)tcy, true);
                        _createsinglehole(hyx, (float)ccy);
                        _createsinglehole(hyx, (float)ccy, true);
                        _createsinglehole(hyx, (float)bcy);
                        _createsinglehole(hyx, (float)bcy, true);
                    }
                }
                else if (_beam.EndTelosType == KongzuType.G)
                {
                    var hx = (float)(endzb - 90 / JwFileConsts.JwScale);
                    _createsinglehole(hx, (float)tcy);
                    _createsinglehole(hx, (float)tcy, true);
                    _createsinglehole(hx, (float)ccy);
                    _createsinglehole(hx, (float)ccy, true);
                    _createsinglehole(hx, (float)bcy);
                    _createsinglehole(hx, (float)bcy, true);
                }
                else
                {
                    var hx = (float)(endzb - 28 / JwFileConsts.JwScale);
                    _createsinglehole(hx, (float)tcy);
                    _createsinglehole(hx, (float)tcy, true);
                    _createsinglehole(hx, (float)ccy);
                    _createsinglehole(hx, (float)ccy, true);
                    _createsinglehole(hx, (float)bcy);
                    _createsinglehole(hx, (float)bcy, true);

                    //if (h.HasBhLinkHole)
                    //{
                    //    double hxf = hx + JwFileConsts.Kongjing / JwFileConsts.JwScale;
                    //    _createsinglehole((float)hxf, (float)bcy);
                    //    _createsinglehole((float)hxf, (float)bcy, true);
                    //    //createhole(hxf, bcy - halfbj);
                    //    //createhole(hxf, bcy + halfbj);
                    //}
                    if (h.HasPreLinkHole)
                    {
                        double hxf = hx - JwFileConsts.Kongjing / JwFileConsts.JwScale;
                        _createsinglehole((float)hxf, (float)bcy);
                        _createsinglehole((float)hxf, (float)bcy, true);
                    }
                }
            }
            else
            {
                var chx = (float)(locationx - halfbj - kzhijing / 2);
                var chyx = (float)(locationx + halfbj - kzhijing / 2);
                if (h.HasTop)
                {
                    _createsinglehole(chx, (float)tcy);
                    _createsinglehole(chx, (float)tcy, true);
                    _createsinglehole(chyx, (float)tcy);
                    _createsinglehole(chyx, (float)tcy, true);
                }
                if (h.HasCenter)
                {
                    _createsinglehole(chx, (float)ccy);
                    _createsinglehole(chx, (float)ccy, true);
                    _createsinglehole(chyx, (float)ccy);
                    _createsinglehole(chyx, (float)ccy, true);
                }
                if (h.HasBottom)
                {
                    _createsinglehole(chx, (float)bcy);
                    _createsinglehole(chx, (float)bcy, true);
                    _createsinglehole(chyx, (float)bcy);
                    _createsinglehole(chyx, (float)bcy, true);
                }

                if (h.HasBhLinkHole)
                {
                    double hxf = chyx + JwFileConsts.Kongjing / JwFileConsts.JwScale;
                    _createsinglehole((float)hxf, (float)bcy);
                    _createsinglehole((float)hxf, (float)bcy, true);
                    //createhole(hxf, bcy - halfbj);
                    //createhole(hxf, bcy + halfbj);
                }
                if (h.HasPreLinkHole)
                {
                    double hxf = chx - JwFileConsts.Kongjing / JwFileConsts.JwScale;
                    _createsinglehole((float)hxf, (float)bcy);
                    _createsinglehole((float)hxf, (float)bcy, true);
                }
            }
        }

        private void _createsinglehole(float x, float y, bool istop = false)
        {
            double kzhijing = JwFileConsts.EllipseDiameter / JwFileConsts.JwScale;
            double halfbj = JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale);
            var hy = istop ? (float)(y - halfbj - kzhijing / 2) : (float)(y + halfbj - kzhijing / 2);

            var sz = new SizeF((float)kzhijing, (float)kzhijing);
            var z = new PointF(x, hy);
            var zs = new RectangleF(z, sz);
            ControlDraw draw = new ControlDraw();
            draw.PenColor = Color.Yellow;
            draw.DrawRectangleF = zs;
            draw.ShapeType = DrawShapeType.Hole;
            ControlDraws.Add(draw);
        }


        private void reset()
        {
            foreach (var z in _beam.jwBeamMarks)
            {
                XXLength=XXLength+(z.HasError?z.PreCenterCorrect:z.PreCenterDistance);
                //if (z.HasAppend)
                //{
                //    var q = _beam.Baifangs.Find(t => t.Center == z.AppendHole.HoleCenter);
                //    if (q != null)
                //    {
                //        z.AppendHole.HasBhLinkHole = q.HasLast;
                //        z.AppendHole.HasPreLinkHole = q.HasPre;
                //    }
                //}
            }
            //foreach(var hhhh in _beam.Holes)
            //{
                
            //        var q = _beam.Baifangs.Find(t => t.Center == hhhh.HoleCenter);
            //        if (q != null)
            //        {
            //        hhhh.HasBhLinkHole = q.HasLast;
            //        hhhh.HasPreLinkHole = q.HasPre;
            //        }
               
            //}
            XXLength = Math.Round(XXLength, 2);
            //XXLength = Math.Round(_beam.jwBeamMarks.Sum(t => t.PreCenterDistance), 2);
            var xmark = _beam.jwBeamMarks.Find(t => t.IsCenterStart);
            if (xmark != null)
            {
                CanDraw = true;
                offsetX = -xmark.Coordinate;//芯起点默认为0 重置各类坐标  
                xstartx = 0;
                xendx = XXLength;
                BLength = XXLength;
                if (_beam.StartTelosType == KongzuType.B)
                {
                    BLength = BLength + 50 / JwFileConsts.JwScale;
                    startx = this.xstartx - 50 / JwFileConsts.JwScale;
                }
                if (_beam.StartTelosType == KongzuType.G)
                {

                    BLength = BLength - 55 / JwFileConsts.JwScale;
                    startx = this.xstartx + 55 / JwFileConsts.JwScale;
                }
                if (_beam.StartTelosType == KongzuType.J)
                {
                    BLength = BLength - 3 / JwFileConsts.JwScale;
                    startx = this.xstartx + 3 / JwFileConsts.JwScale;
                }
                if (_beam.EndTelosType == KongzuType.B)
                {
                    BLength = BLength + 50 / JwFileConsts.JwScale;
                    endx = xendx + 50 / JwFileConsts.JwScale;
                }
                if (_beam.EndTelosType == KongzuType.G)
                {
                    BLength = BLength - 55 / JwFileConsts.JwScale;
                    endx = this.xendx - 55 / JwFileConsts.JwScale;
                }
                if (_beam.EndTelosType == KongzuType.J)
                {
                    BLength = BLength - 3 / JwFileConsts.JwScale;
                    endx = xendx - 3 / JwFileConsts.JwScale;
                }
                //startx=
                showlength = Math.Max(XXLength, BLength);
            }
            //默认芯起点为00 则可以直接使用predistance来生成孔组位置
            //这种情况下offset就为 负 芯起点 
        }

        public List<ControlDraw> ControlDraws { get; set; }

        public List<ControlLine> Lines { get; set; }

        public List<JwwData> Datas = new List<JwwData>();

        public List<JwwSen> Sens = new List<JwwSen>();

        public List<JwwTen> Tens = new List<JwwTen>();

        public List<JwwEnko> Enkos = new List<JwwEnko>();

        public List<JwwMoji> Mojis = new List<JwwMoji>();

        public List<JwwSunpou> Biaozhu = new List<JwwSunpou>();

        private double offsetX = 0;
        private double offsetY = 0;
        private double offsetMsg = 0;

        private double centerHeight = 0;

        private double jianju = 0;

        /// <summary>
        /// 头心的x坐标
        /// </summary>
        private double xstartx = 0;

        private double startx = 0;

        /// <summary>
        /// 尾心的x坐标
        /// </summary>
        private double xendx = 0;

        private double endx = 0;

        private double banjing = 0;

        public void CreateBeam()
        {
            double tpy = _jiangeY + 2;

            double cpy = 0;

            double bpy = -_jiangeY - 2;
            var hodu = JwFileConsts.Lianghoudu / JwFileConsts.JwScale;
            JwwSen leftline = CreateSen(startx, cpy, startx, cpy - 2);
            Sens.Add(leftline);
            JwwSen rightline = CreateSen(endx, cpy, endx, cpy - 2);
            Sens.Add(rightline);
            JwwSen topline = CreateSen(startx, cpy, endx, cpy);
            Sens.Add(topline);
            JwwSen bottomline = CreateSen(startx, cpy - 2, endx, cpy - 2);
            Sens.Add(bottomline);
            JwwSen topfz = CreateSen(topline.m_start_x, topline.m_start_y - hodu, topline.m_end_x, topline.m_end_y - hodu, true);
            Sens.Add(topfz);
            JwwSen bottomfz = CreateSen(bottomline.m_start_x, bottomline.m_start_y + hodu, bottomline.m_end_x, bottomline.m_end_y + hodu, true);
            Sens.Add(bottomfz);

            JwwSen tleftline = CreateSen(startx, tpy, startx, tpy - 1);
            Sens.Add(tleftline);
            JwwSen trightline = CreateSen(endx, tpy, endx, tpy - 1);
            Sens.Add(trightline);
            JwwSen ttopline = CreateSen(startx, tpy, endx, tpy);
            Sens.Add(ttopline);
            JwwSen tbottomline = CreateSen(startx, tpy - 1, endx, tpy - 1);
            Sens.Add(tbottomline);
            JwwSen fushang = CreateSen(startx, tpy - 0.5 - hodu / 2, endx, tpy - 0.5 - hodu / 2, true);
            Sens.Add(fushang);
            JwwSen fuxia = CreateSen(startx, tpy - 0.5 + hodu / 2, endx, tpy - 0.5 + hodu / 2, true);
            Sens.Add(fuxia);


            JwwSen bleftline = CreateSen(startx, bpy, startx, bpy - 1);
            Sens.Add(bleftline);
            JwwSen brightline = CreateSen(endx, bpy, endx, bpy - 1);
            Sens.Add(brightline);
            JwwSen btopline = CreateSen(startx, bpy, endx, bpy);
            Sens.Add(btopline);
            JwwSen bbottomline = CreateSen(startx, bpy - 1, endx, bpy - 1);
            Sens.Add(bbottomline);
            JwwSen bfushang = CreateSen(startx, bpy - 0.5 - hodu / 2, endx, bpy - 0.5 - hodu / 2, true);
            Sens.Add(bfushang);
            JwwSen bfuxia = CreateSen(startx, bpy - 0.5 + hodu / 2, endx, bpy - 0.5 + hodu / 2, true);
            Sens.Add(bfuxia);



            //CreateMsgAndHole(offsetx, offsety, leftline.m_start_y + offsetMsg, iscenter);
            double shumsgx = leftline.m_start_x - 100 / JwFileConsts.JwScale;
            double shumsgxe = leftline.m_start_x - 50 / JwFileConsts.JwScale;
            createsinglemsg(shumsgx, shumsgxe, leftline.m_start_y, leftline.m_end_y, false);
            JwwMoji duan = new JwwMoji();
            duan.m_start_x = shumsgx - 200 / JwFileConsts.JwScale;
            duan.m_start_y = (leftline.m_start_y + leftline.m_end_y) * 3 / 4;
            duan.m_string = _beam.HasStartSide ? _beam.StartTelosType.ToString() : "B";
            duan.m_dSizeX = 200 / JwFileConsts.JwScale;
            duan.m_dSizeY = 200 / JwFileConsts.JwScale;
            Mojis.Add(duan);
            double endduanx = topline.m_end_x + 300 / JwFileConsts.JwScale;
            JwwMoji duanend = new JwwMoji();
            duanend.m_start_x = endduanx;
            duanend.m_start_y = (leftline.m_start_y + leftline.m_end_y) * 3 / 4;
            duanend.m_string = _beam.HasEndSide ? _beam.EndTelosType.ToString() : "B";
            duanend.m_dSizeX = 200 / JwFileConsts.JwScale;
            duanend.m_dSizeY = 200 / JwFileConsts.JwScale;
            Mojis.Add(duanend);

            //由小到大
            _beam.jwBeamMarks = _beam.jwBeamMarks.OrderBy(t => t.Coordinate).ToList();

            double prex = 0;

            Lines = new List<ControlLine>();
            //cretaeFuzhu();




            float sx = 0, ex = 0;

            bool iadd = false;
            fuzhuxian(startx);
            fuzhuxian(endx);
            fuzhuxian(XXLength);
            foreach (var m in _beam.jwBeamMarks)
            {
                if (m.IsCenter)
                {
                    fuzhuxian(prex);
                    if (m.IsCenterStart)
                    {

                        sx = (float)prex;
                        iadd = true;
                        if (m.HasAppend)
                        {
                            jwdrawhole(m.AppendHole, m.IsBias, true);
                        }
                        drawfzline(sx, (float)_jiangeY);

                        drawline((float)startx, sx, (float)(2 * _jiangeY + 2));
                        showprexindistance(startx, sx,true);//梁 边距离中心起始点的标注
                    }
                    else if (m.IsCenterEnd)
                    {
                        drawline(sx, (float)XXLength, (float)(2 * _jiangeY + 2));
                        if (m.HasAppend)
                        {
                            jwdrawhole(m.AppendHole, m.IsBias, false, true);
                        }
                        drawfzline((float)XXLength, (float)_jiangeY);

                        drawline((float)endx, (float)XXLength, (float)(2 * _jiangeY + 2));
                        showprexindistance(endx, XXLength,true);//梁结束距离中心点的标注
                    }
                    else
                    {
                        double zb = prex + (m.HasError ? m.PreCenterCorrect : m.PreCenterDistance);

                        prex = zb;
                        drawfzline((float)prex, (float)_jiangeY);
                        if (iadd)
                        {
                            ex = (float)prex;
                            showprexindistance(sx, ex);
                            drawline(sx, ex, (float)(2 * _jiangeY + 2));
                            sx = (float)prex;
                        }
                        else
                        {
                            iadd = true;
                            sx = (float)prex;
                        }
                        jwdrawhole(m.AppendHole, m.IsBias, false, false, zb);

                    }

                }
            }

            showprexindistance(prex, XXLength);
            showxx();
            showbeamlength();
            Datas.AddRange(this.Sens);
            Datas.AddRange(this.Biaozhu);
            Datas.AddRange(this.Tens);
            Datas.AddRange(this.Enkos);
            Datas.AddRange(this.Mojis);
        }

        private void jwdrawhole(JwHole h, bool isbis, bool istart = false, bool isend = false,double pre=0)
        {

            double tpy = _jiangeY + 2;

            double cpy = 0;

            double bpy = -_jiangeY - 2;

            double tcy = tpy - 0.5;

            double ccy = 0 - 1;

            double bcy = bpy - 0.5;
            double halfbj = JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale);
            double kzhijing = JwFileConsts.EllipseDiameter / JwFileConsts.JwScale;
            double locationx = Math.Round(h.Location.X, 6) + offsetX;
            var sz = new SizeF((float)kzhijing, (float)kzhijing);
            if (istart)
            {
                if (_beam.StartTelosType == KongzuType.B)
                {
                    if (isbis)
                    {
                        double hx = -halfbj;
                        createhole(hx, tcy+ halfbj);
                        createhole(hx, tcy- halfbj);

                        createhole(hx, ccy+ halfbj);
                        createhole(hx, ccy- halfbj);

                        createhole(hx, bcy+ halfbj);
                        createhole(hx, bcy - halfbj);
                    }
                    else
                    {
                        double hx =  - halfbj;
                        double hyx =  halfbj;
                        createhole(hx, tcy + halfbj);
                        createhole(hx, tcy - halfbj);
                        createhole(hx, ccy + halfbj);
                        createhole(hx, ccy- halfbj);
                        createhole(hx, bcy + halfbj);
                        createhole(hx, bcy- halfbj);

                        createhole(hyx,tcy + halfbj);
                        createhole(hyx,tcy - halfbj);
                        createhole(hyx,ccy + halfbj);
                        createhole(hyx,ccy- halfbj);
                        createhole(hyx,bcy + halfbj);
                        createhole(hyx,bcy - halfbj);
                    }
                    if (h.HasBhLinkHole)
                    {
                        double hx = -halfbj + JwFileConsts.Kongjing / JwFileConsts.JwScale;
                        createhole(hx, bcy - halfbj);
                        createhole(hx, bcy + halfbj);
                    }
                }
                else if (_beam.StartTelosType == KongzuType.G)
                {
                    var hx = (float)(90 / JwFileConsts.JwScale);
                    createhole(hx, tcy + halfbj);
                    createhole(hx,tcy - halfbj);
                    createhole(hx,ccy + halfbj);
                    createhole(hx,ccy - halfbj);
                    createhole(hx,bcy + halfbj);
                    createhole(hx,bcy - halfbj);
                    if (h.HasBhLinkHole)
                    {
                        double hxf = hx + JwFileConsts.Kongjing / JwFileConsts.JwScale;
                        createhole(hxf, bcy - halfbj);
                        createhole(hxf, bcy + halfbj);
                    }
                }
                else
                {
                    var hx = (float)(28 / JwFileConsts.JwScale);
                    createhole(hx, tcy + halfbj);
                    createhole(hx, tcy - halfbj);
                    createhole(hx, ccy + halfbj);
                    createhole(hx, ccy - halfbj);
                    createhole(hx, bcy + halfbj);
                    createhole(hx, bcy - halfbj);
                    if (h.HasBhLinkHole)
                    {
                        double hxf = hx + JwFileConsts.Kongjing / JwFileConsts.JwScale;
                        createhole(hxf, bcy - halfbj);
                        createhole(hxf, bcy + halfbj);
                    }

                }


                //else
                //{
                //    var hx = (float)(locationx);
                //    _createsinglehole(hx, (float)tcy);
                //    _createsinglehole(hx, (float)tcy, true);
                //    _createsinglehole(hx, (float)ccy);
                //    _createsinglehole(hx, (float)ccy, true);
                //    _createsinglehole(hx, (float)bcy);
                //    _createsinglehole(hx, (float)bcy, true);

                //}
            }
            else if (isend)
            {
                double endzb = XXLength;
                if (_beam.EndTelosType == KongzuType.B)
                {
                    if (isbis)
                    {
                        //double endzb = XXLength;
                        var hx = (float)(endzb + halfbj);
                        createhole(hx, tcy + halfbj);
                        createhole(hx, tcy- halfbj);
                        createhole(hx, ccy + halfbj);
                        createhole(hx, ccy - halfbj);
                        createhole(hx, bcy + halfbj);
                        createhole(hx, bcy - halfbj);
                    }
                    else
                    {
                        var hx = (float)(endzb - halfbj);
                        var hyx = (float)(endzb + halfbj);
                        createhole(hx, tcy + halfbj);
                        createhole(hx, tcy - halfbj);
                        createhole(hx, ccy + halfbj);
                        createhole(hx, ccy - halfbj);
                        createhole(hx, bcy + halfbj);
                        createhole(hx, bcy - halfbj);
                        
                        createhole(hyx,tcy + halfbj);
                        createhole(hyx,tcy - halfbj);
                        createhole(hyx,ccy + halfbj);
                        createhole(hyx,ccy - halfbj);
                        createhole(hyx,bcy + halfbj);
                        createhole(hyx,bcy - halfbj);
                        if (h.HasPreLinkHole)
                        {
                            double hxf = hx - JwFileConsts.Kongjing / JwFileConsts.JwScale;
                            createhole(hxf, bcy - halfbj);
                            createhole(hxf, bcy + halfbj);
                        }
                    }
                }
                else if (_beam.EndTelosType == KongzuType.G)
                {
                    var hx = (float)(endzb - 90 / JwFileConsts.JwScale);
                    createhole(hx, tcy + halfbj);
                    createhole(hx, tcy - halfbj);
                    createhole(hx, ccy + halfbj);
                    createhole(hx, ccy - halfbj);
                    createhole(hx, bcy + halfbj);
                    createhole(hx, bcy - halfbj);
                    if (h.HasPreLinkHole)
                    {
                        double hxf = hx - JwFileConsts.Kongjing / JwFileConsts.JwScale;
                        createhole(hxf, bcy - halfbj);
                        createhole(hxf, bcy + halfbj);
                    }
                }
                else
                {
                    var hx = (float)(endzb - 28 / JwFileConsts.JwScale);
                    createhole(hx, tcy + halfbj);
                    createhole(hx, tcy - halfbj);
                    createhole(hx, ccy + halfbj);
                    createhole(hx, ccy - halfbj);
                    createhole(hx, bcy + halfbj);
                    createhole(hx, bcy - halfbj);
                    if (h.HasPreLinkHole)
                    {
                        double hxf = hx - JwFileConsts.Kongjing / JwFileConsts.JwScale;
                        createhole(hxf, bcy - halfbj);
                        createhole(hxf, bcy + halfbj);
                    }
                }
            }
            else
            {
                var chx = (float)(pre - halfbj);
                var chyx = (float)(pre + halfbj);
                if (h.HasTop)
                {
                    createhole(chx, tcy+halfbj);
                    createhole(chx, tcy-halfbj);
                    createhole(chyx,tcy + halfbj);
                    createhole(chyx,tcy - halfbj);
                }
                if (h.HasCenter)
                {
                    createhole(chx, ccy+halfbj);
                    createhole(chx, ccy-halfbj);
                    createhole(chyx, ccy + halfbj);
                    createhole(chyx, ccy - halfbj);
                }
                if (h.HasBottom)
                {
                    createhole(chx, bcy + halfbj);
                    createhole(chx, bcy - halfbj);
                    createhole(chyx, bcy + halfbj);
                    createhole(chyx, bcy - halfbj);
                }
                if (h.HasPreLinkHole)
                {
                    double hxf = chx - JwFileConsts.Kongjing / JwFileConsts.JwScale;
                    double hxf1 = chyx - JwFileConsts.Kongjing / JwFileConsts.JwScale;
                    createhole(hxf, bcy - halfbj);
                    createhole(hxf, bcy + halfbj);
                }
                if (h.HasBhLinkHole)
                {
                    double hxf1 = chyx + JwFileConsts.Kongjing / JwFileConsts.JwScale;
                    createhole(hxf1, bcy - halfbj);
                    createhole(hxf1, bcy + halfbj);
                }
            }
        }


        private void fuzhuxian(double x)
        {
            JwwSen sen1 = new JwwSen();
            sen1.m_nPenStyle = 2;
            sen1.m_nPenColor = 4;
            sen1.m_start_x = x;
            sen1.m_start_y = _jiangeY+2;
            sen1.m_end_x = x;
            sen1.m_end_y = -_jiangeY-2;
            Sens.Add(sen1);
        }


        private void CreateMsgAndHole(double offsetx, double offsety, double msgy, KongzuSuoshuMian mian)
        {
            List<JwHole> hole = new List<JwHole>();
            if (mian == KongzuSuoshuMian.Top)
            {
                hole = _beam.Holes.Where(t => t.HasTop).OrderBy(t => t.Location.X).ToList();
            }
            if (mian == KongzuSuoshuMian.Center)
            {
                hole = _beam.Holes.Where(t => t.HasCenter).OrderBy(t => t.Location.X).ToList();
            }
            if (mian == KongzuSuoshuMian.Bottom)
            {
                hole = _beam.Holes.Where(t => t.HasBottom).OrderBy(t => t.Location.X).ToList();
            }
            var xs = hole.Select(t => t.Location.X + offsetx).ToList();
            var thisbeamcentery = (_beam.TopLeft.Y + _beam.BottomLeft.Y) / 2 + offsety;
            createMsgs(xs, thisbeamcentery, msgy, startx, endx, xstartx, xendx);
            createHoles(hole, offsetx, offsety);
        }

        private void createHoles(List<JwHole> holes, double offsetx, double offsety)
        {
            foreach (var hole in holes)
            {
                hole.createTBLF();
                createhole(hole.TopLeft.X + offsetx, hole.TopLeft.Y + offsety);
                createhole(hole.BottomLeft.X + offsetx, hole.BottomLeft.Y + offsety);
                if (hole.KongNum == 4)
                {
                    createhole(hole.TopRight.X + offsetx, hole.TopRight.Y + offsety);
                    createhole(hole.BottomRight.X + offsetx, hole.BottomRight.Y + offsety);
                }
            }
        }

        private void createhole(double x, double y)
        {
            JwwEnko enko = new JwwEnko();
            enko.m_dHankei = banjing;
            enko.m_radKaishiKaku = 0;
            enko.m_radEnkoKaku = 6.2831853;
            enko.m_radKatamukiKaku = 0;
            enko.m_dHenpeiRitsu = 1;
            enko.m_bZenEnFlg = 1;
            enko.m_start_x = x;
            enko.m_start_y = y;
            Enkos.Add(enko);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xs"></param>
        /// <param name="sy">center y</param>
        /// <param name="msgy">标注高点</param>
        /// <param name="beamleftx"></param>
        /// <param name="beamrightx"></param>
        /// <param name="beamxleftx"></param>
        /// <param name="beamxrightx"></param>
        private void createMsgs(List<double> xs, double sy, double msgy, double beamleftx, double beamrightx, double beamxleftx, double beamxrightx)
        {
            if (xs.Count > 0)
            {
                var blsx = beamleftx;

                var blsxx = beamxleftx;

                createsingleTen(blsxx, msgy);
                foreach (var x in xs)
                {
                    if (blsxx != x)
                    {
                        createsingleTen(x, msgy);//创建点
                        createsinglemsg(blsxx, x, msgy, sy);//创建寸法 标注
                        blsxx = x;
                    }

                }
                if (blsxx != beamxrightx)
                {
                    createsingleTen(beamxrightx, msgy);
                    createsinglemsg(blsxx, beamxrightx, msgy, sy);
                }
            }
        }

        private void showxx()
        {
            JwwSunpou sunpou = new JwwSunpou();
            //sunpou.m_nPenColor = 2;
            //sunpou.m_nPenStyle = 1;
            double sy = 1;
           
            sunpou.m_Sen.m_nPenColor = 5;
            sunpou.m_Sen.m_nPenStyle = 2;
            sunpou.m_Sen.m_start_x = 0;
            sunpou.m_Sen.m_start_y = sy;
            sunpou.m_Sen.m_end_y = sy;
            sunpou.m_Sen.m_end_x = XXLength;

            sunpou.m_Moji.m_string = string.Format("{0}", Math.Round(XXLength * JwFileConsts.JwScale, 0));
            double mjsx = 0;
            
                mjsx = XXLength / 2;
            
            sunpou.m_Moji.m_start_x = mjsx;
            sunpou.m_Moji.m_start_y = sy;
            sunpou.m_Moji.m_dSizeX = 0.5;
            sunpou.m_Moji.m_dSizeY = 0.5;
            Biaozhu.Add(sunpou);
            JwwTen ten = new JwwTen();
            ten.m_start_x = 0;
            ten.m_start_y = sy;
            Tens.Add(ten);
            JwwTen ten1 = new JwwTen();
            ten1.m_start_x = XXLength;
            ten1.m_start_y = sy;
            Tens.Add(ten1);
        }

        private void showbeamlength()
        {
            JwwSunpou sunpou = new JwwSunpou();
            //sunpou.m_nPenColor = 2;
            //sunpou.m_nPenStyle = 1;
            double sy = 1+0.5;

            sunpou.m_Sen.m_nPenColor = 6;
            sunpou.m_Sen.m_nPenStyle = 2;
            sunpou.m_Sen.m_start_x = startx;
            sunpou.m_Sen.m_start_y = sy;
            sunpou.m_Sen.m_end_y = sy;
            sunpou.m_Sen.m_end_x = endx;

            sunpou.m_Moji.m_string = string.Format("{0}", Math.Round(BLength * JwFileConsts.JwScale, 0));
            double mjsx = 0;

            mjsx = BLength / 2;

            sunpou.m_Moji.m_start_x = mjsx;
            sunpou.m_Moji.m_start_y = sy;
            sunpou.m_Moji.m_dSizeX = 0.5;
            sunpou.m_Moji.m_dSizeY = 0.5;
            Biaozhu.Add(sunpou);
            JwwTen ten = new JwwTen();
            ten.m_start_x = startx;
            ten.m_start_y = sy;
            Tens.Add(ten);
            JwwTen ten1 = new JwwTen();
            ten1.m_start_x = endx;
            ten1.m_start_y = sy;
            Tens.Add(ten1);
        }


        private void showprexindistance(double num1 ,double num2,bool isbe=false)
        {
            JwwSunpou sunpou = new JwwSunpou();
            //sunpou.m_nPenColor = 2;
            //sunpou.m_nPenStyle = 1;
            double sy = 0;
            if (isbe)
            {
                sy = _fuzhuY - 0.5;
            }
            else
            {
                sy = _fuzhuY;
            }
            sunpou.m_Sen.m_nPenColor = (short)(isbe ? 4 : 2);
            sunpou.m_Sen.m_nPenStyle = 2;
            sunpou.m_Sen.m_start_x = num1;
            sunpou.m_Sen.m_start_y = sy;
            sunpou.m_Sen.m_end_y = sy;
            sunpou.m_Sen.m_end_x = num2;

            sunpou.m_Moji.m_string = string.Format("{0}", Math.Round(Math.Abs(num2 - num1) * JwFileConsts.JwScale, 0));
            double mjsx = 0;
            var z = Math.Abs(num2 - num1);
            if (z >= 2)
            {
                mjsx = (num2 + num1) / 2;
            }
            else
            {
                mjsx = num1;
            }
            sunpou.m_Moji.m_start_x = mjsx;
            sunpou.m_Moji.m_start_y = sy;
            sunpou.m_Moji.m_dSizeX = 0.5;
            sunpou.m_Moji.m_dSizeY = 0.5;
            Biaozhu.Add(sunpou);
            JwwTen ten = new JwwTen();
            ten.m_start_x = num1;
            ten.m_start_y = sy;
            Tens.Add(ten);
            JwwTen ten1 = new JwwTen();
            ten1.m_start_x = num2;
            ten1.m_start_y = sy;
            Tens.Add(ten1);
        }

        private void cretaeFuzhu(double num1,double num2,double fznum1,double fznum2,bool showx=true)
        {
            if (showx)
            {
                JwwSen sen1 = new JwwSen();
                sen1.m_nPenStyle = 2;
                sen1.m_nPenColor = 4;
                sen1.m_start_x = num1;
                sen1.m_start_y = fznum1;
                sen1.m_end_x = num1;
                sen1.m_end_y = fznum2;
                Sens.Add(sen1);

                JwwSen sen2 = new JwwSen();
                sen1.m_nPenStyle = 2;
                sen1.m_nPenColor = 4;
                sen1.m_start_x = num1;
                sen1.m_start_y = fznum1;
                sen1.m_end_x = num1;
                sen1.m_end_y = fznum2;
                Sens.Add(sen1);

            }
            
        }

        private void createsinglemsg(double x1, double x2, double y, double sy, bool showx = true)
        {
            JwwSen sen1 = new JwwSen();
            sen1.m_nPenStyle = 2;
            sen1.m_nPenColor = 4;
            sen1.m_start_x = x1;
            JwwSunpou sunpou = new JwwSunpou();
            //sunpou.m_nPenColor = 2;
            //sunpou.m_nPenStyle = 1;
            sunpou.m_Sen.m_nPenColor = 2;
            sunpou.m_Sen.m_nPenStyle = 2;
            JwwSen sen2 = new JwwSen();
            sen2.m_nPenStyle = 2;
            sen2.m_nPenColor = 4;
            if (showx)
            {
                sen1.m_end_x = x1;
                sen1.m_end_y = y;
                sen2.m_start_x = x2;
                sen2.m_start_y = sy;
                sunpou.m_Sen.m_end_x = x2;
                sunpou.m_Sen.m_start_y = y;
                sunpou.m_Moji.m_string = (Math.Round((x2 - x1) * JwFileConsts.JwScale, 2)).ToString();
            }
            else
            {
                sen1.m_end_x = x2;
                sen1.m_end_y = sy;
                sen2.m_start_x = x1;
                sen2.m_start_y = y;
                sunpou.m_Sen.m_end_x = x1;
                sunpou.m_Sen.m_start_y = sy;
                sunpou.m_Moji.m_string = (Math.Round((y - sy) * JwFileConsts.JwScale, 2)).ToString();
            }
            sen1.m_start_y = sy;

            this.Sens.Add(sen1);
            sen2.m_end_x = x2;
            sen2.m_end_y = y;
            this.Sens.Add(sen2);

            sunpou.m_Sen.m_end_y = y;

            sunpou.m_Sen.m_start_x = x1;
            sunpou.m_Moji.m_start_x = x1;
            sunpou.m_Moji.m_start_y = y;
            sunpou.m_Moji.m_dSizeX = 0.5;
            sunpou.m_Moji.m_dSizeY = 0.5;
            //sunpou.m_Moji.
            Biaozhu.Add(sunpou);
        }

        private void createsingleTen(double x, double y)
        {
            JwwTen ten = new JwwTen();
            ten.m_start_x = x;
            ten.m_start_y = y;
            Tens.Add(ten);
        }

        private JwwSen CreateSen(double x1, double y1, double x2, double y2, bool isfuzhu = false)
        {
            JwwSen sen = new JwwSen();
            if (isfuzhu)
            {
                sen.m_nPenStyle = 2;
            }
            else
            {
                sen.m_nPenStyle = 1;
            }
            sen.m_nPenColor = 3;
            sen.m_start_x = x1;
            sen.m_end_x = x2;
            sen.m_start_y = y1;
            sen.m_end_y = y2;
            return sen;
        }
    }
}
