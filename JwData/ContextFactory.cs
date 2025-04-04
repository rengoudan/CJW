using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwData
{
    public class ContextFactory
    {
        public static JwDataContext? Context { get; set; }

        public static JwDataContext GetContext()
        {
            if (Context == null)
            {
                Context = new JwDataContext();
            }
            return Context;
        }

        public static void DisposeContext()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }
    }
}
