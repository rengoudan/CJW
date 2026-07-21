using JwCore;
using JwShapeCommon.Model;
using Microsoft.Extensions.Logging.Abstractions;
using NetTopologySuite.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwCanvasDraw
    {
        public JwCanvas jwCanvas;

        public JwCanvasDraw(JwCanvas _jwCanvas)
        {
            this.jwCanvas = _jwCanvas;
        }

        public List<ControlDraw> controls { get; set; }

        public List<float> FuzhuXs = new List<float>();

        public List<float> FuzhuYs = new List<float>();
        public List<LinkDrawModel> links = new List<LinkDrawModel>();

        public List<JwDownPillarDrawModel> DownPillarMarks = new List<JwDownPillarDrawModel>();

        public List<ControlText> Texts = new List<ControlText>();

        public List<ControlLine> LianjieLines = new List<ControlLine>();

        public List<JwDownPillarDrawModel> DownPillars = new List<JwDownPillarDrawModel>();

        /// <summary>
        /// 2026年3月19日增加切割绘制
        /// </summary>
        public List<JwCuttingDraw> CuttingDraws = new List<JwCuttingDraw>();

        public void Draw(int wwidth, int wheight, int xoffset, int yoffset)
        {

            if (jwCanvas != null)
            {

                controls = new List<ControlDraw>();
                FuzhuXs.Clear();
                FuzhuYs.Clear();
                links.Clear();
                Texts.Clear();
                LianjieLines.Clear();
                DownPillars.Clear();
                CuttingDraws.Clear();
                var wb = Math.Round((double)(wwidth - xoffset) / jwCanvas.Width, 2);
                var hb = Math.Round((double)(wheight - yoffset) / jwCanvas.Height, 2);
                var _minbeilv = wb > hb ? hb : wb;
                var cx = (wwidth) / 2;
                var cy = (wheight) / 2;
                var axisX = cx - jwCanvas.CenterPoint.X * _minbeilv;
                var axisY = cy + jwCanvas.CenterPoint.Y * _minbeilv;
                foreach (var bm in jwCanvas.Beams)
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
                        JwDrawShape parentpll = new JwDrawShape(pll);
                        var q = parentpll.Change(_minbeilv, axisX, axisY);

                        foreach (var blp in pll.Blocks)
                        {
                            blp.Id = pll.Id;
                            JwDrawShape bz = new JwDrawShape(blp);
                            bz.ParentSquare = pll;
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
                if (jwCanvas.LinkParts != null)
                {
                    if (jwCanvas.LinkParts.Count > 0)
                    {
                        foreach (var lk in jwCanvas.LinkParts)
                        {
                            var lkdraw = new JwLinkDraw(lk);

                            links.Add(lkdraw.Change(_minbeilv, axisX, axisY));
                        }
                    }
                }
                if (!jwCanvas.IsFromData && jwCanvas.LianjieSingles != null && jwCanvas.LianjieSingles.Count > 0)
                {
                    foreach (var zlianjie in jwCanvas.LianjieSingles)
                    {
                        //生成controlline
                        ControlLine cline = new ControlLine();

                        JWPoint jpstart = new JWPoint(zlianjie.Start.RealPoint.X, zlianjie.Start.RealPoint.Y);
                        jpstart.Zoom(_minbeilv);
                        jpstart.ChangeAxis(axisX, axisY);
                        cline.Id = zlianjie.Id;
                        cline.DrawStart = jpstart.ToPointF();
                        JWPoint jpend = new JWPoint(zlianjie.End.RealPoint.X, zlianjie.End.RealPoint.Y);
                        jpend.Zoom(_minbeilv);
                        jpend.ChangeAxis(axisX, axisY);
                        cline.DrawEnd = jpend.ToPointF();
                        cline.Distance= zlianjie.Length;

                        LianjieLines.Add(cline);
                        float dx = (float)(150d / JwFileConsts.JwScale * _minbeilv);
                        if (zlianjie.HasEndChange)
                        {
                            ControlDraw wjx1=new ControlDraw();
                            wjx1.PenColor = Color.Purple;
                            var np = new JWPoint(zlianjie.End.RealPointOriginal.X, zlianjie.End.RealPointOriginal.Y);
                            np.Zoom(_minbeilv);
                            np.ChangeAxis(axisX, axisY);
                            PointF npf = np.ToPointF();
                            // 外接矩形（左上角坐标 + 宽高）
                            RectangleF rect = new RectangleF(
                                npf.X - dx,
                                npf.Y - dx,
                                dx * 2,
                                dx * 2);
                            //var lst= JwShapeHelper.GetStarPoints(np.ToPointF(), dx);
                            //wjx1.DrawPoints = new List<PointF> ();
                            //wjx1.DrawPoints.Add(np.ToPointF());
                            wjx1.DrawRectangleF = rect;
                            wjx1.ShapeType = DrawShapeType.Star;
                            controls.Add(wjx1);
                        }
                        if(zlianjie.HasStartChange)
                        {
                            ControlDraw wjx2 = new ControlDraw();
                            wjx2.PenColor = Color.Purple;
                            var np = new JWPoint(zlianjie.Start.RealPointOriginal.X, zlianjie.Start.RealPointOriginal.Y);
                            np.Zoom(_minbeilv);
                            np.ChangeAxis(axisX, axisY);
                            PointF npf = np.ToPointF();
                            // 外接矩形（左上角坐标 + 宽高）
                            RectangleF rect = new RectangleF(
                                npf.X - dx,
                                npf.Y - dx,
                                dx * 2,
                                dx * 2);
                            //var lst= JwShapeHelper.GetStarPoints(np.ToPointF(), dx);
                            //wjx1.DrawPoints = new List<PointF> ();
                            //wjx1.DrawPoints.Add(np.ToPointF());
                            wjx2.DrawRectangleF = rect;
                            wjx2.ShapeType = DrawShapeType.Star;
                            controls.Add(wjx2);
                        }

                    }
                }
                if (jwCanvas.IsFromData)
                {
                    if (jwCanvas.LianjieLsts.Count > 0)
                    {
                        foreach (var jlj in jwCanvas.LianjieLsts)
                        {
                            //生成controlline
                            ControlLine cline = new ControlLine();
                            JWPoint jpstart = new JWPoint(jlj.Start.X, jlj.Start.Y);
                            jpstart.Zoom(_minbeilv);
                            jpstart.ChangeAxis(axisX, axisY);
                            cline.Id = jlj.Id;
                            cline.DrawStart = jpstart.ToPointF();
                            JWPoint jpend = new JWPoint(jlj.End.X, jlj.End.Y);
                            jpend.Zoom(_minbeilv);
                            jpend.ChangeAxis(axisX, axisY);
                            cline.DrawEnd = jpend.ToPointF();
                            cline.Distance=jlj.Length;
                            LianjieLines.Add(cline);
                            float dx = (float)(150d / JwFileConsts.JwScale * _minbeilv);
                            if (jlj.HasEndChange)
                            {
                                ControlDraw wjx1 = new ControlDraw();
                                wjx1.PenColor = Color.Purple;
                                var np = new JWPoint(jlj.EndOriginal.X, jlj.EndOriginal.Y);
                                np.Zoom(_minbeilv);
                                np.ChangeAxis(axisX, axisY);
                                PointF npf = np.ToPointF();
                                // 外接矩形（左上角坐标 + 宽高）
                                RectangleF rect = new RectangleF(
                                    npf.X - dx,
                                    npf.Y - dx,
                                    dx * 2,
                                    dx * 2);
                                //var lst= JwShapeHelper.GetStarPoints(np.ToPointF(), dx);
                                //wjx1.DrawPoints = new List<PointF> ();
                                //wjx1.DrawPoints.Add(np.ToPointF());
                                wjx1.DrawRectangleF = rect;
                                wjx1.ShapeType = DrawShapeType.Star;
                                controls.Add(wjx1);
                            }
                            if (jlj.HasStartChange)
                            {
                                ControlDraw wjx2 = new ControlDraw();
                                wjx2.PenColor = Color.Purple;
                                var np = new JWPoint(jlj.StartOriginal.X, jlj.StartOriginal.Y);
                                np.Zoom(_minbeilv);
                                np.ChangeAxis(axisX, axisY);
                                PointF npf = np.ToPointF();
                                // 外接矩形（左上角坐标 + 宽高）
                                RectangleF rect = new RectangleF(
                                    npf.X - dx,
                                    npf.Y - dx,
                                    dx * 2,
                                    dx * 2);
                                //var lst= JwShapeHelper.GetStarPoints(np.ToPointF(), dx);
                                //wjx1.DrawPoints = new List<PointF> ();
                                //wjx1.DrawPoints.Add(np.ToPointF());
                                wjx2.DrawRectangleF = rect;
                                wjx2.ShapeType = DrawShapeType.Star;
                                controls.Add(wjx2);
                            }
                        }
                    }
                }
                
                if (jwCanvas.JwDownPillarDatas?.Count > 0)
                {
                    foreach (var dp in jwCanvas.JwDownPillarDatas)
                    {
                        JwDownPillarDraw jwDown = new JwDownPillarDraw(dp);
                        DownPillars.Add(jwDown.Change(_minbeilv, axisX, axisY));
                    }
                }
                if (jwCanvas.Directeds?.Count > 0)
                {
                    foreach (var d in jwCanvas.Directeds)
                    {
                        JwCuttingDraw directedDraw = new JwCuttingDraw(d);
                        directedDraw.Change(_minbeilv, axisX, axisY);
                        CuttingDraws.Add(directedDraw);
                    }
                }
            }
        }
    }
}
