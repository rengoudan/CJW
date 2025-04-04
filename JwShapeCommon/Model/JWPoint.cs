using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace JwShapeCommon
{
    public class JWPoint : IEquatable<JWPoint>, ICanZoom, IChangeAxis
    {

        public double X { get; set; }
        public double Y { get; set; }

        public bool IsValid { get; set; }

        public JWPoint()
        {
            IsValid = false;
        }

        public JWPoint(double x, double y)
        {
            if (JwFileConsts.PickPrecision == 0)
            {

                X = x;
                Y = y;
            }
            else
            {
                X = Math.Round(x, JwFileConsts.PickPrecision);
                Y = Math.Round(y, JwFileConsts.PickPrecision);
            }
            
            IsValid = true;
        }

        public JWPoint(bool isNull)
        {
            IsValid = false;
        }

        public JWPoint(string x, string y)
        {
            X = Convert.ToDouble(x);
            Y = Convert.ToDouble(y);
            IsValid = true;
        }
        public static bool operator ==(JWPoint? left, JWPoint? right)
        {
            if(object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
            {
                return false;
            }
            else
            {
                //return Math.Round(left.X, 4) ==  Math.Round(right.X, 4) && Math.Round(left.Y, 4) == Math.Round(right.Y, 4);
                return left?.X == right?.X && left?.Y == right?.Y;
            }
        }
            //=>  left?.X == right?.X && left?.Y == right?.Y

        public static bool operator !=(JWPoint? left, JWPoint? right) => !(left == right);

        public override bool Equals([NotNullWhen(true)] object? obj) => obj is JWPoint && Equals((JWPoint)obj);

       // public bool Equals(JWPoint other) => this == other;

        public string GetPointString()
        {
            return string.Format("{0} {1}", X, Y);
        }

        public bool Equals(JWPoint? other)
        {
            if (object.ReferenceEquals(other,null))
            {
                return false;
            }
            else
            {
                return this == other;
            }
        }

        public bool IsEqualsWithError(JWPoint? other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            else
            {

                 return (Math.Abs( (this.X - other.X)) < (1 / JwFileConsts.JwScale)) && (Math.Abs((this.Y - other.Y)) < (1 / JwFileConsts.JwScale));

            }
        }

        public void Zoom(double zoom)
        {
            X *= zoom; Y *= zoom;
        }

        public void ChangeAxis(double x, double y)
        {
            Y = y - Y;
            X = X + x;
        }

        /// <summary>
        /// 转换为数据库存储的
        /// </summary>
        /// <returns></returns>
        public Point ToPoint()
        {
            Point p=new Point(this.X,this.Y);
            return p;
        }

        public bool Equals2D(JWPoint other)
        {
            return X == other.X && Y == other.Y;
        }

        public JWPoint Jiangjingdu()
        {
            JWPoint reobj = new JWPoint();
            reobj.X=Math.Round(X,JwFileConsts.JiangjingduInt);
            reobj.Y=Math.Round(Y, JwFileConsts.JiangjingduInt);
            return reobj;
        }

    }

    public class JwPointComparint : IEqualityComparer<JWPoint>
    {
        public bool Equals(JWPoint? x, JWPoint? y)
        {
            if(object.ReferenceEquals(x, null))
            {
                return false;
            }
            if (object.ReferenceEquals(y,null))
            {
                return false;
            }
            else
            {
               return x.Equals(y);
            }
        }

        public int GetHashCode(JWPoint obj)
        {
            return obj.GetPointString().GetHashCode();
        }

    }
}
