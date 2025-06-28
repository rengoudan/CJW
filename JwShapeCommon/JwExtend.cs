using Flurl.Util;
using JwCore;
using JwShapeCommon.Jwbase;
using JwShapeCommon.Model;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Converters;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public static class JwExtend
    {

        public static FourPoints GetLeftTopRightBottom(this List<JWPoint> lst)
        {
            if (lst.Count > 0)
            {
                FourPoints z = new FourPoints();
                z.HasValue = true;
                var lefttop = lst.OrderBy(t => t.X).ToList().OrderByDescending(t => t.Y).ToList().First();

                var rigthtop = lst.OrderByDescending(t => t.X).ToList().OrderByDescending(t => t.Y).First();

                var leftbottom = lst.OrderBy(t => t.X).ToList().OrderBy(t => t.Y).ToList().First();

                var rightbottom = lst.OrderByDescending(t => t.X).ToList().OrderBy(t => t.Y).ToList().First();
                z.TopLeft = lefttop;
                z.TopRight = rigthtop;
                z.BottomLeft = leftbottom;
                z.BottomRight = rightbottom;
                return z;
            }
            else
            {
                return new FourPoints();
            }

        }

        public static List<JWPoint> GetLinesPoints(this List<JwXian> lst)
        {
            List<JWPoint> points = new List<JWPoint>();
            if (lst.Count > 0)
            {
                foreach (var line in lst)
                {
                    points = points.Union(line.GetXianPoints(), new JwPointComparint()).ToList();
                }
                return points;
            }
            else
            {
                return points;
            }
        }

        /// <summary>
        /// 通用jwsquarebase 转对应数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DataToJw<T>(this JwSquareBaseData data) where T : JwSquareBase
        {
            T reobj = System.Activator.CreateInstance<T>();
            reobj.TopLeft = new JWPoint(data.Location!.X, data.Location!.Y);
            reobj.TopRight = new JWPoint(data.Location!.X + data.Width, data.Location!.Y);
            reobj.BottomLeft = new JWPoint(data.Location!.X, data.Location!.Y - data.Height);
            reobj.BottomRight = new JWPoint(reobj.TopRight.X, reobj.BottomLeft.Y);
            reobj.DirectionType = data.DirectionType;
            reobj.CenterPoint = new JWPoint(data.Location!.X + data.Width / 2, data.Location!.Y - data.Height / 2);
            reobj.Height = data.Height;
            reobj.Width = data.Width;
            reobj.Center = data.DirectionType == BeamDirectionType.Horizontal ? reobj.CenterPoint.Y : reobj.CenterPoint.X;
            reobj.HeightScale = data.Scale * Math.Round(data.Height, 2);
            reobj.WidthScale = data.Scale * Math.Round(data.Width, 2);

            return reobj;
        }

        public static JwBeamVertical DataToJw(this JwBeamVerticalData data)
        {
            JwBeamVertical vertical = new JwBeamVertical
            {
                ParentBeamId=data.JwBeamDataId,
                Center = data.Center,
                HasLast = data.HasLast,
                HasPre = data.HasPre
            };
            return vertical;

        }

        public static JwLianjie DataToLianjie(this JwLianjieData data)
        {
            JwLianjie lianjie = new JwLianjie();
            lianjie.Start=new JWPoint(data.Start.X, data.Start.Y);
            lianjie.End=new JWPoint(data.End.X, data.End.Y);
            lianjie.Length = data.Length;
            lianjie.Id=data.Id;
            //lianjie.
            return lianjie;
        }

        public static JwHole DataToHole(this JwHoleData data)
        {
            JwHole hole = new JwHole
            {
                HasBottom = data.HasBottom,
                HasCenter = data.HasCenter,
                HasTop = data.HasTop,
                HasLocationCenter = false,
                FirstCreateFrom = data.FirstCreateFrom,
                ChangeFrom = data.ChangeFrom,
                IsEnd = data.IsEnd,
                HasPreLinkHole = data.HasPreLinkHole,
                HasBhLinkHole = data.HasBhLinkHole,
                IsStart = data.IsStart,
                HoleType = data.HoleType,
                Id = data.Id,
                KongNum = data.KongNum,
                Location = new JWPoint(data.Location.X, data.Location.Y)
            };
            if (data.HasBhLinkHole)
            {
                int i = 0;
            }
            return hole;
        }

        /// <summary>
        /// block 转数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static JwBlock DataToJwBlock(this Point data, double width, double height, double scale)
        {
            JwBlock reobj = new JwBlock();
            reobj.TopLeft = new JWPoint(data.X, data.Y);
            reobj.TopRight = new JWPoint(data!.X + width, data!.Y);
            reobj.BottomLeft = new JWPoint(data!.X, data!.Y - height);
            reobj.BottomRight = new JWPoint(reobj.TopRight.X, reobj.BottomLeft.Y);
            //reobj.DirectionType = data.DirectionType;
            reobj.CenterPoint = new JWPoint(data.X + width / 2, data.Y - height / 2);
            reobj.Height = height;
            reobj.Width = width;

            reobj.HeightScale = scale * Math.Round(height, 2);
            reobj.WidthScale = scale * Math.Round(width, 2);
            if (reobj.HeightScale == reobj.WidthScale)
            {
                reobj.Iszhengfangxing = true;
            }

            reobj.DirectionType = width >= height ? BeamDirectionType.Horizontal : BeamDirectionType.Vertical;
            reobj.Center = reobj.DirectionType == BeamDirectionType.Horizontal ? reobj.CenterPoint.Y : reobj.CenterPoint.X;

            return reobj;
        }

        /// <summary>
        /// 数据转canvas
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JwCanvas DataToCanvas(this JwProjectSubData data)
        {
            JwCanvas reobj = new JwCanvas();
            reobj.TopLeft = new JWPoint(data.Location!.X, data.Location!.Y);
            reobj.TopRight = new JWPoint(data.Location!.X + data.Width, data.Location!.Y);
            reobj.BottomLeft = new JWPoint(data.Location!.X, data.Location!.Y - data.Height);
            reobj.BottomRight = new JWPoint(reobj.TopRight.X, reobj.BottomLeft.Y);
            reobj.DirectionType = data.DirectionType;
            reobj.CenterPoint = new JWPoint(data.Location!.X + data.Width / 2, data.Location!.Y - data.Height / 2);
            reobj.Height = data.Height;
            reobj.Width = data.Width;
            reobj.Center = data.DirectionType == BeamDirectionType.Horizontal ? reobj.CenterPoint.Y : reobj.CenterPoint.X;
            reobj.HeightScale = data.Scale * Math.Round(data.Height, 2);
            reobj.WidthScale = data.Scale * Math.Round(data.Width, 2);
            reobj.BBCount = data.BCount;
            reobj.BGCount = data.BGCount;
            reobj.PillarCount = data.PillarCount;
            reobj.KPillarCount = data.KPillarCount;
            reobj.SinglePillarCount = data.SinglePillarCount;
            reobj.Scale = string.IsNullOrEmpty(data.Biaochi) ? 0 : Convert.ToDouble(data.Biaochi);

            reobj.HorizontalBeamsCount = data.HorizontalBeamsCount;
            reobj.VerticalBeamsCount = data.VerticalBeamsCount;

            reobj.Beams = new List<JwBeam>();
            if (data.BeamCount > 0)
            {
                foreach (var bm in data.JwBeamDatas)
                {
                    var jwbm = bm.DataToJw<JwBeam>();
                    jwbm.BeamCode = bm.BeamCode;
                    jwbm.HasQieGe = bm.HasQieGe;
                    jwbm.IsParentBeam = bm.IsParentBeam;
                    jwbm.IsQiegeBeam = bm.IsQiegeBeam;
                    jwbm.QieGeCount = bm.QieGeCount;
                    jwbm.Id = bm.Id;
                    jwbm.XXLength = bm.XXLength;
                    jwbm.Length = bm.Length;
                    jwbm.HasStartSide = bm.HasStartSide;
                    jwbm.HasEndSide = bm.HasEndSide;
                    jwbm.StartTelosType = bm.StartTelosType;
                    jwbm.EndTelosType = bm.EndTelosType;
                    jwbm.StartCenter = bm.StartCenter;
                    jwbm.EndCenter = bm.EndCenter;
                    //jwbm.HasEndSide=bm.has
                    if (bm.JwHoles.Count > 0)
                    {
                        foreach (var hb in bm.JwHoles)
                        {
                            jwbm.Holes.Add(hb.DataToHole());
                        }
                    }
                    if(bm.JwBeamVerticalDatas.Count> 0)
                    {
                        foreach(var  vb in bm.JwBeamVerticalDatas)
                        {
                            jwbm.Baifangs.Add(vb.DataToJw());
                        }
                    }

                    reobj.Beams.Add(jwbm);
                }
            }
            if (reobj.Beams.Count > 0)
            {
                reobj.HorizontalBeams = reobj.Beams.Where(t => t.DirectionType == BeamDirectionType.Horizontal).ToList();
                reobj.VerticalBeams = reobj.Beams.Where(t => t.DirectionType == BeamDirectionType.Vertical).ToList();
            }
            reobj.Pillars = new List<JwPillar>();
            if (data.JwPillarDatas.Count > 0)
            {
                foreach (var bp in data.JwPillarDatas)
                {
                    var jwbm = bp.DataToJw<JwPillar>();
                    jwbm.PillarCode = bp.PillarCode;
                    jwbm.Id = bp.Id;
                    jwbm.BaseType = bp.BaseType;
                    jwbm.Blocks = new List<JwBlock>();
                    if (jwbm.BaseType == PillarBaseType.KPillar)
                    {
                        jwbm.Blocks.Add(bp.FirstLocation.DataToJwBlock(bp.FirstWidth.Value, bp.FirstHeight.Value, bp.Scale));
                        jwbm.Blocks.Add(bp.CenterLocation.DataToJwBlock(bp.CenterWidth.Value, bp.CenterHeight.Value, bp.Scale));
                        jwbm.Blocks.Add(bp.LastLocation.DataToJwBlock(bp.LastWidth.Value, bp.LastHeight.Value, bp.Scale));
                    }
                    else
                    {
                        jwbm.Blocks.Add(bp.Location.DataToJwBlock(bp.Width, bp.Height, bp.Scale));
                    }
                    reobj.Pillars.Add(jwbm);
                }
            }
            reobj.LinkParts = new List<JwLinkPart>();
            if (data.JwLinkPartDatas.Count > 0)
            {
                foreach (var lp in data.JwLinkPartDatas)
                {
                    JwLinkPart jwLink = new JwLinkPart();
                    jwLink.BjCenterPoint = new JWPoint(lp.Location!.X, lp.Location!.Y);
                    jwLink.IsLianjie = lp.IsLianjie;
                    jwLink.BujianName = lp.BujianName;
                    jwLink.BeamId = lp.BeamId;
                    jwLink.Directed = lp.Directed;
                    jwLink.GouJianType = lp.GouJianType;
                    jwLink.IsNoBeam = lp.IsNoBeam;
                    jwLink.Id = lp.Id;
                    reobj.LinkParts.Add(jwLink);
                }
            }
            
            if(data.JwLianjieDatas.Count > 0)
            {
                foreach(var lianjie in data.JwLianjieDatas)
                {
                    JwLianjie jlj = lianjie.DataToLianjie();
                    reobj.LianjieLsts.Add(jlj);
                }
            }
            reobj.IsFromData= true;
            return reobj;
        }

        /// <summary>
        /// 增加孔
        /// </summary>
        /// <param name="beam"></param>
        /// <param name="hole"></param>
        public static void AddHole(this JwBeam beam, JwKongZu hole)
        {
            if (beam.DirectionType == BeamDirectionType.Horizontal)
            {
                beam.Kongzus.Add(hole);
                beam.Kongzus = beam.Kongzus.OrderBy(t => t.Position.X).ToList();
                double sp = beam.TopLeft.X;
                double per = beam.TopLeft.X;
                foreach (var k in beam.Kongzus)
                {
                    k.StartDistance = k.Position.X - sp;
                    k.PreDistance = k.Position.X - per;
                    per = k.Position.X;
                }
            }
            else
            {
                beam.Kongzus.Add(hole);
                beam.Kongzus = beam.Kongzus.OrderBy(t => t.Position.Y).ToList();
                double sp = beam.TopLeft.Y;
                double per = beam.TopLeft.Y;
                foreach (var k in beam.Kongzus)
                {
                    k.StartDistance = k.Position.Y - sp;
                    k.PreDistance = k.Position.Y - per;
                    per = k.Position.Y;
                }
            }
        }

        public static void AddAnyHole(this JwBeam beam, JWPoint location, HoleCreateFrom createFrom, JWPoint? locationcenter = null, bool isStart = false, bool isEnd = false)
        {
            JwHole hh;
            if (beam.Holes.Count > 0)
            {
                var fh = beam.Holes.Find(t => t.Location == location);
                if (fh == null)
                {
                     hh = new JwHole(location, createFrom, locationcenter, isStart, isEnd);
                    if (beam.DirectionType == BeamDirectionType.Horizontal)
                    {
                        hh.HoleCenter = location.X; 
                    }
                    if (beam.DirectionType == BeamDirectionType.Vertical)
                    {
                        hh.HoleCenter = location.Y;
                    }
                    beam.Holes.Add(hh);
                }
                else
                {
                    if (createFrom == HoleCreateFrom.FengeJ)
                    {
                         hh = new JwHole(location, createFrom, locationcenter, isStart, isEnd);
                        if (beam.DirectionType == BeamDirectionType.Horizontal)
                        {
                            hh.HoleCenter = location.X;
                        }
                        if (beam.DirectionType == BeamDirectionType.Vertical)
                        {
                            hh.HoleCenter = location.Y;
                        }
                        //beam.Holes.Add(hh);
                        fh.changeByOther(HoleCreateFrom.FengeJ);
                    }
                    else
                    {
                        fh.changeByOther(HoleCreateFrom.Pillar);
                    }
                }
            }
            else
            {
                hh = new JwHole(location, createFrom, locationcenter, isStart, isEnd);
                if (beam.DirectionType == BeamDirectionType.Horizontal)
                {
                    hh.HoleCenter = location.X;
                }
                if (beam.DirectionType == BeamDirectionType.Vertical)
                {
                    hh.HoleCenter = location.Y;
                }
                beam.Holes.Add(hh);
            }
            //if (createFrom == HoleCreateFrom.JieChu)
            //{
            //    if (hh != null)
            //    {
            //        hh.HasSG = true;
            //    }
                
            //}
        }

        /// <summary>
        /// 2025年6月24日 返回jwhole 
        /// </summary>
        /// <param name="beam"></param>
        /// <param name="location"></param>
        /// <param name="createFrom"></param>
        /// <param name="locationcenter"></param>
        /// <param name="isStart"></param>
        /// <param name="isEnd"></param>
        public static JwHole AddAnyHoleReturn(this JwBeam beam, JWPoint location, HoleCreateFrom createFrom, JWPoint? locationcenter = null, bool isStart = false, bool isEnd = false)
        {
            JwHole hh=new JwHole();
            if (beam.Holes.Count > 0)
            {
                var fh = beam.Holes.Find(t => t.Location == location);
                if (fh == null)
                {
                    hh = new JwHole(location, createFrom, locationcenter, isStart, isEnd);
                    if (beam.DirectionType == BeamDirectionType.Horizontal)
                    {
                        hh.HoleCenter = location.X;
                    }
                    if (beam.DirectionType == BeamDirectionType.Vertical)
                    {
                        hh.HoleCenter = location.Y;
                    }
                    beam.Holes.Add(hh);
                }
                else
                {
                    if (createFrom == HoleCreateFrom.FengeJ)
                    {
                        hh = new JwHole(location, createFrom, locationcenter, isStart, isEnd);
                        if (beam.DirectionType == BeamDirectionType.Horizontal)
                        {
                            hh.HoleCenter = location.X;
                        }
                        if (beam.DirectionType == BeamDirectionType.Vertical)
                        {
                            hh.HoleCenter = location.Y;
                        }
                        //beam.Holes.Add(hh);
                        fh.changeByOther(HoleCreateFrom.FengeJ);
                    }
                    else
                    {
                        fh.changeByOther(HoleCreateFrom.Pillar);
                    }
                }
            }
            else
            {
                hh = new JwHole(location, createFrom, locationcenter, isStart, isEnd);
                if (beam.DirectionType == BeamDirectionType.Horizontal)
                {
                    hh.HoleCenter = location.X;
                }
                if (beam.DirectionType == BeamDirectionType.Vertical)
                {
                    hh.HoleCenter = location.Y;
                }
                beam.Holes.Add(hh);
            }
            return hh;
        }


        /// <summary>
        /// 2025年4月25日 针对J 记录偏差位置 方便查询下方打连接孔
        /// </summary>
        /// <param name="beam"></param>
        /// <param name="location"></param>
        /// <param name="createFrom"></param>
        /// <param name="locationcenter"></param>
        /// <param name="isStart"></param>
        /// <param name="isEnd"></param>
        public static void AddAnyHole(this JwBeam beam, JWPoint location, double pccenter = 0, bool isStart = false, bool isEnd = false)
        {
            JwHole hh;
            if (beam.Holes.Count > 0)
            {
                var fh = beam.Holes.Find(t => t.Location == location);
                if (fh == null)
                {
                    hh = new JwHole(location, HoleCreateFrom.FengeJ, location, isStart, isEnd);
                    //if (beam.DirectionType == BeamDirectionType.Horizontal)
                    //{
                    //    hh.HoleCenter = location.X;
                    //}
                    //if (beam.DirectionType == BeamDirectionType.Vertical)
                    //{
                    //    hh.HoleCenter = location.Y;
                    //}
                    hh.HoleCenter = pccenter;//2025年6月24日 根据此判断
                    beam.Holes.Add(hh);
                }
                else
                {

                    hh = new JwHole(location, HoleCreateFrom.FengeJ, location, isStart, isEnd);
                    //if (beam.DirectionType == BeamDirectionType.Horizontal)
                    //{
                    //    hh.HoleCenter = location.X;
                    //}
                    //if (beam.DirectionType == BeamDirectionType.Vertical)
                    //{
                    //    hh.HoleCenter = location.Y;
                    //}
                    hh.HoleCenter = pccenter;
                    //beam.Holes.Add(hh);
                    fh.changeByOther(HoleCreateFrom.FengeJ);

                }
            }
            else
            {
                hh = new JwHole(location, HoleCreateFrom.FengeJ, location, isStart, isEnd);
                //if (beam.DirectionType == BeamDirectionType.Horizontal)
                //{
                //    hh.HoleCenter = location.X;
                //}
                //if (beam.DirectionType == BeamDirectionType.Vertical)
                //{
                //    hh.HoleCenter = location.Y;
                //}
                hh.HoleCenter = pccenter;
                beam.Holes.Add(hh);
            }

        }


        public static void AddAnyHole(this JwBeam beam, JwKongZu hole, HoleCreateFrom createFrom)
        {
            if (beam.Holes.Count > 0)
            {
                var fh = beam.Holes.Find(t => t.Location == hole.Position);
                if (fh == null)
                {
                    //var hh = new JwHole(hole, createFrom);
                    //beam.Holes.Add(hh);
                }
                else
                {
                    fh.changeByOther(hole, HoleCreateFrom.Pillar);
                    //switch (createFrom)
                    //{
                    //    case HoleCreateFrom.Pillar:
                    //        {
                    //            fh.changeByOther(hole, HoleCreateFrom.Pillar);
                    //            if (!fh.HasTop)
                    //            {
                    //                fh.TopKongzu = hole;
                    //                fh.HasTop = true;
                    //            }
                    //        }
                    //        break;
                    //    case HoleCreateFrom.JieChuG:
                    //        if (!fh.HasCenter)
                    //        {
                    //            fh.CenterKongzu = hole;
                    //            fh.HasCenter = true;
                    //        }
                    //        break;
                    //    case HoleCreateFrom.FengeJ:
                    //        if (!fh.HasCenter)
                    //        {
                    //            fh.CenterKongzu = hole;
                    //            fh.HasCenter = true;
                    //        }
                    //        break;
                    //}
                }
            }
            else
            {
                //var hh = new JwHole(hole, createFrom);
                //beam.Holes.Add(hh);
            }
        }

        public static void AddAnyHole(this JwBeam beam, JwKongZu hole, HoleCreateFrom createFrom, JWPoint lc)
        {
            //if (beam.DirectionType == BeamDirectionType.Horizontal)
            //{
            //    beam.Kongzus.Add(hole);
            //    beam.Kongzus = beam.Kongzus.OrderBy(t => t.Position.X).ToList();
            //    double sp = beam.TopLeft.X;
            //    double per = beam.TopLeft.X;
            //    foreach (var k in beam.Kongzus)
            //    {
            //        k.StartDistance = k.Position.X - sp;
            //        k.PreDistance = k.Position.X - per;
            //        per = k.Position.X;
            //    }
            //}
            //else
            //{
            //    beam.Kongzus.Add(hole);
            //    beam.Kongzus = beam.Kongzus.OrderBy(t => t.Position.Y).ToList();
            //    double sp = beam.TopLeft.Y;
            //    double per = beam.TopLeft.Y;
            //    foreach (var k in beam.Kongzus)
            //    {
            //        k.StartDistance = k.Position.Y - sp;
            //        k.PreDistance = k.Position.Y - per;
            //        per = k.Position.Y;
            //    }
            //}
            if (beam.Holes.Count > 0)
            {
                var fh = beam.Holes.Find(t => t.Location == hole.Position);
                if (fh == null)
                {
                    var hh = new JwHole(hole, createFrom, lc);
                    beam.Holes.Add(hh);
                }
                else
                {
                    fh.changeByOther(hole, HoleCreateFrom.Pillar);
                    //switch (createFrom)
                    //{
                    //    case HoleCreateFrom.Pillar:
                    //        {
                    //            fh.changeByOther(hole, HoleCreateFrom.Pillar);
                    //            if (!fh.HasTop)
                    //            {
                    //                fh.TopKongzu = hole;
                    //                fh.HasTop = true;
                    //            }
                    //        }
                    //        break;
                    //    case HoleCreateFrom.JieChuG:
                    //        if (!fh.HasCenter)
                    //        {
                    //            fh.CenterKongzu = hole;
                    //            fh.HasCenter = true;
                    //        }
                    //        break;
                    //    case HoleCreateFrom.FengeJ:
                    //        if (!fh.HasCenter)
                    //        {
                    //            fh.CenterKongzu = hole;
                    //            fh.HasCenter = true;
                    //        }
                    //        break;
                    //}
                }
            }
            else
            {
                var hh = new JwHole(hole, createFrom, lc);
                beam.Holes.Add(hh);
            }
        }

        public static List<JwComboData> CreateBindList<T>()
        {
            List<JwComboData> lst = new List<JwComboData>();
            Type type = typeof(T);
            var vs = Enum.GetValues(type);
            var ns = Enum.GetNames(type);
            foreach (var it in vs)
            {
                JwComboData data = new JwComboData();
                data.Name = Enum.GetName(type, it);
                data.Value = Convert.ToInt32(it);
                lst.Add(data);
            }
            return lst;
        }

        /// <summary>
        /// 两个线比较 判定是否有重叠部分 有的化返回合并后的线
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static OverlappingXian LineOverLapping(this JwXian left, JwXian right)
        {
            OverlappingXian overlapping = new OverlappingXian
            {
                IsOverLapping = false
            };
            double startx = 0;
            double endx = 0;
            double starty = 0;
            double endy = 0;
            if (left.DirectionType == right.DirectionType)
            {
                if (left.DirectionType == BeamDirectionType.Horizontal)
                {

                    if (right.Pone.Y != left.Pone.Y)
                    {
                        overlapping.IsOverLapping = false;
                    }
                    else
                    {
                        starty = endy = left.Pone.Y;
                        startx = Math.Min(left.MinX(), right.MinX());
                        endx = Math.Max(left.MaxX(), right.MaxX());
                        if ((right.Pone.X >= left.Pone.X && right.Pone.X <= left.Ptwo.X) || ((right.Ptwo.X >= left.Pone.X && right.Ptwo.X <= left.Ptwo.X)))
                        {
                            overlapping.IsOverLapping = true;
                            left.IsMerge = true;
                            right.IsMerge = true;

                        }
                        else if ((right.Pone.X <= left.Pone.X && right.Ptwo.X >= left.Ptwo.X))
                        {
                            overlapping.IsOverLapping = true;
                            left.IsMerge = true;
                            right.IsMerge = true;
                        }
                        else
                        {
                            overlapping.IsOverLapping = false;
                        }
                    }
                }
                else
                {
                    if (right.Pone.X != left.Pone.X)
                    {
                        overlapping.IsOverLapping = false;
                    }
                    else
                    {
                        startx = endx = left.Pone.X;
                        starty = Math.Min(left.MinY(), right.MinY());
                        endy = Math.Max(left.MaxY(), right.MaxY());
                        if ((right.Pone.Y <= left.Pone.Y && right.Pone.Y >= left.Ptwo.Y) || ((right.Ptwo.Y <= left.Pone.Y && right.Ptwo.Y >= left.Ptwo.Y)))
                        {
                            overlapping.IsOverLapping = true;
                            left.IsMerge = true;
                            right.IsMerge = true;
                        }
                        else if (right.Pone.Y >= left.Pone.Y && right.Ptwo.Y <= left.Ptwo.Y)
                        {
                            overlapping.IsOverLapping = true;
                            left.IsMerge = true;
                            right.IsMerge = true;
                        }
                        else
                        {
                            overlapping.IsOverLapping = false;
                        }
                    }
                }
            }
            else
            {
                overlapping.IsOverLapping = false;
            }
            var startp = new JWPoint(startx, starty);
            var endp = new JWPoint(endx, endy);
            overlapping.MergeLine = new JwXian(startp, endp);
            return overlapping;
        }

        /// <summary>
        /// 判断相交的焦点
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public static JWPoint? Intersect(JwXian l1, JwXian l2)
        {
            double a1 = l1.Ptwo.Y - l1.Pone.Y;
            double b1 = l1.Pone.X - l1.Ptwo.X;
            double c1 = a1 * l1.Pone.X + b1 * l1.Pone.Y;

            double a2 = l2.Ptwo.Y - l2.Pone.Y;
            double b2 = l2.Pone.X - l2.Ptwo.X;
            double c2 = a2 * l2.Pone.X + b2 * l2.Pone.Y;

            double delta = a1 * b2 - a2 * b1;
            if (delta == 0)
            {
                return null; // 平行或重合
            }
            double x = (b2 * c1 - b1 * c2) / delta;
            double y = (a1 * c2 - a2 * c1) / delta;
            if (IsBetween(l1.Pone, l1.Ptwo, new JWPoint(x, y)) && IsBetween(l2.Pone, l2.Ptwo, new JWPoint(x, y)))
            {
                return new JWPoint(x, y);
            }

            return new JWPoint(x, y);
        }


       

        /// <summary>
        /// 判断四个点是否组成矩形
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <returns></returns>
        public static bool IsRectangle(JWPoint p1, JWPoint p2, JWPoint p3, JWPoint p4)
        {
            double d2 = Distance(p1, p2);
            double d3 = Distance(p1, p3);
            double d4 = Distance(p1, p4);

            if (d2 == d3 && 2 * d2 == d4 && Distance(p2, p4) == Distance(p3, p4))
            {
                return true;
            }
            if (d2 == d4 && 2 * d2 == d3 && Distance(p2, p3) == Distance(p3, p4))
            {
                return true;
            }
            if (d3 == d4 && 2 * d3 == d2 && Distance(p2, p3) == Distance(p2, p4))
            {
                return true;
            }
            return false;
        }

        public static bool IsBetween(JWPoint start, JWPoint end, JWPoint p)
        {
            return (Math.Min(start.X, end.X) <= p.X && p.X <= Math.Max(start.X, end.X)) &&
                   (Math.Min(start.Y, end.Y) <= p.Y && p.Y <= Math.Max(start.Y, end.Y));
        }

        public static double Distance(JWPoint p1, JWPoint p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static double Distancewithscale(JWPoint p1, JWPoint p2)
        {
            return Math.Sqrt(Math.Pow(p1.X * JwFileConsts.JwScale - p2.X * JwFileConsts.JwScale, 2) + Math.Pow(p1.Y * JwFileConsts.JwScale - p2.Y + JwFileConsts.JwScale, 2));
        }

        // 判断点是否在直线上
        public static bool LineContainsPoint(JwXian line, JWPoint point)
        {
            double minX = Math.Min(line.Pone.X, line.Ptwo.X);
            double maxX = Math.Max(line.Pone.X, line.Ptwo.X);
            double minY = Math.Min(line.Pone.Y, line.Ptwo.Y);
            double maxY = Math.Max(line.Pone.Y, line.Ptwo.Y);
            return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
        }
    }
    }
