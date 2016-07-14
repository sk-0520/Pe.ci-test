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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Item
{
    [TestFixture]
    class NoteIndexItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new NoteIndexItemModel() {
                History = new HistoryItemModel() {
                    CreateTimestamp = DateTime.MaxValue,
                    UpdateTimestamp = DateTime.Now,
                    UpdateCount = 999,
                },
                Id = Guid.NewGuid(),
                Name = "name",
                AutoLineFeed = true,
                BackColor = Colors.AliceBlue,
                Font = new FontModel() {
                    Bold = true,
                    Italic = true,
                    Size = 741,
                    Family = "###",
                },
                ForeColor = Colors.AntiqueWhite,
                IsCompacted = true,
                IsLocked = true,
                IsTopmost = true,
                IsVisible = true,
                NoteKind = Pe.Library.PeData.Define.NoteKind.Rtf,
                WindowHeight = 789,
                WindowLeft = 456,
                WindowTop = 123,
                WindowWidth = 0.1,
            };

            var dst = (NoteIndexItemModel)src.DeepClone();

            Assert.IsTrue(src.History.CreateTimestamp == dst.History.CreateTimestamp);
            Assert.IsTrue(src.History.UpdateTimestamp == dst.History.UpdateTimestamp);
            Assert.IsTrue(src.History.UpdateCount == dst.History.UpdateCount);
            Assert.IsTrue(src.Id == dst.Id);
            Assert.IsTrue(src.Name == dst.Name);
            Assert.IsTrue(src.AutoLineFeed == dst.AutoLineFeed);
            Assert.IsTrue(src.BackColor == dst.BackColor);
            Assert.IsTrue(src.Font.Bold == dst.Font.Bold);
            Assert.IsTrue(src.Font.Italic == dst.Font.Italic);
            Assert.IsTrue(src.Font.Size == dst.Font.Size);
            Assert.IsTrue(src.Font.Family == dst.Font.Family);
            Assert.IsTrue(src.ForeColor == dst.ForeColor);
            Assert.IsTrue(src.IsCompacted == dst.IsCompacted);
            Assert.IsTrue(src.IsLocked == dst.IsLocked);
            Assert.IsTrue(src.IsTopmost == dst.IsTopmost);
            Assert.IsTrue(src.IsVisible == dst.IsVisible);
            Assert.IsTrue(src.NoteKind == dst.NoteKind);
            Assert.IsTrue(src.WindowHeight == dst.WindowHeight);
            Assert.IsTrue(src.WindowLeft == dst.WindowLeft);
            Assert.IsTrue(src.WindowTop == dst.WindowTop);
            Assert.IsTrue(src.WindowWidth == dst.WindowWidth);
        }
    }
}
