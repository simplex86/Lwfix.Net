using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Shared.Optimization;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Narrowphase
{
    /// <summary>Used to warm start ComputeDistance. Set count to zero on first call.</summary>
    public struct SimplexCache
    {
        /// <summary>Length or area</summary>
        public ushort Count;

        /// <summary>Vertices on shape A</summary>
        public FixedArray3<byte> IndexA;

        /// <summary>Vertices on shape B</summary>
        public FixedArray3<byte> IndexB;

        public Fixed32 Metric;
    }
}
