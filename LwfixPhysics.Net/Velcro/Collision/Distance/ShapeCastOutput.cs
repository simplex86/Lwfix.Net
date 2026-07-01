using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Distance
{
    /// <summary>Output results for b2ShapeCast</summary>
    internal struct ShapeCastOutput
    {
        public Vector2 Point;
        public Vector2 Normal;
        public Fixed32 Lambda;
        public int Iterations;
    }
}
