using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using ContentTypeTextNet.Pe.Standard.Base.Models;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// <see cref="ObservableCollection{TValue}"/> の変更通知を受け取ってなんかする人。
    /// <para>管理者が誰かもうワケわからんことになるのです。</para>
    /// </summary>
    public abstract class ObservableCollectionManagerBase<TValue>: NotifyPropertyBase
    {
        private ObservableCollectionManagerBase(IReadOnlyList<TValue> collection, INotifyCollectionChanged collectionNotifyCollectionChanged)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));

            CollectionNotifyCollectionChanged = collectionNotifyCollectionChanged;
            CollectionNotifyCollectionChanged.CollectionChanged += Collection_CollectionChanged;
        }

        protected ObservableCollectionManagerBase(ReadOnlyObservableCollection<TValue> collection)
            : this(collection, collection)
        { }

        protected ObservableCollectionManagerBase(ObservableCollection<TValue> collection)
            : this(collection, collection)
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
        private void AddItems(IReadOnlyList<TValue> newItems)
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
        private void InsertItems(int insertIndex, IReadOnlyList<TValue> newItems)
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
        private void RemoveItems(IReadOnlyList<TValue> oldItems, int oldStartingIndex)
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
        private void ReplaceItems(IReadOnlyList<TValue> newItems, IReadOnlyList<TValue> oldItems)
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
        private void MoveItems(int newStartingIndex, int oldStartingIndex)
        {
            ThrowIfDisposed();

            MoveItemsImpl(newStartingIndex, oldStartingIndex);
        }

        /// <summary>
        /// コレクションリセット処理実装部。
        /// </summary>
        protected abstract void ResetItemsImpl();

        private void ResetItems()
        {
            ThrowIfDisposed();

            ResetItemsImpl();
        }

        /// <summary>
        /// 非ジェネリックス<see cref="IList"/>を<see cref="List{TValue}"/>として扱う。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IReadOnlyList<TValue> ConvertList(IList list)
        {
            return list.Cast<TValue>().ToList();
        }

        protected virtual void CollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action) {
                case NotifyCollectionChangedAction.Add:
                    if(e.NewStartingIndex == 0 && Collection.Count == 0) {
                        if(e.NewItems != null) {
                            AddItems(ConvertList(e.NewItems));
                        }
                    } else {
                        if(e.NewItems != null) {
                            InsertItems(e.NewStartingIndex, ConvertList(e.NewItems));
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if(e.OldItems != null) {
                        RemoveItems(ConvertList(e.OldItems), e.OldStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if(e.NewItems != null && e.OldItems != null) {
                        ReplaceItems(ConvertList(e.NewItems), ConvertList(e.OldItems));
                    }
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
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

        #region NotifyPropertyBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed && Collection != null) {
                CollectionNotifyCollectionChanged.CollectionChanged -= Collection_CollectionChanged;
                CollectionNotifyCollectionChanged = null!;
                Collection = null!;
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Collection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged(e);
        }
    }
}
