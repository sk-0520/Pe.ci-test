using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test.Linq
{
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
