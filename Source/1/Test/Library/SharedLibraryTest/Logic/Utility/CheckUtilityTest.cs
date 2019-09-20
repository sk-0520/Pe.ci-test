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
    class CheckUtilityTest
    {
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void EnforceTest(bool success, bool test)
        {
            if(success) {
                Assert.DoesNotThrow(() => CheckUtility.Enforce(test));
            } else {
                Assert.Throws<Exception>(() => CheckUtility.Enforce(test));
            }
        }

        [Test]
        public void EnforceNotNull_class_Test()
        {
            var test = new Object();
            Assert.DoesNotThrow(() => CheckUtility.EnforceNotNull(test));

            test = null;
            Assert.Throws<ArgumentNullException>(() => CheckUtility.EnforceNotNull(test));

            Assert.DoesNotThrow(() => CheckUtility.EnforceNotNull(new Exception()));
            Assert.Throws<ArgumentNullException>(() => CheckUtility.EnforceNotNull(default(Exception)));
        }

        [Test]
        public void EnforceNotNull_Nullable_Test()
        {
            int? test1 = 1;
            Assert.DoesNotThrow(() => CheckUtility.EnforceNotNull(test1));
            test1 = null;
            Assert.Throws<ArgumentNullException>(() => CheckUtility.EnforceNotNull(test1));
        }

        [TestCase(true, "b")]
        [TestCase(true, " c")]
        [TestCase(true, "d ")]
        [TestCase(true, " e ")]
        [TestCase(true, " ")]
        [TestCase(false, null)]
        [TestCase(false, "")]
        public void EnforceNotNullAndNotEmptyTest(bool success, string test)
        {
            if(success) {
                Assert.DoesNotThrow(() => CheckUtility.EnforceNotNullAndNotEmpty(test));
            } else {
                Assert.Throws<ArgumentException>(() => CheckUtility.EnforceNotNullAndNotEmpty(test));
            }
        }

        [TestCase(true, "b")]
        [TestCase(true, " c")]
        [TestCase(true, "d ")]
        [TestCase(true, " e ")]
        [TestCase(false, " ")]
        [TestCase(false, null)]
        [TestCase(false, "")]
        public void EnforceNotNullAndNotWhiteSpaceTest(bool success, string test)
        {
            if(success) {
                Assert.DoesNotThrow(() => CheckUtility.EnforceNotNullAndNotWhiteSpace(test));
            } else {
                Assert.Throws<ArgumentException>(() => CheckUtility.EnforceNotNullAndNotWhiteSpace(test));
            }
        }

        [TestCase(true, 1)]
        [TestCase(false, 0)]
        public void EnforceNotZeroTest(bool success, int arg)
        {
            var test = new IntPtr(arg);
            if(success) {
                Assert.DoesNotThrow(() => CheckUtility.EnforceNotZero(test));
            } else {
                Assert.Throws<ArgumentException>(() => CheckUtility.EnforceNotZero(test));
            }
        }

        [Test]
        public void EnforceNotZero_val_Test()
        {
            Assert.Throws<ArgumentException>(() => CheckUtility.EnforceNotZero(IntPtr.Zero));
            Assert.Throws<ArgumentException>(() => CheckUtility.EnforceNotZero((IntPtr)null));
        }
    }
}
