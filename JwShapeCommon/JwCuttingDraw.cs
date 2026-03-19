using JwShapeCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JwShapeCommon
{
    public class JwCuttingDraw : ICanZoom, IChangeAxis
    {
        /// <summary>
        /// 避免地址引用更改原有
        /// </summary>
        private JwDirected _jwDirected;

        public JwCuttingDraw(JwDirected jwDirected)
        {
            _jwDirected = new JwDirected(jwDirected.Points);
        }

        public List<PointF> Polygon= new List<PointF>();

        public void Change(double zoom, double axisx, double axisy)
        {
            Zoom(zoom);

            ChangeAxis(axisx, axisy);
            
            Polygon = _jwDirected.Points.Select(p => p.ToPointF()).ToList();
        }

        public void ChangeAxis(double x, double y)
        {
            _jwDirected.Points.ForEach(p => p.ChangeAxis(x, y));
        }

        public void Zoom(double zoom)
        {
            _jwDirected.Points.ForEach(p => p.Zoom(zoom));
        }
    }
}
