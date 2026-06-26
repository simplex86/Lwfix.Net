// Global using aliases for the fixed-point math types.
// The physics library (Lwfix.Net-Physics) defines these as global usings internally,
// but global usings do NOT propagate across project references.
global using Real = SimplexLab.Fixed.Fixed32;
global using MathR = SimplexLab.Fixed.FMath;

// Physics namespaces
global using SimplexLab.Fixed.Physics;
global using SimplexLab.Fixed.Physics.Collision;
global using SimplexLab.Fixed.Physics.Collision.Shapes;
global using SimplexLab.Fixed.Physics.Dynamics;
global using SimplexLab.Fixed.Physics.Dynamics.Constraints;
global using SimplexLab.Fixed.Physics.LinearMath;

// Rendering math (float-side)
global using Vector2 = System.Numerics.Vector2;
global using Vector3 = System.Numerics.Vector3;
global using Vector4 = System.Numerics.Vector4;
