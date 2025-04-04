using System;
namespace RGB.Jw.JW.Dtos
{
    public class JwCustomerDesignTagDto 
    {
        public long Id { get; set; }
        public string DesignSymbol { get; set; }

        public string ModelParm { get; set; }

        public string UnitName { get; set; }

        public string ComponentsName { get; set; }

        public decimal UnitPrice { get; set; }

        public string Remark { get; set; }

        public string PropertyName { get; set; }

        public long? JwBaseDataId { get; set; }

        public int? JwCustomerId { get; set; }

    }
}