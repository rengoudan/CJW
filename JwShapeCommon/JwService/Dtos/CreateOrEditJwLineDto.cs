using RGB.Jw.JW;

using System;

using System.ComponentModel.DataAnnotations;

namespace RGB.Jw.JW.Dtos
{
    public class CreateOrEditJwLineDto 
    {
        public long? Id { get; set; } 

        [Required]
        public string Origin { get; set; }

        [Required]
        public string EndPoint { get; set; }

        public decimal LineLength { get; set; }

        public LineType LineType { get; set; }

        public long? JwBeamId { get; set; }

    }
}