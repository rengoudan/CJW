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

        public string Id { get; set; }

        public double RelativeStartDistance { get; set; }


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
        

        public string ToCsvString(double y)
        {
            
            //double ysjv = Math.Abs(Y) + .CsvYwucha;
            return string.Format("{3},絶対,先端,{0},{1},1,{2},1,0.0,\\\\r\\\\n", RelativeStartDistance.ToString("0.0"), y.ToString("0.0"), JwFileConsts.Kongjing.ToString("0.0"), JwFileConsts.EllipseDiameter.ToString("0.0"));
            //return $"{Id},{RelativeStartDistance},{HasLeft},{HasRight},{HasTop}";
        }
    }
}
