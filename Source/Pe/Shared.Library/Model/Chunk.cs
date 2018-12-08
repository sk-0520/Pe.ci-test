using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    /// <summary>
    /// 分割データ。
    /// <para>内部的に固定長で扱う。</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChunkItem<T> : ICollection<T>, ICollection, IReadOnlyList<T>
    {
        public ChunkItem(int size)
        {
            Items = new T[size];
        }

        #region property

        T[] Items { get; }

        public int Size => Items.Length;

        #endregion

        #region function

        public void CopyTo(int sourceIndex, Array destinationArray, int destinationIndex, int destinationLength)
        {
            Array.Copy(Items, sourceIndex, destinationArray, destinationIndex, destinationLength);
        }

        public void CopyFrom(int destinationIndex, Array sourceArray, int sourceIndex, int sourceLength)
        {
            if(destinationIndex != 0) {
                if(Count < destinationIndex) {
                    throw new ArgumentOutOfRangeException(nameof(destinationIndex));
                }
            }

            Array.Copy(sourceArray, sourceIndex, Items, destinationIndex, sourceLength);
            Count = Math.Max(destinationIndex + sourceLength, Count);
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
            if(Size == Count) {
                throw new OutOfMemoryException($"{nameof(Size)}: {Size}, {nameof(Count)}: {Count}");
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

    /// <summary>
    /// <see cref="ChunkItem{T}"/>のリスト。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChunkedList<T> : ICollection<T>, ICollection, IReadOnlyList<T>
    {
        public ChunkedList(int capacity, int itemSize)
        {
            Capacity = capacity;
            ChunkItemSize = itemSize;

            ChunkItems = new List<ChunkItem<T>>(Capacity);
        }

        #region property

        List<ChunkItem<T>> ChunkItems { get; }
        int ChunkItemCount { get; set; }

        public int Capacity { get; }
        public int ChunkItemSize { get; }

        #endregion

        #region function

        int GetLastChunkItemIndex()
        {
            if(ChunkItemCount == 0) {
                return 0;
            }

            var count = Count;
            var index = 0;
            while(ChunkItemSize <= count) {
                count -= ChunkItemSize;
                index += 1;
            }

            return Count / ChunkItemSize;
        }

        protected virtual ChunkItem<T> CreateChunkItem()
        {
            return new ChunkItem<T>(ChunkItemSize);
        }

        ChunkItem<T> GetOrCreateChunkItem(int chunkItemIndex)
        {
            if(ChunkItemCount - 1 < chunkItemIndex) {
                if(ChunkItemCount == ChunkItems.Count) {
                    //throw new OutOfMemoryException($"{nameof(Capacity)}: {Capacity}, {nameof(ChunkItemCount)}: {ChunkItemCount}");
                    ChunkItems.Add(null);
                }

                if(ChunkItems[chunkItemIndex] == null) {
                    var item = CreateChunkItem();
                    ChunkItems[chunkItemIndex] = item;
                }
                ChunkItemCount += 1;
            }

            return ChunkItems[chunkItemIndex];
        }

        public void CopyTo(int sourceIndex, Array destinationArray, int destinationIndex, int destinationLength)
        {
            var chunkItemIndex = sourceIndex / ChunkItemSize;
            var startSourceIndex = sourceIndex % ChunkItemSize;
            var destinationCount = destinationLength / ChunkItemSize + 1;

            var destinationDataLength = 0;
            for(int i = chunkItemIndex, j = 0; i < ChunkItemCount && j < destinationCount; i++) {
                var item = ChunkItems[i];
                var dataLength = j == destinationCount - 1
                    ? destinationLength - destinationDataLength
                    : ChunkItemSize - startSourceIndex
                ;

                item.CopyTo(
                    startSourceIndex,
                    destinationArray,
                    destinationIndex + destinationDataLength,
                    dataLength
                );
                destinationDataLength += dataLength;
                if(j == 0) {
                    startSourceIndex = 0;
                }
                j += 1;
            }
        }

        public void CopyFrom(int destinationIndex, Array sourceArray, int sourceIndex, int sourceLength)
        {

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

                var chunkItemIndex = index / ChunkItemSize;
                var workIndex = index - chunkItemIndex * ChunkItemSize;

                return ChunkItems[chunkItemIndex][workIndex];
            }
            set
            {
                if(Count - 1 < index) {
                    throw new IndexOutOfRangeException();
                }

                var chunkItemIndex = index / ChunkItemSize;
                var workIndex = index - chunkItemIndex * ChunkItemSize;

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
            chunkItem.Add(item);
            Count += 1;
        }

        public void Clear()
        {
            for(var i = 0; i < ChunkItemCount; i++) {
                ChunkItems[i].Clear();
            }

            ChunkItemCount = 0;
            Count = 0;
        }

        public bool Contains(T item)
        {
            for(var i = 0; i < ChunkItemCount; i++) {
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
            /*
            var itemIndex = sourceIndex / ChunkItemCapacity;
            var startIndex = sourceIndex % ChunkItemCapacity;
            */
            CopyTo(sourceIndex, array, 0, Count - sourceIndex);
            /*
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
            */
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

    /// <summary>
    /// LOHで悲しまない専用クラス。
    /// </summary>
    public class BinaryChunkedList : ChunkedList<byte>
    {
        public BinaryChunkedList(int capacity, int itemCapacity)
            : base(capacity, itemCapacity)
        {
            if(LargeObjectHeapSize <= itemCapacity) {
                throw new ArgumentOutOfRangeException(nameof(itemCapacity));
            }
        }

        #region static

        public static int LargeObjectHeapSize { get; set; } = 85 * 1024;

        #endregion
    }

    public class BinaryChunkedStream : Stream
    {
        public BinaryChunkedStream()
            : this(new BinaryChunkedList(Capacity, ItemCapacity))
        { }

        public BinaryChunkedStream(BinaryChunkedList binaryChunkedList)
        {
            BinaryChunkedList = binaryChunkedList;
        }

        #region property

        public static int Capacity { get; set; } = 255;
        public static int ItemCapacity { get; set; } = 80 * 1024;

        BinaryChunkedList BinaryChunkedList { get; }

        #endregion

        #region Stream

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length => BinaryChunkedList.Count;

        public override long Position { get; set; }

        public override void Flush()
        { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            BinaryChunkedList.CopyTo((int)Position, buffer, offset, count);
            Seek(count, SeekOrigin.Current);
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch(origin) {
                case SeekOrigin.Begin:
                    SetLength(offset);
                    break;

                case SeekOrigin.End:
                    SetLength(Position - offset);
                    break;

                case SeekOrigin.Current:
                    SetLength(Position + offset);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return Position;
        }

        public override void SetLength(long value)
        {
            Position = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
        }

        #endregion
    }
}
