using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Prefabs;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Demos;

using PhysicsVector2 = Vector2;

/// <summary>
/// A user-controlled Agent plus a stack of jumpy spiders (revolute joints + angle
/// joints with a dynamic target angle). Port of D09_DynamicJoints.
/// </summary>
public sealed class D09_DynamicJoints : DemoScreen
{
    public override string Name => "Dynamic Joints";
    public override string Title => "Revolute & dynamic angle joints";
    public override string Details =>
        "This demo demonstrates the use of revolute joints combined\n" +
        "with angle joints that have a dynamic target angle.";
    public override string Controls =>
        "Keyboard:\n" +
        "  - Rotate object: Q, E\n" +
        "  - Move object: W, S, A, D\n" +
        "Mouse:\n" +
        "  - Grab object (beneath cursor): Left click\n" +
        "  - Drag grabbed object: Move mouse";

    private Agent _agent = null!;
    private JumpySpider[] _spiders = null!;

    protected override void LoadContent()
    {
        World.Gravity = new PhysicsVector2(Fixed32.Zero, (Fixed32)20);

        _agent = new Agent(World, new PhysicsVector2(Fixed32.Zero, (Fixed32)10));
        _spiders = new JumpySpider[8];
        for (int i = 0; i < _spiders.Length; i++)
            _spiders[i] = new JumpySpider(World, new PhysicsVector2(Fixed32.Zero, (Fixed32)(8 - (i + 1) * 2)));

        SetUserAgent(_agent.Body, (Fixed32)1000, (Fixed32)400);
    }

    public override void Update(float dt)
    {
        float ms = dt * 1000f;
        for (int i = 0; i < _spiders.Length; i++)
            _spiders[i].Update(ms);
    }
}
