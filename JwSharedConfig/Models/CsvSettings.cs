using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwSharedConfig.Models
{
    public class CsvSettings
    {
        public int Hxnum { get; set; } = 1;

        public double Hxjianju { get; set; } = 0.0;

        public int Zxnum { get; set; } = 1;
        public double Zxjianju { get;set; } = 0.0;

        public double Ytiaozheng { get; set;} = 0.0;

        public double Kongjing { get; set; } = 0.0;
    }
}
