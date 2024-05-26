using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class IconBoxToHeightConverterTest
    {
        #region function

        [Theory]
        [InlineData(16, IconBox.Small)]
        [InlineData(32, IconBox.Normal)]
        [InlineData(48, IconBox.Big)]
        [InlineData(256, IconBox.Large)]
        public void ConvertTest(int expected, IconBox value)
        {
            var test = new IconBoxToHeightConverter();

            var actual = test.Convert(value, value.GetType(), default!, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertBackTest()
        {
            var test = new IconBoxToHeightConverter();

            Assert.Throws<NotSupportedException>(() => test.ConvertBack(default!, default!, default!, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
