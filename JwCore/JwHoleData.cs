using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwHoleData:JwSquareBaseData
    {
        public bool HasTop { get; set; }

        public bool HasCenter { get; set; }

        public bool HasBottom { get; set; }

        public int KongNum { get; set; }

        public HoleCreateFrom FirstCreateFrom { get; set; }

        public HoleCreateFrom ChangeFrom { get; set; }

        public bool IsStart { get;set; }

        public bool IsEnd { get;set; }

        public KongzuType HoleType { get; set; }

        //孔径和 孔间距存在 projectsub 里
        public virtual string JwBeamDataId { get; set; }

        public virtual JwBeamData JwBeamData { get; set; } = null!;
    }
}
