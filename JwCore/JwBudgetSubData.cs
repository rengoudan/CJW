using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwBudgetSubData:BaseGuidEntityData
    {

        /// <summary>
        /// 预算项
        /// </summary>
        public string BudgetItemName { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal Amount { get; set; }

        ///// <summary>
        ///// 是否匹配到 预算项目，
        ///// </summary>
        //public bool IsParse { get; set; }

        /// <summary>
        /// 等同 参数
        /// </summary>
        public string ModelParm { get; set; }=string.Empty;

        /// <summary>
        /// 取代上述
        /// </summary>
        public BudgetFrom BudgetType { get; set; }

        /// <summary>
        /// 梁 柱 金物
        /// </summary>
        public MaterialType MaterialType { get; set; }

        public string JwMaterialDataId { get; set; } =string.Empty;
        public virtual JwMaterialData? JwMaterialData { get; set; }

        public string JwBudgetMainDataId { get; set; }

        //[ForeignKey("JwBudgetMainDataId")]
        public virtual JwBudgetMainData JwBudgetMainData { get; set; }= null!;


    }
}
