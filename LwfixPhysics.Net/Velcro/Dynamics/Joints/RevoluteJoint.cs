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

using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Definitions.Joints;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Solver;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints
{
    // Point-to-point constraint
    // C = p2 - p1
    // Cdot = v2 - v1
    //      = v2 + cross(w2, r2) - v1 - cross(w1, r1)
    // J = [-I -r1_skew I r2_skew ]
    // Identity used:
    // w k % (rx i + ry j) = w * (-ry i + rx j)

    // Motor constraint
    // Cdot = w2 - w1
    // J = [0 0 -1 0 0 1]
    // K = invI1 + invI2

    /// <summary>
    /// A revolute joint constrains to bodies to share a common point while they are free to rotate about the point.
    /// The relative rotation about the shared point is the joint angle. You can limit the relative rotation with a joint limit
    /// that specifies a lower and upper angle. You can use a motor to drive the relative rotation about the shared point. A
    /// maximum motor torque is provided so that infinite forces are not generated.
    /// </summary>
    public class RevoluteJoint : Joint
    {
        // Solver shared
        internal Vector2 _localAnchorA;
        internal Vector2 _localAnchorB;
        private Vector2 _impulse;
        private Fixed32 _motorImpulse;
        private Fixed32 _lowerImpulse;
        private Fixed32 _upperImpulse;
        private bool _enableMotor;
        private Fixed32 _maxMotorTorque;
        private Fixed32 _motorSpeed;
        private bool _enableLimit;
        internal Fixed32 _referenceAngle;
        private Fixed32 _lowerAngle;
        private Fixed32 _upperAngle;

        // Solver temp
        private int _indexA;
        private int _indexB;
        private Vector2 _rA;
        private Vector2 _rB;
        private Vector2 _localCenterA;
        private Vector2 _localCenterB;
        private Fixed32 _invMassA;
        private Fixed32 _invMassB;
        private Fixed32 _invIA;
        private Fixed32 _invIB;
        private Mat22 _K;
        private Fixed32 _angle;
        private Fixed32 _axialMass;

        public RevoluteJoint(RevoluteJointDef def)
            : base(def)
        {
            _localAnchorA = def.LocalAnchorA;
            _localAnchorB = def.LocalAnchorB;
            _referenceAngle = def.ReferenceAngle;

            _lowerAngle = def.LowerAngle;
            _upperAngle = def.UpperAngle;
            _maxMotorTorque = def.MaxMotorTorque;
            _motorSpeed = def.MotorSpeed;
            _enableLimit = def.EnableLimit;
            _enableMotor = def.EnableMotor;

            _angle = Fixed32.Zero;
        }

        /// <summary>Constructor of RevoluteJoint.</summary>
        /// <param name="bodyA">The first body.</param>
        /// <param name="bodyB">The second body.</param>
        /// <param name="anchorA">The first body anchor.</param>
        /// <param name="anchorB">The second anchor.</param>
        /// <param name="useWorldCoordinates">Set to true if you are using world coordinates as anchors.</param>
        public RevoluteJoint(Body bodyA, Body bodyB, Vector2 anchorA, Vector2 anchorB, bool useWorldCoordinates = false)
            : base(bodyA, bodyB, JointType.Revolute)
        {
            if (useWorldCoordinates)
            {
                _localAnchorA = bodyA.GetLocalPoint(anchorA);
                _localAnchorB = bodyB.GetLocalPoint(anchorB);
            }
            else
            {
                _localAnchorA = anchorA;
                _localAnchorB = anchorB;
            }

            _referenceAngle = bodyB._sweep.A - bodyA._sweep.A;
        }

        /// <summary>Constructor of RevoluteJoint.</summary>
        /// <param name="bodyA">The first body.</param>
        /// <param name="bodyB">The second body.</param>
        /// <param name="anchor">The shared anchor.</param>
        /// <param name="useWorldCoordinates"></param>
        public RevoluteJoint(Body bodyA, Body bodyB, Vector2 anchor, bool useWorldCoordinates = false)
            : this(bodyA, bodyB, anchor, anchor, useWorldCoordinates) { }

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

        /// <summary>The referance angle computed as BodyB angle minus BodyA angle.</summary>
        public Fixed32 ReferenceAngle
        {
            get => _referenceAngle;
            set => _referenceAngle = value;
        }

        /// <summary>Get the current joint angle in radians.</summary>
        public Fixed32 JointAngle => _bodyB._sweep.A - _bodyA._sweep.A - _referenceAngle;

        /// <summary>Get the current joint angle speed in radians per second.</summary>
        public Fixed32 JointSpeed => _bodyB._angularVelocity - _bodyA._angularVelocity;

        /// <summary>Is the joint limit enabled?</summary>
        /// <value><c>true</c> if [limit enabled]; otherwise, <c>false</c>.</value>
        public bool LimitEnabled
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

        /// <summary>Get the lower joint limit in radians.</summary>
        public Fixed32 LowerLimit
        {
            get => _lowerAngle;
            set
            {
                if (_lowerAngle != value)
                {
                    WakeBodies();
                    _lowerAngle = value;
                    _lowerImpulse = Fixed32.Zero;
                }
            }
        }

        /// <summary>Get the upper joint limit in radians.</summary>
        public Fixed32 UpperLimit
        {
            get => _upperAngle;
            set
            {
                if (_upperAngle != value)
                {
                    WakeBodies();
                    _upperAngle = value;
                    _upperImpulse = Fixed32.Zero;
                }
            }
        }

        /// <summary>Is the joint motor enabled?</summary>
        /// <value><c>true</c> if [motor enabled]; otherwise, <c>false</c>.</value>
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

        /// <summary>Get or set the motor speed in radians per second.</summary>
        public Fixed32 MotorSpeed
        {
            set
            {
                if (value != _motorSpeed)
                {
                    WakeBodies();
                    _motorSpeed = value;
                }
            }
            get => _motorSpeed;
        }

        /// <summary>Get or set the maximum motor torque, usually in N-m.</summary>
        public Fixed32 MaxMotorTorque
        {
            set
            {
                if (value != _maxMotorTorque)
                {
                    WakeBodies();
                    _maxMotorTorque = value;
                }
            }
            get => _maxMotorTorque;
        }

        /// <summary>Set the joint limits, usually in meters.</summary>
        /// <param name="lower">The lower limit</param>
        /// <param name="upper">The upper limit</param>
        public void SetLimits(Fixed32 lower, Fixed32 upper)
        {
            if (lower != _lowerAngle || upper != _upperAngle)
            {
                WakeBodies();
                _lowerImpulse = Fixed32.Zero;
                _upperImpulse = Fixed32.Zero;
                _upperAngle = upper;
                _lowerAngle = lower;
            }
        }

        /// <summary>Gets the motor torque in N-m.</summary>
        /// <param name="invDt">The inverse delta time</param>
        public Fixed32 GetMotorTorque(Fixed32 invDt)
        {
            return invDt * _motorImpulse;
        }

        public override Vector2 GetReactionForce(Fixed32 invDt)
        {
            Vector2 p = new Vector2(_impulse.X, _impulse.Y);
            return invDt * p;
        }

        public override Fixed32 GetReactionTorque(Fixed32 invDt)
        {
            return invDt * (_motorImpulse + _lowerImpulse - _upperImpulse);
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

            Fixed32 aA = data.Positions[_indexA].A;
            Vector2 vA = data.Velocities[_indexA].V;
            Fixed32 wA = data.Velocities[_indexA].W;

            Fixed32 aB = data.Positions[_indexB].A;
            Vector2 vB = data.Velocities[_indexB].V;
            Fixed32 wB = data.Velocities[_indexB].W;

            Rot qA = new Rot(aA), qB = new Rot(aB);

            _rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            _rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);

            // J = [-I -r1_skew I r2_skew]
            // r_skew = [-ry; rx]

            // Matlab
            // K = [ mA+r1y^2*iA+mB+r2y^2*iB,  -r1y*iA*r1x-r2y*iB*r2x]
            //     [  -r1y*iA*r1x-r2y*iB*r2x, mA+r1x^2*iA+mB+r2x^2*iB]

            Fixed32 mA = _invMassA, mB = _invMassB;
            Fixed32 iA = _invIA, iB = _invIB;

            _K.ex.X = mA + mB + _rA.Y * _rA.Y * iA + _rB.Y * _rB.Y * iB;
            _K.ey.X = -_rA.Y * _rA.X * iA - _rB.Y * _rB.X * iB;
            _K.ex.Y = _K.ey.X;
            _K.ey.Y = mA + mB + _rA.X * _rA.X * iA + _rB.X * _rB.X * iB;

            _axialMass = iA + iB;
            bool fixedRotation;
            if (_axialMass > Fixed32.Zero)
            {
                _axialMass = (Fixed32)1.0 / _axialMass;
                fixedRotation = false;
            }
            else
            {
                fixedRotation = true;
            }

            _angle = aB - aA - _referenceAngle;
            if (_enableLimit == false || fixedRotation)
            {
                _lowerImpulse = Fixed32.Zero;
                _upperImpulse = Fixed32.Zero;
            }

            if (_enableMotor == false || fixedRotation)
            {
                _motorImpulse = Fixed32.Zero;
            }

            if (data.Step.WarmStarting)
            {
                // Scale impulses to support a variable time step.
                _impulse *= data.Step.DeltaTimeRatio;
                _motorImpulse *= data.Step.DeltaTimeRatio;
                _lowerImpulse *= data.Step.DeltaTimeRatio;
                _upperImpulse *= data.Step.DeltaTimeRatio;

                Fixed32 axialImpulse = _motorImpulse + _lowerImpulse - _upperImpulse;
                Vector2 P = new Vector2(_impulse.X, _impulse.Y);

                vA -= mA * P;
                wA -= iA * (MathUtils.Cross(_rA, P) + axialImpulse);

                vB += mB * P;
                wB += iB * (MathUtils.Cross(_rB, P) + axialImpulse);
            }
            else
            {
                _impulse = Vector2.Zero;
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
            Vector2 vA = data.Velocities[_indexA].V;
            Fixed32 wA = data.Velocities[_indexA].W;
            Vector2 vB = data.Velocities[_indexB].V;
            Fixed32 wB = data.Velocities[_indexB].W;

            Fixed32 mA = _invMassA, mB = _invMassB;
            Fixed32 iA = _invIA, iB = _invIB;

            bool fixedRotation = (iA + iB == Fixed32.Zero);

            // Solve motor constraint.
            if (_enableMotor && fixedRotation == false)
            {
                Fixed32 Cdot = wB - wA - _motorSpeed;
                Fixed32 impulse = -_axialMass * Cdot;
                Fixed32 oldImpulse = _motorImpulse;
                Fixed32 maxImpulse = data.Step.DeltaTime * _maxMotorTorque;
                _motorImpulse = MathUtils.Clamp(_motorImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _motorImpulse - oldImpulse;

                wA -= iA * impulse;
                wB += iB * impulse;
            }

            if (_enableLimit && fixedRotation == false)
            {
                // Lower limit
                {
                    Fixed32 C = _angle - _lowerAngle;
                    Fixed32 Cdot = wB - wA;
                    Fixed32 impulse = -_axialMass * (Cdot + MathUtils.Max(C, Fixed32.Zero) * data.Step.InvertedDeltaTime);
                    Fixed32 oldImpulse = _lowerImpulse;
                    _lowerImpulse = MathUtils.Max(_lowerImpulse + impulse, Fixed32.Zero);
                    impulse = _lowerImpulse - oldImpulse;

                    wA -= iA * impulse;
                    wB += iB * impulse;
                }

                // Upper limit
                // Note: signs are flipped to keep C positive when the constraint is satisfied.
                // This also keeps the impulse positive when the limit is active.
                {
                    Fixed32 C = _upperAngle - _angle;
                    Fixed32 Cdot = wA - wB;
                    Fixed32 impulse = -_axialMass * (Cdot + MathUtils.Max(C, Fixed32.Zero) * data.Step.InvertedDeltaTime);
                    Fixed32 oldImpulse = _upperImpulse;
                    _upperImpulse = MathUtils.Max(_upperImpulse + impulse, Fixed32.Zero);
                    impulse = _upperImpulse - oldImpulse;

                    wA += iA * impulse;
                    wB -= iB * impulse;
                }
            }

            // Solve point-to-point constraint
            {
                Vector2 Cdot = vB + MathUtils.Cross(wB, _rB) - vA - MathUtils.Cross(wA, _rA);
                Vector2 impulse = _K.Solve(-Cdot);

                _impulse.X += impulse.X;
                _impulse.Y += impulse.Y;

                vA -= mA * impulse;
                wA -= iA * MathUtils.Cross(_rA, impulse);

                vB += mB * impulse;
                wB += iB * MathUtils.Cross(_rB, impulse);
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

            Rot qA = new Rot(aA), qB = new Rot(aB);

            Fixed32 angularError = Fixed32.Zero;
            Fixed32 positionError = Fixed32.Zero;

            bool fixedRotation = (_invIA + _invIB == Fixed32.Zero);

            // Solve angular limit constraint
            if (_enableLimit && fixedRotation == false)
            {
                Fixed32 angle = aB - aA - _referenceAngle;
                Fixed32 C = Fixed32.Zero;

                if (MathUtils.Abs(_upperAngle - _lowerAngle) < (Fixed32)2.0 * Settings.AngularSlop)
                {
                    // Prevent large angular corrections
                    C = MathUtils.Clamp(angle - _lowerAngle, -Settings.MaxAngularCorrection, Settings.MaxAngularCorrection);
                }
                else if (angle <= _lowerAngle)
                {
                    // Prevent large angular corrections and allow some slop.
                    C = MathUtils.Clamp(angle - _lowerAngle + Settings.AngularSlop, -Settings.MaxAngularCorrection, Fixed32.Zero);
                }
                else if (angle >= _upperAngle)
                {
                    // Prevent large angular corrections and allow some slop.
                    C = MathUtils.Clamp(angle - _upperAngle - Settings.AngularSlop, Fixed32.Zero, Settings.MaxAngularCorrection);
                }

                Fixed32 limitImpulse = -_axialMass * C;
                aA -= _invIA * limitImpulse;
                aB += _invIB * limitImpulse;
                angularError = MathUtils.Abs(C);
            }

            // Solve point-to-point constraint.
            {
                qA.Set(aA);
                qB.Set(aB);
                Vector2 rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
                Vector2 rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);

                Vector2 C = cB + rB - cA - rA;
                positionError = C.Length();

                Fixed32 mA = _invMassA, mB = _invMassB;
                Fixed32 iA = _invIA, iB = _invIB;

                Mat22 K;
                K.ex.X = mA + mB + iA * rA.Y * rA.Y + iB * rB.Y * rB.Y;
                K.ex.Y = -iA * rA.X * rA.Y - iB * rB.X * rB.Y;
                K.ey.X = K.ex.Y;
                K.ey.Y = mA + mB + iA * rA.X * rA.X + iB * rB.X * rB.X;

                Vector2 impulse = -K.Solve(C);

                cA -= mA * impulse;
                aA -= iA * MathUtils.Cross(rA, impulse);

                cB += mB * impulse;
                aB += iB * MathUtils.Cross(rB, impulse);
            }

            data.Positions[_indexA].C = cA;
            data.Positions[_indexA].A = aA;
            data.Positions[_indexB].C = cB;
            data.Positions[_indexB].A = aB;

            return positionError <= Settings.LinearSlop && angularError <= Settings.AngularSlop;
        }
    }
}
