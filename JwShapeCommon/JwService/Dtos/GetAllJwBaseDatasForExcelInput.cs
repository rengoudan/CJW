using Abp.Application.Services.Dto;
using System;

namespace RGB.Jw.JW.Dtos
{
    public class GetAllJwBaseDatasForExcelInput
    {
        public string Filter { get; set; }

        public string DataNameFilter { get; set; }

        public string DataValueFilter { get; set; }

        public int? LeveTypeFilter { get; set; }

        public string TypeNameFilter { get; set; }

        public string RemarkFilter { get; set; }

    }
}