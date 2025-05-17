using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwCustDesignConstData: BaseEntityData
    {
        public int PickPrecision { get; set; }


        public double JwJianxi { get; set; }

        /// <summary>
        /// bili
        /// </summary>
        public double JwScale { get; set; }

        public int BeamParseColorNumber { get; set; }

        public int BeamSplitParseColor { get; set; }

        public int BeamPillarParseColor { get; set; }

        public int PillarPenStyle { get; set; }


        /// <summary>
        /// 切割标识距离梁距离
        /// </summary>
        public double NearSpliteMax { get; set; }

        public int SplitPenStyle { get; set; }

        public int BeamSymbolTextColorNumber { get; set; }

        /// <summary>
        /// 叉号标识下方有柱
        /// </summary>
        public int DownPillarColorNumber { get; set; }

        ///// <summary>
        ///// BR金屋识别规则 
        ///// </summary>
        //public int BRParseColore { get; set; }

        //public int BRParseStyle { get; set; }

        public int LinkColorNumber { get; set; }

        public long? JwCustomerDataId { get; set; }
        public virtual JwCustomerData? JwCustomerData { get; set; } = null!;

    }
}
