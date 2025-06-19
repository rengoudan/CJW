using JwCore;
using JwShapeCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwCanvas:JwSquareBase
    {
        public JwProjectSubData JwProjectSubData { get; set; }          

        ///// <summary>
        ///// 左上
        ///// </summary>
        //public JWPoint? TopLeft { get; set; }

        ///// <summary>
        ///// 右上
        ///// </summary>
        //public JWPoint? TopRight { get; set; }

        ///// <summary>
        ///// 左下
        ///// </summary>
        //public JWPoint? BottomLeft { get; set; }

        ///// <summary>
        ///// 右下
        ///// </summary>
        //public JWPoint? BottomRight { get; set; }

        //public JWPoint? CenterPoint { get; set; }


        //public double? Width { get; set; }
        //public double? Height { get; set; }

        public double? MinX { get; set; }

        public double? MaxY { get; set; }

        public List<JwBeam> Beams { get; set; }

        public List<JWPoint> Points { get; set; }


        public List<JwPillar> Pillars { get; set; }

        public List<JwBeam> ParentBeams { get; set; }

        public List<JwLinkPart> LinkParts { get; set; }

        public int BBCount { get; set; }

        public int BGCount { get; set; }

        public double Scale { get; set; }

        public int PillarCount { get; set; }

        public int SinglePillarCount { get; set; }

        public int KPillarCount { get; set; }

        public int HorizontalBeamsCount = 0;

        public int VerticalBeamsCount = 0;

        public List<JwBeam> HorizontalBeams { get; set; }

        public List<JwBeam> VerticalBeams { get; set; }

        public List<JwLianjieSingle> LianjieSingles = new List<JwLianjieSingle>();

        public JwCanvas() { }

        public JwCanvas(JWPoint? topLeft,JWPoint? topRight,JWPoint? bottomLeft,JWPoint? bottomRight, List<JwBeam> _beams, List<JWPoint> _points,double? width,double? height,List<JwPillar> pillars,List<JwBeam> _parentbeams) 
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
            Beams = _beams;
            Points = _points;
            Width = width.Value;
            Height= height.Value;
            MinX = TopLeft?.X;
            MaxY = TopLeft?.Y;
            var cx = topRight.X-(topRight.X - topLeft.X) / 2;
            var cy = topRight.Y - (topRight.Y - bottomRight.Y) / 2;
            CenterPoint=new JWPoint(cx, cy);
            Pillars = pillars;
            ParentBeams = _parentbeams;
        }

        public JwCanvas(JwProjectSubData data)
        {
            //JwCanvas jwCanvas

            if (data.BeamCount > 0)
            {
                foreach(var bm in data.JwBeamDatas)
                {
                    var jwbm = bm.DataToJw<JwBeam>();
                    jwbm.BeamCode = bm.BeamCode;
                    jwbm.HasQieGe = bm.HasQieGe;
                    jwbm.IsParentBeam = bm.IsParentBeam;
                    jwbm.IsQiegeBeam = bm.IsQiegeBeam;
                    jwbm.QieGeCount = bm.QieGeCount;
                    jwbm.Id= bm.Id;
                }
            }
            if(data.JwPillarDatas.Count> 0)
            {
                foreach (var bp in data.JwPillarDatas)
                {
                    var jwbm = bp.DataToJw<JwPillar>();
                    jwbm.PillarCode = bp.PillarCode;
                    jwbm.Id = bp.Id;
                    jwbm.BaseType = bp.BaseType;
                    jwbm.Blocks = new List<JwBlock>();
                    if(jwbm.BaseType==PillarBaseType.KPillar)
                    {
                        jwbm.Blocks.Add(bp.FirstLocation.DataToJwBlock(bp.FirstWidth.Value, bp.FirstHeight.Value, bp.Scale));
                        jwbm.Blocks.Add(bp.CenterLocation.DataToJwBlock(bp.CenterWidth.Value, bp.CenterHeight.Value, bp.Scale));
                        jwbm.Blocks.Add(bp.LastLocation.DataToJwBlock(bp.LastWidth.Value, bp.LastHeight.Value, bp.Scale));
                    }
                    else
                    {
                        jwbm.Blocks.Add(bp.Location.DataToJwBlock(bp.Width,bp.Height, bp.Scale));
                    }
                }
            }
            
        }
    }
}
