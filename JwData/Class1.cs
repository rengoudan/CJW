using NetTopologySuite.Geometries;

namespace JwData
{
    public class JwLine
    {
        public long Id { get; set; }
        public string Origin { get; set; }

        public string EndPoint { get; set; }

        public decimal LineLength { get; set; }


        public long? JwBeamId { get; set; }
        public Point Location { get; set; }
    }
}
