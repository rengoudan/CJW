using System;
using System.ComponentModel.DataAnnotations;

namespace RGB.Jw.JW.Dtos
{
    public class GetJwLineForEditOutput
    {
        public CreateOrEditJwLineDto JwLine { get; set; }

        public string JwBeamBeamCode { get; set; }

    }
}