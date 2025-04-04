using RGB.Jw.JW;

using System;

namespace RGB.Jw.JW.Dtos
{
    public class JwLineDto 
    {
        public long Id { get; set; }
        public string Origin { get; set; }

        public string EndPoint { get; set; }

        public decimal LineLength { get; set; }

        public LineType LineType { get; set; }

        public long? JwBeamId { get; set; }

        

    }
}