using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;

namespace SimplexLab.LwfixPhysics.Velcro.Extensions.PhysicsLogics.PhysicsLogicBase
{
    public struct ShapeData
    {
        public Body Body;
        public Fixed32 Max;
        public Fixed32 Min; // absolute angles
    }
}
