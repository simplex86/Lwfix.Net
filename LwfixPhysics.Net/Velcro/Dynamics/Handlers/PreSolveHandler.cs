using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.ContactSystem;
using SimplexLab.LwfixPhysics.Velcro.Collision.Narrowphase;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Handlers
{
    public delegate void PreSolveHandler(Contact contact, ref Manifold oldManifold);
}
