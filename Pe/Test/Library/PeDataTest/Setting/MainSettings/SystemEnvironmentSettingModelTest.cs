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
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting.MainSettings
{
    [TestFixture]
    class SystemEnvironmentSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new SystemEnvironmentSettingModel() {
                ExtensionHotkey = new HotKeyModel() {
                    Key = Key.A,
                    ModifierKeys = ModifierKeys.Alt,
                },
                HideFileHotkey = new HotKeyModel() {
                    Key = Key.B,
                    ModifierKeys = ModifierKeys.Shift,
                },
                SuppressFunction1Key = true,
            };

            var dst = (SystemEnvironmentSettingModel)src.DeepClone();

            Assert.IsTrue(src.ExtensionHotkey!= dst.ExtensionHotkey);
            Assert.IsTrue(src.ExtensionHotkey.Key == dst.ExtensionHotkey.Key);
            Assert.IsTrue(src.ExtensionHotkey.ModifierKeys == dst.ExtensionHotkey.ModifierKeys);
            Assert.IsTrue(src.HideFileHotkey != dst.HideFileHotkey);
            Assert.IsTrue(src.HideFileHotkey.Key == dst.HideFileHotkey.Key);
            Assert.IsTrue(src.HideFileHotkey.ModifierKeys == dst.HideFileHotkey.ModifierKeys);
            Assert.IsTrue(src.SuppressFunction1Key == dst.SuppressFunction1Key);
        }
    }
}
