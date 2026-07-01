using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro;
using SimplexLab.LwfixPhysics.Velcro.Collision.Narrowphase;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Solver
{
    internal sealed class ContactPositionConstraint
    {
        public int IndexA;
        public int IndexB;
        public Fixed32 InvIA, InvIB;
        public Fixed32 InvMassA, InvMassB;
        public Vector2 LocalCenterA, LocalCenterB;
        public Vector2 LocalNormal;
        public Vector2 LocalPoint;
        public Vector2[] LocalPoints = new Vector2[Settings.MaxManifoldPoints];
        public int PointCount;
        public Fixed32 RadiusA, RadiusB;
        public ManifoldType Type;
    }
}
