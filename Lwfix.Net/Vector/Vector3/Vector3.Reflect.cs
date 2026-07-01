using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 三维向量 - 反射
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 反射
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FVector3<T> Reflect(FVector3<T> direction, FVector3<T> normal)
        {
            var t = -2 * Dot(normal, direction);
            var x = t * normal.X + direction.X;
            var y = t * normal.Y + direction.Y;
            var z = t * normal.Z + direction.Z;

            return new FVector3<T>(x, y, z);
        }
    }
}
