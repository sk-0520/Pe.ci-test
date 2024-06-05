using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Database;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database
{
    public class EnumTransferAttributeTest
    {
        #region function

        [Fact]
        public void Constructor_throw_null_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new EnumTransferAttribute(null!));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_throw_empty_Test(string input)
        {
            Assert.Throws<ArgumentException>(() => new EnumTransferAttribute(input));
        }

        #endregion
    }
}
