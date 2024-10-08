using System;
using System.Text;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test.Linq
{
    public class IEnumerableNonGenericsExtensionsTest
    {
        #region function

        [Fact]
        public void NonGenericsAny_normal_Test()
        {
            var input = new[] { 10, 20, 30 };
            var actual = input.NonGenericsAny();
            Assert.True(actual);
        }

        [Fact]
        public void NonGenericsAny_empty_Test()
        {
            var input = Array.Empty<int>();
            var actual = input.NonGenericsAny();
            Assert.False(actual);
        }

        [Fact]
        public void NonGenericsAny_predicate_30_Test()
        {
            var input = new[] { 10, 20, 30 };
            var actual = input.NonGenericsAny(a => 20 < (int)a!);
            Assert.True(actual);
        }

        [Fact]
        public void NonGenericsAny_predicate_empty_Test()
        {
            var input = new[] { 10, 20, 30 };
            var actual = input.NonGenericsAny(a => (int)a! <= 0);
            Assert.False(actual);
        }

        #endregion
    }
}
