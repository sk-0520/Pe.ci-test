using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class CompareConverterTest
    {
        #region define

        public class IntCompareConverter: CompareConverterBase<int>
        { }

        #endregion

        #region function

        [Theory]
        [InlineData(true, 0, 0, Compare.Equal)]
        [InlineData(false, 10, 0, Compare.Equal)]
        [InlineData(false, 0, 10, Compare.Equal)]
        [InlineData(false, 0, 0, Compare.NotEqual)]
        [InlineData(true, 10, 0, Compare.NotEqual)]
        [InlineData(true, 0, 10, Compare.NotEqual)]
        [InlineData(false, 0, 0, Compare.Greater)]
        [InlineData(true, 10, 0, Compare.Greater)]
        [InlineData(false, 0, 10, Compare.Greater)]
        [InlineData(true, 0, 0, Compare.GreaterEqual)]
        [InlineData(true, 10, 0, Compare.GreaterEqual)]
        [InlineData(false, 0, 10, Compare.GreaterEqual)]
        [InlineData(false, 0, 0, Compare.Less)]
        [InlineData(false, 10, 0, Compare.Less)]
        [InlineData(true, 0, 10, Compare.Less)]
        [InlineData(true, 0, 0, Compare.LessEqual)]
        [InlineData(false, 10, 0, Compare.LessEqual)]
        [InlineData(true, 0, 10, Compare.LessEqual)]
        public void Convert_int_Test(bool expected, int value, int parameter, Compare compare)
        {
            var test = new IntCompareConverter() {
                Compare = compare,
            };
            var actual = test.Convert(value, value.GetType(), parameter, CultureInfo.InvariantCulture);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_Throw_NotImplementedException_Test()
        {
            var test = new IntCompareConverter() {
                Compare = (Compare)(-1),
            };
            Assert.Throws<NotImplementedException>(() => test.Convert(0, 0.GetType(), 0, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Convert_UnsetValue_Test()
        {
            var test = new IntCompareConverter() {
                Compare = Compare.Equal,
            };
            var actual = test.Convert("0", 0.GetType(), 0, CultureInfo.InvariantCulture);
            Assert.Equal(DependencyProperty.UnsetValue, actual);
        }

        [Fact]
        public void ConvertBackTest()
        {
            var test = new IntCompareConverter();

            Assert.Throws<NotSupportedException>(() => test.ConvertBack(default!, default!, default!, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
