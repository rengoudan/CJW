using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    internal class GeneralPillar
    {
    }

    public class PillarFeature
    {
        public Coordinate Center1 { get; set; }
        public Coordinate Center2 { get; set; }
        public double SquareSideLength { get; set; }

        public double Distance => Center1.Distance(Center2);

        public double AngleRad { get; private set; }
        public double AngleDeg => AngleRad * 180.0 / Math.PI;

        public OrientationType Orientation { get; private set; }

        public void ComputeAngleAndOrientation()
        {
            double dx = Center2.X - Center1.X;
            double dy = Center2.Y - Center1.Y;

            AngleRad = Math.Atan2(dy, dx);

            double deg = Math.Abs(AngleDeg % 180);

            if (deg < 10 || deg > 170)
                Orientation = OrientationType.Horizontal;
            else if (Math.Abs(deg - 90) < 10)
                Orientation = OrientationType.Vertical;
            else
                Orientation = OrientationType.Diagonal;
        }

        public override string ToString()
        {
            return $"PillarFeature: C1=({Center1.X:F1},{Center1.Y:F1}), " +
                   $"C2=({Center2.X:F1},{Center2.Y:F1}), " +
                   $"Distance={Distance:F1}, Angle={AngleDeg:F1}°, Orientation={Orientation}";
        }
    }

    public enum OrientationType
    {
        Horizontal,
        Vertical,
        Diagonal
    }
}
