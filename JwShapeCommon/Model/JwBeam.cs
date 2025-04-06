using JwCore;
using JwShapeCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using Sunny.UI;

namespace JwShapeCommon
{

    /// <summary>
    /// BEAM 确定矩形 上边 下边 左边 右边
    /// </summary>
    public class JwBeam: JwSquareBase
    {
        /// <summary>
        /// 梁的方向
        /// </summary>
        //public string Id { get; set; }

        private List<JWPoint> points;

        public List<JwLinkPart> LinkParts { get; set; }

        public List<JwPillar> BeamPillars { get; set; }

        public List<JwBlock> ZhuBlocks { get; set; }

        public virtual string BeamCode { get; set; }

        public bool HasQiegeStart { get; set; }

        /// <summary>
        /// topleft
        /// </summary>
        public JWPoint? StartXinPoint { get; set; }

        public KongzuType StartTelosType { get; set; }

        public KongzuType EndTelosType { get; set; }

        public bool HasStartSide { get; set; }

        public bool HasEndSide { get; set; }

        public JwBeamSide? StartSide { get; set; }

        public JwBeamSide? EndSide { get; set; }


        public bool HasQiegeEnd { get; set; }

        /// <summary>
        ///  水平 为 topright 垂直为 bottomleft
        /// </summary>
        public JWPoint? EndXinPoint { get; set; }

        /// <summary>
        /// 中心起点值 水平的话 为X 垂直的 为Y  
        /// </summary>
        public double StartCenter { get; set; }

        /// <summary>
        /// 中心终点
        /// </summary>
        public double EndCenter { get; set; }

        /// <summary>
        /// 每个beam都有的中线的值 ，水平的为Y 垂直的为X
        /// beam 初始是可以判定宽度的  两个方面梁的固定宽度  和center一致暂时不需要
        /// </summary>
        public double CenterLine { get; set; }

        public double Length { get; set; }

        public double XXLength { get; set; }

        

        public JwBeam()
        {
            Id = Guid.NewGuid().ToString();
            LinkParts = new List<JwLinkPart>();
            BeamPillars = new List<JwPillar>();
            ZhuBlocks = new List<JwBlock>();
        }

        /// <summary>
        /// 由识别出的mian 生成beam梁
        /// </summary>
        /// <param name="mian"></param>
        public JwBeam(JWMian mian) 
        {
            Id=Guid.NewGuid().ToString();
            LinkParts = new List<JwLinkPart>();
            BeamPillars = new List<JwPillar>();
            ZhuBlocks = new List<JwBlock>();
            parseBymian(mian);
            desginScale();
        }

        public JwBeam(JWMian mian,bool isjiaodian)
        {
            Id = Guid.NewGuid().ToString();
            LinkParts = new List<JwLinkPart>();
            BeamPillars = new List<JwPillar>();
            ZhuBlocks = new List<JwBlock>();
            parseBymian(mian, isjiaodian);
            desginScale();
        }

        public JwBeam(JwBeam parenbeam,JWPoint start,JWPoint end)
        {
            Id= Guid.NewGuid().ToString();
            parenbeam.IsParentBeam = true;
            if (parenbeam.DirectionType == BeamDirectionType.Horizontal)
            {
                TopLeft = start;
                TopRight = end;
                BottomLeft =new JWPoint(start.X, parenbeam.BottomLeft.Y);
                BottomRight = new JWPoint(end.X, BottomLeft.Y);
            }
            else
            {
                TopLeft = start;
                BottomLeft = end;
                TopRight = new JWPoint(parenbeam.TopRight.X, start.Y);
                BottomRight = new JWPoint(parenbeam.TopRight.X, end.Y);
            }
            IsQiegeBeam = true;
            LinkParts = new List<JwLinkPart>();
            BeamPillars = new List<JwPillar>();
            ZhuBlocks=new List<JwBlock>();
            JisuanWidthHeight();
        }


