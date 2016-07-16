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
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Item
{
    [TestFixture]
    class LauncherGroupItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new LauncherGroupItemModel() {
                GroupIconColor = Colors.Red,
                GroupIconType = LauncherGroupIconType.Bookmark,
                GroupKind = GroupKind.Tag,
                Id = Guid.NewGuid(),
                Name = "name",
                LauncherItems = new CollectionModel<Guid>(new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), })
            };

            var dst = (LauncherGroupItemModel)src.DeepClone();

            Assert.IsTrue(src.GroupIconColor == dst.GroupIconColor);
            Assert.IsTrue(src.GroupIconType == dst.GroupIconType);
            Assert.IsTrue(src.GroupKind == dst.GroupKind);
            Assert.IsTrue(src.Id == dst.Id);
            Assert.IsTrue(src.Name == dst.Name);
            Assert.IsTrue(src.LauncherItems.SequenceEqual(dst.LauncherItems));
            Assert.IsTrue(src.LauncherItems != dst.LauncherItems);
        }
    }
}
