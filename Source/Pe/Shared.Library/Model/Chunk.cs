using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public class ChunkItem<T> : ICollection<T>, ICollection, IReadOnlyList<T>
    {
        public ChunkItem(int capacity)
        {
            Items = new T[capacity];
        }

        #region property

        T[] Items { get; }

        public int Capacity => Items.Length;

        #endregion

        #region IReadOnlyList

        public T this[int index]
        {
            get
            {
                if(Count - 1 < index) {
                    throw new IndexOutOfRangeException();
                }

                return Items[index];
            }
            set
            {
                if(Count - 1 < index) {
                    throw new IndexOutOfRangeException();
                }

                Items[index] = value;
            }
        }

        #endregion

        #region ICollection

        public bool IsReadOnly => false;

        public int Count { get; private set; }

        public object SyncRoot => throw new NotSupportedException();

        public bool IsSynchronized => false;

        public void Add(T item)
        {
            if(Capacity == Count) {
                throw new OutOfMemoryException($"{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count}");
            }
            Items[Count++] = item;
        }

        public void Clear()
        {
            Array.Clear(Items, 0, Count);
            Count = 0;
        }

        public bool Contains(T item)
        {
            return Array.IndexOf(Items, item, 0, Count) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(Items, 0, array, arrayIndex, Count);
        }
        public void CopyTo(Array array, int index)
        {
            Array.Copy(Items, 0, array, index, Count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArraySegment<T>(Items, 0, Count).AsEnumerable().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T item)
        {
            var index = Array.IndexOf(Items, item, 0, Count);
            if(index == -1) {
                return false;
            }

            if(index < Count - 1) {
                Array.Copy(Items, index + 1, Items, index, Count - index - 1);
            }
            Items[Count - 1] = default(T);
            Count -= 1;

            return true;
        }

        #endregion

    }

    public class ChunkedList<T> : ICollection<T>, ICollection
    {
        public ChunkedList(int capacity, int itemCapacity)
        {
            Capacity = capacity;
            ChunkItemCapacity = itemCapacity;

            ChunkItems = new ChunkItem<T>[Capacity];
        }

        #region property

        ChunkItem<T>[] ChunkItems { get; }
        int ChunkItemCount { get; set; }

        public int Capacity { get; }
        public int ChunkItemCapacity { get; }

        ChunkItem<T> CurrentChunkItem { get; set; }
        int CurrentChunkItemIndex { get; set; }

        #endregion

        #region function

        int GetLastChunkItemIndex()
        {
            if(ChunkItemCount == 0) {
                return 0;
            }

            var count = Count;
            var index = 0;
            while(ChunkItemCapacity <= count) {
                count -= ChunkItemCapacity;
                index += 1;
            }

            return Count / ChunkItemCapacity;
        }

        ChunkItem<T> GetOrCreateChunkItem(int chunkItemIndex)
        {
            if(ChunkItemCount - 1 < chunkItemIndex) {
                if(ChunkItemCount == Capacity) {
                    throw new OutOfMemoryException($"{nameof(Capacity)}: {Capacity}, {nameof(ChunkItemCount)}: {ChunkItemCount}");
                }

                if(ChunkItems[chunkItemIndex] == null) {
                    var item = new ChunkItem<T>(ChunkItemCapacity);
                    ChunkItems[chunkItemIndex] = item;
                }
                ChunkItemCount += 1;
            }

            return ChunkItems[chunkItemIndex];
        }

        void AddCore(int chunkItemIndex, ChunkItem<T> chunkItem, T item)
        {
            chunkItem.Add(item);

            Count += 1;
            CurrentChunkItem = chunkItem;
            CurrentChunkItemIndex = chunkItemIndex;
        }

        #endregion

        #region ICollection

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        public object SyncRoot => throw new NotSupportedException();

        public bool IsSynchronized => false;

        public void Add(T item)
        {
            var chunkItemIndex = GetLastChunkItemIndex();
            var chunkItem = GetOrCreateChunkItem(chunkItemIndex);
            AddCore(chunkItemIndex, chunkItem, item);
        }

        public void Clear()
        {
            for(var i = 0; i < ChunkItems.Length; i++) {
                if(i == ChunkItemCount) {
                    break;
                }
                ChunkItems[i].Clear();
            }

            CurrentChunkItem = null;
            CurrentChunkItemIndex = 0;

            ChunkItemCount = 0;
            Count = 0;
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
