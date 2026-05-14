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
        public JwDataContext Context { get; }

        public ServiceFactory() 
        { 
            _contextFactory = DbContextFactoryBuilder.CreateFactory(enableLogging: true) ?? 
                throw new InvalidOperationException("数据库工厂创建失败。"); 
            Context = _contextFactory.CreateDbContext();
        }

        private ServiceFactory(string provider, string? connectionString, bool enableLogging)
        {
            _contextFactory = DbContextFactoryBuilder.CreateFactory(
                provider: provider,
                connectionString: connectionString,
                enableLogging: enableLogging
            ) ?? throw new InvalidOperationException("数据库工厂创建失败。");
            Context = _contextFactory.CreateDbContext();
        }

        /// <summary>
        /// 初始化 ServiceFactory（必须在程序启动时调用一次）
        /// </summary>
        public static void Initialize(string provider, string? connectionString = null, bool enableLogging = true)
        {
            _instance = new ServiceFactory(provider, connectionString, enableLogging);
        }


        public JwProjectMainService CreateJwProjectMainService() 
        {
            //return new JwProjectMainService(_contextFactory);
            return new JwProjectMainService(Context);
        }

        public JwqitaService CreateJwqitaService()
        {
            //return new JwqitaService(_contextFactory);
            return new JwqitaService(Context);
        }
    }
}

