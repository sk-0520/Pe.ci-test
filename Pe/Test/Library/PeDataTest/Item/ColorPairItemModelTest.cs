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
using NUnit.Framework;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Item
{
    [TestFixture]
    class ColorPairItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new ColorPairItemModel() {
                BackColor = Colors.Red,
                ForeColor = Colors.Green,
            };

            var dst = (ColorPairItemModel)src.DeepClone();
            Assert.AreEqual(dst.BackColor, src.BackColor);
            Assert.AreEqual(dst.ForeColor, src.ForeColor);

            src.BackColor = Colors.Pink;
            src.ForeColor = Colors.Lime;
            Assert.AreNotEqual(dst.BackColor, src.BackColor);
            Assert.AreNotEqual(dst.ForeColor, src.ForeColor);
        }
    }
}
