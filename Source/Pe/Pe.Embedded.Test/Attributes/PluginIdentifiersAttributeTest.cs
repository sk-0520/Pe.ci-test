using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Embedded.Attributes;
using Xunit;

namespace ContentTypeTextNet.Pe.Embedded.Test.Attributes
{
    public class PluginIdentifiersAttributeTest
    {
        #region function

        [Theory]
        [InlineData("")]
        [InlineData("N")]
        [InlineData("D")]
        [InlineData("B")]
        [InlineData("P")]
        [InlineData("X")]
        public void ConstructorTest(string format)
        {
            var guid = Guid.NewGuid();
            var test = new PluginIdentifiersAttribute("pluginName", guid.ToString(format));
            Assert.Equal("pluginName", test.PluginName);
            Assert.Equal(guid, test.PluginId.Id);
        }

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
