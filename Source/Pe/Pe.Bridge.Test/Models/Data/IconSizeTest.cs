using System;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
    public class IconSizeTest
    {
        [Fact]
        public void Constructor_1_Test()
        {
            var actual = new IconSize(1);
            Assert.Equal(1, actual.Width);
            Assert.Equal(1, actual.Height);
        }

        [Fact]
        public void Constructor_2_Test()
        {
            var actual = new IconSize(1, 2);
            Assert.Equal(1, actual.Width);
            Assert.Equal(2, actual.Height);
        }

        [Theory]
        [InlineData(16, 16, IconBox.Small, 1, 1)]
        [InlineData(8, 8, IconBox.Small, 0.5, 0.5)]
        [InlineData(24, 24, IconBox.Small, 1.5, 1.5)]
        [InlineData(32, 32, IconBox.Small, 2, 2)]
        [InlineData(32, 32, IconBox.Normal, 1, 1)]
        [InlineData(48, 48, IconBox.Big, 1, 1)]
        [InlineData(256, 256, IconBox.Large, 1, 1)]
        public void Constructor_3_Test(int expectedWidth, int expectedHeight, IconBox iconBox, double dpiScaleX, double dpiScaleY)
        {
            var actual = new IconSize(iconBox, new Point(dpiScaleX, dpiScaleY));
            Assert.Equal(expectedWidth, actual.Width);
            Assert.Equal(expectedHeight, actual.Height);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Constructor_throw_1_Test(int boxSize)
        {
            var ex = Assert.Throws<ArgumentException>(() => new IconSize(boxSize));
            Assert.Equal("boxSize", ex.ParamName);
        }

        [Theory]
        [InlineData("width", -1, -1)]
        [InlineData("width", 0, -1)]
        [InlineData("width", 0, 0)]
        [InlineData("height", 1, -1)]
        [InlineData("height", 1, 0)]
        public void Constructor_throw_2_Test(string expectedParamName, int width, int height)
        {
            var ex = Assert.Throws<ArgumentException>(() => new IconSize(width, height));
            Assert.Equal(expectedParamName, ex.ParamName);
        }

        [Fact]
        public void DefaultScaleTest()
        {
            Assert.Equal(1, IconSize.DefaultScale.X);
            Assert.Equal(1, IconSize.DefaultScale.Y);
        }

        [Theory]
        [InlineData(true, 1, 1)]
        [InlineData(false, 10, 1)]
        [InlineData(false, 1, 10)]
        public void IsSquareTest(bool expected, int width, int height)
        {
            var test = new IconSize(width, height);
            var actual = test.IsSquare;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(10, 1)]
        [InlineData(1, 10)]
        public void ToSizeTest(int width, int height)
        {
            var test = new IconSize(width, height);
            var actual = test.ToSize();
            Assert.Equal(actual.Width, test.Width);
            Assert.Equal(actual.Height, test.Height);
        }

        [Theory]
        [InlineData("1 x 1", 1, 1)]
        [InlineData("10 x 1", 10, 1)]
        [InlineData("1 x 10", 1, 10)]
        public void ToStringTest(string expected, int width, int height)
        {
            var test = new IconSize(width, height);
            Assert.Equal(expected, test.ToString());
        }
    }

    public class IconScaleTest
    {
        [Fact]
        public void ConstructorTest()
        {
            var actual = new IconScale(IconBox.Normal, new Point(1.5, 2.0));
            Assert.Equal(IconBox.Normal, actual.Box);
            Assert.Equal(1.5, actual.Dpi.X);
            Assert.Equal(2.0, actual.Dpi.Y);
        }

        [Theory]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(0)]
        public void Constructor_DpiX_throw_Test(double x)
        {
            var exception = Assert.Throws<ArgumentException>(() => new IconScale(IconBox.Normal, new Point(x, 1)));
            Assert.Equal("dpiScale.X", exception.ParamName);
        }

        [Theory]
        [InlineData(double.NaN)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(0)]
        public void Constructor_DpiY_throw_Test(double y)
        {
            var exception = Assert.Throws<ArgumentException>(() => new IconScale(IconBox.Normal, new Point(1, y)));
            Assert.Equal("dpiScale.Y", exception.ParamName);
        }

        [Theory]
        [InlineData(16, 16, IconBox.Small, 1, 1)]
        [InlineData(24, 24, IconBox.Small, 1.5, 1.5)]
        public void ToIconSizeTest(int expectedWidth, int expectedHeight, IconBox iconBox, double dpiScaleX, double dpiScaleY)
        {
            var test = new IconScale(iconBox, new Point(dpiScaleX, dpiScaleY));
            var actual = test.ToIconSize();
            Assert.Equal(expectedWidth, actual.Width);
            Assert.Equal(expectedHeight, actual.Height);
        }

        [Theory]
        [InlineData("IconScale: Small, 1x1 -> 16x16", IconBox.Small, 1, 1)]
        [InlineData("IconScale: Small, 1.5x1.5 -> 24x24", IconBox.Small, 1.5, 1.5)]
        [InlineData("IconScale: Normal, 2x2 -> 64x64", IconBox.Normal, 2, 2)]
        public void ToToString(string expected, IconBox iconBox, double dpiScaleX, double dpiScaleY)
        {
            var test = new IconScale(iconBox, new Point(dpiScaleX, dpiScaleY));
            var actual = test.ToString();
            Assert.Equal(expected, actual);
        }
    }
}
