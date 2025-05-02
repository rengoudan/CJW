using JwCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        /// <summary>
        /// 记录拜访中心点方便排序使用
        /// </summary>
        public double Center { get; set; }

        public bool HasPre { get; set; }

        public bool HasLast { get; set; }

        public JwBeamVerticalData ToData()
        {
            JwBeamVerticalData r = new JwBeamVerticalData
            {
                ParentBeamId = ParentBeamId,
                BaiBeamId = BaiBeamId,
                Position=Position,
                Center = Center,
                HasPre = HasPre,
                HasLast = HasLast
            };
            return r;
        }
    }
}
