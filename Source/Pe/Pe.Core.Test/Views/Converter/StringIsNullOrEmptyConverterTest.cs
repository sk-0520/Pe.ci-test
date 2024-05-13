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
    public class StringIsNullOrEmptyConverterTest
    {
        #region function

        [Theory]
        [InlineData(true, null)]
        [InlineData(true, "")]
        [InlineData(false, " ")]
        [InlineData(false, "a")]
        [InlineData(false, " a ")]
        public void ConvertTest(bool expected, string? value)
        {
            var test = new StringIsNullOrEmptyConverter();

            var actual = test.Convert(value!, typeof(string), default!, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertBackTest()
        {
            var converter = new StringIsNullOrEmptyConverter();
            Assert.Throws<NotSupportedException>(() => converter.ConvertBack(default!, default!, default!, System.Globalization.CultureInfo.CurrentCulture));
        }

        #endregion
    }
}
