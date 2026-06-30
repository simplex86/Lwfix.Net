using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.ContactSystem;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Solver;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Handlers
{
    public delegate void AfterCollisionHandler(Fixture fixtureA, Fixture fixtureB, Contact contact, ContactVelocityConstraint impulse);
}
