using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// <see cref="ObservableCollection"/> の変更通知を受け取ってなんかする人。
    /// <para>管理者が誰かもうワケわからんことになるのです。</para>
    /// </summary>
    public abstract class ObservableCollectionManagerBase<TValue> : BindModelBase
    {
        private ObservableCollectionManagerBase(IReadOnlyList<TValue> collection, INotifyCollectionChanged collectionNotifyCollectionChanged, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if(collection == null) {
                throw new ArgumentNullException(nameof(collection));
            }

            Collection = collection;
            CollectionNotifyCollectionChanged = collectionNotifyCollectionChanged;
            CollectionNotifyCollectionChanged.CollectionChanged += Collection_CollectionChanged;
        }

        public ObservableCollectionManagerBase(ReadOnlyObservableCollection<TValue> collection, ILoggerFactory loggerFactory)
            : this(collection, collection, loggerFactory)
        { }

        public ObservableCollectionManagerBase(ObservableCollection<TValue> collection, ILoggerFactory loggerFactory)
            : this(collection, collection, loggerFactory)
        { }


        #region property

        /// <summary>
        /// 管理対象コレクション。
        /// </summary>
        protected IReadOnlyList<TValue> Collection { get; private set; }
        /// <summary>
        /// <see cref="Collection"/> の変更通知。
        /// </summary>
        protected INotifyCollectionChanged CollectionNotifyCollectionChanged { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// アイテム追加処理実装部。
        /// </summary>
        /// <param name="newItems"></param>
        protected abstract void AddItemsImpl(IReadOnlyList<TValue> newItems);
        void AddItems(IReadOnlyList<TValue> newItems)
        {
            ThrowIfDisposed();

            AddItemsImpl(newItems);
        }

        /// <summary>
        /// アイテム挿入処理実装部。
        /// </summary>
        /// <param name="insertIndex"></param>
        /// <param name="newItems"></param>
        protected abstract void InsertItemsImpl(int insertIndex, IReadOnlyList<TValue> newItems);
        void InsertItems(int insertIndex, IReadOnlyList<TValue> newItems)
        {
            ThrowIfDisposed();

            InsertItemsImpl(insertIndex, newItems);
        }

        /// <summary>
        /// アイテム削除処理実装部。
        /// </summary>
        /// <param name="oldStartingIndex"></param>
        /// <param name="oldItems"></param>
        protected abstract void RemoveItemsImpl(int oldStartingIndex, IReadOnlyList<TValue> oldItems);
        void RemoveItems(IReadOnlyList<TValue> oldItems, int oldStartingIndex)
        {
            ThrowIfDisposed();

            RemoveItemsImpl(oldStartingIndex, oldItems);
        }

        /// <summary>
        /// アイテム置き換え処理実装部。
        /// </summary>
        /// <param name="newItems"></param>
        /// <param name="oldItems"></param>
        protected abstract void ReplaceItemsImpl(IReadOnlyList<TValue> newItems, IReadOnlyList<TValue> oldItems);
        void ReplaceItems(IReadOnlyList<TValue> newItems, IReadOnlyList<TValue> oldItems)
        {
            ThrowIfDisposed();

            ReplaceItemsImpl(newItems, oldItems);
        }

        /// <summary>
        /// アイテム移動処理実装部。
        /// </summary>
        /// <param name="newStartingIndex"></param>
        /// <param name="oldStartingIndex"></param>
        protected abstract void MoveItemsImpl(int newStartingIndex, int oldStartingIndex);
        void MoveItems(int newStartingIndex, int oldStartingIndex)
        {
            ThrowIfDisposed();

            MoveItemsImpl(newStartingIndex, oldStartingIndex);
        }

        /// <summary>
        /// コレクションリセット処理実装部。
        /// </summary>
        protected abstract void ResetItemsImpl();
        void ResetItems()
        {
            ThrowIfDisposed();

            ResetItemsImpl();
        }

        /// <summary>
        /// 非ジェネリックス<see cref="IList"/>を<see cref="List{TValue}"/>として扱う。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        IReadOnlyList<TValue> ConvertList(IList list)
        {
            return list.Cast<TValue>().ToList();
        }

        protected virtual void CollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action) {
                case NotifyCollectionChangedAction.Add:
                    if(e.NewStartingIndex == 0 && Collection.Count == 0) {
                        AddItems(ConvertList(e.NewItems));
                    } else {
                        InsertItems(e.NewStartingIndex, ConvertList(e.NewItems));
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    RemoveItems(ConvertList(e.OldItems), e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    ReplaceItems(ConvertList(e.NewItems), ConvertList(e.OldItems));
                    break;

                case NotifyCollectionChangedAction.Move:
                    MoveItems(e.NewStartingIndex, e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ResetItems();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// インデックスを取得。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(TValue value)
        {
            ThrowIfDisposed();

            if(Collection is IList<TValue> list) {
                return list.IndexOf(value);
            }

            var items = Collection.Counting();
            foreach(var item in items) {
                if(object.Equals(item.Value, value)) {
                    return item.Number;
                }
            }

            return -1;
        }


        #endregion

        #region ModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                CollectionNotifyCollectionChanged.CollectionChanged -= Collection_CollectionChanged;
                CollectionNotifyCollectionChanged = null!;
                Collection = null!;
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged(e);
        }

    }
}
