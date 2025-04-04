using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwProjectPathModel
    {
        public string Path { get; set; }

        public JwProjectMainData MainData { get; set; }

        public string FloorName { get; set; }

        public JwMaterialData MaterialData { get; set; }
    }
}
