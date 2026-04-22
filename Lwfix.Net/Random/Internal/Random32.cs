using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 32位随机数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class FRandom32 : IRandom<Fixed32>
    {
        // 线性同余伪随机
        // 详情见《计算机程序设计艺术》.卷2.第三章

        private int s = 0;

        private const int a = 1103515245;
        private const int b = 15107;
        private const int m = 838859327;

        /// <summary>
        /// 
        /// </summary>
        public FRandom32()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0); ;
            s = (int)(ts.TotalSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        public FRandom32(int seed)
        {
            s = seed;
        }

        /// <summary>
        /// 获取 [0, 1) 区间内的随机数
        /// </summary>
        /// <returns></returns>
        public Fixed32 Next()
        {
            s = (int)((s * (long)a + b) % m);
            return new Fixed32(s) / m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Fixed32 Next(int min, int max)
        {
            var mod = Next();
            max = max - min;

            return (mod * max + min).Integral();
        }

        /// <summary>
        /// 获取 [min, max) 区间内的随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Fixed32 Next(Fixed32 min, Fixed32 max)
        {
            min = min.Round();
            max = max.Round();

            var mod = Next();
            max = max - min;

            return (mod * max + min).Integral();
        }
    }
}
