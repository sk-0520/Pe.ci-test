using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
    public class HitValueTest
    {
        #region function

        [Fact]
        public void Test()
        {
            var test = new HitValue("value", true);
            Assert.Equal("value", test.Value);
            Assert.True(test.IsHit);
        }

        #endregion
    }
}
