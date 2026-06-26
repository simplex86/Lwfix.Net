using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Random
{
    public partial class TFRandom
    {
        private const double TOLERANCE = 10e-3;

        [Fact]
        public void Next_ReturnsValueInZeroToOneRange()
        {
            var value = FRandom.Shared.Next<Fixed32>();

            Assert.True(value.ToDouble() >= 0.0, $"Expected >= 0, got {value.ToDouble()}");
            Assert.True(value.ToDouble() < 1.0, $"Expected < 1, got {value.ToDouble()}");
        }

        [Fact]
        public void Next_MultipleCalls_ReturnDifferentValues()
        {
            var v1 = FRandom.Shared.Next<Fixed32>();
            var v2 = FRandom.Shared.Next<Fixed32>();
            var v3 = FRandom.Shared.Next<Fixed32>();

            // At least two of three should differ (non-constant)
            var allSame = (v1 == v2) && (v2 == v3);
            Assert.False(allSame, "Multiple calls to Next() should not all return the same value");
        }

        [Fact]
        public void Next_MultipleCalls_AllInValidRange()
        {
            for (int i = 0; i < 50; i++)
            {
                var value = FRandom.Shared.Next<Fixed32>();
                var d = value.ToDouble();

                Assert.True(d >= 0.0, $"Value {d} at iteration {i} is less than 0");
                Assert.True(d < 1.0, $"Value {d} at iteration {i} is >= 1");
            }
        }
    }
}
