using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 64位随机数
    /// <para>使用线性同余法生成伪随机数</para>
    /// </summary>
    internal class FRandom64 : IRandom<Fixed64>
    {
        // 线性同余伪随机
        // 详情见《计算机程序设计艺术》.卷2.第三章

        private long s = 0;

        private const long a = 6364136223846793005L;
        private const long b = 1442695040888963407L;

        /// <summary>
        /// 默认构造，使用当前时间作为种子
        /// </summary>
        public FRandom64()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            s = (long)ts.TotalSeconds;
        }

        /// <summary>
        /// 使用指定种子构造
        /// </summary>
        /// <param name="seed">随机种子</param>
        public FRandom64(long seed)
        {
            s = seed;
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <returns>随机数</returns>
        public Fixed64 Next()
        {
            s = s * a + b;
            return new Fixed64((double)s / long.MaxValue);
        }

        /// <summary>
        /// 获取指定范围内的随机整数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>随机整数</returns>
        public Fixed64 Next(int min, int max)
        {
            var mod = Next();
            max = max - min;

            return (mod * max + min).Integral();
        }

        /// <summary>
        /// 获取指定范围内的随机整数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>随机整数</returns>
        public Fixed64 Next(Fixed64 min, Fixed64 max)
        {
            min = min.Round();
            max = max.Round();

            var mod = Next();
            max = max - min;

            return (mod * max + min).Integral();
        }
    }
}
