using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    public class IReadOnlyCollectionExtensions
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
