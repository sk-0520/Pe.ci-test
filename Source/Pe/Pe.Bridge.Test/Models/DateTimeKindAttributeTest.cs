using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models
{
    public class DateTimeKindAttributeTest
    {
        #region function

        [Theory]
        [InlineData(DateTimeKind.Unspecified)]
        [InlineData(DateTimeKind.Utc)]
        [InlineData(DateTimeKind.Local)]
        public void Test(DateTimeKind dateTimeKind)
        {
            var test = new DateTimeKindAttribute(dateTimeKind);
            Assert.Equal(dateTimeKind, test.DateTimeKind);
        }

        #endregion
    }
}
