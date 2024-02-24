using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class ObservableCollectionManagerBaseTest
    {
        #region define

        private enum LastAction
        {
            None,
            Add,
            Insert,
            Move,
            Remove,
            Replace,
            Reset,
        }

        private class TestObservableCollectionManager<T>: ObservableCollectionManagerBase<T>
        {
            public TestObservableCollectionManager(ReadOnlyObservableCollection<T> collection)
                : base(collection)
            { }

            public TestObservableCollectionManager(ObservableCollection<T> collection)
                : base(collection)
            { }

            public LastAction LastAction { get; private set; }

            protected override void AddItemsImpl(IReadOnlyList<T> newItems)
            {
                LastAction = LastAction.Add;
            }

            protected override void InsertItemsImpl(int insertIndex, IReadOnlyList<T> newItems)
            {
                LastAction = LastAction.Insert;
            }

            protected override void MoveItemsImpl(int newStartingIndex, int oldStartingIndex)
            {
                LastAction = LastAction.Move;
            }

            protected override void RemoveItemsImpl(int oldStartingIndex, IReadOnlyList<T> oldItems)
            {
                LastAction = LastAction.Remove;
            }

            protected override void ReplaceItemsImpl(IReadOnlyList<T> newItems, IReadOnlyList<T> oldItems)
            {
                LastAction = LastAction.Replace;
            }

            protected override void ResetItemsImpl()
            {
                LastAction = LastAction.Reset;
            }
        }

        private class Collections<T>
        {
            public Collections(IEnumerable<T> source)
            {
                Source = new ObservableCollection<T>(source);
                Manager = new TestObservableCollectionManager<T>(Source);
            }

            public ObservableCollection<T> Source { get; }
            public TestObservableCollectionManager<T> Manager { get; }
        }

        #endregion

        #region function

        private Collections<T> Create<T>(params T[] args)
        {
            return new Collections<T>(args);
        }

        [TestMethod]
        public void NoneTest()
        {
            var collection = Create(1, 2, 3);
            Assert.AreEqual(LastAction.None, collection.Manager.LastAction);
        }

        [TestMethod]
        public void AddTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source.Add(4);
            Assert.AreEqual(LastAction.Add, collection.Manager.LastAction);
        }

        [TestMethod]
        public void InsertTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source.Insert(1, 4);
            Assert.AreEqual(LastAction.Insert, collection.Manager.LastAction);
        }

        [TestMethod]
        public void InsertIsAddTest()
        {
            var collection = Create(1, 2, 3);
            // 終端挿入は追加扱い
            collection.Source.Insert(3, 4);
            Assert.AreEqual(LastAction.Add, collection.Manager.LastAction);
        }

        [TestMethod]
        public void MoveTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source.Move(1, 2);
            Assert.AreEqual(LastAction.Move, collection.Manager.LastAction);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source.Remove(2);
            Assert.AreEqual(LastAction.Remove, collection.Manager.LastAction);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source[1] = 20;
            Assert.AreEqual(LastAction.Replace, collection.Manager.LastAction);
        }

        [TestMethod]
        public void ResetTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source.Clear();
            Assert.AreEqual(LastAction.Reset, collection.Manager.LastAction);
        }

        [TestMethod]
        [DataRow(0, 10)]
        [DataRow(1, 20)]
        [DataRow(2, 30)]
        [DataRow(-1, 0)]
        [DataRow(-1, 40)]
        public void IndexOfTest(int expected, int value)
        {
            var collection = Create(10, 20, 30);
            var actual = collection.Manager.IndexOf(value);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
