using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Definitions.Joints
{
    public sealed class MotorJointDef : JointDef
    {
        public MotorJointDef() : base(JointType.Motor)
        {
            SetDefaults();
        }

        /// <summary>The bodyB angle minus bodyA angle in radians.</summary>
        public Fixed32 AngularOffset { get; set; }

        /// <summary>Position correction factor in the range [0,1].</summary>
        public Fixed32 CorrectionFactor { get; set; }

        /// <summary>Position of bodyB minus the position of bodyA, in bodyA's frame, in meters.</summary>
        public Vector2 LinearOffset { get; set; }

        /// <summary>The maximum motor force in N.</summary>
        public Fixed32 MaxForce { get; set; }

        /// <summary>The maximum motor torque in N-m.</summary>
        public Fixed32 MaxTorque { get; set; }

        public void Initialize(Body bA, Body bB)
        {
            BodyA = bA;
            BodyB = bB;
            Vector2 xB = BodyB.Position;
            LinearOffset = BodyA.GetLocalPoint(xB);

            Fixed32 angleA = BodyA.Rotation;
            Fixed32 angleB = BodyB.Rotation;
            AngularOffset = angleB - angleA;
        }

        public override void SetDefaults()
        {
            LinearOffset = Vector2.Zero;
            AngularOffset = Fixed32.Zero;
            MaxForce = Fixed32.One;
            MaxTorque = Fixed32.One;
            CorrectionFactor = (Fixed32)0.3;
        }
    }
}
