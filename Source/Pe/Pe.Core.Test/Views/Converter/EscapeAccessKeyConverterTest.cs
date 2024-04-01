using ContentTypeTextNet.Pe.Core.Views.Converter;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class EscapeAccessKeyConverterTest
    {
        #region function

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("__", "_")]
        [InlineData("____", "__")]
        [InlineData("__a__", "_a_")]
        public void ConvertTest(string? expected, object? value)
        {
            var converter = new EscapeAccessKeyConverter();
            var actual = converter.Convert(value!, value?.GetType() ?? typeof(object), null!, System.Globalization.CultureInfo.CurrentCulture);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("_", "_")]
        [InlineData("_", "__")]
        [InlineData("__", "____")]
        [InlineData("_a_", "__a__")]
        public void ConvertBackTest(string? expected, object? value)
        {
            var converter = new EscapeAccessKeyConverter();
            var actual = converter.ConvertBack(value!, value?.GetType() ?? typeof(object), null!, System.Globalization.CultureInfo.CurrentCulture);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
