using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.ContactSystem;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Handlers
{
    public delegate void OnSeparationHandler(Fixture fixtureA, Fixture fixtureB, Contact contact);
}
