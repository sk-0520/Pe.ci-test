using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class SizeConverterTest
    {
        #region function

        [Theory]
        [InlineData("0 byte", 0, "{0} {1}", new[] { "byte" })]
        [InlineData("0.00 byte", 0, "{0:0.00} {1}", new[] { "byte" })]
        [InlineData("1023.00 byte", 1023, "{0:0.00} {1}", new[] { "byte", "KB" })]
        [InlineData("1.00 KB", 1024, "{0:0.00} {1}", new[] { "byte", "KB" })]
        [InlineData("1024.00 KB", 1024 * 1024, "{0:0.00} {1}", new[] { "byte", "KB" })]
        [InlineData("1048576.00 KB", 1024 * 1024 * 1024, "{0:0.00} {1}", new[] { "byte", "KB" })]
        [InlineData("1.00 MB", 1024 * 1024, "{0:0.00} {1}", new[] { "byte", "KB", "MB" })]
        [InlineData("1024.00 MB", 1024 * 1024 * 1024, "{0:0.00} {1}", new[] { "byte", "KB", "MB" })]
        public void ConvertHumanReadableByte_3_Test(string expected, long byteSize, string sizeFormat, IReadOnlyList<string> units)
        {
            var sc = new SizeConverter();
            var actual = sc.ConvertHumanReadableByte(byteSize, sizeFormat, units);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0.00 byte", 0, new[] { "byte" })]
        [InlineData("1023.00 byte", 1023, new[] { "byte", "KB" })]
        [InlineData("1.00 KB", 1024, new[] { "byte", "KB" })]
        public void ConvertHumanReadableByte_2_1_Test(string expected, long byteSize, IReadOnlyList<string> units)
        {
            var sc = new SizeConverter();
            var actual = sc.ConvertHumanReadableByte(byteSize, units);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0.00 byte", 0, "{0:0.00} {1}")]
        [InlineData("1023.00 byte", 1023, "{0:0.00} {1}")]
        [InlineData("1.00 KB", 1024, "{0:0.00} {1}")]
        public void ConvertHumanReadableByte_2_2_Test(string expected, long byteSize, string sizeFormat)
        {
            var sc = new SizeConverter();
            var actual = sc.ConvertHumanReadableByte(byteSize, sizeFormat);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("0.00 byte", 0)]
        [InlineData("1023.00 byte", 1023)]
        [InlineData("1.00 KB", 1024)]
        public void ConvertHumanReadableByte_1_Test(string expected, long byteSize)
        {
            var sc = new SizeConverter();
            var actual = sc.ConvertHumanReadableByte(byteSize);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
