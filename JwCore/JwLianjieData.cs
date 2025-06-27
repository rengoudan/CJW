using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwLianjieData
    {

        public Point Start { get; set; }

        public Point End { get; set; }

        /// <summary>
        /// 实际的长度单位mM
        /// </summary>
        public decimal Length { get; set; }

        public string ProjectSubName { get; set; }

        public string Id { get; set; }
        public string JwProjectSubDataId { get; set; }
        public JwProjectSubData JwProjectSubData { get; set; } = null!;
    }

}
