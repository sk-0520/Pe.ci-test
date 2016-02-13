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
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting.MainSettings
{
    [TestFixture]
    class RunningInformationSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new RunningInformationSettingModel() {
                Accept = true,
                CheckUpdateRelease = true,
                CheckUpdateRC = true,
                ExecuteCount = 999,
                IgnoreUpdateVersion = new Version(1,2,3,4),
                LastExecuteVersion = new Version(9,8,7,6),
                UserId = "123456789",
                SendPersonalInformation = true,
            };

            var dst = (RunningInformationSettingModel)src.DeepClone();

            Assert.AreEqual(src.Accept, dst.Accept);
            Assert.AreEqual(src.CheckUpdateRelease, dst.CheckUpdateRelease);
            Assert.AreEqual(src.CheckUpdateRC, dst.CheckUpdateRC);
            Assert.AreEqual(src.ExecuteCount, dst.ExecuteCount);
            Assert.AreEqual(src.IgnoreUpdateVersion.ToString(), dst.IgnoreUpdateVersion.ToString());
            Assert.AreEqual(src.LastExecuteVersion.ToString(), dst.LastExecuteVersion.ToString());
            Assert.AreEqual(src.UserId, dst.UserId);
            Assert.AreEqual(src.SendPersonalInformation, dst.SendPersonalInformation);
        }
    }
}
