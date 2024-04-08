using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
    public class IconSizeTest
    {
        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentException>(() => new IconSize(-1, -1));
            Assert.Throws<ArgumentException>(() => new IconSize(0, -1));
            Assert.Throws<ArgumentException>(() => new IconSize(0, 0));
            Assert.Throws<ArgumentException>(() => new IconSize(1, 0));
            try {
                new IconSize(1, 1);
                new IconSize(1);
                Assert.True(true);
            } catch {
                Assert.True(false);
            }
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
        public void Constructor_throw_2_Test(string expectedParamName, int width,  int height)
        {
            var ex = Assert.Throws<ArgumentException>(() => new IconSize(width, height));
            Assert.Equal(expectedParamName, ex.ParamName);
        }
    }
}
