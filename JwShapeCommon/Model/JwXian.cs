using JwCore;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{

    public class JwXian
    {

        public bool IsMianNeixian { get; set; }

        public JWPoint Pone { get; set; }
        public string Ponestr { get; set; }
        public JWPoint Ptwo { get; set; }
        public string Ptwostr { get; set; }
        public bool CreateSuccess = false;

        public string Id { get; set; }

        public decimal Length { get; set; }

        public BeamDirectionType DirectionType { get; set; }

        public bool IsValid { get; set; }

        public JwXian()
        {
            IsValid = false;
        }

        public JwXian(string jwstr)
        {
            var q = jwstr.Split(" ");
            if (q.Length == 4)
            {
                this.Pone = new JWPoint(q[0], q[1]);
                this.Ptwo = new JWPoint(q[2], q[3]);
                this.CreateSuccess = true;
                this.Ponestr = Pone.GetPointString();
                this.Ptwostr = Ptwo.GetPointString();
                this.Length = Convert.ToDecimal(JwXian.distance(this.Pone, this.Ptwo));
                if(Pone.X==Ptwo.X)
                {
                    DirectionType = BeamDirectionType.Vertical;
                }
                else if(Pone.Y==Ptwo.Y)
                {
                    DirectionType = BeamDirectionType.Horizontal;
                }
                else
                {
                    double angle=Math.Atan2(Pone.Y-Ptwo.Y,Pone.X-Ptwo.X);
                    double thela=angle*180/Math.PI;
                    if(thela>90)
                    {
                        DirectionType = BeamDirectionType.YouXia;
                    }
                    else
                    {
                        DirectionType = BeamDirectionType.Youshang;
                    }
                }
            }
            Id = Guid.NewGuid().ToString();
            IsValid= true;
        }

        public JwXian(JWPoint pone, JWPoint ptwo)
        {
            Pone = pone;
            Ponestr = pone.GetPointString();
            Ptwo = ptwo;
            Ptwostr = ptwo.GetPointString();
            this.Length = Convert.ToDecimal(JwXian.distance(this.Pone, this.Ptwo));
            if (Pone.X == Ptwo.X)
            {
                DirectionType = BeamDirectionType.Vertical;
            }
            else if (Pone.Y == Ptwo.Y)
            {
                DirectionType = BeamDirectionType.Horizontal;
            }
            else
            {
                double angle = Math.Atan2(Pone.Y - Ptwo.Y, Pone.X - Ptwo.X);
                double thela = angle * 180 / Math.PI;
                if (thela > 90)
                {
                    DirectionType = BeamDirectionType.YouXia;
                }
                else
                {
                    DirectionType = BeamDirectionType.Youshang;
                }
            }
            Id = Guid.NewGuid().ToString();
            IsValid = true;
        }

        public List<JWPoint> GetXianPoints()
        {
            List<JWPoint> q = new List<JWPoint>
            {
                Pone,
                Ptwo
            };
            return q;
        }

        public bool Isxiangjiao(JwXian other)
        {
            if (other.Pone == this.Pone || other.Ptwo == this.Ptwo || other.Ptwo == this.Pone || other.Pone == this.Ptwo)
            {
                return true;
            }
            else
            {
                return false;
            }
            // var z=other.GetXianPoints();
            //var q = this.GetXianPoints();
            //var xj=z.Intersect(q).ToList();
            //if (xj.Count() > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public bool IsLianjie(JwXian other, out JWPoint jiaodian,out JWPoint bjdian)
        {
            if ((this.Pone == other.Pone&&this.Ptwo==other.Ptwo)|| (this.Pone == other.Ptwo && this.Ptwo == other.Pone))
            {
                jiaodian = new JWPoint();
                bjdian = new JWPoint();
                return false;
            }
            if(this.Pone == other.Pone)
            {
                jiaodian = this.Pone;
                bjdian = other.Ptwo;
                return true;
            }
            if(this.Pone == other.Ptwo)
            {
                jiaodian = this.Pone;
                bjdian = other.Pone;
                return true;
            }
            if (this.Ptwo == other.Ptwo)
            {
                jiaodian = this.Ptwo;
                bjdian = other.Pone;
                return true;
            }
            if(this.Ptwo == other.Pone)
            {
                jiaodian = this.Ptwo;
                bjdian = other.Ptwo;
                return true;

            }
            jiaodian = new JWPoint();
            bjdian = new JWPoint(); 
            return false;
        }


        /// <summary>
        /// 前提业务约束，水平自左向右 垂直自上到下
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsChongdie(JwXian other)
        {
            if(this.DirectionType==other.DirectionType)
            {
                if(this.DirectionType== BeamDirectionType.Horizontal)
                {
                    if(other.Pone.Y!=Pone.Y)
                    {
                        return false;
                    }
                    else
                    {
                        if ((other.Pone.X >= Pone.X && other.Pone.X <= Ptwo.X) || ((other.Ptwo.X >= Pone.X && other.Ptwo.X <= Ptwo.X)))
                        {
                            return true;
                        }
                        else if ((other.Pone.X <= Pone.X &&other.Ptwo.X >= Ptwo.X))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (other.Pone.X != Pone.X)
                    {
                        return false;
                    }
                    else
                    {
                        if ((other.Pone.Y <= Pone.Y && other.Pone.Y >= Ptwo.Y) || ((other.Ptwo.Y <= Pone.Y && other.Ptwo.Y >= Ptwo.Y)))
                        {
                            return true;
                        }
                        else if (other.Pone.Y >= Pone.Y &&other.Ptwo.Y <= Ptwo.Y )
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 接触但不能重叠  
        /// 为了避免两个相邻的柱 识别错误
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsJiechuNotChongdie(JwXian other)
        {
            if (this.DirectionType == other.DirectionType)
            {
                if (this.DirectionType == BeamDirectionType.Horizontal)
                {
                    if (other.Pone.Y != Pone.Y)
                    {
                        return false;
                    }
                    else
                    {
                        if ((other.Pone.X > Pone.X && other.Pone.X < Ptwo.X) || ((other.Ptwo.X > Pone.X && other.Ptwo.X < Ptwo.X)))
                        {
                            return true;
                        }
                        else if ((other.Pone.X < Pone.X && other.Ptwo.X > Ptwo.X))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (other.Pone.X != Pone.X)
                    {
                        return false;
                    }
                    else
                    {
                        if ((other.Pone.Y < Pone.Y && other.Pone.Y > Ptwo.Y) || ((other.Ptwo.Y < Pone.Y && other.Ptwo.Y > Ptwo.Y)))
                        {
                            return true;
                        }
                        else if (other.Pone.Y > Pone.Y && other.Ptwo.Y < Ptwo.Y)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsJiechuNotJiechu(JwXian other)
        {
            if (this.DirectionType == other.DirectionType)
            {
                if (this.DirectionType == BeamDirectionType.Horizontal)
                {

                    if (other.Pone.Y != Pone.Y)
                    {
                        return false;
                    }
                    else
                    {
                        if ((other.Pone.X < this.Ptwo.X)&&(this.Pone.X < other.Ptwo.X))
                        {
                            if(this.Pone.X==other.Pone.X&&this.Ptwo.X==other.Ptwo.X)
                            { 
                                return false; 
                            }
                            return true;
                        }
                        return false;
                    }
                }
                else
                {
                    if (other.Pone.X != Pone.X)
                    {
                        return false;
                    }
                    else
                    {
                        if ((other.Pone.Y < this.Ptwo.Y) && (this.Pone.Y     < other.Ptwo.X))
                        {
                            if (this.Pone.Y == other.Pone.Y && this.Ptwo.Y == other.Ptwo.Y)
                            {
                                return false;
                            }
                            return true;
                        }
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }



        public bool IsSelected = false;

        public static double distance(JWPoint p1, JWPoint p2)
        {
            double r = 0;
            r = Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            return r;
        }


        public double MaxX()
        {
            return Math.Max(Pone.X, Ptwo.X);
        }

        public double MinX()
        {
            return Math.Min(Pone.X, Ptwo.X);
        }

        public double MaxY()
        {
            return Math.Max(Pone.Y, Ptwo.Y);
        }

        public double MinY()
        {
            return Math.Min(Pone.Y,Ptwo.Y);
        }


        /// <summary>
        /// 线判定不区分起始点 点集合排除cout为0
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(JwXian left, JwXian right) => left.GetXianPoints().Except(right.GetXianPoints(),new JwPointComparint()).Count()==0;

        public static bool operator !=(JwXian left, JwXian right) => !(left == right);

        public string GetXianString()
        {
            return "z";  
        }

        public bool IsMerge { get; set; }

        public void ZhongxinXiangjiao(JwXian jwXian)
        {
            //CGAlgorithmsDD.
        }

        // 将线段转换为 NetTopologySuite 的 LineString
        public LineString ToLineString()
        {
            var coordinates = new[] {
            new Coordinate(Pone.X, Pone.Y),
            new Coordinate(Ptwo.X, Ptwo.Y)
        };
            return new LineString(coordinates);
        }

        public bool IntersectsWith(JwXian other)
        {
            double x1 = Pone.X, y1 = Pone.Y;
            double x2 = Ptwo.X, y2 = Ptwo.Y;
            double x3 = other.Pone.X, y3 = other.Pone.Y;
            double x4 = other.Ptwo.X, y4 = other.Ptwo.Y;

            // 计算叉积
            double cross1 = CrossProduct(x3 - x1, y3 - y1, x2 - x1, y2 - y1);
            double cross2 = CrossProduct(x4 - x1, y4 - y1, x2 - x1, y2 - y1);
            double cross3 = CrossProduct(x1 - x3, y1 - y3, x4 - x3, y4 - y3);
            double cross4 = CrossProduct(x2 - x3, y2 - y3, x4 - x3, y4 - y3);

            // 判断是否相交
            if (Math.Sign(cross1) != Math.Sign(cross2) && Math.Sign(cross3) != Math.Sign(cross4))
                return true;

            // 检查是否共线且有重叠
            if (cross1 == 0 && IsPointOnSegment(x3, y3, x1, y1, x2, y2))
                return true;
            if (cross2 == 0 && IsPointOnSegment(x4, y4, x1, y1, x2, y2))
                return true;
            if (cross3 == 0 && IsPointOnSegment(x1, y1, x3, y3, x4, y4))
                return true;
            if (cross4 == 0 && IsPointOnSegment(x2, y2, x3, y3, x4, y4))
                return true;

            return false;
        }

        // 计算叉积
        private double CrossProduct(double ax, double ay, double bx, double by)
        {
            return ax * by - ay * bx;
        }

        // 判断点是否在线段上
        private bool IsPointOnSegment(double px, double py, double x1, double y1, double x2, double y2)
        {
            double cross = CrossProduct(px - x1, py - y1, x2 - x1, y2 - y1);
            if (Math.Abs(cross) > 1e-10) // 不共线
                return false;

            double minX = Math.Min(x1, x2);
            double maxX = Math.Max(x1, x2);
            double minY = Math.Min(y1, y2);
            double maxY = Math.Max(y1, y2);

            return (px >= minX && px <= maxX && py >= minY && py <= maxY);
        }


        public double Jiaodu()
        {
            double slope = (Pone.Y - Ptwo.Y) / (Pone.X - Ptwo.X);

            // 计算相对于X轴的角度（以度数表示）
            double angleInRadians = Math.Atan(slope);
            double angleInDegrees = angleInRadians * (180 / Math.PI);
            return angleInDegrees;
        }

        public double Distance(){
            return Distance(Pone, Ptwo);
        }

        // 计算两点之间的距离
        private double Distance(JWPoint p1, JWPoint p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        // 判断两条线段是否重叠
        public bool OverlapsWith(JwXian other)
        {
            // 判断是否共线
            if (!AreCollinear(this, other))
                return false;

            // 判断投影是否有重叠
            return ProjectionOverlaps(this, other);
        }

        // 判断两条线段是否共线
        private bool AreCollinear(JwXian seg1, JwXian seg2)
        {
            double cross = CrossProduct(
                seg1.Ptwo.X - seg1.Pone.X, seg1.Ptwo.Y - seg1.Pone.Y,
                seg2.Ptwo.X - seg2.Pone.X, seg2.Ptwo.Y - seg2.Pone.Y
            );
            return Math.Abs(cross) < 1e-10; // 叉积为0表示共线
        }

        // 判断两条线段的投影是否有重叠
        private bool ProjectionOverlaps(JwXian seg1, JwXian seg2)
        {
            // 投影到x轴
            double seg1MinX = Math.Min(seg1.Pone.X, seg1.Ptwo.X);
            double seg1MaxX = Math.Max(seg1.Pone.X, seg1.Ptwo.X);
            double seg2MinX = Math.Min(seg2.Pone.X, seg2.Ptwo.X);
            double seg2MaxX = Math.Max(seg2.Pone.X, seg2.Ptwo.X);

            bool xOverlap = seg1MaxX >= seg2MinX && seg2MaxX >= seg1MinX;

            // 投影到y轴
            double seg1MinY = Math.Min(seg1.Pone.Y, seg1.Ptwo.Y);
            double seg1MaxY = Math.Max(seg1.Pone.Y, seg1.Ptwo.Y);
            double seg2MinY = Math.Min(seg2.Pone.Y, seg2.Ptwo.Y);
            double seg2MaxY = Math.Max(seg2.Pone.Y, seg2.Ptwo.Y);

            bool yOverlap = seg1MaxY >= seg2MinY && seg2MaxY >= seg1MinY;

            return xOverlap && yOverlap;
        }

        public JwXian Merge(JwXian other)
        {
            if (!this.OverlapsWith(other))
                throw new InvalidOperationException("Segments do not overlap.");

            // 找到所有端点中最远的两个点
            JWPoint[] points = { this.Pone, this.Ptwo, other.Pone, other.Ptwo };
            double maxDistance = 0;
            JWPoint p1 = null, p2 = null;

            for (int i = 0; i < points.Length; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    double distance = Distance(points[i], points[j]);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        p1 = points[i];
                        p2 = points[j];
                    }
                }
            }

            return new JwXian(p1, p2);
        }

    }
    public class JwXianComparint : IEqualityComparer<JwXian>
    {
        public bool Equals(JwXian? x, JwXian? y)
        {
            if (object.ReferenceEquals(x, null))
            {
                return false;
            }
            if (object.ReferenceEquals(y, null))
            {
                return false;
            }
            else
            {
                return x==y;
            }
        }

        public int GetHashCode(JwXian obj)
        {
            return obj.GetXianString().GetHashCode();
        }

    }

    public class OverlappingXian
    {
        public bool IsOverLapping { get; set; }

        public JwXian MergeLine { get; set; }
    }
}
