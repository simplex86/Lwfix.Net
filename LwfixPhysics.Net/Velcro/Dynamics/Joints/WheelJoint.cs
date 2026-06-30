/*
* Velcro Physics:
* Copyright (c) 2017 Ian Qvist
* 
* Original source Box2D:
* Copyright (c) 2006-2011 Erin Catto http://www.box2d.org 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using System.Diagnostics;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Definitions.Joints;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Solver;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints
{
    // Linear constraint (point-to-line)
    // d = pB - pA = xB + rB - xA - rA
    // C = dot(ay, d)
    // Cdot = dot(d, cross(wA, ay)) + dot(ay, vB + cross(wB, rB) - vA - cross(wA, rA))
    //      = -dot(ay, vA) - dot(cross(d + rA, ay), wA) + dot(ay, vB) + dot(cross(rB, ay), vB)
    // J = [-ay, -cross(d + rA, ay), ay, cross(rB, ay)]

    // Spring linear constraint
    // C = dot(ax, d)
    // Cdot = = -dot(ax, vA) - dot(cross(d + rA, ax), wA) + dot(ax, vB) + dot(cross(rB, ax), vB)
    // J = [-ax -cross(d+rA, ax) ax cross(rB, ax)]

    // Motor rotational constraint
    // Cdot = wB - wA
    // J = [0 0 -1 0 0 1]

    /// <summary>
    /// A wheel joint. This joint provides two degrees of freedom: translation along an axis fixed in bodyA and
    /// rotation in the plane. In other words, it is a point to line constraint with a rotational motor and a linear
    /// spring/damper. The spring/damper is initialized upon creation. This joint is designed for vehicle suspensions.
    /// </summary>
    public class WheelJoint : Joint
    {
        private Vector2 _localAnchorA;
        private Vector2 _localAnchorB;
        private Vector2 _localXAxisA;
        private Vector2 _localYAxisA;

        private Fixed32 _impulse;
        private Fixed32 _motorImpulse;
        private Fixed32 _springImpulse;

        private Fixed32 _lowerImpulse;
        private Fixed32 _upperImpulse;
        private Fixed32 _translation;
        private Fixed32 _lowerTranslation;
        private Fixed32 _upperTranslation;

        private Fixed32 _maxMotorTorque;
        private Fixed32 _motorSpeed;

        private bool _enableLimit;
        private bool _enableMotor;

        private Fixed32 _stiffness;
        private Fixed32 _damping;

        // Solver temp
        private int _indexA;
        private int _indexB;
        private Vector2 _localCenterA;
        private Vector2 _localCenterB;
        private Fixed32 _invMassA;
        private Fixed32 _invMassB;
        private Fixed32 _invIA;
        private Fixed32 _invIB;

        private Vector2 _ax, _ay;
        private Fixed32 _sAx, _sBx;
        private Fixed32 _sAy, _sBy;

        private Fixed32 _mass;
        private Fixed32 _motorMass;
        private Fixed32 _axialMass;
        private Fixed32 _springMass;

        private Fixed32 _bias;
        private Fixed32 _gamma;

        /// <summary>Constructor for WheelJoint</summary>
        /// <param name="bodyA">The first body</param>
        /// <param name="bodyB">The second body</param>
        /// <param name="anchor">The anchor point</param>
        /// <param name="axis">The axis</param>
        /// <param name="useWorldCoordinates">Set to true if you are using world coordinates as anchors.</param>
        public WheelJoint(Body bodyA, Body bodyB, Vector2 anchor, Vector2 axis, bool useWorldCoordinates = false)
            : base(bodyA, bodyB, JointType.Wheel)
        {
            if (useWorldCoordinates)
            {
                _localAnchorA = bodyA.GetLocalPoint(anchor);
                _localAnchorB = bodyB.GetLocalPoint(anchor);
                _localXAxisA = bodyA.GetLocalVector(axis);
            }
            else
            {
                _localAnchorA = bodyA.GetLocalPoint(bodyB.GetWorldPoint(anchor));
                _localAnchorB = anchor;
                _localXAxisA = bodyA.GetLocalVector(axis);
            }

            _localYAxisA = MathUtils.Cross((Fixed32)1.0, _localXAxisA);
        }

        public WheelJoint(WheelJointDef def) : base(def)
        {
            _localAnchorA = def.LocalAnchorA;
            _localAnchorB = def.LocalAnchorB;
            _localXAxisA = def.LocalAxisA;
            _localYAxisA = MathUtils.Cross((Fixed32)1.0, _localXAxisA);

            _lowerTranslation = def.LowerTranslation;
            _upperTranslation = def.UpperTranslation;
            _enableLimit = def.EnableLimit;

            _maxMotorTorque = def.MaxMotorTorque;
            _motorSpeed = def.MotorSpeed;
            _enableMotor = def.EnableMotor;

            _stiffness = def.Stiffness;
            _damping = def.Damping;
        }

        public Vector2 LocalXAxisA => _localXAxisA;
        public Vector2 LocalYAxisA => _localYAxisA;

        /// <summary>The local anchor point on BodyA</summary>
        public Vector2 LocalAnchorA
        {
            get => _localAnchorA;
            set => _localAnchorA = value;
        }

        /// <summary>The local anchor point on BodyB</summary>
        public Vector2 LocalAnchorB
        {
            get => _localAnchorB;
            set => _localAnchorB = value;
        }

        public override Vector2 WorldAnchorA
        {
            get => _bodyA.GetWorldPoint(_localAnchorA);
            set => _localAnchorA = _bodyA.GetLocalPoint(value);
        }

        public override Vector2 WorldAnchorB
        {
            get => _bodyB.GetWorldPoint(_localAnchorB);
            set => _localAnchorB = _bodyB.GetLocalPoint(value);
        }

        /// <summary>The axis in local coordinates relative to BodyA</summary>
        public Vector2 LocalXAxis { get; private set; }

        /// <summary>The desired motor speed in radians per second.</summary>
        public Fixed32 MotorSpeed
        {
            get => _motorSpeed;
            set
            {
                if (value != _motorSpeed)
                {
                    WakeBodies();
                    _motorSpeed = value;
                }
            }
        }

        /// <summary>The maximum motor torque, usually in N-m.</summary>
        public Fixed32 MaxMotorTorque
        {
            get => _maxMotorTorque;
            set
            {
                if (value != _maxMotorTorque)
                {
                    WakeBodies();
                    _maxMotorTorque = value;
                }
            }
        }

        /// <summary>Gets the translation along the axis</summary>
        public Fixed32 JointTranslation
        {
            get
            {
                Body bA = _bodyA;
                Body bB = _bodyB;

                Vector2 pA = bA.GetWorldPoint(_localAnchorA);
                Vector2 pB = bB.GetWorldPoint(_localAnchorB);
                Vector2 d = pB - pA;
                Vector2 axis = bA.GetWorldVector(_localXAxisA);

                Fixed32 translation = Vector2.Dot(d, axis);
                return translation;
            }
        }

        public Fixed32 JointLinearSpeed
        {
            get
            {
                Body bA = _bodyA;
                Body bB = _bodyB;

                Vector2 rA = MathUtils.Mul(bA._xf.q, _localAnchorA - bA._sweep.LocalCenter);
                Vector2 rB = MathUtils.Mul(bB._xf.q, _localAnchorB - bB._sweep.LocalCenter);
                Vector2 p1 = bA._sweep.C + rA;
                Vector2 p2 = bB._sweep.C + rB;
                Vector2 d = p2 - p1;
                Vector2 axis = MathUtils.Mul(bA._xf.q, _localXAxisA);

                Vector2 vA = bA._linearVelocity;
                Vector2 vB = bB._linearVelocity;
                Fixed32 wA = bA._angularVelocity;
                Fixed32 wB = bB._angularVelocity;

                Fixed32 speed = MathUtils.Dot(d, MathUtils.Cross(wA, axis)) + MathUtils.Dot(axis, vB + MathUtils.Cross(wB, rB) - vA - MathUtils.Cross(wA, rA));
                return speed;
            }
        }

        public Fixed32 JointAngle
        {
            get
            {
                Body bA = _bodyA;
                Body bB = _bodyB;
                return bB._sweep.A - bA._sweep.A;
            }
        }

        /// <summary>Gets the angular velocity of the joint</summary>
        public Fixed32 JointAngularSpeed
        {
            get
            {
                Fixed32 wA = _bodyA.AngularVelocity;
                Fixed32 wB = _bodyB.AngularVelocity;
                return wB - wA;
            }
        }

        /// <summary>Enable/disable the joint motor.</summary>
        public bool MotorEnabled
        {
            get => _enableMotor;
            set
            {
                if (value != _enableMotor)
                {
                    WakeBodies();
                    _enableMotor = value;
                }
            }
        }

        public Fixed32 UpperLimit
        {
            get => _upperTranslation;
            set
            {
                if (_upperTranslation != value)
                {
                    WakeBodies();
                    _upperTranslation = value;
                    _upperImpulse = Fixed32.Zero;
                }
            }
        }

        public Fixed32 LowerLimit
        {
            get => _lowerTranslation;
            set
            {
                if (_lowerTranslation != value)
                {
                    WakeBodies();
                    _lowerTranslation = value;
                    _lowerImpulse = Fixed32.Zero;
                }
            }
        }

        public bool EnableLimit
        {
            get => _enableLimit;
            set
            {
                if (_enableLimit != value)
                {
                    WakeBodies();
                    _enableLimit = value;
                    _lowerImpulse = Fixed32.Zero;
                    _upperImpulse = Fixed32.Zero;
                }
            }
        }

        public Fixed32 Damping
        {
            get => _damping;
            set => _damping = value;
        }

        public Fixed32 Stiffness
        {
            get => _stiffness;
            set => _stiffness = value;
        }

        public void SetLimits(Fixed32 lower, Fixed32 upper)
        {
            Debug.Assert(lower <= upper);
            if (lower != _lowerTranslation || upper != _upperTranslation)
            {
                WakeBodies();
                _lowerTranslation = lower;
                _upperTranslation = upper;
                _lowerImpulse = Fixed32.Zero;
                _upperImpulse = Fixed32.Zero;
            }
        }

        /// <summary>Gets the torque of the motor</summary>
        /// <param name="invDt">inverse delta time</param>
        public Fixed32 GetMotorTorque(Fixed32 invDt)
        {
            return invDt * _motorImpulse;
        }

        public override Vector2 GetReactionForce(Fixed32 invDt)
        {
            return invDt * (_impulse * _ay + (_springImpulse + _lowerImpulse - _upperImpulse) * _ax);
        }

        public override Fixed32 GetReactionTorque(Fixed32 invDt)
        {
            return invDt * _motorImpulse;
        }

        internal override void InitVelocityConstraints(ref SolverData data)
        {
            _indexA = _bodyA.IslandIndex;
            _indexB = _bodyB.IslandIndex;
            _localCenterA = _bodyA._sweep.LocalCenter;
            _localCenterB = _bodyB._sweep.LocalCenter;
            _invMassA = _bodyA._invMass;
            _invMassB = _bodyB._invMass;
            _invIA = _bodyA._invI;
            _invIB = _bodyB._invI;

            Fixed32 mA = _invMassA, mB = _invMassB;
            Fixed32 iA = _invIA, iB = _invIB;

            Vector2 cA = data.Positions[_indexA].C;
            Fixed32 aA = data.Positions[_indexA].A;
            Vector2 vA = data.Velocities[_indexA].V;
            Fixed32 wA = data.Velocities[_indexA].W;

            Vector2 cB = data.Positions[_indexB].C;
            Fixed32 aB = data.Positions[_indexB].A;
            Vector2 vB = data.Velocities[_indexB].V;
            Fixed32 wB = data.Velocities[_indexB].W;

            Rot qA = new Rot(aA), qB = new Rot(aB);

            // Compute the effective masses.
            Vector2 rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            Vector2 rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
            Vector2 d = cB + rB - cA - rA;

            // Point to line constraint
            {
                _ay = MathUtils.Mul(qA, _localYAxisA);
                _sAy = MathUtils.Cross(d + rA, _ay);
                _sBy = MathUtils.Cross(rB, _ay);

                _mass = mA + mB + iA * _sAy * _sAy + iB * _sBy * _sBy;

                if (_mass > Fixed32.Zero)
                {
                    _mass = (Fixed32)1.0 / _mass;
                }
            }

            // Spring constraint
            _ax = MathUtils.Mul(qA, _localXAxisA);
            _sAx = MathUtils.Cross(d + rA, _ax);
            _sBx = MathUtils.Cross(rB, _ax);

            Fixed32 invMass = mA + mB + iA * _sAx * _sAx + iB * _sBx * _sBx;
            if (invMass > Fixed32.Zero)
            {
                _axialMass = (Fixed32)1.0 / invMass;
            }
            else
            {
                _axialMass = Fixed32.Zero;
            }

            _springMass = Fixed32.Zero;
            _bias = Fixed32.Zero;
            _gamma = Fixed32.Zero;

            if (_stiffness > Fixed32.Zero && invMass > Fixed32.Zero)
            {
                _springMass = (Fixed32)1.0 / invMass;

                Fixed32 C = MathUtils.Dot(d, _ax);

                // magic formulas
                Fixed32 h = data.Step.DeltaTime;
                _gamma = h * (_damping + h * _stiffness);
                if (_gamma > Fixed32.Zero)
                {
                    _gamma = (Fixed32)1.0 / _gamma;
                }

                _bias = C * h * _stiffness * _gamma;

                _springMass = invMass + _gamma;
                if (_springMass > Fixed32.Zero)
                {
                    _springMass = (Fixed32)1.0 / _springMass;
                }
            }
            else
            {
                _springImpulse = Fixed32.Zero;
            }

            if (_enableLimit)
            {
                _translation = MathUtils.Dot(_ax, d);
            }
            else
            {
                _lowerImpulse = Fixed32.Zero;
                _upperImpulse = Fixed32.Zero;
            }

            if (_enableMotor)
            {
                _motorMass = iA + iB;
                if (_motorMass > Fixed32.Zero)
                {
                    _motorMass = (Fixed32)1.0 / _motorMass;
                }
            }
            else
            {
                _motorMass = Fixed32.Zero;
                _motorImpulse = Fixed32.Zero;
            }

            if (data.Step.WarmStarting)
            {
                // Account for variable time step.
                _impulse *= data.Step.DeltaTimeRatio;
                _springImpulse *= data.Step.DeltaTimeRatio;
                _motorImpulse *= data.Step.DeltaTimeRatio;

                Fixed32 axialImpulse = _springImpulse + _lowerImpulse - _upperImpulse;
                Vector2 P = _impulse * _ay + axialImpulse * _ax;
                Fixed32 LA = _impulse * _sAy + axialImpulse * _sAx + _motorImpulse;
                Fixed32 LB = _impulse * _sBy + axialImpulse * _sBx + _motorImpulse;

                vA -= _invMassA * P;
                wA -= _invIA * LA;

                vB += _invMassB * P;
                wB += _invIB * LB;
            }
            else
            {
                _impulse = Fixed32.Zero;
                _springImpulse = Fixed32.Zero;
                _motorImpulse = Fixed32.Zero;
                _lowerImpulse = Fixed32.Zero;
                _upperImpulse = Fixed32.Zero;
            }

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        internal override void SolveVelocityConstraints(ref SolverData data)
        {
            Fixed32 mA = _invMassA, mB = _invMassB;
            Fixed32 iA = _invIA, iB = _invIB;

            Vector2 vA = data.Velocities[_indexA].V;
            Fixed32 wA = data.Velocities[_indexA].W;
            Vector2 vB = data.Velocities[_indexB].V;
            Fixed32 wB = data.Velocities[_indexB].W;

            // Solve spring constraint
            {
                Fixed32 Cdot = MathUtils.Dot(_ax, vB - vA) + _sBx * wB - _sAx * wA;
                Fixed32 impulse = -_springMass * (Cdot + _bias + _gamma * _springImpulse);
                _springImpulse += impulse;

                Vector2 P = impulse * _ax;
                Fixed32 LA = impulse * _sAx;
                Fixed32 LB = impulse * _sBx;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }

            // Solve rotational motor constraint
            {
                Fixed32 Cdot = wB - wA - _motorSpeed;
                Fixed32 impulse = -_motorMass * Cdot;

                Fixed32 oldImpulse = _motorImpulse;
                Fixed32 maxImpulse = data.Step.DeltaTime * _maxMotorTorque;
                _motorImpulse = MathUtils.Clamp(_motorImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _motorImpulse - oldImpulse;

                wA -= iA * impulse;
                wB += iB * impulse;
            }

            if (_enableLimit)
            {
                // Lower limit
                {
                    Fixed32 C = _translation - _lowerTranslation;
                    Fixed32 Cdot = MathUtils.Dot(_ax, vB - vA) + _sBx * wB - _sAx * wA;
                    Fixed32 impulse = -_axialMass * (Cdot + MathUtils.Max(C, Fixed32.Zero) * data.Step.InvertedDeltaTime);
                    Fixed32 oldImpulse = _lowerImpulse;
                    _lowerImpulse = MathUtils.Max(_lowerImpulse + impulse, Fixed32.Zero);
                    impulse = _lowerImpulse - oldImpulse;

                    Vector2 P = impulse * _ax;
                    Fixed32 LA = impulse * _sAx;
                    Fixed32 LB = impulse * _sBx;

                    vA -= mA * P;
                    wA -= iA * LA;
                    vB += mB * P;
                    wB += iB * LB;
                }

                // Upper limit
                // Note: signs are flipped to keep C positive when the constraint is satisfied.
                // This also keeps the impulse positive when the limit is active.
                {
                    Fixed32 C = _upperTranslation - _translation;
                    Fixed32 Cdot = MathUtils.Dot(_ax, vA - vB) + _sAx * wA - _sBx * wB;
                    Fixed32 impulse = -_axialMass * (Cdot + MathUtils.Max(C, Fixed32.Zero) * data.Step.InvertedDeltaTime);
                    Fixed32 oldImpulse = _upperImpulse;
                    _upperImpulse = MathUtils.Max(_upperImpulse + impulse, Fixed32.Zero);
                    impulse = _upperImpulse - oldImpulse;

                    Vector2 P = impulse * _ax;
                    Fixed32 LA = impulse * _sAx;
                    Fixed32 LB = impulse * _sBx;

                    vA += mA * P;
                    wA += iA * LA;
                    vB -= mB * P;
                    wB -= iB * LB;
                }
            }

            // Solve point to line constraint
            {
                Fixed32 Cdot = MathUtils.Dot(_ay, vB - vA) + _sBy * wB - _sAy * wA;
                Fixed32 impulse = -_mass * Cdot;
                _impulse += impulse;

                Vector2 P = impulse * _ay;
                Fixed32 LA = impulse * _sAy;
                Fixed32 LB = impulse * _sBy;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        internal override bool SolvePositionConstraints(ref SolverData data)
        {
            Vector2 cA = data.Positions[_indexA].C;
            Fixed32 aA = data.Positions[_indexA].A;
            Vector2 cB = data.Positions[_indexB].C;
            Fixed32 aB = data.Positions[_indexB].A;

            Fixed32 linearError = Fixed32.Zero;

            if (_enableLimit)
            {
                Rot qA = new Rot(aA), qB = new Rot(aB);

                Vector2 rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
                Vector2 rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
                Vector2 d = (cB - cA) + rB - rA;

                Vector2 ax = MathUtils.Mul(qA, _localXAxisA);
                Fixed32 sAx = MathUtils.Cross(d + rA, _ax);
                Fixed32 sBx = MathUtils.Cross(rB, _ax);

                Fixed32 C = Fixed32.Zero;
                Fixed32 translation = MathUtils.Dot(ax, d);
                if (MathUtils.Abs(_upperTranslation - _lowerTranslation) < (Fixed32)2.0 * Settings.LinearSlop)
                {
                    C = translation;
                }
                else if (translation <= _lowerTranslation)
                {
                    C = MathUtils.Min(translation - _lowerTranslation, Fixed32.Zero);
                }
                else if (translation >= _upperTranslation)
                {
                    C = MathUtils.Max(translation - _upperTranslation, Fixed32.Zero);
                }

                if (C != Fixed32.Zero)
                {

                    Fixed32 invMass = _invMassA + _invMassB + _invIA * sAx * sAx + _invIB * sBx * sBx;
                    Fixed32 impulse = Fixed32.Zero;
                    if (invMass != Fixed32.Zero)
                    {
                        impulse = -C / invMass;
                    }

                    Vector2 P = impulse * ax;
                    Fixed32 LA = impulse * sAx;
                    Fixed32 LB = impulse * sBx;

                    cA -= _invMassA * P;
                    aA -= _invIA * LA;
                    cB += _invMassB * P;
                    aB += _invIB * LB;

                    linearError = MathUtils.Abs(C);
                }
            }

            // Solve perpendicular constraint
            {
                Rot qA = new Rot(aA), qB = new Rot(aB);

                Vector2 rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
                Vector2 rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
                Vector2 d = (cB - cA) + rB - rA;

                Vector2 ay = MathUtils.Mul(qA, _localYAxisA);

                Fixed32 sAy = MathUtils.Cross(d + rA, ay);
                Fixed32 sBy = MathUtils.Cross(rB, ay);

                Fixed32 C = MathUtils.Dot(d, ay);

                Fixed32 invMass = _invMassA + _invMassB + _invIA * _sAy * _sAy + _invIB * _sBy * _sBy;

                Fixed32 impulse = Fixed32.Zero;
                if (invMass != Fixed32.Zero)
                {
                    impulse = -C / invMass;
                }

                Vector2 P = impulse * ay;
                Fixed32 LA = impulse * sAy;
                Fixed32 LB = impulse * sBy;

                cA -= _invMassA * P;
                aA -= _invIA * LA;
                cB += _invMassB * P;
                aB += _invIB * LB;

                linearError = MathUtils.Max(linearError, MathUtils.Abs(C));
            }

            data.Positions[_indexA].C = cA;
            data.Positions[_indexA].A = aA;
            data.Positions[_indexB].C = cB;
            data.Positions[_indexB].A = aB;

            return linearError <= Settings.LinearSlop;
        }
    }
}
