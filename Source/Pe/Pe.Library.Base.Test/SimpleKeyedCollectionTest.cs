using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class SimpleKeyedCollectionTest
    {
        #region function

        [Fact]
        public void Constructor_1_Test()
        {
            var test = new SimpleKeyedCollection<int, string>(a => int.Parse(a, CultureInfo.InvariantCulture)) {
                "123"
            };
            Assert.True(test.TryGetValue(123, out var actual));
            Assert.Equal("123", actual);
        }

        #endregion
    }
}
