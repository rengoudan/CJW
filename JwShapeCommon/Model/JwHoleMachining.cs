using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    /// <summary>
    /// 机加工 孔组所需的信息
    /// 在beam 的holeorder方法里 实现  对于一个孔组的 需要增加两个
    /// 内部实现tocsv的方法
    /// </summary>
    public class JwHoleMachining
    {
        public double RelativeStartDistance { get; set; } = 0;


        /// <summary>
        /// 工字梁上面
        /// </summary>
        public bool HasLeft { get; set; }


        /// <summary>
        /// 工字梁下面
        /// </summary>
        public bool HasRight { get; set; }  

        /// <summary>
        /// 工字梁中间
        /// </summary>
        public bool HasTop { get; set; }    
    }
}
