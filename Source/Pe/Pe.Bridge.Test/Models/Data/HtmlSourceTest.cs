using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Xunit;
using Xunit.Abstractions;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
    public class HtmlAddressTest
    {
        #region function

        [Fact]
        public void Test()
        {
            var uri = new Uri("http://localhost");
            var test = new HtmlAddress(uri);
            Assert.Equal(uri, test.Address);
            Assert.Equal(HtmlSourceKind.Address, test.HtmlSourceKind);
        }

        #endregion
    }

    public class HtmlSourceCodeTest
    {
        #region function

        [Fact]
        public void Contractor_1_Test()
        {
            var src = "<html></html>";
            var test = new HtmlSourceCode(src);
            Assert.Equal(src, test.SourceCode);
            Assert.Equal(HtmlSourceKind.SourceCode, test.HtmlSourceKind);
            Assert.Null(test.BaseAddress);
        }

        [Fact]
        public void Contractor_21_Test()
        {
            var uri = new Uri("http://localhost");
            var src = "<html></html>";
            var test = new HtmlSourceCode(src, uri);
            Assert.Equal(src, test.SourceCode);
            Assert.Equal(HtmlSourceKind.SourceCode, test.HtmlSourceKind);
            Assert.Equal(uri, test.BaseAddress);
        }

        #endregion
    }

}
