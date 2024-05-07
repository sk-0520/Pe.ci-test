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

    public class PluginVersionsTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var versions = new {
                Plugin = new Version(50, 0),
                Minimum = new Version(10, 0),
                Maximum = new Version(100, 0)
            };
            var actual = new PluginVersions(versions.Plugin, versions.Minimum, versions.Maximum, ["a", "b", "c"]);
            Assert.Equal(versions.Plugin, actual.PluginVersion);
            Assert.Equal(versions.Minimum, actual.MinimumSupportVersion);
            Assert.Equal(versions.Maximum, actual.MaximumSupportVersion);
            Assert.Equal(["a", "b", "c"], actual.CheckUrls);
        }

        #endregion
    }

    public class PluginAuthorsTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var actual = new PluginAuthors(new Author("author"), "license");
            Assert.Equal("author", actual.PluginAuthor.Name);
            Assert.Empty(actual.PluginAuthor.Contacts);
            Assert.Equal("license", actual.PluginLicense);
        }

        #endregion
    }

    public class PluginInformationTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var plugin = new {
                Id = new PluginIdentifiers(PluginId.NewId(), "pluginName"),
                Versions = new PluginVersions(new Version(10, 0), new Version(1, 0), new Version(100, 0), ["a, b"]),
                Authors = new PluginAuthors(new Author("author"), "license"),
            };
            var actual = new PluginInformation(plugin.Id, plugin.Versions, plugin.Authors);
            Assert.Equal(plugin.Id, actual.PluginIdentifiers);
            Assert.Equal(plugin.Versions, actual.PluginVersions);
            Assert.Equal(plugin.Authors, actual.PluginAuthors);
        }

        #endregion
    }
}
