using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Html;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Html
{
    public class HtmlElementTest
    {
        #region function

        [Fact]
        public void SimpleInlineTest()
        {
            var html = new HtmlDocument();
            var element = html.CreateElement("element");
            element.IsVoid = false;
            element.IsInline = true;
            var actual = element.Output();
            Assert.Equal("<element></element>", actual);
        }

        [Fact]
        public void SimpleBlockTest()
        {
            var html = new HtmlDocument();
            var element = html.CreateElement("element");
            element.IsVoid = false;
            element.IsInline = false;
            var actual = element.Output();
            Assert.Equal(@"<element>
</element>
", actual);
        }

        [Fact]
        public void SimpleAvoidBlockTest()
        {
            var html = new HtmlDocument();
            var element = html.CreateElement("element");
            element.IsVoid = true;
            element.IsInline = false;
            var actual = element.Output();
            Assert.Equal(@"<element>
", actual);
        }

        [Fact]
        public void SimpleAvoidInlineTest()
        {
            var html = new HtmlDocument();
            var element = html.CreateElement("element");
            element.IsVoid = true;
            element.IsInline = true;
            var actual = element.Output();
            Assert.Equal(@"<element>", actual);
        }

        [Fact]
        public void TextTest()
        {
            var html = new HtmlDocument();
            var element = html.CreateElement("element");
            element.IsVoid = false;
            element.IsInline = true;
            element.Append(html.CreateTextNode("text"));
            var actual = element.Output();
            Assert.Equal(@"<element>text</element>", actual);
        }

        [Fact]
        public void TextEntityReferencesTest()
        {
            var html = new HtmlDocument();
            var element = html.CreateElement("element");
            element.IsVoid = false;
            element.IsInline = true;
            element.Append(html.CreateTextNode("<t&e'x\"t >"));
            var actual = element.Output();
            Assert.Equal(@"<element>&lt;t&amp;e&apos;x&quot;t&nbsp;&gt;</element>", actual);
        }

        [Fact]
        public void Attribute_KV_Test()
        {
            var html = new HtmlDocument();
            var element = html.CreateElement("element");
            element.IsVoid = false;
            element.IsInline = true;
            element.Attributes["key"] = "value";
            var actual = element.Output();
            Assert.Equal("<element key=\"value\"></element>", actual);
        }

        [Fact]
        public void Attribute_KeyOnly_Test()
        {
            var html = new HtmlDocument();
            var element = html.CreateElement("element");
            element.IsVoid = false;
            element.IsInline = true;
            element.Attributes["key"] = "";
            var actual = element.Output();
            Assert.Equal("<element key></element>", actual);
        }

        #endregion
    }
}
