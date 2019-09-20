using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting.MainSettings
{
    [TestFixture]
    class CommandSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new CommandSettingModel() {
                FindFile = true,
                FindTag = true,
                Font = new FontModel() {
                    Bold = true,
                    Family = "$",
                    Italic = true,
                    Size = 9.9,
                },
                HideTime = TimeSpan.MaxValue,
                IconScale = IconScale.Large,
                ShowHotkey = new HotKeyModel() {
                    Key = Key.Enter,
                    ModifierKeys = ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift,
                },
                WindowWidth = 777,
            };

            var dst = (CommandSettingModel)src.DeepClone();

            Assert.IsTrue(src.FindFile == dst.FindFile);
            Assert.IsTrue(src.FindTag == dst.FindTag);
            Assert.IsTrue(src.Font.Bold == dst.Font.Bold);
            Assert.IsTrue(src.Font.Family == dst.Font.Family);
            Assert.IsTrue(src.Font.Italic == dst.Font.Italic);
            Assert.IsTrue(src.Font.Size == dst.Font.Size);
            Assert.IsTrue(src.HideTime == dst.HideTime);
            Assert.IsTrue(src.IconScale == dst.IconScale);
            Assert.IsTrue(src.ShowHotkey.Key == dst.ShowHotkey.Key);
            Assert.IsTrue(src.ShowHotkey.ModifierKeys == dst.ShowHotkey.ModifierKeys);
            Assert.IsTrue(src.WindowWidth == dst.WindowWidth);
        }
    }
}
