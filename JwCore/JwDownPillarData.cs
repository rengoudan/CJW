using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace JwCore
{
    public class JwDownPillarData : JwSquareBaseData
    {
        public Point LineAS { get; set; }

        public Point LineAE { get; set; }

        public Point LineBS { get; set; }

        public Point LineBE { get; set; }

        public bool HasBeam { get; set; }

        //public bool IsInBeamCenter { get; set; }

        /// <summary>
        /// 判断交叉中心点是否再pillar里
        /// </summary>
        public bool HasPillar { get; set; }

        public virtual string JwProjectSubDataId { get; set; }

        public virtual JwProjectSubData JwProjectSubData { get; set; } = null!;
    }
}
