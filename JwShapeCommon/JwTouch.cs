using JwShapeCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwTouch
    {
        /// <summary>
        /// 胜方梁
        /// </summary>
        public JwBeam WinnerBeam { get; set; }

        public JwBeamVertical JwBeamVertical { get; set; }

        /// <summary>
        /// 败方梁
        /// </summary>
        public JwBeam LoserBeam { get; set; }
    }

    public class JwChengduiXian
    {
        public JwXian XianOne { get; set; }

        public JwXian XianTwo { get; set; }

        public List<JwXian> Xians  { get; set; }
    }
}
