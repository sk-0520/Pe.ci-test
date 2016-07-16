/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting.MainSettings
{
    [TestFixture]
    class ClipboardSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new ClipboardSettingModel() {
                CaptureType = Pe.Library.PeData.Define.ClipboardType.All,
                DoubleClickBehavior = Pe.Library.PeData.Define.IndexItemsDoubleClickBehavior.Send,
                DuplicationCount = 2,
                DuplicationMoveHead = true,
                Font = new ContentTypeTextNet.Library.SharedLibrary.Model.FontModel() {
                    Bold = true,
                    Family = "#",
                    Italic = true,
                    Size = 9.9,
                },
                IsEnabled = true,
                IsEnabledApplicationCopy = true,
                IsTopmost = true,
                IsVisible = true,
                ItemsListWidth = 90,
                LimitSize = new ClipboardLimitSizeItemModel() {
                    Html = 777,
                    ImageHeight = 9,
                    ImageWidth = 1,
                    LimitType = ClipboardType.All,
                    Rtf = 666,
                    Text = 555,
                },
                SaveCount = 123,
                ToggleHotKey = new HotKeyModel() {
                    Key = Key.A,
                    ModifierKeys = ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Windows,
                },
                UsingClipboard = true,
                WaitTime = TimeSpan.FromMinutes(1),
                WindowHeight = 1.5,
                WindowLeft = 2.5,
                WindowTop = 3.5,
                WindowWidth = 4.5,
                WindowState = System.Windows.WindowState.Maximized,
            };

            var dst = (ClipboardSettingModel)src.DeepClone();

            Assert.IsTrue(src.CaptureType == dst.CaptureType);
            Assert.IsTrue(src.DoubleClickBehavior == dst.DoubleClickBehavior);
            Assert.IsTrue(src.DuplicationCount == dst.DuplicationCount);
            Assert.IsTrue(src.DuplicationMoveHead == dst.DuplicationMoveHead);
            Assert.IsTrue(src.Font.Bold == dst.Font.Bold);
            Assert.IsTrue(src.Font.Family == dst.Font.Family);
            Assert.IsTrue(src.Font.Italic == dst.Font.Italic);
            Assert.IsTrue(src.Font.Size == dst.Font.Size);
            Assert.IsTrue(src.IsEnabled == dst.IsEnabled);
            Assert.IsTrue(src.IsEnabledApplicationCopy == dst.IsEnabledApplicationCopy);
            Assert.IsTrue(src.IsTopmost == dst.IsTopmost);
            Assert.IsTrue(src.IsVisible == dst.IsVisible);
            Assert.IsTrue(src.ItemsListWidth == dst.ItemsListWidth);
            Assert.IsTrue(src.LimitSize.Html == dst.LimitSize.Html);
            Assert.IsTrue(src.LimitSize.ImageHeight == dst.LimitSize.ImageHeight);
            Assert.IsTrue(src.LimitSize.ImageWidth == dst.LimitSize.ImageWidth);
            Assert.IsTrue(src.LimitSize.LimitType == dst.LimitSize.LimitType);
            Assert.IsTrue(src.LimitSize.Rtf == dst.LimitSize.Rtf);
            Assert.IsTrue(src.LimitSize.Text == dst.LimitSize.Text);
            Assert.IsTrue(src.SaveCount == dst.SaveCount);
            Assert.IsTrue(src.ToggleHotKey.Key == dst.ToggleHotKey.Key);
            Assert.IsTrue(src.ToggleHotKey.ModifierKeys == dst.ToggleHotKey.ModifierKeys);
            Assert.IsTrue(src.UsingClipboard == dst.UsingClipboard);
            Assert.IsTrue(src.WaitTime == dst.WaitTime);
            Assert.IsTrue(src.WindowHeight == dst.WindowHeight);
            Assert.IsTrue(src.WindowLeft == dst.WindowLeft);
            Assert.IsTrue(src.WindowTop == dst.WindowTop);
            Assert.IsTrue(src.WindowWidth == dst.WindowWidth);
            Assert.IsTrue(src.WindowState == dst.WindowState);
        }
    }
}
