using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Distance;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.TOI
{
    /// <summary>Input parameters for CalculateTimeOfImpact</summary>
    internal struct TOIInput
    {
        public DistanceProxy ProxyA;
        public DistanceProxy ProxyB;
        public Sweep SweepA;
        public Sweep SweepB;
        public Fixed32 TMax; // defines sweep interval [0, tMax]
    }
}
