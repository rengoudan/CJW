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

        private List<JWPoint> _points;
        /// <summary>
        /// 四个焦点 支持斜方向的梁 通过交点 去生成矩形 方法更科学2025年2月12日
        /// </summary>
        public List<JWPoint> Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
                double cx=_points.Average(p => p.X);
                double cy=_points.Average(p => p.Y);
                CenterPoint = new JWPoint(cx, cy);
            }
        }

        /// <summary>
        /// 特指length与x轴的夹角 0-180度 0表示水平 90表示垂直 180表示水平
        /// </summary>
        public double Jiaodu { get; set; }

        /// <summary>
        /// 指定是否存在角度变差，标记一下 2026年7月9日
        /// </summary>
        public bool IsJiaoduPiancha { get; set; }

        /// <summary>
        /// 特指长的 等同length length赋值给beam的length
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 特指短的
        /// 
        /// </summary>
        public double Height { get; set; }

        public JWPoint CenterPoint { get; set; }

        /// <summary>
        /// 边长
        /// </summary>
        public double Length { get; set; }

    }
}
