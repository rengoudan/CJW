using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace JwCore
{
    public class JwCutting
    {
        public string Id { get; set; }

        public Point FirstPoint { get; set; }

        public Point SecondPoint { get; set; }  

        public Point ThirdPoint { get; set; }

        public virtual string JwProjectSubDataId { get; set; }

        public virtual JwProjectSubData JwProjectSubData { get; set; } = null!;
    }
}
