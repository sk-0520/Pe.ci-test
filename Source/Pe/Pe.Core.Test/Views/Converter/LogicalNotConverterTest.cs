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
    public class LogicalNotConverterTest
    {
        #region function

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void ConvertTest(bool expected, bool value)
        {
            var test = new LogicalNotConverter();

            var actual = test.Convert(value, typeof(bool), default!, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void ConvertBackTest(bool expected, bool value)
        {
            var test = new LogicalNotConverter();

            var actual = test.ConvertBack(value, typeof(bool), default!, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
