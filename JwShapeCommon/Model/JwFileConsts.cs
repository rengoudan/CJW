using JwShapeCommon.Jwbase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public static class JwFileConsts
    {

        /// <summary>
        /// 单位为mm 实际的宽度
        /// </summary>
        public static double KWidth = 1000;

        /// <summary>
        /// 同上实际的 单位为mm
        /// </summary>
        public static double KErrorMargin = 60;

        public static System.Drawing.Color[] JwFileColors =new System.Drawing.Color[] { System.Drawing.Color.Red, System.Drawing.Color.Green, System.Drawing.Color.Yellow, System.Drawing.Color.Blue, System.Drawing.Color.White, System.Drawing.Color.Purple, System.Drawing.Color.Orange, System.Drawing.Color.Sienna,System.Drawing.Color.Navy, System.Drawing.Color.LightCoral, Color.LawnGreen, Color.LightBlue, Color.LightGoldenrodYellow, Color.Magenta };

        public static int BRParseColore = 1;

        public static int BRParseStyle = 1;


        public static double MaxBeamScope = 100;

        /// <summary>
        /// 文字中点 和所标识对象中间距离最大偏差值
        /// </summary>
        public static double TextParseMaxDistance = 100;

        public static double TextParseChuizhiMaxDistance = 100;

        public static double Lianghoudu = 5.5;

        public static double Gjubian = 35;

        public static double GBianjuZhongxin = 55;

        /// <summary>
        /// B构建 孔间距
        /// </summary>
        public static double Kongjing = 56;

        /// <summary>
        /// jww
        /// 转换过来的 读取到小数点第几位 0的话 不做改变
        /// </summary>
        public static int PickPrecision = 6;

        /// <summary>
        /// 现实里的 孔直径  需要 按比例计算
        /// </summary>
        public static double EllipseDiameter = 17;

        /// <summary>
        /// 现实里的孔距 同上
        /// </summary>
        public static double EllipseSpacing = 56;

        //标准间隙 图 8厘米？  程序里需要除 jwscale
        public static double JwJianxi = 120;

        public static double GoujianWidth = 100;

        public static double GoujianHeight = 48;

        /// <summary>
        /// 切割标识距离
        /// </summary>
        public static double NearSpliteMax = 1;

        /// <summary>
        /// jw 设计图纸 标注的缩放比例 默认为100
        /// </summary>
        public static double JwScale = 100;

        /// <summary>
        /// 获取保留小数点的位数
        /// </summary>
        public static int JiangjingduInt
        {
            get
            {
               return  (int)Math.Log(JwFileConsts.JwScale, 10);
            }
        }

        /// <summary>
        /// 识别梁的 颜色编号
        /// </summary>
        public static int BeamParseColorNumber = 0;

        /// <summary>
        /// 梁xia
        /// </summary>
        public static JwColor BeamParseColor;

        /// <summary>
        /// 梁的分割标识符
        /// </summary>
        public static JwColor BeamSplitParseColor;

        /// <summary>
        /// 柱子 识别颜色
        /// </summary>
        public static JwColor BeamPillarParseColor;

        public static JwPenStyle PillarPenStyle;

        public static JwPenStyle SplitPenStyle;

        /// <summary>
        /// 施工链接部件
        /// </summary>
        public static JwColor LianjieParseColor;

        /// <summary>
        /// 识别梁旁边的类型 文字颜色编号
        /// </summary>
        public static int BeamSymbolTextColorNumber = 0;

        public static JwColor BeamSymbolTextColor;

        public static List<JwColor> JwColors;

        public static List<JwColor> GetJwColors()
        {
            if(JwColors == null)
            {
                JwColors=new List<JwColor>();
                for (int i = -1; i < 15; i++)
                {
                    string s;
                    if (i == -1)
                    {
                        s = "線⾊番号";
                    }
                    else
                    {
                        s = string.Format("lc{0}", i);
                    }
                    
                    JwColors.Add(new JwColor { JwColorName = s, ColorNumber = i });
                }
            }
            return JwColors;
        }


        public static List<JwPenStyle> JwPenStyles;

        public static List<JwPenStyle> GetJwPenStyles()
        {
            if(JwPenStyles == null)
            {
                JwPenStyles=new List<JwPenStyle>();
                for(int i = -1;i < 15; i++)
                {
                    string s;
                    if (i == -1)
                    {
                        s = "線種番号";
                    }
                    else
                    {
                        s = string.Format("ls{0}", i);
                    }
                    JwPenStyles.Add(new JwPenStyle { JwPenStyleName=s,StyleNumber = i });
                }
            }
            return JwPenStyles;
        }

    }
}
