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

        #endregion
    }
}
