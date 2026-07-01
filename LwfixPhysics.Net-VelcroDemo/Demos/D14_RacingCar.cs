using System.Collections.Generic;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Shapes;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints;
using SimplexLab.LwfixPhysics.Velcro.Factories;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;
using Silk.NET.Input;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Demos;

using PhysicsVector2 = Vector2;

/// <summary>
/// A side-scrolling racing car on a procedural track. The car uses two wheel
/// joints (suspension + motor). The track features a teeter board, a revolute
/// bridge, and stacked boxes. Port of D14_RacingCar.
/// </summary>
public sealed class D14_RacingCar : DemoScreen
{
    public override string Name => "Racing Car";
    public override string Title => "Racing car";
    public override string Details =>
        "This demo shows a side scrolling car on a race track.\n" +
        "The car uses two wheel joints, which combine a revolute and\n" +
        "a (soft) distance joint for the tire suspension.\n" +
        "The track is composed of several edge shapes and different\n" +
        "obstacles are attached to the track.";
    public override string Controls =>
        "Keyboard:\n" +
        "  - Accelerate / reverse: D / A\n" +
        "  - Break: S\n" +
        "  - Pan: Arrow keys | Zoom: PgUp/Dn";

    private const float HzFront = 8.5f;
    private const float HzBack = 5.0f;
    private const float Zeta = 0.85f;
    private const float MaxSpeed = 50.0f;

    private float _acceleration;
    private Body _ground = null!;
    private Body _car = null!;
    private WheelJoint _springBack = null!;
    private List<Body> _bridgeSegments = null!;

    public D14_RacingCar() => HasBorder = false;

