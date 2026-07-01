using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints;
using SimplexLab.LwfixPhysics.Velcro.Factories;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Utilities;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Demos;

using PhysicsVector2 = Vector2;

/// <summary>
/// Bodies connected by angle joints (forced equal rotation) and distance joints
/// (one rigid, one soft/spring). Port of D08_DistanceAngleJoint.
/// </summary>
public sealed class D08_DistanceAngleJoint : DemoScreen
{
    public override string Name => "Distance & Angle Joints";
    public override string Title => "Distance & angle joints";
    public override string Details =>
        "This demo shows several bodies connected by distance and angle joints.\n" +
        "Orange bodies are forced to have the same angle at all times.\n" +
        "Striped bodies are forced to have the same distance at all times.\n" +
        "Two of them have a rigid distance joint.\n" +
        "The other two have a soft (spring-like) distance joint.";
    public override string Controls =>
        "Mouse:\n" +
        "  - Grab object (beneath cursor): Left click\n" +
        "  - Drag grabbed object: Move mouse";

    private readonly Body[] _angleBody = new Body[3];
    private readonly Body[] _distanceBody = new Body[4];
    private Body _obstacles = null!;

    protected override void LoadContent()
    {
        World.Gravity = new PhysicsVector2(Fixed32.Zero, (Fixed32)20);

        _obstacles = BodyFactory.CreateBody(World);
        FixtureFactory.AttachEdge(new PhysicsVector2((Fixed32)(-16), (Fixed32)(-1)), new PhysicsVector2((Fixed32)(-14), (Fixed32)1), _obstacles);
        FixtureFactory.AttachEdge(new PhysicsVector2((Fixed32)(-14), (Fixed32)1), new PhysicsVector2((Fixed32)(-12), (Fixed32)(-1)), _obstacles);
        FixtureFactory.AttachEdge(new PhysicsVector2((Fixed32)14, (Fixed32)(-1)), new PhysicsVector2((Fixed32)12, (Fixed32)5), _obstacles);
        FixtureFactory.AttachEdge(new PhysicsVector2((Fixed32)14, (Fixed32)(-1)), new PhysicsVector2((Fixed32)16, (Fixed32)5), _obstacles);

        // Angle-jointed bodies (left side)
        for (int i = 0; i < 3; i++)
        {
            _angleBody[i] = BodyFactory.CreateRectangle(World, (Fixed32)1.5, (Fixed32)1.5, (Fixed32)1);
            _angleBody[i].BodyType = BodyType.Dynamic;
            _angleBody[i].Friction = (Fixed32)0.7;
        }
        _angleBody[0].Position = new PhysicsVector2((Fixed32)(-15), (Fixed32)(-5));
        _angleBody[1].Position = new PhysicsVector2((Fixed32)(-18), (Fixed32)5);
        _angleBody[2].Position = new PhysicsVector2((Fixed32)(-10), (Fixed32)5);

        World.AddJoint(new AngleJoint(_angleBody[0], _angleBody[1]));
        World.AddJoint(new AngleJoint(_angleBody[0], _angleBody[2]));

        // Distance-jointed bodies (right side)
        Fixed32[] xs = { (Fixed32)11.5, (Fixed32)16.5, (Fixed32)11.5, (Fixed32)16.5 };
        Fixed32[] ys = { (Fixed32)(-4), (Fixed32)(-4), (Fixed32)(-6), (Fixed32)(-6) };
        for (int i = 0; i < 4; i++)
        {
            _distanceBody[i] = BodyFactory.CreateRectangle(World, (Fixed32)1.5, (Fixed32)1.5, (Fixed32)1);
            _distanceBody[i].BodyType = BodyType.Dynamic;
            _distanceBody[i].Friction = (Fixed32)0.7;
            _distanceBody[i].Position = new PhysicsVector2(xs[i], ys[i]);
        }

        var softDistance = new DistanceJoint(_distanceBody[0], _distanceBody[1], PhysicsVector2.Zero, PhysicsVector2.Zero);
        JointHelper.LinearStiffness((Fixed32)5, (Fixed32)0.3, softDistance.BodyA, softDistance.BodyB, out Fixed32 stiffness, out Fixed32 damping);
        softDistance.Damping = damping;
        softDistance.Stiffness = stiffness;
        World.AddJoint(softDistance);

        World.AddJoint(new DistanceJoint(_distanceBody[2], _distanceBody[3], PhysicsVector2.Zero, PhysicsVector2.Zero));
    }
}
