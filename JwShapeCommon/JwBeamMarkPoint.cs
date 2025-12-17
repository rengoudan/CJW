using JwCore;
using JwShapeCommon.Model;
using JwwHelper;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    /// <summary>
    /// 表示中心点  一个梁 所拥有的所有 需要在 设计图里体现出的之间的距离
    /// start  水平由左到右， 垂直由下到上
    /// </summary>
    public class JwBeamMarkPoint
    {

        private JwBeam _sourceBeam;

        /// <summary>
        /// beam 起始边际记录
        /// </summary>
        /// <param name="jbeam"></param>
        /// <param name="isbs"></param>
        /// <param name="isbe"></param>
        public JwBeamMarkPoint(JwBeam jbeam, bool isbs=false,bool isbe=false) 
        {
            _sourceBeam = jbeam;    
            if (jbeam.DirectionType == JwCore.BeamDirectionType.Horizontal)
            {
                
                this.IsBeamEnd = isbe;
                this.IsBeamStart = isbs;
                this.IsCenterStart = false;
                this.IsCenterEnd = false;
                this.IsCenter = false;
                this.HasAppend = false;
                 
                if (this.IsBeamStart)
                {
                    this.Point = new JWPoint(jbeam.TopLeft.X, jbeam.Center);
                    this.PreCenterDistance = 0;
                }
                if (this.IsBeamEnd)
                {
                    this.Point = new JWPoint(jbeam.TopRight.X, jbeam.Center);

                }
                this.Coordinate = Math.Round(this.Point.X, 6);

            }
            if (jbeam.DirectionType == JwCore.BeamDirectionType.Vertical)
            {
                this.Point = new JWPoint(jbeam.Center, jbeam.BottomLeft.Y);
                this.IsBeamEnd = isbe;
                this.IsBeamStart = isbs;
                this.IsCenterStart = false;
                this.IsCenterEnd = false;
                this.IsCenter = false;
                this.HasAppend = false;
                 if (this.IsBeamStart)
                {
                    this.Point = new JWPoint(jbeam.Center, jbeam.BottomLeft.Y);
                    this.PreCenterDistance = 0;
                }
                if (this.IsBeamEnd)
                {
                    this.Point = new JWPoint(jbeam.Center, jbeam.TopLeft.Y);
                }
                this.Coordinate = Math.Round(this.Point.Y, 6);
            }

        }

        /// <summary>
        /// 添加中心点 的起始 和结束
        /// </summary>
        /// <param name="jbeam"></param>
        /// <param name="iscenter"></param>
        /// <param name="iscbs"></param>
        /// <param name="iscbe"></param>
        public JwBeamMarkPoint(JwBeam jbeam,bool iscenter, bool iscbs,bool iscbe)
        {
            _sourceBeam = jbeam;
            this.IsCenter=iscenter;
            this.IsCenterStart=iscbs;
            this.IsCenterEnd=iscbe;
            this.HasAppend = false;
        }

        /// <summary>
        /// 2025年4月12日
        /// 根据坐标 生成中心点
        /// </summary>
        public void coordinated()
        {
            if (_sourceBeam?.DirectionType == JwCore.BeamDirectionType.Horizontal)
            {
                this.Point = new JWPoint(this.Coordinate, this._sourceBeam.Center);
            }
            if (_sourceBeam?.DirectionType == JwCore.BeamDirectionType.Vertical)
            {
                this.Point=new JWPoint(this._sourceBeam.Center,this.Coordinate);
            }
            


        }

        public JwBeamMarkPoint()
        {

        }

        public bool IsBeamStart { get; set; }

        public bool IsBeamEnd { get; set; } 

        /// <summary>
        /// 孔组中心点
        /// </summary>
        public bool IsHolePoint { get; set; }


        public bool IsCenterStart { get; set; }

        public bool IsCenterEnd { get; set; }

        /// <summary>
        /// 是否中心点些
        /// </summary>
        public bool IsCenter { get; set; }

        /// <summary>
        /// 包含B的中心点所构成的孔组 理论上 首孔组+各类柱的孔组（holes） +尾孔组
        /// 约定holes仅存由柱或者分割等产生的孔组
        /// B端又由于距离第一个及最后一个柱产生孔组之间的距离分为两类
        /// 1.>=150 常规为4个
        /// 2.<150  为2两个 首保留靠近开始 尾保留靠近结束，但是都是偏离中心点28（即二分之一五十六）
        /// </summary>
        public bool HasAppend { get; set; }

        private JWPoint _point;
        public JWPoint Point 
        {
            get 
            {  
                return _point; 
            }
            set
            {
                _point = value;
            }
        }

        public double Coordinate { get; set; }

        /// <summary>
        /// 针对于0.01 默认梁之间 精确到厘米，
        /// 相对于前一个中心点距离
        /// </summary>
        private double _preCenterDistance;
        public double PreCenterDistance 
        { 
            get 
            {
                return _preCenterDistance;
            } 
            set 
            { 
                _preCenterDistance = value;
                double z = Math.Round(value, 1);
                if (z != value)
                {
                    HasError = true;
                }
                PreCenterCorrect = z;
            } 
        }

        /// <summary>
        /// 修正后的值
        /// </summary>
        public double PreCenterCorrect { get; set; }

        /// <summary>
        /// 相对于梁起始点的距离
        /// </summary>
        public double PreBeamStartDistance { get; set; }

        public JwHole AppendHole { get; set; }


        /// <summary>
        /// 用来指示 孔组是否有中心点（即如果num为2 isbias为true 则location为中心点，孔组需要偏离中心点56/2）
        /// 默认为false
        /// </summary>
        public bool IsBias { get; set; }

        /// <summary>
        /// pr 相对于前一个 是否有误差，
        /// 
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// 相对于梁起始点的相对距离
        /// </summary>
        public double RelativeStartDistance { get; set; } = 0;

        public BeamDirectionType DirectionType 
        {
            get 
            {
                return _sourceBeam.DirectionType;
            }
        }

        public List<JwwData> DrawToJww()
        {
            List<JwwData> jd = new List<JwwData>();
            //var sen = new JwwSen();
            //sen.m_nPenColor = 2;
            //sen.m_start_x = Point.X - 50;
            //sen.m_start_y = Point.Y;
            //sen.m_end_x = Point.X + 50;
            //sen.m_end_y = Point.Y;
            //sen.m_nPenStyle = 1;
            //sen.m_nPenWidth = 0;
            //jd.Add(sen);
            //var sen1 = new JwwSen();
            //sen1.m_nPenColor = 2;
            //sen1.m_start_x = Point.X;
            //sen1.m_start_y = Point.Y - 50;
            //sen1.m_end_x = Point.X;
            //sen1.m_end_y = Point.Y + 50;
            //sen1.m_nPenStyle = 1;
            //sen1.m_nPenWidth = 0;
            //jd.Add(sen1);
            return jd;
        }
    }
}
