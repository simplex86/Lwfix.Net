using System.Numerics;
using System;
using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 四元数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 创建具有指定的 forward 方向的旋转
        /// </summary>
        /// <param name="forward"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FQuaternion<T> LookRotation(FVector3<T> forward)
        {
            return LookRotation(forward, FVector3<T>.Up);
        }

        /// <summary>
        /// 创建具有指定的 forward 和 up 方向的旋转
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="upwards"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FQuaternion<T> LookRotation(FVector3<T> forward, FVector3<T> upwards)
        {
            return FromMatrix(FMatrix3x3<T>.LookAt(forward, upwards));
        }

        /// <summary>
        /// 创建从 from 到 to 的旋转
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FQuaternion<T> FromToRotation(FVector3<T> from, FVector3<T> to)
        {
            var w = FVector3<T>.Cross(from, to);
            var quaternion = new FQuaternion<T>(w.X, w.Y, w.Z, FVector3<T>.Dot(from, to));
            quaternion.W += (from.SqrMagnitude * to.SqrMagnitude).Sqrt();
            quaternion.Normalize();

            return quaternion;
        }

        /// <summary>
        /// 从 from 向 to 旋转
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="maxDegreesDelta"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FQuaternion<T> RotateTowards(FQuaternion<T> from, FQuaternion<T> to, T maxDegreesDelta)
        {
            var angle = Angle(from, to);
            return angle.IsZero() ? to : Slerp(from, to, T.Min(T.One, maxDegreesDelta / angle));
        }
    }
}
