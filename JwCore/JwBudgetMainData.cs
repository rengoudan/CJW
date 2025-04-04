using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwBudgetMainData : BaseGuidEntityData
    {
        public long JwProjectMainDataId { get; set; }

        public virtual JwProjectMainData JwProjectMainData { get; set; } = null!;

        public string ProjectName { get; set; } = string.Empty;

        /// <summary>
        /// 预算总金额
        /// </summary>
        public decimal Amount { get; set; }

        public virtual ObservableCollectionListSource<JwBudgetSubData> JwBudgetSubDatas { get; } = new();
    }
}
