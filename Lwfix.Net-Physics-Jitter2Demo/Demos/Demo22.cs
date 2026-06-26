using System;
using System.Collections.Generic;
using SimplexLab.Fixed.Physics.Collision.Shapes;
using SimplexLab.Fixed.Physics.Dynamics;
using SimplexLab.Fixed.Physics.JDemo.Renderer;
using SimplexLab.Fixed.Physics.LinearMath;

namespace SimplexLab.Fixed.Physics.JDemo;

public class Demo22 : IDemo, ICleanDemo, IDrawUpdate
{
    public string Name => "Conveyor Belt";
    public string Description => "Kinematic planks forming a conveyor belt that transports rigid bodies.";

    private Playground pg = null!;
    private World world = null!;

    // We need to track time manually for the physics steps
    // to ensure the belt moves perfectly in sync with the solver.
    private Real physicsTime = Real.Zero;

    private struct BeltPlank
    {
        public RigidBody Body;
        public Real DistanceOffset;
    }

    private readonly List<BeltPlank> planks = new();

    private static class Curve
    {
        public const float Speed = 2f;
        public const float StraightLength = 12.0f;
        public const float Radius = 6.0f;

        public static float TotalLength => (StraightLength * 2.0f) + (MathF.PI * Radius * 2.0f);

        public static void GetState(float distance, out JVector pos, out JVector vel, out float angVelY)
        {
            distance = distance % TotalLength;
            if (distance < 0) distance += TotalLength;

            float curveLen = MathF.PI * Radius;

            // 1. Bottom Straight
            if (distance < StraightLength)
            {
                float t = distance;
                pos = new JVector((Real)(-StraightLength * 0.5f + t), (Real)4.0f, (Real)(-Radius));
                vel = new JVector((Real)Speed, 0, 0);
                angVelY = 0;
            }
            // 2. Right Turn
            else if (distance < StraightLength + curveLen)
            {
                float t = distance - StraightLength;
                float angle = -MathF.PI * 0.5f + (t / Radius);
                pos = new JVector((Real)(StraightLength * 0.5f + MathF.Cos(angle) * Radius), (Real)4.0f, (Real)(MathF.Sin(angle) * Radius));
                vel = new JVector((Real)(-MathF.Sin(angle) * Speed), 0, (Real)(MathF.Cos(angle) * Speed));
                angVelY = Speed / Radius;
            }
            // 3. Top Straight
            else if (distance < (StraightLength * 2.0f) + curveLen)
            {
                float t = distance - (StraightLength + curveLen);
                pos = new JVector((Real)(StraightLength * 0.5f - t), (Real)4.0f, (Real)Radius);
                vel = new JVector((Real)(-Speed), 0, 0);
                angVelY = 0;
            }
            // 4. Left Turn
            else
            {
                float t = distance - ((StraightLength * 2.0f) + curveLen);
                float angle = MathF.PI * 0.5f + (t / Radius);
                pos = new JVector((Real)(-StraightLength * 0.5f + MathF.Cos(angle) * Radius), (Real)4.0f, (Real)(MathF.Sin(angle) * Radius));
                vel = new JVector((Real)(-MathF.Sin(angle) * Speed), 0, (Real)(MathF.Cos(angle) * Speed));
                angVelY = Speed / Radius;
            }
        }
    }

    public void Build(Playground pg, World world)
    {
        this.pg = pg;
        this.world = world;
        pg.AddFloor();
        planks.Clear();
        physicsTime = Real.Zero;

        // Subscribe to the physics step event
        world.PreSubStep += OnPreStep;

        float plankWidth = 0.6f;
        int plankCount = (int)(Curve.TotalLength / plankWidth);
        float distStep = Curve.TotalLength / plankCount;

        for (int i = 0; i < plankCount; i++)
        {
            var body = world.CreateRigidBody();
            body.AddShape(new BoxShape((Real)1.8, (Real)0.1, (Real)0.55));
            body.MotionType = MotionType.Kinematic;
            body.Friction = (Real)1.0;

            float dist = i * distStep;
            Curve.GetState(dist, out JVector pos, out JVector vel, out float w);

            body.Position = pos;

            // Align initial orientation
            JVector fwd = JVector.Normalize(vel);
            JVector up = JVector.UnitY;
            JVector right = JVector.Normalize(JVector.Cross(up, fwd));
            up = JVector.Cross(fwd, right);
            JMatrix ori = JMatrix.FromColumns(right, up, fwd);
            body.Orientation = JQuaternion.CreateFromMatrix(ori);

            planks.Add(new BeltPlank { Body = body, DistanceOffset = (Real)dist });
        }

        // Debris
        for (int i = 0; i < 8; i++)
        {
            var box = world.CreateRigidBody();
            box.AddShape(new BoxShape((Real)1));
            box.Position = new JVector((Real)(-5 + i * 1.5), (Real)6, (Real)(-Curve.Radius));
        }

        var floor = world.CreateRigidBody();
        floor.MotionType = MotionType.Static;
        floor.AddShape(new BoxShape((Real)100, (Real)1, (Real)100), MassInertiaUpdateMode.Preserve);
        floor.Position = new JVector(0, (Real)(-5), 0);
    }

    // Called automatically by Jitter before every physics sub-step
    private void OnPreStep(Real dt)
    {
        physicsTime += dt;
        float globalDist = (float)physicsTime * Curve.Speed;

        foreach (var plank in planks)
        {
            float d = globalDist + (float)plank.DistanceOffset;

            Curve.GetState(d, out JVector targetPos, out JVector targetVel, out float targetAngVelY);

            // Motion Control Logic - Feed-Forward + Proportional Controller.
            plank.Body.Velocity = targetVel + (targetPos - plank.Body.Position) * (Real)10.0;

            JVector currentForward = plank.Body.Orientation.GetBasisZ();
            JVector targetForward = JVector.Normalize(targetVel);

            // Calculate angle sine error (Cross Product Y-component)
            Real angleError = (currentForward.Z * targetForward.X - currentForward.X * targetForward.Z);

            Real correction = angleError * (Real)20.0;
            plank.Body.AngularVelocity = new JVector(0, (Real)targetAngVelY + correction, 0);
        }
    }

    public void DrawUpdate()
    {
        // Only drawing logic remains here
        const int stepMax = 200;
        float totalLen = Curve.TotalLength;

        for (int step = 0; step < stepMax; step++)
        {
            float d1 = totalLen / stepMax * step;
            float d2 = totalLen / stepMax * (step + 1);
            Curve.GetState(d1, out JVector p1, out _, out _);
            Curve.GetState(d2, out JVector p2, out _, out _);
            pg.DebugRenderer.PushLine(DebugRenderer.Color.Green, Conversion.FromJitter(p1), Conversion.FromJitter(p2));
        }
    }

    // Important: Unsubscribe when the demo is switched to avoid memory leaks
    // or ghost logic running in the next demo.
    public void CleanUp()
    {
        if (world != null)
        {
            world.PreSubStep -= OnPreStep;
        }
    }
}
