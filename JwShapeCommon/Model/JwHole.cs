using JwCore;
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

        //public JwKongZu? BottomKongzu { get; set;}

        public JwHole()
        {

        }

        public JwHole(JWPoint location, HoleCreateFrom firstCreateFrom, JWPoint? locationCenter = null, bool isStart = false, bool isEnd = false)
        {
            Id = Guid.NewGuid().ToString();
            Location = location;
            LocationCenter = locationCenter;
            FirstCreateFrom = firstCreateFrom;
            ChangeFrom = firstCreateFrom;
            IsStart = isStart;
            IsEnd = isEnd;
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
                    KongNum = 2;
                    break;
                case HoleCreateFrom.FengeJ:
                    HoleType = KongzuType.J;
                    HasCenter = true;
                    KongNum = 2;
                    break;
                case HoleCreateFrom.JieChu:
                    HasSG = true;
                    HasCenter = true;

                    HasBottom = true;   
                    KongNum = 4;
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

            IsBias = true; 

            IsFromBSE = true;
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

            JwHoleData holeData = new JwHoleData
            {
                Id = Id,
                IsEnd = IsEnd,
                IsStart = IsStart,
                ChangeFrom = ChangeFrom,
                FirstCreateFrom = FirstCreateFrom,
                HasBottom = HasBottom,
                HasCenter = HasCenter,
                HasTop = HasTop,
                HoleType = HoleType,
                KongNum = KongNum,
                Location = new NetTopologySuite.Geometries.Point(Location.X, Location.Y),

            };
            return holeData;
        }

        public double AbsoluteP { get; set; }

        /// <summary>
        /// 相对前一个的值
        /// </summary>
        public double RelativeP { get; set;}
    }

    public class JwBeamSide
    {
        public KongzuType SideType { get; set; }

        public JwHole KongZu { get; set; }
    }
}
