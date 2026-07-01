using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Shared;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Distance
{
    /// <summary>Input for Distance.ComputeDistance(). You have to option to use the shape radii in the computation.</summary>
    internal struct DistanceInput
    {
        public DistanceProxy ProxyA;
        public DistanceProxy ProxyB;
        public Transform TransformA;
        public Transform TransformB;
        public bool UseRadii;
    }
}
