using System;
using System.ComponentModel.DataAnnotations;

namespace RGB.Jw.JW.Dtos
{
    public class GetJwCustomerDesignTagForEditOutput
    {
        public CreateOrEditJwCustomerDesignTagDto JwCustomerDesignTag { get; set; }

        public string JwBaseDataDataName { get; set; }

        public string JwCustomerCompanyName { get; set; }

    }
}