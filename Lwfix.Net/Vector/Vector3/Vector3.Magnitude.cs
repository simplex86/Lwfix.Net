using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 三维向量 - 大小
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 大小
        /// </summary>
        public readonly T Magnitude => SqrMagnitude.Sqrt();

        /// <summary>
        /// 大小
        /// </summary>
        public readonly T SqrMagnitude => X * X + Y * Y + Z * Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FVector3<T> ClampMagnitude(FVector3<T> vector, T maxMagnitude)
        {
            var magnitude = vector.Magnitude;
            if (magnitude > maxMagnitude)
            {
                var x = vector.X / magnitude * maxMagnitude;
                var y = vector.Y / magnitude * maxMagnitude;
                var z = vector.Z / magnitude * maxMagnitude;

                return new FVector3<T>(x, y, z);
            }

            return vector;
        }
    }
}
