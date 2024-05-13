using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class BooleanConverterTest
    {
        #region function

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        public void Convert_bool_Test(int expected, bool value)
        {
            var test = new BooleanConverter<int>(1, 0);
            var actual = test.Convert(value, default!, default!, CultureInfo.InvariantCulture);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(false, 0)]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public void ConvertBack_bool_Test(bool expected, int value)
        {
            var test = new BooleanConverter<int>(1, 0);
            var actual = test.ConvertBack(value, default!, default!, CultureInfo.InvariantCulture);
            Assert.Equal(expected, actual);
        }

        #endregion
    }

    public class BooleanToVisibilityConverterTest
    {
        #region function

        [Theory]
        [InlineData(Visibility.Collapsed, false)]
        [InlineData(Visibility.Visible, true)]
        public void Convert_bool_Test(Visibility expected, bool value)
        {
            var test = new BooleanToVisibilityConverter();
            var actual = test.Convert(value, default!, default!, CultureInfo.InvariantCulture);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(false, Visibility.Collapsed)]
        [InlineData(true, Visibility.Visible)]
        [InlineData(false, Visibility.Hidden)]
        public void ConvertBack_bool_Test(bool expected, Visibility value)
        {
            var test = new BooleanToVisibilityConverter();
            var actual = test.ConvertBack(value, default!, default!, CultureInfo.InvariantCulture);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
