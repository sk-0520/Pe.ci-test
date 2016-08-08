using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Item
{
    [TestFixture]
    class ClipboardIndexItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new ClipboardIndexItemModel() {
                History = new HistoryItemModel() {
                    CreateTimestamp = DateTime.MaxValue,
                    UpdateTimestamp = DateTime.Now,
                    UpdateCount = 999,
                },
                Id = Guid.NewGuid(),
                Name = "name",
                Hash = new HashItemModel() {
                    Code = new byte[] { 1, 2, 3, 4},
                    Type = HashType.SHA1,
                },
                Sort = DateTime.UtcNow,
                Type = ClipboardType.All,
            };

            var dst = (ClipboardIndexItemModel)src.DeepClone();

            Assert.IsTrue(src.History.CreateTimestamp == dst.History.CreateTimestamp);
            Assert.IsTrue(src.History.UpdateTimestamp == dst.History.UpdateTimestamp);
            Assert.IsTrue(src.History.UpdateCount == dst.History.UpdateCount);
            Assert.IsTrue(src.Id == dst.Id);
            Assert.IsTrue(src.Name == dst.Name);
            Assert.IsTrue(src.Hash.Code.SequenceEqual(dst.Hash.Code));
            Assert.IsTrue(src.Hash.Type == dst.Hash.Type);
            Assert.IsTrue(src.Sort == dst.Sort);
            Assert.IsTrue(src.Type == dst.Type);
        }
    }
}
