using JwShapeCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{


    //public class JWMian
    //{
    //    public List<JwXian> Xians { get; set; }

    //    public int XianCout { get; set; }


    //}

    //public class JWPoint : IEquatable<JWPoint>
    //{
    //    public double X { get; set; }
    //    public double Y { get; set; }

    //    public JWPoint(string x, string y)
    //    {
    //        X = Convert.ToDouble(x);
    //        Y = Convert.ToDouble(y);
    //    }
    //    public static bool operator ==(JWPoint left, JWPoint right) => left.X == right.X && left.Y == right.Y;

    //    public static bool operator !=(JWPoint left, JWPoint right) => !(left == right);

    //    public override bool Equals([NotNullWhen(true)] object? obj) => obj is JWPoint && Equals((JWPoint)obj);

    //    public bool Equals(JWPoint other) => this == other;

    //    public string GetPointString()
    //    {
    //        return string.Format("{0} {1}", X, Y);
    //    }
    //}

    //public class JwXian
    //{
    //    public JWPoint Pone { get; set; }
    //    public string Ponestr { get; set; }
    //    public JWPoint Ptwo { get; set; }
    //    public string Ptwostr { get; set; }
    //    public bool CreateSuccess = false;

    //    public string Id { get; set; }

    //    public decimal Length { get; set; }

    //    public JwXian(string jwstr)
    //    {
    //        var q = jwstr.Split(" ");
    //        if (q.Length == 4)
    //        {
    //            this.Pone = new JWPoint(q[0], q[1]);
    //            this.Ptwo = new JWPoint(q[2], q[3]);
    //            this.CreateSuccess = true;
    //            this.Ponestr = Pone.GetPointString();
    //            this.Ptwostr = Ptwo.GetPointString();
    //            this.Length=Convert.ToDecimal(JwXian.distance(this.Pone, this.Ptwo));   
    //        }
    //        Id = Guid.NewGuid().ToString();
    //    }

    //    public List<JWPoint> GetXianPoints()
    //    {
    //        List<JWPoint> q = new List<JWPoint>
    //        {
    //            Pone,
    //            Ptwo
    //        };
    //        return q;
    //    }

    //    public bool Isxiangjiao(JwXian other)
    //    {
    //        if (other.Pone == this.Pone || other.Ptwo == this.Ptwo || other.Ptwo == this.Pone || other.Pone == this.Ptwo)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //        // var z=other.GetXianPoints();
    //        //var q = this.GetXianPoints();
    //        //var xj=z.Intersect(q).ToList();
    //        //if (xj.Count() > 0)
    //        //{
    //        //    return true;
    //        //}
    //        //else
    //        //{
    //        //    return false;
    //        //}
    //    }

    //    public bool IsSelected = false;

    //    public static double distance(JWPoint p1, JWPoint p2)
    //    {
    //        double r = 0;
    //        r=Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X)+(p1.Y-p2.Y)*(p1.Y-p2.Y));
    //        return r;
    //    }
    //}

    public class JwProjectDto 
    {

        public long Id { get; set; }
        public string ProjectName { get; set; }

        public string CustomerName { get; set; }

        public decimal ProjectCost { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? DateCompleted { get; set; }

        public string PlaceofDelivery { get; set; }

        public int? BeamsNumber { get; set; }

        public string CreateDesigner { get; set; }

        public int? JwCustomerId { get; set; }

    }


    [Serializable]
    public class ResultDto<T>
    {
        public ResultDto()
        {

        }
        public ResultDto(T obj)
        {
            Result = obj;
        }

        /// <summary>
        /// 通用返回类型
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// 请求相应状态
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 需要传递给三维程序的参数string
        /// </summary>
        public string JsonArg { get; set; }

    }
}
