using System;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class LogicalIsNotNullConverterTest
    {
        #region function

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, 1)]
        [InlineData(true, 0)]
        [InlineData(true, 0.0)]
        [InlineData(true, "")]
        [InlineData(true, ' ')]
        public void ConvertTest(bool expected, object? value)
        {
            var converter = new LogicalIsNotNullConverter();
            var actual = converter.Convert(value!, value?.GetType() ?? typeof(object), null!, System.Globalization.CultureInfo.CurrentCulture);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertBackTest()
        {
            var converter = new LogicalIsNotNullConverter();
            Assert.Throws<NotSupportedException>(() => converter.ConvertBack(default!, default!, default!, System.Globalization.CultureInfo.CurrentCulture));
        }

        #endregion
    }
}
