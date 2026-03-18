using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwCuttingDraw : ICanZoom, IChangeAxis
    {

        private JwDirected _jwDirected;

        public JwCuttingDraw(JwDirected jwDirected)
        {
            _jwDirected = jwDirected;
        }

        public void ChangeAxis(double x, double y)
        {
            throw new NotImplementedException();
        }

        public void Zoom(double zoom)
        {
            throw new NotImplementedException();
        }
    }
}
