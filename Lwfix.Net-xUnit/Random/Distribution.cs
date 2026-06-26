using SimplexLab.Fixed;
using Xunit;
using System;
using System.Collections.Generic;

namespace LwfixTest.Fixed.Random
{
    public partial class TFRandom
    {
        [Fact]
        public void Distribution_100Values_AllInValidRange()
        {
            for (int i = 0; i < 100; i++)
            {
                var value = FRandom.Shared.Next<Fixed32>();
                var d = value.ToDouble();

                Assert.True(d >= 0.0, $"Value {d} at iteration {i} is less than 0");
                Assert.True(d < 1.0, $"Value {d} at iteration {i} is >= 1");
            }
        }

        [Fact]
        public void Distribution_100Values_MeanIsRoughlyHalf()
        {
            var values = new List<double>();
            for (int i = 0; i < 100; i++)
            {
                var value = FRandom.Shared.Next<Fixed32>();
                values.Add(value.ToDouble());
            }

            var mean = 0.0;
            foreach (var v in values)
            {
                mean += v;
            }
            mean /= values.Count;

            // Mean should be roughly 0.5 for uniform [0, 1)
            // Allow generous tolerance since it's only 100 samples
            Assert.True(Math.Abs(mean - 0.5) < 0.15, $"Mean {mean} is too far from 0.5");
        }

        [Fact]
        public void Distribution_100ValuesInRange_MeanIsRoughlyCenter()
        {
            var values = new List<double>();
            for (int i = 0; i < 100; i++)
            {
                var value = FRandom.Shared.Next<Fixed32>(0, 10);
                values.Add(value.ToDouble());
            }

            var mean = 0.0;
            foreach (var v in values)
            {
                mean += v;
            }
            mean /= values.Count;

            // Mean should be roughly 5.0 for uniform [0, 10)
            Assert.True(Math.Abs(mean - 5.0) < 1.5, $"Mean {mean} is too far from 5.0");
        }

        [Fact]
        public void Distribution_ValuesAreNotAllSame()
        {
            var values = new HashSet<double>();
            for (int i = 0; i < 20; i++)
            {
                var value = FRandom.Shared.Next<Fixed32>();
                values.Add(value.ToDouble());
            }

            // With 20 samples, we should get at least 2 distinct values
            Assert.True(values.Count >= 2, "Random values should not all be identical");
        }
    }
}
