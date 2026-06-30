using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Solver
{
    public sealed class VelocityConstraintPoint
    {
        public Fixed32 NormalImpulse;
        public Fixed32 NormalMass;
        public Vector2 rA;
        public Vector2 rB;
        public Fixed32 TangentImpulse;
        public Fixed32 TangentMass;
        public Fixed32 VelocityBias;
    }
}
