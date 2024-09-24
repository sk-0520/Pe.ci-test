using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Html;
using ContentTypeTextNet.Pe.Main.Models.Html.TagElement;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Html
{
    public class HtmlDocumentTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var html = new HtmlDocument();
            Assert.Equal(HtmlVersion.Html5, html.Version);
            Assert.IsType<HtmlHtmlTagElement>(html.Root);
            Assert.IsType<HtmlHeadTagElement>(html.Head);
            Assert.IsType<HtmlBodyTagElement>(html.Body);
        }

        [Fact]
        public void ToStringTest()
        {
            var html = new HtmlDocument();
            var actual = html.ToString();
            Assert.Equal(@"<!doctype html>
<html>
<head>
</head>
<body>
</body>
</html>
", actual);
        }

        #endregion
    }
}
