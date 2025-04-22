using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JwShapeCommon
{
    public class ControlLine
    {
        public JWPoint Start { get; set; }

        public PointF DrawStart { get; set; }

        public JWPoint End { get; set;}

        public PointF DrawEnd { get; set; }

        public string Title { get; set; }

        public double Distance { get; set; }

        public float HeightLine { get;set; }

        public bool HasMsg { get; set; }

        public bool IsXX { get; set; }

        //public DashStyle DashStyle { get; set; }
    }
}
