using JwCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public class JwVpl
    {

        public JwVpl(JWPoint p,BeamEndPosition bep) 
        {
            Location = p;
            Position = bep;
        }

        public JWPoint Location { get; set; }

        public JWPoint SecondLoaction { get; set; }

        public JWPoint ThirdLocation { get; set; }

        public BeamEndPosition Position { get; set; }

        public JwXian SideLine { get; set; }

        public JwXian TopLine { get; set; }

        public JwXian BottomLine { get; set; }

        public JwXian Slash { get; set; }

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
                        break;

                        JwArc ja = new JwArc(Location, bottomSpacing / JwFileConsts.JwScale, -1.5707963267948966, 2.4035303736600242);
                        var l = ja.ArcFinish;
                        Slash=new JwXian(topother, l);

                    }
                case BeamEndPosition.上左:
                    {
                        break;
                    }

            }
        }
    }

    public class JwArc
    {
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
