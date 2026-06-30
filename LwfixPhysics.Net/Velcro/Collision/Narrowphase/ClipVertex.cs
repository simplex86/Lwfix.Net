using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.ContactSystem;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Narrowphase
{
    /// <summary>Used for computing contact manifolds.</summary>
    internal struct ClipVertex
    {
        public ContactId Id;
        public Vector2 V;
    }
}
