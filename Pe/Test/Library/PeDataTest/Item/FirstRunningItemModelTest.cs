/**
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
    class FirstRunningItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new FirstRunningItemModel() {
                Timestamp = DateTime.Now,
                Version = new Version(1, 2, 3, 4)
            };

            var dst = (FirstRunningItemModel)src.DeepClone();
            Assert.AreEqual(src.Timestamp, dst.Timestamp);
            Assert.AreEqual(src.Version, dst.Version);
        }
    }
}
