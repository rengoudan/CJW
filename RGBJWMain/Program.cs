using AutoUpdaterDotNET;
using JwShapeCommon;
using JwShapeCommon.JwService;
using JwShapeCommon.JwService.Dtos;
using JwShapeCommon.JwService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

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
            //����DI�������� serviceProvider, Ȼ��ͨ�� serviceProvider ��ȡMain Form��ע��ʵ��
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

        public static void z()
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

        //private void CheckDllUpdate()
        //{
        //    //AutoUpdater.CheckForUpdateEvent += OnDllUpdateCheck;
        //    AutoUpdater.CheckForUpdateEvent += (args) =>
        //    {
                
        //        // ��ȡ DLL �İ汾��
        //        string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BusinessLogic.dll");
        //        Version localDllVersion = Assembly.LoadFrom(dllPath).GetName().Version;
        //        Version serverVersion = new Version(args.CurrentVersion);

        //        if (serverVersion > localDllVersion)
        //        {
        //            //DialogResult result = MessageBox.Show(
        //            //    $"���� DLL �°汾 {serverVersion}���Ƿ���£�\n\n{args.Changelog}",
        //            //    "������ʾ", MessageBoxButtons.YesNo);


        //            // �ֶ����� ZipExtractor.exe
        //            string zipExtractorPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZipExtractor.exe");
        //            Process.Start(zipExtractorPath, $"\"{Application.ExecutablePath}\" \"{args.DownloadURL}\"");
        //            Application.Exit();

        //            //if (result == DialogResult.Yes)
        //            //{

        //            //}
        //        }
        //    };

        //    AutoUpdater.Start("");
        //}

        

    }
}