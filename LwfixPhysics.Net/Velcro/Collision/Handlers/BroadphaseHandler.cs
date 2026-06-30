using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Handlers
{
    public delegate void BroadphaseHandler(ref FixtureProxy proxyA, ref FixtureProxy proxyB);
}
