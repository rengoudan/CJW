using JwCore;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public class JwHole : JwSquareBase
    {

        /// <summary>
        /// 特殊无柱在起始或者最终的位置，多补打的孔
        /// </summary>
        public bool IsFromBSE { get; set; }

        /// <summary>
        /// 除去首尾端默认为 都为Pillar产生的/还有一种是由胜方 HoleType为center
        /// </summary>
        public bool IsFromPillar { get; set; }

        public JWPoint Location { get; set; }

        public bool HasLocationCenter { get; set; }
        public JWPoint? LocationCenter { get; set; }

        public HoleCreateFrom FirstCreateFrom { get; set; }

        public HoleCreateFrom ChangeFrom { get; set; }

        /// <summary>
        /// 中间的  还是端 及端类型
        /// </summary>
        public KongzuType HoleType { get; set; }

        public bool IsStart { get; set; }

        public bool IsEnd { get; set; }

        public bool HasTop { get; set; }
        //public JwKongZu? TopKongzu { get; set; }

        public bool HasCenter { get; set; }

        //public JwKongZu? CenterKongzu { get;set; }

        public bool HasBottom { get; set; }

        public int KongNum { get; set; }

        /// <summary>
        /// 胜方G
        /// </summary>
        public bool HasSG { get; set; } 

        /// <summary>
        /// 拥有前 的链接洞
        /// </summary>
        public bool HasPreLinkHole { get; set; }

        public bool HasBhLinkHole { get; set; }

        /// <summary>
        /// 水平的 存X 垂直的存Y
        /// </summary>
        public double HoleCenter { get; set; }

        public bool IsMachining { get; set; }

        //public JwKongZu? BottomKongzu { get; set;}

        public JwHole()
        {

        }

        /// <summary>
        /// 接触调用
        /// </summary>
        /// <param name="location"></param>
        /// <param name="firstCreateFrom"></param>
        /// <param name="locationCenter"></param>
        /// <param name="isStart"></param>
        /// <param name="isEnd"></param>
        public JwHole(JWPoint location, HoleCreateFrom firstCreateFrom, JWPoint? locationCenter = null, bool isStart = false, bool isEnd = false)
        {
            Id = Guid.NewGuid().ToString();
            Location = location;
            LocationCenter = locationCenter;
            FirstCreateFrom = firstCreateFrom;
            ChangeFrom = firstCreateFrom;
            IsStart = isStart;
            IsEnd = isEnd;
            IsMachining = true;
            if (!isStart && !isEnd)
            {
                HoleType = KongzuType.Center;
                IsFromPillar = true;
                
            }
            switch (firstCreateFrom)
            {
                case HoleCreateFrom.Pillar:
                    HasTop = true;

                    HasBottom = true;

                    HasCenter = true;
                    KongNum = 4;
                    break;
                case HoleCreateFrom.JieChuG:
                    HoleType = KongzuType.G;
                    HasCenter = true;
                    HasTop= true;
                    HasBottom=true;
                    KongNum = 2;
                    break;
                case HoleCreateFrom.FengeJ:
                    HoleType = KongzuType.J;
                    HasCenter = true;
                    HasTop = true;
                    HasBottom = true;
                    KongNum = 2;
                    break;
                case HoleCreateFrom.JieChu:
                    HasSG = true;
                    HasCenter = true;
                    HasTop = true;
                    HasBottom = true;   
                    KongNum = 4;
                    break;
                case HoleCreateFrom.AddedHole:
                    HasCenter = false;
                    HasTop = true;
                    HasBottom = false;
                    KongNum = 2;
                    HoleType = KongzuType.AddedHole;
                    IsFromPillar = false;
                    break;
            }
        }

        /// <summary>
        /// 2025年4月11日 作废
        /// </summary>
        /// <param name="kongZu"></param>
        /// <param name="createFrom"></param>
        public JwHole(JwKongZu kongZu, HoleCreateFrom createFrom)
        {
            Id = Guid.NewGuid().ToString();
            Location = new JWPoint(kongZu.Position.X, kongZu.Position.Y);
            //Kongzu = kongZu;
            if (createFrom == HoleCreateFrom.Pillar)
            {
                //需确认 柱的化 是创建上 还是上下中都要的孔
                HasTop = true;

                HasBottom = true;

                HasCenter = true;

            }
            if (createFrom == HoleCreateFrom.JieChuG)
            {
                HasCenter = true;
            }
            if (createFrom == HoleCreateFrom.FengeJ)
            {
                HasCenter = true;
            }
            if (createFrom == HoleCreateFrom.JieChu)
            {
                HasCenter = true;
            }
        }

        /// <summary>
        /// 2025年4月11日 暂时无用
        /// </summary>
        /// <param name="kongZu"></param>
        /// <param name="createFrom"></param>
        /// <param name="lc"></param>
        public JwHole(JwKongZu kongZu, HoleCreateFrom createFrom, JWPoint lc)
        {
            Id = Guid.NewGuid().ToString();

            Location = new JWPoint(kongZu.Position.X, kongZu.Position.Y);
            LocationCenter = new JWPoint(lc.X, lc.Y);
            if (createFrom == HoleCreateFrom.Pillar)
            {
                //需确认 柱的化 是创建上 还是上下中都要的孔
                HasTop = true;

                HasBottom = true;

                HasCenter = true;

            }
            if (createFrom == HoleCreateFrom.JieChuG)
            {
                HasCenter = true;

            }
            if (createFrom == HoleCreateFrom.FengeJ)
            {
                HasCenter = true;

            }
            if (createFrom == HoleCreateFrom.JieChu)
            {
                HasCenter = true;

            }
        }


        public JwHole(double x,double y)
        {
            Id = Guid.NewGuid().ToString();
            IsMachining = true;
            FirstCreateFrom = ChangeFrom = HoleCreateFrom.Lianjie;
            Location = new JWPoint(x, y);
        }

        /// <summary>
        /// 用来指示 孔组是否有中心点（即如果num为2 isbias为true 则location为中心点，孔组需要偏离中心点56/2）
        /// 默认为false
        /// </summary>
        public bool IsBias { get; set; }

        /// <summary>
        /// 用来处理BC BP的孔  根据isstart isend 如果num 为2 可以图里出孔的
        /// </summary>
        /// <param name="isForB"></param>
        /// <param name="location"></param>
        public JwHole(bool isForB,JWPoint location,KongzuType kongzuType)
        {
            Id = Guid.NewGuid().ToString();
            Location = new JWPoint(location.X, location.Y);
            //Kongzu = kongZu;
            //需确认 柱的化 是创建上 还是上下中都要的孔
            HasTop = true;

            HasBottom = true;

            HasCenter = true;

            IsBias = false; 

            IsFromBSE = true;

            HoleType = kongzuType;
        }


        /// <summary>
        /// 权重为 柱 》 g胜》链接J=败G 弃用
        /// </summary>
        /// <param name="other"></param>
        /// <param name="createFrom"></param>
        public void changeByOther(JwKongZu other, HoleCreateFrom createFrom)
        {
            switch (createFrom)
            {
                case HoleCreateFrom.Pillar:
                    if (!HasTop)
                    {
                        HasTop = true;

                    }
                    if (!HasBottom)
                    {
                        HasBottom = true;

                    }
                    break;

            }
        }

        public void changeByOther(HoleCreateFrom createFrom)
        {
            switch (createFrom)
            {
                case HoleCreateFrom.Pillar:
                    if (!HasTop)
                    {
                        HasTop = true;

                    }
                    if (!HasBottom)
                    {
                        HasBottom = true;

                    }
                    break;
                case HoleCreateFrom.FengeJ:
                    HasTop = true;
                    HasCenter=true;
                    HasBottom=true;
                    KongNum = 2;
                    break;

            }
        }

        public void createTBLF()
        {
            double half = JwFileConsts.EllipseSpacing / (2 * JwFileConsts.JwScale);
            if (KongNum == 2)
            {
                TopLeft = new JWPoint(Location.X, Location.Y + half);
                BottomLeft = new JWPoint(Location.X, Location.Y - half);
            }
            else
            {
                TopLeft = new JWPoint(Location.X - half, Location.Y + half);
                BottomLeft = new JWPoint(Location.X - half, Location.Y - half);
                TopRight = new JWPoint(Location.X + half, Location.Y + half);
                BottomRight = new JWPoint(Location.X + half, Location.Y - half);
            }
        }

        public JwHoleData ToData()
        {
            if (HasBhLinkHole)
            {
                int z = 1;
            }

            JwHoleData holeData = new JwHoleData
            {
                Id = Id,
                IsEnd = IsEnd,
                IsStart = IsStart,
                ChangeFrom = ChangeFrom,
                FirstCreateFrom = FirstCreateFrom,
                HasBhLinkHole = HasBhLinkHole,
                HasPreLinkHole = HasPreLinkHole,
                HasBottom = HasBottom,
                HasCenter = HasCenter,
                HasTop = HasTop,
                HoleType = HoleType,
                KongNum = KongNum,
                Location = new NetTopologySuite.Geometries.Point(Location.X, Location.Y),
                IsMachining=IsMachining

            };
            return holeData;
        }

        public double AbsoluteP { get; set; }

        /// <summary>
        /// 相对前一个的值
        /// </summary>
        public double RelativeP { get; set;}

        /// <summary>
        /// 所属的梁
        /// </summary>
        public JwBeam Beam { get; set; }

        /// <summary>
        /// createfrom为lianjie时，且间距小于43时候标识成对的孔
        /// </summary>
        public JwHole PairedHole { get; set; }

        /// <summary>
        /// 2026年7月19日是否被替换
        /// </summary>
        public bool IsPairedChanged { get; set; }

        /// <summary>
        /// 2026年7月19日 仅在createfrom为lianjie时，标记连接线的坐标点逻辑如下
        /// 如果是水平梁，标记为Y坐标，相对于beam的center是add还是增加
        /// 即连接线起点在梁的上方还是下方，上为add下为reduce
        /// 如果是垂直梁，标记为X坐标，相对于beam的center是add还是reduce
        /// left为reduce right为add
        /// </summary>
        public ZhengfuType Direct { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ispre"></param>
        /// <returns></returns>
        public JwHole AppendHole(bool ispre,JwBeam _beam)
        {
            JwHole hole = new JwHole();
            hole.Id = Guid.NewGuid().ToString();
            hole.IsMachining = true;
            hole.FirstCreateFrom =hole.ChangeFrom = HoleCreateFrom.Lianjie;
            double x, y;
            if(ispre)
            {
                if(_beam.DirectionType== BeamDirectionType.Horizontal)
                {
                    x = Location.X - JwFileConsts.PianchaLianjieValue / JwFileConsts.JwScale;
                    y = Location.Y ;
                    hole.HoleCenter = x;
                }
                else
                {
                    x = Location.X ;
                    y = Location.Y - JwFileConsts.PianchaLianjieValue / JwFileConsts.JwScale;
                    hole.HoleCenter = y;
                }
            }
            else
            {
                if(_beam.DirectionType== BeamDirectionType.Horizontal)
                {
                    x = Location.X+ JwFileConsts.PianchaLianjieValue / JwFileConsts.JwScale;
                    y = Location.Y;
                    hole.HoleCenter = x;
                }
                else
                {
                    x = Location.X ;
                    y = Location.Y+ JwFileConsts.PianchaLianjieValue / JwFileConsts.JwScale;
                    hole.HoleCenter = y;
                }
            }
            hole.Location=hole.LocationCenter = new JWPoint(x, y);
            hole.HasTop = hole.HasCenter = false;
            hole.HasBottom = true;
            return hole;
        }


    }

    public class JwBeamSide
    {
        public KongzuType SideType { get; set; }

        public JwHole KongZu { get; set; }
    }
}
