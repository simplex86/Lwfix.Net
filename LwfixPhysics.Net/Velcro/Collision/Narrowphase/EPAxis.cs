using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Narrowphase
{
    /// <summary>This structure is used to keep track of the best separating axis.</summary>
    public struct EPAxis
    {
        public Vector2 Normal;
        public int Index;
        public Fixed32 Separation;
        public EPAxisType Type;
    }
}
