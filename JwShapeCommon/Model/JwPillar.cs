using JwCore;
using RGB.Jw.JW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    /// <summary>
    /// 业务特有柱 包含k
    /// 标注为矩形特有属性
    /// </summary>
    public class JwPillar: JwSquareBase
    {
        //public string Id { get; set; }
        public string PillarCode { get; set; }
        public List<JwBlock> Blocks { get; set; }

        public int BlocksCount { get; set; }

        public List<JWPoint> Points { get; set; }

        /// <summary>
        /// 是否有匹配到的标注
        /// </summary>
        public bool HasTag { get; set; }

        public string TagId { get;set; }

        public string TagName { get; set; }

        public JwTagg Tagg { get; set; }

        public PillarBaseType BaseType { get; set; }

        /// <summary>
        /// 是否归属 beam k的话 如果有一个归属则判定为归属
        /// </summary>
        public bool HasBeam { get; set; }


        public JwPillar() 
        { 
            Id=Guid.NewGuid().ToString();
            Blocks = new List<JwBlock>();
            Points=new List<JWPoint>();
        }

        /// <summary>
        /// 组成柱的 四边形的中心点集合
        /// </summary>
        public List<JWPoint> CenterPoints = new List<JWPoint>();

        public List<JWPoint> PillarInBeamCenterPoints=new List<JWPoint>();
        public bool HasPillarBeamCenter = false;

        ///// <summary>
        ///// 多种功能形状 仅考虑至少两个以上的块组成  
        ///// </summary>
        //public BeamDirectionType DirectionType { get; set; }
        public void squareParse()
        {
            int izhengfangcou = 0;
            
            foreach (var block in Blocks)
            {
                if (block.Iszhengfangxing)
                {
                    izhengfangcou++;
                }
                Points.AddRange(block.BlockPoint);
                if(block.HasCenter)
                {
                    CenterPoints.Add(block.CenterPoint);
                }
                
            }
            //
            if(Blocks.Count==1)
            {
                this.BaseType = PillarBaseType.SinglePillar;
                //this.CenterPoint = Blocks[0].CenterPoint;
            }
            //去掉正方形 我觉得非必要
            if (Blocks.Count == 3)
            {
                this.BaseType = PillarBaseType.KPillar;
            }
            TopLeft = Points.OrderBy(t => t.X).ThenByDescending(t => t.Y).ToList().First();
            TopRight=Points.OrderByDescending(t=>t.X).ThenByDescending(t=>t.Y).ToList().First();
            BottomLeft = Points.OrderBy(t => t.X).ThenBy(t => t.Y).ToList().First();
            BottomRight = Points.OrderByDescending(t => t.X).ThenBy(t => t.Y).ToList().First();
            Width=TopRight.X-TopLeft.X;
            Height= TopLeft.Y- BottomLeft.Y;
            if (Width >= Height)
            {
                DirectionType = BeamDirectionType.Horizontal;
                if(Blocks.Count>1&&CenterPoints?.Count>1)
                {
                    var _tempps = CenterPoints.OrderBy(t => t.X).ToList();
                    PillarInBeamCenterPoints.Add(_tempps.First());
                    PillarInBeamCenterPoints.Add(_tempps.Last());
                    HasPillarBeamCenter=true;
                }
                else if(Blocks.Count==1&& CenterPoints?.Count==1)
                {
                    PillarInBeamCenterPoints = CenterPoints;
                    HasPillarBeamCenter = true;
                }
                else
                {
                    HasPillarBeamCenter = false;
                }
            }
            else
            {
                DirectionType = BeamDirectionType.Vertical;
                if (Blocks.Count > 1 && CenterPoints?.Count > 1)
                {
                    var _tempps = CenterPoints.OrderBy(t => t.Y).ToList();
                    PillarInBeamCenterPoints.Add(_tempps.First());
                    PillarInBeamCenterPoints.Add(_tempps.Last());
                    HasPillarBeamCenter = true;
                }
                else if (Blocks.Count == 1 && CenterPoints?.Count == 1)
                {
                    PillarInBeamCenterPoints = CenterPoints;
                    HasPillarBeamCenter = true;
                }
                else
                {
                    HasPillarBeamCenter = false;
                }
            }
            BlocksCount = Blocks.Count;
            JisuanWidthHeight();
        }

        public JwPillarData ToPillarData()
        {
            JwPillarData data = new JwPillarData();
            data.PillarCode = PillarCode;
            data.BaseType = this.BaseType;
            data.Height = this.Height;
            data.Width = this.Width;
            data.Location = this.TopLeft.ToPoint();
            if (!string.IsNullOrEmpty(this.TagName)){
                data.TaggTitle = this.TagName;
            }
            else
            {
                data.TaggTitle = "";
            }
           
            if(data.BaseType==PillarBaseType.KPillar)
            {
                if (this.Blocks.Count == 3)
                {
                    //可以增加两个属性  标识 todata flag 及 errmsg 后期做
                    List<JwBlock> zlst;
                    if(this.DirectionType==BeamDirectionType.Horizontal)
                    {
                        zlst = this.Blocks.OrderBy(t => t.TopLeft.X).ToList();
                    }
                    else
                    {
                        zlst=this.Blocks.OrderByDescending(t=>t.TopLeft.Y).ToList();
                    }
                    var fb = zlst.First();
                    data.FirstLocation = fb.TopLeft.ToPoint();
                    data.FirstHeight=fb.Height; 
                    data.FirstWidth=fb.Width;
                    var cb = zlst[1];
                    data.CenterLocation = cb.TopLeft.ToPoint();
                    data.CenterHeight = cb.Height;
                    data.CenterWidth = cb.Width;
                    var lb = zlst.Last();
                    data.LastLocation = lb.TopLeft.ToPoint();
                    data.LastHeight = lb.Height;
                    data.LastWidth = lb.Width;
                }
            }
            return data;
        }
    }
}
