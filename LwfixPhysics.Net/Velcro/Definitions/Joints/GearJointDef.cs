using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;

namespace SimplexLab.LwfixPhysics.Velcro.Definitions.Joints
{
    public sealed class GearJointDef : JointDef
    {
        public GearJointDef() : base(JointType.Gear)
        {
            SetDefaults();
        }

        /// <summary>The first revolute/prismatic joint attached to the gear joint.</summary>
        public Joint JointA { get; set; }

        /// <summary>The second revolute/prismatic joint attached to the gear joint.</summary>
        public Joint JointB { get; set; }

        /// <summary>The gear ratio.</summary>
        public Fixed32 Ratio { get; set; }

        public override void SetDefaults()
        {
            JointA = null;
            JointB = null;
            Ratio = Fixed32.One;
        }
    }
}
