using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContentTypeTextNet.Pe.Standard.Models;

namespace ContentTypeTextNet.Pe.Standard.Test.Models
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
                Assert.IsFalse(c.IsCompleted);

                if(i == max) {
                    Assert.IsTrue(c.IsLast);
                } else {
                    Assert.IsFalse(c.IsLast);
                }
                i += 1;
            }

            Assert.IsTrue(counter.IsCompleted);
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
                Assert.IsFalse(c.IsCompleted);

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

            Assert.IsFalse(counter.IsCompleted);
        }

        [TestMethod]
        public void IncrementTest()
        {
            var counter = new Counter(3);

            Assert.AreEqual(1, counter.CurrentCount);
            Assert.IsTrue(counter.IsFirst);
            Assert.IsFalse(counter.IsLast);
            Assert.IsFalse(counter.IsCompleted);

            Assert.IsTrue(counter.Increment());
            Assert.AreEqual(2, counter.CurrentCount);
            Assert.IsFalse(counter.IsFirst);
            Assert.IsFalse(counter.IsLast);
            Assert.IsFalse(counter.IsCompleted);

            Assert.IsTrue(counter.Increment());
            Assert.AreEqual(3, counter.CurrentCount);
            Assert.IsFalse(counter.IsFirst);
            Assert.IsTrue(counter.IsLast);
            Assert.IsTrue(counter.IsCompleted);

            Assert.IsFalse(counter.Increment());
            Assert.IsFalse(counter.IsFirst);
            Assert.IsTrue(counter.IsLast);
            Assert.IsTrue(counter.IsCompleted);
        }

        [TestMethod]
        public void ForeachAndIncrement()
        {
            var counter = new Counter(3);
            foreach(var c in counter) {
                Assert.AreEqual(counter.CurrentCount, c.CurrentCount);
                Assert.AreEqual(counter.IsFirst, c.IsFirst);
                Assert.AreEqual(counter.IsLast, c.IsLast);
                Assert.AreEqual(counter.IsCompleted, c.IsCompleted);

                if(counter.CurrentCount == 1) {
                    Assert.IsTrue(counter.IsFirst);
                    Assert.IsFalse(counter.IsLast);
                    Assert.IsFalse(counter.IsCompleted);
                    counter.Increment();
                    Assert.AreEqual(2, counter.CurrentCount);
                    Assert.IsFalse(counter.IsFirst);
                    Assert.IsFalse(counter.IsLast);
                    Assert.IsFalse(counter.IsCompleted);
                } else {
                    Assert.AreEqual(3, counter.CurrentCount);
                    Assert.IsFalse(counter.IsFirst);
                    Assert.IsTrue(counter.IsLast);
                    Assert.IsFalse(counter.IsCompleted);
                }
            }
            Assert.IsTrue(counter.IsCompleted);
        }
    }
}
