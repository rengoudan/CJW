using Abp.Application.Services.Dto;
using System;

namespace RGB.Jw.JW.Dtos
{
    public class GetAllJwCustomersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CompanyNameFilter { get; set; }

        public string CompanyAddressFilter { get; set; }

        public string ContactFilter { get; set; }

        public string TelephoneFilter { get; set; }

    }
}