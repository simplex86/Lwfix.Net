using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Utilities;

namespace SimplexLab.LwfixPhysics.Velcro.Definitions.Joints
{
    /// <summary>Distance joint definition. This requires defining an anchor point on both bodies and the non-zero length of
    /// the distance joint. The definition uses local anchor points so that the initial configuration can violate the
    /// constraint slightly. This helps when saving and loading a game.
    /// <remarks>Do not use a zero or a short length.</remarks>
    /// </summary>
    public sealed class DistanceJointDef : JointDef
    {
        public DistanceJointDef() : base(JointType.Distance)
        {
            SetDefaults();
        }

        /// <summary>The linear damping in N*s/m.</summary>
        public Fixed32 Damping { get; set; }

        /// <summary>The linear stiffness in N/m.</summary>
        public Fixed32 Stiffness { get; set; }

        /// <summary>The rest length of this joint. Clamped to a stable minimum value.</summary>
        public Fixed32 Length { get; set; }

        /// <summary>Minimum length. Clamped to a stable minimum value.</summary>
        public Fixed32 MinLength { get; set; }

        /// <summary>Maximum length. Must be greater than or equal to the minimum length.</summary>
        public Fixed32 MaxLength { get; set; }

        /// <summary>The local anchor point relative to bodyA's origin.</summary>
        public Vector2 LocalAnchorA { get; set; }

        /// <summary>The local anchor point relative to bodyB's origin.</summary>
        public Vector2 LocalAnchorB { get; set; }

        public void Initialize(Body b1, Body b2, Vector2 anchor1, Vector2 anchor2)
        {
            BodyA = b1;
            BodyB = b2;
            LocalAnchorA = BodyA.GetLocalPoint(anchor1);
            LocalAnchorB = BodyB.GetLocalPoint(anchor2);
            Vector2 d = anchor2 - anchor1;
            Length = MathUtils.Max(d.Length(), Settings.LinearSlop);
            MinLength = Length;
            MaxLength = Length;
        }

        public override void SetDefaults()
        {
            LocalAnchorA = Vector2.Zero;
            LocalAnchorB = Vector2.Zero;
            Length = Fixed32.One;
            MinLength = Fixed32.Zero;
            MaxLength = Fixed32.MaxValue;
            Stiffness = Fixed32.Zero;
            Damping = Fixed32.Zero;
        }
    }
}
