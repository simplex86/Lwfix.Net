using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Filtering;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Factories;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Prefabs;

using PhysicsVector2 = SimplexLab.LwfixPhysics.Velcro.Primitives.Vector2;

/// <summary>
/// Compound body (a central circle with four rectangular arms tipped by circles)
/// used as a user-controlled agent in several demos. Physics-only port of the
/// MonoGame sample's Agent; rendering is handled by the DebugView.
/// </summary>
public sealed class Agent
{
    public Body Body { get; }

    public Agent(World world, PhysicsVector2 position)
    {
        Body = BodyFactory.CreateBody(world, position);
        Body.BodyType = BodyType.Dynamic;

        // Center
        FixtureFactory.AttachCircle((Fixed32)0.5, (Fixed32)0.5, Body);

        // Left arm
        FixtureFactory.AttachRectangle((Fixed32)1.5, (Fixed32)0.4, (Fixed32)1, new PhysicsVector2(-(Fixed32)1, Fixed32.Zero), Body);
        FixtureFactory.AttachCircle((Fixed32)0.5, (Fixed32)0.5, Body, new PhysicsVector2(-(Fixed32)2, Fixed32.Zero));

        // Right arm
        FixtureFactory.AttachRectangle((Fixed32)1.5, (Fixed32)0.4, (Fixed32)1, new PhysicsVector2((Fixed32)1, Fixed32.Zero), Body);
        FixtureFactory.AttachCircle((Fixed32)0.5, (Fixed32)0.5, Body, new PhysicsVector2((Fixed32)2, Fixed32.Zero));

        // Top arm
        FixtureFactory.AttachRectangle((Fixed32)0.4, (Fixed32)1.5, (Fixed32)1, new PhysicsVector2(Fixed32.Zero, (Fixed32)1), Body);
        FixtureFactory.AttachCircle((Fixed32)0.5, (Fixed32)0.5, Body, new PhysicsVector2(Fixed32.Zero, (Fixed32)2));

        // Bottom arm
        FixtureFactory.AttachRectangle((Fixed32)0.4, (Fixed32)1.5, (Fixed32)1, new PhysicsVector2(Fixed32.Zero, -(Fixed32)1), Body);
        FixtureFactory.AttachCircle((Fixed32)0.5, (Fixed32)0.5, Body, new PhysicsVector2(Fixed32.Zero, -(Fixed32)2));

        Body.CollisionCategories = Category.All;
        Body.CollidesWith = Category.All;
    }
}
