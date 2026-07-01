using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Solver
{
    internal sealed class ContactVelocityConstraint
    {
        public int ContactIndex;
        public Fixed32 Friction;
        public int IndexA;
        public int IndexB;
        public Fixed32 InvIA, InvIB;
        public Fixed32 InvMassA, InvMassB;
        public Mat22 K;
        public Vector2 Normal;
        public Mat22 NormalMass;
        public int PointCount;
        public VelocityConstraintPoint[] Points = new VelocityConstraintPoint[Settings.MaxManifoldPoints];
        public Fixed32 Restitution;
        public Fixed32 Threshold;
        public Fixed32 TangentSpeed;

        public ContactVelocityConstraint()
        {
            for (int i = 0; i < Settings.MaxManifoldPoints; i++)
            {
                Points[i] = new VelocityConstraintPoint();
            }
        }
    }
}
