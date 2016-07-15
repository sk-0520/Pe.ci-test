using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting
{
    [TestFixture]
    class LauncherGroupSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new LauncherGroupSettingModel();
            src.Groups.Add(new Pe.Library.PeData.Item.LauncherGroupItemModel() { Name = "name", });

            var dst = (LauncherGroupSettingModel)src.DeepClone();

            Assert.IsTrue(src.Groups != dst.Groups);
            for(var i = 0; i < src.Groups.Count; i++) {
                var s = src.Groups[i];
                var d = dst.Groups[i];
                Assert.True(s.GroupIconColor == d.GroupIconColor);
            }
        }
    }
}
