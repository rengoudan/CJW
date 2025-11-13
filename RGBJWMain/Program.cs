using AutoUpdaterDotNET;
using JwShapeCommon;
using JwShapeCommon.JwService;
using JwShapeCommon.JwService.Dtos;
using JwShapeCommon.JwService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RGBControls;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // 创建一个隐藏窗体，用于处理更新逻辑
            using (Form tempForm = new Form { ShowInTaskbar = false, WindowState = FormWindowState.Minimized })
            {
                tempForm.Load += (s, e) =>
                {
                    AutoUpdater.CheckForUpdateEvent += (updateArgs) =>
                    {
                        tempForm.Invoke(new Action(() =>
                        {
                            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RGBControls.dll");
                            Version localDllVersion = Assembly.LoadFrom(dllPath).GetName().Version;
                            Version serverVersion = new Version(updateArgs.CurrentVersion);

                            if (serverVersion > localDllVersion)
                            {
                                DialogResult result = MessageBox.Show(
                                    $"新しいバージョンのDLLが見つかりました {serverVersion}，アップデート？\n\n",
                            "アップデートのヒント", MessageBoxButtons.YesNo);

                                if (result == DialogResult.Yes)
                                {
                                    AutoUpdater.DownloadUpdate(updateArgs);
                                    System.Threading.Thread.Sleep(3000);
                                    Application.Exit();
                                }
                                else
                                {
                                    tempForm.Close(); // 关闭临时窗体，继续启动主窗体
                                }
                            }
                            else
                            {
                                tempForm.Close(); // 没有更新，继续启动主窗体
                            }
                        }));
                    };

                    AutoUpdater.Start("https://www.rgballwin.com/zupdate.xml");
                };

                tempForm.ShowDialog(); // 用 ShowDialog 启动消息循环
            }

            // 更新处理完毕后，启动主窗体
            Application.Run(new MainForm());
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

        private static void CheckDllUpdate()
        {
            //AutoUpdater.CheckForUpdateEvent += OnDllUpdateCheck;
            AutoUpdater.CheckForUpdateEvent += (args) =>
            {

                // 获取 DLL 的版本号
                string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RGBControls.dll");
                Version localDllVersion = Assembly.LoadFrom(dllPath).GetName().Version;
                Version serverVersion = new Version(args.CurrentVersion);

                if (serverVersion > localDllVersion)
                {
                    DialogResult result = MessageBox.Show(
                        $"新しいバージョンのDLLが見つかりました {serverVersion}，アップデート？\n\n",
                        "アップデートのヒント", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        // 手动调用 ZipExtractor.exe
                        //string zipExtractorPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZipExtractor.exe");
                        //Process.Start(zipExtractorPath, $"\"{Application.ExecutablePath}\" \"{args.DownloadURL}\"");
                        AutoUpdater.DownloadUpdate(args);
                        Application.Exit();
                        //Application.Exit();
                    }
                    else
                    {
                        Application.Run(new FMain());
                    }
                }
                else
                {
                    Application.Run(new FMain());
                }
            };

            AutoUpdater.Start("https://www.rgballwin.com/zupdate.xml");
        }



    }
}