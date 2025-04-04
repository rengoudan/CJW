using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwMaterialData: BaseGuidEntityData
    {
        /// <summary>
        /// 材料名称 全程 
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 通用标识 缩写
        /// </summary>
        public string GeneralTitle { get; set; }

        /// <summary>
        /// 参数明细
        /// </summary>
        public string MaterialParameter { get; set; }

        [NotMapped]
        public virtual string MaterialDescription
        {
            get { return MaterialName + MaterialParameter; }
        }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        public string UnitName { get; set; }

        /// <summary>
        /// 可以用枚举 梁 柱 金物
        /// </summary>
        public MaterialType MaterialType { get; set; }

        public string Remark { get; set; }

        public string JwMaterialTypeDataId { get; set; }

        public JwMaterialTypeData JwMaterialTypeData { get; set; }
    }
}
