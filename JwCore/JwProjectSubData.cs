using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwProjectSubData: JwSquareBaseData
    {
        public virtual string? FloorName { get; set; }

        public virtual int BeamCount { get; set; }

        public virtual int HorizontalBeamsCount { get; set; }

        public virtual int VerticalBeamsCount { get; set; }

        public virtual string? Biaochi { get; set; }

        /// <summary>
        /// 梁符号
        /// </summary>
        public virtual int? MarkBeam { get; set; }

        public virtual int PillarCount { get; set; }

        public virtual int KPillarCount { get; set; }

        public virtual int SinglePillarCount { get; set; }

        public virtual int BCount { get; set; }

        public virtual int BGCount { get; set; }

        /// <summary>
        /// 默认梁的 型号
        /// </summary>
        public virtual string DefaultBeamXHId { get; set; } = string.Empty;

        public virtual string DefaultBeamXHName { get; set; } = string.Empty;

        /// <summary>
        /// 导出次数
        /// </summary>
        public virtual int ExportCount { get; set; }

        public long JwProjectMainDataId { get; set; }
        public virtual JwProjectMainData JwProjectMainData { get; set; } = null!;

        public virtual ObservableCollectionListSource<JwBeamData> JwBeamDatas { get; } = new();

        public virtual ObservableCollectionListSource<JwPillarData> JwPillarDatas { get; } = new();

        public virtual ObservableCollectionListSource<JwLinkPartData> JwLinkPartDatas { get; } = new();

        public virtual ObservableCollectionListSource<JwLianjieData> JwLianjieDatas { get; } = new();
    }
}
