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
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Utility
{
    [TestFixture]
    class EnumUtilityTest
    {
        [TestCase(IconScale.Small, IconScale.Small, IconScale.Small)]
        [TestCase(IconScale.Small, IconScale.Small, IconScale.Normal)]
        [TestCase(IconScale.Small, IconScale.Small, IconScale.Big)]
        [TestCase(IconScale.Small, IconScale.Small, IconScale.Large)]
        [TestCase(IconScale.Small, 16, IconScale.Large)]
        [TestCase(IconScale.Large, 17, IconScale.Large)]
        [TestCase(IconScale.Normal, 32, IconScale.Large)]
        public void GetNormalization_IconScale_Test(IconScale result, object test, IconScale def)
        {
            var r = EnumUtility.GetNormalization(test, def);
            Assert.AreEqual(r, result);
        }
    }
}
