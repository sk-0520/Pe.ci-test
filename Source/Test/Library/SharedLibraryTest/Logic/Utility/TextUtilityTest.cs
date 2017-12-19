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
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Utility
{
    [TestFixture]
    class TextUtilityTest
    {
        [TestCase("a", "a", "")]
        [TestCase("a", "a", "b")]
        [TestCase("a(2)", "a", "a")]
        [TestCase("A", "A", "A(2)")]
        [TestCase("a(3)", "a", "a(5)", "a(2)", "a(4)", "a")]
        public void ToUniqueDefaultTest(string test, string src, params string[] list)
        {
            Assert.IsTrue(TextUtility.ToUniqueDefault(src, list, StringComparison.Ordinal) == test);
        }
    }
}
