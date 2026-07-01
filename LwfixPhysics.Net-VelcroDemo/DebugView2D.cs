using System.Numerics;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.ContactSystem;
using SimplexLab.LwfixPhysics.Velcro.Collision.Shapes;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints;
using SimplexLab.LwfixPhysics.Velcro.Extensions.DebugView;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;
using SimplexLab.LwfixPhysics.VelcroDemo.Renderer;
using SimplexLab.LwfixPhysics.VelcroDemo.Renderer.OpenGL;
using Color = SimplexLab.LwfixPhysics.Velcro.Primitives.Color;

namespace SimplexLab.LwfixPhysics.VelcroDemo;

using PhysicsVector2 = SimplexLab.LwfixPhysics.Velcro.Primitives.Vector2;

/// <summary>
/// Concrete Silk.NET implementation of the engine's <see cref="DebugViewBase"/>.
/// Iterates the world each frame and streams line/triangle primitives to
/// <see cref="PrimitiveBatch"/> according to the active <see cref="DebugViewFlags"/>.
/// Replaces the MonoGame DebugView from the original sample.
/// </summary>
public sealed class DebugView2D : DebugViewBase
{
    private readonly PrimitiveBatch _batch;

    // Default colors mirror the original MonoGame DebugView defaults.
    // Color is internal in the engine; the properties must match its accessibility.
    internal Color DefaultShapeColor { get; set; } = new Color(255, 255, 255, 200);
    internal Color SleepingShapeColor { get; set; } = new Color(190, 190, 190, 200);
    internal Color StaticShapeColor { get; set; } = new Color(140, 140, 140, 200);
    internal Color KinematicShapeColor { get; set; } = new Color(110, 190, 230, 200);

    private static readonly Color _aabbColor = new Color(255, 230, 90, 255);
    private static readonly Color _jointColor = new Color(80, 255, 80, 255);
    private static readonly Color _contactPoint = new Color(255, 90, 90, 255);
    private static readonly Color _contactNormal = new Color(90, 200, 255, 255);
    private static readonly Color _comColor = new Color(255, 80, 255, 255);
    private static readonly Color _polyPoint = new Color(255, 90, 255, 255);
    private static readonly Color _xAxis = new Color(255, 60, 60, 255);
    private static readonly Color _yAxis = new Color(60, 255, 60, 255);

    public DebugView2D(World world, PrimitiveBatch batch) : base(world)
    {
        _batch = batch;
        Flags = DebugViewFlags.Shape | DebugViewFlags.Joint | DebugViewFlags.ContactPoints;
    }

    public void RenderDebugData(in Matrix4x4 projection)
    {
        _batch.Begin(in projection);

        if ((Flags & DebugViewFlags.Shape) != 0)
            DrawShapes();

        if ((Flags & DebugViewFlags.Joint) != 0)
            DrawJoints();

        if ((Flags & DebugViewFlags.AABB) != 0)
            DrawAABBs();

        if ((Flags & DebugViewFlags.ContactPoints) != 0 ||
            (Flags & DebugViewFlags.ContactNormals) != 0)
            DrawContacts();

        if ((Flags & DebugViewFlags.CenterOfMass) != 0)
            DrawCentersOfMass();

        if ((Flags & DebugViewFlags.PolygonPoints) != 0)
            DrawPolygonPoints();

        if ((Flags & DebugViewFlags.DebugPanel) != 0)
        { /* profile panel is handled in ImGui by Playground */ }

        _batch.End();
    }

    // ---------------------------------------------------------------------
    // World iteration
    // ---------------------------------------------------------------------
    private void DrawShapes()
    {
        foreach (Body b in World.BodyList)
        {
            if (!b.Enabled) continue;

            Color color;
            if (!b.Awake && b.BodyType != BodyType.Static && World.SleepingAllowed)
                color = SleepingShapeColor;
            else
                color = b.BodyType switch
                {
                    BodyType.Static => StaticShapeColor,
                    BodyType.Kinematic => KinematicShapeColor,
                    _ => DefaultShapeColor
                };

            b.GetTransform(out Transform xf);

            foreach (Fixture f in b.FixtureList)
            {
                if (f == null) continue;
                DrawShape(f.Shape, ref xf, color);
            }
        }
    }

