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
using NUnit.Framework;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Extension
{
    [TestFixture]
    class LinqExtensionTest
    {
        [TestCase(false, true, 1, 2, 3)]
        [TestCase(true, false, 1, 2, 3)]
        public void IfReveseTest(bool test, bool revese, params object[] array)
        {
            var r = array.IfRevese(revese);
            var rt = Enumerable.SequenceEqual(array, r);
            Assert.True(test == rt);
            Assert.True(Enumerable.SequenceEqual(array, r.IfRevese(revese)));
        }


        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 3, 2, 1 }, true)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 2, 3, 1 }, true)]
        [TestCase(new[] { 3, 2, 1 }, new[] { 3, 2, 1 }, false)]
        [TestCase(new[] { 3, 2, 1 }, new[] { 1, 2, 3 }, false)]
        [TestCase(new[] { 3, 2, 1 }, new[] { 2, 3, 1 }, false)]
        public void IfOrderByAscTest(int[] result, int[] array, bool orderByAsc)
        {
            var r = array.IfOrderByAsc(k => k, orderByAsc);
            Assert.True(Enumerable.SequenceEqual(result, r));
        }
    }
}
