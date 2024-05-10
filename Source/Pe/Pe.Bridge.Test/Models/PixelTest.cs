using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models
{
    public class PixelKindAttributeTest
    {
        #region function

        [Theory]
        [InlineData(Px.Unknown)]
        [InlineData(Px.Device)]
        [InlineData(Px.Logical)]
        public void Test(Px px)
        {
            var test = new PixelKindAttribute(px);
            Assert.Equal(px, test.Px);
        }

        #endregion
    }
}
