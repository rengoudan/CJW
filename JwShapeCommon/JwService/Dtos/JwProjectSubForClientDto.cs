using System;

namespace RGB.Jw.JW.Dtos
{
    public class JwProjectSubForClientDto
    {
        public long Id { get; set; }
        public string ProjectName { get; set; }


        public string SubName { get; set; }

        public virtual int? BeamCount { get; set; }

        public virtual int HorizontalBeamsCount { get; set; }

        public virtual int VerticalBeamsCount { get; set; }

        public virtual string Biaochi { get; set; }

        public virtual int PillarCount { get; set; }

        public virtual int KPillarCount { get; set; }

        public virtual int BCount { get; set; }

        public virtual int BGCount { get; set; }

        public virtual int SinglePillarCount { get; set; }

        public DateTime ParseTime { get; set; }

        public string CustomerName { get; set; }
    }
}