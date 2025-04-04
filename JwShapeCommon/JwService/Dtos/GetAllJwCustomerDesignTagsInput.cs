using Abp.Application.Services.Dto;
using System;

namespace RGB.Jw.JW.Dtos
{
    public class GetAllJwCustomerDesignTagsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DesignSymbolFilter { get; set; }

        public string ModelParmFilter { get; set; }

        public string UnitNameFilter { get; set; }

        public string ComponentsNameFilter { get; set; }

        public decimal? MaxUnitPriceFilter { get; set; }
        public decimal? MinUnitPriceFilter { get; set; }

        public string RemarkFilter { get; set; }

        public string PropertyNameFilter { get; set; }

        public string JwBaseDataDataNameFilter { get; set; }

        public string JwCustomerCompanyNameFilter { get; set; }

    }
}