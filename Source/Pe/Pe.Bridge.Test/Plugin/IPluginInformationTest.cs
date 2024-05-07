using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Plugin
{
    public class PluginIdentifiersTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var pluginId = PluginId.NewId();
            var test = new PluginIdentifiers(pluginId, "name");

            Assert.Equal(pluginId, test.PluginId);
            Assert.Equal("name", test.PluginName);
        }


        [Fact]
        public void ToStringTest()
        {
            var pluginId = PluginId.NewId();
            var test = new PluginIdentifiers(pluginId, "name");
            var actual = test.ToString();
            Assert.Equal($"name({pluginId})", actual);
        }
        #endregion
    }
}
