using System;
using System.Collections.Generic;
using SimplexLab.Fixed.Physics.Collision;
using SimplexLab.Fixed.Physics.Collision.Shapes;
using SimplexLab.Fixed.Physics.Dynamics;
using SimplexLab.Fixed.Physics.JDemo.Renderer;
using SimplexLab.Fixed.Physics.LinearMath;

namespace SimplexLab.Fixed.Physics.JDemo;

public class RigidBodyTag(bool doNotDraw = true)
{
    public bool DoNotDraw { get; set; } = doNotDraw;
}

// Built-in primitive drawables. Subclassing is the simplest way to key them in
// the RenderWindow's Type-based registry and to configure per-shape material bits.
public sealed class CubeDrawable() : InstancedDrawable(Mesh.Cube());
public sealed class SphereDrawable() : InstancedDrawable(Mesh.Sphere());
public sealed class ConeDrawable() : InstancedDrawable(Mesh.Cone());
public sealed class CylinderDrawable() : InstancedDrawable(Mesh.Cylinder());
public sealed class HalfSphereDrawable() : InstancedDrawable(Mesh.HalfSphere());

public sealed class FloorDrawable : InstancedDrawable
{
    public FloorDrawable() : base(Mesh.Quad(halfSize: 100, uvScale: 100))
    {
        Material = new Material
        {
            Tint = new Vector3(0.6f, 0.6f, 0.6f),
            Specular = new Vector3(0.1f, 0.1f, 0.1f),
            Shininess = 10f,
            Alpha = 1f,
            VertexColorWeight = 0f,
            TextureWeight = 0f,
            Texture = null
        };
    }
}

public partial class Playground : RenderWindow
{
    private readonly World world;

    private static readonly Real PhysicsTimestep = (Real)0.01;
    private bool multiThread = true;
    private RigidBodyShape? floorShape;

    private CubeDrawable cubes = null!;
    private SphereDrawable spheres = null!;
    private ConeDrawable cones = null!;
    private CylinderDrawable cylinders = null!;
    private HalfSphereDrawable halfSpheres = null!;
    private FloorDrawable floor = null!;

    public CubeDrawable Cubes => cubes;
    public SphereDrawable Spheres => spheres;
    public ConeDrawable Cones => cones;
    public CylinderDrawable Cylinders => cylinders;
    public HalfSphereDrawable HalfSpheres => halfSpheres;
    public FloorDrawable Floor => floor;

    private readonly List<IDemo> demos = new();

    private IDemo? currentDemo;

    public int SelectedDemoIndex { get; private set; } = -1;

    public IReadOnlyList<IDemo> Demos => demos;

    private void SwitchDemo(int index)
    {
        (currentDemo as ICleanDemo)?.CleanUp();
        ResetScene();
        SelectedDemoIndex = index;
        currentDemo = demos[index];
        currentDemo.Build(this, world);
    }

    public void RegisterDemo(IDemo demo) => demos.Add(demo);

    public void SelectDemo(int index)
    {
        if (index >= 0 && index < demos.Count) SwitchDemo(index);
    }

    public Playground(Silk.NET.Windowing.IWindow window) : base(window)
    {
        world = new World();
        world.NullBody.Tag = new RigidBodyTag();
        drawBox = DrawBox;
    }

    private void ResetScene()
    {
        floorShape = null;
        world.Clear();
        world.DynamicTree.Filter = World.DefaultDynamicTreeFilter;
        world.BroadPhaseFilter = null;
        world.NarrowPhaseFilter = new TriangleEdgeCollisionFilter();
        world.Gravity = new JVector(0, (Real)(-9.81), 0);
        world.SubstepCount = 1;
        world.SolverIterations = (8, 4);
    }

    public void AddFloor()
    {
        RigidBody body = World.CreateRigidBody();
        floorShape = new BoxShape((Real)200, (Real)200, (Real)200);
        body.Position = new JVector(0, (Real)(-100), 0);
        body.MotionType = MotionType.Static;
        // The 200³ floor box has an inertia tensor (~53 billion) that overflows
        // Fixed32 (Q32.32, max integer ~2.1 billion). Static bodies don't need
        // mass/inertia, so skip the computation.
        body.AddShape(floorShape, MassInertiaUpdateMode.Preserve);
    }

