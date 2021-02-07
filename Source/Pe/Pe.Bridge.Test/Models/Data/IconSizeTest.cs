using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
    [TestClass]
    public class IconSizeTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentException>(() => new IconSize(-1, -1));
            Assert.ThrowsException<ArgumentException>(() => new IconSize(0, -1));
            Assert.ThrowsException<ArgumentException>(() => new IconSize(0, 0));
            Assert.ThrowsException<ArgumentException>(() => new IconSize(1, 0));
            Assert.ThrowsException<ArgumentException>(() => new IconSize(-1));
            Assert.ThrowsException<ArgumentException>(() => new IconSize(0));
            try {
                new IconSize(1, 1);
                new IconSize(1);
                Assert.IsTrue(true);
            } catch {
                Assert.IsTrue(false);
            }
        }

    }
}
