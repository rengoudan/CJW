using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.RGB
{
    /// <summary>
    /// rgb cad 绘制元素的抽象类
    /// </summary>
    public abstract class RGBBase
    {
        public int BrushStyle { get; set; }

        public int BrushColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract void DrawCad(Graphics g);

    }
}
