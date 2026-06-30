using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Definitions.Joints
{
    /// <summary>Revolute joint definition. This requires defining an anchor point where the bodies are joined. The definition
    /// uses local anchor points so that the initial configuration can violate the constraint slightly. You also need to
    /// specify the initial relative angle for joint limits. This helps when saving and loading a game. The local anchor points
    /// are measured from the body's origin rather than the center of mass because: 1. you might not know where the center of
    /// mass will be. 2. if you add/remove shapes from a body and recompute the mass, the joints will be broken.</summary>
    public sealed class RevoluteJointDef : JointDef
    {
        public RevoluteJointDef() : base(JointType.Revolute)
        {
            SetDefaults();
        }

        /// <summary>A flag to enable joint limits.</summary>
        public bool EnableLimit { get; set; }

        /// <summary>A flag to enable the joint motor.</summary>
        public bool EnableMotor { get; set; }

        /// <summary>The local anchor point relative to bodyA's origin.</summary>
        public Vector2 LocalAnchorA { get; set; }

        /// <summary>The local anchor point relative to bodyB's origin.</summary>
        public Vector2 LocalAnchorB { get; set; }

        /// <summary>The lower angle for the joint limit (radians).</summary>
        public Fixed32 LowerAngle { get; set; }

        /// <summary>The maximum motor torque used to achieve the desired motor speed. Usually in N-m.</summary>
        public Fixed32 MaxMotorTorque { get; set; }

        /// <summary>The desired motor speed. Usually in radians per second.</summary>
        public Fixed32 MotorSpeed { get; set; }

        /// <summary>The bodyB angle minus bodyA angle in the reference state (radians).</summary>
        public Fixed32 ReferenceAngle { get; set; }

        /// <summary>The upper angle for the joint limit (radians).</summary>
        public Fixed32 UpperAngle { get; set; }

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
            LowerAngle = Fixed32.Zero;
            UpperAngle = Fixed32.Zero;
            MaxMotorTorque = Fixed32.Zero;
            MotorSpeed = Fixed32.Zero;
            EnableLimit = false;
            EnableMotor = false;

            base.SetDefaults();
        }
    }
}
