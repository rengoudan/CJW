using JwCore;
using JwwHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JwShapeCommon.Model
{
    /// <summary>
    /// 机加工 孔组所需的信息
    /// 在beam 的holeorder方法里 实现  对于一个孔组的 需要增加两个
    /// 内部实现tocsv的方法
    /// </summary>
    public class JwHoleMachining
    {

        public string Id { get; set; }

        public double RelativeStartDistance { get; set; }

        /// <summary>
        /// 用来绘制jww 使用 水平此为X  垂直此为Y  然后通过center 生成具体的location
        /// </summary>
        public double RealLocation { get; set; }

        /// <summary>
        /// 工字梁上面
        /// </summary>
        public bool HasLeft { get; set; }


        /// <summary>
        /// 工字梁下面
        /// </summary>
        public bool HasRight { get; set; }

        /// <summary>
        /// 工字梁中间
        /// </summary>
        public bool HasTop { get; set; }


        public string ToCsvString(double y)
        {

            //double ysjv = Math.Abs(Y) + .CsvYwucha;
            return string.Format("{3},絶対,先端,{0},{1},1,{2},1,0.0,\r\n", RelativeStartDistance.ToString("0.0"), y.ToString("0.0"), "0.0", JwFileConsts.EllipseDiameter.ToString("0.0"));
            //return $"{Id},{RelativeStartDistance},{HasLeft},{HasRight},{HasTop}";
        }

        /// <summary>
        /// 根据beam的baifangdistancetype
        /// 右上左下
        /// </summary>
        /// <param name="isadd"></param>
        /// <returns></returns>
        public JwHoleMachining GTopBottomAddHole(bool isadd)
        {
            var offset = JwFileConsts.Kongjing / JwFileConsts.JwScale;
            JwHoleMachining result = new JwHoleMachining();
            result.Id = Guid.NewGuid().ToString();
            result.RelativeStartDistance = isadd ? this.RelativeStartDistance + offset * JwFileConsts.JwScale : this.RelativeStartDistance - offset * JwFileConsts.JwScale;
            result.RealLocation = isadd ? this.RealLocation + offset : this.RealLocation - offset;
            result.HasLeft = true;
            result.HasRight = true;
            result.HasTop = false;
            return result;
        }

        public JwHoleMachining GTopBottomAddHole(bool isadd, BaiFangGTBDistanceType baiFangGTB)
        {
            var offset = JwFileConsts.Kongjing / JwFileConsts.JwScale;
            JwHoleMachining result = new JwHoleMachining();
            result.Id = Guid.NewGuid().ToString();
            result.RelativeStartDistance = isadd ? this.RelativeStartDistance + offset : this.RelativeStartDistance - offset;
            result.RealLocation = isadd ? this.RealLocation + offset : this.RealLocation - offset;
            result.HasLeft = true;
            result.HasRight = true;
            result.HasTop = false;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">为beamstart的coordrate</param>
        /// <param name="cy">cy 为绘制的梁面的中心线的y</param>
        /// <returns></returns>
        public List<JwwData> DrawToJww(double offset,double cy)
        {
            var result = new List<JwwData>();
            var banjing = JwFileConsts.EllipseDiameter / (2 * JwFileConsts.JwScale);
            var halfy=JwFileConsts.EllipseSpacing/ (2 * JwFileConsts.JwScale);
            JwwEnko enkoup = new JwwEnko();
            enkoup.m_nPenColor = 2;
            enkoup.m_dHankei = banjing;
            enkoup.m_radKaishiKaku = 0;
            enkoup.m_radEnkoKaku = 6.2831853;
            enkoup.m_radKatamukiKaku = 0;
            enkoup.m_dHenpeiRitsu = 1;
            enkoup.m_bZenEnFlg = 1;
            enkoup.m_start_x = this.RealLocation-offset;
            enkoup.m_start_y = cy + halfy;
            enkoup.m_nLayer = (int)DrawShapeType.Beam + 1;
            result.Add(enkoup);
            JwwEnko enkodown = new JwwEnko();
            enkodown.m_nPenColor = 2;
            enkodown.m_dHankei = banjing;
            enkodown.m_radKaishiKaku = 0;
            enkodown.m_radEnkoKaku = 6.2831853;
            enkodown.m_radKatamukiKaku = 0;
            enkodown.m_dHenpeiRitsu = 1;
            enkodown.m_bZenEnFlg = 1;
            enkodown.m_start_x = this.RealLocation - offset;
            enkodown.m_start_y = cy - halfy;
            enkodown.m_nLayer = (int)DrawShapeType.Beam + 1;
            result.Add(enkodown);
            return result;
        }
    }

    public class JwHoleMachiningComparint : IEqualityComparer<JwHoleMachining>
    {
        public bool Equals(JwHoleMachining? x, JwHoleMachining? y)
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
                return x.Id.Equals(y.Id);
            }
        }

        public int GetHashCode(JwHoleMachining obj)
        {
            return obj.Id.GetHashCode();
        }

    }
}
