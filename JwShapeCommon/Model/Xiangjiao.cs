using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public class Xiangjiao
    {
        public JwXian seg1 { get; set; }

        public JwXian seg2 { get; set; }

        public JWPoint Jiaodian { get; set; }

        public bool IsXiangjiao { get; set; }

        public List<JwXian> jwXians { get; set; }

        public JwXian GetMaxLengthXian()
        {
            if (seg1.Distance() >= seg2.Distance())
            {
                return seg1;
            }
            else
            {
                return seg2;
            }
        }
    }
}
