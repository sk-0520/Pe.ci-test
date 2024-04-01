using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    public class SizeConverterTest
    {
        #region function

        [Theory]
        [InlineData("0 byte", 0, "{0} {1}", new[] { "byte" })]
        [InlineData("0.00 byte", 0, "{0:0.00} {1}", new[] { "byte" })]
        [InlineData("1023.00 byte", 1023, "{0:0.00} {1}", new[] { "byte", "KB" })]
        [InlineData("1.00 KB", 1024, "{0:0.00} {1}", new[] { "byte", "KB" })]
        public void ConvertHumanReadableByteTest(string expected, long byteSize, string sizeFormat, IReadOnlyList<string> units)
        {
            var sc = new SizeConverter();
            var actual = sc.ConvertHumanReadableByte(byteSize, sizeFormat, units);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
