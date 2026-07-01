using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints;
using SimplexLab.LwfixPhysics.Velcro.Factories;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Prefabs;

using PhysicsVector2 = SimplexLab.LwfixPhysics.Velcro.Primitives.Vector2;

/// <summary>
/// A spider-like compound body (torso + two jointed legs per side) driven by
/// angle joints that periodically flex/relax, making it jump. Physics-only port.
/// </summary>
public sealed class JumpySpider
{
    private const float FlexTime = 5000f;
    private const float RelaxTime = 5000f;

    private const float ShoulderFlexed = -1.2f;
    private const float ShoulderRelaxed = -0.2f;
    private const float KneeFlexed = -1.4f;
    private const float KneeRelaxed = -0.4f;

    private const float SpiderBodyRadius = 0.65f;

    private readonly AngleJoint _leftShoulder;
    private readonly AngleJoint _leftKnee;
    private readonly AngleJoint _rightShoulder;
    private readonly AngleJoint _rightKnee;

    private bool _flexed;
    private float _timer;

    public Body Body { get; }

    public JumpySpider(World world, PhysicsVector2 position)
    {
        var upperLegSize = new PhysicsVector2((Fixed32)1.8, (Fixed32)0.3);
        var lowerLegSize = new PhysicsVector2((Fixed32)1.8, (Fixed32)0.3);

        // Torso
        Body = BodyFactory.CreateCircle(world, (Fixed32)SpiderBodyRadius, (Fixed32)0.1, position);
        Body.BodyType = BodyType.Dynamic;

        var radius = new PhysicsVector2((Fixed32)SpiderBodyRadius, Fixed32.Zero);
        var halfUpper = new PhysicsVector2(upperLegSize.X / (Fixed32)2, Fixed32.Zero);
        var fullUpper = new PhysicsVector2(upperLegSize.X, Fixed32.Zero);
        var halfLower = new PhysicsVector2(lowerLegSize.X / (Fixed32)2, Fixed32.Zero);

        // Left upper leg
        Body leftUpper = BodyFactory.CreateRectangle(world, upperLegSize.X, upperLegSize.Y, (Fixed32)0.1,
            Body.Position - radius - halfUpper);
        leftUpper.BodyType = BodyType.Dynamic;

        // Left lower leg
        Body leftLower = BodyFactory.CreateRectangle(world, lowerLegSize.X, lowerLegSize.Y, (Fixed32)0.1,
            Body.Position - radius - fullUpper - halfLower);
        leftLower.BodyType = BodyType.Dynamic;

        // Right upper leg
        Body rightUpper = BodyFactory.CreateRectangle(world, upperLegSize.X, upperLegSize.Y, (Fixed32)0.1,
            Body.Position + radius + halfUpper);
        rightUpper.BodyType = BodyType.Dynamic;

        // Right lower leg
        Body rightLower = BodyFactory.CreateRectangle(world, lowerLegSize.X, lowerLegSize.Y, (Fixed32)0.1,
            Body.Position + radius + fullUpper + halfLower);
        rightLower.BodyType = BodyType.Dynamic;

        // Joints
        JointFactory.CreateRevoluteJoint(world, Body, leftUpper, halfUpper);
        _leftShoulder = JointFactory.CreateAngleJoint(world, Body, leftUpper);
        _leftShoulder.MaxImpulse = (Fixed32)3;

        JointFactory.CreateRevoluteJoint(world, Body, rightUpper, -halfUpper);
        _rightShoulder = JointFactory.CreateAngleJoint(world, Body, rightUpper);
        _rightShoulder.MaxImpulse = (Fixed32)3;

        JointFactory.CreateRevoluteJoint(world, leftUpper, leftLower, halfLower);
        _leftKnee = JointFactory.CreateAngleJoint(world, leftUpper, leftLower);
        _leftKnee.MaxImpulse = (Fixed32)3;

        JointFactory.CreateRevoluteJoint(world, rightUpper, rightLower, -halfLower);
        _rightKnee = JointFactory.CreateAngleJoint(world, rightUpper, rightLower);
        _rightKnee.MaxImpulse = (Fixed32)3;

        _leftShoulder.TargetAngle = (Fixed32)ShoulderRelaxed;
        _leftKnee.TargetAngle = (Fixed32)KneeRelaxed;
        _rightShoulder.TargetAngle = (Fixed32)(-ShoulderRelaxed);
        _rightKnee.TargetAngle = (Fixed32)(-KneeRelaxed);
    }

    public void Update(float dtMilliseconds)
    {
        _timer += dtMilliseconds;
        if (_flexed)
        {
            if (_timer >= FlexTime)
            {
                _timer = 0;
                _flexed = false;
                _leftShoulder.TargetAngle = (Fixed32)ShoulderRelaxed;
                _leftKnee.TargetAngle = (Fixed32)KneeRelaxed;
                _rightShoulder.TargetAngle = (Fixed32)(-ShoulderRelaxed);
                _rightKnee.TargetAngle = (Fixed32)(-KneeRelaxed);
            }
        }
        else if (_timer >= RelaxTime)
        {
            _timer = 0;
            _flexed = true;
            _leftShoulder.TargetAngle = (Fixed32)ShoulderFlexed;
            _leftKnee.TargetAngle = (Fixed32)KneeFlexed;
            _rightShoulder.TargetAngle = (Fixed32)(-ShoulderFlexed);
            _rightKnee.TargetAngle = (Fixed32)(-KneeFlexed);
        }
    }
}
