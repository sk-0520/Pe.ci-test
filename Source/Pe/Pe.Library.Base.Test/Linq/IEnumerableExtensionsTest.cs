using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test.Linq
{
    public class IEnumerableExtensionsTest
    {
        [Fact]
        public void CountingTest()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var counting = array.Counting().ToArray();

            for(var i = 0; i < array.Length; i++) {
                Assert.Equal(i, counting[i].Number);
            }
        }

        [Fact]
        public void Counting_Base_Test()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var counting = array.Counting(99).ToArray();

            for(var i = 0; i < array.Length; i++) {
                Assert.Equal(i + 99, counting[i].Number);
            }
        }

        [Theory]
        [InlineData("a,b,c", new[] { "a", "b", "c" }, ",")]
        [InlineData("abc", new[] { "a", "b", "c" }, "")]
        [InlineData("abc", new[] { "a", "b", "c" }, null)]
        public void JoinStringTest(string expected, IEnumerable<string> source, string? separator)
        {
            var actual = source.JoinString(separator);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new[] { "a", "b", "c" }, new[] { "a", "b", "c" }, Order.Ascending)]
        [InlineData(new[] { "a", "b", "c" }, new[] { "c", "b", "a" }, Order.Ascending)]
        [InlineData(new[] { "c", "b", "a" }, new[] { "a", "b", "c" }, Order.Descending)]
        [InlineData(new[] { "c", "b", "a" }, new[] { "c", "b", "a" }, Order.Descending)]
        public void OrderByTest(IEnumerable<string> expected, IEnumerable<string> source, Order order)
        {
            var actual = source.OrderBy(order, a => a);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(false, new object[] { 1, 2, 3 })]
        [InlineData(false, new object[] { "a", "b", "c" })]
        [InlineData(true, new object[] { })]
        [InlineData(true, new object[] { "a" })]
        [InlineData(true, new object[] { "a", "a" })]
        [InlineData(false, new object?[] { "a", null })]
        [InlineData(true, new object[] { 1 })]
        [InlineData(true, new object[] { 1, 1 })]
        [InlineData(false, new object?[] { 1, null })]
        [InlineData(true, new object?[] { null })]
        [InlineData(true, new object?[] { null, null })]
        [InlineData(false, new object?[] { null, "str" })]
        [InlineData(false, new object?[] { null, 1 })]
        public void AllEqualsTest<T>(bool expected, IEnumerable<object?> source)
        {
            var actual = source.AllEquals();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AllEquals_throw_Test()
        {
            IEnumerable<object> source = null!;
            Assert.Throws<ArgumentNullException>(() => source.AllEquals());
        }
    }
}
