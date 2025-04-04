using Abp.Application.Services.Dto;
using System;

namespace RGB.Jw.JW.Dtos
{
    public class GetAllJwProjectSubsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string SubNameFilter { get; set; }

        public int? IsCreatedFilter { get; set; }

        public string JwctempFilter { get; set; }

        public DateTime? MaxParseTimeFilter { get; set; }
        public DateTime? MinParseTimeFilter { get; set; }

        public int? MaxBeamCountFilter { get; set; }
        public int? MinBeamCountFilter { get; set; }

        public string JwctempPathFilter { get; set; }

        public string JwProjectProjectNameFilter { get; set; }

        public long? JwProjectIdFilter { get; set; }
    }
}