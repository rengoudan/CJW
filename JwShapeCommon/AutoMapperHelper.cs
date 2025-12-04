using AutoMapper;
using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class AutoMapperHelper
    {

        private static AutoMapperHelper _instance;

        public static AutoMapperHelper GetInstance()
        {
            if(_instance == null)
            {
                _instance = new AutoMapperHelper();
            }
            return _instance;
        }

        MapperConfiguration config ;

        public IMapper GetMapper()
        {
            if(config == null)
            {
                config = new MapperConfiguration(cfg =>
                {
                    // 扫描当前程序集
                    //cfg.AddMaps(System.AppDomain.CurrentDomain.GetAssemblies());
                    cfg.AddMaps(Assembly.GetExecutingAssembly());
                    //cfg.Internal().ForAllPropertyMaps()
                    // 也可以传程序集名称（dll 名称）

                });
            }
            
           return  config.CreateMapper();
        }

    }
}
