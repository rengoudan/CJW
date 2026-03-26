using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwSharedConfig.Config
{
    public interface IConfigProvider
    {
        AppConfig Load();
        void Save(AppConfig config);
    }
}
