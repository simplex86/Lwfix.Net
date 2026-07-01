using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Factories;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Demos;

using PhysicsVector2 = Vector2;

/// <summary>
/// A single dynamic body with one fixture. Demonstrates the basic body/fixture
/// pipeline plus keyboard-driven user agent (WASD/QE) and mouse grabbing.
/// Port of D01_SingleFixture from the MonoGame sample.
/// </summary>
public sealed class D01_SingleFixture : DemoScreen
{
    public override string Name => "Single Fixture";
    public override string Title => "Single body with a single fixture";
    public override string Details =>
        "This demo shows a single body with one attached fixture and shape.\n" +
        "A fixture binds a shape to a body and adds material properties such\n" +
        "as density, friction, and restitution.";
    public override string Controls =>
        "Keyboard:\n" +
        "  - Rotate object: Q, E\n" +
        "  - Move object: W, S, A, D\n" +
        "Mouse:\n" +
        "  - Grab object (beneath cursor): Left click\n" +
        "  - Drag grabbed object: Move mouse";

    private Body _rectangle = null!;

    protected override void LoadContent()
    {
        World.Gravity = PhysicsVector2.Zero;

        _rectangle = BodyFactory.CreateRectangle(World, (Fixed32)5, (Fixed32)5, (Fixed32)1);
        _rectangle.BodyType = BodyType.Dynamic;

        SetUserAgent(_rectangle, (Fixed32)100, (Fixed32)100);
    }
}
