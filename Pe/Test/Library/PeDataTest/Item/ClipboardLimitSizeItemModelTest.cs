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
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Item
{
    [TestFixture]
    class ClipboardLimitSizeItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new ClipboardLimitSizeItemModel() {
                Html = 777,
                ImageHeight = 9,
                ImageWidth = 1,
                LimitType = ClipboardType.All,
                Rtf = 666,
                Text = 555,
            };

            var dst = (ClipboardLimitSizeItemModel)src.DeepClone();

            Assert.IsTrue(src.Html == dst.Html);
            Assert.IsTrue(src.ImageHeight == dst.ImageHeight);
            Assert.IsTrue(src.ImageWidth == dst.ImageWidth);
            Assert.IsTrue(src.LimitType == dst.LimitType);
            Assert.IsTrue(src.Rtf == dst.Rtf);
            Assert.IsTrue(src.Text == dst.Text);
        }
    }
}
