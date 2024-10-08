using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class UniqueKeyPoolTest
    {
        #region function

        [Fact]
        public void GetTest()
        {
            var test = new UniqueKeyPool();
            var a = test.Get();
            var b = test.Get();
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Get_eq_Test()
        {
            var test = new UniqueKeyPool();
            var a = test.Get("ABCD", 1234);
            var b = test.Get("ABCD", 1234);
            Assert.Equal(a, b);
        }

        #endregion
    }
}
