using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class DialogFilterItemTest
    {
        #region function

        [Theory]
        [InlineData("a", "d", new [] { "x", "y", "z" })]
        public void Constructor_IEnumerable_Test(string display, string defaultExtension, IEnumerable<string> wildcards)
        {
            var test = new DialogFilterItem(display, defaultExtension, wildcards);
            Assert.Equal(display, test.Display);
            Assert.Equal(defaultExtension, test.DefaultExtension);
            Assert.Equal(wildcards, test.Wildcards);
        }

        [Theory]
        [InlineData("a", "d", "*", "x", "y", "z")]
        [InlineData("a", "d", "*")]
        public void Constructor_params_Test(string display, string defaultExtension, string wildcard, params string[] wildcards)
        {
            var test = new DialogFilterItem(display, defaultExtension, wildcard, wildcards);
            Assert.Equal(display, test.Display);
            Assert.Equal(defaultExtension, test.DefaultExtension);
            Assert.Equal(new[] { wildcard }.Concat(wildcards), test.Wildcards);
        }

        [Theory]
        [InlineData("a|", "a", "")]
        [InlineData("a|*", "a", "*")]
        [InlineData("text|*.txt;*.md", "text", "*.txt", "*.md")]
        public void ToStringTest(string expected, string display, params string[] wildcards)
        {
            var test = new DialogFilterItem(display, "DEFAULT", wildcards);
            var actual = test.ToString();
            Assert.Equal(expected, actual);
        }

        #endregion
    }

    public class DialogFilterListTest
    {
        #region function

        [Fact]
        public void Test()
        {
            DialogFilterList test = [
                new DialogFilterItem("text", "txt", "*.txt", "*.md"),
                new DialogFilterItem("all", "*.*", "*"),
            ];
            var actual = test.ToString();
            Assert.Equal("text|*.txt;*.md|all|*", actual);
        }

        #endregion
    }
}
