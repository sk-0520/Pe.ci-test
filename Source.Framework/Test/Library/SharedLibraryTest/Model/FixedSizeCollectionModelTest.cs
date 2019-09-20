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

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Model
{
    [TestFixture]
    class FixedSizeCollectionModelTest
    {
        [Test]
        public void CtorListTest()
        {
            var values = new[] { 1, 2, 3 };
            var list = new FixedSizeCollectionModel<int>(values, values.Length);
            Assert.AreEqual(values.Length, list.Count, list.LimitSize);
            Assert.True(list.IsFIFO);
        }

        [Test]
        public void CtorListAndLimitTest()
        {
            var limit = 2;
            var values = new[] { 1, 2, 3 };
            var list = new FixedSizeCollectionModel<int>(values, limit);
            Assert.AreEqual(list.LimitSize, limit);
            Assert.AreEqual(list.Count, limit);
        }

        [TestCase(3, new[] { 0 }, new[] { 0 })]
        [TestCase(3, new[] { 1, 2 }, new[] { 1, 2 })]
        [TestCase(3, new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        [TestCase(3, new[] { 2, 3, 4 }, new[] { 1, 2, 3, 4 })]
        [TestCase(3, new[] { 4, 5, 6 }, new[] { 1, 2, 3, 4, 5, 6 })]
        public void CtroLimitFifo(int limit, int[] tests, int[] values)
        {
            var list = new FixedSizeCollectionModel<int>(values, limit, true);
            for(var i = 0; i < list.Count; i++) {
                Assert.AreEqual(list[i], tests[i]);
            }
        }

        [TestCase(3, new[] { 0 }, new[] { 0 })]
        [TestCase(3, new[] { 1, 2 }, new[] { 1, 2 })]
        [TestCase(3, new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        [TestCase(3, new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 })]
        [TestCase(3, new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4, 5, 6 })]
        public void CtroLimitLifo(int limit, int[] tests, int[] values)
        {
            var list = new FixedSizeCollectionModel<int>(values, limit, false);
            for(var i = 0; i < list.Count; i++) {
                Assert.AreEqual(list[i], tests[i]);
            }
        }

        public FixedSizeCollectionModel<int> GetList()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            return new FixedSizeCollectionModel<int>(values, values.Length);
        }

        [TestCase(10, 20)]
        [TestCase(10, 20, 30)]
        public void AddFifoTest(params int[] values)
        {
            var list = GetList();
            foreach(var value in values) {
                list.Add(value);
            }
            var test = GetList();
            for(var i = 0; i < values.Length; i++) {
                Assert.AreEqual(list[i], test[i + values.Length]);
            }
        }

        [TestCase(10, 20)]
        [TestCase(10, 20, 30)]
        public void AddLifoTest(params int[] values)
        {
            var list = GetList();
            list.IsFIFO = false;
            foreach(var value in values) {
                list.Add(value);
            }
            var test = GetList();
            for(var i = 0; i < values.Length; i++) {
                Assert.AreEqual(list[i], test[i]);
            }
        }

        [Test]
        public void InsertFifoTest1()
        {
            var list = GetList();
            list.Insert(0, 100);
            Assert.AreEqual(list[0], 1);
            Assert.AreEqual(list[1], 2);
            Assert.AreEqual(list[8], 9);
        }

        [Test]
        public void InsertFifoTest2()
        {
            var list = GetList();
            list.Insert(5, 100);
            Assert.AreEqual(list[0], 2);
            Assert.AreEqual(list[1], 3);
            Assert.AreEqual(list[3], 5);
            Assert.AreEqual(list[4], 100);
            Assert.AreEqual(list[5], 6);
            Assert.AreEqual(list[8], 9);
        }

        [Test]
        public void InsertLifoTest1()
        {
            var list = GetList();
            list.IsFIFO = false;
            list.Insert(0, 100);
            Assert.AreEqual(list[0], 100);
            Assert.AreEqual(list[1], 1);
            Assert.AreEqual(list[8], 8);
        }

        [Test]
        public void InsertLifoTest2()
        {
            var list = GetList();
            list.IsFIFO = false;
            list.Insert(5, 100);
            Assert.AreEqual(list[0], 1);
            Assert.AreEqual(list[1], 2);
            Assert.AreEqual(list[3], 4);
            Assert.AreEqual(list[4], 5);
            Assert.AreEqual(list[5], 100);
            Assert.AreEqual(list[8], 8);
        }

    }
}
