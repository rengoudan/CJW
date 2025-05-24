using JwCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    /// <summary>
    /// 如果线是用来链接 相互平行的 水平梁， start为X 小的  start相对BG孔组 需要加 即在右边
    /// end 为X 大的  需要减  即在接触的左边
    /// 如果相互平行为的垂直梁  start 为y小的 相对于bg孔组 需要加 即在上
    /// </summary>
    public class JwLianjieSingle
    {
        public JwXian Xian { get; set; }

        public JwLianjieSingle(JwXian jwXian)
        {

        }

        /// <summary>
        /// 指示连接线用来链接 垂直 还是水平的胜方梁
        /// </summary>
        public BeamDirectionType DirectionType { get; set; }


        /// <summary>
        /// 排序规则  由下到上  由左到右 根据胜方梁
        /// </summary>
        public JwPointBeam Start { get; set; }

        public JwPointBeam End { get; set; }

        
    }

    /// <summary>
    /// 连接线的 端点  
    /// </summary>
    public class JwPointBeam
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="touch"></param>
        public JwPointBeam(JWPoint point,JwTouch touch)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="touch"></param>
        /// <param name="istart"></param>
        public JwPointBeam(JWPoint point,JwTouch touch,bool istart, ZhengfuType direct)
        {
            this.LianjieType=touch.WinnerBeam.DirectionType;
            this.IsStart=istart;
            this.IsEnd=!istart;
            NearPoint = point;
            this.Direct= direct;
            calculate();
        }
        private void calculate()
        {
            if (this.LianjieType == BeamDirectionType.Horizontal)
            {
                double pinayix = this.IsStart ? 50 : -50;

                double realy = Touch.WinnerBeam.Center + ((int)Direct) * 50;

                double realx = Touch.LoserBeam.Center + pinayix;
                this.RealPoint=new JWPoint(realx, realy);
            }
            else
            {
                double pinayix = this.IsStart ? 50 : -50;
                double realx = Touch.WinnerBeam.Center + ((int)Direct) * 50;

                double realy= Touch.LoserBeam.Center + pinayix;
            }
        }

        /// <summary>
        /// 设计图里表示的 接触点
        /// </summary>
        public JWPoint NearPoint { get;set; }

        public JWPoint RealPoint { get; set; }

        /// <summary>
        /// 败方梁
        /// </summary>
        public JwBeam JieChuBeam { get; set; }

        /// <summary>
        /// 对应胜方梁 所属的梁
        /// </summary>
        public JwBeam ShengfangBeam { get; set; }

        /// <summary>
        /// 点关联的接触方
        /// </summary>
        public JwTouch Touch { get; set; }

        /// <summary>
        /// touch isshuping  true 判断 pone ptwo 的 Y大小 大的 减 小的加 false 判断 X大小 同样
        /// </summary>
        public double OffsetLosetCenter { get; set; }


        /// <summary>
        /// 暂无用
        /// 2025年5月24日 标识两个 相互平行的需要链接梁 如果 是水平 start 为X小的 偏移需要加
        /// 相互平行的梁为垂直的梁  start为Y 小的  偏移需要加   
        /// </summary>
        public bool IsStart { get; set; }

        /// <summary>
        /// 2025年5月24日  
        /// 约定好end 偏移为减   
        /// </summary>
        public bool IsEnd { get; set; } = false;


        /// <summary>
        /// 水平 为 链接两个水平的梁，即loserbeam为垂直的败
        /// 上述情况 direct 分为两种 上下， 
        /// 如果起点为上，则 实际Y 的计算取win梁的中心线 +50
        /// 如果方向为下，则 实际Y 的计算取win梁的中心线 -50
        /// 垂直 为 链接两个垂直的梁，即loserbeam为水平的败
        /// left 为X -50
        /// right为Y +50
        /// 
        /// </summary>
        public BeamDirectionType LianjieType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ZhengfuType Direct { get; set; }
    }


}