        /// <summary>
        /// 链接间隙为6
        /// </summary>
        /// <param name="parenbeam"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="qiegestart">头是否为 切割  标记J端位置</param>
        /// <param name="qiegeen">end 是否为J端</param>
        public JwBeam(JwBeam parenbeam, JWPoint start, JWPoint end,bool qiegestart,bool qiegeend)
        {
            Id = Guid.NewGuid().ToString();
            parenbeam.IsParentBeam = true;
            List<JWPoint> qglst = new List<JWPoint>();

            if (parenbeam.DirectionType == BeamDirectionType.Horizontal)
            {
                if(qiegestart)
                {

                    TopLeft = new JWPoint(start.X+3/JwFileConsts.JwScale,start.Y);
                    
                    BottomLeft = new JWPoint(TopLeft.X, parenbeam.BottomLeft.Y);
                    
                    qglst.Add(new JWPoint(start.X, parenbeam.CenterPoint.Y));
                    StartTelosType = KongzuType.J;
                    HasStartSide = true;
                    StartXinPoint = new JWPoint(start.X, parenbeam.Center);
                    JWPoint kongzucenter = new JWPoint(start.X + JwFileConsts.Kongjing / (2*JwFileConsts.JwScale), parenbeam.CenterPoint.Y);
                    //this.AddAnyHole(kongzucenter, HoleCreateFrom.FengeJ,StartXinPoint,true,false);
                    this.AddAnyHole(kongzucenter, HoleCreateFrom.FengeJ, kongzucenter, true, false);
                    this.HasQiegeStart = true;
                    this.StartSide = new JwBeamSide
                    {
                        KongZu = this.Holes.Last(),
                        SideType = KongzuType.J
                    };
                    StartCenter = start.X;//中心点为切割点
                }
                else
                {
                    TopLeft = start;
                    BottomLeft = new JWPoint(TopLeft.X, parenbeam.BottomLeft.Y);
                    HasStartSide = true;
                    StartTelosType = parenbeam.StartTelosType;
                    StartCenter = parenbeam.StartCenter;//和父保持一致
                    var z = parenbeam.Holes.Find(t => t.IsStart);
                    if (z != null)
                    {
                        Holes.Add(z);
                    }
                    //hol
                }
                if(qiegeend)
                {
                    TopRight = new JWPoint(end.X - 3 / JwFileConsts.JwScale, end.Y);
                    BottomRight = new JWPoint(TopRight.X, BottomLeft.Y);
                    qglst.Add(new JWPoint(end.X, parenbeam.CenterPoint.Y));
                    EndTelosType = KongzuType.J;
                    HasEndSide = true;
                    EndXinPoint = new JWPoint( parenbeam.CenterPoint.X,end.Y);
                    JWPoint kongzucenter = new JWPoint(end.X - JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale), parenbeam.Center);
                    this.AddAnyHole(kongzucenter, HoleCreateFrom.FengeJ, kongzucenter, false, true);
                    //this.AddAnyHole(kongzucenter, HoleCreateFrom.FengeJ,EndXinPoint,false,true);
                    this.HasQiegeEnd = true;
                    this.EndSide = new JwBeamSide
                    {
                        KongZu = this.Holes.Last(),
                        SideType = KongzuType.J
                    };
                    EndCenter = end.X;//中心点及为切割点

                }
                else
                {
                    TopRight = end;
                    BottomRight = new JWPoint(TopRight.X, BottomLeft.Y);
                    HasEndSide = true;
                    EndTelosType = parenbeam.EndTelosType;
                    EndCenter = parenbeam.EndCenter;//和父保持一致
                    var lasth = parenbeam.Holes.Find(t => t.IsEnd);
                    if (lasth != null)
                    {
                        Holes.Add(lasth);
                    }
                }
            }
            else
            {
                if (qiegeend)
                {
                    TopLeft = new JWPoint(end.X, end.Y - 3 / JwFileConsts.JwScale);
                    TopRight = new JWPoint(parenbeam.TopRight.X, TopLeft.Y);
                    qglst.Add(new JWPoint(parenbeam.CenterPoint.X, end.Y));
                    EndTelosType = KongzuType.J;
                    EndXinPoint = new JWPoint(parenbeam.Center, end.Y);
                    JWPoint kongzucenter = new JWPoint(parenbeam.Center, end.Y - JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale));

                    this.AddAnyHole(kongzucenter, HoleCreateFrom.FengeJ, kongzucenter, false,true);
                    this.HasEndSide = true;
                    this.EndSide = new JwBeamSide
                    {
                        KongZu = this.Holes.Last(),
                        SideType = KongzuType.J
                    };
                    EndCenter = end.Y;//终点为切割点
                }
                else
                {
                    TopLeft = end;
                    TopRight = new JWPoint(parenbeam.TopRight.X, TopLeft.Y);
                    HasEndSide= true; 
                    EndTelosType = parenbeam.EndTelosType;
                    EndCenter = parenbeam.EndCenter;
                    var lasth = parenbeam.Holes.Find(t => t.IsEnd);
                    if (lasth != null)
                    {
                        Holes.Add(lasth);
                    }
                }
                if (qiegestart)
                {

                    BottomLeft = new JWPoint(start.X, start.Y + 3 / JwFileConsts.JwScale);
                    BottomRight = new JWPoint(TopRight.X, BottomLeft.Y);
                    qglst.Add(new JWPoint(parenbeam.CenterPoint.X, start.Y));
                    StartTelosType = KongzuType.J;
                    StartXinPoint = new JWPoint(parenbeam.CenterPoint.X, start.Y);
                    JWPoint kongzucenter = new JWPoint(parenbeam.CenterPoint.X, start.Y + JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale));
                    //this.AddAnyHole(kongzucenter, HoleCreateFrom.FengeJ,StartXinPoint,true,false);
                    this.AddAnyHole(kongzucenter, HoleCreateFrom.FengeJ, kongzucenter, true, false);
                    this.HasStartSide = true;
                    this.StartSide = new JwBeamSide
                    {
                        KongZu = this.Holes.Last(),
                        SideType = KongzuType.J
                    };
                    StartCenter = start.Y;
                }
                else
                {
                    BottomLeft = start;
                    BottomRight = new JWPoint(parenbeam.TopRight.X, start.Y);
                    HasEndSide= true;
                    StartTelosType = parenbeam.StartTelosType;
                    StartCenter = parenbeam.StartCenter;
                    var z = parenbeam.Holes.Find(t => t.IsStart);
                    if (z != null)
                    {
                        Holes.Add(z);
                    }
                }
                //TopLeft = start;
                //BottomLeft = end;
                //TopRight = new JWPoint(parenbeam.TopRight.X, start.Y);
                //BottomRight = new JWPoint(parenbeam.TopRight.X, end.Y);
                //if (qiegestart)
                //{
                //    TopLeft = new JWPoint(start.X,start.Y-3/JwFileConsts.JwScale);
                //    TopRight = new JWPoint(parenbeam.TopRight.X, TopLeft.Y);
                //    qglst.Add(new JWPoint(parenbeam.CenterPoint.X, start.Y));
                //    StartTelosType = KongzuType.J;
                //    StartXinPoint = new JWPoint(parenbeam.CenterPoint.X, start.Y);
                //    JWPoint kongzucenter = new JWPoint(parenbeam.CenterPoint.X, start.Y - JwFileConsts.Kongjing / 2);

