using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    public class FixedQueueTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentException>(() => new FixedQueue<int>(0));
            Assert.Throws<ArgumentException>(() => new FixedQueue<int>(-1));
            new FixedQueue<int>(1);
            Assert.True(true);
        }

        [Theory]
        [InlineData(0, 1, 0)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 5)]
        [InlineData(5, 7, 5)]
        public void EnqueueTest(int expected, int limit, int count)
        {
            var fx = new FixedQueue<int>(limit);
            foreach(var i in Enumerable.Range(0, count)) {
                fx.Enqueue(i);
            }
            Assert.Equal(expected, fx.Count);
        }

        #endregion
    }

    public class ConcurrentFixedQueueTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentException>(() => new ConcurrentFixedQueue<int>(0));
            Assert.Throws<ArgumentException>(() => new ConcurrentFixedQueue<int>(-1));
            new ConcurrentFixedQueue<int>(1);
            Assert.True(true);
        }

        [Theory]
        [InlineData(0, 1, 0)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 5)]
        [InlineData(5, 7, 5)]
        public void EnqueueTest(int expected, int limit, int count)
        {
            var fx = new ConcurrentFixedQueue<int>(limit);
            foreach(var i in Enumerable.Range(1, count)) {
                fx.Enqueue(i);
            }
            Assert.Equal(expected, fx.Count);
        }

        /*
        [Fact]
        public void ConcurrentEnqueueTest()
        {
            var limit = 100;
            var fx = new ConcurrentFixedQueue<int>(limit);
            Parallel.ForEach(Enumerable.Range(0, ushort.MaxValue), i => { fx.Enqueue(i); });
            Assert.Equal(limit, fx.Count);
        }
        */
        #endregion
    }
}
