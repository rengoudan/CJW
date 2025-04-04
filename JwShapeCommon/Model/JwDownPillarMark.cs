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


        public string GetXianString()
        {
            return "x";
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
