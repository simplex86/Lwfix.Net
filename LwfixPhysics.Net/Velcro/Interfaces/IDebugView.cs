using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Shapes;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Interfaces
{
    public interface IDebugView
    {
        void DrawJoint(Joint joint);
        void DrawShape(Shape shape, ref Transform transform, Color color);
    }
}
