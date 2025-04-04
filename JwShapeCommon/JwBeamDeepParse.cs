using JwCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwBeamDeepParse
    {

        public List<JwBeam> lst { get; set; }
        /// <summary>
        /// 水平组
        /// </summary>
        public List<JwBeam> HorizontalBeams { get; set; }

        /// <summary>
        /// 垂直组
        /// </summary>
        public List<JwBeam> VerticalBeams { get; set; }

        public List<double> RowsPointY { get; set; }

        public List<double> ColumnPointX { get; set; }

        public List<JWPoint> CenterPoints { get; set; }

        public JwBeamDeepParse(List<JwBeam> beams)
        {
            lst = beams;
            PareBeamByMian();
        }

        public void PareBeamByMian()
        {
            HorizontalBeams = lst.Where(t => t.DirectionType == BeamDirectionType.Horizontal).ToList();
            VerticalBeams = lst.Where(t => t.DirectionType == BeamDirectionType.Vertical).ToList();
            RowsPointY = HorizontalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
            ColumnPointX = VerticalBeams.Select(t => t.Center).OrderBy(t => t).ToList();
            CenterPoints = lst.Select(t => t.CenterPoint).OrderBy(t => t.X).ThenBy(t => t.Y).ToList();
            List<IGrouping<double, JwBeam>> shuipinggroup = HorizontalBeams.GroupBy(t => t.Center).OrderByDescending(t=>t.Key).ToList();

            foreach(var q in shuipinggroup)
            {
                var groupbottomy = q.Max(t => t.BottomLeft.Y);
                var grouptopy = q.Max(t => t.TopLeft.Y);
                var chuizhishang = VerticalBeams.Where(t => t.BottomLeft.Y > q.Key && t.BottomLeft.Y < (grouptopy + 60)).ToList();
                //double shuiymax = VerticalBeams.Select(t => t.BottomLeft.Y).Max();
                if (chuizhishang.Count>0)//该组需要往上找，
                {
                    var lx = chuizhishang.Min(t => t.BottomLeft.X);
                    var f=q.Where(t=>t.TopRight.X>lx).ToList();
                    foreach(var l in f)
                    {
                        foreach (var c in chuizhishang)
                        {
                            if (c.Center > l.TopLeft.X && c.Center < l.TopRight.X)
                            {
                                JwLinkPart jbb = new JwLinkPart();
                                jbb.BujianName = "BG";
                                jbb.BjCenterPoint = new JWPoint
                                {
                                    X = c.Center,
                                    Y = l.Center
                                };
                                jbb.ParentBeam = l;
                                jbb.BeamId = l.Id;
                                jbb.BBeam = c;
                                l.LinkParts.Add(jbb);
                            }
                        }
                    }
                }
                var chuizhixia = VerticalBeams.Where(t => t.TopLeft.Y < q.Key && t.TopLeft.Y > (groupbottomy - 60)).ToList();
                if(chuizhixia.Count>0)
                {
                    var lx = chuizhixia.Min(t => t.TopLeft.X);
                    //进一步缩小范围 
                    var f = q.Where(t => t.TopRight.X > lx).ToList();
                    foreach(var l in f)
                    {
                        foreach(var c in chuizhixia)
                        {
                            if (c.Center > l.TopLeft.X && c.Center < l.TopRight.X)
                            {
                                JwLinkPart jbb = new JwLinkPart();
                                jbb.BujianName = "BG";
                                jbb.BjCenterPoint = new JWPoint
                                {
                                    X = c.Center, 
                                    Y = l.Center
                                };
                                jbb.ParentBeam = l;
                                jbb.BeamId = l.Id;
                                jbb.BBeam = c;
                                l.LinkParts.Add(jbb);
                            }
                        }
                    }
                }
                //else
                //{
                //    //该组不用寻找
                //}
                //往下早 
               
            }

            List<IGrouping<double, JwBeam>> chuizhigroup = VerticalBeams.GroupBy(t => t.Center).OrderBy(t => t.Key).ToList();

            foreach(var q in chuizhigroup)
            {
                var groupleft = q.Min(t => t.BottomLeft.X);
                var grouprightx = q.Max(t => t.BottomRight.X);
                var shuipingleft = HorizontalBeams.Where(t => t.TopRight.X < groupleft && t.TopRight.X > (groupleft - 60)).ToList();
                if(shuipingleft.Count > 0)
                {
                    var ly = shuipingleft.Max(t => t.TopLeft.Y);

                    var f=q.Where(t=>t.BottomLeft.Y<ly).ToList();
                    foreach(var l in f)
                    {
                        foreach(var r in shuipingleft)
                        {
                            if (r.Center > l.BottomLeft.Y && r.Center < l.TopLeft.Y)
                            {
                                JwLinkPart jbb = new JwLinkPart();
                                jbb.BujianName = "BG";
                                jbb.BjCenterPoint = new JWPoint
                                {
                                    X = l.Center,
                                    Y =r.Center
                                };
                                jbb.ParentBeam = l;
                                jbb.BeamId = l.Id;
                                jbb.BBeam = r;
                                l.LinkParts.Add(jbb);
                            }
                        }
                    }
                }
                var shuipingright = HorizontalBeams.Where(t => t.TopLeft.X > grouprightx && t.TopLeft.X < (grouprightx + 70)).ToList();
                if(shuipingright.Count > 0)
                {
                    var ly = shuipingright.Max(t => t.TopLeft.Y);
                    var f = q.Where(t => t.BottomLeft.Y <ly).ToList();
                    foreach (var l in f)
                    {
                        foreach (var r in shuipingright)
                        {
                            if (r.Center > l.BottomLeft.Y && r.Center < l.TopLeft.Y)
                            {
                                JwLinkPart jbb = new JwLinkPart();
                                jbb.BujianName = "BG";
                                jbb.BjCenterPoint = new JWPoint
                                {
                                    X = l.Center,
                                    Y = r.Center
                                };
                                jbb.ParentBeam = l;
                                jbb.BeamId = l.Id;
                                jbb.BBeam = r;
                                l.LinkParts.Add(jbb);
                            }
                        }
                    }
                }
            }
        }
    }
}
