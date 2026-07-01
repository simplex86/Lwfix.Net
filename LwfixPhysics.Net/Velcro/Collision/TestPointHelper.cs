using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;

namespace SimplexLab.LwfixPhysics.Velcro.Collision
{
    internal static class TestPointHelper
    {
        public static bool TestPointCircle(ref Vector2 pos, Fixed32 radius, ref Vector2 point, ref Transform transform)
        {
            Vector2 center = transform.p + MathUtils.Mul(transform.q, pos);
            Vector2 d = point - center;
            return Vector2.Dot(d, d) <= radius * radius;
        }

        public static bool TestPointPolygon(Vertices vertices, Vertices normals, ref Vector2 point, ref Transform transform)
        {
            Vector2 pLocal = MathUtils.MulT(transform.q, point - transform.p);

            for (int i = 0; i < vertices.Count; ++i)
            {
                Fixed32 dot = Vector2.Dot(normals[i], pLocal - vertices[i]);
                if (dot > (Fixed32)0.0)
                    return false;
            }

            return true;
        }
    }
}
