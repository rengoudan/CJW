using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace JwCore
{
    public class JwAddedHoleMarkData: JwSquareBaseData
    {
        public Point LineAS { get; set; }

        public Point LineAE { get; set; }

        public Point LineBS { get; set; }

        public Point LineBE { get; set; }

        public Point HoleCenter { get; set; }

        public bool HasBeam { get; set; }

        //public bool IsInBeamCenter { get; set; }

        /// <summary>
        /// 这个孔洞标记所在的梁id 方便生成当个粱的预览图
        /// </summary>
        public virtual string JwBeamDataId { get; set; }

        public virtual string JwProjectSubDataId { get; set; }

        public virtual JwProjectSubData JwProjectSubData { get; set; } = null!;
    }
}
