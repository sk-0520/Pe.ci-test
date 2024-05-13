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
    public class LogicalMultiAndConverterTest
    {
        #region function

        [Theory]
        [InlineData(true, new object[] { true })]
        [InlineData(true, new object[] { true, true })]
        [InlineData(true, new object[] { true, true, true })]
        [InlineData(true, new object[] { })] // Linq.All の挙動
        [InlineData(false, new object[] { false})]
        [InlineData(false, new object[] { true, false, true })]
        public void ConvertTest(bool expected, object[] value)
        {
            var test = new LogicalMultiAndConverter();

            var actual = test.Convert(value, default!, default!, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertBackTest()
        {
            var converter = new LogicalMultiAndConverter();
            Assert.Throws<NotSupportedException>(() => converter.ConvertBack(default!, default!, default!, System.Globalization.CultureInfo.CurrentCulture));
        }

        #endregion
    }
}
