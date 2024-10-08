using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class KeepStreamTest
    {
        #region define

        int TestLength { get; } = 32;

        #endregion

        #region function

        Stream CreateStream() => new MemoryStream(Enumerable.Repeat((byte)0, TestLength).ToArray());

        [Fact]
        public void KeepStream_PositionDefaultTrue_Test()
        {
            using var mem = CreateStream();

            using(var stream = new KeepStream(mem)) {
                Assert.Equal(0, stream.Position);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17 });
                Assert.Equal(8, stream.Position);
            }

            Assert.Equal(0, mem.Position);

            using(var stream = new KeepStream(mem)) {
                Assert.Equal(0, stream.Position);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 });
                Assert.Equal(9, stream.Position);
            }
            Assert.Equal(0, mem.Position);
            Assert.Equal(mem.Length, TestLength);
        }

        [Fact]
        public void KeepStream_PositionTrue_Test()
        {
            using var mem = CreateStream();

            using(var stream = new KeepStream(mem, true)) {
                Assert.Equal(0, stream.Position);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17 });
                Assert.Equal(8, stream.Position);
            }

            Assert.Equal(0, mem.Position);

            using(var stream = new KeepStream(mem, true)) {
                Assert.Equal(0, stream.Position);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 });
                Assert.Equal(9, stream.Position);
            }
            Assert.Equal(0, mem.Position);
            Assert.Equal(mem.Length, TestLength);
        }

        [Fact]
        public void KeepStream_PositionFalse_Test()
        {
            using var mem = CreateStream();

            using(var stream = new KeepStream(mem, false)) {
                Assert.Equal(0, stream.Position);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17 });
                Assert.Equal(8, stream.Position);
            }

            Assert.Equal(8, mem.Position);

            using(var stream = new KeepStream(mem, false)) {
                Assert.Equal(8, stream.Position);
                stream.Write(new byte[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 });
                Assert.Equal(17, stream.Position);
            }
            Assert.Equal(17, mem.Position);
            Assert.Equal(mem.Length, TestLength);
        }

        #endregion
    }
}
