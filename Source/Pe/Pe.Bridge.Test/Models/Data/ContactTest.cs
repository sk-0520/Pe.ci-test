using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
    public class ContactTest
    {
        #region function

        [Fact]
        public void Test()
        {
            var test = new Contact("kind", "value");
            Assert.Equal("kind", test.ContactKind);
            Assert.Equal("value", test.ContactValue);
        }

        #endregion
    }
}
