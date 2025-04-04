using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwLinkPartData:JwSquareBaseData
    {
        public string BujianName { get; set; }

        public string BeamId { get; set; }

        public TaggDirect Directed { get; set; }

        public GouJianType GouJianType { get; set; }

        public GouJianCreateFrom CreateFrom { get; set; }

        public bool IsLianjie { get; set; }

        public bool IsNoBeam { get; set; }

        public string JwProjectSubDataId { get; set; }

        public JwProjectSubData JwProjectSubData { get; set; } = null!;
    }
}
