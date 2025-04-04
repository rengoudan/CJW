using System;
using System.ComponentModel.DataAnnotations;

namespace RGB.Jw.JW.Dtos
{
    public class GetJwProjectSubForEditOutput
    {
        public CreateOrEditJwProjectSubDto JwProjectSub { get; set; }

        public string JwProjectProjectName { get; set; }

    }
}