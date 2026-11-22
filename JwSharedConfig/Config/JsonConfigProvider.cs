using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JwSharedConfig.Config
{
    public class JsonConfigProvider : IConfigProvider
    {
        private readonly string _filePath;

        public JsonConfigProvider(string filePath = "appconfig.json")
        {
            // 2026年11月22日 相対パスはプログラムのフォルダ基準にする。
            // 原来按「当前目录」解析：运行中当前目录被改变时（部分文件对话框会改），设置会读写到别的文件夹。
            // 通常启动时当前目录就是程序文件夹，所以读写的仍是原来那个文件。
            _filePath = Path.IsPathRooted(filePath) ? filePath : Path.Combine(AppContext.BaseDirectory, filePath);
        }

        public AppConfig Load()
        {
            if (!File.Exists(_filePath))
                return new AppConfig();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
        }

        public void Save(AppConfig config)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }
    }
}
