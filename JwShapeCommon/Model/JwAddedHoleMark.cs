using JwCore;
using JwwHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public  class JwAddedHoleMark
    {
        public string Id { get; set; }
        public JWPoint CenterPoint { get; set; }
        public JwXian Line1 { get; set; }
        public JwXian Line2 { get; set; }

        /// <summary>
        /// 用来避免交叉点不在梁中心线的情况重新生成孔中心点 方便holeorder
        /// </summary>
        public JWPoint HoleCenter { get; set; }

        public bool HasBeam { get; set; }

        public JwBeam? OwerBeam { get; set; }

        public string GetXianString()
        {
            return "x";
        }

        public JwAddedHoleMarkData ToData()
        {
            var data = new JwAddedHoleMarkData();
            data.Id = Id;
            data.Location = new NetTopologySuite.Geometries.Point(CenterPoint.X, CenterPoint.Y);
            data.LineAS = new NetTopologySuite.Geometries.Point(Line1.Pone.X, Line1.Pone.Y);
            data.LineAE = new NetTopologySuite.Geometries.Point(Line1.Ptwo.X, Line1.Ptwo.Y);
            data.LineBS = new NetTopologySuite.Geometries.Point(Line2.Pone.X, Line2.Pone.Y);
            data.LineBE = new NetTopologySuite.Geometries.Point(Line2.Ptwo.X, Line2.Ptwo.Y);
            data.HoleCenter = new NetTopologySuite.Geometries.Point(HoleCenter.X, HoleCenter.Y);
            data.HasBeam = HasBeam;
            return data;
        }

        public List<JwwData> ToJwwData()
        {
            List<JwwData> jd = new List<JwwData>();
            jd.Add(Line1.ToJwwData(DrawShapeType.AddedHole));
            jd.Add(Line2.ToJwwData(DrawShapeType.AddedHole));
            return jd;
        }

    }
}
