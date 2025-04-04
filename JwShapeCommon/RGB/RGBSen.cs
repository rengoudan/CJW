using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.RGB
{
    public class RGBSen : RGBBase
    {

        public double StartX { get;set; }

        public double StartY { get;set; }

        public double EndX { get;set; }

        public double EndY { get;set; }

        public override void DrawCad(Graphics g)
        {
            throw new NotImplementedException();
        }
    }
}
