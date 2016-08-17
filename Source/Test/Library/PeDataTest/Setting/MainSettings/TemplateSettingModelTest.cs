using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting.MainSettings
{
    [TestFixture]
    class TemplateSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new TemplateSettingModel() {
                DoubleClickBehavior = IndexItemsDoubleClickBehavior.Send,
                Font = new FontModel() {
                    Bold = true,
                    Family = "@",
                    Italic = true,
                    Size = 0.1,
                },
                IsTopmost = true,
                IsVisible = true,
                ItemsListWidth = 123,
                ReplaceListWidth = 456,
                ToggleHotKey = new HotKeyModel() {
                    Key = Key.LeftAlt,
                    ModifierKeys = ModifierKeys.Alt,
                },
                WindowHeight = 1,
                WindowLeft = 2,
                WindowTop = 3,
                WindowWidth = 4,
                WindowState = WindowState.Minimized,
            };

            var dst = (TemplateSettingModel)src.DeepClone();

            Assert.IsTrue(src.DoubleClickBehavior == dst.DoubleClickBehavior);
            Assert.IsTrue(src.Font != dst.Font);
            Assert.IsTrue(src.Font.Bold == dst.Font.Bold);
            Assert.IsTrue(src.Font.Family == dst.Font.Family);
            Assert.IsTrue(src.Font.Italic == dst.Font.Italic);
            Assert.IsTrue(src.Font.Size == dst.Font.Size);
            Assert.IsTrue(src.IsTopmost == dst.IsTopmost);
            Assert.IsTrue(src.IsVisible == dst.IsVisible);
            Assert.IsTrue(src.ItemsListWidth == dst.ItemsListWidth);
            Assert.IsTrue(src.ReplaceListWidth == dst.ReplaceListWidth);
            Assert.IsTrue(src.ToggleHotKey != dst.ToggleHotKey);
            Assert.IsTrue(src.ToggleHotKey.Key == dst.ToggleHotKey.Key);
            Assert.IsTrue(src.ToggleHotKey.ModifierKeys == dst.ToggleHotKey.ModifierKeys);
            Assert.IsTrue(src.WindowHeight == dst.WindowHeight);
            Assert.IsTrue(src.WindowLeft == dst.WindowLeft);
            Assert.IsTrue(src.WindowTop == dst.WindowTop);
            Assert.IsTrue(src.WindowWidth == dst.WindowWidth);
            Assert.IsTrue(src.WindowState == dst.WindowState);
        }
    }
}
