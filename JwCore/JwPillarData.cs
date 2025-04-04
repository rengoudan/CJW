using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace JwCore
{
    [Serializable]
    public class JwPillarData: JwSquareBaseData
    {

        public virtual string PillarCode { get; set; } 

        public virtual int BlocksCount { get; set; }

        public virtual PillarBaseType BaseType { get; set; }

        public virtual Point? FirstLocation { get; set; }

        public virtual double? FirstWidth { get; set; }

        public virtual double? FirstHeight { get; set; } 

        public virtual Point? CenterLocation { get; set; } 

        public virtual double? CenterWidth { get; set; } 

        public virtual double? CenterHeight { get; set; } 

        public virtual Point? LastLocation { get; set;}

        public virtual double? LastWidth { get; set; } 

        public virtual double? LastHeight { get; set; }

        public virtual string TaggTitle { get; set; } = "";

        

        public virtual string JwProjectSubDataId { get; set; } 

        public virtual JwProjectSubData JwProjectSubData { get; set; } = null!;

    }
}
