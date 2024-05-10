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
        public void ConstructorTest()
        {
            var test = new PluginAuthorsAttribute(
                "name",
                "license",
                "website",
                "projectSite",
                "email"
            );
            Assert.Equal("name", test.Name);
            Assert.Equal("license", test.License);
            Assert.Equal("website", test.Website);
            Assert.Equal("projectSite", test.ProjectSite);
            Assert.Equal("email", test.Email);
        }

        [Fact]
        public void Constructor_name_null_throw_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PluginAuthorsAttribute(null!, "license"));
            Assert.Equal("name", ex.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("　")]
        public void Constructor_name_throw_Test(string name)
        {
            var ex = Assert.Throws<ArgumentException>(() => new PluginAuthorsAttribute(name!, "license"));
            Assert.Equal("name", ex.ParamName);
        }

        [Fact]
        public void Constructor_license_null_throw_Test()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PluginAuthorsAttribute("name", null!));
            Assert.Equal("license", ex.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("　")]
        public void Constructor_license_throw_Test(string license)
        {
            var ex = Assert.Throws<ArgumentException>(() => new PluginAuthorsAttribute("name", license!));
            Assert.Equal("license", ex.ParamName);
        }

        #endregion
    }
}
