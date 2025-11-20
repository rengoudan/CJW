using JwCore;
using JwwHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    /// <summary>
    /// 指示分割点 确定为三角形
    /// 将jwblock 如果是线 将线的点填充进入jwdirected 进行初始化
    /// </summary>
    public class JwDirected
    {

        /// <summary>
        /// 初始化点的集合
        /// </summary>
        public List<JWPoint> _initialPoints;

        public List<JWPoint> Points;

        public JWPoint DirectPoint;

        public bool IsDirected { get; set; } = false;

        public JwDirected() { } 

        public JwDirected(JwwSolid solid)
        {
            _initialPoints = new List<JWPoint>
            {
                new JWPoint(solid.m_start_x, solid.m_start_y),
                new JWPoint(solid.m_end_x, solid.m_end_y),
                new JWPoint(solid.m_DPoint2_x, solid.m_DPoint2_y),
                new JWPoint(solid.m_DPoint3_x, solid.m_DPoint3_y)
            };

            Points = _initialPoints.Distinct(new JwPointComparint()).ToList();
            if (Points.Count == 3)
            {
                parse();
                IsDirected = true;
            }
            
        }

        public JwDirected(JwwBlock block,List<JwwSolid> solids)
        {

        }

        public JwDirected(JwwBlock block,List<JwwSen> sens)
        {
            
        }


        public JwDirected(JwBlock block)
        {
            Points = new List<JWPoint>();
            foreach (var p in block.BlockPoint)
            {
                Points.Add(new JWPoint(p.X, p.Y));
            }
            if(Points.Count== 3)
            {
                parse();
                IsDirected = true;
            }
        }

        public bool IsValidDirected = false;

        public TaggDirect TaggDirect;

        /// <summary>
        /// 切割目标beam 的类型
        /// </summary>
        public BeamDirectionType QieGeBeamDirectionType;

        /// <summary>
        /// 上面水平 则  为 X  垂直 Y
        /// </summary>
        public double? QieGeZhi;

        /// <summary>
        /// 和梁接触 垂直的X 水平的为Y
        /// </summary>
        public double? JiaohuiZhi; 

        /// <summary>
        /// 暂时仅考虑 水平 或垂直 其他方向不考虑
        /// </summary>
        private void parse()
        {
            var lstx = Points.GroupBy(t => t.X).ToList();
            var lsty = Points.GroupBy(t => t.Y).ToList();

            //if (lstx.Count == 2 || lstx.Count == 2)
            //{
            //    IsValidDirected= true;

            //}
            if (lstx.Count == 2)
            {
                IsValidDirected = true;
                QieGeBeamDirectionType = BeamDirectionType.Vertical;
                var tg = lstx.Where(t=>t.Count()==2).FirstOrDefault();
                var og=lstx.Where(t=>t.Count()==1).FirstOrDefault();
                if(og?.Key>tg?.Key)
                {
                    TaggDirect = TaggDirect.Right;
                }
                else
                {
                    TaggDirect=TaggDirect.Left;
                }
                DirectPoint = og.FirstOrDefault();
                QieGeZhi = DirectPoint?.Y;
                JiaohuiZhi = og.Key;
            }

            if(lsty.Count == 2)
            {
                IsValidDirected = true;
                QieGeBeamDirectionType = BeamDirectionType.Horizontal;
                var tg = lsty.Where(t => t.Count() == 2).FirstOrDefault();
                var og = lsty.Where(t => t.Count() == 1).FirstOrDefault();
                if (og?.Key > tg?.Key)
                {
                    TaggDirect = TaggDirect.Up;
                }
                else
                {
                    TaggDirect = TaggDirect.Down;
                }
                DirectPoint = og.FirstOrDefault();
                QieGeZhi = DirectPoint?.X;
                JiaohuiZhi = og.Key;
            }

        }

        public string GetJwDirectString()
        {
            return string.Format("{0}-{1}-{2}", TaggDirect, JiaohuiZhi,QieGeZhi);
        }
    }

    public class JwDirectedComparint : IEqualityComparer<JwDirected>
    {
        public bool Equals(JwDirected? x, JwDirected? y)
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
                if (GetHashCode(x) == GetHashCode(y))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int GetHashCode(JwDirected obj)
        {
            return obj.GetJwDirectString().GetHashCode();
        }

    }

    public class JwSenComparint : IEqualityComparer<JwwSen>
    {
        public bool Equals(JwwSen? x, JwwSen? y)
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
                if (GetHashCode(x) == GetHashCode(y))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int GetHashCode(JwwSen obj)
        {
            return obj.ToString().GetHashCode();
        }

    }
}
