using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.ViewModels
{
    // 保守したくはないけどテストくらいはまぁ。。。 という気持ち
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

            protected override void ReplaceItemsImpl(int startIndex, IReadOnlyList<T> newItems, IReadOnlyList<T> oldItems)
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

        [Fact]
        public void NoneTest()
        {
            var collection = Create(1, 2, 3);
            Assert.Equal(LastAction.None, collection.Manager.LastAction);
        }

        [Fact]
        public void Constructor_throw_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new TestObservableCollectionManager<int>((ReadOnlyObservableCollection<int>)null!));
        }

        [Fact]
        public void AddTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source.Add(4);
            Assert.Equal(LastAction.Add, collection.Manager.LastAction);
        }

        [Fact]
        public void InsertTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source.Insert(1, 4);
            Assert.Equal(LastAction.Insert, collection.Manager.LastAction);
        }

        [Fact]
        public void InsertIsAddTest()
        {
            var collection = Create(1, 2, 3);
            // 終端挿入は追加扱い
            collection.Source.Insert(3, 4);
            Assert.Equal(LastAction.Add, collection.Manager.LastAction);
        }

        [Fact]
        public void MoveTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source.Move(1, 2);
            Assert.Equal(LastAction.Move, collection.Manager.LastAction);
        }

        [Fact]
        public void RemoveTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source.Remove(2);
            Assert.Equal(LastAction.Remove, collection.Manager.LastAction);
        }

        [Fact]
        public void ReplaceTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source[1] = 20;
            Assert.Equal(LastAction.Replace, collection.Manager.LastAction);
        }

        [Fact]
        public void ResetTest()
        {
            var collection = Create(1, 2, 3);
            collection.Source.Clear();
            Assert.Equal(LastAction.Reset, collection.Manager.LastAction);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 20)]
        [InlineData(2, 30)]
        [InlineData(-1, 0)]
        [InlineData(-1, 40)]
        public void IndexOfTest(int expected, int value)
        {
            var collection = Create(10, 20, 30);
            var actual = collection.Manager.IndexOf(value);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
