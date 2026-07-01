using System.Collections.Generic;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Shapes;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Tools.PolygonManipulation;
using SimplexLab.LwfixPhysics.Velcro.Tools.Triangulation.TriangulationBase;
using Silk.NET.Input;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Demos;

using PhysicsVector2 = SimplexLab.LwfixPhysics.Velcro.Primitives.Vector2;

/// <summary>
/// Three procedural "cookies" (concave polygons) decomposed with Bayazit into
/// convex parts, attached to a <see cref="BreakableBody"/>. Right-click triggers
/// an explosion that impulses nearby bodies away from the cursor; strong enough
/// impacts shatter a cookie into its constituent convex pieces.
/// Port of D16_BreakableBody. The original loaded the cookie outline from an SVG
/// pipeline asset; we substitute a procedurally-generated star-flower polygon so
/// the demo remains self-contained.
/// </summary>
public sealed class D16_BreakableBody : DemoScreen
{
    public override string Name => "Breakable Body";
    public override string Title => "Breakable body and explosions";
    public override string Details =>
        "This demo shows a breakable cookie. The outline is a procedural\n" +
        "star-flower polygon decomposed into convex parts with Bayazit.\n" +
        "Strong impacts exceed the body's strength and shatter it into its\n" +
        "constituent pieces, each of which then behaves as an independent body.";
    public override string Controls =>
        "Mouse:\n" +
        "  - Explosion (at cursor): Right click\n" +
        "  - Grab object (beneath cursor): Left click\n" +
        "  - Drag grabbed object: Move mouse\n" +
        "Keyboard:\n" +
        "  - Pan: Arrow keys | Zoom: PgUp/Dn";

    private const int CookieCount = 3;
    private const double CookieRadius = 5.0;
    private const int CookieSpikes = 7;
    private const double Strength = 120.0;
    private const double ExplosionRadius = 10.0;
    private const double ExplosionImpulse = 80.0;

    private readonly BreakableBody[] _cookies = new BreakableBody[CookieCount];

    protected override void LoadContent()
    {
        World.Gravity = PhysicsVector2.Zero;

        for (int i = 0; i < CookieCount; i++)
        {
            Vertices outline = MakeCookieOutline(CookieRadius, CookieSpikes, seed: i * 17 + 3);

            // Mirror the original pipeline: simplify, then convex-decompose.
            Vertices simplified = SimplifyTools.DouglasPeuckerSimplify(outline, (Fixed32)0.1);
            List<Vertices> parts = Triangulate.ConvexPartition(simplified, TriangulationAlgorithm.Bayazit);

            List<PolygonShape> polygons = new List<PolygonShape>(parts.Count);
            foreach (Vertices v in parts)
                polygons.Add(new PolygonShape(v, (Fixed32)1));

            _cookies[i] = new BreakableBody(World, polygons);
            _cookies[i].Strength = (Fixed32)Strength;
            _cookies[i].MainBody.Position = new PhysicsVector2(
                (Fixed32)(-20.33 + 15.0 * i),
                (Fixed32)(-5.33));
            World.AddBreakableBody(_cookies[i]);
        }
    }

    /// <summary>
    /// Build a closed star-flower polygon: alternates between an outer and inner
    /// radius around a circle so the resulting outline is concave and the Bayazit
    /// decomposer actually has work to do. A small per-vertex jitter (derived from
    /// the seed) makes each cookie look slightly different.
    /// </summary>
    private static Vertices MakeCookieOutline(double radius, int spikes, int seed)
    {
        Vertices v = new Vertices(spikes * 2);
        double inner = radius * 0.55;
        for (int i = 0; i < spikes * 2; i++)
        {
            double ang = System.Math.PI * i / spikes;
            double r = (i % 2 == 0) ? radius : inner;
            // Deterministic jitter so the three cookies differ slightly.
            double jitter = 0.08 * System.Math.Sin(seed * 12.9898 + i * 78.233);
            r += r * jitter;
            v.Add(new PhysicsVector2((Fixed32)(r * System.Math.Cos(ang)),
                                     (Fixed32)(r * System.Math.Sin(ang))));
        }
        return v;
    }

    protected override void HandleCustomInput(float dt)
    {
        if (Input.IsNewRightMousePress)
        {
            PhysicsVector2 cursor = Camera.ConvertScreenToWorld(Input.MouseX, Input.MouseY);
            PhysicsVector2 min = new PhysicsVector2(cursor.X - (Fixed32)ExplosionRadius,
                                                    cursor.Y - (Fixed32)ExplosionRadius);
            PhysicsVector2 max = new PhysicsVector2(cursor.X + (Fixed32)ExplosionRadius,
                                                    cursor.Y + (Fixed32)ExplosionRadius);
            AABB aabb = new AABB(ref min, ref max);

            World.QueryAABB(fixture =>
            {
                PhysicsVector2 dir = fixture.Body.Position - cursor;
                Fixed32 len = dir.Length();
                if (len > (Fixed32)0.001)
                {
                    dir = dir * ((Fixed32)ExplosionImpulse / len);
                }
                else
                {
                    dir = new PhysicsVector2((Fixed32)ExplosionImpulse, Fixed32.Zero);
                }
                PhysicsVector2 impulse = dir;
                fixture.Body.ApplyLinearImpulse(ref impulse);
                return true;
            }, ref aabb);
        }
    }
}
