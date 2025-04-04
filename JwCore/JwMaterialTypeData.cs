using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwMaterialTypeData:BaseGuidEntityData
    {
        public string MaterialTypeName { get; set; }

        public string DefaultDataId { get; set; } = string.Empty;

        public int MaterialCount {  get; set; }

        /// <summary>
        /// 可以用枚举 梁 柱 金物 大类
        /// </summary>
        public MaterialType MaterialType { get; set; }

        public virtual ObservableCollectionListSource<JwMaterialData> JwMaterialDatas { get; } = new();

    }
}
