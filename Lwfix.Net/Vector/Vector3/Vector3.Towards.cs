using System.Numerics;
using System;
using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 三维向量 - 移动
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 移动到
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FVector3<T> MoveTowards(FVector3<T> current, FVector3<T> target, T maxDistanceDelta)
        {
            var x = target.X - current.X;
            var y = target.Y - current.Y;
            var z = target.Z - current.Z;

            var d = (x * x + y * y + z * z).Sqrt();
            if (d.IsZero() || maxDistanceDelta.IsPositive() && d <= maxDistanceDelta)
            {
                return target;
            }

            x = current.X + x / d * maxDistanceDelta;
            y = current.Y + y / d * maxDistanceDelta;
            z = current.Z + z / d * maxDistanceDelta;

            return new FVector3<T>(x, y, z);
        }

        /// <summary>
        /// 旋转到
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxRadiansDelta"></param>
        /// <param name="maxMagnitudeDelta"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FVector3<T> RotateTowards(FVector3<T> current, FVector3<T> target, T maxRadiansDelta, T maxMagnitudeDelta)
        {
            var from = current.Normalized;
            var to = target.Normalized;

            var radians = T.Acos(FVector3<T>.Dot(from, to));
            radians = T.Min(radians, maxRadiansDelta);

            var degrees = T.RadianToDegree(radians);
            var axis = FVector3<T>.Cross(from, to);
            var increment = FQuaternion<T>.AngleAxis(degrees, axis);

            return FVector3<T>.ClampMagnitude(increment * current, maxMagnitudeDelta);
        }
    }
}
