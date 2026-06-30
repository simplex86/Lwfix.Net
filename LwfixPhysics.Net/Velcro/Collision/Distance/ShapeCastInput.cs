using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Distance
{
    /// <summary>Input parameters for b2ShapeCast</summary>
    public struct ShapeCastInput
    {
        public DistanceProxy ProxyA;
        public DistanceProxy ProxyB;
        public Transform TransformA;
        public Transform TransformB;
        public Vector2 TranslationB;
    }
}
