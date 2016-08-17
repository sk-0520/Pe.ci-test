using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Item
{
    [TestFixture]
    class EnvironmentVariablesItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new EnvironmentVariablesItemModel() {
                Edit = true,
            };
            src.Update.InitializeRange(new[] { new EnvironmentVariableUpdateItemModel() { Id = "id", Value = "value" } });
            src.Remove.InitializeRange(new[] { "a", "b", "c" });

            var dst = (EnvironmentVariablesItemModel)src.DeepClone();

            Assert.IsTrue(src.Edit == dst.Edit);
            for(var i =0; i < src.Update.Count; i++) {
                var srcItem = src.Update[i];
                var dstItem = dst.Update[i];

                Assert.IsTrue(srcItem.Id == dstItem.Id);
                Assert.IsTrue(srcItem.Value == dstItem.Value);
            }
            Assert.IsTrue(src.Update != dst.Update);
            Assert.IsTrue(src.Remove.SequenceEqual(dst.Remove));
        }
    }
}
