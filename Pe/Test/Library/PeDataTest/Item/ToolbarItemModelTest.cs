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
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Item
{
    [TestFixture]
    class ToolbarItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new ToolbarItemModel() {
                AutoHide = true,
                ButtonPosition = ToolbarButtonPosition.Center,
                DefaultGroupId = Guid.NewGuid(),
                DockType = DockType.Top,
                FloatToolbar = new FloatToolbarItemModel() {
                    HeightButtonCount = 3,
                    Left = 1.1,
                    Top = 9.9,
                    WidthButtonCount = 2,
                },
                Font = new FontModel() {
                    Bold = true,
                    Italic = true,
                    Size = 3.3,
                    Family = "family",
                },
                HideAnimateTime = TimeSpan.FromDays(3),
                HideWaitTime = TimeSpan.FromHours(3),
                IconScale = IconScale.Large,
                Id = "display!",
                IsTopmost = true,
                IsVisible = true,
                IsVisibleMenuButton = true,
                MenuPositionCorrection = true,
                TextVisible = true,
                TextWidth = 99.9,
            };

            var dst = (ToolbarItemModel)src.DeepClone();

            Assert.IsTrue(src.AutoHide == dst.AutoHide);
            Assert.IsTrue(src.ButtonPosition == dst.ButtonPosition);
            Assert.IsTrue(src.DefaultGroupId == dst.DefaultGroupId);
            Assert.IsTrue(src.DockType == dst.DockType);
            Assert.IsTrue(src.FloatToolbar.HeightButtonCount == dst.FloatToolbar.HeightButtonCount);
            Assert.IsTrue(src.FloatToolbar.Left == dst.FloatToolbar.Left);
            Assert.IsTrue(src.FloatToolbar.Top == dst.FloatToolbar.Top);
            Assert.IsTrue(src.FloatToolbar.WidthButtonCount == dst.FloatToolbar.WidthButtonCount);
            Assert.IsTrue(src.Font.Bold == dst.Font.Bold);
            Assert.IsTrue(src.Font.Italic == dst.Font.Italic);
            Assert.IsTrue(src.Font.Size == dst.Font.Size);
            Assert.IsTrue(src.Font.Family == dst.Font.Family);
            Assert.IsTrue(src.HideAnimateTime == dst.HideAnimateTime);
            Assert.IsTrue(src.HideWaitTime == dst.HideWaitTime);
            Assert.IsTrue(src.IconScale == dst.IconScale);
            Assert.IsTrue(src.Id == dst.Id);
            Assert.IsTrue(src.IsTopmost == dst.IsTopmost);
            Assert.IsTrue(src.IsVisible == dst.IsVisible);
            Assert.IsTrue(src.IsVisibleMenuButton == dst.IsVisibleMenuButton);
            Assert.IsTrue(src.MenuPositionCorrection == dst.MenuPositionCorrection);
            Assert.IsTrue(src.TextVisible == dst.TextVisible);
            Assert.IsTrue(src.TextWidth == dst.TextWidth);
        }
    }
}