                //    JwKongZu sside = new JwKongZu
                //    {
                //        KongNum = 2,
                //        SuoShuMian = KongzuSuoshuMian.All,
                //        Position = kongzucenter,
                //        BeamId = Id,
                //        Sourec = KongzuSuoshuMian.Center
                //    };
                //    this.AddHole(sside);
                //    this.HasStartSide = true;
                //    this.StartSide = new JwBeamSide
                //    {
                //        KongZu = sside,
                //        SideType = KongzuType.J
                //    };
                //}
                //else
                //{
                //    TopLeft = start;
                //    TopRight = new JWPoint(parenbeam.TopRight.X, TopLeft.Y);
                //    StartTelosType = KongzuType.B;
                //}
                //if (qiegeend)
                //{

                //    BottomLeft = new JWPoint(end.X, end.Y + 3 / JwFileConsts.JwScale);
                //    BottomRight = new JWPoint(TopRight.X, BottomLeft.Y);
                //    qglst.Add(new JWPoint(parenbeam.CenterPoint.X, end.Y));
                //    EndTelosType= KongzuType.J;
                //    EndXinPoint = new JWPoint(parenbeam.CenterPoint.X, end.Y);
                //    JWPoint kongzucenter = new JWPoint(parenbeam.CenterPoint.X, start.Y + JwFileConsts.Kongjing / 2);

