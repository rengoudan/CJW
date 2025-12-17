using JwCore;
using JwwHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public abstract class JwSquareBase
    {
        public string Id { get; set; }

        #region 矩形才有
        public JwXian? TopLine { get; set; }

        public JwXian? BottomLine { get; set; }

        public JwXian? LeftLine { get; set; }

        public JwXian? RightLine { get; set; }

        /// <summary>
        /// 左上
        /// </summary>
        public JWPoint? TopLeft { get; set; }

        /// <summary>
        /// 右上
        /// </summary>
        public JWPoint? TopRight { get; set; }

        /// <summary>
        /// 左下
        /// </summary>
        public JWPoint? BottomLeft { get; set; }

        /// <summary>
        /// 右下
        /// </summary>
        public JWPoint? BottomRight { get; set; }

        public List<JwXian> BlockLines { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        /// <summary>
        /// jw图纸坐标差 乘 jw设计绘制里标注的 比例 默认100
        /// 
        /// </summary>
        public double WidthScale { get; set; }

        public double HeightScale { get; set; }

        /// <summary>
        /// 水平的为Y 垂直的为X
        /// </summary>
        public double Center { get; set; }

        

        public JWPoint CenterPoint { get; set; }

        /// <summary>
        /// 约定小于零右上，大于零右下
        /// </summary>
        public double Jiaodu { get; set; }

        /// <summary>
        /// 实现 style 1  虚线2
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public JwwSen CreateSenByTwoPoint(JWPoint p1, JWPoint p2)
        {
            var sen = new JwwSen();
            //sen.m_nPenWidth=1/
            sen.m_nPenColor = 2;
            sen.m_start_x = p1.X;
            sen.m_start_y = p1.Y;
            sen.m_end_x = Math.Round(p2.X, 6);
            sen.m_end_y = Math.Round(p2.Y, 6);
            sen.m_nPenColor = 5;
            sen.m_nPenStyle = 1;
            sen.m_nPenWidth = 0;
            return sen;
        }


        public BeamDirectionType DirectionType { get; set; }

        public void JisuanWidthHeight()
        {
            Width = TopRight.X - TopLeft.X;
            Height = TopLeft.Y - BottomLeft.Y;

            if (Width >= Height)
            {
                DirectionType = BeamDirectionType.Horizontal;
                Center = (TopLeft.Y + BottomLeft.Y) / 2;
            }
            else
            {
                DirectionType = BeamDirectionType.Vertical;
                Center = (TopLeft.X + TopRight.X) / 2;
            }
            CenterPoint = new JWPoint();
            CenterPoint.X = Math.Round((TopLeft.X + TopRight.X) / 2.0, 6);
            CenterPoint.Y = Math.Round((TopLeft.Y + BottomLeft.Y) / 2.0, 6);
        }

        // 计算两点之间的距离
        private double Distance(JWPoint p1, JWPoint p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public bool Contains(double x, double y) => TopLeft.X <= x && x <= TopLeft.X + Width && TopLeft.Y >= y && y >= TopLeft.Y - Height;

        public bool Contains(JWPoint pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// 可以扩大范围
        /// 2025年6月21日
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool ContainShenglue(JWPoint pt)
        {
            //return Contains(Math.Round(pt.X, 2), Math.Round(pt.Y, 2));

            var slx = Math.Round(pt.X, 2);
            var sly = Math.Round(pt.Y, 2);
            double wc = 0.05;
            return TopLeft.X-wc <= slx && slx <= TopLeft.X + Width+wc && TopLeft.Y+wc >= sly && sly >= TopLeft.Y - Height-wc;


        }

        public bool WuchaContains(double x, double y) => TopLeft.X- JwFileConsts.NearSpliteMax / JwFileConsts.JwScale <= x && x <= TopLeft.X + Width+ JwFileConsts.NearSpliteMax / JwFileConsts.JwScale && TopLeft.Y+ JwFileConsts.NearSpliteMax / JwFileConsts.JwScale >= y && y >= TopLeft.Y - Height- JwFileConsts.NearSpliteMax / JwFileConsts.JwScale;

        public bool WuchaContains(JWPoint pt) => WuchaContains(pt.X, pt.Y);

        public bool NearContains(JWPoint pt) => NearContains(pt.X,pt.Y);

        public bool NearContains(double x, double y)
        {
            //if (DirectionType == BeamDirectionType.Horizontal)
            //{
            //    return (TopLeft.Y + JwFileConsts.NearSpliteMax / JwFileConsts.JwScale >= y && y >= TopLeft.Y - JwFileConsts.NearSpliteMax / JwFileConsts.JwScale) || (BottomLeft.Y + JwFileConsts.NearSpliteMax / JwFileConsts.JwScale >= y && y >= BottomLeft.Y - JwFileConsts.NearSpliteMax / JwFileConsts.JwScale);
            //}
            //else
            //{
            //    return (TopLeft.X + JwFileConsts.NearSpliteMax / JwFileConsts.JwScale >= x && x >= TopLeft.X - JwFileConsts.NearSpliteMax / JwFileConsts.JwScale) || (TopRight.X + JwFileConsts.NearSpliteMax / JwFileConsts.JwScale >= x && x >= TopRight.X - JwFileConsts.NearSpliteMax / JwFileConsts.JwScale);
            //}
            return TopLeft.X - JwFileConsts.NearSpliteMax / JwFileConsts.JwScale <= x && x <= TopRight.X + JwFileConsts.NearSpliteMax / JwFileConsts.JwScale && TopLeft.Y + JwFileConsts.NearSpliteMax / JwFileConsts.JwScale >= y && y >= BottomLeft.Y - JwFileConsts.NearSpliteMax / JwFileConsts.JwScale;
            //return true;
        }


        public bool ContainsDirected(JwDirected directed)
        {
            bool re = true;

            foreach (var p in directed.Points)
            {
                re = re && Contains(p);
            }

            return re;
        }

        public bool JieChuDirected(JwDirected directed) 
        {
            if (directed.IsDirected)
            {
                if (directed.TaggDirect == TaggDirect.Left)
                {
                    return (TopLeft.Y>directed.QieGeZhi&& directed.QieGeZhi > BottomLeft.Y)&&(TopRight.X - JwFileConsts.NearSpliteMax / JwFileConsts.JwScale <= directed.DirectPoint.X && directed.DirectPoint.X <= TopRight.X + JwFileConsts.NearSpliteMax / JwFileConsts.JwScale);
                }
                if (directed.TaggDirect == TaggDirect.Right)
                {
                    return (TopLeft.Y > directed.QieGeZhi && directed.QieGeZhi > BottomLeft.Y)&&(TopLeft.X - JwFileConsts.NearSpliteMax / JwFileConsts.JwScale <= directed.DirectPoint.X && directed.DirectPoint.X <= TopLeft.X + JwFileConsts.NearSpliteMax / JwFileConsts.JwScale);
                }
                if (directed.TaggDirect == TaggDirect.Up)
                {
                    return (TopLeft.X < directed.QieGeZhi && directed.QieGeZhi < TopRight.X) &&( BottomLeft.Y - JwFileConsts.NearSpliteMax / JwFileConsts.JwScale <= directed.DirectPoint.Y && directed.DirectPoint.Y <= BottomLeft.Y + JwFileConsts.NearSpliteMax / JwFileConsts.JwScale);
                }
                if (directed.TaggDirect == TaggDirect.Down)
                {
                    return (TopLeft.X < directed.QieGeZhi && directed.QieGeZhi < TopRight.X) &&(TopLeft.Y - JwFileConsts.NearSpliteMax / JwFileConsts.JwScale <= directed.DirectPoint.Y && directed.DirectPoint.Y <= TopLeft.Y + JwFileConsts.NearSpliteMax / JwFileConsts.JwScale);
                }
                return false;
            }
            else
            {
                return false;
            }
        }


        public bool ContainsAnyPoint(List<JWPoint> points)
        {
            bool r = false;
            foreach(JWPoint point in points)
            {
                if (Contains(point))
                {
                    r=true; 
                    break;
                }
            }
            return r;
        }

        #endregion
    }
}
