using JwCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public class JwKongZu:JwSquareBase
    {
        public double Kongjing { get; set; }

        public int KongNum { get; set; }

        /// <summary>
        /// 孔组中心点
        /// 2 个的话  上下 4个 左上 右上等
        /// </summary>
        public JWPoint Position { get; set; }

        /// <summary>
        /// 基本逻辑为  垂直交会为中间孔 区分200 100 柱所需要为 上面 下面暂时不考虑
        /// </summary>
        public KongzuSuoshuMian SuoShuMian {get;set;}

        /// <summary>
        /// 端部  还是中间
        /// </summary>
        public KongzuType Type { get; set; }  

        /// <summary>
        /// 来源 交会 就是中间面 柱 就是上面
        /// 如果来源是中间 交会链接的  且同样中心位置上方无柱 可同时绘制上下也可不
        /// 其值 只能是top center
        /// </summary>
        public KongzuSuoshuMian Sourec { get; set; }
        
        /// <summary>
        /// 所属beam Id
        /// </summary>
        public string BeamId { get; set; }

        private double _predistance;
        /// <summary>
        /// 距离前一个孔的距离
        /// </summary>
        public double PreDistance 
        {
            get { return _predistance; }
            set
            {
                _predistance = value;
                PreDistanceScale = Math.Round(_predistance) * JwFileConsts.JwScale;
            } 
        }

        /// <summary>
        /// 上述
        /// </summary>
        public double PreDistanceScale { get; set; }

        private double _startdistance { get; set; }
        /// <summary>
        /// 距离  水平 为左边 垂直为下边 的距离
        /// </summary>
        public double StartDistance 
        {
            get { return _startdistance; }
            set
            {
                _startdistance = value;
                StartDistanceScale=Math.Round(_startdistance) * JwFileConsts.JwScale;
            }
        }

        public double StartDistanceScale { get; set; }
    }
}
