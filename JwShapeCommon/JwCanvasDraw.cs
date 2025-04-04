using JwCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwCanvasDraw
    {
        private JwCanvas jwCanvas;

        public JwCanvasDraw( JwCanvas _jwCanvas)
        {
            this.jwCanvas = _jwCanvas;
        }

        public List<ControlDraw> controls { get; set; }

        public List<float> FuzhuXs = new List<float>();

        public List<float> FuzhuYs = new List<float>();
        public List<LinkDrawModel> links = new List<LinkDrawModel>();

        public List<ControlText> Texts = new List<ControlText>();

        public void Draw(int wwidth, int wheight, int xoffset, int yoffset)
        {

            if (jwCanvas != null)
            {
                controls = new List<ControlDraw>();
                var wb = Math.Round((double)(wwidth - xoffset) / jwCanvas.Width, 2);
                var hb = Math.Round((double)(wheight - yoffset) / jwCanvas.Height, 2);
                var _minbeilv = wb > hb ? hb : wb;
                var cx = (wwidth) / 2;
                var cy = (wheight) / 2;
                var axisX = cx - jwCanvas.CenterPoint.X * _minbeilv;
                var axisY = cy + jwCanvas.CenterPoint.Y * _minbeilv;
                foreach(var bm in jwCanvas.Beams)
                {
                    JwDrawShape beamsp = new JwDrawShape(bm);
                    controls.AddRange(beamsp.Change(_minbeilv, axisX, axisY));
                    Texts.AddRange(beamsp.Texts);
                    FuzhuYs.AddRange(beamsp.FuzhuYs);
                    FuzhuXs.AddRange(beamsp.FuzhuXs);
                    //if (bm.DirectionType==BeamDirectionType.Horizontal)
                    //{
                    //    JwDrawShape beamsp = new JwDrawShape(bm);
                    //    controls.AddRange(beamsp.Change(_minbeilv, axisX, axisY));
                    //}
                    //if(bm.DirectionType==BeamDirectionType.Vertical)
                    //{
                    //    var verticalbeam= JwShapeHelper.VerticalToHorizontal(bm);
                    //    JwDrawShape beamsp = new JwDrawShape(verticalbeam);
                    //    controls.AddRange(beamsp.Change(_minbeilv, axisX, axisY));
                    //}
                    //if (bm.LinkParts != null)
                    //{
                    //    foreach (var lk in bm.LinkParts)
                    //    {
                    //        var lkdraw=new JwLinkDraw(lk);

                    //        links.Add(lkdraw.Change(_minbeilv, axisX, axisY));
                    //    }
                    //}
                    
                }
                if (jwCanvas.Pillars != null)
                {
                    foreach (var pll in jwCanvas.Pillars)
                    {
                        foreach (var blp in pll.Blocks)
                        {
                            blp.Id=pll.Id;
                            JwDrawShape bz = new JwDrawShape(blp);
                            //bz.Id = pll.Id;
                            bz.ShapeType = DrawShapeType.Pillar;
                            controls.AddRange(bz.Change(_minbeilv, axisX, axisY));
                        }
                    }
                }
                //if(jwCanvas.ParentBeams!=null)
                //{
                //    if (jwCanvas.ParentBeams.Count > 0)
                //    {
                //        foreach (var b in jwCanvas.ParentBeams)
                //        {
                //            foreach (var lk in b.LinkParts)
                //            {
                //                var lkdraw = new JwLinkDraw(lk);

                //                links.Add(lkdraw.Change(_minbeilv, axisX, axisY));
                //            }
                //        }
                //    }
                //}
                if(jwCanvas.LinkParts != null)
                {
                    if(jwCanvas.LinkParts.Count > 0)
                    {
                        foreach(var lk in jwCanvas.LinkParts)
                        {
                            var lkdraw = new JwLinkDraw(lk);

                            links.Add(lkdraw.Change(_minbeilv, axisX, axisY));
                        }
                    }
                }
                
            }
        }
    }
}
