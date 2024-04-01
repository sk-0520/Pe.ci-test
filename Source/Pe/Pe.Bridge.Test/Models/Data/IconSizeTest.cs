using System;
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
            Assert.Throws<ArgumentException>(() => new IconSize(-1));
            Assert.Throws<ArgumentException>(() => new IconSize(0));
            try {
                new IconSize(1, 1);
                new IconSize(1);
                Assert.True(true);
            } catch {
                Assert.True(false);
            }
        }

    }
}
