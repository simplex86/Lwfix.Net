using System;
using System.Collections.Generic;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Extensions.Controllers.ControllerBase;

namespace SimplexLab.LwfixPhysics.Velcro.Extensions.Controllers.Velocity
{
    /// <summary>
    /// Put a limit on the linear (translation - the move speed) and angular (rotation) velocity of bodies added to
    /// this controller.
    /// </summary>
    public class VelocityLimitController : Controller
    {
        private List<Body> _bodies = new List<Body>();
        private Fixed32 _maxAngularSqared;
        private Fixed32 _maxAngularVelocity;
        private Fixed32 _maxLinearSqared;
        private Fixed32 _maxLinearVelocity;
        public bool LimitAngularVelocity = true;
        public bool LimitLinearVelocity = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="VelocityLimitController" /> class. Sets the max linear velocity
        /// to Settings.MaxTranslation Sets the max angular velocity to Settings.MaxRotation
        /// </summary>
        public VelocityLimitController()
            : base(ControllerType.VelocityLimitController)
        {
            MaxLinearVelocity = Settings.MaxTranslation;
            MaxAngularVelocity = Settings.MaxRotation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VelocityLimitController" /> class. Pass in 0 or float.MaxValue to
        /// disable the limit. maxAngularVelocity = 0 will disable the angular velocity limit.
        /// </summary>
        /// <param name="maxLinearVelocity">The max linear velocity.</param>
        /// <param name="maxAngularVelocity">The max angular velocity.</param>
        public VelocityLimitController(Fixed32 maxLinearVelocity, Fixed32 maxAngularVelocity)
            : base(ControllerType.VelocityLimitController)
        {
            if (maxLinearVelocity == 0 || maxLinearVelocity == Fixed32.MaxValue)
                LimitLinearVelocity = false;

            if (maxAngularVelocity == 0 || maxAngularVelocity == Fixed32.MaxValue)
                LimitAngularVelocity = false;

            MaxLinearVelocity = maxLinearVelocity;
            MaxAngularVelocity = maxAngularVelocity;
        }

        /// <summary>Gets or sets the max angular velocity.</summary>
        /// <value>The max angular velocity.</value>
        public Fixed32 MaxAngularVelocity
        {
            get => _maxAngularVelocity;
            set
            {
                _maxAngularVelocity = value;
                _maxAngularSqared = _maxAngularVelocity * _maxAngularVelocity;
            }
        }

        /// <summary>Gets or sets the max linear velocity.</summary>
        /// <value>The max linear velocity.</value>
        public Fixed32 MaxLinearVelocity
        {
            get => _maxLinearVelocity;
            set
            {
                _maxLinearVelocity = value;
                _maxLinearSqared = _maxLinearVelocity * _maxLinearVelocity;
            }
        }

        public override void Update(Fixed32 dt)
        {
            foreach (Body body in _bodies)
            {
                if (!IsActiveOn(body))
                    continue;

                if (LimitLinearVelocity)
                {
                    //Translation
                    // Check for large velocities.
                    Fixed32 translationX = dt * body._linearVelocity.X;
                    Fixed32 translationY = dt * body._linearVelocity.Y;
                    Fixed32 result = translationX * translationX + translationY * translationY;

                    if (result > dt * _maxLinearSqared)
                    {
                        Fixed32 sq = Fixed32.Sqrt(result);

                        Fixed32 ratio = _maxLinearVelocity / sq;
                        body._linearVelocity.X *= ratio;
                        body._linearVelocity.Y *= ratio;
                    }
                }

                if (LimitAngularVelocity)
                {
                    //Rotation
                    Fixed32 rotation = dt * body._angularVelocity;
                    if (rotation * rotation > _maxAngularSqared)
                    {
                        Fixed32 ratio = _maxAngularVelocity / FMath.Abs(rotation);
                        body._angularVelocity *= ratio;
                    }
                }
            }
        }

        public void AddBody(Body body)
        {
            _bodies.Add(body);
        }

        public void RemoveBody(Body body)
        {
            _bodies.Remove(body);
        }
    }
}
