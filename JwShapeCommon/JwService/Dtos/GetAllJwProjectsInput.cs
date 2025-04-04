using Abp.Application.Services.Dto;
using System;

namespace RGB.Jw.JW.Dtos
{
    public class GetAllJwProjectsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ProjectNameFilter { get; set; }

        public string CustomerNameFilter { get; set; }

        public decimal? MaxProjectCostFilter { get; set; }
        public decimal? MinProjectCostFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxDueDateFilter { get; set; }
        public DateTime? MinDueDateFilter { get; set; }

        public DateTime? MaxDateCompletedFilter { get; set; }
        public DateTime? MinDateCompletedFilter { get; set; }

        public string PlaceofDeliveryFilter { get; set; }

        public int? MaxBeamsNumberFilter { get; set; }
        public int? MinBeamsNumberFilter { get; set; }

        public string CreateDesignerFilter { get; set; }

        public string JwCustomerCompanyNameFilter { get; set; }

    }
}