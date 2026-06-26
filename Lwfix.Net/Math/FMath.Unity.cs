namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 函数库 - 数学
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDelta"></param>
        /// <returns></returns>
        public static T MoveTowards<T>(T current, T target, T maxDelta) where T : struct, IFixed<T>
        {
            return T.MoveTowards(current, target, maxDelta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDelta"></param>
        /// <returns></returns>
        public static T MoveTowardsAngle<T>(T current, T target, T maxDelta) where T : struct, IFixed<T>
        {
            return T.MoveTowardsAngle(current, target, maxDelta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T Repeat<T>(T t, T length) where T : struct, IFixed<T>
        {
            return T.Repeat(t, length);
        }

        /// <summary>
        /// 两个角度之间的最小差（度）
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T DeltaAngle<T>(T current, T target) where T : struct, IFixed<T>
        {
            return T.DeltaAngle(current, target);
        }
    }
}
