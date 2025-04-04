using System;

namespace RGB.Jw.JW.Dtos
{
    public class GetAllJwLinesForExcelInput
    {
        public string Filter { get; set; }

        public string OriginFilter { get; set; }

        public string EndPointFilter { get; set; }

        public decimal? MaxLineLengthFilter { get; set; }
        public decimal? MinLineLengthFilter { get; set; }

        public int? LineTypeFilter { get; set; }

        public string JwBeamBeamCodeFilter { get; set; }

        public long? JwBeamIdFilter { get; set; }
    }
}