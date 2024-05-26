using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Xunit;
using static ContentTypeTextNet.Pe.Core.Test.Views.Converter.CompareConverterTest;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class DoubleToParameterPercentConverterTest
    {
        #region function

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(50, 100, 0.5)]
        [InlineData(100, 100, 1)]
        [InlineData(200, 100, 2)]
        public void ConvertTest(double expected, double value, double parameter)
        {
            var test = new DoubleToParameterPercentConverter();
            var actual = test.Convert(value, default!, parameter, CultureInfo.InvariantCulture);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertBackTest()
        {
            var test = new DoubleToParameterPercentConverter();

            Assert.Throws<NotSupportedException>(() => test.ConvertBack(default!, default!, default!, CultureInfo.InvariantCulture));
        }
        #endregion
    }
}
