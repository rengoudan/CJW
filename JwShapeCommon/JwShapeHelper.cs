using JwCore;
using JwShapeCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwShapeHelper
    {
        public static JwBeam VerticalToHorizontal(JwBeam beam)
        {
            if(beam?.DirectionType== BeamDirectionType.Vertical)
            {
                List<JWPoint> _points = new List<JWPoint>
                {
                    new JWPoint(beam.TopLeft.Y, beam.TopLeft.X),
                new JWPoint(beam.TopRight.Y, beam.TopRight.X),
                new JWPoint(beam.BottomLeft.Y, beam.BottomLeft.X),
                new JWPoint(beam.BottomRight.Y, beam.BottomRight.X),
                };
                JwBeam jwBeam = new JwBeam();
                jwBeam.TopLeft = _points.OrderBy(t => t.X).ThenByDescending(t => t.Y).ToList().First();
                jwBeam.TopRight = _points.OrderByDescending(t => t.X).ThenByDescending(t => t.Y).ToList().First();
                jwBeam.BottomLeft = _points.OrderBy(t => t.X).ThenBy(t => t.Y).ToList().First();
                jwBeam.BottomRight = _points.OrderByDescending(t => t.X).ThenBy(t => t.Y).First();
                jwBeam.JisuanWidthHeight();
                jwBeam.HasEndSide = beam.HasEndSide;
                jwBeam.HasStartSide = beam.HasStartSide;
                jwBeam.StartTelosType= beam.StartTelosType;
                jwBeam.EndTelosType= beam.EndTelosType;
                jwBeam.StartCenter = beam.StartCenter;
                jwBeam.EndCenter = beam.EndCenter;

                foreach(var p in beam.ZhuBlocks)
                {
                    JwBlock jb = new JwBlock();
                    var ps = new List<JWPoint>()
                    {
                        new JWPoint(p.TopLeft.Y, p.TopLeft.X),
                new JWPoint(p.TopRight.Y, p.TopRight.X),
                new JWPoint(p.BottomLeft.Y, p.BottomLeft.X),
                new JWPoint(p.BottomRight.Y, p.BottomRight.X),
                    };
                    jb.TopLeft = ps.OrderBy(t => t.X).ThenByDescending(t => t.Y).ToList().First();
                    jb.TopRight = ps.OrderByDescending(t => t.X).ThenByDescending(t => t.Y).ToList().First();
                    jb.BottomLeft = ps.OrderBy(t => t.X).ThenBy(t => t.Y).ToList().First();
                    jb.BottomRight = ps.OrderByDescending(t => t.X).ThenBy(t => t.Y).First();
                    jb.JisuanWidthHeight();
                    jwBeam.ZhuBlocks.Add(jb);
                }
                var plinks = beam.LinkParts.Where(t => t.BujianName == "BG").ToList();
                if(plinks?.Count > 0)
                {
                    foreach(var link in plinks) 
                    { 
                        JwLinkPart jlnk=new JwLinkPart();
                        jlnk.BjCenterPoint=new JWPoint(link.BjCenterPoint.Y, link.BjCenterPoint.X);
                        jwBeam.LinkParts.Add(jlnk);
                    }
                }

                if(beam.Holes?.Count > 0)
                {
                    foreach(var hole in beam.Holes)
                    {
                        JwHole jh = new JwHole
                        {
                            Location = new JWPoint(hole.Location.Y,hole.Location.X),
                            FirstCreateFrom = hole.FirstCreateFrom,
                            ChangeFrom = hole.ChangeFrom,
                            HasBottom = hole.HasBottom,
                            HasCenter = hole.HasCenter,
                            HasTop = hole.HasTop,
                            IsEnd = hole.IsEnd,
                            IsStart = hole.IsStart,
                            KongNum = hole.KongNum,
                            HasLocationCenter = hole.HasLocationCenter,
                            Id = hole.Id,
                            HoleType = hole.HoleType
                        };
                        if(jh.HasLocationCenter)
                        {
                            jh.LocationCenter=new JWPoint(hole.LocationCenter.Y,hole.LocationCenter.X);
                        }
                        jwBeam.Holes.Add(jh);
                        //JwKongZu newkz = new JwKongZu
                        //{
                        //    KongNum=hole
                        //};
                        //if (!hole.HasLocationCenter)
                        //{
                        //    JwHole jhh = new JwHole(new JWPoint(hole.Location.Y, hole.Location.X), hole.FirstCreateFrom, null, hole.IsStart, hole.IsEnd);

                        //    jwBeam.Holes.Add(jhh);
                        //}
                        //else
                        //{
                        //    JwHole jhh = new JwHole(new JWPoint(hole.Location.Y, hole.Location.X), hole.FirstCreateFrom, new JWPoint(hole.LocationCenter.Y, hole.LocationCenter.X), hole.IsStart, hole.IsEnd);

                        //    jwBeam.Holes.Add(jhh);
                        //}
                        
                    }
                }
                return jwBeam;
            }
            return AutoMapperHelper.GetInstance().GetMapper().Map<JwBeam>(beam);

            
        }


        public static JwBlock CreateNewBlock(JwBlock block)
        {
            JwBlock jwBlock = new JwBlock();
            jwBlock.TopLeft = new JWPoint(block.TopLeft.X,block.TopLeft.Y);
            jwBlock.TopRight = new JWPoint(block.TopRight.X, block.TopRight.Y);
            jwBlock.BottomLeft = new JWPoint(block.BottomLeft.X, block.BottomLeft.Y);
            jwBlock.BottomRight = new JWPoint(block.BottomRight.X, block.BottomRight.Y);
            jwBlock.JisuanWidthHeight();
            return jwBlock;
        }
    }
}
