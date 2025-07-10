using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public interface IHasCreateFrom
    {
        public CreateFromType CreateFrom { get; set; }
    }

    public enum CreateFromType
    {
        Analysis = 0,
        ManuallyAdd= 1,
    }
}
