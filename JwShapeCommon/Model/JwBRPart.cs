using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public class JwBRPart
    {

        #region 属性
        public string Id { get; set; }

        public JwXian LineOne { get; set; }

        public JwXian LineTwo { get; set; }

        public JWPoint IntersectionPoint { get; set; }

        public List<JWPoint> Points { get; set; }

        /// <summary>
        /// 左上
        /// </summary>
        public JWPoint? TopLeft { get; set; }

        /// <summary>
        /// 右上
        /// </summary>
        public JWPoint? TopRight { get; set; }

        /// <summary>
        /// 左下
        /// </summary>
        public JWPoint? BottomLeft { get; set; }
        #endregion

        public JwBRPart() { }

        public JwBRPart(JwXian line1,JwXian line2)
        {
            Points = new List<JWPoint>();
            Points.AddRange(line1.GetXianPoints());
            Points.AddRange(line2.GetXianPoints());

        }

    }
}
