using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwProjectMainData:BaseEntityData
    {
        public JwProjectMainData() {
            Biaochi = "";

        }
        public virtual string ProjectName { get; set; }

        public virtual int? BeamsNumber { get; set; }

        public virtual string? Biaochi { get; set; }

        public virtual int PillarCount { get; set; }

        public virtual int KPillarCount { get; set; }

        public virtual int SinglePillarCount { get; set; }

        public virtual int BCount { get; set; }

        public virtual int BGCount { get; set; }

        

        /// <summary>
        /// 楼层数量 也可是图纸数量
        /// </summary>
        public int FloorQuantity { get; set; }

        /// <summary>
        /// 已处理数量
        /// </summary>
        public int ParsedQuantity { get; set; }

        /// <summary>
        /// 总的导出次数
        /// </summary>
        public virtual int ExportCount { get; set; }

        public ProjectStatus ProjectStatus { get; set; }

        public long? JwCustomerDataId { get; set; }
        public virtual JwCustomerData? JwCustomerData { get; set; } = null!;
        public virtual ObservableCollectionListSource<JwProjectSubData> JwProjectSubDatas { get;  } = new();
    }
}
