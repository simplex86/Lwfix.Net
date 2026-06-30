using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;

namespace SimplexLab.LwfixPhysics.Velcro.Tools.ConvexHull.GiftWrap
{
    /// <summary>
    /// Giftwrap convex hull algorithm. O(n * h) time complexity, where n is the number of points and h is the number
    /// of points on the convex hull. See http://en.wikipedia.org/wiki/Gift_wrapping_algorithm for more details.
    /// </summary>
    public static class GiftWrap
    {
        //Extracted from Box2D

        /// <summary>Returns the convex hull from the given vertices.</summary>
        /// <param name="vertices">The vertices.</param>
        public static Vertices GetConvexHull(Vertices vertices)
        {
            if (vertices.Count <= 3)
                return vertices;

            // Find the right most point on the hull
            int i0 = 0;
            Fixed32 x0 = vertices[0].X;
            for (int i = 1; i < vertices.Count; ++i)
            {
                Fixed32 x = vertices[i].X;
                if (x > x0 || (x == x0 && vertices[i].Y < vertices[i0].Y))
                {
                    i0 = i;
                    x0 = x;
                }
            }

            int[] hull = new int[vertices.Count];
            int m = 0;
            int ih = i0;

            for (;;)
            {
                hull[m] = ih;

                int ie = 0;
                for (int j = 1; j < vertices.Count; ++j)
                {
                    if (ie == ih)
                    {
                        ie = j;
                        continue;
                    }

                    Vector2 r = vertices[ie] - vertices[hull[m]];
                    Vector2 v = vertices[j] - vertices[hull[m]];
                    Fixed32 c = MathUtils.Cross(ref r, ref v);
                    if (c < Fixed32.Zero)
                        ie = j;

                    // Collinearity check
                    if (c == Fixed32.Zero && v.LengthSquared() > r.LengthSquared())
                        ie = j;
                }

                ++m;
                ih = ie;

                if (ie == i0)
                    break;
            }

            Vertices result = new Vertices(m);

            // Copy vertices.
            for (int i = 0; i < m; ++i)
            {
                result.Add(vertices[hull[i]]);
            }
            return result;
        }
    }
}
