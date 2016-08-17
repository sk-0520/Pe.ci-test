using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting
{
    [TestFixture]
    class LauncherItemSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new LauncherItemSettingModel() {
                FileDropMode = LauncherItemFileDropMode.ArgumentExecute,
            };

            var dst = (LauncherItemSettingModel)src.DeepClone();

            Assert.IsTrue(src.FileDropMode == dst.FileDropMode);
            Assert.IsTrue(src.Items != dst.Items);
        }
    }
}
