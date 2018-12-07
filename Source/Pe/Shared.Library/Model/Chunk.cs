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

        #region function

        void CopyTo(int sourceIndex, Array destinationArray, int destinationIndex, int destinationLength)
        {
            Array.Copy(Items, sourceIndex, destinationArray, destinationIndex, destinationLength);
        }

        public void CopyTo(T[] destinationArray, int sourceIndex, int destinationIndex)
        {
            CopyTo(sourceIndex, destinationArray, destinationIndex, Count - sourceIndex);
        }
        public void CopyTo(Array destinationArray, int sourceIndex, int destinationIndex)
        {
            CopyTo(sourceIndex, destinationArray, destinationIndex, Count - sourceIndex);
        }

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

        public void CopyTo(T[] destinationArray, int sourceIndex)
        {
            CopyTo(sourceIndex, destinationArray, 0, Count - sourceIndex);
        }
        public void CopyTo(Array destinationArray, int sourceIndex)
        {
            CopyTo(sourceIndex, destinationArray, 0, Count - sourceIndex);
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

    public class ChunkedList<T> : ICollection<T>, ICollection, IReadOnlyList<T>
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

        protected virtual ChunkItem<T> CreateChunkItem()
        {
            return new ChunkItem<T>(ChunkItemCapacity);
        }

        ChunkItem<T> GetOrCreateChunkItem(int chunkItemIndex)
        {
            if(ChunkItemCount - 1 < chunkItemIndex) {
                if(ChunkItemCount == Capacity) {
                    throw new OutOfMemoryException($"{nameof(Capacity)}: {Capacity}, {nameof(ChunkItemCount)}: {ChunkItemCount}");
                }

                if(ChunkItems[chunkItemIndex] == null) {
                    var item = CreateChunkItem();
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
        }

        #endregion

        #region IReadOnlyList

        public T this[int index]
        {
            get
            {
                if(Count - 1 < index) {
                    throw new IndexOutOfRangeException();
                }

                var chunkItemIndex = index / ChunkItemCapacity;
                var workIndex = index - chunkItemIndex * ChunkItemCapacity;

                return ChunkItems[chunkItemIndex][workIndex];
            }
            set
            {
                if(Count - 1 < index) {
                    throw new IndexOutOfRangeException();
                }

                var chunkItemIndex = index / ChunkItemCapacity;
                var workIndex = index - chunkItemIndex * ChunkItemCapacity;

                ChunkItems[chunkItemIndex][workIndex] = value;
            }
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

            ChunkItemCount = 0;
            Count = 0;
        }

        public bool Contains(T item)
        {
            for(var i = 0; i < ChunkItems.Length; i++) {
                if(i == ChunkItemCount) {
                    return false;
                }

                if(ChunkItems[i].Contains(item)) {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int sourceIndex)
        {
            CopyTo((Array)array, sourceIndex);
        }
        public void CopyTo(Array array, int sourceIndex)
        {
            var itemIndex = sourceIndex / ChunkItemCapacity;
            var startIndex = sourceIndex % ChunkItemCapacity;
            var destAddIndex = 0;
            for(int i = 0, j = 0; i < ChunkItemCount; i++) {
                if(i < itemIndex) {
                    continue;
                }
                var item = ChunkItems[i];
                item.CopyTo(array, startIndex, j * ChunkItemCapacity - destAddIndex);
                if(j == 0) {
                    destAddIndex = startIndex;
                    startIndex = 0;
                }
                j += 1;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for(var i = 0; i < ChunkItemCount; i++) {
                var item = ChunkItems[i];
                for(var j = 0; j < item.Count; j++) {
                    yield return item[j];
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T item)
        {
            for(var i = 0; i < ChunkItemCount; i++) {
                var chunkItem = ChunkItems[i];
                var chunkCount = chunkItem.Count;
                if(chunkItem.Remove(item)) {
                    // 後ろの子らをずらす
                    var prevChunkItem = chunkItem;
                    for(var j = i + 1; j < ChunkItemCount; j++) {
                        var nextChunkItem = ChunkItems[j];
                        prevChunkItem.Add(nextChunkItem[0]);
                        nextChunkItem.Remove(nextChunkItem[0]);
                        prevChunkItem = nextChunkItem;
                    }
                    Count -= 1;
                    return true;
                }
            }

            return false;
        }


        #endregion

    }
}
