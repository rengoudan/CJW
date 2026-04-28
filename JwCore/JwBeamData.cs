using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwBeamData: JwSquareBaseData
    {
        public string BeamCode { get; set; } = "";

        public string FloorName { get; set; } = "";

        public string GongQu { get; set; } = "";

        public bool HasQieGe { get; set; } = false;

        public int QieGeCount { get; set; }

        /// <summary>
        /// 是否为父beam  只有切割 才为True
        /// </summary>
        public bool IsParentBeam { get; set; } = false;

        public bool IsQiegeBeam { get; set; } = false;

        public double Length { get; set; }

        public double XXLength { get; set; }

        public bool HasStartSide { get; set; }

        public bool HasEndSide { get; set; }

        public KongzuType StartTelosType { get; set; }

        public KongzuType EndTelosType { get; set; }

        public string JwProjectSubDataId { get; set; }

        /// <summary>
        /// 默认梁的 型号
        /// </summary>
        public virtual string BeamXHId { get; set; } = string.Empty;

        public virtual string BeamXHName { get; set; } = string.Empty;

        public double StartCenter { get; set; }

        /// <summary>
        /// 中心终点
        /// </summary>
        public double EndCenter { get; set; }

        /// <summary>
        /// 2026年3月28日 是否有败方G
        /// </summary>
        public bool HasBFG { get; set; }

        public string InitialBeamCode { get; set; } = "";

        /// <summary>
        /// 同上 在选择查看梁的时候 如果上面的属性为true则允许设置此
        /// </summary>
        public BaiFangGTBDistanceType BaiFangGTBDistance { get; set; } = BaiFangGTBDistanceType.A35;
        /// <summary>
        /// 2026年4月21日 增加梁指纹的概念
        /// </summary>
        public string BeamSignature { get; set; } = "";
        public JwProjectSubData JwProjectSubData { get; set; } = null!;

        public ObservableCollectionListSource<JwHoleData> JwHoles { get; } = new();

        public ObservableCollectionListSource<JwBeamVerticalData> JwBeamVerticalDatas { get; } = new();
    }

    public class JwBeamHuiZong
    {
        public string BeamSignature { get; set; }

        public string InitialBeamCode { get; set; }

        public string BeamCode { get; set; }

        public int BeamCount { get; set; }

        public double TotalLength { get; set; }

        public double TotalXXLength { get; set; }

        public string Remark { get; set; } 

        public List<JwBeamData> BeamDatas { get; set; }
    }
}
