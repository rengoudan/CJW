using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    /// <summary>
    /// 参考  RobustLineIntersector  实现判定交点
    /// </summary>
    public class JwLineIntersector
    {

        public JWPoint IntersectionPoint { get; set; }
        /// <summary>
        /// Indicates that line segments do not intersect
        /// </summary>
        public const int NoIntersection = 0;

        /// <summary>
        /// Indicates that line segments intersect in a single point
        /// </summary>
        public const int PointIntersection = 1;

        /// <summary>
        /// Indicates that line segments intersect in a line segment
        /// </summary>
        public const int CollinearIntersection = 2;

        public bool HasIntersection { get; set; }

        public int ComputeIntersect(JwXian x1,JwXian x2)
        {
            return ComputeIntersect(x1.Pone, x1.Ptwo, x2.Pone, x2.Ptwo);
        }

        public int ComputeIntersect(JWPoint p1, JWPoint p2, JWPoint q1, JWPoint q2)
        {
            if (!Intersects(p1, p2, q1, q2))
                return NoIntersection;
            var Pq1 = CGAlgorithmsDD.OrientationIndex(p1.X, p1.Y, p2.X, p2.Y, q1.X, q1.Y);
            var Pq2 = CGAlgorithmsDD.OrientationIndex(p1.X, p1.Y, p2.X, p2.Y, q2.X, q2.Y);
            if ((Pq1 > 0 && Pq2 > 0) ||
               (Pq1 < 0 && Pq2 < 0))
                return NoIntersection;
            var Qp1 = CGAlgorithmsDD.OrientationIndex(q1.X, q1.Y, q2.X, q2.Y, p1.X, p1.Y);
            var Qp2 = CGAlgorithmsDD.OrientationIndex(q1.X, q1.Y, q2.X, q2.Y, p2.X, p2.Y);

            if ((Qp1 > 0 && Qp2 > 0) ||
                (Qp1 < 0 && Qp2 < 0))
                return NoIntersection;

            /*
             * Intersection is collinear if each endpoint lies on the other line.
             * 共线
             */
            bool collinear = Pq1 == 0 && Pq2 == 0 && Qp1 == 0 && Qp2 == 0;


            JWPoint p = null;
            double z = double.NaN;
            if (Pq1 == 0 || Pq2 == 0 || Qp1 == 0 || Qp2 == 0)
            {

                if (p1.Equals2D(q1))
                {
                    p = p1;
                }
                else if (p1.Equals2D(q2))
                {
                    p = p1;
                }
                else if (p2.Equals2D(q1))
                {
                    p = p2;
                }
                else if (p2.Equals2D(q2))
                {
                    p = p2;
                }
                else if (Pq1 == 0)
                {
                    p = q1;
                }
                else if (Pq2 == 0)
                {
                    p = q2;
                }
                else if (Qp1 == 0)
                {
                    p = p1;
                }
                else if (Qp2 == 0)
                {
                    p = p2;
                }
                return NoIntersection;
            }
            else
            {
                HasIntersection = true;
                IntersectionPoint = Intersection(p1, p2, q1, q2);
                //z = zInterpolate(p, p1, p2, q1, q2);
                return PointIntersection;
            }
            
        }

        public JWPoint Intersection(JWPoint p1, JWPoint p2, JWPoint q1, JWPoint q2)
        {
            // compute midpoint of "kernel envelope"
            double minX0 = p1.X < p2.X ? p1.X : p2.X;
            double minY0 = p1.Y < p2.Y ? p1.Y : p2.Y;
            double maxX0 = p1.X > p2.X ? p1.X : p2.X;
            double maxY0 = p1.Y > p2.Y ? p1.Y : p2.Y;

            double minX1 = q1.X < q2.X ? q1.X : q2.X;
            double minY1 = q1.Y < q2.Y ? q1.Y : q2.Y;
            double maxX1 = q1.X > q2.X ? q1.X : q2.X;
            double maxY1 = q1.Y > q2.Y ? q1.Y : q2.Y;

            double intMinX = minX0 > minX1 ? minX0 : minX1;
            double intMaxX = maxX0 < maxX1 ? maxX0 : maxX1;
            double intMinY = minY0 > minY1 ? minY0 : minY1;
            double intMaxY = maxY0 < maxY1 ? maxY0 : maxY1;

            double midx = (intMinX + intMaxX) / 2.0;
            double midy = (intMinY + intMaxY) / 2.0;

            // condition ordinate values by subtracting midpoint
            double p1x = p1.X - midx;
            double p1y = p1.Y - midy;
            double p2x = p2.X - midx;
            double p2y = p2.Y - midy;
            double q1x = q1.X - midx;
            double q1y = q1.Y - midy;
            double q2x = q2.X - midx;
            double q2y = q2.Y - midy;

            // unrolled computation using homogeneous coordinates eqn
            double px = p1y - p2y;
            double py = p2x - p1x;
            double pw = p1x * p2y - p2x * p1y;

            double qx = q1y - q2y;
            double qy = q2x - q1x;
            double qw = q1x * q2y - q2x * q1y;

            double x = py * qw - qy * pw;
            double y = qx * pw - px * qw;
            double w = px * qy - qx * py;

            double xInt = x / w;
            double yInt = y / w;

            // check for parallel lines
            if ((double.IsNaN(xInt)) || (double.IsInfinity(xInt)
                || double.IsNaN(yInt)) || (double.IsInfinity(yInt)))
            {
                return null;
            }
            // de-condition intersection point
            return new JWPoint(xInt + midx, yInt + midy);
        }

        public bool Intersects(JWPoint p1, JWPoint p2, JWPoint q1, JWPoint q2)
        {
            double minP = Math.Min(p1.X, p2.X);
            double maxQ = Math.Max(q1.X, q2.X);
            if (minP > maxQ)
                return false;

            double minQ = Math.Min(q1.X, q2.X);
            double maxP = Math.Max(p1.X, p2.X);
            if (maxP < minQ)
                return false;

            minP = Math.Min(p1.Y, p2.Y);
            maxQ = Math.Max(q1.Y, q2.Y);
            if (minP > maxQ)
                return false;

            minQ = Math.Min(q1.Y, q2.Y);
            maxP = Math.Max(p1.Y, p2.Y);
            if (maxP < minQ)
                return false;

            return true;
        }
    }
}
