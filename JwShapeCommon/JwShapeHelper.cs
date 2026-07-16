using JwCore;
using JwShapeCommon.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                            HoleType = hole.HoleType,
                            HasBhLinkHole=hole.HasBhLinkHole,
                            HasPreLinkHole=hole.HasPreLinkHole
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

        public static JwBeam QingxieToHorizontal(JwBeam beam)
        {
            JwBeam jwBeam = new JwBeam();
            jwBeam.Width = beam.Length;
            jwBeam.Height = beam.Height;

            double w2 = beam.Length / 2d;
            double h2 = beam.Height / 2d;

            double cx = beam.CenterPoint.X;
            double cy = beam.CenterPoint.Y;


            jwBeam.TopLeft = new JWPoint(cx - w2, cy - h2); // 左上
            jwBeam.TopRight = new JWPoint(cx + w2, cy - h2); // 右上
            jwBeam.BottomRight = new JWPoint(cx + w2, cy + h2); // 右下
            jwBeam.BottomLeft = new JWPoint(cx - w2, cy + h2);  // 左下

            jwBeam.CenterPoint = beam.CenterPoint;

            return jwBeam;
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

        public static Color GetColor(string letter)
        {
            if (string.IsNullOrEmpty(letter))
                return Color.Black;
            char firstLetter = letter[0];
            //return GetHighContrastColor(firstLetter);
            return GetLetterColor(firstLetter);
        }

        private const double GoldenAngle = 137.508; // 黄金角度

        public static Color GetLetterColor(char letter)
        {
            int index = char.ToUpper(letter) - 'A'; // 0..25

            int hueIndex = index % 13;      // 13 个色相
            int toneIndex = index / 13;     // 0 或 1，两档亮度

            double hue = hueIndex * (360.0 / 13.0); // 均匀分布 13 色
            double saturation = 0.80;

            // 两档亮度：一浅一深，但都不黑
            double lightness = (toneIndex == 0) ? 0.60 : 0.40;

            return FromHsl(hue, saturation, lightness);
        }

        public static Color GetHighContrastColor(char letter)
        {
            int index = char.ToUpper(letter) - 'A'; // A=0, B=1...

            // 使用黄金角度生成高对比色
            double hue = (index * GoldenAngle) % 360.0;

            double saturation = 0.85; // 高饱和度
            double lightness = 0.55;  // 中亮度，杜绝黑色

            return FromHsl(hue, saturation, lightness);
        }

        public static Color FromHsl(double h, double s, double l)
        {
            double c = (1 - Math.Abs(2 * l - 1)) * s;
            double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            double m = l - c / 2;

            double r = 0, g = 0, b = 0;

            if (h < 60) { r = c; g = x; }
            else if (h < 120) { r = x; g = c; }
            else if (h < 180) { g = c; b = x; }
            else if (h < 240) { g = x; b = c; }
            else if (h < 300) { r = x; b = c; }
            else { r = c; b = x; }

            return Color.FromArgb(
                (int)((r + m) * 255),
                (int)((g + m) * 255),
                (int)((b + m) * 255)
            );
        }
    }
}
