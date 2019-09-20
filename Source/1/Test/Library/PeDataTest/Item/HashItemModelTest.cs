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
    class HashItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new HashItemModel() {
                Code = new byte[] { 1, 2, 3, 4 },
                Type = HashType.SHA1,
            };

            var dst = (HashItemModel)src.DeepClone();

            Assert.IsTrue(src.Code.SequenceEqual(dst.Code));
            Assert.IsTrue(src.Code != dst.Code);
            Assert.IsTrue(src.Type == dst.Type);
        }
    }
}
