// Global using aliases for the fixed-point math types.
// The physics library (Lwfix.Net-Physics) defines these as global usings internally,
// but global usings do NOT propagate across project references.
global using Real = SimplexLab.Lwfix.Fixed32;
global using MathR = SimplexLab.Lwfix.FMath;

// Physics namespaces
global using SimplexLab.Lwfix.Physics;
global using SimplexLab.Lwfix.Physics.Collision;
global using SimplexLab.Lwfix.Physics.Collision.Shapes;
global using SimplexLab.Lwfix.Physics.Dynamics;
global using SimplexLab.Lwfix.Physics.Dynamics.Constraints;
global using SimplexLab.Lwfix.Physics.LinearMath;

// Rendering math (float-side)
global using Vector2 = System.Numerics.Vector2;
global using Vector3 = System.Numerics.Vector3;
global using Vector4 = System.Numerics.Vector4;
