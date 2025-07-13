using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwCore
{
    public class JwLianjieData: IHasCreateFrom
    {
        public JwLianjieData()
        {
            //Id = Guid.NewGuid().ToString();
        }

        public JwLianjieData(bool f)
        {
            Id = Guid.NewGuid().ToString();
            Start = new Point(0, 0);
            End = new Point(0, 0);
            //Length = 0;
            //ProjectSubName = string.Empty;
            //JwProjectSubDataId = string.Empty;
        }

        public Point Start { get; set; }

        public Point End { get; set; }

        /// <summary>
        /// 实际的长度单位mM
        /// </summary>
        public double Length { get; set; }

        public string ProjectSubName { get; set; }

        public string Id { get; set; }
        public string JwProjectSubDataId { get; set; }
        public JwProjectSubData JwProjectSubData { get; set; } = null!;
        public CreateFromType CreateFrom { get; set; }
    }

    //public class JwLianjieGroupData
    //{
    //    public decimal Length { get; set; }

    //    public int Num { get; set; }

    //    public string ProjectSubName { get; set; }
    //}

}
