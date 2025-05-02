using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    /// <summary>
    /// 存储败方数据 解决批量导出
    /// </summary>
    public class JwBeamVerticalData: BaseGuidEntityData
    {
        public string ParentBeamId { get; set; }

        public string BaiBeamId { get; set; }

        /// <summary>
        /// 相对的位置
        /// </summary>
        public TaggDirect Position { get; set; }

        /// <summary>
        /// 败方
        /// </summary>
        public JwBeamData VerticalBeam { get; set; }

        /// <summary>
        /// 记录拜访中心点方便排序使用
        /// </summary>
        public double Center { get; set; }

        public bool HasPre { get; set; }

        public bool HasLast { get; set; }

        
    }
}
