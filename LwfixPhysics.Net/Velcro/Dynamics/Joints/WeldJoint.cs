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

using System;
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

    // Angle constraint
    // C = angle2 - angle1 - referenceAngle
    // Cdot = w2 - w1
    // J = [0 0 -1 0 0 1]
    // K = invI1 + invI2

    /// <summary>
    /// A weld joint essentially glues two bodies together. A weld joint may distort somewhat because the island
    /// constraint solver is approximate. The joint is soft constraint based, which means the two bodies will move relative to
    /// each other, when a force is applied. To combine two bodies in a rigid fashion, combine the fixtures to a single body
    /// instead.
    /// </summary>
    public class WeldJoint : Joint
    {
        private Fixed32 _stiffness;
        private Fixed32 _damping;
        private Fixed32 _bias;

        // Solver shared
        private Vector2 _localAnchorA;
        private Vector2 _localAnchorB;
        private Fixed32 _referenceAngle;
        private Fixed32 _gamma;
        private Vector3 _impulse;

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
        private Mat33 _mass;

        public WeldJoint(WeldJointDef def)
            : base(def)
        {
            _localAnchorA = def.LocalAnchorA;
            _localAnchorB = def.LocalAnchorB;
            _referenceAngle = def.ReferenceAngle;
            _stiffness = def.Stiffness;
            _damping = def.Damping;

            _impulse = Vector3.Zero;
        }

        /// <summary>
        /// You need to specify an anchor point where they are attached. The position of the anchor point is important for
        /// computing the reaction torque.
        /// </summary>
        /// <param name="bodyA">The first body</param>
        /// <param name="bodyB">The second body</param>
        /// <param name="anchorA">The first body anchor.</param>
        /// <param name="anchorB">The second body anchor.</param>
        /// <param name="useWorldCoordinates">Set to true if you are using world coordinates as anchors.</param>
        public WeldJoint(Body bodyA, Body bodyB, Vector2 anchorA, Vector2 anchorB, bool useWorldCoordinates = false)
            : base(bodyA, bodyB, JointType.Weld)
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

        /// <summary>The bodyB angle minus bodyA angle in the reference state (radians).</summary>
        public Fixed32 ReferenceAngle => _referenceAngle;

        /// <summary>
        /// The frequency of the joint. A higher frequency means a stiffer joint, but a too high value can cause the joint
        /// to oscillate. Default is 0, which means the joint does no spring calculations.
        /// </summary>
        public Fixed32 Stiffness
        {
            get => _stiffness;
            set => _stiffness = value;
        }

        /// <summary>
        /// The damping on the joint. The damping is only used when the joint has a frequency (> 0). A higher value means
        /// more damping.
        /// </summary>
        public Fixed32 Damping
        {
            get => _damping;
            set => _damping = value;
        }

        public override Vector2 GetReactionForce(Fixed32 invDt)
        {
            Vector2 P = new Vector2(_impulse.X, _impulse.Y);
            return invDt * P;
        }

        public override Fixed32 GetReactionTorque(Fixed32 invDt)
        {
            return invDt * _impulse.Z;
        }

        internal override void InitVelocityConstraints(ref SolverData data)
        {
            _indexA = BodyA.IslandIndex;
            _indexB = BodyB.IslandIndex;
            _localCenterA = BodyA._sweep.LocalCenter;
            _localCenterB = BodyB._sweep.LocalCenter;
            _invMassA = BodyA._invMass;
            _invMassB = BodyB._invMass;
            _invIA = BodyA._invI;
            _invIB = BodyB._invI;

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
            //     [ 0       -1 0       1]
            // r_skew = [-ry; rx]

            // Matlab
            // K = [ mA+r1y^2*iA+mB+r2y^2*iB,  -r1y*iA*r1x-r2y*iB*r2x,          -r1y*iA-r2y*iB]
            //     [  -r1y*iA*r1x-r2y*iB*r2x, mA+r1x^2*iA+mB+r2x^2*iB,           r1x*iA+r2x*iB]
            //     [          -r1y*iA-r2y*iB,           r1x*iA+r2x*iB,                   iA+iB]

            Fixed32 mA = _invMassA, mB = _invMassB;
            Fixed32 iA = _invIA, iB = _invIB;

            Mat33 K;
            K.ex.X = mA + mB + _rA.Y * _rA.Y * iA + _rB.Y * _rB.Y * iB;
            K.ey.X = -_rA.Y * _rA.X * iA - _rB.Y * _rB.X * iB;
            K.ez.X = -_rA.Y * iA - _rB.Y * iB;
            K.ex.Y = K.ey.X;
            K.ey.Y = mA + mB + _rA.X * _rA.X * iA + _rB.X * _rB.X * iB;
            K.ez.Y = _rA.X * iA + _rB.X * iB;
            K.ex.Z = K.ez.X;
            K.ey.Z = K.ez.Y;
            K.ez.Z = iA + iB;

            if (_stiffness > Fixed32.Zero)
            {
                K.GetInverse22(ref _mass);

                Fixed32 invM = iA + iB;

                Fixed32 C = aB - aA - _referenceAngle;

                // Damping coefficient
                Fixed32 d = _damping;

                // Spring stiffness
                Fixed32 k = _stiffness;

                // magic formulas
                Fixed32 h = data.Step.DeltaTime;
                _gamma = h * (d + h * k);
                _gamma = _gamma != Fixed32.Zero ? (Fixed32)1.0 / _gamma : Fixed32.Zero;
                _bias = C * h * k * _gamma;

                invM += _gamma;
                _mass.ez.Z = invM != Fixed32.Zero ? (Fixed32)1.0 / invM : Fixed32.Zero;
            }
            else if (K.ez.Z == Fixed32.Zero)
            {
                K.GetInverse22(ref _mass);
                _gamma = Fixed32.Zero;
                _bias = Fixed32.Zero;
            }
            else
            {
                K.GetSymInverse33(ref _mass);
                _gamma = Fixed32.Zero;
                _bias = Fixed32.Zero;
            }

            if (data.Step.WarmStarting)
            {
                // Scale impulses to support a variable time step.
                _impulse *= data.Step.DeltaTimeRatio;

                Vector2 P = new Vector2(_impulse.X, _impulse.Y);

                vA -= mA * P;
                wA -= iA * (MathUtils.Cross(_rA, P) + _impulse.Z);

                vB += mB * P;
                wB += iB * (MathUtils.Cross(_rB, P) + _impulse.Z);
            }
            else
                _impulse = Vector3.Zero;

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

            if (_stiffness > Fixed32.Zero)
            {
                Fixed32 Cdot2 = wB - wA;

                Fixed32 impulse2 = -_mass.ez.Z * (Cdot2 + _bias + _gamma * _impulse.Z);
                _impulse.Z += impulse2;

                wA -= iA * impulse2;
                wB += iB * impulse2;

                Vector2 Cdot1 = vB + MathUtils.Cross(wB, _rB) - vA - MathUtils.Cross(wA, _rA);

                Vector2 impulse1 = -MathUtils.Mul22(_mass, Cdot1);
                _impulse.X += impulse1.X;
                _impulse.Y += impulse1.Y;

                Vector2 P = impulse1;

                vA -= mA * P;
                wA -= iA * MathUtils.Cross(_rA, P);

                vB += mB * P;
                wB += iB * MathUtils.Cross(_rB, P);
            }
            else
            {
                Vector2 Cdot1 = vB + MathUtils.Cross(wB, _rB) - vA - MathUtils.Cross(wA, _rA);
                Fixed32 Cdot2 = wB - wA;
                Vector3 Cdot = new Vector3(Cdot1.X, Cdot1.Y, Cdot2);

                Vector3 impulse = -MathUtils.Mul(_mass, Cdot);
                _impulse += impulse;

                Vector2 P = new Vector2(impulse.X, impulse.Y);

                vA -= mA * P;
                wA -= iA * (MathUtils.Cross(_rA, P) + impulse.Z);

                vB += mB * P;
                wB += iB * (MathUtils.Cross(_rB, P) + impulse.Z);
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

            Fixed32 mA = _invMassA, mB = _invMassB;
            Fixed32 iA = _invIA, iB = _invIB;

            Vector2 rA = MathUtils.Mul(qA, LocalAnchorA - _localCenterA);
            Vector2 rB = MathUtils.Mul(qB, LocalAnchorB - _localCenterB);

            Fixed32 positionError, angularError;

            Mat33 K = new Mat33();
            K.ex.X = mA + mB + rA.Y * rA.Y * iA + rB.Y * rB.Y * iB;
            K.ey.X = -rA.Y * rA.X * iA - rB.Y * rB.X * iB;
            K.ez.X = -rA.Y * iA - rB.Y * iB;
            K.ex.Y = K.ey.X;
            K.ey.Y = mA + mB + rA.X * rA.X * iA + rB.X * rB.X * iB;
            K.ez.Y = rA.X * iA + rB.X * iB;
            K.ex.Z = K.ez.X;
            K.ey.Z = K.ez.Y;
            K.ez.Z = iA + iB;

            if (_stiffness > Fixed32.Zero)
            {
                Vector2 C1 = cB + rB - cA - rA;

                positionError = C1.Length();
                angularError = Fixed32.Zero;

                Vector2 P = -K.Solve22(C1);

                cA -= mA * P;
                aA -= iA * MathUtils.Cross(rA, P);

                cB += mB * P;
                aB += iB * MathUtils.Cross(rB, P);
            }
            else
            {
                Vector2 C1 = cB + rB - cA - rA;
                Fixed32 C2 = aB - aA - _referenceAngle;

                positionError = C1.Length();
                angularError = FMath.Abs(C2);

                Vector3 C = new Vector3(C1.X, C1.Y, C2);

                Vector3 impulse;
                if (K.ez.Z > Fixed32.Zero)
                    impulse = -K.Solve33(C);
                else
                {
                    Vector2 impulse2 = -K.Solve22(C1);
                    impulse = new Vector3(impulse2.X, impulse2.Y, Fixed32.Zero);
                }

                Vector2 P = new Vector2(impulse.X, impulse.Y);

                cA -= mA * P;
                aA -= iA * (MathUtils.Cross(rA, P) + impulse.Z);

                cB += mB * P;
                aB += iB * (MathUtils.Cross(rB, P) + impulse.Z);
            }

            data.Positions[_indexA].C = cA;
            data.Positions[_indexA].A = aA;
            data.Positions[_indexB].C = cB;
            data.Positions[_indexB].A = aB;

            return positionError <= Settings.LinearSlop && angularError <= Settings.AngularSlop;
        }
    }
}
