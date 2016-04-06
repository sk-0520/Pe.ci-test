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
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using NUnit.Framework;
using System.Windows.Media;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting.MainSettings
{
    [TestFixture]
    class StreamSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new StreamSettingModel();
            src.OutputColor.ForeColor = Colors.Red;
            src.OutputColor.BackColor = Colors.Green;
            src.ErrorColor.ForeColor = Colors.Blue;
            src.ErrorColor.BackColor = Colors.Yellow;
            src.Font.Italic = true;
            src.Font.Bold = true;
            src.Font.Size = 999;
            src.Font.Family = "test";

            var dst = (StreamSettingModel)src.DeepClone();
            Assert.AreNotEqual(dst.OutputColor, src.OutputColor);
            Assert.AreNotEqual(dst.ErrorColor, src.ErrorColor);
            Assert.AreNotEqual(dst.Font, src.Font);

            Assert.AreEqual(dst.OutputColor.BackColor, src.OutputColor.BackColor);
            Assert.AreEqual(dst.OutputColor.ForeColor, src.OutputColor.ForeColor);
            Assert.AreEqual(dst.ErrorColor.BackColor, src.ErrorColor.BackColor);
            Assert.AreEqual(dst.ErrorColor.ForeColor, src.ErrorColor.ForeColor);
            Assert.AreEqual(dst.Font.Italic, src.Font.Italic);
            Assert.AreEqual(dst.Font.Bold, src.Font.Bold);
            Assert.AreEqual(dst.Font.Family, src.Font.Family);
        }
    }
}
