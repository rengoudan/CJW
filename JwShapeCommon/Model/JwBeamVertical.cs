using JwCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    /// <summary>
    /// beam
    /// 胜方所拥有的 垂直于它的梁数据信息 
    /// </summary>
    public class JwBeamVertical
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
        public JwBeam VerticalBeam { get; set; }

        /// <summary>
        /// 坐标
        /// </summary>
        public JWPoint PositionPoint { get;set; }
    }
}
