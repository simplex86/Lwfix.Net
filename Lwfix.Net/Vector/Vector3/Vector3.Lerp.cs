namespace SimplexLab.Fixed
{
    /// <summary>
    /// 三维向量 - 插值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// （不限制）插值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FVector3<T> Lerp(FVector3<T> a, FVector3<T> b, T t)
        {
            
            return new FVector3<T>(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t, a.Z + (b.Z - a.Z) * t);
        }

        /// <summary>
        /// 插值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FVector3<T> ClampLerp(FVector3<T> a, FVector3<T> b, T t)
        {
            t = T.Clamp01(t);
            return Lerp(a, b, t);
        }

        /// <summary>
        /// 球面插值
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FVector3<T> Slerp(FVector3<T> from, FVector3<T> to, T t)
        {
            
            var dot = Dot(from, to);
            dot = T.Clamp(dot, T.NegativeOne, T.One);

            var theta = T.Acos(dot) * t;
            var cos = T.Cos(theta);
            var sin = T.Sin(theta);

            var relative = to - (from * dot);
            relative.Normalize();

            var v = from * cos + relative * sin;
            return v;
        }

        /// <summary>
        /// 球面插值
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FVector3<T> ClampSlerp(FVector3<T> from, FVector3<T> to, T t)
        {
            t = T.Clamp01(t);
            return Slerp(from, to, t);
        }
    }
}