    private void DrawShape(Shape shape, ref Transform xf, Color color)
    {
        switch (shape)
        {
            case PolygonShape poly:
                DrawPolygonShape(poly, ref xf, color);
                break;
            case CircleShape circle:
                {
                    PhysicsVector2 center = MathUtils.Mul(ref xf, circle.Position);
                    DrawSolidCircle(center, circle.Radius, xf.q.GetXAxis(), color);
                }
                break;
            case EdgeShape edge:
                {
                    PhysicsVector2 p1 = MathUtils.Mul(ref xf, edge.Vertex1);
                    PhysicsVector2 p2 = MathUtils.Mul(ref xf, edge.Vertex2);
                    DrawSegment(p1, p2, color);
                    if (edge.OneSided)
                    {
                        PhysicsVector2 p0 = MathUtils.Mul(ref xf, edge.Vertex0);
                        PhysicsVector2 p3 = MathUtils.Mul(ref xf, edge.Vertex3);
                        DrawSegment(p0, p1, color);
                        DrawSegment(p2, p3, color);
                    }
                }
                break;
            case ChainShape chain:
                {
                    int count = chain.Vertices.Count;
                    PhysicsVector2 v1 = MathUtils.Mul(ref xf, chain.Vertices[0]);
                    for (int i = 1; i < count; ++i)
                    {
                        PhysicsVector2 v2 = MathUtils.Mul(ref xf, chain.Vertices[i]);
                        DrawSegment(v1, v2, color);
                        v1 = v2;
                    }
                }
                break;
        }
    }

    private void DrawPolygonShape(PolygonShape poly, ref Transform xf, Color color)
    {
        int count = poly.Vertices.Count;
        if (count < 2) return;

        PhysicsVector2[] world = new PhysicsVector2[count];
        for (int i = 0; i < count; ++i)
            world[i] = MathUtils.Mul(ref xf, poly.Vertices[i]);

        // Convert to engine Vector2 array expected by the base API.
        DrawSolidPolygon(world, count, color, true);
    }

    private void DrawJoints()
    {
        foreach (Joint j in World.JointList)
            DrawJoint(j);
    }

    private void DrawJoint(Joint joint)
    {
        PhysicsVector2 a = joint.WorldAnchorA;
        PhysicsVector2 b = joint.WorldAnchorB;
        Body ba = joint.BodyA;
        Body bb = joint.BodyB;

        PhysicsVector2 anchorA = ba != null ? a : a;
        PhysicsVector2 anchorB = bb != null ? b : b;

        // Connect the two anchors with a segment; draw the anchors as small crosses.
        DrawSegment(anchorA, anchorB, _jointColor);
        DrawCross(anchorA, (Fixed32)0.3, _jointColor);
        if (bb != null)
            DrawCross(anchorB, (Fixed32)0.3, _jointColor);
    }

    private void DrawAABBs()
    {
        foreach (Body b in World.BodyList)
        {
            if (!b.Enabled) continue;
            foreach (Fixture f in b.FixtureList)
            {
                if (f == null) continue;
                for (int i = 0; i < f.ProxyCount; ++i)
                {
                    AABB aabb = f.Proxies[i].AABB;
                    DrawAABB(ref aabb, _aabbColor);
                }
            }
        }
    }

    private void DrawContacts()
    {
        bool drawPoints = (Flags & DebugViewFlags.ContactPoints) != 0;
        bool drawNormals = (Flags & DebugViewFlags.ContactNormals) != 0;

        for (Contact? c = World.ContactManager._contactList; c != null; c = c.Next)
        {
            if (!c.IsTouching || !c.Enabled) continue;

            c.GetWorldManifold(out PhysicsVector2 normal, out var points);

            if (drawNormals)
            {
                PhysicsVector2 p1 = points[0];
                PhysicsVector2 p2 = p1 + normal * (Fixed32)0.5;
                DrawSegment(p1, p2, _contactNormal);
            }

            if (drawPoints)
            {
                for (int i = 0; i < 2 && !points[i].Equals(PhysicsVector2.Zero); ++i)
                    DrawPoint(points[i], (Fixed32)0.08, _contactPoint);
            }
        }
    }

    private void DrawCentersOfMass()
    {
        foreach (Body b in World.BodyList)
        {
            if (!b.Enabled) continue;
            DrawPoint(b.WorldCenter, (Fixed32)0.1, _comColor);
        }
    }

    private void DrawPolygonPoints()
    {
        foreach (Body b in World.BodyList)
        {
            if (!b.Enabled) continue;
            b.GetTransform(out Transform xf);
            foreach (Fixture f in b.FixtureList)
            {
                if (f?.Shape is PolygonShape poly)
                {
                    for (int i = 0; i < poly.Vertices.Count; ++i)
                    {
                        PhysicsVector2 p = MathUtils.Mul(ref xf, poly.Vertices[i]);
                        DrawPoint(p, (Fixed32)0.06, _polyPoint);
                    }
                }
            }
        }
    }

    private void DrawCross(PhysicsVector2 p, Fixed32 size, Color color)
    {
        PhysicsVector2 sx = new PhysicsVector2(size, Fixed32.Zero);
        PhysicsVector2 sy = new PhysicsVector2(Fixed32.Zero, size);
        DrawSegment(p - sx, p + sx, color);
        DrawSegment(p - sy, p + sy, color);
    }

