using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting.MainSettings
{
    [TestFixture]
    class ToolbarSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new ToolbarSettingModel();
            src.Items.Add(new ToolbarItemModel() { Id = "id", });

            var dst = (ToolbarSettingModel)src.DeepClone();

            Assert.IsTrue(src.Items != dst.Items);
            Assert.IsTrue(src.Items[0] != dst.Items[0]);

        }
    }
}
