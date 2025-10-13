using JwShapeCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
 
    /// <summary>
    /// 2025年6月24日 增加hole方便处理
    /// </summary>
    public class JwTouch
    {
        /// <summary>
        /// 胜方梁
        /// </summary>
        public JwBeam WinnerBeam { get; set; }

        public JwBeamVertical JwBeamVertical { get; set; }

        /// <summary>
        /// 败方梁
        /// </summary>
        public JwBeam LoserBeam { get; set; }

        /// <summary>
        /// 接触点  
        /// </summary>
        public JWPoint JieChuPoint { get; set; }

        /// <summary>
        /// 胜方的孔
        /// </summary>
        public JwHole JwHoleG { get; set; }

        public double BFCenter { get; set; }
    }

    /// <summary>
    /// 初步筛选出来的成对线
    /// </summary>
    public class JwChengduiXian
    {
        public JwXian XianOne { get; set; }

        public JwXian XianTwo { get; set; }

        public List<JwXian> Xians  { get; set; }

        public bool IsLianjie { get; set; }

    }

    public class JwQiegeZu
    {
        public double Qiegezb { get; set; }

        /// <summary>
        /// 左边 减 的小的一边
        /// </summary>
        public JwBeam RJwBeam { get; set; }

        /// <summary>
        /// 右边 加 大的一边
        /// </summary>
        public JwBeam AJwBeam { get; set; }
    }
}