    private void DrawPoint(PhysicsVector2 p, Fixed32 size, Color color)
    {
        PhysicsVector2 sx = new PhysicsVector2(size, Fixed32.Zero);
        PhysicsVector2 sy = new PhysicsVector2(Fixed32.Zero, size);
        DrawSegment(p - sx, p + sx, color);
        DrawSegment(p - sy, p + sy, color);
    }

    private void DrawAABB(ref AABB aabb, Color color)
    {
        PhysicsVector2 lb = aabb.LowerBound;
        PhysicsVector2 ub = aabb.UpperBound;
        PhysicsVector2[] v =
        {
            new(lb.X, lb.Y),
            new(ub.X, lb.Y),
            new(ub.X, ub.Y),
            new(lb.X, ub.Y)
        };
        DrawPolygon(v, 4, color, true);
    }

    public override void DrawTransform(ref Transform transform)
    {
        PhysicsVector2 p = transform.p;
        PhysicsVector2 xAxis = p + transform.q.GetXAxis() * (Fixed32)0.4;
        PhysicsVector2 yAxis = p + transform.q.GetYAxis() * (Fixed32)0.4;
        DrawSegment(p, xAxis, _xAxis);
        DrawSegment(p, yAxis, _yAxis);
    }

    // ---------------------------------------------------------------------
    // DebugViewBase overrides (engine calls these; we forward to the batch).
    // ---------------------------------------------------------------------
    internal override void DrawPolygon(PhysicsVector2[] vertices, int count, Color color, bool closed = true)
    {
        var c = ToVec4(color);
        for (int i = 0; i < count - 1; ++i)
            _batch.AddLine((float)vertices[i].X, (float)vertices[i].Y,
                           (float)vertices[i + 1].X, (float)vertices[i + 1].Y, c);
        if (closed && count > 1)
            _batch.AddLine((float)vertices[count - 1].X, (float)vertices[count - 1].Y,
                           (float)vertices[0].X, (float)vertices[0].Y, c);
    }

    internal override void DrawSolidPolygon(PhysicsVector2[] vertices, int count, Color color, bool outline = true)
    {
        var c = ToVec4(color);
        // Fan triangulation (polygons are convex CCW).
        for (int i = 1; i < count - 1; ++i)
        {
            _batch.AddTriangle((float)vertices[0].X, (float)vertices[0].Y,
                               (float)vertices[i].X, (float)vertices[i].Y,
                               (float)vertices[i + 1].X, (float)vertices[i + 1].Y, c);
        }
        if (outline) DrawPolygon(vertices, count, new Color(color.R, color.G, color.B, (byte)255));
    }

    internal override void DrawCircle(PhysicsVector2 center, Fixed32 radius, Color color)
    {
        var c = ToVec4(color);
        const int segments = 32;
        double twoPi = Math.PI * 2.0;
        float r = (float)radius;
        float cx = (float)center.X, cy = (float)center.Y;
        float px = cx + r, py = cy;
        for (int i = 1; i <= segments; ++i)
        {
            double a = twoPi * i / segments;
            float nx = cx + r * (float)Math.Cos(a);
            float ny = cy + r * (float)Math.Sin(a);
            _batch.AddLine(px, py, nx, ny, c);
            px = nx; py = ny;
        }
    }

    internal override void DrawSolidCircle(PhysicsVector2 center, Fixed32 radius, PhysicsVector2 axis, Color color)
    {
        var c = ToVec4(color);
        const int segments = 32;
        double twoPi = Math.PI * 2.0;
        float r = (float)radius;
        float cx = (float)center.X, cy = (float)center.Y;

        // Triangle fan from center.
        float px = cx + r, py = cy;
        for (int i = 1; i <= segments; ++i)
        {
            double a = twoPi * i / segments;
            float nx = cx + r * (float)Math.Cos(a);
            float ny = cy + r * (float)Math.Sin(a);
            _batch.AddTriangle(cx, cy, px, py, nx, ny, c);
            px = nx; py = ny;
        }

        // Outline
        DrawCircle(center, radius, new Color(color.R, color.G, color.B, (byte)255));
        // Axis indicator
        PhysicsVector2 end = center + axis * radius;
        DrawSegment(center, end, new Color(color.R, color.G, color.B, (byte)255));
    }

    internal override void DrawSegment(PhysicsVector2 start, PhysicsVector2 end, Color color)
    {
        var c = ToVec4(color);
        _batch.AddLine((float)start.X, (float)start.Y, (float)end.X, (float)end.Y, c);
    }

    private static Vector4 ToVec4(Color c) =>
        new(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
}
