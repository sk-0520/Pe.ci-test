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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Data
{
    [TestFixture]
    class TripleRangeTest
    {
        [Test]
        public void Constructor_Test()
        {
            var data = new TripleRange<int>(1, 2, 3);
            Assert.AreEqual(data.minimum, 1);
            Assert.AreEqual(data.median, 2);
            Assert.AreEqual(data.maximum, 3);
        }


        [Test]
        public void Wrapper_Test()
        {
            var data = TripleRange.Create(1, 2, 3);
            Assert.AreEqual(data.minimum, 1);
            Assert.AreEqual(data.median, 2);
            Assert.AreEqual(data.maximum, 3);
        }

        [Test]
        public void WrapperParse_Test1()
        {
            var data = TripleRange.Parse<int>("1, 2, 3");
            Assert.AreEqual(data.minimum, 1);
            Assert.AreEqual(data.median, 2);
            Assert.AreEqual(data.maximum, 3);
        }

        [TestCase(false, "", default(int))]
        [TestCase(false, "1", default(int))]
        [TestCase(false, "1,", default(int))]
        [TestCase(false, "1,2", default(int))]
        [TestCase(false, "1,2,", default(int))]
        [TestCase(false, "1,2,,", default(int))]
        [TestCase(false, "1,2,,4", default(int))]
        [TestCase(true, "1,2,3", 6)]
        [TestCase(true, "1,2,-3", 0)]
        [TestCase(true, "    1   ,   2     ,    -3     ", 0)]
        [TestCase(false, "1.0,2,3", default(int))]
        public void WrapperParse_Test2(bool isSuccess, string s, int sum)
        {
            if(isSuccess) {
                var a = TripleRange.Parse<int>(s);
                var sum2 = a.median + a.minimum + a.maximum;
                Assert.IsTrue(sum == sum2);
            } else {
                Assert.Catch(() => TripleRange.Parse<int>(s));
            }
        }

    }
}
