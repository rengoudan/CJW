using JwCore;
using JwwHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    /// <summary>
    /// 处理sl 
    /// </summary>
    public class JwBlock: JwSquareBase
    {
        public string RawString { get; set; }

        public List<JWPoint> BlockPoint { get; set; }

        /// <summary>
        /// 三角形的话 考虑整合 存在习惯 把正方形当作两个三角形
        /// 
        /// </summary>
        public JwBlockShapeType ShapeType { get; set; } 

        public bool HasParseSuccess { get; set; }

        //public string Id { get; set; }

        public bool Iszhengfangxing { get; set; }

        public JWPoint ZhengfangCenter { get; set; }

        public JwPillar? ParentPillar;


        public JwBlock()
        {
            Id = Guid.NewGuid().ToString();
            //Id = "caonima";
        }

        /// <summary>
        /// 使用jw 外部形变 初始化
        /// </summary>
        /// <param name="raw"></param>
        public JwBlock(string raw)
        {
            RawString = raw;
            parseStr(raw);
            Id = Guid.NewGuid().ToString();
        }


        /// <summary>
        /// 使用jwwsolid进行初始化 标识方块
        /// </summary>
        /// <param name="solid"></param>
        public JwBlock(JwwSolid solid)
        {
            BlockPoint = new List<JWPoint>
            {
                new JWPoint(solid.m_start_x, solid.m_start_y),
                new JWPoint(solid.m_end_x, solid.m_end_y),
                new JWPoint(solid.m_DPoint2_x, solid.m_DPoint2_y)
            };
            ShapeType = JwBlockShapeType.Triangular;
            if (solid.m_DPoint2_x != solid.m_DPoint3_x || solid.m_DPoint2_y != solid.m_DPoint3_y)
            {
                //直接处理四边形
                BlockPoint.Add(new JWPoint(solid.m_DPoint3_x, solid.m_DPoint3_y));
                ShapeType = JwBlockShapeType.Square;
                squareParse();//处理
            }
            HasParseSuccess = true;
            Id = Guid.NewGuid().ToString();
        }

        public JwBlock(JwwSolid solid,JwwBlock block)
        {
            var str = new JWPoint(solid.m_start_x, solid.m_start_y);
            var end = new JWPoint(solid.m_end_x, solid.m_end_y);
            var dp = new JWPoint(solid.m_DPoint2_x, solid.m_DPoint2_y);
            BlockPoint = new List<JWPoint>
            {
                str.PianyiXuanzhuan(block),
                end.PianyiXuanzhuan(block),
                dp.PianyiXuanzhuan(block)
            };
            ShapeType = JwBlockShapeType.Triangular;
            if (solid.m_DPoint2_x != solid.m_DPoint3_x || solid.m_DPoint2_y != solid.m_DPoint3_y)
            {
                var dp3 = new JWPoint(solid.m_DPoint3_x, solid.m_DPoint3_y);
                //直接处理四边形
                BlockPoint.Add(dp3.PianyiXuanzhuan(block));
                ShapeType = JwBlockShapeType.Square;
                squareParse();//处理
            }
            ColorInt = solid.m_nPenColor;
            HasParseSuccess = true;
            Id = Guid.NewGuid().ToString();
        }

        public int ColorInt { get; set; }

        /// <summary>
        /// 主要用来 两个三角合并矩形
        /// </summary>
        /// <param name="blockPoints"></param>
        public JwBlock(List<JWPoint> blockPoints)
        {
            BlockPoint= blockPoints;
            if(blockPoints.Count == 4)//暂时只考虑 
            {
                ShapeType = JwBlockShapeType.Square;
                squareParse();
            }
            Id= Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 解析str生成点 判断形状
        /// </summary>
        /// <param name="str"></param>
        private void parseStr(string str)
        {
            BlockPoint=new List<JWPoint>();
            var q = str.Split(" ").ToList();
            if (q.Count > 6)//最低只考虑三角形
            {
                var f = q.First();
                if (f != "sl")
                {
                    HasParseSuccess = false;    
                }
                else
                {
                    q.Remove(f);
                    if (q.Count % 2 == 0)//整除
                    {
                        string x = "";
                        string y = "";
                        for (int i = 1; i < q.Count + 1; i++)
                        {
                            if (i % 2 == 0)
                            {
                                y = q[i-1];
                                JWPoint jw = new JWPoint(x, y);
                                BlockPoint.Add(jw);
                            }
                            else
                            {
                                x = q[i-1];
                            }
                        }
                        HasParseSuccess = true;
                        if (BlockPoint.Count == 3)
                        {
                            ShapeType = JwBlockShapeType.Triangular;
                        }
                        else if(BlockPoint.Count == 4)
                        {
                            ShapeType = JwBlockShapeType.Square;
                            squareParse();
                        }
                        else
                        {
                            ShapeType = JwBlockShapeType.Polygon;
                        }
                    }
                    else
                    {
                        HasParseSuccess = false;
                    }
                }
            }
            else
            {
                HasParseSuccess = false;
            }
        }

        /// <summary>
        /// 通过线来匹配 线是否右重叠 仅考虑 水平或者垂直
        /// </summary>
        private void squareParse()
        {
            BlockLines = new List<JwXian>();
            TopLeft = BlockPoint.OrderBy(p => p.X).ThenByDescending(p => p.Y).ToList().First();
            TopRight=BlockPoint.OrderByDescending(p=>p.X).ThenByDescending(p=>p.Y).ToList().First();
            BottomLeft = BlockPoint.OrderBy(p=>p.X).ThenBy(p=>p.Y).ToList().First();
            BottomRight = BlockPoint.OrderByDescending(p=>p.X).ThenBy(p=>p.Y).ToList().First();
            TopLine=new JwXian(TopLeft, TopRight);
            BlockLines.Add(TopLine);
            LeftLine = new JwXian(BottomLeft,TopLeft);
            BlockLines.Add(LeftLine);
            BottomLine = new JwXian(BottomLeft, BottomRight);
            BlockLines.Add(BottomLine);
            RightLine=new JwXian(BottomRight,TopRight);
            BlockLines.Add(RightLine);
            //CalculateCenter();
            JisuanWidthHeight();
            if (Width == Height || Math.Round(Width, 2) == Math.Round(Height, 2))
            {
                Iszhengfangxing = true;
            }
            HasCenter = true;
        }

        public bool HasCenter { get; set; } = false;


        private void CalculateCenter()
        {
            if (TopLeft != null && TopRight != null && BottomLeft != null && BottomRight != null)
            {
                
                CenterPoint = new JWPoint((TopRight.X + TopLeft.X) / 2, (TopRight.Y + BottomLeft.Y) / 2);
            }
        }

        /// <summary>
        /// 前提都是矩形
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Isjiechu(JwBlock other)
        {
            foreach(var l in BlockLines)
            {
                foreach(var r in other.BlockLines)
                {
                    if (l.IsChongdie(r))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// 前提都是矩形
        /// 2024年5月23日 有bug 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsjiechuNotxianglin(JwBlock other)
        {
            foreach (var l in BlockLines)
            {
                foreach (var r in other.BlockLines)
                {
                    //if(l.IsJiechuNotJiechu(r)) 
                    if (l.IsJiechuNotChongdie(r)) //2024年5月23日
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsAdjacentTo(JwBlock other)
        {
            // Check for horizontal adjacency
            bool horizontallyAdjacent =
                (this.TopRight.X == other.TopLeft.X || this.TopLeft.X == other.TopRight.X) &&
                (Math.Max(this.BottomLeft.Y, other.BottomLeft.Y) < Math.Min(this.TopLeft.Y, other.TopLeft.Y));

            // Check for vertical adjacency
            bool verticallyAdjacent =
                (this.BottomLeft.Y == other.TopLeft.Y || this.TopLeft.Y == other.BottomLeft.Y) &&
                (Math.Max(this.TopLeft.X, other.TopLeft.X) < Math.Min(this.TopRight.X, other.TopRight.X));

            return horizontallyAdjacent || verticallyAdjacent;
        }

        /// <summary>
        /// 有业务bug  dddd示例 两根K柱相邻 解决不了
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IntersectsWith(JwBlock other)
        {
            //为了规避 接触
            if (this.Width == other.Width || this.Height == other.Height)
            {
                return false;
            }
            if (other.TopLeft.X <= this.TopRight.X && this.TopLeft.X <= other.TopRight.X && other.TopLeft.Y >= this.BottomLeft.Y)
            {
                return this.TopLeft.Y >= other.BottomLeft.Y;
            }
            return false;
        }



        public string GetBlockString()
        {
            return "y";
        }

    }

    public class JwBlockComparint : IEqualityComparer<JwBlock>
    {
        public bool Equals(JwBlock? x, JwBlock? y)
        {
            if (object.ReferenceEquals(x, null))
            {
                return false;
            }
            if (object.ReferenceEquals(y, null))
            {
                return false;
            }
            else
            {
                var lst = x.BlockPoint.Union(y.BlockPoint, new JwPointComparint()).Distinct(new JwPointComparint()).ToList();
                if (lst.Count == 4)
                {
                    return true;   
                }
                else
                {
                    if (x.HasCenter && y.HasCenter)
                    {
                        return isnear(x,y);
                    }
                    else
                    {
                        return false;
                    }
                    
                }
            }
        }

        private bool isnear(JwBlock x,JwBlock y)
        {
            var z=Distance(x.CenterPoint,y.CenterPoint);
            if (Math.Abs(z) <= (5d / 100))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private double Distance(JWPoint p1, JWPoint p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public int GetHashCode(JwBlock obj)
        {
            return obj.GetBlockString().GetHashCode();
        }

    }

}
