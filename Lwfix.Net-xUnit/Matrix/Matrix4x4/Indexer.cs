using Xunit;
using SimplexLab.Lwfix;
using System;

namespace SimplexLab.Lwfix.Test.Matrix
{
    public partial class TMatrix4x4
    {
        [Fact]
        public void Indexer_ReadAllElements()
        {
            var m = new FMatrix4x4<Fixed32>(
                new Fixed32(1),  new Fixed32(2),  new Fixed32(3),  new Fixed32(4),
                new Fixed32(5),  new Fixed32(6),  new Fixed32(7),  new Fixed32(8),
                new Fixed32(9),  new Fixed32(10), new Fixed32(11), new Fixed32(12),
                new Fixed32(13), new Fixed32(14), new Fixed32(15), new Fixed32(16));

            // Indexer maps column-major: [0]=M11, [1]=M21, [2]=M31, [3]=M41,
            // [4]=M12, [5]=M22, [6]=M32, [7]=M42,
            // [8]=M13, [9]=M23, [10]=M33, [11]=M43,
            // [12]=M14, [13]=M24, [14]=M34, [15]=M44
            Assert.Equal(1.0,  m[0].ToDouble(),  TOLERANCE);
            Assert.Equal(5.0,  m[1].ToDouble(),  TOLERANCE);
            Assert.Equal(9.0,  m[2].ToDouble(),  TOLERANCE);
            Assert.Equal(13.0, m[3].ToDouble(),  TOLERANCE);
            Assert.Equal(2.0,  m[4].ToDouble(),  TOLERANCE);
            Assert.Equal(6.0,  m[5].ToDouble(),  TOLERANCE);
            Assert.Equal(10.0, m[6].ToDouble(),  TOLERANCE);
            Assert.Equal(14.0, m[7].ToDouble(),  TOLERANCE);
            Assert.Equal(3.0,  m[8].ToDouble(),  TOLERANCE);
            Assert.Equal(7.0,  m[9].ToDouble(),  TOLERANCE);
            Assert.Equal(11.0, m[10].ToDouble(), TOLERANCE);
            Assert.Equal(15.0, m[11].ToDouble(), TOLERANCE);
            Assert.Equal(4.0,  m[12].ToDouble(), TOLERANCE);
            Assert.Equal(8.0,  m[13].ToDouble(), TOLERANCE);
            Assert.Equal(12.0, m[14].ToDouble(), TOLERANCE);
            Assert.Equal(16.0, m[15].ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Indexer_WriteElement()
        {
            var m = FMatrix4x4<Fixed32>.Identity;

            m[0] = new Fixed32(42);
            m[5] = new Fixed32(99);
            m[15] = new Fixed32(7);

            Assert.Equal(42.0, m[0].ToDouble(), TOLERANCE);
            Assert.Equal(99.0, m[5].ToDouble(), TOLERANCE);
            Assert.Equal(7.0,  m[15].ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Indexer_OutOfRange_Throws()
        {
            var m = FMatrix4x4<Fixed32>.Identity;

            Assert.Throws<IndexOutOfRangeException>(() => m[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => m[16]);
            Assert.Throws<IndexOutOfRangeException>(() => { m[-1] = Fixed32.Zero; });
            Assert.Throws<IndexOutOfRangeException>(() => { m[16] = Fixed32.Zero; });
        }

        [Fact]
        public void Indexer_IdentityDiagonal()
        {
            var identity = FMatrix4x4<Fixed32>.Identity;

            // In column-major layout, diagonal elements are at indices 0, 5, 10, 15
            Assert.Equal(1.0, identity[0].ToDouble(),  TOLERANCE);
            Assert.Equal(1.0, identity[5].ToDouble(),  TOLERANCE);
            Assert.Equal(1.0, identity[10].ToDouble(), TOLERANCE);
            Assert.Equal(1.0, identity[15].ToDouble(), TOLERANCE);
        }
    }
}
