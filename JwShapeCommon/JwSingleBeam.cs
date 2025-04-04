using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwSingleBeam : JwSquareBase, ICanZoom, IChangeAxis
    {
        public JwBeam DrawBeam { get; set; }

        public JwSingleBeam(JwBeam beam) 
        {
            if(beam.DirectionType==BeamDirectionType.Horizontal)
            {
                DrawBeam = AutoMapperHelper.GetInstance().GetMapper().Map<JwBeam>(beam);
            }
            else
            {
                DrawBeam=JwShapeHelper.VerticalToHorizontal(beam);
            }
        }

        public List<RectangleF> Ellipselst = new List<RectangleF>();

        /// <summary>
        /// 绘制圆圈前调用
        /// </summary>
        public void CreateDrawShape()
        {
            foreach(var bl in DrawBeam.ZhuBlocks)
            {
                var cp = bl.CenterPoint;
                if (cp != null)
                {
                    var sz = new SizeF((float)EllipseDiameter, (float)EllipseDiameter);
                    var sp = new PointF((float)(cp.X- EllipseSpacing / 2), (float)(cp.Y+ EllipseSpacing / 2));
                    Ellipselst.Add(new RectangleF(sp, sz));
                    var sp1 = new PointF((float)(cp.X + EllipseSpacing / 2), (float)(cp.Y + EllipseSpacing / 2));
                    Ellipselst.Add(new RectangleF(sp1, sz));

                    var sp2 = new PointF((float)(cp.X + EllipseSpacing / 2), (float)(cp.Y - EllipseSpacing / 2));
                    Ellipselst.Add(new RectangleF(sp2, sz));
                    var sp3 = new PointF((float)(cp.X - EllipseSpacing / 2), (float)(cp.Y - EllipseSpacing / 2));
                    Ellipselst.Add(new RectangleF(sp3, sz));
                }
               
            }

            
        }
        private double EllipseDiameter = 0;
        public  double EllipseSpacing = 0;

        public Rectangle BeamRectangle { get; set; }

        public bool IsDraw = false;


        public bool HasZoom = false;

        public bool HasChangeAxis = false;

    }
}
