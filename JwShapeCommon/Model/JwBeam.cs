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
using static System.Windows.Forms.AxHost;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Primitives;

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
            StartTelosType = KongzuType.B;
            EndTelosType = KongzuType.B;
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
            double wc = 30 / JwFileConsts.JwScale;
            Id = Guid.NewGuid().ToString();
            parenbeam.IsParentBeam = true;
            List<JWPoint> qglst = new List<JWPoint>();

            bool isdengyustart = false;
            bool isdengyuend = false;
            List<JwHole> yhs;
            if (parenbeam.DirectionType == BeamDirectionType.Horizontal)
            {
                yhs = parenbeam.Holes.Where(t => t.Location.X > (start.X+wc) && t.Location.X < (end.X-wc) && !t.IsStart && !t.IsEnd).ToList();

                this.Holes.AddRange(yhs);
                if (qiegestart)
                {
                    isdengyuend=true;
                    TopLeft = new JWPoint(start.X+3/JwFileConsts.JwScale,start.Y);
                    
                    BottomLeft = new JWPoint(TopLeft.X, parenbeam.BottomLeft.Y);
                    
                    qglst.Add(new JWPoint(start.X, parenbeam.CenterPoint.Y));
                    StartTelosType = KongzuType.J;
                    HasStartSide = true;
                    StartXinPoint = new JWPoint(start.X, parenbeam.Center);
                    JWPoint kongzucenter = new JWPoint(start.X + JwFileConsts.Kongjing / (2*JwFileConsts.JwScale), parenbeam.CenterPoint.Y);
                    //this.AddAnyHole(kongzucenter, HoleCreateFrom.FengeJ,StartXinPoint,true,false);
                    this.AddAnyHole(kongzucenter, start.X, true, false);
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
                    var nfsh = Holes.Find(t => t.IsStart);
                    if (z != null && nfsh == null)
                    {
                        Holes.Add(z);
                    }
                    //hol
                }
                if(qiegeend)
                {
                    isdengyuend = true;
                    TopRight = new JWPoint(end.X - 3 / JwFileConsts.JwScale, end.Y);
                    BottomRight = new JWPoint(TopRight.X, BottomLeft.Y);
                    qglst.Add(new JWPoint(end.X, parenbeam.CenterPoint.Y));
                    EndTelosType = KongzuType.J;
                    HasEndSide = true;
                    EndXinPoint = new JWPoint( parenbeam.CenterPoint.X,end.Y);
                    JWPoint kongzucenter = new JWPoint(end.X - JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale), parenbeam.Center);
                    this.AddAnyHole(kongzucenter, end.X, false, true);
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
                    var nfsh = Holes.Find(t => t.IsEnd);

                    if (lasth != null && nfsh == null)
                    {
                        Holes.Add(lasth);
                    }
                }

                

            }
            else
            {
                yhs = parenbeam.Holes.Where(t => t.Location.Y > (start.Y+wc) && t.Location.Y < (end.Y-wc) && !t.IsStart && !t.IsEnd).ToList();

                this.Holes.AddRange(yhs);
                //var hls = parenbeam.Holes.Where(t => t.Location.Y >= start.Y && t.Location.Y <= end.Y).ToList();
                //this.Holes.AddRange(hls);
                if (qiegeend)
                {
                    TopLeft = new JWPoint(end.X, end.Y - 3 / JwFileConsts.JwScale);
                    TopRight = new JWPoint(parenbeam.TopRight.X, TopLeft.Y);
                    qglst.Add(new JWPoint(parenbeam.CenterPoint.X, end.Y));
                    EndTelosType = KongzuType.J;
                    EndXinPoint = new JWPoint(parenbeam.Center, end.Y);
                    JWPoint kongzucenter = new JWPoint(parenbeam.Center, end.Y - JwFileConsts.Kongjing / (2 * JwFileConsts.JwScale));

                    this.AddAnyHole(kongzucenter, end.Y, false,true);
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
                    var nfsh=Holes.Find(t=>t.IsEnd);

                    if (lasth != null&& nfsh==null)
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
                    this.AddAnyHole(kongzucenter, start.Y, true, false);
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
                    var nfsh = Holes.Find(t => t.IsStart);
                    if (z != null&&nfsh==null)
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
            var bflst=parenbeam.Baifangs.Where(t=>t.Center>=start.X&&t.Center<=end.X).ToList();
            this.Baifangs.AddRange(bflst);
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
            data.StartCenter=this.StartCenter;
            data.EndCenter=this.EndCenter;
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
                if (this.DirectionType == BeamDirectionType.Horizontal || this.DirectionType == BeamDirectionType.Vertical)
                {
                    holeorder();
                }
                
            }
        }
        private double _absolutePd;

        public List<JwBeamMarkPoint> jwBeamMarks = new List<JwBeamMarkPoint>();

        public List<JwHoleMachining> JwHoleMachinings = new List<JwHoleMachining>();

        public void holesorder()
        {
            if (this.DirectionType == BeamDirectionType.Horizontal)
            {
                this.Holes = this.Holes.OrderBy(t => t.Location.X).ToList();
            }
            if (this.DirectionType == BeamDirectionType.Vertical)
            {
                this.Holes = this.Holes.OrderBy(t => t.Location.Y).ToList();
            }
        }


        //beam 增加一个新类 并有相对的data类对应

        //存有 相对开始距离  需要打孔的位置 上下中 或者all


        /// <summary>
        /// 2025年10月28日本身在判断是否距离开始 150的时候判断了相对梁开始的距离 
        /// 2025年10月28日 增加beammarkpoint的 RelativeStartDistance属性
        /// 前提默认holes 是完整 包含首尾
        /// 对holes进行排序 并生成需要计算区间的点数据
        /// 2025年4月16日 由于endhole 
        /// 统一一下起始和结束 端口 2及端口所包含的hole 
        /// 2025年10月22日 起始点缺少prebeam
        /// </summary>
        private void holeorder()
        {
            this.jwBeamMarks = new List<JwBeamMarkPoint>();
            if (this.DirectionType == BeamDirectionType.Horizontal)
            {
                this.Holes = this.Holes.OrderBy(t => t.Location.X).ToList();
            }
            if (this.DirectionType == BeamDirectionType.Vertical)
            {
                this.Holes = this.Holes.OrderBy(t => t.Location.Y).ToList();
            }
            
            var starthole = this.Holes.Find(t => t.IsStart);

            var endhole = this.Holes.Find(t => t.IsEnd);

            var centerholes = this.Holes.Where(t=>!t.IsEnd&&!t.IsStart).ToList();

            double pr = 0;
            double sb = 0;//startbeam 起点坐标
            double lastholes = 0;

            double precb = 0;

            //处理beam开始 结束标记点信息
            var bs = new JwBeamMarkPoint(this, true);
            this.jwBeamMarks.Add(bs);
            sb = bs.Coordinate;
            //cbs 为芯起点 不是first 孔组中心点
            var cbs = new JwBeamMarkPoint(this, true, true, false);//芯起点

            //this.Baifangs = this.Baifangs.OrderBy(t => t.Center).ToList();
            //for(int i=0; i < this.Baifangs.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        this.Baifangs[i].HasLast = true;
            //    }
            //    else if(i==this.Baifangs.Count-1)
            //    {
            //        this.Baifangs[i].HasPre = true;
            //    }
            //    else
            //    {
            //        this.Baifangs[i].HasPre = true;
            //        this.Baifangs[i].HasLast = true;
            //    }
            //}

            //处理B端
            if (this.StartTelosType == KongzuType.B)
            {
                cbs.Coordinate = bs.Coordinate + 50 / JwFileConsts.JwScale;//不用区分水平和垂直
                cbs.coordinated();
                //this.jwBeamMarks.Add(cbs);
                //cbs.PreCenterDistance = 0;//他就是中心点
                //prcentercoordinate = cbs.Coordinate;
                //starthole = new JwHole(true, cbs.Point, KongzuType.BC);
                double pbmark = 0;
                if (centerholes?.Count > 0)
                {
                    var fch=centerholes[0];
                    double fcc = 0;
                    if (this.DirectionType == BeamDirectionType.Horizontal)
                    {
                        fcc = fch.Location.X;
                    }
                    else if (this.DirectionType == BeamDirectionType.Vertical)
                    {
                        fcc = fch.Location.Y;
                    }
                    double ce=(fcc-sb)*JwFileConsts.JwScale;
                    cbs.IsBias = true;
                    if (ce >= 150)
                    {
                        starthole= new JwHole(true, cbs.Point, KongzuType.BC);
                        starthole.KongNum = 4;

                    }
                    else
                    {
                        starthole = new JwHole(true, cbs.Point, KongzuType.BP);
                        starthole.KongNum = 2;
                        cbs.IsBias = true;
                    }
                }
            }
            else if (this.StartTelosType == KongzuType.G)
            {
                //g端的逻辑需要调整
                cbs.Coordinate = this.StartCenter;
                cbs.coordinated();
            }
            else if (this.StartTelosType == KongzuType.J)
            {
                cbs.Coordinate = this.StartCenter;
                cbs.coordinated();
            }
            cbs.PreBeamStartDistance=Math.Round(cbs.Coordinate-sb,2);


            

            this.jwBeamMarks.Add(cbs);
            precb = cbs.Coordinate;
            
            //
            if (starthole != null)
            {
                //hole的中心带你可以和mark的不一致  只有B是一致的
                cbs.HasAppend = true;
                //var kshibf = this.Baifangs.Find(t => t.Center == cbs.Coordinate);
                //if (kshibf != null)
                //{
                //    starthole.HasBhLinkHole = kshibf.HasLast;
                //    starthole.HasPreLinkHole= kshibf.HasPre;
                //}
                cbs.AppendHole = starthole;
                

                //var startholejmp = new JwBeamMarkPoint(this, true, false, false);//端口洞中心位置
                //if (this.DirectionType == BeamDirectionType.Horizontal)
                //{
                //    startholejmp.Coordinate = starthole.Location.X;
                //}
                //if (this.DirectionType == BeamDirectionType.Vertical)
                //{
                //    startholejmp.Coordinate = starthole.Location.Y;
                //}
                //startholejmp.PreCenterDistance = Math.Round(startholejmp.Coordinate - precb, 2);
                //startholejmp.PreBeamStartDistance=Math.Round(startholejmp.Coordinate-sb,2);
                //precb = startholejmp.Coordinate;
            }

            double realfirstholeloaction=0;
            if (this.DirectionType == BeamDirectionType.Horizontal)
            {
                realfirstholeloaction = cbs.AppendHole.Location.X;
            }
            else if (this.DirectionType == BeamDirectionType.Vertical)
            {
                realfirstholeloaction = cbs.AppendHole.Location.Y;
            }

            addMachining(sb,realfirstholeloaction,cbs.AppendHole,true,false);

            if (centerholes?.Count > 0)
            {
                //针对出了端口之外的中间的柱产生中心点， 确定之间距离是否存在各位 如果不存在保留1
                //前提是遍历的holes都为pillar产生及胜方
                for (int i = 0; i < centerholes.Count; i++)
                {
                   
                    var cccc = new JwBeamMarkPoint(this, true, false, false);//端口洞中心位置
                    if (this.DirectionType == BeamDirectionType.Horizontal)
                    {
                        cccc.Coordinate = centerholes[i].Location.X;
                    }
                    if (this.DirectionType == BeamDirectionType.Vertical)
                    {
                        cccc.Coordinate = centerholes[i].Location.Y;
                    }

                    //var fbf = this.Baifangs.Find(t => t.Center == cccc.Coordinate);

                    //if(fbf != null)
                    //{
                    //    centerholes[i].HasPreLinkHole = fbf.HasPre;
                    //    centerholes[i].HasBhLinkHole = fbf.HasLast;
                    //}

                    cccc.PreCenterDistance = Math.Round(cccc.Coordinate - precb, 1);
                    cccc.PreBeamStartDistance=Math.Round(cccc.Coordinate -sb,1);
                    cccc.AppendHole=centerholes[i];
                    cccc.HasAppend = true;
                    this.jwBeamMarks.Add(cccc);
                    precb=cccc.Coordinate;//循环完即为最后一个洞坐标非 首位端口的洞

                    addMachining(sb,cccc.Coordinate,cccc.AppendHole,false,false);

                    //if (i == centerholes.Count - 1)
                    //{
                    //    lastholes = cccc.Coordinate;
                    //}
                }

              
            }

            //处理beam开始 结束标记点信息
            var es = new JwBeamMarkPoint(this, false, true);
            es.PreBeamStartDistance=Math.Round(es.Coordinate - sb, 2);
            this.jwBeamMarks.Add(es);
            var endx = new JwBeamMarkPoint(this, true,false, true);//芯终点

            if (this.EndTelosType == KongzuType.B)
            {
                //芯终点
                endx.Coordinate = es.Coordinate - 50 / JwFileConsts.JwScale;//不用区分水平和垂直
                endx.coordinated();

                double lastce = (es.Coordinate - precb) * JwFileConsts.JwScale;

                JwHole ewholeend;
                if (lastce >= 150)
                {
                    endhole = new JwHole(true, endx.Point, KongzuType.BC);
                    endhole.KongNum = 4;
                }
                else
                {
                    endhole = new JwHole(true, endx.Point, KongzuType.BP);
                    endhole.KongNum = 2;
                    endx.IsBias = true;
                }
                //endhole = ewholeend;
                endx.HasAppend = true;
                //var kshibf = this.Baifangs.Find(t => t.Center == cbs.Coordinate);
                //if (kshibf != null)
                //{
                //    endhole.HasBhLinkHole = kshibf.HasLast;
                //    endhole.HasPreLinkHole = kshibf.HasPre;
                //}
                endx.AppendHole = endhole;
                
            }
            else
            {
                endx.Coordinate = this.EndCenter;
                endx.coordinated();
                if (endhole != null)
                {
                    endx.HasAppend = true;
                    //var kshibf = this.Baifangs.Find(t => t.Center == cbs.Coordinate);
                    //if (kshibf != null)
                    //{
                    //    endhole.HasBhLinkHole = kshibf.HasLast;
                    //    endhole.HasPreLinkHole = kshibf.HasPre;
                    //}
                    endx.AppendHole = endhole;
                    //precb = endholejmp.Coordinate;
                }
            }

            double reallastholeloaction = 0;
            if (this.DirectionType == BeamDirectionType.Horizontal)
            {
                reallastholeloaction = endx.AppendHole.Location.X;
            }
            else if (this.DirectionType == BeamDirectionType.Vertical)
            {
                reallastholeloaction = endx.AppendHole.Location.Y;
            }
            addMachining(sb, reallastholeloaction, endx.AppendHole,false,true);
            endx.PreBeamStartDistance=Math.Round(endx.Coordinate-sb,2);
            endx.PreCenterDistance=Math.Round(endx.Coordinate-precb,2);
            this.jwBeamMarks.Add(endx);
            var z = this.jwBeamMarks.Count;
            double xxlength=this.jwBeamMarks.Sum(t=>t.PreCenterDistance);

        }

        /// <summary>
        /// 记录加工点位信息
        /// </summary>
        /// <param name="ks">梁开始坐标 </param>
        /// <param name="location">hole孔组中心点 判断完的xy</param>
        /// <param name="hole">hole</param>
        /// <param name="isbis">是否偏心2</param>
        /// <param name="isstart"></param>
        /// <param name="isend"></param>
        public void addMachining(double ks,double location,JwHole hole,bool isstart=false,bool isend=false)
        {
           
            var wc =Math.Round((JwFileConsts.Kongjing / JwFileConsts.JwScale) / 2,2);
            var holerealleft = location-wc;
            var holerealright = location+wc;

            JwHoleMachining machiningleft = new JwHoleMachining
            {
                Id = Id,
                RelativeStartDistance = Math.Round((holerealleft - ks), 2) * JwFileConsts.JwScale,
                HasLeft = hole.HasTop,
                HasRight = hole.HasBottom,
                HasTop = hole.HasCenter
            };
            //JwHoleMachinings.Add(machiningleft);
            JwHoleMachining machiningright = new JwHoleMachining
            {
                Id = Id,
                RelativeStartDistance = Math.Round((holerealright - ks), 2) * JwFileConsts.JwScale,
                HasLeft = hole.HasTop,
                HasRight = hole.HasBottom,
                HasTop = hole.HasCenter
            };
            //JwHoleMachinings.Add(machiningright);
            JwHoleMachining machiningsingle = new JwHoleMachining
            {
                Id = Id,
                RelativeStartDistance = Math.Round((location - ks), 2) * JwFileConsts.JwScale,
                HasLeft = hole.HasTop,
                HasRight = hole.HasBottom,
                HasTop = hole.HasCenter
            };
            switch (hole.HoleType)
            {
                case KongzuType.BC:
                    //4孔
                        JwHoleMachinings.Add(machiningleft);
                        JwHoleMachinings.Add(machiningright);
                    break;
                case KongzuType.BP:
                    if (isstart)
                    {
                        JwHoleMachinings.Add(machiningleft);
                    }
                    if (isend)
                    {
                        JwHoleMachinings.Add(machiningright);
                    }
                    break;
                case KongzuType.J:
                    JwHoleMachinings.Add(machiningsingle);
                    break;
                case KongzuType.G:
                    JwHoleMachinings.Add(machiningsingle);
                    break;
                default:
                    JwHoleMachinings.Add(machiningleft);
                    JwHoleMachinings.Add(machiningright);
                    break;
            }
        }

        public List<JwQiegeZu> jwQiegeZus = new List<JwQiegeZu>();

        /// <summary>
        /// 导出加工csv
        /// </summary>
        /// <returns></returns>
        public string ToProcessCsv()
        {
            StringBuilder sb=new StringBuilder();

            var rights=  JwHoleMachinings.Where(t => t.HasRight).ToList();

            var lefts= JwHoleMachinings.Where(t => t.HasLeft).ToList();

            var tops= JwHoleMachinings.Where(t => t.HasTop).ToList();
            sb.Append("右面穴\r\n");

            double ry = JwFileConsts.Kongjing / 2;
            if(rights.Count>0)
            {
                foreach (var h in rights)
                {
                    sb.Append(h.ToCsvString(-ry));
                    //sb.Append(string.Format("0, {0}, , , , , , , , \r\n", h.RelativeStartDistance.ToString("0.00")));

                }
            }
            
            sb.Append("左面穴\r\n");
            if(lefts.Count>0)
            {
                foreach (var h in lefts)
                {
                    sb.Append(h.ToCsvString(-ry));
                    //sb.Append(string.Format("0, {0}, , , , , , , , \r\n", h.RelativeStartDistance.ToString("0.00")));
                }
            }
            
            sb.Append("上面穴\r\n");
            double cy = 100 - JwFileConsts.Kongjing / 2;
            if(tops.Count>0)
            {
                foreach (var h in tops)
                {
                    sb.Append(h.ToCsvString(cy));
                    //sb.Append(string.Format("0, {0}, , , , , , , , \r\n", h.RelativeStartDistance.ToString("0.00")));
                }
            }
            //sb.Append(string.Format("{0},{1}-,{2}-,{3},,, {4}, 0.0, {5}, {6}, 0, 0.0, 0.0\r\n", "", Gongqu, Liangfuhao, Louceng, "H-200x100x5.5x8", SingleBeamLength.ToString("0.0"), benshu));
            //sb.Append("0, 0, 0, , 0, 0\r\n");
            //绘制上  对应csv的 右

            //foreach(var jfm in jwBeamMarks)
            //{
            //    if(jfm.HasAppend&&jfm.AppendHole!=null)
            //    {
            //        sb.Append(string.Format("1, {0}, {1}, , {2}, {3}\r\n", jfm.Point.X.ToString("0.00"), jfm.Point.Y.ToString("0.00"), jfm.AppendHole.KongNum, jfm.AppendHole.SuoShuMian.ToString("D")));
            //    }
            //    else
            //    {
            //        sb.Append(string.Format("1, {0}, {1}, , {2}, {3}\r\n", jfm.Point.X.ToString("0.00"), jfm.Point.Y.ToString("0.00"), 0, 0));
            //    }
            //}

            //绘制center 对应csv的 top

            //绘制下 对应csv的 left

            return sb.ToString();
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
                        JwQiegeZu qiegeZu = new JwQiegeZu { Qiegezb = endp.X, RJwBeam = nb };
                        var pre = beam.jwQiegeZus.Last();
                        if (pre != null)
                        {
                            pre.AJwBeam = nb;
                        }
                        beam.jwQiegeZus.Add(qiegeZu);
                        int t = i + 1;
                        nb.BeamCode = beam.BeamCode + "-" + t.ToString();
                        relst.Add(nb);
                        startp = endp;
                    }
                    JWPoint enddp = beam.TopRight;
                    var edp = new JwBeam(beam, startp, enddp, true, false);
                    var pres = beam.jwQiegeZus.Last();
                    if (pres != null)
                    {
                        pres.AJwBeam = edp;
                    }
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
                        JwQiegeZu qiegeZu = new JwQiegeZu { Qiegezb = endp.X, RJwBeam = nb };
                        var pre = beam.jwQiegeZus.Last();
                        if (pre != null)
                        {
                            pre.AJwBeam = nb;
                        }
                        beam.jwQiegeZus.Add(qiegeZu);
                        relst.Add(nb);

                        startp = endp;
                    }
                    JWPoint enddp = beam.TopLeft;
                    var endbeam = new JwBeam(beam, startp, enddp, true, false);
                    endbeam.BeamCode = beam.BeamCode + "-" + (beam.QieGePoints.Count + 1).ToString();
                    var pres = beam.jwQiegeZus.Last();
                    if (pres != null)
                    {
                        pres.AJwBeam = endbeam;
                    }
                    
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
                        var existbb = beam.LinkParts.Where(t => t.BjCenterPoint.IsEqualsWithError(z) && t.Directed == TaggDirect.Up).ToList();
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
                        var existbbdown = beam.LinkParts.Where(t => t.BjCenterPoint.IsEqualsWithError(z) && t.Directed == TaggDirect.Down).ToList();
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
                        var existbbleft = beam.LinkParts.Where(t => t.BjCenterPoint.IsEqualsWithError(z) && t.Directed == TaggDirect.Left).ToList();
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
                        var existbbright = beam.LinkParts.Where(t => t.BjCenterPoint.IsEqualsWithError(z) && t.Directed == TaggDirect.Right).ToList();
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
