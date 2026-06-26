using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Random
{
    public partial class TFRandom
    {
        [Fact]
        public void Next_IntRange_ReturnsValueInValidRange()
        {
            var value = FRandom.Shared.Next<Fixed32>(0, 10);

            var d = value.ToDouble();
            Assert.True(d >= 0.0, $"Expected >= 0, got {d}");
            Assert.True(d < 10.0, $"Expected < 10, got {d}");
        }

        [Fact]
        public void Next_IntRange_ReturnsIntegerValue()
        {
            var value = FRandom.Shared.Next<Fixed32>(0, 10);

            var d = value.ToDouble();
            Assert.Equal(Math.Floor(d), d, 1e-9);
        }

        [Fact]
        public void Next_FixedRange_ReturnsValueInValidRange()
        {
            var value = FRandom.Shared.Next<Fixed32>(Fixed32.Zero, Fixed32.One);

            var d = value.ToDouble();
            Assert.True(d >= 0.0, $"Expected >= 0, got {d}");
            Assert.True(d < 1.0, $"Expected < 1, got {d}");
        }

        [Fact]
        public void Next_FixedRange_NegativeRange_ReturnsValueInValidRange()
        {
            var value = FRandom.Shared.Next<Fixed32>(new Fixed32(-5), new Fixed32(5));

            var d = value.ToDouble();
            Assert.True(d >= -5.0, $"Expected >= -5, got {d}");
            Assert.True(d < 5.0, $"Expected < 5, got {d}");
        }

        [Fact]
        public void Next_IntRange_SmallRange_ReturnsValidValue()
        {
            var value = FRandom.Shared.Next<Fixed32>(0, 2);

            var d = value.ToDouble();
            Assert.True(d >= 0.0, $"Expected >= 0, got {d}");
            Assert.True(d < 2.0, $"Expected < 2, got {d}");
        }
    }
}
