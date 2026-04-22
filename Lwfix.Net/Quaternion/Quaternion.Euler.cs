using System.Numerics;
using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 四元数 - 欧拉角
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 欧拉角
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static FQuaternion<T> Euler(T x, T y, T z)
        {
            x *= T.DegToRad;
            y *= T.DegToRad;
            z *= T.DegToRad;

            var num9 = z * T.Half;
            var num6 = T.Sin(num9);
            var num5 = T.Cos(num9);
            var num8 = x * T.Half;
            var num4 = T.Sin(num8);
            var num3 = T.Cos(num8);
            var num7 = y * T.Half;
            var num2 = T.Sin(num7);
            var num1 = T.Cos(num7);

            var rotation = new FQuaternion<T>(num1 * num4 * num5 + num2 * num3 * num6,
                                              num2 * num3 * num5 - num1 * num4 * num6,
                                              num1 * num3 * num6 - num2 * num4 * num5,
                                              num1 * num3 * num5 + num2 * num4 * num6);

            return rotation;
        }

        /// <summary>
        /// 欧拉角
        /// </summary>
        /// <param name="eulerAngles"></param>
        /// <returns></returns>
        public static FQuaternion<T> Euler(FVector3<T> eulerAngles)
        {
            return Euler(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
        }
    }
}
