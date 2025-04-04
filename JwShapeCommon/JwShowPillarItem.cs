using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace JwShapeCommon
{
    public class JwShowPillarItem: JwShowItemBase,IChangeAxis,ICanZoom
    {
        public JwPillar Pillar { get; set; }

        public JwPillar _canchangepillar;

        public List<Rectangle> RectangleS = new List<Rectangle>();

        public JwShowPillarItem(JwPillar pillar)
        {
            Pillar = pillar;
            _canchangepillar = AutoMapperHelper.GetInstance().GetMapper().Map<JwPillar>(pillar);
            CanDraw = true;
        }

        public void ChangeAxis(double x, double y)
        {
            _canchangepillar.ChangeAxis(x, y);
            foreach(var b in _canchangepillar.Blocks)
            {
                var sp=new Point((int)b.TopLeft.X, (int)b.TopLeft.Y);
                RectangleS.Add(new Rectangle(sp, new Size((int)b.Width, (int)b.Height)));
            }
        }

        public void Zoom(double zoom)
        {
            _canchangepillar.Zoom(zoom);

        }
    }
}
