using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Definitions.Joints
{
    /// <summary>Mouse joint definition. This requires a world target point, tuning parameters, and the time step.</summary>
    public sealed class FixedMouseJointDef : JointDef
    {
        public FixedMouseJointDef() : base(JointType.FixedMouse)
        {
            SetDefaults();
        }

        /// <summary>The linear damping in N*s/m</summary>
        public Fixed32 Damping { get; set; }

        /// <summary>The linear stiffness in N/m</summary>
        public Fixed32 Stiffness { get; set; }

        /// <summary>The maximum constraint force that can be exerted to move the candidate body. Usually you will express as some
        /// multiple of the weight (multiplier * mass * gravity).</summary>
        public Fixed32 MaxForce { get; set; }

        /// <summary>The initial world target point. This is assumed to coincide with the body anchor initially.</summary>
        public Vector2 Target { get; set; }

        public override void SetDefaults()
        {
            Target = Vector2.Zero;
            MaxForce = Fixed32.Zero;
            Stiffness = Fixed32.Zero;
            Damping = Fixed32.Zero;
        }
    }
}
