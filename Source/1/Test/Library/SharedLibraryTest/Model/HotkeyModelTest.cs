/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Model
{
    [TestFixture]
    class HotkeyModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new HotKeyModel() {
                Key = Key.A,
                ModifierKeys = ModifierKeys.Control
            };

            var cp = (HotKeyModel)src.DeepClone();

            Assert.AreEqual(cp.Key, src.Key);
            Assert.AreEqual(cp.ModifierKeys, src.ModifierKeys);

            src.Key = Key.B;
            src.ModifierKeys = ModifierKeys.Alt | ModifierKeys.Control;

            Assert.AreNotEqual(cp.Key, src.Key);
            Assert.AreNotEqual(cp.ModifierKeys, src.ModifierKeys);
        }
    }
}
