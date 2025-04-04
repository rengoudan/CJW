using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    [Serializable]
    public abstract class BaseEntityData
    {
        public virtual long Id { get; set; }

        public virtual DateTime CreationTime { get; set; }

        protected BaseEntityData()
        {
            CreationTime = DateTime.Now;
        }
    }

    [Serializable]
    public abstract class BaseGuidEntityData
    {
        public virtual string Id { get; set; }

        public virtual DateTime CreationTime { get; set; }

        protected BaseGuidEntityData()
        {
            Id = Guid.NewGuid().ToString();
            CreationTime = DateTime.Now;
        }
    }
}
