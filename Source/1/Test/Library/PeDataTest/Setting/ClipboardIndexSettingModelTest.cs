using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting
{
    [TestFixture]
    class ClipboardIndexSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new ClipboardIndexSettingModel() {
            };

            var dst = (ClipboardIndexSettingModel)src.DeepClone();

            Assert.IsTrue(src.Items != dst.Items);
        }
    }
}
