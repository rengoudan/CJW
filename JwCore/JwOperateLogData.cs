using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwOperateLogData:BaseGuidEntityData
    {
        /// <summary>
        /// 关联 指 main 或者sub的 id
        /// </summary>
        public string OperateRelatedId { get; set; }    

        /// <summary>
        /// 操作结果对应的id  预算 就是预算表ID
        /// </summary>
        public string OperateResultId { get; set; }

        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作层级  0 上面id main 1 为sub
        /// </summary>
        public OperateLevel OperateLevel { get; set; }


        public OperateType OperateType { get; set; }
    }
}
