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

        /// <summary>
        /// 对应K 的中心位置
        /// </summary>
        public virtual Point? CenterLocation { get; set; }

        /// <summary>
        /// 2026年3月17日 用来标记K柱 目前有两个值对应解析出来的distance
        /// 9.1
        /// 9.0
        /// 保留小数点后一位
        /// </summary>
        public virtual double? CenterWidth { get; set; } 

        /// <summary>
        /// 可以考虑不使用 默认位0.5 且只有k柱才有 
        /// </summary>
        public virtual double? CenterHeight { get; set; } 

        public virtual Point? LastLocation { get; set;}

        public virtual double? LastWidth { get; set; } 

        public virtual double? LastHeight { get; set; }

        public virtual string TaggTitle { get; set; } = "";

        

        public virtual string JwProjectSubDataId { get; set; } 

        public virtual JwProjectSubData JwProjectSubData { get; set; } = null!;

    }
}
