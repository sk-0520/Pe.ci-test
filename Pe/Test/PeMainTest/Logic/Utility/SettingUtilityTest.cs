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
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.Test.Library.PeMainTest.Logic.Utility
{
    [TestFixture]
    class SettingUtilityTest
    {
        [Test]
        public void CreateUserIdFromEnvironmentTest()
        {
            var a = SettingUtility.CreateUserIdFromEnvironment();
            var b = SettingUtility.CreateUserIdFromEnvironment();
            Assert.AreEqual(a, b);
        }
        [Test]
        public void CreateUserIdFromDateTimeTest()
        {
            var eq = DateTime.Now;
            var a = SettingUtility.CreateUserIdFromDateTime(eq);
            var b = SettingUtility.CreateUserIdFromDateTime(eq);
            Assert.AreEqual(a, b);

            var c = SettingUtility.CreateUserIdFromDateTime(DateTime.Now);
            var d = SettingUtility.CreateUserIdFromDateTime(DateTime.MaxValue);
            Assert.AreNotEqual(c, d);
        }
    }
}
