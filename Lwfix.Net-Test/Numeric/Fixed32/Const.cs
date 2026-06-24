using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TConst
    {
        private static readonly int Zero = 0;
        private static readonly int One = 1;
        private static readonly int NegativeOne = -1;
        private static readonly double Half = 0.5;
        private static readonly int MaxValue = int.MaxValue;
        private static readonly int MinValue = int.MinValue;
        private static readonly double PI = Math.PI;
        private static readonly double E = Math.E;
        private static readonly double Ln2 = Math.Log(2);
        private static readonly double Ln10 = Math.Log(10);
        private const int PRECISION = 6;

        [Fact]
        public void Normal()
        {
            Assert.Equal(Zero,        Fixed32.Zero.ToInt());
            Assert.Equal(One,         Fixed32.One.ToInt());
            Assert.Equal(NegativeOne, Fixed32.NegativeOne.ToInt());
            Assert.Equal(Half,        Fixed32.Half.ToDouble(),      PRECISION);
            Assert.Equal(MaxValue,    Fixed32.MaxValue.ToInt());
            Assert.Equal(MinValue,    Fixed32.MinValue.ToInt());
            Assert.Equal(PI,          Fixed32.PI.ToDouble(),        PRECISION);
            Assert.Equal(PI / 2,      Fixed32.Half_PI.ToDouble(),   PRECISION);
            Assert.Equal(PI * 2,      Fixed32.Two_PI.ToDouble(),    PRECISION);
            Assert.Equal(E,           Fixed32.E.ToDouble(),         PRECISION);
            Assert.Equal(Ln2,         Fixed32.Ln2.ToDouble(),       PRECISION);
            Assert.Equal(Ln10,        Fixed32.Ln10.ToDouble(),      PRECISION);
        }
    }
}
