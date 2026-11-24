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

        public ParseSettings Parse { get; set; } = new();

        /// <summary>2026年11月22日 画面操作（出力先フォルダの記憶など）</summary>
        public UiSettings Ui { get; set; } = new();

        /// <summary>2026年11月24日 データの保存先（ローカル SQLite／リモート SQL Server）。再起動後に反映。</summary>
        public DatabaseSettings Database { get; set; } = new();
        //public FeatureFlags Features { get; set; } = new();
    }
}
