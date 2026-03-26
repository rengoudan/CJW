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
            _filePath = filePath;
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
