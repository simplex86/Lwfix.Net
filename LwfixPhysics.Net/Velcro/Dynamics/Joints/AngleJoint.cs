/*
* Velcro Physics:
* Copyright (c) 2017 Ian Qvist
*/

using System.Diagnostics;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.Misc;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Solver;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Utilities;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints
{
    /// <summary>Maintains a fixed angle between two bodies</summary>
    public class AngleJoint : Joint
    {
        private Fixed32 _bias;
        private Fixed32 _jointError;
        private Fixed32 _massFactor;
        private Fixed32 _targetAngle;
        private Fixed32 _biasFactor;
        private Fixed32 _maxImpulse;
        private Fixed32 _softness;

        public AngleJoint(Body bodyA, Body bodyB)
            : base(bodyA, bodyB, JointType.Angle)
        {
            _biasFactor = (Fixed32)0.2;
            _maxImpulse = Fixed32.MaxValue;
        }

        public override Vector2 WorldAnchorA
        {
            get => _bodyA.Position;
            set => Debug.Assert(false, "You can't set the world anchor on this joint type.");
        }

        public override Vector2 WorldAnchorB
        {
            get => _bodyB.Position;
            set => Debug.Assert(false, "You can't set the world anchor on this joint type.");
        }

        /// <summary>The desired angle between BodyA and BodyB</summary>
        public Fixed32 TargetAngle
        {
            get => _targetAngle;
            set
            {
                if (_targetAngle != value)
                {
                    _targetAngle = value;
                    WakeBodies();
                }
            }
        }

        /// <summary>Gets or sets the bias factor. Defaults to 0.2</summary>
        public Fixed32 BiasFactor
        {
            get => _biasFactor;
            set => _biasFactor = value;
        }

        /// <summary>Gets or sets the maximum impulse. Defaults to float.MaxValue</summary>
        public Fixed32 MaxImpulse
        {
            get => _maxImpulse;
            set => _maxImpulse = value;
        }

        /// <summary>Gets or sets the softness of the joint. Defaults to 0</summary>
        public Fixed32 Softness
        {
            get => _softness;
            set => _softness = value;
        }

        public override Vector2 GetReactionForce(Fixed32 invDt)
        {
            return Vector2.Zero;
        }

        public override Fixed32 GetReactionTorque(Fixed32 invDt)
        {
            return Fixed32.Zero;
        }

        internal override void InitVelocityConstraints(ref SolverData data)
        {
            int indexA = _bodyA.IslandIndex;
            int indexB = _bodyB.IslandIndex;

            Fixed32 aW = data.Positions[indexA].A;
            Fixed32 bW = data.Positions[indexB].A;

            _jointError = bW - aW - _targetAngle;
            _bias = -_biasFactor * data.Step.InvertedDeltaTime * _jointError;
            _massFactor = ((Fixed32)1.0 - _softness) / (_bodyA._invI + _bodyB._invI);
        }

        internal override void SolveVelocityConstraints(ref SolverData data)
        {
            int indexA = _bodyA.IslandIndex;
            int indexB = _bodyB.IslandIndex;

            Fixed32 p = (_bias - data.Velocities[indexB].W + data.Velocities[indexA].W) * _massFactor;

            data.Velocities[indexA].W -= _bodyA._invI * MathUtils.Sign(p) * MathUtils.Min(MathUtils.Abs(p), _maxImpulse);
            data.Velocities[indexB].W += _bodyB._invI * MathUtils.Sign(p) * MathUtils.Min(MathUtils.Abs(p), _maxImpulse);
        }

        internal override bool SolvePositionConstraints(ref SolverData data)
        {
            //no position solving for this joint
            return true;
        }
    }
}
