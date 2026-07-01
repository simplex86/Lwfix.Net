using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 四元数 - 插值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FQuaternion<T> Lerp(FQuaternion<T> from, FQuaternion<T> to, T t)
        {
            var quaternion = from * (1 - t) + to * t;
            quaternion.Normalize();

            return quaternion;

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FQuaternion<T> ClampLerp(FQuaternion<T> from, FQuaternion<T> to, T t)
        {
            t = T.Clamp01(t);
            return Lerp(from, to, t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FQuaternion<T> Slerp(FQuaternion<T> from, FQuaternion<T> to, T t)
        {
            var dot = Dot(from, to);
            if (IsEqualUsingDot(dot)) return from;

            dot = T.Clamp(dot, T.NegativeOne, T.One);
            var cos = T.Acos(dot);
            var sin = T.Sqrt(T.One - cos * cos);

            var t1 = T.Half;
            var t2 = T.Half;
            if (sin != T.Zero)
            {
                t1 = T.Sin((T.One - t) * cos) / sin;
                t2 = T.Sin(t * cos) / sin;
            }

            return new FQuaternion<T>(from.X * t1 + to.X * t2,
                                      from.Y * t1 + to.Y * t2,
                                      from.Z * t1 + to.Z * t2,
                                      from.W * t1 + to.W * t2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FQuaternion<T> ClampSlerp(FQuaternion<T> from, FQuaternion<T> to, T t)
        {
            t = T.Clamp01(t);
            return Slerp(from, to, t);
        }
    }
}
