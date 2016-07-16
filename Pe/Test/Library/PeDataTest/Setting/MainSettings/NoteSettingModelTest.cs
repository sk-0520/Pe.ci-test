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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting.MainSettings
{
    [TestFixture]
    class NoteSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new NoteSettingModel() {
                AutoLineFeed = true,
                BackColor = Colors.Blue,
                CompactHotKey = new HotKeyModel() {
                    Key = Key.A,
                    ModifierKeys = ModifierKeys.Control,
                },
                CreateHotKey = new HotKeyModel() {
                    Key = Key.B,
                    ModifierKeys = ModifierKeys.Alt,
                },
                Font = new FontModel() {
                    Bold = true,
                    Family = "#",
                    Italic = true,
                    Size = 1.23,
                },
                ForeColor = Colors.Cyan,
                HideHotKey = new HotKeyModel() {
                    Key = Key.C,
                    ModifierKeys = ModifierKeys.Shift,
                },
                IsTopmost = true,
                NoteKind = Pe.Library.PeData.Define.NoteKind.Rtf,
                NoteTitle = Pe.Library.PeData.Define.NoteTitle.DefaultCaption,
                ShowFrontHotKey = new HotKeyModel() {
                    Key = Key.D,
                    ModifierKeys = ModifierKeys.Windows,
                }
            };

            var dst = (NoteSettingModel)src.DeepClone();

            Assert.IsTrue(src.AutoLineFeed == dst.AutoLineFeed);
            Assert.IsTrue(src.BackColor == dst.BackColor);
            Assert.IsTrue(src.CompactHotKey.Key == dst.CompactHotKey.Key);
            Assert.IsTrue(src.CompactHotKey.ModifierKeys == dst.CompactHotKey.ModifierKeys);
            Assert.IsTrue(src.CreateHotKey.Key == dst.CreateHotKey.Key);
            Assert.IsTrue(src.CreateHotKey.ModifierKeys == dst.CreateHotKey.ModifierKeys);
            Assert.IsTrue(src.Font.Bold == dst.Font.Bold);
            Assert.IsTrue(src.Font.Family == dst.Font.Family);
            Assert.IsTrue(src.Font.Italic == dst.Font.Italic);
            Assert.IsTrue(src.Font.Size == dst.Font.Size);
            Assert.IsTrue(src.ForeColor == dst.ForeColor);
            Assert.IsTrue(src.HideHotKey.Key == dst.HideHotKey.Key);
            Assert.IsTrue(src.HideHotKey.ModifierKeys == dst.HideHotKey.ModifierKeys);
            Assert.IsTrue(src.IsTopmost == dst.IsTopmost);
            Assert.IsTrue(src.NoteKind == dst.NoteKind);
            Assert.IsTrue(src.NoteTitle == dst.NoteTitle);
            Assert.IsTrue(src.ShowFrontHotKey.Key == dst.ShowFrontHotKey.Key);
            Assert.IsTrue(src.ShowFrontHotKey.ModifierKeys == dst.ShowFrontHotKey.ModifierKeys);
        }
    }
}
