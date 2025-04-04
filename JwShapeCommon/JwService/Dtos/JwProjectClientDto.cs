using System;

namespace RGB.Jw.JW.Dtos
{
    public class JwProjectClientDto 
    {
        public long Id { get; set; }    
        public string ProjectName { get; set; }

        public string CustomerName { get; set; }

        //public decimal ProjectCost { get; set; }

        //public DateTime? StartDate { get; set; }

        //public DateTime? DueDate { get; set; }

        //public DateTime? DateCompleted { get; set; }

        public string PlaceofDelivery { get; set; }
        public virtual int? BeamsNumber { get; set; }

        public virtual int HorizontalBeamsCount { get; set; }

        public virtual int VerticalBeamsCount { get; set; }

        public virtual string Biaochi { get; set; }

        public virtual int PillarCount { get; set; }

        public virtual int KPillarCount { get; set; }

        public virtual int SinglePillarCount { get; set; }

        public virtual int BCount { get; set; }

        public virtual int BGCount { get; set; }

        public virtual int? JwCustomerId { get; set; }

    }
}