using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.ContactSystem;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Solver;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Handlers
{
    internal delegate void PostSolveHandler(Contact contact, ContactVelocityConstraint impulse);
}
