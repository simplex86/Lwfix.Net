namespace SimplexLab.LwfixPhysics.VelcroDemo;

using PhysicsVector2 = SimplexLab.LwfixPhysics.Velcro.Primitives.Vector2;

/// <summary>
/// Contract every demo implements. Demos are registered with the
/// <see cref="Playground"/>, which builds/clears them on selection and
/// forwards input + per-frame updates.
/// </summary>
public abstract class DemoScreen
{
    public Playground Host { get; private set; } = null!;
    public SimplexLab.LwfixPhysics.Velcro.Dynamics.World World { get; private set; } = null!;

    public Renderer.Camera2D Camera => Host.Camera;
    public Renderer.InputHelper Input => Host.Input;

    /// <summary>Short name shown in the demo selection list.</summary>
    public abstract string Name { get; }

    /// <summary>One-line title shown in the details panel.</summary>
    public virtual string Title => Name;

    /// <summary>Multiline description shown in the details panel.</summary>
    public virtual string Details => string.Empty;

    /// <summary>Control hints shown in the details panel.</summary>
    public virtual string Controls => string.Empty;

    /// <summary>Whether the demo gets an invisible border around the visible area.</summary>
    protected bool HasBorder { get; set; } = true;

    // --- User agent (keyboard-driven body) ---
    private SimplexLab.LwfixPhysics.Velcro.Dynamics.Body? _userAgent;
    private SimplexLab.Lwfix.Fixed32 _agentForce;
    private SimplexLab.Lwfix.Fixed32 _agentTorque;

    // --- Mouse joint for grabbing bodies ---
    private SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.FixedMouseJoint? _mouseJoint;
    private SimplexLab.LwfixPhysics.Velcro.Dynamics.Prefabs.Border? _border;

    internal void Initialize(Playground host)
    {
        Host = host;
        World = new SimplexLab.LwfixPhysics.Velcro.Dynamics.World(PhysicsVector2.Zero);
    }

    /// <summary>Called by Playground when this demo is selected. Sets up a fresh world + border.</summary>
    internal void Load()
    {
        World.Clear();
        if (HasBorder)
            _border = new SimplexLab.LwfixPhysics.Velcro.Dynamics.Prefabs.Border(World, Camera);

        LoadContent();
    }

    /// <summary>Dem override to build the scene (bodies, joints, gravity, ...).</summary>
    protected abstract void LoadContent();

    /// <summary>Optional per-frame logic (e.g. updating procedural prefabs).</summary>
    public virtual void Update(float dt) { }

    /// <summary>Optional extra drawing on top of the DebugView (usually empty).</summary>
    public virtual void Draw() { }

    protected void SetUserAgent(SimplexLab.LwfixPhysics.Velcro.Dynamics.Body agent,
                                SimplexLab.Lwfix.Fixed32 force, SimplexLab.Lwfix.Fixed32 torque)
    {
        _userAgent = agent;
        _agentForce = force;
        _agentTorque = torque;
    }

    /// <summary>Process camera + mouse-grab + user-agent input. Called by Playground each frame.</summary>
    internal void HandleInput(float dt)
    {
        HandleCamera(dt);
        HandleCursor();
        if (_userAgent != null)
            HandleUserAgent();
        HandleCustomInput(dt);
    }

    /// <summary>Override to handle demo-specific input (e.g. the racing car throttle).</summary>
    protected virtual void HandleCustomInput(float dt) { }

    private void HandleCamera(float dt)
    {
        PhysicsVector2 move = PhysicsVector2.Zero;
        SimplexLab.Lwfix.Fixed32 s = (SimplexLab.Lwfix.Fixed32)(10f * dt);
        if (Input.IsKeyDown(Silk.NET.Input.Key.Up)) move = new PhysicsVector2(move.X, move.Y - s);
        if (Input.IsKeyDown(Silk.NET.Input.Key.Down)) move = new PhysicsVector2(move.X, move.Y + s);
        if (Input.IsKeyDown(Silk.NET.Input.Key.Left)) move = new PhysicsVector2(move.X - s, move.Y);
        if (Input.IsKeyDown(Silk.NET.Input.Key.Right)) move = new PhysicsVector2(move.X + s, move.Y);
        if (move != PhysicsVector2.Zero) Camera.MoveCamera(move);

        if (Input.IsKeyDown(Silk.NET.Input.Key.PageUp))
            Camera.Zoom += (SimplexLab.Lwfix.Fixed32)(5f * dt * (float)Camera.Zoom / 20f);
        if (Input.IsKeyDown(Silk.NET.Input.Key.PageDown))
            Camera.Zoom -= (SimplexLab.Lwfix.Fixed32)(5f * dt * (float)Camera.Zoom / 20f);

        if (Input.IsNewKeyPress(Silk.NET.Input.Key.Home))
            Camera.ResetCamera();
    }

    private void HandleCursor()
    {
        PhysicsVector2 pos = Camera.ConvertScreenToWorld(Input.MouseX, Input.MouseY);

        if (Input.IsNewLeftMousePress && _mouseJoint == null)
        {
            SimplexLab.LwfixPhysics.Velcro.Dynamics.Fixture? hit = World.TestPoint(pos);
            if (hit != null)
            {
                var body = hit.Body;
                _mouseJoint = new SimplexLab.LwfixPhysics.Velcro.Dynamics.Joints.FixedMouseJoint(body, pos);
                _mouseJoint.MaxForce = (SimplexLab.Lwfix.Fixed32)1000.0 * body.Mass;
                SimplexLab.LwfixPhysics.Velcro.Utilities.JointHelper.LinearStiffness(
                    (SimplexLab.Lwfix.Fixed32)5.0, (SimplexLab.Lwfix.Fixed32)0.7, body, null!,
                    out SimplexLab.Lwfix.Fixed32 stiffness, out SimplexLab.Lwfix.Fixed32 damping);
                _mouseJoint.Stiffness = stiffness;
                _mouseJoint.Damping = damping;
                World.AddJoint(_mouseJoint);
                body.Awake = true;
            }
        }

        if (Input.IsNewLeftMouseRelease && _mouseJoint != null)
        {
            World.RemoveJoint(_mouseJoint);
            _mouseJoint = null;
        }

        if (_mouseJoint != null)
            _mouseJoint.WorldAnchorB = pos;
    }

    private void HandleUserAgent()
    {
        PhysicsVector2 force = PhysicsVector2.Zero;
        SimplexLab.Lwfix.Fixed32 torque = SimplexLab.Lwfix.Fixed32.Zero;

        SimplexLab.Lwfix.Fixed32 f = _agentForce * (SimplexLab.Lwfix.Fixed32)0.6;
        if (Input.IsKeyDown(Silk.NET.Input.Key.A)) force = new PhysicsVector2(force.X - f, force.Y);
        if (Input.IsKeyDown(Silk.NET.Input.Key.S)) force = new PhysicsVector2(force.X, force.Y + f);
        if (Input.IsKeyDown(Silk.NET.Input.Key.D)) force = new PhysicsVector2(force.X + f, force.Y);
        if (Input.IsKeyDown(Silk.NET.Input.Key.W)) force = new PhysicsVector2(force.X, force.Y - f);
        if (Input.IsKeyDown(Silk.NET.Input.Key.Q)) torque -= _agentTorque;
        if (Input.IsKeyDown(Silk.NET.Input.Key.E)) torque += _agentTorque;

        _userAgent!.ApplyForce(force);
        _userAgent.ApplyTorque(torque);
    }
}
