using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Distance
{
    /// <summary>Output for Distance.ComputeDistance().</summary>
    public struct DistanceOutput
    {
        public Fixed32 Distance;

        /// <summary>Number of GJK iterations used</summary>
        public int Iterations;

        /// <summary>Closest point on shapeA</summary>
        public Vector2 PointA;

        /// <summary>Closest point on shapeB</summary>
        public Vector2 PointB;
    }
}
