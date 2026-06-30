using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Definitions.Joints
{
    /// <summary>Weld joint definition. You need to specify local anchor points where they are attached and the relative body
    /// angle. The position of the anchor points is important for computing the reaction torque.</summary>
    public sealed class WeldJointDef : JointDef
    {
        public WeldJointDef() : base(JointType.Weld)
        {
            SetDefaults();
        }

        /// <summary>The rotational damping in N*m*s</summary>
        public Fixed32 Damping { get; set; }

        /// <summary>The rotational stiffness in N*m. Disable softness with a value of 0</summary>
        public Fixed32 Stiffness { get; set; }

        /// <summary>The local anchor point relative to bodyA's origin.</summary>
        public Vector2 LocalAnchorA { get; set; }

        /// <summary>The local anchor point relative to bodyB's origin.</summary>
        public Vector2 LocalAnchorB { get; set; }

        /// <summary>The bodyB angle minus bodyA angle in the reference state (radians).</summary>
        public Fixed32 ReferenceAngle { get; set; }

        public void Initialize(Body bA, Body bB, Vector2 anchor)
        {
            BodyA = bA;
            BodyB = bB;
            LocalAnchorA = BodyA.GetLocalPoint(anchor);
            LocalAnchorB = BodyB.GetLocalPoint(anchor);
            ReferenceAngle = BodyB.Rotation - BodyA.Rotation;
        }

        public override void SetDefaults()
        {
            LocalAnchorA = Vector2.Zero;
            LocalAnchorB = Vector2.Zero;
            ReferenceAngle = Fixed32.Zero;
            Stiffness = Fixed32.Zero;
            Damping = Fixed32.Zero;

            base.SetDefaults();
        }
    }
}
