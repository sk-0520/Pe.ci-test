using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Standard.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
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
    }

    public class CollectionExtensionsTest
    {
        #region function

        [Fact]
        public void SetRange_List_Test()
        {
            var test = new List<int>() {
                1, 2, 3
            };
            test.SetRange(Enumerable.Range(10, 3));
            Assert.Equal(new[] { 10, 11, 12 }, test);
        }

        [Fact]
        public void SetRange_NotList_Test()
        {
            var test = new Collection<int>() {
                1, 2, 3
            };
            test.SetRange(Enumerable.Range(10, 3));
            Assert.Equal(new[] { 10, 11, 12 }, test);
        }

        [Fact]
        public void AddRange_NotList_Test()
        {
            var test = new Collection<int>() {
                1, 2, 3
            };
            test.AddRange(Enumerable.Range(10, 3));
            Assert.Equal(new[] { 1, 2, 3, 10, 11, 12 }, test);
        }

        #endregion
    }

    public class IReadOnlyCollectionExtensionsTest
    {
        [Fact]
        public void IndexOf_IReadOnlyList_Test()
        {
            IReadOnlyList<int> items = new[] { 10, 20, 30 }.ToList();
            var actual = items.IndexOf(20);
            Assert.Equal(1, actual);
            Assert.Equal(-1, items.IndexOf(40));
        }

        private class Test_IndexOf_IReadOnlyCollection: IReadOnlyCollection<int>
        {
            private static int[] Items { get; } = new[] { 10, 20, 30 };

            public int Count => Items.Length;

            public IEnumerator<int> GetEnumerator()
            {
                return Items.AsEnumerable().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [Fact]
        public void IndexOf_IReadOnlyCollection_Test()
        {
            IReadOnlyCollection<int> items = new Test_IndexOf_IReadOnlyCollection();
            var actual = items.IndexOf(20);
            Assert.Equal(1, actual);

            Assert.Equal(-1, items.IndexOf(40));
        }
    }
}
