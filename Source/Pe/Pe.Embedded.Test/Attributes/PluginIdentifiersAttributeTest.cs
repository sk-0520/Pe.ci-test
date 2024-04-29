using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ContentTypeTextNet.Pe.Embedded.Attributes;
using Xunit;

namespace ContentTypeTextNet.Pe.Embedded.Test.Attributes
{
    public class PluginIdentifiersAttributeTest
    {
        #region function

        [Fact]
        public void Constructor_pluginName_null_Throw()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PluginIdentifiersAttribute(null!, "pluginId"));
            Assert.Equal("pluginName", ex.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("　")]
        public void Constructor_pluginName_Throw(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() => new PluginIdentifiersAttribute(name, "pluginId"));
            Assert.Equal("pluginName", ex.ParamName);
        }

        [Fact]
        public void Constructor_pluginId_null_Throw()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PluginIdentifiersAttribute("name", null!));
            Assert.Equal("pluginId", ex.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("　")]
        public void Constructor_pluginId_Throw(string pluginId)
        {
            var ex = Assert.Throws<ArgumentException>(() => new PluginIdentifiersAttribute("name", pluginId));
            Assert.Equal("pluginId", ex.ParamName);
        }


        #endregion
    }
}
