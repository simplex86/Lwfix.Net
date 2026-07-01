using System.Collections.Generic;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Shapes;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Factories;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Prefabs;

using PhysicsVector2 = SimplexLab.LwfixPhysics.Velcro.Primitives.Vector2;

/// <summary>
/// Stacks a pyramid of square dynamic bodies. Used to stress-test the solver's
/// stacking stability. Physics-only port of the MonoGame sample's Pyramid.
/// </summary>
public sealed class Pyramid
{
    private readonly List<Body> _boxes = new();

    public IReadOnlyList<Body> Boxes => _boxes;

    public Pyramid(World world, PhysicsVector2 position, int count, Fixed32 density)
    {
        Vertices rect = PolygonUtils.CreateRectangle((Fixed32)0.5, (Fixed32)0.5);
        var shape = new PolygonShape(rect, density);

        PhysicsVector2 rowStart = position;
        rowStart = new PhysicsVector2(rowStart.X, rowStart.Y - ((Fixed32)0.5 + (Fixed32)count * (Fixed32)1.1));

        PhysicsVector2 deltaRow = new PhysicsVector2(-(Fixed32)0.625, (Fixed32)1.1);
        const float Spacing = 1.25f;

        for (int i = 0; i < count; i++)
        {
            PhysicsVector2 pos = rowStart;
            for (int j = 0; j < i + 1; j++)
            {
                Body body = BodyFactory.CreateBody(world);
                body.BodyType = BodyType.Dynamic;
                body.Position = pos;
                body.AddFixture(shape);
                _boxes.Add(body);
                pos = new PhysicsVector2(pos.X + (Fixed32)Spacing, pos.Y);
            }
            rowStart += deltaRow;
        }
    }
}
