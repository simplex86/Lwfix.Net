using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Filtering;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Factories;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.VelcroDemo.Renderer;

namespace SimplexLab.LwfixPhysics.Velcro.Dynamics.Prefabs;

using PhysicsVector2 = SimplexLab.LwfixPhysics.Velcro.Primitives.Vector2;

/// <summary>
/// Builds an invisible loop of static edges around the visible camera area so
/// bodies stay on screen. Rendering is handled by the DebugView (edge shapes).
/// Replaces the MonoGame sample's Border (which drew a textured frame).
/// </summary>
public sealed class Border
{
    public Body Anchor { get; }

    public Border(World world, Camera2D camera)
    {
        // Compute the visible simulation area from the camera at construction time.
        PhysicsVector2 lowerLeft = camera.ConvertScreenToWorld(0, 0);
        PhysicsVector2 upperRight = camera.ConvertScreenToWorld(camera.Width, camera.Height);

        Fixed32 halfWidth = (upperRight.X - lowerLeft.X) * (Fixed32)0.5 - (Fixed32)0.75;
        Fixed32 halfHeight = (upperRight.Y - lowerLeft.Y) * (Fixed32)0.5 - (Fixed32)0.75;

        Vertices borders = new Vertices(4);
        borders.Add(new PhysicsVector2(-halfWidth, halfHeight));   // lower left  (sim +y is up)
        borders.Add(new PhysicsVector2(halfWidth, halfHeight));    // lower right
        borders.Add(new PhysicsVector2(halfWidth, -halfHeight));   // upper right
        borders.Add(new PhysicsVector2(-halfWidth, -halfHeight));  // upper left

        Anchor = BodyFactory.CreateLoopShape(world, borders);
        Anchor.CollisionCategories = Category.All;
        Anchor.CollidesWith = Category.All;
    }
}
