using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{

    public class JWMian
    {
        public List<JwXian> Xians { get; set; }

        public int XianCout { get; set; }

        public bool IsClosedLoop { get; set; }

        /// <summary>
        /// 四个焦点 支持斜方向的梁 通过交点 去生成矩形 方法更科学2025年2月12日
        /// </summary>
        public List<JWPoint> Points { get; set; }


        public double Jiaodu { get; set; }

        /// <summary>
        /// 特指长的
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 特指短的
        /// 
        /// </summary>
        public double Height { get; set; }

        public JWPoint CenterPoint { get; set; }

    }
}
