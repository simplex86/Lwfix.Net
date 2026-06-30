using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Narrowphase
{
    /// <summary>Reference face used for clipping</summary>
    public struct ReferenceFace
    {
        public int i1, i2;
        public Vector2 v1, v2;
        public Vector2 Normal;

        public Vector2 SideNormal1;
        public Fixed32 SideOffset1;

        public Vector2 SideNormal2;
        public Fixed32 SideOffset2;
    }
}
