using System;
using System.ComponentModel.DataAnnotations;

namespace RGB.Jw.JW.Dtos
{
    public class GetJwProjectForEditOutput
    {
        public CreateOrEditJwProjectDto JwProject { get; set; }

        public string JwCustomerCompanyName { get; set; }

    }
}