using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    public class ArrayPoolObjectTest
    {
        #region function

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(32)]
        [InlineData(64)]
        [InlineData(128)]
        [InlineData(256)]
        [InlineData(512)]
        [InlineData(1024)]
        [InlineData(2048)]
        [InlineData(4096)]
        [InlineData(8192)]
        [InlineData(16384)]
        [InlineData(32768)]
        [InlineData(65536)]
        [InlineData(131072)]
        [InlineData(262144)]
        [InlineData(524288)]
        [InlineData(1048576)]
        [InlineData(2097152)]
        [InlineData(4194304)]
        [InlineData(8388608)]
        [InlineData(16777216)]
        [InlineData(33554432)]
        public void ConstructorTest(int size)
        {
            using var array = new ArrayPoolObject<byte>(size);
            Assert.True(array.Length <= size);
        }

        #endregion
    }
}
