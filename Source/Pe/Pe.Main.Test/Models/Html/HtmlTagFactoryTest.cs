using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Main.Models.Html;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Html
{
    public class HtmlTagFactoryTest
    {
        #region function

        [Fact]
        public void MappingTest()
        {
            var html = new HtmlDocument();
            var factory = new HtmlTagFactory(html);
            var po = new PrivateObject(factory);
            var map = po.GetProperty("TagElementMap") as Dictionary<string, Type>;
            Debug.Assert(map != null);
            var classTypes = map.Values.Distinct().ToArray();
            var assembly = Assembly.GetAssembly(typeof(ContentTypeTextNet.Pe.Main.App));
            var classNames = assembly!.GetTypes().Where(a => a.Namespace == "ContentTypeTextNet.Pe.Main.Models.Html.TagElement").ToArray();
            Assert.Equal(classTypes.Length, classNames.Length);
            foreach(var className in classNames) {
                Assert.Contains(className, classTypes);
            }
        }

        [Fact]
        public void CreateTree_Single_Test()
        {
            var html = new HtmlDocument();
            var factory = new HtmlTagFactory(html);
            html.Body.AppendChild(
                factory.CreateTree(
                    "elm",
                    factory.CreateTree(
                        "child"
                    )
                )
            );

            var elm = Assert.IsType<HtmlTagElement>(html.Body.Children[0]);
            Assert.Equal("elm", elm.TagName);

            var child = Assert.IsType<HtmlTagElement>(elm.Children[0]);
            Assert.Equal("child", child.TagName);
        }

        [Fact]
        public void CreateTree_Multi_Test()
        {
            var html = new HtmlDocument();
            var factory = new HtmlTagFactory(html);
            html.Body.AppendChild(
                factory.CreateTree(
                    "elm",
                    [
                        factory.CreateTree(
                            "child1"
                        ),
                        factory.CreateTree(
                            "child2"
                        )
                    ]
                )
            );

            var elm = Assert.IsType<HtmlTagElement>(html.Body.Children[0]);
            Assert.Equal("elm", elm.TagName);

            var child1 = Assert.IsType<HtmlTagElement>(elm.Children[0]);
            Assert.Equal("child1", child1.TagName);

            var child2 = Assert.IsType<HtmlTagElement>(elm.Children[1]);
            Assert.Equal("child2", child2.TagName);
        }

        #endregion
    }
}
