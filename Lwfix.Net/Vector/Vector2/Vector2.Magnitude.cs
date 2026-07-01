using System.Numerics;
using System;
using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 大小
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 向量的长度
        /// </summary>
        public readonly T Magnitude => SqrMagnitude.Sqrt();

        /// <summary>
        /// 向量的长度平方
        /// </summary>
        public readonly T SqrMagnitude => X * X + Y * Y;

        /// <summary>
        /// vector的拷贝，其长度被限制为 maxMagnitude
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxMagnitude"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FVector2<T> ClampMagnitude(FVector2<T> vector, T maxMagnitude)
        {
            var magnitude = vector.Magnitude;
            if (magnitude > maxMagnitude)
            {
                var x = vector.X / magnitude * maxMagnitude;
                var y = vector.Y / magnitude * maxMagnitude;

                return new FVector2<T>(x, y);
            }

            return vector;
        }
    }
}
