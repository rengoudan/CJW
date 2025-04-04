using JwCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwSingleBeamDraw
    {
        private JwBeam _beam;

        public JwSingleBeamDraw(JwBeam beam)
        {
            if(beam.DirectionType==BeamDirectionType.Horizontal)
            {
                _beam = beam;
            }
            else
            {
                _beam = JwShapeHelper.VerticalToHorizontal(beam);
                //    JwDrawShape beamsp = new JwDrawShape(verticalbeam);
                //    controls.AddRange(beamsp.Change(_minbeilv, axisX, axisY));
            }
        }

        public List<ControlDraw> controls { get; set; }

        public List<ControlLine> Lines { get; set; }

        public List<float> FuzhuXs =new List<float>();

        public List<float> FuzhuYs = new List<float>();

        public void Draw(int wwidth,int wheight,int xoffset,int yoffset)
        {
            int everyheight=wheight/6;
            int eachheight=wheight/3;
            int everyoffset = wheight / 8;
            int everyoffsetw = wwidth / 5;
            if (_beam != null)
            {
                controls=new List<ControlDraw>();
                Lines=new List<ControlLine>();
                var wb = Math.Round((double)(wwidth - everyoffsetw) / _beam.Width, 2);
                var hb = Math.Round((double)(eachheight - everyoffset) / (_beam.Height*2), 2);
                var _minbeilv = wb > hb ? hb : wb;
                var cx = (wwidth) / 2;
                var cy = (wheight) / 2;
                //中间梁偏移
                var axisX = cx - _beam.CenterPoint.X*_minbeilv;
                var axisY = cy + _beam.CenterPoint.Y*_minbeilv;
                //上面 偏移
                var axisXt = cx - _beam.CenterPoint.X * _minbeilv;
                var axisYt = everyheight + _beam.CenterPoint.Y * _minbeilv;
                //下面 偏移
                var axisXb = cx - _beam.CenterPoint.X * _minbeilv;
                var axisYb = everyheight * 5 + _beam.CenterPoint.Y * _minbeilv;

                //梁三面绘制
                JwDrawShape beamsp = new JwDrawShape(_beam,2);//现阶段200
                JwDrawShape beamspt = new JwDrawShape(_beam);
                JwDrawShape beamspb = new JwDrawShape(_beam);
                controls.AddRange(beamsp.Change(_minbeilv, axisX, axisY));
                controls.AddRange(beamspt.Change(_minbeilv, axisXt, axisYt));
                controls.AddRange(beamspb.Change(_minbeilv, axisXb, axisYb));
                FuzhuYs.Add((float)beamspt.CenterPoint.Y);
                FuzhuYs.Add((float)beamsp.CenterPoint.Y);
                FuzhuYs.Add((float)beamspb.CenterPoint.Y);
                //总长 仅需要一个
                ControlLine julit = new ControlLine
                {
                    Start = _beam.TopLeft,
                    End = _beam.TopRight
                };
                julit.Distance = Math.Round((julit.End.X - julit.Start.X), 2) * JwFileConsts.JwScale;
                julit.Title = Convert.ToString((int)julit.Distance);
                julit.HeightLine = (float)(beamspt.TopLeft.Y / 3);
                julit.DrawStart = new System.Drawing.PointF((float)beamspt.TopLeft.X, julit.HeightLine);
                julit.DrawEnd = new System.Drawing.PointF((float)beamspt.TopRight.X, julit.HeightLine);
                Lines.Add(julit);

                float ty = (float)(beamspt.TopLeft.Y / 3)*2;

                float zjy = (float)(beamsp.TopLeft!.Y + beamspt.BottomLeft!.Y) / 2;
                
                float bdy= (float)(beamsp.BottomLeft!.Y + beamspb.TopLeft!.Y) / 2;
                //JWPoint bls = beamsp.jwShape.TopLeft;
                //PointF dbls=juli.DrawStart;
                //float jzy = ((float)beamsp.TopLeft.Y + juli.DrawEnd.Y) / 2;
                //var blocklst = _beam.ZhuBlocks.OrderBy(t => t.CenterPoint.X).ToList();
                //foreach (var b in blocklst)
                //{
                //    JwDrawShape bz=new JwDrawShape(b);
                //    controls.AddRange(bz.Change(_minbeilv,axisX,axisY));
                //    FuzhuXs.Add((float)bz.CenterPoint.X);
                //    ControlLine line = new ControlLine
                //    {
                //        Start = bls,
                //        End = b.CenterPoint,
                //        DrawStart = new PointF(dbls.X,jzy),
                //        DrawEnd = new PointF((float)bz.CenterPoint.X, jzy)
                //    };
                //    line.Distance = Math.Round((b.CenterPoint.X - bls.X), 2) * JwFileConsts.JwScale;
                //    line.Title = Convert.ToString((int)line.Distance);
                //    Lines.Add(line);
                //    bls = b.CenterPoint;
                //    dbls = line.DrawEnd;
                //}
                //if(_beam.ZhuBlocks.Count > 0)
                //{
                //    ControlLine line = new ControlLine
                //    {
                //        Start = bls,
                //        End = _beam.TopRight,
                //        DrawStart = dbls,
                //        DrawEnd = new PointF((float)beamsp.TopRight.X, jzy)
                //    };
                //    line.Distance = Math.Round((_beam.TopRight.X - bls.X), 2) * JwFileConsts.JwScale;
                //    line.Title = Convert.ToString((int)line.Distance);
                //    Lines.Add(line);
                //}

                if (_beam.Holes.Count > 0)
                {
                    JWPoint tbones = _beam.TopLeft;
                    JWPoint ctones = _beam.TopLeft;
                    JWPoint btones = _beam.TopLeft;

                    var tholes = _beam.Holes.Where(t => t.HasTop).OrderBy(t=>t.Location.X).ToList();
                    PointF dbls = new PointF((float)beamspt.TopLeft.X, ty);
                    if (tholes.Count > 0)
                    {
                        foreach (var chole in tholes)
                        {
                            JwDrawShape thh = new JwDrawShape(chole, DrawShapeType.Hole);
                            controls.AddRange(thh.Change(_minbeilv, axisXt, axisYt));
                            FuzhuXs.Add((float)thh.CenterPoint.X);
                            ControlLine hline = new ControlLine
                            {
                                Start = tbones,
                                End = chole.Location,
                                DrawStart = new PointF(dbls.X, dbls.Y),
                                DrawEnd = new PointF((float)thh.CenterPoint.X, ty)
                            };
                            //hline.Distance = Math.Round((chole.Location.X - tbones.X), 2) * JwFileConsts.JwScale;
                            hline.Distance = Math.Round((chole.Location.X - tbones.X) * JwFileConsts.JwScale, 0);
                            hline.Title = Convert.ToString((int)hline.Distance);
                            Lines.Add(hline);
                            tbones = chole.Location;
                            dbls = hline.DrawEnd;
                        }
                        ControlLine lastline = new ControlLine
                        {
                            Start = tbones,
                            End = _beam.TopRight,
                            DrawStart = dbls,
                            DrawEnd = new PointF((float)beamspt.TopRight.X, ty)
                        };
                        //lastline.Distance = Math.Round((_beam.TopRight.X - tbones.X), 2) * JwFileConsts.JwScale;
                        lastline.Distance = Math.Round((_beam.TopRight.X - tbones.X) * JwFileConsts.JwScale, 0) ;
                        lastline.Title = Convert.ToString((int)lastline.Distance);
                        Lines.Add(lastline);

                    }

                    var choles = _beam.Holes.Where(t => t.HasCenter).OrderBy(t=>t.Location.X).ToList();
                    PointF cdbls = new PointF((float)beamsp.TopLeft.X, zjy);
                    if (choles.Count > 0)
                    {
                        foreach (var chole in choles)
                        {
                            JwDrawShape tlh = new JwDrawShape(chole, DrawShapeType.Hole);
                            controls.AddRange(tlh.Change(_minbeilv, axisX, axisY));

                            FuzhuXs.Add((float)tlh.CenterPoint.X);
                            ControlLine hline = new ControlLine
                            {
                                Start = ctones,
                                End = chole.Location,
                                DrawStart = new PointF(cdbls.X, cdbls.Y),
                                DrawEnd = new PointF((float)tlh.CenterPoint.X, zjy)
                            };
                            //hline.Distance = Math.Round((chole.Location.X - ctones.X), 2) * JwFileConsts.JwScale;
                            hline.Distance = Math.Round((chole.Location.X - ctones.X) * JwFileConsts.JwScale, 0);
                            hline.Title = Convert.ToString((int)hline.Distance);
                            Lines.Add(hline);
                            ctones = chole.Location;
                            cdbls = hline.DrawEnd;
                        }
                        ControlLine lastline = new ControlLine
                        {
                            Start = ctones,
                            End = _beam.TopRight,
                            DrawStart = cdbls,
                            DrawEnd = new PointF((float)beamsp.TopRight.X, zjy)
                        };
                        //lastline.Distance = Math.Round((_beam.TopRight.X - ctones.X), 2) * JwFileConsts.JwScale;
                        lastline.Distance = Math.Round((_beam.TopRight.X - ctones.X) * JwFileConsts.JwScale, 0);
                        lastline.Title = Convert.ToString((int)lastline.Distance);
                        Lines.Add(lastline);
                    }


                    var bholes = _beam.Holes.Where(t => t.HasBottom).OrderBy(t=>t.Location.X).ToList();
                    PointF bdbls = new PointF((float)beamsp.TopLeft.X, bdy);
                    if (bholes.Count > 0)
                    {
                        foreach (var chole in bholes)
                        {
                            JwDrawShape tlh = new JwDrawShape(chole, DrawShapeType.Hole);
                            controls.AddRange(tlh.Change(_minbeilv, axisXb, axisYb)); FuzhuXs.Add((float)tlh.CenterPoint.X);
                            ControlLine hline = new ControlLine
                            {
                                Start = btones,
                                End = chole.Location,
                                DrawStart = new PointF(bdbls.X, bdbls.Y),
                                DrawEnd = new PointF((float)tlh.CenterPoint.X, bdy)
                            };
                            hline.Distance = Math.Round((chole.Location.X - btones.X) * JwFileConsts.JwScale, 0);
                            hline.Title = Convert.ToString((int)hline.Distance);
                            Lines.Add(hline);
                            btones = chole.Location;
                            bdbls = hline.DrawEnd;
                        }
                        ControlLine lastline = new ControlLine
                        {
                            Start = btones,
                            End = _beam.TopRight,
                            DrawStart = bdbls,
                            DrawEnd = new PointF((float)beamspb.TopRight.X, bdy)
                        };
                        lastline.Distance = Math.Round((_beam.TopRight.X - btones.X) * JwFileConsts.JwScale, 0);
                        lastline.Title = Convert.ToString((int)lastline.Distance);
                        Lines.Add(lastline);
                    }
                }
            }
        }
    }
}
