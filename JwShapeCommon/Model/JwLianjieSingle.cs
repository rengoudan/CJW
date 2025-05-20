using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Model
{
    public class JwLianjieSingle
    {

        public JwLianjieSingle(JwXian jwXian)
        {

        }

        private void findBeam()
        {

        }

        public JwBeam ShengfangBeam { get; set; }

        public JwBeam BaifangBeam { get; set; }
    }

    public class JwPointBeam
    {
        public JWPoint NearPoint { get;set; }

        public JwBeam JieChuBeam { get; set; }


        public bool IsStart { get; set; }

        public bool IsEnd { get; set; } = false;
    }


}
