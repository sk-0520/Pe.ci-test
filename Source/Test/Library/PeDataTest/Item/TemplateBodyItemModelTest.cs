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
    class TemplateBodyItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new TemplateBodyItemModel() {
                PreviousVersion = new Version(1, 2, 3, 4),
                History = new HistoryItemModel() {
                    CreateTimestamp = DateTime.MaxValue,
                    UpdateTimestamp = DateTime.UtcNow,
                    UpdateCount = 123,
                },
                Source = "source",
            };

            var dst = (TemplateBodyItemModel)src.DeepClone();

            Assert.IsTrue(src.PreviousVersion == dst.PreviousVersion);
            Assert.IsTrue(src.History.CreateTimestamp == dst.History.CreateTimestamp);
            Assert.IsTrue(src.History.UpdateTimestamp == dst.History.UpdateTimestamp);
            Assert.IsTrue(src.History.UpdateCount == dst.History.UpdateCount);
            Assert.IsTrue(src.Source == dst.Source);
        }
    }
}
