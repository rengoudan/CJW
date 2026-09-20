using JwCore;
using JwwHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public class JwVpl:IDrawToJww
    {

        public JwVpl(JWPoint p,BeamEndPosition bep) 
        {
            Location = p;
            Position = bep;
            createOther();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 链接点 即三个圆孔中的基准孔中心
        /// </summary>
        public JWPoint Location { get; set; }

        /// <summary>
        /// 相对于Location的第二个孔位置
        /// </summary>
        public JWPoint SecondLoaction { get; set; }

        /// <summary>
        /// 第三个孔位置一般为败方G 打孔位置
        /// </summary>
        public JWPoint ThirdLocation { get; set; }

        public BeamEndPosition Position { get; set; }

        /// <summary>
        /// 部件侧边
        /// </summary>
        public JwXian SideLine { get; set; }

        /// <summary>
        /// 部件顶边
        /// </summary>
        public JwXian TopLine { get; set; }

        /// <summary>
        /// 部件底边
        /// </summary>
        public JwXian BottomLine { get; set; }

        /// <summary>
        /// 部件倾斜轮廓线
        /// </summary>
        public JwXian Slash { get; set; }

        public JwArc Arc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private const double bottomSpacing= 40;

        private const double secondCenterSpacing = 68;

        private const double sideLength = 131;

        private const double longestsideLength = 95;

        private const double shortsideLength = 50;

        private const double topSpacing = 35;
        private const double jpSpacing = 45;

        private void createOther()
        {
            switch (Position)
            {
                case BeamEndPosition.上右:
                    {
                        var cx = Location.X;
                        var cy = Location.Y;
                        var sly = cy;
                        var slx=cx-(secondCenterSpacing/JwFileConsts.JwScale); 
                        SecondLoaction=new JWPoint(slx,sly);
                        var by = cy - (bottomSpacing / JwFileConsts.JwScale);
                        var bx = cx - (longestsideLength / JwFileConsts.JwScale);
                        var sidebottom =new JWPoint(bx, by);
                        var ul = new JWPoint(cx, by);
                        BottomLine = new JwXian(sidebottom, ul);
                        var tpsy = cy + ((sideLength - bottomSpacing) / JwFileConsts.JwScale);
                        var topside = new JWPoint(bx, tpsy);
                        SideLine = new JwXian(sidebottom, topside);
                        var trx=cx-(jpSpacing/JwFileConsts.JwScale);
                        var topother = new JWPoint(trx, tpsy);
                        TopLine = new JwXian(topother, topside);
                        var tlx=cy+JwFileConsts.EllipseSpacing/JwFileConsts.JwScale;
                        ThirdLocation=new JWPoint(slx, tlx);
                        JwArc ja = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, -1.5707963267948966, 2.4035303736600242);
                        var l = ja.ArcFinish;
                        this.Arc = ja;  
                        Slash=new JwXian(topother, l);
                        break;
                    }
                case BeamEndPosition.上左:
                    {
                        break;
                    }

            }
        }

        public List<JwwData> DrawToJww()
        {
            List<JwwData> jwws = new List<JwwData>();
            return jwws;
        }

        /// <summary>
        /// 用来实现绘制到屏幕 需要增加缩放和便宜旋转等操作
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        public void Draw(Graphics g, Pen pen)
        {
            using var geoPath = BuildPath();
            //using var screenPath = (GraphicsPath)geoPath.Clone();

            //using var m = new Matrix();
            //m.Scale(Scale, -Scale);
            //m.Translate(Offset.X, Offset.Y, MatrixOrder.Append);

            //screenPath.Transform(m);

            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.DrawPath(pen, screenPath);
        }

        /// <summary>
        /// 根据缩放直接存放到path中 便于绘制到屏幕上
        /// </summary>
        /// <param name="zoom">缩放比例</param>
        /// <param name="axisx"></param>
        /// <param name="axisy"></param>
        /// <returns></returns>
        private GraphicsPath BuildPath(double zoom, double axisx, double axisy)
        {
            var path = new GraphicsPath();
            //绘制三根线
            path.AddLine(SideLine.Pone.ToChangeCoordinate(zoom,axisx, axisy), SideLine.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            path.AddLine(TopLine.Pone.ToChangeCoordinate(zoom, axisx, axisy), TopLine.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            path.AddLine(BottomLine.Pone.ToChangeCoordinate(zoom, axisx, axisy), BottomLine.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            path.AddLine(Slash.Pone.ToChangeCoordinate(zoom, axisx, axisy), Slash.Ptwo.ToChangeCoordinate(zoom, axisx, axisy));
            var zoomradius= Arc.Radius * zoom;
            var arcnewcenterx= Arc.Center.X * zoom + axisx;
            var arcnewcentery = axisy - Arc.Center.Y * zoom;

            path.AddArc(
            (float)(arcnewcenterx - zoomradius), (float)(arcnewcentery - zoomradius),
            (float)zoomradius * 2, (float)
            zoomradius * 2,
            (float)Arc.StartAngle,
            (float)Arc.SweepAngle
        );
            return path;
        }


        private void DrawCircles(Graphics g, Pen pen, double zoom,double ax,double ay)
        {
            var pc = SecondLoaction.ToChangeCoordinate(zoom, ax, ay);

            var holeradius=JwFileConsts.EllipseDiameter/JwFileConsts.JwScale*zoom;
            var newradius = holeradius * zoom;
            var rect = new RectangleF(
                pc.X - (float)newradius,
                pc.Y - (float)newradius,
                (float)newradius * 2,
                (float)newradius * 2
            );

            g.DrawEllipse(pen, rect);
        }

    }



    public class JwArc
    {
        public string Id { get; set; }
        public JWPoint Center { get; }
        public double Radius { get; }


        /// <summary>
        /// radkatamukikaku 起始度数 0 与x轴正方向相同
        /// -1.5707963267948966 
        /// </summary>
        public double StartAngle { get; }

        /// <summary>
        /// radenkokaku 旋转角度 2.4035303736600242 固定正为逆时针
        /// </summary>
        public double SweepAngle { get; }

        public JWPoint ArcFinish { get; set; }

        public JwArc(JWPoint center, double radius, double startAngle, double endAngle)
        {
            Id=Guid.NewGuid().ToString();
            Center = center;
            Radius = radius;
            StartAngle = startAngle;
            SweepAngle = endAngle;
            ArcFinish=JwExtend.GetArcEndPoint(center,radius, startAngle, endAngle);
        }

        public JwArc(JWPoint center, float radius, PointF start, PointF end, float verticalX)
        {
            Center = center;
            Radius = radius;

            StartAngle = AngleFromPoints(center, start);
            double endAngle = AngleFromPoints(center, end);

            double sweep = endAngle - StartAngle;

            if ((verticalX > center.X && sweep < 0) ||
                (verticalX < center.X && sweep > 0))
            {
                sweep = -sweep;
            }

            SweepAngle = sweep;
        }

        double AngleFromPoints(JWPoint center, PointF pt)
        {
            return Math.Atan2(pt.Y - center.Y, pt.X - center.X) * 180.0 / Math.PI;
        }
    }

}