                //    JwKongZu sside = new JwKongZu
                //    {
                //        KongNum = 2,
                //        SuoShuMian = KongzuSuoshuMian.All,
                //        Position = kongzucenter,
                //        BeamId = Id,
                //        Sourec = KongzuSuoshuMian.Center
                //    };
                //    this.AddHole(sside);
                //    this.HasEndSide = true;
                //    this.EndSide = new JwBeamSide
                //    {
                //        KongZu = sside,
                //        SideType = KongzuType.J
                //    };
                //}
                //else
                //{
                //    BottomLeft = end;
                //    BottomRight=new JWPoint(parenbeam.TopRight.X,end.Y);
                //    EndTelosType = KongzuType.B;
                //}
            }
            IsQiegeBeam = true;
            LinkParts = new List<JwLinkPart>();
            BeamPillars = new List<JwPillar>();
            ZhuBlocks = new List<JwBlock>();
            JisuanWidthHeight();
            foreach(var lk in parenbeam.LinkParts)
            {
                if (this.Contains(lk.BjCenterPoint))
                {
                    this.LinkParts.Add(lk);
                }
            }
            JwLinkPart jbb = new JwLinkPart();
            jbb.Directed = TaggDirect.Up;
            jbb.GouJianType = GouJianType.BG;
            jbb.BujianName = "BG";
            //jbb.BjCenterPoint = new JWPoint
            //{
            //    X=end.X+
            //}
            //{
            //    X = ,
            //    Y = Math.Round(l.Center, 2)
            //};
            //jbb.ParentBeam = l;
            //jbb.BeamId = l.Id;
            //jbb.BBeam = c;
            //var existbb = l.LinkParts.Where(t => t.BjCenterPoint == jbb.BjCenterPoint && t.Directed == TaggDirect.Up).ToList();

        }


        private void desginScale()
        {
            var w = Math.Round(this.Width, 2);
            var h=Math.Round(this.Height, 2);
            WidthScale = w * JwFileConsts.JwScale;
            HeightScale= h * JwFileConsts.JwScale;
        }
        
        /// <summary>
        /// first 水平垂直判断- 生成默认 起始中心属性
        /// 判断接触和切割的时候进行修改 起始中心属性
        /// </summary>
        /// <param name="mian"></param>
        private void parseBymian(JWMian mian)
        {
            points = new List<JWPoint>();
            foreach (var x in mian.Xians)
            {
                points.AddRange(x.GetXianPoints());
            }
            points = points.Distinct(new JwPointComparint()).ToList();

            TopLeft = points.OrderBy(t => t.X).ThenByDescending(t => t.Y).ToList().First();
            TopRight = points.OrderByDescending(t => t.X).ThenByDescending(t => t.Y).ToList().First();
            BottomLeft = points.OrderBy(t => t.X).ThenBy(t => t.Y).ToList().First();
            BottomRight = points.OrderByDescending(t => t.X).ThenBy(t => t.Y).First();
            JisuanWidthHeight();
            //默认起始 梁起 +50 终-50 /除去scale
            if (DirectionType == BeamDirectionType.Horizontal)
            {
                StartCenter = TopLeft.X + (50 / JwFileConsts.JwScale);
                EndCenter = TopRight.X - (50 / JwFileConsts.JwScale);
            }
            if(DirectionType == BeamDirectionType.Vertical) 
            {
                StartCenter = BottomLeft.Y + (50 / JwFileConsts.JwScale);
                EndCenter = TopLeft.Y - (50 / JwFileConsts.JwScale);
            }
        }

        /// <summary>
        /// jwmian里的point是经过极角排序后的
        /// </summary>
        /// <param name="mian"></param>
        /// <param name="frompoints"></param>
        private void parseBymian(JWMian mian,bool frompoints)
        {
            points = new List<JWPoint>();
            points = mian.Points;
            points = points.Distinct(new JwPointComparint()).ToList();

            TopLeft = points[0].Jiangjingdu();
            TopRight=points[1].Jiangjingdu();
            BottomRight=points[2].Jiangjingdu();
            BottomLeft = points[3].Jiangjingdu();
            CenterPoint = new JWPoint();
            CenterPoint.X = points.Average(t => t.X);
            //CenterPoint.X=Math.Round(CenterPoint.X,2);
            CenterPoint.Y = points.Average(t => t.Y);
            Jiaodu = mian.Jiaodu;
            //JisuanWidthHeight();
            if (mian.Jiaodu == 0 || mian.Jiaodu == 180)
            {
                Width = Math.Round(mian.Width * JwFileConsts.JwScale, JwFileConsts.JiangjingduInt - 2) / JwFileConsts.JwScale;
                Height = Math.Round(mian.Height * JwFileConsts.JwScale, JwFileConsts.JiangjingduInt - 2) / JwFileConsts.JwScale;
                DirectionType = BeamDirectionType.Horizontal;
                TopLeft = points.OrderBy(t => t.X).ThenByDescending(t => t.Y).ToList().First().Jiangjingdu();
                TopRight = points.OrderByDescending(t => t.X).ThenByDescending(t => t.Y).ToList().First().Jiangjingdu();
                BottomLeft = points.OrderBy(t => t.X).ThenBy(t => t.Y).ToList().First().Jiangjingdu();
                BottomRight = points.OrderByDescending(t => t.X).ThenBy(t => t.Y).First().Jiangjingdu();
                StartCenter = Math.Round((TopLeft.X + (50 / JwFileConsts.JwScale)) * JwFileConsts.JwScale, JwFileConsts.JiangjingduInt - 2) / JwFileConsts.JwScale;
                EndCenter = Math.Round((TopRight.X + (50 / JwFileConsts.JwScale)) * JwFileConsts.JwScale, JwFileConsts.JiangjingduInt - 2) / JwFileConsts.JwScale;
                Center = CenterPoint.Y;
            }
            //默认起始 梁起 +50 终-50 /除去scale
            else if (mian.Jiaodu == -90||mian.Jiaodu==90)
            {
                Height = Math.Round(mian.Width * JwFileConsts.JwScale, JwFileConsts.JiangjingduInt - 2) / JwFileConsts.JwScale;
                Width = Math.Round(mian.Height * JwFileConsts.JwScale, JwFileConsts.JiangjingduInt - 2) / JwFileConsts.JwScale;
                DirectionType = BeamDirectionType.Vertical;
                TopLeft = points.OrderBy(t => t.X).ThenByDescending(t => t.Y).ToList().First().Jiangjingdu();
                TopRight = points.OrderByDescending(t => t.X).ThenByDescending(t => t.Y).ToList().First().Jiangjingdu();
                BottomLeft = points.OrderBy(t => t.X).ThenBy(t => t.Y).ToList().First().Jiangjingdu();
                BottomRight = points.OrderByDescending(t => t.X).ThenBy(t => t.Y).First().Jiangjingdu();
                StartCenter = Math.Round((BottomLeft.Y + (50 / JwFileConsts.JwScale)) * JwFileConsts.JwScale, JwFileConsts.JiangjingduInt - 2) / JwFileConsts.JwScale;
                EndCenter = Math.Round((TopLeft.Y + (50 / JwFileConsts.JwScale)) * JwFileConsts.JwScale, JwFileConsts.JiangjingduInt - 2) / JwFileConsts.JwScale;
                Center = CenterPoint.X;
            }
            else
            {
                Width = Math.Round(mian.Width * JwFileConsts.JwScale, JwFileConsts.JiangjingduInt - 2) / JwFileConsts.JwScale;
                Height = Math.Round(mian.Height * JwFileConsts.JwScale, JwFileConsts.JiangjingduInt - 2) / JwFileConsts.JwScale;
                if (mian.Jiaodu < 0)
                {
                    DirectionType = BeamDirectionType.Youshang;
                }
                else
                {
                    DirectionType = BeamDirectionType.YouXia;
                }
            }
            
        }


        public void ChangeStartCenter()
        {
            if(DirectionType==BeamDirectionType.Horizontal)
            {
                TopLeft.X = StartCenter + (55 / JwFileConsts.JwScale);
                BottomLeft.X = StartCenter + (55 / JwFileConsts.JwScale);
            }
            if (DirectionType == BeamDirectionType.Vertical) {
                BottomLeft.Y = StartCenter + (55 / JwFileConsts.JwScale);
                BottomRight.Y = StartCenter + (55 / JwFileConsts.JwScale);
            }
            rejisuan();
        }

        public void ChangeEndCenter()
        {
            if (DirectionType == BeamDirectionType.Horizontal)
            {
                TopRight.X = EndCenter - (55 / JwFileConsts.JwScale);
                BottomRight.X = EndCenter - (55 / JwFileConsts.JwScale);
            }
            if (DirectionType == BeamDirectionType.Vertical)
            {
                TopLeft.Y = EndCenter - (55 / JwFileConsts.JwScale);
                TopRight.Y = EndCenter - (55 / JwFileConsts.JwScale);
            }
            rejisuan();
        }

        private void rejisuan()
        {
            Width = TopRight.X - TopLeft.X;
            Height = TopLeft.Y - BottomLeft.Y;
        }

        public bool HasQieGe=false;

        public int QieGeCount = 0;

        public List<JWPoint> QieGePoints = new List<JWPoint>();

        /// <summary>
        /// 是否为父beam  只有切割 才为True
        /// </summary>
        public bool IsParentBeam = false;

        /// <summary>
        /// 是否  是切割后的beam
        /// </summary>
        public bool IsQiegeBeam = false;

        public List<JwBeamVertical> Baifangs= new List<JwBeamVertical>();

        public List<JwKongZu> Kongzus=new List<JwKongZu>();

        /// <summary>
        /// 测试完了后 替代上面方法
        /// </summary>
        public List<JwHole> Holes=new List<JwHole>();

        public override string ToString()
        {
            //var z = Math.Round(Math.Max(Width, Height), 2) * JwFileConsts.JwScale;
            return  string.Format("{0}-length is {1}", BeamCode, Length);
        }

        public JwBeamData ToDbData()
        {
            JwBeamData data = new JwBeamData();
            data.Location = this.TopLeft?.ToPoint();
            data.Scale=JwFileConsts.JwScale;
            data.Width = this.Width;
            data.Height = this.Height;
            data.Length = this.Length;//Math.Max(WidthScale, HeightScale);//取长的
            data.DirectionType = this.DirectionType;
            data.BeamCode=this.BeamCode;
            data.HasQieGe=this.HasQieGe;
            data.QieGeCount=this.QieGeCount;
            data.IsParentBeam=this.IsParentBeam;
            data.IsQiegeBeam = this.IsQiegeBeam;
            data.XXLength=this.XXLength;
            data.HasEndSide=this.HasEndSide;
            data.HasStartSide=this.HasStartSide;
            data.StartTelosType=this.StartTelosType;
            data.EndTelosType=this.EndTelosType;
            return data;
        }

        /// <summary>
        /// 2025年4月5日
        /// 水平的为X 最左 垂直为Y 最上？下把 最小
        /// 增加方法 重置hole 相对距离
        /// </summary>
        public double AbsolutePD
        {
            get
            {
                return _absolutePd;
            }
            set
            {
                _absolutePd = value;
                holeorder();
            }
        }
        private double _absolutePd;

        private void holeorder()
        {
            if (this.Holes?.Count > 0)
            {
                foreach(var h in this.Holes)
                {
                    if (this.DirectionType == BeamDirectionType.Horizontal)
                    {
                        h.AbsoluteP = Math.Round(Math.Round(h.Location.X - _absolutePd, 6) * JwFileConsts.JwScale, 0);
                        
                    }
                    if (this.DirectionType == BeamDirectionType.Vertical)
                    {
                        h.AbsoluteP = Math.Round(Math.Round(h.Location.Y - _absolutePd, 6) * JwFileConsts.JwScale, 0);
                    }
                }
            }
        }
    }

    public static class JwBeamExtensions
    {
        public static List<JwBeam> BeamSplite(this JwBeam beam)
        {
            List<JwBeam> relst = new List<JwBeam>();
            if (beam.HasQieGe)
            {
                beam.IsParentBeam = true;
                if (beam.DirectionType == BeamDirectionType.Horizontal)
                {
                    //升序
                    beam.QieGePoints = beam.QieGePoints.OrderBy(t => t.X).ToList();
                    JWPoint startp = beam.TopLeft;
                    List<double> qieges=new List<double>();
                    for (int i = 0; i < beam.QieGePoints.Count; i++)
                    {
                        qieges.Add(beam.QieGePoints[i].X);
                        JWPoint endp = new JWPoint(beam.QieGePoints[i].X, beam.TopLeft.Y);
                        var nb = new JwBeam(beam, startp, endp, qieges.Contains(startp.X), qieges.Contains(endp.X));
                        int t = i + 1;
                        nb.BeamCode = beam.BeamCode + "-" + t.ToString();
                        relst.Add(nb);
                        startp = endp;
                    }
                    JWPoint enddp = beam.TopRight;
                    var edp = new JwBeam(beam, startp, enddp, true, false);
                    edp.BeamCode = beam.BeamCode + "-" + (beam.QieGePoints.Count + 1).ToString();
                    relst.Add(edp);
                }
                if (beam.DirectionType == BeamDirectionType.Vertical)
                {
                    //左上 和左下 自下向上
                    //升序
                    //beam.QieGePoints = beam.QieGePoints.OrderByDescending(t => t.Y).ToList();
                    beam.QieGePoints = beam.QieGePoints.OrderBy(t => t.Y).ToList();
                    //JWPoint startp = beam.TopLeft;
                    JWPoint startp = beam.BottomLeft;
                    List<double> qieges = new List<double>();
                    for (int i = 0; i < beam.QieGePoints.Count; i++)
                    {
                        qieges.Add(beam.QieGePoints[i].Y);
                        JWPoint endp = new JWPoint(beam.TopLeft.X, beam.QieGePoints[i].Y);
                        var nb = new JwBeam(beam, startp, endp, qieges.Contains(startp.Y), qieges.Contains(endp.Y));
                        int t = i + 1;
                        nb.BeamCode = beam.BeamCode + "-" + t.ToString();
                        relst.Add(nb);

                        startp = endp;
                    }
                    JWPoint enddp = beam.TopLeft;
                    var endbeam = new JwBeam(beam, startp, enddp, true, false);
                    endbeam.BeamCode = beam.BeamCode + "-" + (beam.QieGePoints.Count + 1).ToString();
                    relst.Add(endbeam);
                }
                //linkpart
                var qiegecenterpoints = new List<JWPoint>();
                foreach(var bp in  beam.QieGePoints)
                {
                    if (beam.DirectionType == BeamDirectionType.Horizontal)
                    {
                        var z = new JWPoint(bp.X, beam.Center);
                        qiegecenterpoints.Add(z);
                        var existbb = beam.LinkParts.Where(t => t.BjCenterPoint == z && t.Directed == TaggDirect.Up).ToList();
                        if(existbb.Count==0)
                        {
                            JwLinkPart jbb = new JwLinkPart();
                            jbb.Directed = TaggDirect.Up;
                            jbb.GouJianType = GouJianType.B;
                            jbb.BujianName = "B";
                            jbb.BjCenterPoint = z;
                            jbb.ParentBeam = beam;
                            jbb.BeamId = beam.Id;
                            jbb.IsLianjie = true;
                            beam.LinkParts.Add(jbb);
                            if (GlobalEvent.GetGlobalEvent().AddLinkPartEvent != null)
                            {
                                GlobalEvent.GetGlobalEvent().AddLinkPartEvent(beam, new AddLinkPartArgs { LinkPart = jbb }); 
                            }
                        }
                        else
                        {
                            existbb.ForEach(t=>t.IsLianjie=true);
                        }
                        var existbbdown = beam.LinkParts.Where(t => t.BjCenterPoint == z && t.Directed == TaggDirect.Down).ToList();
                        if (existbbdown.Count == 0)
                        {
                            JwLinkPart jbb = new JwLinkPart();
                            jbb.Directed = TaggDirect.Down;
                            jbb.GouJianType = GouJianType.B;
                            jbb.BujianName = "B";
                            jbb.BjCenterPoint = z;
                            jbb.ParentBeam = beam;
                            jbb.BeamId = beam.Id;
                            jbb.IsLianjie = true;
                            beam.LinkParts.Add(jbb);
                            if (GlobalEvent.GetGlobalEvent().AddLinkPartEvent != null)
                            {
                                GlobalEvent.GetGlobalEvent().AddLinkPartEvent(beam, new AddLinkPartArgs { LinkPart = jbb });
                            }
                        }
                        else
                        {
                            existbbdown.ForEach(t => t.IsLianjie = true);
                        }
                    }
                    else
                    {
                        var z = new JWPoint(beam.Center, bp.Y);
                        qiegecenterpoints.Add(new JWPoint(beam.CenterPoint.X, bp.Y));
                        var existbbleft = beam.LinkParts.Where(t => t.BjCenterPoint == z && t.Directed == TaggDirect.Left).ToList();
                        if (existbbleft.Count == 0)
                        {
                            JwLinkPart jbb = new JwLinkPart();
                            jbb.Directed = TaggDirect.Left;
                            jbb.GouJianType = GouJianType.B;
                            jbb.BujianName = "B";
                            jbb.BjCenterPoint = z;
                            jbb.ParentBeam = beam;
                            jbb.BeamId = beam.Id;
                            jbb.IsLianjie = true;
                            beam.LinkParts.Add(jbb);
                            if (GlobalEvent.GetGlobalEvent().AddLinkPartEvent != null)
                            {
                                GlobalEvent.GetGlobalEvent().AddLinkPartEvent(beam, new AddLinkPartArgs { LinkPart = jbb });
                            }
                        }
                        else
                        {
                            existbbleft.ForEach(t => t.IsLianjie = true);
                        }
                        var existbbright = beam.LinkParts.Where(t => t.BjCenterPoint == z && t.Directed == TaggDirect.Right).ToList();
                        if (existbbright.Count == 0)
                        {
                            JwLinkPart jbb = new JwLinkPart();
                            jbb.Directed = TaggDirect.Right;
                            jbb.GouJianType = GouJianType.B;
                            jbb.BujianName = "B";
                            jbb.BjCenterPoint = z;
                            jbb.ParentBeam = beam;
                            jbb.BeamId = beam.Id;
                            jbb.IsLianjie = true;
                            beam.LinkParts.Add(jbb);
                            if (GlobalEvent.GetGlobalEvent().AddLinkPartEvent != null)
                            {
                                GlobalEvent.GetGlobalEvent().AddLinkPartEvent(beam, new AddLinkPartArgs { LinkPart = jbb });
                            }
                        }
                        else
                        {
                            existbbright.ForEach(t => t.IsLianjie = true);
                        }
                    }
                }
                //foreach(var z in qiegecenterpoints)
                //{
                //    var existbb = beam.LinkParts.Where(t => t.BjCenterPoint == z && t.Directed == TaggDirect.Up).ToList();
                //}
            }
            
            return relst;
        }
    }

    public class FourPoints
    {
        /// <summary>
        /// 左上
        /// </summary>
        public JWPoint TopLeft { get; set; }

        /// <summary>
        /// 右上
        /// </summary>
        public JWPoint TopRight { get; set; }

        /// <summary>
        /// 左下
        /// </summary>
        public JWPoint BottomLeft { get; set; }

        /// <summary>
        /// 右下
        /// </summary>
        public JWPoint BottomRight { get; set; }

        public bool HasValue { get; set; }=false;
    }
}
