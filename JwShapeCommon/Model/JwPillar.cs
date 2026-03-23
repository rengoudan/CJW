using JwCore;
using JwwHelper;
using RGB.Jw.JW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    /// <summary>
    /// 业务特有柱 包含k
    /// 标注为矩形特有属性
    /// 2026年3月9日简化识别进一步抽象将柱同一属性记录为正方形中心点，在解析识别的时候就输出正方形中心点
    /// 
    /// </summary>
    public class JwPillar : JwSquareBase
    {
        //public string Id { get; set; }
        public string PillarCode { get; set; } = "";
        public List<JwBlock> Blocks { get; set; }

        public int BlocksCount { get; set; }

        public List<JWPoint> Points { get; set; }

        public JWPoint PointA { get; set; }

        public JWPoint PointB { get; set; }

        public double Distance { get; set; }


        /// <summary>
        /// 是否有匹配到的标注
        /// </summary>
        public bool HasTag { get; set; }

        public string TagId { get; set; }

        public string TagName { get; set; }

        public JwTagg Tagg { get; set; }

        public PillarBaseType BaseType { get; set; }

        /// <summary>
        /// 是否归属 beam k的话 如果有一个归属则判定为归属
        /// </summary>
        public bool HasBeam { get; set; }

        /// <summary>
        /// 2026年3月9日添加K柱类型 按照距离区分
        /// </summary>
        public KPillarType kPillarType { get; set; }

        public PillarCreateFrom CreateFrom { get; set; }

        public JwPillar()
        {
            Id = Guid.NewGuid().ToString();

            Blocks = new List<JwBlock>();
            Points = new List<JWPoint>();
            CreateFrom = PillarCreateFrom.Shape;
        }

        /// <summary>
        /// 2026年3月15日 还差一个中间的矩形 没绘制
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="distance"></param>
        public JwPillar(JWPoint a, JWPoint b, double distance)
        {
            Id = Guid.NewGuid().ToString();
            Blocks = new List<JwBlock>();
            PointA = a;
            var ba = new JwBlock(a);
            Blocks.Add(ba);
            var bb = new JwBlock(b);
            Blocks.Add(bb);
            PointB = b;
            Distance = distance;
            CreateFrom = PillarCreateFrom.Sen;
            Points = new List<JWPoint>();
            if (distance == 0)
            {
                BaseType = PillarBaseType.SinglePillar;
                CenterPoint = new JWPoint(a.X, a.Y);
                CenterPoints.Add(CenterPoint);
                Points.AddRange(ba.BlockPoint);
            }
            else
            {
                BaseType = PillarBaseType.KPillar;
                CenterPoint = new JWPoint((a.X + b.X) / 2, (a.Y + b.Y) / 2);
                CenterPoints.Add(a);
                CenterPoints.Add(b);
                Points.AddRange(ba.BlockPoint);
                Points.AddRange(bb.BlockPoint);
                if (JwExtend.DoubleEqual(a.X, b.X))
                {
                    DirectionType = BeamDirectionType.Vertical;
                }
                else if (JwExtend.DoubleEqual(a.Y, b.Y))
                {
                    DirectionType = BeamDirectionType.Horizontal;
                }
                var jx = new JwBlock(CenterPoint, distance, DirectionType);
                Blocks.Add(jx);
            }
            TopLeft = Points.OrderBy(t => t.X).ThenByDescending(t => t.Y).ToList().First();
            TopRight = Points.OrderByDescending(t => t.X).ThenByDescending(t => t.Y).ToList().First();
            BottomLeft = Points.OrderBy(t => t.X).ThenBy(t => t.Y).ToList().First();
            BottomRight = Points.OrderByDescending(t => t.X).ThenBy(t => t.Y).ToList().First();
            Width = TopRight.X - TopLeft.X;
            Height = TopLeft.Y - BottomLeft.Y;
            this.kPillarType = JwFileConsts.Ktype;
        }

        //private JwBlock createBlockbycp(JWPoint c)
        //{

        //}


        /// <summary>
        /// 组成柱的 四边形的中心点集合
        /// </summary>
        public List<JWPoint> CenterPoints = new List<JWPoint>();

        public List<JWPoint> PillarInBeamCenterPoints = new List<JWPoint>();
        public bool HasPillarBeamCenter = false;

        ///// <summary>
        ///// 多种功能形状 仅考虑至少两个以上的块组成  
        ///2026年3月23日 在判断水平还是垂直的时候 增加误差调整
        ///2026年3月23日 额外增加一部 中心点修正，后续top right bottom left等点的修正 以中心点为基准进行修正
        ///此处可以放弃设置
        ///2026年3月23日 point统计左上 右上问题 由于有小数点后2位导致误差 保留一位
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
                foreach(var p in block.BlockPoint)
                {
                   var px = Math.Round(p.X, 1);
                   var py = Math.Round(p.Y, 1);
                   Points.Add(new JWPoint(px, py)); 
                }
                //Points.AddRange(block.BlockPoint);
                if (block.HasCenter)
                {
                    CenterPoints.Add(block.CenterPoint);
                }

            }
            //
            if (Blocks.Count == 1)
            {
                this.BaseType = PillarBaseType.SinglePillar;
                this.PointA = Blocks[0].CenterPoint;
                this.PointB = Blocks[0].CenterPoint;
                this.Distance = 0;
                TopLeft = Points.OrderBy(t => t.X).ThenByDescending(t => t.Y).ToList().First();
                TopRight = Points.OrderByDescending(t => t.X).ThenByDescending(t => t.Y).ToList().First();
                BottomLeft = Points.OrderBy(t => t.X).ThenBy(t => t.Y).ToList().First();
                BottomRight = Points.OrderByDescending(t => t.X).ThenBy(t => t.Y).ToList().First();
                Width = TopRight.X - TopLeft.X;
                Height = TopLeft.Y - BottomLeft.Y;
                this.CenterPoint = this.PointA;
                //this.CenterPoint = Blocks[0].CenterPoint;
            }
            //去掉正方形 我觉得非必要
            if (Blocks.Count == 3)
            {
                this.BaseType = PillarBaseType.KPillar;
                TopLeft = Points.OrderBy(t => t.X).ThenByDescending(t => t.Y).ToList().First();
                TopRight = Points.OrderByDescending(t => t.X).ThenByDescending(t => t.Y).ToList().First();
                BottomLeft = Points.OrderBy(t => t.X).ThenBy(t => t.Y).ToList().First();
                BottomRight = Points.OrderByDescending(t => t.X).ThenBy(t => t.Y).ToList().First();
                Width = TopRight.X - TopLeft.X;
                Height = TopLeft.Y - BottomLeft.Y;
                if (Width >= Height)
                {
                    //如果是水平的 可以强制所有中心点的Y为first的
                    DirectionType = BeamDirectionType.Horizontal;
                    if (Blocks.Count > 1 && CenterPoints?.Count > 1)
                    {
                        var _tempps = CenterPoints.OrderBy(t => t.X).ToList();
                        PillarInBeamCenterPoints.Add(_tempps.First());
                        PillarInBeamCenterPoints.Add(_tempps.Last());
                        this.PointA= _tempps.First();
                        this.PointB= _tempps.Last();
                        this.CenterPoint=_tempps[1];
                        this.Distance = JwExtend.Distance(PointA, PointB);
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
                else
                {
                    DirectionType = BeamDirectionType.Vertical;
                    if (Blocks.Count > 1 && CenterPoints?.Count > 1)
                    {
                        var _tempps = CenterPoints.OrderBy(t => t.Y).ToList();
                        PillarInBeamCenterPoints.Add(_tempps.First());
                        PillarInBeamCenterPoints.Add(_tempps.Last());
                        this.PointA = _tempps.First();
                        this.PointB = _tempps.Last();
                        this.CenterPoint = _tempps[1];
                        this.Distance = JwExtend.Distance(PointA, PointB);
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

            }
            BlocksCount = Blocks.Count;
            JisuanWidthHeight();
        }


        /// <summary>
        /// 2026年3月16日 针对pointa b进行修改
        /// 原来记录的point位左上角 现在改为记录中心点 以便于后续标注关联
        /// </summary>
        /// <returns></returns>
        public JwPillarData ToPillarData()
        {
            JwPillarData data = new JwPillarData();
            data.PillarCode = PillarCode;
            data.BaseType = this.BaseType;
            data.Height = this.Height;
            data.Width = this.Width;
            //data.Location = this.TopLeft.ToPoint();
            data.Location = this.CenterPoint.ToPoint();
            data.DirectionType= this.DirectionType;
            if (!string.IsNullOrEmpty(this.TagName))
            {
                data.TaggTitle = this.TagName;
            }
            else
            {
                data.TaggTitle = "";
            }

            if (data.BaseType == PillarBaseType.KPillar)
            {
                //2026年3月17日 剔除不使用
                //if (this.Blocks.Count == 3)
                //{
                //    //可以增加两个属性  标识 todata flag 及 errmsg 后期做
                //    List<JwBlock> zlst;
                //    if(this.DirectionType==BeamDirectionType.Horizontal)
                //    {
                //        zlst = this.Blocks.OrderBy(t => t.TopLeft.X).ToList();
                //    }
                //    else
                //    {
                //        zlst=this.Blocks.OrderByDescending(t=>t.TopLeft.Y).ToList();
                //    }
                //    var fb = zlst.First();
                //    data.FirstLocation = fb.TopLeft.ToPoint();
                //    data.FirstHeight=fb.Height; 
                //    data.FirstWidth=fb.Width;
                //    var cb = zlst[1];
                //    data.CenterLocation = cb.TopLeft.ToPoint();
                //    data.CenterHeight = cb.Height;
                //    data.CenterWidth = cb.Width;
                //    var lb = zlst.Last();
                //    data.LastLocation = lb.TopLeft.ToPoint();
                //    data.LastHeight = lb.Height;
                //    data.LastWidth = lb.Width;
                //}
                data.DirectionType = this.DirectionType;
                data.FirstLocation = this.PointA.ToPoint();
                data.LastLocation = this.PointB.ToPoint();
                data.CenterLocation = this.CenterPoint.ToPoint();
                data.CenterWidth = this.Distance;//
            }
            else
            {
                data.DirectionType = this.DirectionType;
                data.FirstLocation = this.PointA.ToPoint();
                data.LastLocation= this.PointA.ToPoint();
                data.CenterLocation = this.CenterPoint.ToPoint();
                data.CenterWidth = this.Distance;
            }
            return data;
        }

        public List<JwwData> ToJwwData()
        {
            List<JwwData>  datas=new List<JwwData>();
            if(BaseType== PillarBaseType.KPillar)
            {
                datas.AddRange(Blocks.Select(t => t.ToJwwData()));
            }
            else
            {
                datas.Add(Blocks.First().ToJwwData());
            }
            
            return datas;
        }
    }
}
