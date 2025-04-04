using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using JwShapeCommon;
using JwShapeCommon.JwService;
using JwShapeCommon.JwService.Dtos;
using JwShapeCommon.JwService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RGBJWMain
{
    internal static class Program
    {
        /// <summary>
        /// args zhiding caozuo leixing 0 1 The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var services = new ServiceCollection();
            ConfigureServices(services);
            //先用DI容器生成 serviceProvider, 然后通过 serviceProvider 获取Main Form的注册实例
            var serviceProvider = services.BuildServiceProvider();
            var formMain = serviceProvider.GetRequiredService<JwMainForm>();
            string optype = "";
            InitBaseData();
            if (args.Length >0)
            {
                optype= args[0];
                formMain.Optype= optype;
                Application.Run(new FMain());
                //Application.Run(formMain);
            }
            else
            {
                formMain.Optype = "1";
                //Application.Run(formMain);
                Application.Run(new FMain());
                //Application.Run(new JwMainForm("1"));
            }
        }

        public static void InitBaseData()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ServerUrl))
            {
                JwConsts.ServerUrl = Properties.Settings.Default.ServerUrl;
            }
           
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(JwMainForm));
            IConfigurationBuilder cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json")
                //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true, reloadOnChange: false)
                ;
            
            IConfiguration  configuration = cfgBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);
        }
    }
}