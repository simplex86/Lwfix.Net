namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 角度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 两向量的角度（单位：度）
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static T Angle(FVector2<T> from, FVector2<T> to)
        {
            var magnitude = from.Magnitude * to.Magnitude;
            if (magnitude.IsZero()) return T.Zero;

            var acos = T.Acos(T.Clamp(Dot(from, to) / magnitude, T.NegativeOne, T.One));
            return T.RadianToDegree(acos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static T SignedAngle(FVector2<T> from, FVector2<T> to)
        {
            return Angle(from, to) * (from.X * to.Y - from.Y * to.X).Sign();
        }
    }
}
