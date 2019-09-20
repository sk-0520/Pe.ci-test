using System;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
{
    [TestClass]
    public class CounterTest
    {
        [TestMethod]
        public void CountTest()
        {
            var i = 1;
            var max = 5;
            var counter = new Counter(max);
            foreach(var c in counter) {
                if(i == 1) {
                    Assert.IsTrue(c.IsFirst);
                } else {
                    Assert.IsFalse(c.IsFirst);
                }

                Assert.IsTrue(c.CurrentCount == i);
                Assert.IsFalse(c.Complete);

                if(i == max) {
                    Assert.IsTrue(c.IsLast);
                } else {
                    Assert.IsFalse(c.IsLast);
                }
                i += 1;
            }

            Assert.IsTrue(counter.Complete);
        }
        [TestMethod]
        public void CompleteTest()
        {
            var i = 1;
            var max = 5;
            var counter = new Counter(max);
            foreach(var c in counter) {
                if(i == 1) {
                    Assert.IsTrue(c.IsFirst);
                } else {
                    Assert.IsFalse(c.IsFirst);
                }

                Assert.IsTrue(c.CurrentCount == i);
                Assert.IsFalse(c.Complete);

                if(i == max) {
                    Assert.IsTrue(c.IsLast);
                } else {
                    Assert.IsFalse(c.IsLast);
                }
                if(i == max - 1) {
                    break;
                }
                i += 1;
            }

            Assert.IsFalse(counter.Complete);
        }
    }
}
