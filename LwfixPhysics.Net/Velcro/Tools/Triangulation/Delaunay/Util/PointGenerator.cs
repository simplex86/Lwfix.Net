using System;
using System.Collections.Generic;
using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Velcro.Tools.Triangulation.Delaunay.Util
{
    internal class PointGenerator
    {
        private static readonly Random RNG = new Random();

        public static List<TriangulationPoint> UniformDistribution(int n, Fixed32 scale)
        {
            List<TriangulationPoint> points = new List<TriangulationPoint>();
            for (int i = 0; i < n; i++)
            {
                points.Add(new TriangulationPoint(scale * (Fixed32)(0.5 - RNG.NextDouble()), scale * (Fixed32)(0.5 - RNG.NextDouble())));
            }
            return points;
        }

        public static List<TriangulationPoint> UniformGrid(int n, Fixed32 scale)
        {
            Fixed32 x = 0;
            Fixed32 size = scale / n;
            Fixed32 halfScale = Fixed32.Half * scale;

            List<TriangulationPoint> points = new List<TriangulationPoint>();
            for (int i = 0; i < n + 1; i++)
            {
                x = halfScale - i * size;
                for (int j = 0; j < n + 1; j++)
                {
                    points.Add(new TriangulationPoint(x, halfScale - j * size));
                }
            }
            return points;
        }
    }
}
