using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 正弦查找表
    /// <para>延迟生成正弦查找表，覆盖[0, π/2]区间</para>
    /// </summary>
    public partial struct Fixed64
    {
        private static readonly Lazy<Int128[]> _sinLut = new(() => GenerateSinLut());
        private static Int128[] SinLut => _sinLut.Value;

        /// <summary>
        /// 生成正弦查找表
        /// </summary>
        private static Int128[] GenerateSinLut()
        {
            // 表大小与>>47移位匹配：Half_PI.rawvalue >> 47 ≈ 205887
            var length = 205888;
            var lut = new Int128[length];
            var step = Half_PI / (length - 1);
            for (int i = 0; i < length; i++)
            {
                lut[i] = Sin(step * i).rawvalue;
            }
            return lut;
        }
    }
}
