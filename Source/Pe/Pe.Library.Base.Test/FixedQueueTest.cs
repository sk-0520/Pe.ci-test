using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class FixedQueueTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentException>(() => new FixedQueue<int>(0));
            Assert.Throws<ArgumentException>(() => new FixedQueue<int>(-1));
            var exception = Record.Exception(() => new FixedQueue<int>(1));
            Assert.Null(exception);

            var test = new FixedQueue<int>(10);
            Assert.Equal(10, test.Limit);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_throw_Test(int limit)
        {
            Assert.Throws<ArgumentException>(() => new FixedQueue<int>(limit));
        }

        [Theory]
        [InlineData(0, 1, 0)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 5)]
        [InlineData(5, 7, 5)]
        public void EnqueueTest(int expected, int limit, int count)
        {
            var test = new FixedQueue<int>(limit);
            foreach(var i in Enumerable.Range(0, count)) {
                test.Enqueue(i);
            }
            Assert.Equal(expected, test.Count);
        }

        [Fact]
        public void TryDequeueTest()
        {
            var test = new FixedQueue<int>(3);
            Assert.True(test.IsEmpty);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }
            Assert.False(test.IsEmpty);

            Assert.True(test.TryDequeue(out var result1));
            Assert.False(test.IsEmpty);
            Assert.True(test.TryDequeue(out var result2));
            Assert.False(test.IsEmpty);
            Assert.True(test.TryDequeue(out var result3));
            Assert.True(test.IsEmpty);
            Assert.False(test.TryDequeue(out var result4));

            Assert.Equal(10, result1);
            Assert.Equal(20, result2);
            Assert.Equal(30, result3);
            Assert.Equal(default, result4);
        }

        [Fact]
        public void TryPeekTest()
        {
            var test = new FixedQueue<int>(1);
            test.Enqueue(10);

            Assert.True(test.TryPeek(out var result1));
            Assert.True(test.TryPeek(out var result2));
            test.TryDequeue(out _);
            Assert.False(test.TryPeek(out var result3));

            Assert.Equal(10, result1);
            Assert.Equal(10, result2);
            Assert.Equal(default, result3);
        }

        [Fact]
        public void ClearTest()
        {
            var test = new FixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            Assert.Equal(3, test.Count);
            Assert.True(test.TryPeek(out _));

            test.Clear();
            Assert.Equal(0, test.Count);
            Assert.False(test.TryPeek(out _));
        }

        [Fact]
        public void CopyToTest()
        {
            var test = new FixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            var array = new int[5];

            Array.Fill(array, 99);
            test.CopyTo(array, 0);
            Assert.Equal(new int[] { 10, 20, 30, 99, 99 }, array);

            Array.Fill(array, 99);
            test.CopyTo(array, 1);
            Assert.Equal(new int[] { 99, 10, 20, 30, 99 }, array);

            Array.Fill(array, 99);
            test.CopyTo(array, 2);
            Assert.Equal(new int[] { 99, 99, 10, 20, 30 }, array);
        }

        [Fact]
        public void CopyTo_throw_range_Test()
        {
            var test = new FixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            var array = new int[2];
            Assert.Throws<ArgumentException>(() => test.CopyTo(array, 0));
        }

        [Fact]
        public void CopyTo_throw_index_Test()
        {
            var test = new FixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            var array = new int[2];
            Assert.Throws<ArgumentException>(() => test.CopyTo(array, 2));
        }

        [Fact]
        public void ToArrayTest()
        {
            var test = new FixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            var actual1 = test.ToArray();
            Assert.Equal(new int[] { 10, 20, 30 }, actual1);

            test.Clear();
            var actual2 = test.ToArray();
            Assert.Empty(actual2);
        }

        [Fact]
        public void GetEnumeratorTest()
        {
            var test = new FixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            int index = 0;
            foreach(var value in test) {
                Assert.Equal((++index) * 10, value);
            }
            Assert.Equal(3, index);
        }

        #endregion
    }

    public class ConcurrentConcurrentFixedQueueTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentException>(() => new ConcurrentFixedQueue<int>(0));
            Assert.Throws<ArgumentException>(() => new ConcurrentFixedQueue<int>(-1));
            var exception = Record.Exception(() => new ConcurrentFixedQueue<int>(1));
            Assert.Null(exception);

            var test = new ConcurrentFixedQueue<int>(10);
            Assert.Equal(10, test.Limit);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_throw_Test(int limit)
        {
            Assert.Throws<ArgumentException>(() => new ConcurrentFixedQueue<int>(limit));
        }

        [Theory]
        [InlineData(0, 1, 0)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 5)]
        [InlineData(5, 7, 5)]
        public void EnqueueTest(int expected, int limit, int count)
        {
            var test = new ConcurrentFixedQueue<int>(limit);
            foreach(var i in Enumerable.Range(0, count)) {
                test.Enqueue(i);
            }
            Assert.Equal(expected, test.Count);
        }

        [Fact]
        public void TryDequeueTest()
        {
            var test = new ConcurrentFixedQueue<int>(3);
            Assert.True(test.IsEmpty);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }
            Assert.False(test.IsEmpty);

            Assert.True(test.TryDequeue(out var result1));
            Assert.False(test.IsEmpty);
            Assert.True(test.TryDequeue(out var result2));
            Assert.False(test.IsEmpty);
            Assert.True(test.TryDequeue(out var result3));
            Assert.True(test.IsEmpty);
            Assert.False(test.TryDequeue(out var result4));

            Assert.Equal(10, result1);
            Assert.Equal(20, result2);
            Assert.Equal(30, result3);
            Assert.Equal(default, result4);
        }

        [Fact]
        public void TryPeekTest()
        {
            var test = new ConcurrentFixedQueue<int>(1);
            test.Enqueue(10);

            Assert.True(test.TryPeek(out var result1));
            Assert.True(test.TryPeek(out var result2));
            test.TryDequeue(out _);
            Assert.False(test.TryPeek(out var result3));

            Assert.Equal(10, result1);
            Assert.Equal(10, result2);
            Assert.Equal(default, result3);
        }

        [Fact]
        public void ClearTest()
        {
            var test = new ConcurrentFixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            Assert.Equal(3, test.Count);
            Assert.True(test.TryPeek(out _));

            test.Clear();
            Assert.Equal(0, test.Count);
            Assert.False(test.TryPeek(out _));
        }

        [Fact]
        public void CopyToTest()
        {
            var test = new ConcurrentFixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            var array = new int[5];

            Array.Fill(array, 99);
            test.CopyTo(array, 0);
            Assert.Equal(new int[] { 10, 20, 30, 99, 99 }, array);

            Array.Fill(array, 99);
            test.CopyTo(array, 1);
            Assert.Equal(new int[] { 99, 10, 20, 30, 99 }, array);

            Array.Fill(array, 99);
            test.CopyTo(array, 2);
            Assert.Equal(new int[] { 99, 99, 10, 20, 30 }, array);
        }

        [Fact]
        public void CopyTo_throw_range_Test()
        {
            var test = new ConcurrentFixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            var array = new int[2];
            Assert.Throws<ArgumentException>(() => test.CopyTo(array, 0));
        }

        [Fact]
        public void CopyTo_throw_index_Test()
        {
            var test = new ConcurrentFixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            var array = new int[2];
            Assert.Throws<ArgumentException>(() => test.CopyTo(array, 2));
        }

        [Fact]
        public void ToArrayTest()
        {
            var test = new ConcurrentFixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            var actual1 = test.ToArray();
            Assert.Equal(new int[] { 10, 20, 30 }, actual1);

            test.Clear();
            var actual2 = test.ToArray();
            Assert.Empty(actual2);
        }

        [Fact]
        public void GetEnumeratorTest()
        {
            var test = new ConcurrentFixedQueue<int>(3);
            foreach(var i in Enumerable.Range(0, test.Limit).Select(a => (a + 1) * 10)) {
                test.Enqueue(i);
            }

            int index = 0;
            foreach(var value in test) {
                Assert.Equal((++index) * 10, value);
            }
            Assert.Equal(3, index);
        }

        #endregion
    }
}