    public override void Load()
    {
        base.Load();

        cubes = GetDrawable<CubeDrawable>();
        spheres = GetDrawable<SphereDrawable>();
        cones = GetDrawable<ConeDrawable>();
        cylinders = GetDrawable<CylinderDrawable>();
        halfSpheres = GetDrawable<HalfSphereDrawable>();
        floor = GetDrawable<FloorDrawable>();

        VerticalSync = false;

        // Auto-select the first demo so the scene is populated on startup.
        // SwitchDemo takes care of ResetScene + AddFloor + Build.
        if (demos.Count > 0) SelectDemo(0);
        else { ResetScene(); AddFloor(); }
    }

    public RigidBodyShape? FloorShape => floorShape;

    public World World => world;

    public void ShootPrimitive()
    {
        const float primitiveVelocity = 20f;

        var pos = Camera.Position;
        var dir = Camera.Direction;

        var sb = World.CreateRigidBody();
        sb.Position = Conversion.ToJitterVector(pos);
        sb.Velocity = Conversion.ToJitterVector(dir * primitiveVelocity);

        var ss = new BoxShape((Real)1);
        sb.AddShape(ss);
    }

    private void DrawShape(Shape shape, in Matrix4 mat, in Vector3 color)
    {
        Matrix4 ms;

        switch (shape)
        {
            case BoxShape s:
                ms = MatrixHelper.CreateScale((float)s.Size.X, (float)s.Size.Y, (float)s.Size.Z);
                cubes.Push(mat * ms, color);
                break;
            case SphereShape s:
                ms = MatrixHelper.CreateScale((float)s.Radius * 2);
                spheres.Push(mat * ms, color);
                break;
            case CylinderShape s:
                ms = MatrixHelper.CreateScale((float)s.Radius, (float)s.Height, (float)s.Radius);
                cylinders.Push(mat * ms, color);
                break;
            case CapsuleShape s:
                ms = MatrixHelper.CreateScale((float)s.Radius, (float)s.Length, (float)s.Radius);
                cylinders.Push(mat * ms, color);
                ms = MatrixHelper.CreateTranslation(0, 0.5f * (float)s.Length, 0) * MatrixHelper.CreateScale((float)s.Radius * 2);
                halfSpheres.Push(mat * ms, color);
                halfSpheres.Push(mat * MatrixHelper.CreateRotationX(MathF.PI) * ms, color);
                break;
            case ConeShape s:
                ms = MatrixHelper.CreateScale((float)s.Radius * 2, (float)s.Height, (float)s.Radius * 2);
                cones.Push(mat * ms, color);
                break;
        }
    }

    public override void Draw()
    {
        world.Step(PhysicsTimestep, multiThread);

        UpdateDisplayText();

        foreach (RigidBody body in world.RigidBodies)
        {
            if (body.Tag is RigidBodyTag { DoNotDraw: true }) continue;

            Matrix4 mat = Conversion.FromJitter(body);

            foreach (var shape in body.Shapes)
            {
                if (shape == floorShape)
                {
                    floor.Push(Matrix4.Identity);
                    continue;
                }

                var color = ColorGenerator.GetColor(shape.GetHashCode());
                if (!shape.RigidBody.Data.IsActive) color += new Vector3(0.2f, 0.2f, 0.2f);

                if (shape is TransformedShape ts)
                {
                    Matrix4 tmat = mat * MatrixHelper.CreateTranslation(Conversion.FromJitter(ts.Translation)) *
                                   Conversion.FromJitter(ts.Transformation);
                    DrawShape(ts.OriginalShape, tmat, color);
                }
                else
                {
                    DrawShape(shape, mat, color);
                }
            }
        }

        (currentDemo as IDrawUpdate)?.DrawUpdate();

        DebugDraw();

        if (!WantsCaptureMouse && (Mouse.ButtonPressBegin(Mouse.Button.Left) || grabbing))
        {
            Pick();
        }

        if (!Mouse.IsButtonDown(Mouse.Button.Left))
        {
            ClearGrab();
        }

        if (Keyboard.KeyPressBegin(Keyboard.Key.M))
        {
            multiThread = !multiThread;
        }

        if (Keyboard.KeyPressBegin(Keyboard.Key.Space))
        {
            ShootPrimitive();
        }

        base.Draw();
    }
}
