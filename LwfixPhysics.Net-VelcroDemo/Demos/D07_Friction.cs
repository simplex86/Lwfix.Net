using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Factories;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Demos;

using PhysicsVector2 = Vector2;

/// <summary>
/// Several boxes with varying friction sliding down ramps made of edge shapes.
/// Port of D07_Friction.
/// </summary>
public sealed class D07_Friction : DemoScreen
{
    public override string Name => "Friction";
    public override string Title => "Friction";
    public override string Details =>
        "This demo shows several bodies with varying friction.";
    public override string Controls =>
        "Mouse:\n" +
        "  - Grab object (beneath cursor): Left click\n" +
        "  - Drag grabbed object: Move mouse";

    private readonly Body[] _rectangle = new Body[5];
    private Body _ramps = null!;

    protected override void LoadContent()
    {
        World.Gravity = new PhysicsVector2(Fixed32.Zero, (Fixed32)20);

        _ramps = BodyFactory.CreateBody(World);
        FixtureFactory.AttachEdge(new PhysicsVector2((Fixed32)(-20), (Fixed32)(-11.2)), new PhysicsVector2((Fixed32)10, (Fixed32)(-3.8)), _ramps);
        FixtureFactory.AttachEdge(new PhysicsVector2((Fixed32)12, (Fixed32)(-5.6)), new PhysicsVector2((Fixed32)12, (Fixed32)(-3.2)), _ramps);

        FixtureFactory.AttachEdge(new PhysicsVector2((Fixed32)(-10), (Fixed32)4.4), new PhysicsVector2((Fixed32)20, (Fixed32)(-1.4)), _ramps);
        FixtureFactory.AttachEdge(new PhysicsVector2((Fixed32)(-12), (Fixed32)2.6), new PhysicsVector2((Fixed32)(-12), (Fixed32)5), _ramps);

        FixtureFactory.AttachEdge(new PhysicsVector2((Fixed32)(-20), (Fixed32)6.8), new PhysicsVector2((Fixed32)10, (Fixed32)11.5), _ramps);

        float[] friction = { 0.75f, 0.45f, 0.28f, 0.17f, 0.0f };
        for (int i = 0; i < 5; i++)
        {
            _rectangle[i] = BodyFactory.CreateRectangle(World, (Fixed32)1.5, (Fixed32)1.5, (Fixed32)1);
            _rectangle[i].BodyType = BodyType.Dynamic;
            _rectangle[i].Position = new PhysicsVector2((Fixed32)(-18 + 5.2 * i), (Fixed32)(-13.0 + 1.282 * i));
            _rectangle[i].Friction = (Fixed32)friction[i];
        }
    }
}
