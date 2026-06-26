using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    public partial class TConst64
    {
        private static readonly int Zero = 0;
        private static readonly int One = 1;
        private static readonly int NegativeOne = -1;
        private static readonly double Half = 0.5;
        private static readonly long MaxValue = long.MaxValue;
        private static readonly long MinValue = long.MinValue;
        private static readonly double PI = Math.PI;
        private static readonly double E = Math.E;
        private static readonly double Ln2 = Math.Log(2);
        private static readonly double Ln10 = Math.Log(10);
        private const int PRECISION = 6;

        [Fact]
        public void Normal()
        {
            Assert.Equal(Zero,        Fixed64.Zero.ToInt());
            Assert.Equal(One,         Fixed64.One.ToInt());
            Assert.Equal(NegativeOne, Fixed64.NegativeOne.ToInt());
            Assert.Equal(Half,        Fixed64.Half.ToDouble(),      PRECISION);
            Assert.Equal(MaxValue,    Fixed64.MaxValue.ToLong());
            Assert.Equal(MinValue,    Fixed64.MinValue.ToLong());
            Assert.Equal(PI,          Fixed64.PI.ToDouble(),        PRECISION);
            Assert.Equal(PI / 2,      Fixed64.Half_PI.ToDouble(),   PRECISION);
            Assert.Equal(PI * 2,      Fixed64.Two_PI.ToDouble(),    PRECISION);
            Assert.Equal(E,           Fixed64.E.ToDouble(),         PRECISION);
            Assert.Equal(Ln2,         Fixed64.Ln2.ToDouble(),       PRECISION);
            Assert.Equal(Ln10,        Fixed64.Ln10.ToDouble(),      PRECISION);
        }
    }
}
