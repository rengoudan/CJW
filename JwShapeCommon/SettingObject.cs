using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class SettingObject
    {
        public string ParseColor { get; set; } = "lc105";

        public string Overlengthjiagu { get; set; } = "Triangular";

        public const string Rangetag = "hn";

        /// <summary>
        /// 找不到或者未选择 默认未C1
        /// </summary>
        public const string DefaultBeamType = "C1";

        /// <summary>
        /// 默认匹配方法
        /// </summary>
        public const string DefaultParseBeamTypeStr = "";
    }
}
