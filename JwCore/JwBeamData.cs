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

        public JwProjectSubData JwProjectSubData { get; set; } = null!;

        public ObservableCollectionListSource<JwHoleData> JwHoles { get; } = new();
    }
}
