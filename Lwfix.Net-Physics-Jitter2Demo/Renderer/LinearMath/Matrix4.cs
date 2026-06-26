using System.Runtime.InteropServices;

namespace SimplexLab.Lwfix.Physics.JDemo.Renderer;

/// <summary>
/// Column-major 4x4 matrix (memory layout matches OpenGL's default).
/// Uses column-vector multiplication convention: v' = M * v.
/// Translation lives in M14/M24/M34 (the last column).
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 64)]
public struct Matrix4
{
    public static Matrix4 Identity { get; } = new(
        1f, 0f, 0f, 0f,
        0f, 1f, 0f, 0f,
        0f, 0f, 1f, 0f,
        0f, 0f, 0f, 1f
    );

    public Matrix4(float m11, float m12, float m13, float m14,
        float m21, float m22, float m23, float m24,
        float m31, float m32, float m33, float m34,
        float m41, float m42, float m43, float m44)
    {
        M11 = m11; M12 = m12; M13 = m13; M14 = m14;
        M21 = m21; M22 = m22; M23 = m23; M24 = m24;
        M31 = m31; M32 = m32; M33 = m33; M34 = m34;
        M41 = m41; M42 = m42; M43 = m43; M44 = m44;
    }

    public static Matrix4 operator *(in Matrix4 a, in Matrix4 b) => Multiply(a, b);
    public static Matrix4 operator +(in Matrix4 a, in Matrix4 b) => Add(a, b);
    public static Matrix4 operator -(in Matrix4 a, in Matrix4 b) => Subtract(a, b);

    public static Vector4 operator *(in Matrix4 m, Vector4 v) => new(
        m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14 * v.W,
        m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24 * v.W,
        m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34 * v.W,
        m.M41 * v.X + m.M42 * v.Y + m.M43 * v.Z + m.M44 * v.W
    );

    public static Matrix4 Transpose(Matrix4 m) => new(
        m.M11, m.M21, m.M31, m.M41,
        m.M12, m.M22, m.M32, m.M42,
        m.M13, m.M23, m.M33, m.M43,
        m.M14, m.M24, m.M34, m.M44
    );

    public static Matrix4 Add(in Matrix4 a, in Matrix4 b) => new(
        a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13, a.M14 + b.M14,
        a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23, a.M24 + b.M24,
        a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33, a.M34 + b.M34,
        a.M41 + b.M41, a.M42 + b.M42, a.M43 + b.M43, a.M44 + b.M44
    );

    public static Matrix4 Subtract(in Matrix4 a, in Matrix4 b) => new(
        a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13, a.M14 - b.M14,
        a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23, a.M24 - b.M24,
        a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33, a.M34 - b.M34,
        a.M41 - b.M41, a.M42 - b.M42, a.M43 - b.M43, a.M44 - b.M44
    );

    public static Matrix4 Multiply(in Matrix4 a, in Matrix4 b) => new(
        a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
        a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
        a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
        a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,

        a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
        a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
        a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
        a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,

        a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
        a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
        a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
        a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,

        a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
        a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
        a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
        a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44
    );

    public static bool Invert(Matrix4 m, out Matrix4 result)
    {
        float a = m.M11, b = m.M12, c = m.M13, d = m.M14;
        float e = m.M21, f = m.M22, g = m.M23, h = m.M24;
        float i = m.M31, j = m.M32, k = m.M33, l = m.M34;
        float mn = m.M41, n = m.M42, o = m.M43, p = m.M44;

        float kp_lo = k * p - l * o;
        float jp_ln = j * p - l * n;
        float jo_kn = j * o - k * n;
        float ip_lm = i * p - l * mn;
        float io_km = i * o - k * mn;
        float in_jm = i * n - j * mn;

        float a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
        float a12 = -(e * kp_lo - g * ip_lm + h * io_km);
        float a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
        float a14 = -(e * jo_kn - f * io_km + g * in_jm);

        float det = a * a11 + b * a12 + c * a13 + d * a14;
        if (Math.Abs(det) < float.Epsilon)
        {
            result = new Matrix4(
                float.NaN, float.NaN, float.NaN, float.NaN,
                float.NaN, float.NaN, float.NaN, float.NaN,
                float.NaN, float.NaN, float.NaN, float.NaN,
                float.NaN, float.NaN, float.NaN, float.NaN);
            return false;
        }

        float invDet = 1.0f / det;

        result.M11 = a11 * invDet;
        result.M21 = a12 * invDet;
        result.M31 = a13 * invDet;
        result.M41 = a14 * invDet;

        result.M12 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
        result.M22 = +(a * kp_lo - c * ip_lm + d * io_km) * invDet;
        result.M32 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
        result.M42 = +(a * jo_kn - b * io_km + c * in_jm) * invDet;

        float gp_ho = g * p - h * o;
        float fp_hn = f * p - h * n;
        float fo_gn = f * o - g * n;
        float ep_hm = e * p - h * mn;
        float eo_gm = e * o - g * mn;
        float en_fm = e * n - f * mn;

        result.M13 = +(b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
        result.M23 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
        result.M33 = +(a * fp_hn - b * ep_hm + d * en_fm) * invDet;
        result.M43 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

        float gl_hk = g * l - h * k;
        float fl_hj = f * l - h * j;
        float fk_gj = f * k - g * j;
        float el_hi = e * l - h * i;
        float ek_gi = e * k - g * i;
        float ej_fi = e * j - f * i;

        result.M14 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
        result.M24 = +(a * gl_hk - c * el_hi + d * ek_gi) * invDet;
        result.M34 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
        result.M44 = +(a * fk_gj - b * ek_gi + c * ej_fi) * invDet;

        return true;
    }

    // Column-major memory layout (matches OpenGL default with transpose=false).
    [FieldOffset(0)] public float M11;
    [FieldOffset(4)] public float M21;
    [FieldOffset(8)] public float M31;
    [FieldOffset(12)] public float M41;

    [FieldOffset(16)] public float M12;
    [FieldOffset(20)] public float M22;
    [FieldOffset(24)] public float M32;
    [FieldOffset(28)] public float M42;

    [FieldOffset(32)] public float M13;
    [FieldOffset(36)] public float M23;
    [FieldOffset(40)] public float M33;
    [FieldOffset(44)] public float M43;

    [FieldOffset(48)] public float M14;
    [FieldOffset(52)] public float M24;
    [FieldOffset(56)] public float M34;
    [FieldOffset(60)] public float M44;
}
