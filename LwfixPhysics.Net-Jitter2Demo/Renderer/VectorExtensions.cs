namespace SimplexLab.LwfixPhysics.Jitter2Demo.Renderer;

/// <summary>
/// Extension methods for System.Numerics vector types to mirror the
/// helper API that the original JitterDemo's custom Vector4/Vector3 exposed.
/// </summary>
public static class VectorExtensions
{
    public static Vector3 XYZ(this Vector4 v) => new(v.X, v.Y, v.Z);

    /// <summary>Transforms a position (w=1) by the matrix and returns the XYZ components.</summary>
    public static Vector3 Transform(this Vector3 position, in Matrix4 matrix)
        => MatrixHelper.Transform(position, matrix);
}
