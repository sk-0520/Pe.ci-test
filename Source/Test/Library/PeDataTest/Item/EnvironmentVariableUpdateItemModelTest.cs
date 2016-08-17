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
    class EnvironmentVariableUpdateItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new EnvironmentVariableUpdateItemModel() {
                Id = "id",
                Value = "value",
            };

            var dst = (EnvironmentVariableUpdateItemModel)src.DeepClone();

            Assert.IsTrue(src.Id == dst.Id);
            Assert.IsTrue(src.Value == dst.Value);
        }
    }
}
