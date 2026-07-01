using System;
using System.Diagnostics;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.RayCast;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;

namespace SimplexLab.LwfixPhysics.Velcro.Collision
{
    internal static class RayCastHelper
    {
        public static bool RayCastEdge(ref Vector2 start, ref Vector2 end, bool oneSided, ref RayCastInput input, ref Transform transform, out RayCastOutput output)
        {
            // p = p1 + t * d
            // v = v1 + s * e
            // p1 + t * d = v1 + s * e
            // s * e - t * d = p1 - v1

            output = new RayCastOutput();

            // Put the ray into the edge's frame of reference.
            Vector2 p1 = MathUtils.MulT(transform.q, input.Point1 - transform.p);
            Vector2 p2 = MathUtils.MulT(transform.q, input.Point2 - transform.p);
            Vector2 d = p2 - p1;

            Vector2 v1 = start;
            Vector2 v2 = end;
            Vector2 e = v2 - v1;

            // Normal points to the right, looking from v1 at v2
            Vector2 normal = new Vector2(e.Y, -e.X); //Velcro TODO: Could possibly cache the normal.
            normal.Normalize();

            // q = p1 + t * d
            // dot(normal, q - v1) = 0
            // dot(normal, p1 - v1) + t * dot(normal, d) = 0
            Fixed32 numerator = Vector2.Dot(normal, v1 - p1);
            if (oneSided && numerator > (Fixed32)0.0)
            {
                return false;
            }

            Fixed32 denominator = Vector2.Dot(normal, d);

            if (denominator == (Fixed32)0.0)
                return false;

            Fixed32 t = numerator / denominator;
            if (t < (Fixed32)0.0 || input.MaxFraction < t)
                return false;

            Vector2 q = p1 + t * d;

            // q = v1 + s * r
            // s = dot(q - v1, r) / dot(r, r)
            Vector2 r = v2 - v1;
            Fixed32 rr = Vector2.Dot(r, r);
            if (rr == (Fixed32)0.0)
                return false;

            Fixed32 s = Vector2.Dot(q - v1, r) / rr;
            if (s < (Fixed32)0.0 || (Fixed32)1.0 < s)
                return false;

            output.Fraction = t;
            if (numerator > (Fixed32)0.0)
                output.Normal = -MathUtils.MulT(transform.q, normal);
            else
                output.Normal = MathUtils.MulT(transform.q, normal);
            return true;
        }

        public static bool RayCastCircle(ref Vector2 pos, Fixed32 radius, ref RayCastInput input, ref Transform transform, out RayCastOutput output)
        {
            // Collision Detection in Interactive 3D Environments by Gino van den Bergen
            // From Section 3.1.2
            // x = s + a * r
            // norm(x) = radius

            output = new RayCastOutput();

            Vector2 position = transform.p + MathUtils.Mul(transform.q, pos);
            Vector2 s = input.Point1 - position;
            Fixed32 b = Vector2.Dot(s, s) - radius * radius;

            // Solve quadratic equation.
            Vector2 r = input.Point2 - input.Point1;
            Fixed32 c = Vector2.Dot(s, r);
            Fixed32 rr = Vector2.Dot(r, r);
            Fixed32 sigma = c * c - rr * b;

            // Check for negative discriminant and short segment.
            if (sigma < (Fixed32)0.0 || rr < MathConstants.Epsilon)
                return false;

            // Find the point of intersection of the line with the circle.
            Fixed32 a = -(c + Fixed32.Sqrt(sigma));

            // Is the intersection point on the segment?
            if ((Fixed32)0.0 <= a && a <= input.MaxFraction * rr)
            {
                a /= rr;
                output.Fraction = a;
                output.Normal = s + a * r;
                output.Normal.Normalize();
                return true;
            }

            return false;
        }

        public static bool RayCastPolygon(Vertices vertices, Vertices normals, ref RayCastInput input, ref Transform transform, out RayCastOutput output)
        {
            output = new RayCastOutput();

            // Put the ray into the polygon's frame of reference.
            Vector2 p1 = MathUtils.MulT(transform.q, input.Point1 - transform.p);
            Vector2 p2 = MathUtils.MulT(transform.q, input.Point2 - transform.p);
            Vector2 d = p2 - p1;

            Fixed32 lower = (Fixed32)0.0, upper = input.MaxFraction;

            int index = -1;

            for (int i = 0; i < vertices.Count; ++i)
            {
                // p = p1 + a * d
                // dot(normal, p - v) = 0
                // dot(normal, p1 - v) + a * dot(normal, d) = 0
                Fixed32 numerator = Vector2.Dot(normals[i], vertices[i] - p1);
                Fixed32 denominator = Vector2.Dot(normals[i], d);

                if (denominator == (Fixed32)0.0)
                {
                    if (numerator < (Fixed32)0.0)
                        return false;
                }
                else
                {
                    // Note: we want this predicate without division:
                    // lower < numerator / denominator, where denominator < 0
                    // Since denominator < 0, we have to flip the inequality:
                    // lower < numerator / denominator <==> denominator * lower > numerator.
                    if (denominator < (Fixed32)0.0 && numerator < lower * denominator)
                    {
                        // Increase lower.
                        // The segment enters this half-space.
                        lower = numerator / denominator;
                        index = i;
                    }
                    else if (denominator > (Fixed32)0.0 && numerator < upper * denominator)
                    {
                        // Decrease upper.
                        // The segment exits this half-space.
                        upper = numerator / denominator;
                    }
                }

                // The use of epsilon here causes the assert on lower to trip
                // in some cases. Apparently the use of epsilon was to make edge
                // shapes work, but now those are handled separately.
                //if (upper < lower - b2_epsilon)
                if (upper < lower)
                    return false;
            }

            Debug.Assert((Fixed32)0.0 <= lower && lower <= input.MaxFraction);

            if (index >= 0)
            {
                output.Fraction = lower;
                output.Normal = MathUtils.Mul(transform.q, normals[index]);
                return true;
            }

            return false;
        }
    }
}
