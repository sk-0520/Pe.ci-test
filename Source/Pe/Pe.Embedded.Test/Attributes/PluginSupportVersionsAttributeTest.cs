using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Embedded.Attributes;
using Xunit;

namespace ContentTypeTextNet.Pe.Embedded.Test.Attributes
{
    public class PluginSupportVersionsAttributeTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var test = new PluginSupportVersionsAttribute(
                "1.2.3",
                "4.5.6",
                [
                    "a",
                    "b",
                ]
            );
            Assert.Equal(new Version(1, 2, 3), test.MinimumVersion);
            Assert.Equal(new Version(4, 5, 6), test.MaximumVersion);
            Assert.Equal(["a", "b"], test.CheckUrls);
        }

        #endregion
    }
}
