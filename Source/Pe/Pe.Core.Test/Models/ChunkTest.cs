using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class ChunkBlockTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var block = new ChunkBlock<int>(100);
            Assert.AreEqual(100, block.Size);
        }

        [TestMethod]
        public void Constructor_0_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => new ChunkBlock<int>(0));
        }

        [TestMethod]
        public void AddTest()
        {
            var block = new ChunkBlock<int>(3);
            block.Add(10);
            Assert.AreEqual(1, block.Count);

            block.Add(20);
            block.Add(30);
            Assert.ThrowsException<OutOfMemoryException>(() => block.Add(40));
        }

        [TestMethod]
        public void ClearTest()
        {
            var block = new ChunkBlock<int>(3);
            block.Add(10);
            Assert.AreEqual(1, block.Count);

            block.Clear();
            Assert.AreEqual(0, block.Count);

            block.Add(10);
            Assert.AreEqual(1, block.Count);
        }

        [TestMethod]
        [DataRow(true, 0, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        [DataRow(true, 3, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        [DataRow(true, 6, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        [DataRow(false, -1, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        [DataRow(false, 7, new[] { 0, 1, 2, 3, 4, 5, 6 })]
        public void ContainsTest(bool result, int value, int[] items)
        {
            var block = new ChunkBlock<int>(items.Length);
            foreach(var i in items) {
                block.Add(i);
            }
            var test = block.Contains(value);
            Assert.AreEqual(result, test);
        }

        [TestMethod]
        public void CopyToTest()
        {
            var block = new ChunkBlock<int>(2);
            block.Add(10);
            block.Add(20);

            var array = new int[block.Size];
            block.CopyTo(array, 0);
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

            var block1 = new ChunkBlock<int>(4);
            block1.CopyFrom(0, array, 0, array.Length);
            Assert.AreEqual(4, block1.Count);
            CollectionAssert.AreEqual(array, block1.ToArray());

            var block2 = new ChunkBlock<int>(4);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => block2.CopyFrom(1, array, 1, 2));
            block2.Add(99);
            block2.CopyFrom(1, array, 1, 2);
            CollectionAssert.AreEqual(new[] { 99, 20, 30 }, block2.ToArray());

        }

        [TestMethod]
        public void RemoveTest()
        {
            var block = new ChunkBlock<int>(4);
            block.Add(10);
            block.Add(20);
            block.Add(30);
            block.Add(40);

            Assert.IsTrue(block.Remove(40));
            Assert.IsFalse(block.Remove(40));
            Assert.AreEqual(3, block.Count);

            Assert.AreEqual(10, block.ElementAt(0));
            Assert.AreEqual(20, block.ElementAt(1));
            Assert.AreEqual(30, block.ElementAt(2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => block.ElementAt(3));

            block.Add(50);
            Assert.AreEqual(50, block.ElementAt(3));

            block.Remove(20);
            Assert.AreEqual(10, block.ElementAt(0));
            Assert.AreEqual(30, block.ElementAt(1));
            Assert.AreEqual(50, block.ElementAt(2));

            block.Remove(10);
            Assert.AreEqual(30, block.ElementAt(0));
            Assert.AreEqual(50, block.ElementAt(1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => block.ElementAt(2));

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
            Assert.AreEqual(2 * 3, list.Count);

            list.Add(70);
            Assert.AreEqual(2 * 3 + 1, list.Count);
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
        public void CopyFromTest()
        {
            var array = Enumerable.Range(0, 100).ToArray();

            foreach(var (cap, item) in new[] { (5, 3), (1, 1), (100, 1), (1, 100) }) {
                var listAll = new ChunkedList<int>(cap, item);
                listAll.CopyFrom(0, array, 0, array.Length);
                Assert.AreEqual(array.Length, listAll.Count);
                CollectionAssert.AreEqual(array, listAll.ToArray());
            }
            foreach(var (cap, item) in new[] { (5, 3), (1, 1), (100, 1), (1, 100) }) {
                var list10 = new ChunkedList<int>(cap, item);
                list10.CopyFrom(0, array, 0, array.Length);
                list10.CopyFrom(10, array, 0, 10);
                Assert.AreEqual(array.Length, list10.Count);
                CollectionAssert.AreEqual(array.Take(10).Concat(array.Take(10)).ToArray(), list10.Take(20).ToArray());
            }
            foreach(var (cap, item) in new[] { (5, 3), (1, 1), (100, 1), (1, 100) }) {
                var list100 = new ChunkedList<int>(cap, item);
                list100.CopyFrom(0, array, 0, array.Length);
                list100.CopyFrom(100, array, 0, 5);
                Assert.AreEqual(array.Length + 5, list100.Count);
                CollectionAssert.AreEqual(array.Concat(array.Take(5)).ToArray(), list100.ToArray());
            }
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

        [TestMethod]
        [DataRow(50, 10, 500, 10)]
        [DataRow(100, 5, 500, 5)]
        [DataRow(10, 1, 10, 1)]
        [DataRow(5, 2, 10, 2)]
        [DataRow(4, 1, 10, 3)]
        [DataRow(3, 2, 10, 4)]
        [DataRow(2, 5, 10, 5)]
        [DataRow(2, 4, 10, 6)]
        [DataRow(2, 3, 10, 7)]
        [DataRow(2, 2, 10, 8)]
        [DataRow(2, 1, 10, 9)]
        [DataRow(1, 10, 10, 10)]
        public void GetAllVal0uesTest(int resultCount, int resultLastLength, int dataSize, int chunkSize)
        {
            var items = Enumerable.Range(0, dataSize).ToArray();
            var list = new ChunkedList<int>(3, 3);
            foreach(var item in items) {
                list.Add(item);
            }
            var bins = list.GroupSplit(chunkSize);

            Assert.AreEqual(resultCount, bins.Count());
            Assert.AreEqual(chunkSize, bins.First().Count());
            Assert.AreEqual(resultLastLength, bins.Last().Count());
            var binArray = bins.SelectMany(i => i).ToArray();
            CollectionAssert.AreEqual(items, binArray);
        }

    }

    [TestClass]
    public class BinaryChunkedListTest
    {
        [TestMethod]
        public void CopyFromTest()
        {
            var list = new BinaryChunkedList(255, BinaryChunkedList.DefaultBlockSize);
            var rnd = new Random();
            var data = Enumerable.Range(0, BinaryChunkedList.LargeObjectHeapSize * 9).Select(i => (byte)rnd.Next(0, 255)).ToArray();
            list.CopyFrom(0, data, 0, data.Length);
            CollectionAssert.AreEqual(data, list);
        }

        [TestMethod]
        public void CopyFromTest_2()
        {
            var list = new BinaryChunkedList(255, BinaryChunkedList.DefaultBlockSize);
            var rnd = new Random();
            var buffer = 65536-1;
            var count = 30;
            var data = Enumerable.Range(0, buffer * count).Select(i => (byte)rnd.Next(0, 255)).ToArray();
            for(var i = 0; i < count; i++) {
                list.CopyFrom(buffer * i, data, buffer * i, buffer);
            }
            CollectionAssert.AreEqual(data, list);
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

        [TestMethod]
        public void WriteTest()
        {
            var array = Enumerable.Range(0, 255).Select(i => (byte)i).ToArray();
            foreach(var (cap, item) in new[] { (5, 3), (1, 1), (100, 1), (1, 100), (255, 1), (1, 255) }) {
                var stream = new BinaryChunkedStream(new BinaryChunkedList(cap, item));
                stream.Write(array, 40, 30);
                var a = stream.BinaryChunkedList.Take(30).ToArray();
                var b = array.Skip(40).Take(30).ToArray();
                CollectionAssert.AreEqual(a, b);

                stream.Write(array, 30, 255 - 30);
                var a2 = stream.BinaryChunkedList.Skip(30).Take(255 - 30).ToArray();
                var b2 = array.Skip(30).Take(255 - 30).ToArray();
                CollectionAssert.AreEqual(a2, b2);
            }
        }

    }
}
