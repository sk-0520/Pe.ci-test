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
using ContentTypeTextNet.Pe.Library.PeData.Define;
using NUnit.Framework;
using ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.Test.Library.PeMainTest.ViewModel.SettingPage
{
    [TestFixture]
    class ClipboardSettingViewModelTest
    {
        [TestCase(ClipboardType.Text, ClipboardType.None, true)]
        [TestCase(ClipboardType.None, ClipboardType.None, false)]
        [TestCase(ClipboardType.Text, ClipboardType.Text, true)]
        [TestCase(ClipboardType.None, ClipboardType.Text, false)]
        [TestCase(ClipboardType.Text | ClipboardType.Rtf, ClipboardType.Rtf, true)]
        [TestCase(ClipboardType.All, ClipboardType.All, true)]
        [TestCase(ClipboardType.All ^ ClipboardType.Text, ClipboardType.All, false)]
        public void CaptureTypeTextTest(ClipboardType test, ClipboardType init, bool set)
        {
            var model = new ClipboardSettingModel() {
                CaptureType = init,
            };
            var vm = new ClipboardSettingViewModel(model, null, null, null);
            vm.CaptureTypeText = set;
            Assert.AreEqual(model.CaptureType, test);
        }

        [TestCase(ClipboardType.Rtf, ClipboardType.None, true)]
        [TestCase(ClipboardType.None, ClipboardType.None, false)]
        [TestCase(ClipboardType.Rtf, ClipboardType.Rtf, true)]
        [TestCase(ClipboardType.None, ClipboardType.Rtf, false)]
        [TestCase(ClipboardType.Text | ClipboardType.Rtf, ClipboardType.Text, true)]
        [TestCase(ClipboardType.All, ClipboardType.All, true)]
        [TestCase(ClipboardType.All ^ ClipboardType.Rtf, ClipboardType.All, false)]
        public void CaptureTypeRtfTest(ClipboardType test, ClipboardType init, bool set)
        {
            var model = new ClipboardSettingModel() {
                CaptureType = init,
            };
            var vm = new ClipboardSettingViewModel(model, null, null, null);
            vm.CaptureTypeRtf = set;
            Assert.AreEqual(model.CaptureType, test);
        }

        [TestCase(ClipboardType.Html, ClipboardType.None, true)]
        [TestCase(ClipboardType.None, ClipboardType.None, false)]
        [TestCase(ClipboardType.Html, ClipboardType.Html, true)]
        [TestCase(ClipboardType.None, ClipboardType.Html, false)]
        [TestCase(ClipboardType.Text | ClipboardType.Html, ClipboardType.Text, true)]
        [TestCase(ClipboardType.All, ClipboardType.All, true)]
        [TestCase(ClipboardType.All ^ ClipboardType.Html, ClipboardType.All, false)]
        public void CaptureTypeHtmlTest(ClipboardType test, ClipboardType init, bool set)
        {
            var model = new ClipboardSettingModel() {
                CaptureType = init,
            };
            var vm = new ClipboardSettingViewModel(model, null, null, null);
            vm.CaptureTypeHtml = set;
            Assert.AreEqual(model.CaptureType, test);
        }

        [TestCase(ClipboardType.Image, ClipboardType.None, true)]
        [TestCase(ClipboardType.None, ClipboardType.None, false)]
        [TestCase(ClipboardType.Image, ClipboardType.Image, true)]
        [TestCase(ClipboardType.None, ClipboardType.Image, false)]
        [TestCase(ClipboardType.Text | ClipboardType.Image, ClipboardType.Text, true)]
        [TestCase(ClipboardType.All, ClipboardType.All, true)]
        [TestCase(ClipboardType.All ^ ClipboardType.Image, ClipboardType.All, false)]
        public void CaptureTypeImageTest(ClipboardType test, ClipboardType init, bool set)
        {
            var model = new ClipboardSettingModel() {
                CaptureType = init,
            };
            var vm = new ClipboardSettingViewModel(model, null, null, null);
            vm.CaptureTypeImage = set;
            Assert.AreEqual(model.CaptureType, test);
        }

        [TestCase(ClipboardType.Files, ClipboardType.None, true)]
        [TestCase(ClipboardType.None, ClipboardType.None, false)]
        [TestCase(ClipboardType.Files, ClipboardType.Files, true)]
        [TestCase(ClipboardType.None, ClipboardType.Files, false)]
        [TestCase(ClipboardType.Text | ClipboardType.Files, ClipboardType.Text, true)]
        [TestCase(ClipboardType.All, ClipboardType.All, true)]
        [TestCase(ClipboardType.All ^ ClipboardType.Files, ClipboardType.All, false)]
        public void CaptureTypeFilesTest(ClipboardType test, ClipboardType init, bool set)
        {
            var model = new ClipboardSettingModel() {
                CaptureType = init,
            };
            var vm = new ClipboardSettingViewModel(model, null, null, null);
            vm.CaptureTypeFiles = set;
            Assert.AreEqual(model.CaptureType, test);
        }


    }
}
