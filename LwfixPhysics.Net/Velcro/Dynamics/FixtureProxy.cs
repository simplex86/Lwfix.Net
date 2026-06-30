using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Broadphase;
using SimplexLab.LwfixPhysics.Velcro.Shared;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics
{
    /// <summary>This proxy is used internally to connect fixtures to the broad-phase.</summary>
    public struct FixtureProxy
    {
        public AABB AABB;
        public int ChildIndex;
        public Fixture Fixture;
        public int ProxyId;
    }
}
