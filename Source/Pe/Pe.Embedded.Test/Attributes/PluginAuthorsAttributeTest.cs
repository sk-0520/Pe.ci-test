using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Embedded.Attributes;
using Xunit;

namespace ContentTypeTextNet.Pe.Embedded.Test.Attributes
{
    public class PluginAuthorsAttributeTest
    {
        #region function

        [Fact]
        public void Constructor_name_null_Throw()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PluginAuthorsAttribute(null!, "license"));
            Assert.Equal("name", ex.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("　")]
        public void Constructor_name_Throw(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() => new PluginAuthorsAttribute(name!, "license"));
            Assert.Equal("name", ex.ParamName);
        }

        [Fact]
        public void Constructor_license_null_Throw()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PluginAuthorsAttribute("name", null!));
            Assert.Equal("license", ex.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("　")]
        public void Constructor_license_Throw(string license)
        {
            var ex = Assert.Throws<ArgumentException>(() => new PluginAuthorsAttribute("name", license!));
            Assert.Equal("license", ex.ParamName);
        }

        #endregion
    }
}
