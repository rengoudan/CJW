using JwData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwServices
{
    public class ServiceFactory
    {
        private readonly IDbContextFactory<JwDataContext> _contextFactory;

        private static ServiceFactory _instance = null;

        public static ServiceFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ServiceFactory();
            }
            return _instance;
        }


        public ServiceFactory() 
        { 
            _contextFactory = DbContextFactoryBuilder.CreateFactory(enableLogging: true) ?? 
                throw new InvalidOperationException("数据库工厂创建失败。"); 
        }


        public JwProjectMainService CreateJwProjectMainService() 
        { 
            return new JwProjectMainService(_contextFactory);
        }

        public JwqitaService CreateJwqitaService()
        {
            return new JwqitaService(_contextFactory);
        }
    }
}
