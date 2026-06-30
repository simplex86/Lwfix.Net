using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;

namespace SimplexLab.LwfixPhysics.Velcro.Utilities
{
    public static class JointHelper
    {
        public static void LinearStiffness(Fixed32 frequencyHertz, Fixed32 dampingRatio, Body bodyA, Body bodyB, out Fixed32 stiffness, out Fixed32 damping)
        {
            Fixed32 massA = bodyA.Mass;

            Fixed32 massB = 0;

            if (bodyB != null)
                massB = bodyB.Mass;

            Fixed32 mass;

            if (massA > Fixed32.Zero && massB > Fixed32.Zero)
                mass = massA * massB / (massA + massB);
            else if (massA > Fixed32.Zero)
                mass = massA;
            else
                mass = massB;

            Fixed32 omega = MathConstants.TwoPi * frequencyHertz;
            stiffness = mass * omega * omega;
            damping = (Fixed32)2.0 * mass * dampingRatio * omega;
        }

        public static void AngularStiffness(Fixed32 frequencyHertz, Fixed32 dampingRatio, Body bodyA, Body bodyB, out Fixed32 stiffness, out Fixed32 damping)
        {
            Fixed32 inertiaA = bodyA.Inertia;
            Fixed32 inertiaB = bodyB.Inertia;
            Fixed32 I;

            if (inertiaA > Fixed32.Zero && inertiaB > Fixed32.Zero)
                I = inertiaA * inertiaB / (inertiaA + inertiaB);
            else if (inertiaA > Fixed32.Zero)
                I = inertiaA;
            else
                I = inertiaB;

            Fixed32 omega = MathConstants.TwoPi * frequencyHertz;
            stiffness = I * omega * omega;
            damping = (Fixed32)2.0 * I * dampingRatio * omega;
        }
    }
}
