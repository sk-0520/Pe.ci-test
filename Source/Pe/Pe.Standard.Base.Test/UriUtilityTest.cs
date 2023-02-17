using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    [TestClass]
    public class UriUtilityTest
    {
        #region function

        [TestMethod]
        public void CombinePath_Exception_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() => UriUtility.CombinePath(null!, ""));
            Assert.ThrowsException<ArgumentNullException>(() => UriUtility.CombinePath(new Uri("http://aaa.bbb"), null!));
            Assert.ThrowsException<ArgumentNullException>(() => UriUtility.CombinePath(new Uri("http://aaa.bbb"), "", null!));
            Assert.ThrowsException<ArgumentNullException>(() => UriUtility.CombinePath(new Uri("http://aaa.bbb"), "", "", null!));
        }

        [TestMethod]
        [DataRow("http://a.b/c", "http://a.b", "c")]
        [DataRow("http://a.b:8080/c", "http://a.b:8080", "c")]
        [DataRow("http://u:p@a.b/c", "http://u:p@a.b", "c")]
        [DataRow("http://u:p@a.b:8080/c", "http://u:p@a.b:8080", "c")]
        [DataRow("http://a.b/c", "http://a.b", "c/")]
        [DataRow("http://a.b/c", "http://a.b", "/c/")]
        [DataRow("http://a.b/c", "http://a.b", "//c//")]
        [DataRow("http://a.b/c", "http://a.b/", "//c//")]
        [DataRow("http://a.b/c/d", "http://a.b/c", "//d//")]
        [DataRow("http://a.b/c/d", "http://a.b/c/", "//d//")]
        [DataRow("http://a.b/c/d?q=v", "http://a.b/c?q=v", "//d//")]
        [DataRow("http://a.b/c/d/e/f/g", "http://a.b/", "//c//", "d", "/e", "f/", "/g/")]
        [DataRow("http://a.b/x/y/z/c/d/e/f/g", "http://a.b/x/y/z", "//c//", "d", "/e", "f/", "/g/")]
        [DataRow("http://a.b/x/y/z/c/d/e/f/g", "http://a.b/x/y/z/", "//c//", "d", "/e", "f/", "/g/")]
        public void CombinePathTest(string expected, string url, string path, params string[] paths)
        {
            var uri = UriUtility.CombinePath(new Uri(url), path, paths);
            Assert.AreEqual(expected, uri.ToString());
        }

        [TestMethod]
        [DataRow("http://a.b/c/", "http://a.b", "c")]
        [DataRow("http://a.b:8080/c/", "http://a.b:8080", "c")]
        [DataRow("http://u:p@a.b/c/", "http://u:p@a.b", "c")]
        [DataRow("http://u:p@a.b:8080/c/", "http://u:p@a.b:8080", "c")]
        [DataRow("http://a.b/c/", "http://a.b", "c/")]
        [DataRow("http://a.b/c/", "http://a.b", "/c/")]
        [DataRow("http://a.b/c/", "http://a.b", "//c//")]
        [DataRow("http://a.b/c/", "http://a.b/", "//c//")]
        [DataRow("http://a.b/c/d/", "http://a.b/c", "//d//")]
        [DataRow("http://a.b/c/d/", "http://a.b/c/", "//d//")]
        [DataRow("http://a.b/c/d/?q=v", "http://a.b/c?q=v", "//d//")]
        [DataRow("http://a.b/c/d/e/f/g/", "http://a.b/", "//c//", "d", "/e", "f/", "/g/")]
        [DataRow("http://a.b/x/y/z/c/d/e/f/g/", "http://a.b/x/y/z", "//c//", "d", "/e", "f/", "/g/")]
        [DataRow("http://a.b/x/y/z/c/d/e/f/g/", "http://a.b/x/y/z/", "//c//", "d", "/e", "f/", "/g/")]
        public void CombinePath_appendLastSeparator_Test(string expected, string url, string path, params string[] paths)
        {
            var uri = UriUtility.CombinePath(new Uri(url), true, path, paths);
            Assert.AreEqual(expected, uri.ToString());
        }

        #endregion
    }

}
