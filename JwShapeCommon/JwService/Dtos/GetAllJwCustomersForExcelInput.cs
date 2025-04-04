using System;

namespace RGB.Jw.JW.Dtos
{
    public class GetAllJwCustomersForExcelInput
    {
        public string Filter { get; set; }

        public string CompanyNameFilter { get; set; }

        public string CompanyAddressFilter { get; set; }

        public string ContactFilter { get; set; }

        public string TelephoneFilter { get; set; }

    }
}