using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Prefabs;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Demos;

using PhysicsVector2 = Vector2;

/// <summary>
/// A pyramid of stacked boxes plus a user-controlled agent. Stress-tests the
/// solver's stacking stability. Port of D04_StackedBodies.
/// </summary>
public sealed class D04_StackedBodies : DemoScreen
{
    private const int PyramidBaseBodyCount = 14;

    public override string Name => "Stacked Bodies";
    public override string Title => "Stacked bodies";
    public override string Details =>
        "This demo shows the stacking stability.\n" +
        "It shows a bunch of rectangular bodies stacked in the shape of a pyramid.";
    public override string Controls =>
        "Keyboard:\n" +
        "  - Rotate object: Q, E\n" +
        "  - Move object: W, S, A, D\n" +
        "Mouse:\n" +
        "  - Grab object (beneath cursor): Left click\n" +
        "  - Drag grabbed object: Move mouse";

    private Agent _agent = null!;
    private Pyramid _pyramid = null!;

    protected override void LoadContent()
    {
        World.Gravity = new PhysicsVector2(Fixed32.Zero, (Fixed32)20);

        _agent = new Agent(World, new PhysicsVector2((Fixed32)5, (Fixed32)(-10)));
        _pyramid = new Pyramid(World, new PhysicsVector2(Fixed32.Zero, (Fixed32)15), PyramidBaseBodyCount, (Fixed32)1);

        SetUserAgent(_agent.Body, (Fixed32)1000, (Fixed32)400);
    }
}
