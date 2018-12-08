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
            Assert.ThrowsException<OutOfMemoryException>(() => item.Add(40));
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
        public void CopyFromTest()
        {
            var array = new int[] {
                10, 20, 30, 40
            };

            var item1 = new ChunkItem<int>(4);
            item1.CopyFrom(0, array, 0, array.Length);
            Assert.AreEqual(4, item1.Count);
            CollectionAssert.AreEqual(array, item1.ToArray());

            var item2 = new ChunkItem<int>(4);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => item2.CopyFrom(1, array, 1, 2));
            item2.Add(99);
            item2.CopyFrom(1, array, 1, 2);
            CollectionAssert.AreEqual(new[] { 99, 20, 30 }, item2.ToArray());

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
            var list = new ChunkedList<int>(2, 3);
            list.Add(10);
            Assert.AreEqual(1, list.Count);

            list.Add(20);
            list.Add(30);

            list.Add(40);
            list.Add(50);
            list.Add(60);
            Assert.ThrowsException<OutOfMemoryException>(() => list.Add(70));
        }

        [TestMethod]
        public void ClearTest()
        {
            var list = new ChunkedList<int>(5, 1);

            list.Add(10);
            list.Add(20);
            list.Add(30);
            list.Add(40);
            list.Add(50);
            list.Clear();
            Assert.AreEqual(0, list.Count);

            list.Add(10);
            list.Add(20);
            list.Add(30);
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void ContainsTest()
        {
            var list = new ChunkedList<int>(5, 1);

            list.Add(10);
            list.Add(20);
            list.Add(30);
            list.Add(40);
            list.Add(50);

            Assert.IsFalse(list.Contains(0));
            Assert.IsTrue(list.Contains(10));
            Assert.IsTrue(list.Contains(20));
            Assert.IsTrue(list.Contains(30));
            Assert.IsTrue(list.Contains(40));
            Assert.IsTrue(list.Contains(50));
            Assert.IsFalse(list.Contains(51));
        }

        [TestMethod]
        public void CopyToTest()
        {
            var list1 = new ChunkedList<int>(5, 2);
            foreach(var n in Enumerable.Range(0, 5 * 2)) {
                list1.Add(n);
            }

            var array1 = new int[list1.Count];
            list1.CopyTo(array1, 0);
            CollectionAssert.AreEqual(array1, Enumerable.Range(0, list1.Count).ToArray());

            var array2 = new int[list1.Count];
            list1.CopyTo(array2, 5);
            var a = array2.Take(5).ToArray();
            var b = Enumerable.Range(0, list1.Count).Skip(5).ToArray();
            CollectionAssert.AreEqual(a, b);


            var list2 = new ChunkedList<int>(10, 1);
            foreach(var n in Enumerable.Range(0, 10 * 1)) {
                list2.Add(n);
            }
            var array3 = new int[5];
            list2.CopyTo(5, array3, 1, 3);
            CollectionAssert.AreEqual(array3.ToArray(), new[] { 0, 5, 6, 7, 0 });

            var list3 = new ChunkedList<int>(1, 10);
            foreach(var n in Enumerable.Range(0, 10 * 1)) {
                list3.Add(n);
            }
            var array4 = new int[5];
            list2.CopyTo(5, array4, 1, 3);
            CollectionAssert.AreEqual(array4.ToArray(), new[] { 0, 5, 6, 7, 0 });
        }


        [TestMethod]
        [DataRow(3, 4, 3 * 4 - 0)]
        [DataRow(3, 4, 3 * 4 - 1)]
        [DataRow(3, 4, 3 * 4 - 2)]
        [DataRow(3, 4, 3 * 4 - 3)]
        [DataRow(3, 4, 3 * 4 - 4)]
        [DataRow(3, 4, 3 * 4 - 5)]
        [DataRow(3, 4, 3 * 4 - 11)]
        [DataRow(1, 1, 1 * 1)]
        [DataRow(100, 1, 100 * 1)]
        [DataRow(1, 100, 1 * 100)]
        [DataRow(100, 1, 50)]
        [DataRow(1, 100, 50)]
        public void GetEnumeratorTest(int capacity, int itemCapacity, int size)
        {
            var rnd = new Random();
            var items = Enumerable.Repeat(0, size).Select(i => rnd.Next()).ToArray();
            var list = new ChunkedList<int>(capacity, itemCapacity);
            foreach(var item in items) {
                list.Add(item);
            }
            Assert.IsTrue(list.SequenceEqual(items));

        }

        [TestMethod]
        [DataRow(3, 4, 3 * 4 - 0)]
        [DataRow(3, 4, 3 * 4 - 1)]
        [DataRow(3, 4, 3 * 4 - 2)]
        [DataRow(3, 4, 3 * 4 - 3)]
        [DataRow(3, 4, 3 * 4 - 4)]
        [DataRow(3, 4, 3 * 4 - 5)]
        [DataRow(3, 4, 3 * 4 - 11)]
        [DataRow(1, 1, 1 * 1)]
        [DataRow(100, 1, 100 * 1)]
        [DataRow(1, 100, 1 * 100)]
        [DataRow(100, 1, 50)]
        [DataRow(1, 100, 50)]
        public void RemoveTest(int capacity, int itemCapacity, int size)
        {
            var items = Enumerable.Range(0, size).ToArray();
            var list1 = new ChunkedList<int>(capacity, itemCapacity);
            var list2 = new ChunkedList<int>(capacity, itemCapacity);
            var list3 = new ChunkedList<int>(capacity, itemCapacity);
            foreach(var item in items) {
                list1.Add(item);
                list2.Add(item);
                list3.Add(item);
            }

            var count1 = list1.Count;
            list1.Remove(items[0]);
            Assert.AreEqual(count1, list1.Count + 1);
            Assert.IsTrue(list1.SequenceEqual(items.Skip(1)));

            var count2 = list2.Count;
            list2.Remove(items.Last());
            Assert.AreEqual(count2, list2.Count + 1);
            Assert.IsTrue(list2.SequenceEqual(items.Take(items.Length - 1)));

            var count3 = list3.Count;
            var removeIndex = count3 / 2;
            list3.Remove(items[removeIndex]);
            Assert.AreEqual(count3, list3.Count + 1);
            Assert.IsTrue(list3.SequenceEqual(items.Take(removeIndex).Concat(items.Skip(removeIndex + 1))));
        }

        [TestMethod]
        [DataRow(3, 4, 3 * 4 - 0)]
        [DataRow(3, 4, 3 * 4 - 1)]
        [DataRow(3, 4, 3 * 4 - 2)]
        [DataRow(3, 4, 3 * 4 - 3)]
        [DataRow(3, 4, 3 * 4 - 4)]
        [DataRow(3, 4, 3 * 4 - 5)]
        [DataRow(3, 4, 3 * 4 - 11)]
        [DataRow(1, 1, 1 * 1)]
        [DataRow(100, 1, 100 * 1)]
        [DataRow(1, 100, 1 * 100)]
        [DataRow(100, 1, 50)]
        [DataRow(1, 100, 50)]
        public void IndexTest(int capacity, int itemCapacity, int size)
        {
            var items = Enumerable.Range(0, size).ToArray();
            var list = new ChunkedList<int>(capacity, itemCapacity);
            foreach(var item in items) {
                list.Add(item);
            }

            for(var i = 0; i < items.Length; i++) {
                Assert.AreEqual(items[i], list[i]);
            }
        }
    }

    [TestClass]
    public class BinaryChunkedStreamTest
    {
        [TestMethod]
        public void ReadTest()
        {
            var list = new BinaryChunkedList(100, 3);
            var items = Enumerable.Range(0, 255).Select(i => (byte)i).ToArray();
            foreach(var item in items) {
                list.Add(item);
            }
            var stream = new BinaryChunkedStream(list);

            var buffer1 = new byte[10];
            stream.Read(buffer1, 0, buffer1.Length);
            CollectionAssert.AreEqual(buffer1, Enumerable.Range(0, 10).Select(i => (byte)i).ToArray());

            var buffer2 = new byte[10];
            var count2 = stream.Read(buffer2, 1, buffer2.Length - 2);
            Assert.AreEqual(8, count2);
            CollectionAssert.AreEqual(buffer2.Skip(1).Take(8).ToArray(), Enumerable.Range(10, 8).Select(i => (byte)i).ToArray());

            Assert.AreEqual(18, stream.Position);
        }
    }
}
