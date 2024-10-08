using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class UnusedAttributeTest
    {
        #region function

        [Theory]
        [InlineData(UnusedKinds.Unknown)]
        [InlineData(UnusedKinds.Dispose)]
        [InlineData(UnusedKinds.TwoWayBinding)]
        [InlineData(UnusedKinds.Dispose | UnusedKinds.TwoWayBinding)]
        public void Test(UnusedKinds unusedKinds)
        {
            var test = new UnusedAttribute(unusedKinds);
            Assert.Equal(unusedKinds, test.Kinds);
        }

        #endregion
    }

}
