using JwCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public  class ControlDraw
    {
        public JwSquareBase JwSquareBase { get; set; }

        public JwSquareBase ParentSquareBase { get; set; }
        public Color PenColor { get; set; }

        public bool IsNeed { get; set; }

        public DrawShapeType ShapeType { get; set; }

        public RectangleF DrawRectangleF { get; set; }

        public List<PointF> DrawPoints { get; set; }

        public bool IsTeshuBeam { get; set; }

        private bool _iselected;
        public bool IsSelected 
        {
            get { return _iselected; }
            set
            {
                _iselected = value;
                //if (_iselected!=value)
                //{
                //    _iselected = value;
                //    ///好像没必要
                //    //if (GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent != null)
                //    //{
                //    //    GlobalEvent.GetGlobalEvent().ControlSelectedSquareEvent(this,new ControlSelectedSquareArgs { Id=JwSquareBase.Id });
                //    //}
                //}
                
            }
        }
    }
}
