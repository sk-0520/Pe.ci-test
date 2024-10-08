using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class CounterTest
    {
        [Fact]
        public void CountTest()
        {
            var i = 1;
            var max = 5;
            var counter = new Counter(max);
            foreach(var c in counter) {
                if(i == 1) {
                    Assert.True(c.IsFirst);
                } else {
                    Assert.False(c.IsFirst);
                }

                Assert.Equal(c.CurrentCount, i);
                Assert.False(c.IsCompleted);

                if(i == max) {
                    Assert.True(c.IsLast);
                } else {
                    Assert.False(c.IsLast);
                }
                i += 1;
            }

            Assert.True(counter.IsCompleted);
        }

        [Fact]
        public void CompleteTest()
        {
            var i = 1;
            var max = 5;
            var counter = new Counter(max);
            foreach(var c in counter) {
                if(i == 1) {
                    Assert.True(c.IsFirst);
                } else {
                    Assert.False(c.IsFirst);
                }

                Assert.Equal(c.CurrentCount, i);
                Assert.False(c.IsCompleted);

                if(i == max) {
                    Assert.True(c.IsLast);
                } else {
                    Assert.False(c.IsLast);
                }
                if(i == max - 1) {
                    break;
                }
                i += 1;
            }

            Assert.False(counter.IsCompleted);
        }

        [Fact]
        public void IncrementTest()
        {
            var counter = new Counter(3);

            Assert.Equal(1, counter.CurrentCount);
            Assert.True(counter.IsFirst);
            Assert.False(counter.IsLast);
            Assert.False(counter.IsCompleted);

            Assert.True(counter.Increment());
            Assert.Equal(2, counter.CurrentCount);
            Assert.False(counter.IsFirst);
            Assert.False(counter.IsLast);
            Assert.False(counter.IsCompleted);

            Assert.True(counter.Increment());
            Assert.Equal(3, counter.CurrentCount);
            Assert.False(counter.IsFirst);
            Assert.True(counter.IsLast);
            Assert.True(counter.IsCompleted);

            Assert.False(counter.Increment());
            Assert.False(counter.IsFirst);
            Assert.True(counter.IsLast);
            Assert.True(counter.IsCompleted);
        }

        [Fact]
        public void ForeachAndIncrement()
        {
            var counter = new Counter(3);
            foreach(var c in counter) {
                Assert.Equal(counter.CurrentCount, c.CurrentCount);
                Assert.Equal(counter.IsFirst, c.IsFirst);
                Assert.Equal(counter.IsLast, c.IsLast);
                Assert.Equal(counter.IsCompleted, c.IsCompleted);

                if(counter.CurrentCount == 1) {
                    Assert.True(counter.IsFirst);
                    Assert.False(counter.IsLast);
                    Assert.False(counter.IsCompleted);
                    counter.Increment();
                    Assert.Equal(2, counter.CurrentCount);
                    Assert.False(counter.IsFirst);
                    Assert.False(counter.IsLast);
                    Assert.False(counter.IsCompleted);
                } else {
                    Assert.Equal(3, counter.CurrentCount);
                    Assert.False(counter.IsFirst);
                    Assert.True(counter.IsLast);
                    Assert.False(counter.IsCompleted);
                }
            }
            Assert.True(counter.IsCompleted);
        }
    }
}
