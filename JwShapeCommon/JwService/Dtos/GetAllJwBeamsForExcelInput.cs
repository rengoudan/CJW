using Abp.Application.Services.Dto;
using System;

namespace RGB.Jw.JW.Dtos
{
    public class GetAllJwBeamsForExcelInput
    {
        public string Filter { get; set; }

        public int? ShapeTypeFilter { get; set; }

        public decimal? MaxLengthFilter { get; set; }
        public decimal? MinLengthFilter { get; set; }

        public decimal? MaxWidthFilter { get; set; }
        public decimal? MinWidthFilter { get; set; }

        public string BeamCodeFilter { get; set; }

    }
}