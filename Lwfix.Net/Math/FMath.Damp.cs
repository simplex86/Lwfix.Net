namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 函数库 - 
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="smoothTime"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static T SmoothDamp<T>(T current, T target, ref T currentVelocity, T smoothTime, T maxSpeed, T deltaTime) where T : struct, IFixed<T>
        {
            return T.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="smoothTime"></param>
        /// <param name="maxSpeed"></param>
        /// <returns></returns>
        public static T SmoothDamp<T>(T current, T target, ref T currentVelocity, T smoothTime, T maxSpeed) where T : struct, IFixed<T>
        {
            return T.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="smoothTime"></param>
        /// <returns></returns>
        public static T SmoothDamp<T>(T current, T target, ref T currentVelocity, T smoothTime) where T : struct, IFixed<T>
        {
            return T.SmoothDamp(current, target, ref currentVelocity, smoothTime);
        }
    }
}
