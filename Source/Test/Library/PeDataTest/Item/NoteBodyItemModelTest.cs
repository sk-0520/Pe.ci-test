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
using ContentTypeTextNet.Pe.Library.PeData.Item;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Item
{
    [TestFixture]
    class NoteBodyItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new NoteBodyItemModel() {
                History = new HistoryItemModel() {
                    CreateTimestamp = DateTime.MaxValue,
                    UpdateTimestamp = DateTime.UtcNow,
                    UpdateCount = 123456,
                },
                PreviousVersion = new Version(9, 8, 7, 6),
                Rtf = "!!rtf!!",
                Text = "text",
            };

            var dst = (NoteBodyItemModel)src.DeepClone();

            Assert.IsTrue(src.PreviousVersion == dst.PreviousVersion);
            Assert.IsTrue(src.History.CreateTimestamp == dst.History.CreateTimestamp);
            Assert.IsTrue(src.History.UpdateTimestamp == dst.History.UpdateTimestamp);
            Assert.IsTrue(src.History.UpdateCount == dst.History.UpdateCount);
            Assert.IsTrue(src.Rtf == dst.Rtf);
            Assert.IsTrue(src.Text == dst.Text);
        }
    }
}
