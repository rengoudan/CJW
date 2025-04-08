using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    /// <summary>
    /// 表示中心点  一个梁 所拥有的所有 需要在 设计图里体现出的之间的距离
    /// </summary>
    public class JwBeamMarkPoint
    {
        public bool IsBeamStart { get; set; }

        public bool IsBeamEnd { get; set; } 

        


        public bool IsCenterStart { get; set; }

        public bool IsCenterEnd { get; set; }

        /// <summary>
        /// 是否中心点些
        /// </summary>
        public bool IsCenter { get; set; }

        private JWPoint _point;
        public JWPoint Point 
        {
            get 
            {  
                return _point; 
            }
            set
            {
                _point = value;
            }
        }

        public double Coordinate { get; set; }

        public double PreCenterDistance { get; set; }
    }
}
