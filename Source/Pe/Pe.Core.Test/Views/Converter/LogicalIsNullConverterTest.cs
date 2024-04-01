using System;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class LogicalIsNullConverterTest
    {
        #region function

        [Theory]
        [InlineData(true, null)]
        [InlineData(false, 1)]
        [InlineData(false, 0)]
        [InlineData(false, 0.0)]
        [InlineData(false, "")]
        [InlineData(false, ' ')]
        public void ConvertTest(bool expected, object? value)
        {
            var converter = new LogicalIsNullConverter();
            var actual = converter.Convert(value!, value?.GetType() ?? typeof(object), null!, System.Globalization.CultureInfo.CurrentCulture);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertBackTest()
        {
            var converter = new LogicalIsNullConverter();
            Assert.Throws<NotSupportedException>(() => converter.ConvertBack(default!, default!, default!, System.Globalization.CultureInfo.CurrentCulture));
        }

        #endregion
    }
}
