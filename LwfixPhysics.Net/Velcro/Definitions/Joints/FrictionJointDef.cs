using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Definitions.Joints
{
    public sealed class FrictionJointDef : JointDef
    {
        public FrictionJointDef() : base(JointType.Friction)
        {
            SetDefaults();
        }

        /// <summary>The local anchor point relative to bodyA's origin.</summary>
        public Vector2 LocalAnchorA { get; set; }

        /// <summary>The local anchor point relative to bodyB's origin.</summary>
        public Vector2 LocalAnchorB { get; set; }

        /// <summary>The maximum friction force in N.</summary>
        public Fixed32 MaxForce { get; set; }

        /// <summary>The maximum friction torque in N-m.</summary>
        public Fixed32 MaxTorque { get; set; }

        public override void SetDefaults()
        {
            LocalAnchorA = Vector2.Zero;
            LocalAnchorB = Vector2.Zero;
            MaxForce = Fixed32.Zero;
            MaxTorque = Fixed32.Zero;

            base.SetDefaults();
        }
    }
}
