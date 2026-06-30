using System.Diagnostics;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Utilities;

namespace SimplexLab.LwfixPhysics.Velcro.Definitions.Joints
{
    /// <summary>Pulley joint definition. This requires two ground anchors, two dynamic body anchor points, and a pulley ratio.</summary>
    public sealed class PulleyJointDef : JointDef
    {
        public PulleyJointDef() : base(JointType.Pulley)
        {
            SetDefaults();
        }

        /// <summary>The first ground anchor in world coordinates. This point never moves.</summary>
        public Vector2 GroundAnchorA { get; set; }

        /// <summary>The second ground anchor in world coordinates. This point never moves.</summary>
        public Vector2 GroundAnchorB { get; set; }

        /// <summary>The a reference length for the segment attached to bodyA.</summary>
        public Fixed32 LengthA { get; set; }

        /// <summary>The a reference length for the segment attached to bodyB.</summary>
        public Fixed32 LengthB { get; set; }

        /// <summary>The local anchor point relative to bodyA's origin.</summary>
        public Vector2 LocalAnchorA { get; set; }

        /// <summary>The local anchor point relative to bodyB's origin.</summary>
        public Vector2 LocalAnchorB { get; set; }

        /// <summary>The pulley ratio, used to simulate a block-and-tackle.</summary>
        public Fixed32 Ratio { get; set; }

        public void Initialize(Body bA, Body bB, Vector2 groundA, Vector2 groundB, Vector2 anchorA, Vector2 anchorB, Fixed32 r)
        {
            BodyA = bA;
            BodyB = bB;
            GroundAnchorA = groundA;
            GroundAnchorB = groundB;
            LocalAnchorA = BodyA.GetLocalPoint(anchorA);
            LocalAnchorB = BodyB.GetLocalPoint(anchorB);
            Vector2 dA = anchorA - groundA;
            LengthA = dA.Length();
            Vector2 dB = anchorB - groundB;
            LengthB = dB.Length();
            Ratio = r;
            Debug.Assert(Ratio > MathConstants.Epsilon);
        }

        public override void SetDefaults()
        {
            GroundAnchorA = new Vector2(Fixed32.NegativeOne, Fixed32.One);
            GroundAnchorB = new Vector2(Fixed32.One, Fixed32.One);
            LocalAnchorA = new Vector2(Fixed32.NegativeOne, Fixed32.Zero);
            LocalAnchorB = new Vector2(Fixed32.One, Fixed32.Zero);
            LengthA = Fixed32.Zero;
            LengthB = Fixed32.Zero;
            Ratio = Fixed32.One;
            CollideConnected = true;
        }
    }
}
