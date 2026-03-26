using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwSharedConfig.Config
{
    public static class AppConfigManager
    {
        private static IConfigProvider _provider = new JsonConfigProvider();
        private static AppConfig? _cache;

        public static void UseProvider(IConfigProvider provider)
        {
            _provider = provider;
            _cache = null;
        }

        public static AppConfig Current
        {
            get
            {
                if (_cache == null)
                    _cache = _provider.Load();

                return _cache;
            }
        }

        public static void Save()
        {
            if (_cache != null)
                _provider.Save(_cache);
        }
    }
}
