using Abp.Application.Services.Dto;

namespace RGB.Jw.JW.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}