    protected override void LoadContent()
    {
        World.Gravity = new PhysicsVector2(Fixed32.Zero, (Fixed32)10);

        // --- Terrain (chain of edges) ---
        _ground = BodyFactory.CreateBody(World);
        Vertices terrain = new Vertices();
        float[] pts =
        {
            -20,-5,  -20,0,  20,0,  25,-0.25f, 30,-1, 35,-4, 40,0, 45,0, 50,1, 55,2, 60,2, 65,1.25f,
            70,0, 75,-0.3f, 80,-1.5f, 85,-3.5f, 90,0, 95,0.5f, 100,1, 105,2, 110,2.5f, 115,1.3f, 120,0,
            160,0, 159,10, 201,10, 200,0, 240,0, 250,-5, 250,10, 270,10, 270,0, 310,0, 310,-5
        };
        for (int i = 0; i < pts.Length; i += 2)
            terrain.Add(new PhysicsVector2((Fixed32)pts[i], (Fixed32)pts[i + 1]));

        for (int i = 0; i < terrain.Count - 1; ++i)
            FixtureFactory.AttachEdge(terrain[i], terrain[i + 1], _ground);
        _ground.Friction = (Fixed32)0.6;

        // --- Teeter board ---
        Body board = BodyFactory.CreateBody(World);
        board.BodyType = BodyType.Dynamic;
        board.Position = new PhysicsVector2((Fixed32)140, (Fixed32)(-1));
        var teeterShape = new PolygonShape(PolygonUtils.CreateRectangle((Fixed32)10, (Fixed32)0.25), (Fixed32)1);
        board.AddFixture(teeterShape);

        var teeterAxis = JointFactory.CreateRevoluteJoint(World, _ground, board, PhysicsVector2.Zero);
        teeterAxis.LowerLimit = (Fixed32)(-8.0 * System.Math.PI / 180.0);
        teeterAxis.UpperLimit = (Fixed32)(8.0 * System.Math.PI / 180.0);
        teeterAxis.LimitEnabled = true;
        board.ApplyAngularImpulse((Fixed32)(-100));

        // --- Bridge (revolute chain) ---
        _bridgeSegments = new List<Body>();
        var segShape = new PolygonShape(PolygonUtils.CreateRectangle((Fixed32)1, (Fixed32)0.125), (Fixed32)1);
        Body prevBody = _ground;
        for (int i = 0; i < 20; ++i)
        {
            Body body = BodyFactory.CreateBody(World);
            body.BodyType = BodyType.Dynamic;
            body.Position = new PhysicsVector2((Fixed32)(161 + 2 * i), (Fixed32)0.125);
            Fixture fix = body.AddFixture(segShape);
            fix.Friction = (Fixed32)0.6;
            JointFactory.CreateRevoluteJoint(World, prevBody, body, -PhysicsVector2.UnitX);
            prevBody = body;
            _bridgeSegments.Add(body);
        }
        JointFactory.CreateRevoluteJoint(World, _ground, prevBody, PhysicsVector2.UnitX);

        // --- Stacked boxes ---
        var boxShape = new PolygonShape(PolygonUtils.CreateRectangle((Fixed32)0.5, (Fixed32)0.5), (Fixed32)1);
        for (int i = 0; i < 3; ++i)
        {
            Body body = BodyFactory.CreateBody(World);
            body.BodyType = BodyType.Dynamic;
            body.Position = new PhysicsVector2((Fixed32)220, (Fixed32)(-0.5 - i));
            body.AddFixture(boxShape);
        }

        // --- Car ---
        Vertices chassisVerts = new Vertices(8);
        chassisVerts.Add(new PhysicsVector2((Fixed32)(-2.5), (Fixed32)0.08));
        chassisVerts.Add(new PhysicsVector2((Fixed32)(-2.375), (Fixed32)(-0.46)));
        chassisVerts.Add(new PhysicsVector2((Fixed32)(-0.58), (Fixed32)(-0.92)));
        chassisVerts.Add(new PhysicsVector2((Fixed32)0.46, (Fixed32)(-0.92)));
        chassisVerts.Add(new PhysicsVector2((Fixed32)2.5, (Fixed32)(-0.17)));
        chassisVerts.Add(new PhysicsVector2((Fixed32)2.5, (Fixed32)0.205));
        chassisVerts.Add(new PhysicsVector2((Fixed32)2.3, (Fixed32)0.33));
        chassisVerts.Add(new PhysicsVector2((Fixed32)(-2.25), (Fixed32)0.35));
        var chassis = new PolygonShape(chassisVerts, (Fixed32)2);

        _car = BodyFactory.CreateBody(World);
        _car.BodyType = BodyType.Dynamic;
        _car.Position = new PhysicsVector2(Fixed32.Zero, (Fixed32)(-1));
        _car.AddFixture(chassis);

        Body wheelBack = BodyFactory.CreateBody(World);
        wheelBack.BodyType = BodyType.Dynamic;
        wheelBack.Position = new PhysicsVector2((Fixed32)(-1.709), (Fixed32)(-0.78));
        Fixture wbFix = wheelBack.AddFixture(new CircleShape((Fixed32)0.5, (Fixed32)0.8));
        wbFix.Friction = (Fixed32)0.9;

        Body wheelFront = BodyFactory.CreateBody(World);
        wheelFront.BodyType = BodyType.Dynamic;
        wheelFront.Position = new PhysicsVector2((Fixed32)1.54, (Fixed32)(-0.8));
        wheelFront.AddFixture(new CircleShape((Fixed32)0.5, (Fixed32)1));

        PhysicsVector2 axis = new PhysicsVector2(Fixed32.Zero, (Fixed32)(-1.2));
        _springBack = new WheelJoint(_car, wheelBack, wheelBack.Position, axis, true);
        _springBack.MotorSpeed = Fixed32.Zero;
        _springBack.MaxMotorTorque = (Fixed32)20;
        _springBack.MotorEnabled = true;
        JointHelper.LinearStiffness((Fixed32)HzBack, (Fixed32)Zeta, _springBack.BodyA, _springBack.BodyB, out Fixed32 stB, out Fixed32 dmpB);
        _springBack.Stiffness = stB;
        _springBack.Damping = dmpB;
        World.AddJoint(_springBack);

        var springFront = new WheelJoint(_car, wheelFront, wheelFront.Position, axis, true);
        springFront.MotorSpeed = Fixed32.Zero;
        springFront.MaxMotorTorque = (Fixed32)10;
        springFront.MotorEnabled = false;
        JointHelper.LinearStiffness((Fixed32)HzFront, (Fixed32)Zeta, springFront.BodyA, springFront.BodyB, out Fixed32 stF, out Fixed32 dmpF);
        springFront.Stiffness = stF;
        springFront.Damping = dmpF;
        World.AddJoint(springFront);

        // Camera follows the car.
        Camera.TrackingBody = _car;
        Camera.Zoom = (Fixed32)1.4;
    }

    protected override void HandleCustomInput(float dt)
    {
        if (Input.IsKeyDown(Key.D))
            _acceleration = System.Math.Min(_acceleration + 2f * dt, 1f);
        else if (Input.IsKeyDown(Key.A))
            _acceleration = System.Math.Max(_acceleration - 2f * dt, -1f);
        else if (Input.IsNewKeyPress(Key.S))
            _acceleration = 0f;
        else
            _acceleration -= System.Math.Sign(_acceleration) * 2f * dt;
    }

    public override void Update(float dt)
    {
        float target = System.Math.Sign(_acceleration) * SmoothStep(0f, MaxSpeed, System.Math.Abs(_acceleration));
        _springBack.MotorSpeed = (Fixed32)target;
        _springBack.MotorEnabled = System.Math.Abs(target) > MaxSpeed * 0.06f;
    }

    private static float SmoothStep(float min, float max, float x)
    {
        float t = System.Math.Clamp((x - min) / (max - min), 0f, 1f);
        return t * t * (3f - 2f * t) * max;
    }
}
