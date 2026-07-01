using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Shared.Optimization;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Narrowphase
{
    /// <summary>Used to warm start ComputeDistance. Set count to zero on first call.</summary>
    internal struct SimplexCache
    {
        /// <summary>Length or area</summary>
        public ushort Count;

        /// <summary>Vertices on shape A</summary>
        internal FixedArray3<byte> IndexA;

        /// <summary>Vertices on shape B</summary>
        internal FixedArray3<byte> IndexB;

        public Fixed32 Metric;
    }
}
