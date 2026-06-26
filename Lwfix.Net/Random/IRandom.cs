using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 随机数 - 接口
    /// </summary>
    public interface IRandom
    {

    }

    /// <summary>
    /// 随机数 - 接口
    /// </summary>
    public interface IRandom<T> : IRandom where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Next();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        T Next(int min, int max);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        T Next(T min, T max);
    }

    /// <summary>
    /// 
    /// </summary>
    public class FRandomEntryAttribute<T> : Attribute where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 
        /// </summary>
        public FRandomEntryAttribute()
        {
            Type = typeof(T);
        }
    }
}
