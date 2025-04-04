using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwShowItemBase
    {
        public bool CanDraw { get; set; }

        public bool IsNeed { get; set; }

        public bool NeedReDraw = false;

        

        public Point DrawStartPoint { get; set; }

        public int Width { get; set; }

        private double _widthd;
        public double Widthd
        {
            get { return _widthd; }
            set { _widthd = value; Width = (int)value; }
        }

        public int Height { get; set; }

        private double _heightd;
        public double Heightd
        {
            get { return _heightd; }
            set
            {
                _heightd = value; Height = (int)value;
            }
        }

        public JWPoint JwDrawStartPoint { get; set; }
    }
}
