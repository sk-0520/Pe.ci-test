using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
{
    [TestClass]
    public class ChunkItemTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var item = new ChunkItem<int>(100);
            Assert.AreEqual(100, item.Capacity);
        }

        [TestMethod]
        public void AddTest()
        {
            var item = new ChunkItem<int>(3);
            item.Add(10);
            Assert.AreEqual(1, item.Count);

            item.Add(20);
            item.Add(30);
            Assert.ThrowsException<OutOfMemoryException>(() =>item.Add(40));
        }

        [TestMethod]
        public void ClearTest()
        {
            var item = new ChunkItem<int>(3);
            item.Add(10);
            Assert.AreEqual(1, item.Count);

            item.Clear();
            Assert.AreEqual(0, item.Count);

            item.Add(10);
            Assert.AreEqual(1, item.Count);
        }

        [TestMethod]
        [DataRow(true, 0, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        [DataRow(true, 3, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        [DataRow(true, 6, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        [DataRow(false, -1, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        [DataRow(false, 7, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        public void ContainsTest(bool result, int value, params int[] items)
        {
            var item = new ChunkItem<int>(items.Length);
            foreach(var i in items) {
                item.Add(i);
            }
            var test = item.Contains(value);
            Assert.AreEqual(result, test);
        }

        [TestMethod]
        public void CopyToTest()
        {
            var item = new ChunkItem<int>(2);
            item.Add(10);
            item.Add(20);

            var array = new int[item.Capacity];
            item.CopyTo(array, 0);
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(10, array[0]);
            Assert.AreEqual(20, array[1]);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var item = new ChunkItem<int>(4);
            item.Add(10);
            item.Add(20);
            item.Add(30);
            item.Add(40);

            Assert.IsTrue(item.Remove(40));
            Assert.IsFalse(item.Remove(40));
            Assert.AreEqual(3, item.Count);

            Assert.AreEqual(10, item.ElementAt(0));
            Assert.AreEqual(20, item.ElementAt(1));
            Assert.AreEqual(30, item.ElementAt(2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => item.ElementAt(3));

            item.Add(50);
            Assert.AreEqual(50, item.ElementAt(3));

            item.Remove(20);
            Assert.AreEqual(10, item.ElementAt(0));
            Assert.AreEqual(30, item.ElementAt(1));
            Assert.AreEqual(50, item.ElementAt(2));

            item.Remove(10);
            Assert.AreEqual(30, item.ElementAt(0));
            Assert.AreEqual(50, item.ElementAt(1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => item.ElementAt(2));

        }
    }

    [TestClass]
    public class ChunkedListTest
    {
        [TestMethod]
        public void AddTest()
        {
            var item = new ChunkedList<int>(2, 3);
            item.Add(10);
            Assert.AreEqual(1, item.Count);

            item.Add(20);
            item.Add(30);

            item.Add(40);
            item.Add(50);
            item.Add(60);
            Assert.ThrowsException<OutOfMemoryException>(() => item.Add(70));
        }

        [TestMethod]
        public void ClearTest()
        {
            var item = new ChunkedList<int>(5, 1);

            item.Add(10);
            item.Add(20);
            item.Add(30);
            item.Add(40);
            item.Add(50);
            item.Clear();
            Assert.AreEqual(0, item.Count);

            item.Add(10);
            item.Add(20);
            item.Add(30);
            Assert.AreEqual(3, item.Count);
        }
    }
}
