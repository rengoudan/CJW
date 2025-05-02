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
       
        //孔径和 孔间距存在 projectsub 里
        public virtual string JwBeamDataId { get; set; }

        public virtual JwBeamData JwBeamData { get; set; } = null!;

        

        /// <summary>
        /// 相对的位置
        /// </summary>
        public virtual TaggDirect Position { get; set; }


        /// <summary>
        /// 记录拜访中心点方便排序使用
        /// </summary>
        public virtual double Center { get; set; }

        public virtual bool HasPre { get; set; } = false;

        public virtual bool HasLast { get; set; }=false;

        
    }
}
