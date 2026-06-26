using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 正切查找表
    /// <para>延迟生成正切查找表，覆盖[0, π/2]区间</para>
    /// </summary>
    public partial struct Fixed64
    {
        private static readonly Lazy<Int128[]> _tanLut = new(() => GenerateTanLut());
        private static Int128[] TanLut => _tanLut.Value;

        /// <summary>
        /// 生成正切查找表
        /// </summary>
        private static Int128[] GenerateTanLut()
        {
            var length = 65537;
            var lut = new Int128[length];
            var step = Half_PI / (length - 1);
            for (int i = 0; i < length; i++)
            {
                var value = Tan(step * i);
                if (value.IsInfinity() || value.IsMax())
                {
                    lut[i] = MaxValue.rawvalue;
                }
                else
                {
                    lut[i] = value.rawvalue;
                }
            }
            return lut;
        }
    }
}
