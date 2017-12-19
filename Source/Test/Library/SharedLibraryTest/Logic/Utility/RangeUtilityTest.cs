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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Utility
{
    [TestFixture]
    class RangeUtilityTest
    {
        [TestCase(true, 20, 10, 30)]
        [TestCase(true, 10, 10, 30)]
        [TestCase(true, 30, 10, 30)]
        [TestCase(false, 9, 10, 30)]
        [TestCase(false, 31, 10, 30)]
        public void BetweenTest(bool test, int value, int min, int max)
        {
            var result = RangeUtility.Between(value, min, max);
            Assert.AreEqual(result, test);
        }

        [TestCase(20, 20, 10, 30)]
        [TestCase(10, 10, 10, 30)]
        [TestCase(30, 30, 10, 30)]
        [TestCase(10, 9, 10, 30)]
        [TestCase(30, 31, 10, 30)]
        public void ClampTest(int test, int value, int min, int max)
        {
            var result = RangeUtility.Clamp(value, min, max);
            Assert.AreEqual(result, test);
        }

        [TestCase(1, 0)]
        [TestCase(-999, -1000)]
        [TestCase(int.MaxValue, int.MaxValue - 1)]
        [TestCase(int.MaxValue, int.MaxValue)]
        [TestCase(int.MinValue + 1, int.MinValue)]
        public void Increment_int_Test(int test, int value)
        {
            var result = RangeUtility.Increment(value);
            Assert.AreEqual(result, test);
        }

        [TestCase(1u, 0u)]
        [TestCase(uint.MaxValue, uint.MaxValue - 1)]
        [TestCase(uint.MaxValue, uint.MaxValue)]
        [TestCase(uint.MinValue + 1, uint.MinValue)]
        public void Increment_uint_Test(uint test, uint value)
        {
            var result = RangeUtility.Increment(value);
            Assert.AreEqual(result, test);
        }

    }
}
