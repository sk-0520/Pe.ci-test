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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Utility
{
    [TestFixture]
    class MediaUtilityTest
    {
        [TestCase(0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff)]
        [TestCase(0x00, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00)]
        [TestCase(0x00, 0x10, 0xff, 0xff, 0x00, 0xef, 0x00, 0x00)]
        public void GetNegativeColorTest(byte testA, byte testR, byte testG, byte testB, byte argA, byte argR, byte argG, byte argB)
        {
            var test = Color.FromArgb(testA, testR, testG, testB);
            var arg = Color.FromArgb(argA, argR, argG, argB);

            var result = MediaUtility.GetNegativeColor(arg);

            Assert.AreEqual(test, result);
        }
    }
}
