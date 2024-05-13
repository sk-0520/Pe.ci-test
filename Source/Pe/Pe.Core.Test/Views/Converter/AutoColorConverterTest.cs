using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    public class AutoColorConverterTest
    {
        #region function

        [Theory]
        [InlineData(
            MediaUtility.Opaque, 0xff, 0xff, 0xff,
            MediaUtility.Opaque, 0x00, 0x00, 0x00
        )]
        [InlineData(
            MediaUtility.Opaque, 0x00, 0x00, 0x00,
            MediaUtility.Opaque, 0xff, 0xff, 0xff
        )]
        public void ConvertTest(byte expectedA, byte expectedR, byte expectedG, byte expectedB, byte a, byte r, byte g, byte b)
        {
            var expected = Color.FromArgb(expectedA, expectedR, expectedG, expectedB);
            var color = Color.FromArgb(a, r, g, b);

            var test = new AutoColorConverter();

            var actual = test.Convert(color, color.GetType(), default!, CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Convert_UnsetValue_Test()
        {
            var test = new AutoColorConverter();
            var actual = test.Convert(new object(), default!, default!, CultureInfo.InvariantCulture);
            Assert.Equal(DependencyProperty.UnsetValue, actual);
        }

        [Fact]
        public void ConvertBackTest()
        {
            var test = new AutoColorConverter();

            Assert.Throws<NotSupportedException>(() => test.ConvertBack(default!, default!, default!, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
