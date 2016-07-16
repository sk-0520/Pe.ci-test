using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting.MainSettings
{
    [TestFixture]
    class WindowSaveSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new WindowSaveSettingModel() {
                IsEnabled = true,
                SaveCount = 999,
                SaveIntervalTime = TimeSpan.MaxValue,
            };

            var dst = (WindowSaveSettingModel)src.DeepClone();

            Assert.IsTrue(src.IsEnabled == dst.IsEnabled);
            Assert.IsTrue(src.SaveCount == dst.SaveCount);
            Assert.IsTrue(src.SaveIntervalTime == dst.SaveIntervalTime);
        }
    }
}
