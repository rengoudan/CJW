using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public class JwLianjieSingle
    {
        public JwXian Xian { get; set; }

        public JwLianjieSingle(JwXian jwXian)
        {

        }


        /// <summary>
        /// 排序规则  由下到上  由左到右 根据胜方梁
        /// </summary>
        public JwPointBeam Start { get; set; }

        public JwPointBeam End { get; set; }

        
    }

    /// <summary>
    /// 连接线的 端点  
    /// </summary>
    public class JwPointBeam
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="touch"></param>
        public JwPointBeam(JWPoint point,JwTouch touch)
        {

        }

        /// <summary>
        /// 设计图里表示的 接触点
        /// </summary>
        public JWPoint NearPoint { get;set; }

        public JWPoint RealPoint { get; set; }

        /// <summary>
        /// 败方梁
        /// </summary>
        public JwBeam JieChuBeam { get; set; }

        /// <summary>
        /// 对应胜方梁 所属的梁
        /// </summary>
        public JwBeam ShengfangBeam { get; set; }

        /// <summary>
        /// 点关联的接触方
        /// </summary>
        public JwTouch Touch { get; set; }

        /// <summary>
        /// touch isshuping  true 判断 pone ptwo 的 Y大小 大的 减 小的加 false 判断 X大小 同样
        /// </summary>
        public double OffsetLosetCenter { get; set; }


        /// <summary>
        /// 暂无用
        /// </summary>
        public bool IsStart { get; set; }

        /// <summary>
        /// 暂无用
        /// </summary>
        public bool IsEnd { get; set; } = false;
    }


}
