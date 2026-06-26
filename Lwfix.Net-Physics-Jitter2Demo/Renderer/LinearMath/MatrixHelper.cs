using System;

namespace SimplexLab.Lwfix.Physics.JDemo.Renderer;

/// <summary>
/// Factory methods for building Matrix4 transforms. All matrices use the
/// column-vector convention (translation in the last column).
/// </summary>
public static class MatrixHelper
{
    public static Matrix4 CreateTranslation(float x, float y, float z)
    {
        Matrix4 r = Matrix4.Identity;
        r.M14 = x; r.M24 = y; r.M34 = z;
        return r;
    }

    public static Matrix4 CreateTranslation(Vector3 position)
    {
        Matrix4 r = Matrix4.Identity;
        r.M14 = position.X; r.M24 = position.Y; r.M34 = position.Z;
        return r;
    }

    public static Matrix4 CreateScale(float scale)
    {
        Matrix4 r = Matrix4.Identity;
        r.M11 = scale; r.M22 = scale; r.M33 = scale;
        return r;
    }

    public static Matrix4 CreateScale(float x, float y, float z)
    {
        Matrix4 r = Matrix4.Identity;
        r.M11 = x; r.M22 = y; r.M33 = z;
        return r;
    }

    public static Matrix4 CreateRotationX(float radians)
    {
        Matrix4 r = Matrix4.Identity;
        float c = MathF.Cos(radians), s = MathF.Sin(radians);
        r.M22 = c; r.M23 = -s; r.M32 = s; r.M33 = c;
        return r;
    }

    public static Matrix4 CreateRotationY(float radians)
    {
        Matrix4 r = Matrix4.Identity;
        float c = MathF.Cos(radians), s = MathF.Sin(radians);
        r.M11 = c; r.M13 = s; r.M31 = -s; r.M33 = c;
        return r;
    }

    public static Matrix4 CreateRotationZ(float radians)
    {
        Matrix4 r = Matrix4.Identity;
        float c = MathF.Cos(radians), s = MathF.Sin(radians);
        r.M11 = c; r.M12 = -s; r.M21 = s; r.M22 = c;
        return r;
    }

    public static Matrix4 CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio,
        float nearPlaneDistance, float farPlaneDistance)
    {
        if (fieldOfView <= 0.0f || fieldOfView >= MathF.PI)
            throw new ArgumentOutOfRangeException(nameof(fieldOfView));

        float yScale = 1.0f / MathF.Tan(fieldOfView * 0.5f);
        float xScale = yScale / aspectRatio;

        Matrix4 r;
        r.M11 = xScale; r.M12 = 0; r.M13 = 0; r.M14 = 0;
        r.M21 = 0; r.M22 = yScale; r.M23 = 0; r.M24 = 0;
        r.M31 = 0; r.M32 = 0; r.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance); r.M34 = -1.0f;
        r.M41 = 0; r.M42 = 0; r.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance); r.M44 = 0;
        return Matrix4.Transpose(r);
    }

    public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top,
        float zNearPlane, float zFarPlane)
    {
        Matrix4 r;
        r.M11 = 2.0f / (right - left); r.M12 = 0; r.M13 = 0; r.M14 = 0;
        r.M21 = 0; r.M22 = 2.0f / (top - bottom); r.M23 = 0; r.M24 = 0;
        r.M31 = 0; r.M32 = 0; r.M33 = 1.0f / (zNearPlane - zFarPlane); r.M34 = 0;
        r.M41 = (left + right) / (left - right);
        r.M42 = (top + bottom) / (bottom - top);
        r.M43 = zNearPlane / (zNearPlane - zFarPlane);
        r.M44 = 1.0f;
        return Matrix4.Transpose(r);
    }

    public static Matrix4 CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
    {
        Vector3 zaxis = Vector3.Normalize(cameraPosition - cameraTarget);
        Vector3 xaxis = Vector3.Normalize(Vector3.Cross(cameraUpVector, zaxis));
        Vector3 yaxis = Vector3.Cross(zaxis, xaxis);

        Matrix4 r;
        r.M11 = xaxis.X; r.M12 = yaxis.X; r.M13 = zaxis.X; r.M14 = 0.0f;
        r.M21 = xaxis.Y; r.M22 = yaxis.Y; r.M23 = zaxis.Y; r.M24 = 0.0f;
        r.M31 = xaxis.Z; r.M32 = yaxis.Z; r.M33 = zaxis.Z; r.M34 = 0.0f;
        r.M41 = -Vector3.Dot(xaxis, cameraPosition);
        r.M42 = -Vector3.Dot(yaxis, cameraPosition);
        r.M43 = -Vector3.Dot(zaxis, cameraPosition);
        r.M44 = 1.0f;
        return Matrix4.Transpose(r);
    }

    /// <summary>Transforms a position (w=1) by the matrix and returns the XYZ components.</summary>
    public static Vector3 Transform(Vector3 position, in Matrix4 matrix)
    {
        Matrix4 m = matrix; // copy for safe ref
        return new Vector3(
            m.M11 * position.X + m.M12 * position.Y + m.M13 * position.Z + m.M14,
            m.M21 * position.X + m.M22 * position.Y + m.M23 * position.Z + m.M24,
            m.M31 * position.X + m.M32 * position.Y + m.M33 * position.Z + m.M34
        );
    }
}
