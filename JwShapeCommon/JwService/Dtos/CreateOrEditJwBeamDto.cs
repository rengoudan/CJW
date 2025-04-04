using RGB.Jw.JW;

using System;
using System.ComponentModel.DataAnnotations;

namespace RGB.Jw.JW.Dtos
{
    public class CreateOrEditJwBeamDto
    {
        public long? Id { get; set; }
        public ShapeType ShapeType { get; set; }

        public decimal Length { get; set; }

        public decimal Width { get; set; }

        public string BeamCode { get; set; }

    }
}