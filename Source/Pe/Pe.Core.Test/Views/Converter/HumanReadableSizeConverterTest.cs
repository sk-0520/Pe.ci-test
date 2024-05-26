using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class HumanReadableSizeConverterTest
    {
        #region function

        [Theory]
        [InlineData("0.00 byte", 0, "")]
        [InlineData("0 byte", 0, "{0} {1}")]
        [InlineData("1023.00 byte", 1023, "")]
        [InlineData("1023 byte", 1023, "{0} {1}")]
        [InlineData("1.00 KB", 1024, "")]
        [InlineData("1 KB", 1024, "{0} {1}")]
        public void ConvertTest(string expected, long value, string sizeFormat)
        {
            var test = new HumanReadableSizeConverter() {
                SizeFormat = sizeFormat,
            };

            var actual = test.Convert(value, value.GetType(), default!, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertBackTest()
        {
            var test = new HumanReadableSizeConverter();

            Assert.Throws<NotSupportedException>(() => test.ConvertBack(default!, default!, default!, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
