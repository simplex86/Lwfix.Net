using System.Numerics;
using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 四元数 - 角
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Angle(FQuaternion<T> a, FQuaternion<T> b)
        {
            var dot = Dot(a, b);
            var cos = T.Acos(T.Min(dot.Abs(), T.One)) * T.Two;

            return IsEqualUsingDot(dot) ? T.Zero : cos * T.RadToDeg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static FQuaternion<T> AngleAxis(T angle, FVector3<T> axis)
        {
            axis = axis * T.DegToRad;
            axis.Normalize();

            var half = angle * T.DegToRad * T.Half;
            var sin = T.Sin(half);

            var rotation = new FQuaternion<T>()
            {
                X = axis.X * sin,
                Y = axis.Y * sin,
                Z = axis.Z * sin,
                W = T.Cos(half),
            };
            return rotation;
        }
    }
}
