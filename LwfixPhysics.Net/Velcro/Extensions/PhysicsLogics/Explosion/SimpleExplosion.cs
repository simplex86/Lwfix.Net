using System;
using System.Collections.Generic;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Extensions.PhysicsLogics.PhysicsLogicBase;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Extensions.PhysicsLogics.Explosion
{
    /// <summary>Creates a simple explosion that ignores other bodies hiding behind static bodies.</summary>
    public sealed class SimpleExplosion : PhysicsLogic
    {
        public SimpleExplosion(World world)
            : base(world, PhysicsLogicType.Explosion)
        {
            Power = 1; //linear
        }

        /// <summary>
        /// This is the power used in the power function. A value of 1 means the force applied to bodies in the explosion
        /// is linear. A value of 2 means it is exponential.
        /// </summary>
        public Fixed32 Power { get; set; }

        /// <summary>Activate the explosion at the specified position.</summary>
        /// <param name="pos">The position (center) of the explosion.</param>
        /// <param name="radius">The radius of the explosion.</param>
        /// <param name="force">The force applied</param>
        /// <param name="maxForce">A maximum amount of force. When force gets over this value, it will be equal to maxForce</param>
        /// <returns>A list of bodies and the amount of force that was applied to them.</returns>
        public Dictionary<Body, Vector2> Activate(Vector2 pos, Fixed32 radius, Fixed32 force, Fixed32 maxForce = default)
        {
            if (maxForce == default)
                maxForce = Fixed32.MaxValue;

            HashSet<Body> affectedBodies = new HashSet<Body>();

            AABB aabb;
            aabb.LowerBound = pos - new Vector2(radius);
            aabb.UpperBound = pos + new Vector2(radius);

            // Query the world for bodies within the radius.
            World.QueryAABB(fixture =>
            {
                if (Vector2.Distance(fixture.Body.Position, pos) <= radius)
                {
                    if (!affectedBodies.Contains(fixture.Body))
                        affectedBodies.Add(fixture.Body);
                }

                return true;
            }, ref aabb);

            return ApplyImpulse(pos, radius, force, maxForce, affectedBodies);
        }

        private Dictionary<Body, Vector2> ApplyImpulse(Vector2 pos, Fixed32 radius, Fixed32 force, Fixed32 maxForce, HashSet<Body> overlappingBodies)
        {
            Dictionary<Body, Vector2> forces = new Dictionary<Body, Vector2>(overlappingBodies.Count);

            foreach (Body overlappingBody in overlappingBodies)
            {
                if (IsActiveOn(overlappingBody))
                {
                    Fixed32 distance = Vector2.Distance(pos, overlappingBody.Position);
                    Fixed32 forcePercent = GetPercent(distance, radius);

                    Vector2 forceVector = pos - overlappingBody.Position;
                    forceVector *= Fixed32.One / Fixed32.Sqrt(forceVector.X * forceVector.X + forceVector.Y * forceVector.Y);
                    forceVector *= MathHelper.Min(force * forcePercent, maxForce);
                    forceVector *= -1;

                    overlappingBody.ApplyLinearImpulse(forceVector);
                    forces.Add(overlappingBody, forceVector);
                }
            }

            return forces;
        }

        private Fixed32 GetPercent(Fixed32 distance, Fixed32 radius)
        {
            //(1-(distance/radius))^power-1
            Fixed32 percent = FMath.Pow(1 - ((distance - radius) / radius), Power) - 1;

            if (Fixed32.IsNaN(percent))
                return Fixed32.Zero;

            return MathHelper.Clamp(percent, Fixed32.Zero, Fixed32.One);
        }
    }
}
