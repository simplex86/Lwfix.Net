namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 角度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 两向量的角度（单位：度）
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static T Angle(FVector3<T> from, FVector3<T> to)
        {
            var magnitude = from.Magnitude * to.Magnitude;
            if (magnitude.IsZero()) return T.Zero;

            var acos = T.Acos(T.Clamp(Dot(from, to) / magnitude, T.NegativeOne, T.One));
            return T.RadianToDegree(acos);
        }

        /// <summary>
        /// 角度
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static T SignedAngle(FVector3<T> from, FVector3<T> to, FVector3<T> axis)
        {
            var fx = from.X;
            var fy = from.Y;
            var fz = from.Z;

            var tx = to.X;
            var ty = to.Y;
            var tz = to.Z;

            var ax = axis.X;
            var ay = axis.Y;
            var az = axis.Z;

            var n1 = Angle(from, to);
            var n2 = (fy * tz - fz * ty);
            var n3 = (fz * tx - fx * tz);
            var n4 = (fx * ty - fy * tx);
            var n5 = T.Sign(ax * n2 + ay * n3 + az * n4);

            return n1 * n5;
        }
    }
}
