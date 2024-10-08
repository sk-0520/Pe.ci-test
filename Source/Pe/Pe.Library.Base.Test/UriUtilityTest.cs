using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class UriUtilityTest
    {
        #region function

        [Fact]
        public void CombinePath_Exception_Test()
        {
            Assert.Throws<ArgumentNullException>(() => UriUtility.CombinePath(null!, ""));
            Assert.Throws<ArgumentNullException>(() => UriUtility.CombinePath(new Uri("http://aaa.bbb"), null!));
            Assert.Throws<ArgumentNullException>(() => UriUtility.CombinePath(new Uri("http://aaa.bbb"), "", null!));
            Assert.Throws<ArgumentNullException>(() => UriUtility.CombinePath(new Uri("http://aaa.bbb"), "", "", null!));
        }

        [Theory]
        [InlineData("http://a.b/c", "http://a.b", "c")]
        [InlineData("http://a.b:8080/c", "http://a.b:8080", "c")]
        [InlineData("http://u:p@a.b/c", "http://u:p@a.b", "c")]
        [InlineData("http://u:p@a.b:8080/c", "http://u:p@a.b:8080", "c")]
        [InlineData("http://a.b/c", "http://a.b", "c/")]
        [InlineData("http://a.b/c", "http://a.b", "/c/")]
        [InlineData("http://a.b/c", "http://a.b", "//c//")]
        [InlineData("http://a.b/c", "http://a.b/", "//c//")]
        [InlineData("http://a.b/c/d", "http://a.b/c", "//d//")]
        [InlineData("http://a.b/c/d", "http://a.b/c/", "//d//")]
        [InlineData("http://a.b/c/d?q=v", "http://a.b/c?q=v", "//d//")]
        [InlineData("http://a.b/c/d/e/f/g", "http://a.b/", "//c//", "d", "/e", "f/", "/g/")]
        [InlineData("http://a.b/x/y/z/c/d/e/f/g", "http://a.b/x/y/z", "//c//", "d", "/e", "f/", "/g/")]
        [InlineData("http://a.b/x/y/z/c/d/e/f/g", "http://a.b/x/y/z/", "//c//", "d", "/e", "f/", "/g/")]
        public void CombinePathTest(string expected, string url, string path, params string[] paths)
        {
            var uri = UriUtility.CombinePath(new Uri(url), path, paths);
            Assert.Equal(expected, uri.ToString());
        }

        [Theory]
        [InlineData("http://a.b/c/", "http://a.b", "c")]
        [InlineData("http://a.b:8080/c/", "http://a.b:8080", "c")]
        [InlineData("http://u:p@a.b/c/", "http://u:p@a.b", "c")]
        [InlineData("http://u:p@a.b:8080/c/", "http://u:p@a.b:8080", "c")]
        [InlineData("http://a.b/c/", "http://a.b", "c/")]
        [InlineData("http://a.b/c/", "http://a.b", "/c/")]
        [InlineData("http://a.b/c/", "http://a.b", "//c//")]
        [InlineData("http://a.b/c/", "http://a.b/", "//c//")]
        [InlineData("http://a.b/c/d/", "http://a.b/c", "//d//")]
        [InlineData("http://a.b/c/d/", "http://a.b/c/", "//d//")]
        [InlineData("http://a.b/c/d/?q=v", "http://a.b/c?q=v", "//d//")]
        [InlineData("http://a.b/c/d/e/f/g/", "http://a.b/", "//c//", "d", "/e", "f/", "/g/")]
        [InlineData("http://a.b/x/y/z/c/d/e/f/g/", "http://a.b/x/y/z", "//c//", "d", "/e", "f/", "/g/")]
        [InlineData("http://a.b/x/y/z/c/d/e/f/g/", "http://a.b/x/y/z/", "//c//", "d", "/e", "f/", "/g/")]
        public void CombinePath_appendLastSeparator_Test(string expected, string url, string path, params string[] paths)
        {
            var uri = UriUtility.CombinePath(new Uri(url), true, path, paths);
            Assert.Equal(expected, uri.ToString());
        }

        #endregion
    }

}
