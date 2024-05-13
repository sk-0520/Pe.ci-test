using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class EnumToBooleanConverterTest
    {
        #region define

        public enum TestEnum
        {
            A,
            B,
            C,
        }

        #endregion

        #region function

        [Theory]
        [InlineData(true, TestEnum.A, TestEnum.A)]
        [InlineData(false, TestEnum.A, TestEnum.B)]
        [InlineData(false, TestEnum.A, TestEnum.C)]
        [InlineData(false, TestEnum.B, TestEnum.A)]
        [InlineData(true, TestEnum.B, TestEnum.B)]
        [InlineData(false, TestEnum.B, TestEnum.C)]
        [InlineData(false, TestEnum.C, TestEnum.A)]
        [InlineData(false, TestEnum.C, TestEnum.B)]
        [InlineData(true, TestEnum.C, TestEnum.C)]
        public void ConvertTest(bool expected, TestEnum value, TestEnum parameter)
        {
            var test = new EnumToBooleanConverter();

            var actual = test.Convert(value, value.GetType(), parameter, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_null_Test()
        {
            var test = new EnumToBooleanConverter();

            var actual = test.Convert(null!, default!, default!, CultureInfo.InvariantCulture);

            Assert.Equal(false, actual);
        }

        [Theory]
        [InlineData(TestEnum.A, TestEnum.A)]
        [InlineData(TestEnum.B, TestEnum.B)]
        [InlineData(TestEnum.C, TestEnum.C)]
        public void ConvertBackTest(TestEnum expected, TestEnum parameter)
        {
            var test = new EnumToBooleanConverter();

            var actual = test.ConvertBack(true, true.GetType(), parameter, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(TestEnum.A)]
        [InlineData(TestEnum.B)]
        [InlineData(TestEnum.C)]
        public void ConvertBack_false_Test(TestEnum parameter)
        {
            var test = new EnumToBooleanConverter();

            var actual = test.ConvertBack(false, false.GetType(), parameter, CultureInfo.InvariantCulture);

            Assert.Equal(Binding.DoNothing, actual);
        }

        #endregion
    }
}
