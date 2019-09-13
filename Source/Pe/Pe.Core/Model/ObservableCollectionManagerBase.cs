using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Model
{
    /// <summary>
    /// <see cref="ObservableCollection"/> の変更通知を受け取ってなんかする人。
    /// <para>管理者が誰かもうワケわからんことになるのです。</para>
    /// </summary>
    public abstract class ObservableCollectionManagerBase<TValue> : BindModelBase
    {
        private ObservableCollectionManagerBase(IReadOnlyList<TValue> collection, INotifyCollectionChanged collectionNotifyCollectionChanged, ILogger logger)
              : base(logger)
        {
            if(collection == null) {
                throw new ArgumentNullException(nameof(collection));
            }

            Collection = collection;
            CollectionNotifyCollectionChanged = collectionNotifyCollectionChanged;
            CollectionNotifyCollectionChanged.CollectionChanged += Collection_CollectionChanged;
        }

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

        public ObservableCollectionManagerBase(ReadOnlyObservableCollection<TValue> collection, ILogger logger)
            : this(collection, collection, logger)
        { }

        public ObservableCollectionManagerBase(ReadOnlyObservableCollection<TValue> collection, ILoggerFactory loggerFactory)
            : this(collection, collection, loggerFactory)
        { }

        public ObservableCollectionManagerBase(ObservableCollection<TValue> collection, ILogger logger)
            : this(collection, collection, logger)
        { }

        public ObservableCollectionManagerBase(ObservableCollection<TValue> collection, ILoggerFactory loggerFactory)
            : this(collection, collection, loggerFactory)
        { }


        #region property

        protected IReadOnlyList<TValue> Collection { get; private set; }
        protected INotifyCollectionChanged CollectionNotifyCollectionChanged { get; private set; }

        #endregion

        #region function

        protected abstract void AddItemsImple(IReadOnlyList<TValue> newItems);
        void AddItems(IReadOnlyList<TValue> newItems)
        {
            AddItemsImple(newItems);
        }

        protected abstract void RemoveItemsImpl(IReadOnlyList<TValue> oldItems, int oldStartingIndex);
        void RemoveItems(IReadOnlyList<TValue> oldItems, int oldStartingIndex)
        {
            RemoveItemsImpl(oldItems, oldStartingIndex);
        }

        protected abstract void ReplaceItemsImpl(IReadOnlyList<TValue> newItems, IReadOnlyList<TValue> oldItems);
        void ReplaceItems(IReadOnlyList<TValue> newItems, IReadOnlyList<TValue> oldItems)
        {
            ReplaceItemsImpl(newItems, oldItems);
        }

        protected abstract void MoveItemsImpl(int newStartingIndex, int oldStartingIndex);
        void MoveItems(int newStartingIndex, int oldStartingIndex)
        {
            MoveItemsImpl(newStartingIndex, oldStartingIndex);
        }

        protected abstract void ResetItemsImpl();
        void ResetItems()
        {
            ResetItemsImpl();
        }

        IReadOnlyList<TValue> ConvertList(IList list)
        {
            return list.Cast<TValue>().ToList();
        }

        protected virtual void CollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action) {
                case NotifyCollectionChangedAction.Add:
                    AddItems(ConvertList(e.NewItems));
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

        public int IndexOf(TValue value)
        {
            if(Collection is IList<TValue> list) {
                return list.IndexOf(value);
            }

            var items = Collection.Select((v, i) => (value: v, index: i));
            foreach(var item in items) {
                if(object.Equals(item.value, value)) {
                    return item.index;
                }
            }

            return -1;
        }

        #endregion

        #region ModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    CollectionNotifyCollectionChanged.CollectionChanged -= Collection_CollectionChanged;
#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                    CollectionNotifyCollectionChanged = null;
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
                }
#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                Collection = null;
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
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
