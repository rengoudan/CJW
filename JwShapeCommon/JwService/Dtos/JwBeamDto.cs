using RGB.Jw.JW;

using System;

namespace RGB.Jw.JW.Dtos
{
    public class JwBeamDto 
    {
        public long Id { get; set; }
        public ShapeType ShapeType { get; set; }

        public decimal Length { get; set; }

        public decimal Width { get; set; }

        public string BeamCode { get; set; }

    }
}