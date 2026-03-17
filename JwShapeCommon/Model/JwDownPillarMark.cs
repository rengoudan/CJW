using JwCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public class JwDownPillarMark
    {
        public string Id { get; set; }

        public JWPoint CenterPoint { get; set; }

        public JwXian Line1 { get; set; }

        public JwXian Line2 { get; set; }

        public bool HasBeam { get; set; }

        public JwBeam? OwerBeam { get; set; }

        public bool IsInBeamCenter { get; set; }

        /// <summary>
        /// 判断交叉中心点是否再pillar里
        /// </summary>
        public bool HasPillar { get; set; }


        public string GetXianString()
        {
            return "x";
        }

        public JwDownPillarData ToData()
        {
            var data = new JwDownPillarData();
            data.Id = Id;
            data.Location = new NetTopologySuite.Geometries.Point(CenterPoint.X, CenterPoint.Y);
            data.LineAS = new NetTopologySuite.Geometries.Point(Line1.Pone.X, Line1.Pone.Y);
            data.LineAE = new NetTopologySuite.Geometries.Point(Line1.Ptwo.X, Line1.Ptwo.Y);
            data.LineBS = new NetTopologySuite.Geometries.Point(Line2.Pone.X, Line2.Pone.Y);
            data.LineBE = new NetTopologySuite.Geometries.Point(Line2.Ptwo.X, Line2.Ptwo.Y);
            data.HasBeam = HasBeam;
            data.HasPillar = HasPillar;
            return data;
        }

    }


    public class JwDownPillarMarkComparint : IEqualityComparer<JwDownPillarMark>
    {
        public bool Equals(JwDownPillarMark? x, JwDownPillarMark? y)
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

                var x1 = Math.Round(x.CenterPoint.X,4);
                var x2 = Math.Round(y.CenterPoint.X, 4);
                var y1 = Math.Round(x.CenterPoint.Y, 4);
                var y2 = Math.Round(y.CenterPoint.Y, 4);
                return x1==x2 && y1==y2;
            }
        }

        public int GetHashCode(JwDownPillarMark obj)
        {
            return obj.GetXianString().GetHashCode();
        }

    }
}
