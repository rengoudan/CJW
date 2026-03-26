using JwSharedConfig.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwSharedConfig.Config
{
    public class AppConfig
    {
        //public ApiSettings Api { get; set; } = new();
        public CsvSettings Csv { get; set; } = new();
        //public FeatureFlags Features { get; set; } = new();
    }
}
