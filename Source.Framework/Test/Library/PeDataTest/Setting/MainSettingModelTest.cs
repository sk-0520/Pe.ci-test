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
    class MainSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new MainSettingModel();

            var dst = (MainSettingModel)src.DeepClone();

            Assert.IsTrue(src.Clipboard != dst.Clipboard);
            Assert.IsTrue(src.Command != dst.Command);
            Assert.IsTrue(src.General != dst.General);
            Assert.IsTrue(src.Language != dst.Language);
            Assert.IsTrue(src.Logging != dst.Logging);
            Assert.IsTrue(src.Note != dst.Note);
            Assert.IsTrue(src.RunningInformation != dst.RunningInformation);
            Assert.IsTrue(src.Stream != dst.Stream);
            Assert.IsTrue(src.SystemEnvironment != dst.SystemEnvironment);
            Assert.IsTrue(src.Template != dst.Template);
            Assert.IsTrue(src.Toolbar != dst.Toolbar);
            Assert.IsTrue(src.WindowSave != dst.WindowSave);
        }
    }
}
