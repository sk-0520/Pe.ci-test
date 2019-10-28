using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 分割データ。
    /// <para>内部的に固定長で扱う。</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChunkBlock<T> : ICollection<T>, ICollection, IReadOnlyList<T>
    {
        public ChunkBlock(int size)
        {
            if(size == 0) {
                throw new ArgumentException(nameof(size));
            }

            Items = new T[size];
        }

        #region property

        T[] Items { get; }

        /// <summary>
        /// 予約済みサイズ。
        /// </summary>
        public int Size => Items.Length;

        #endregion

        #region function

        public void CopyTo(int sourceIndex, T[] destinationArray, int destinationIndex, int destinationLength)
        {
            Array.Copy(Items, sourceIndex, destinationArray, destinationIndex, destinationLength);
        }

        public void CopyFrom(int destinationIndex, T[] sourceArray, int sourceIndex, int sourceLength)
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

        /// <summary>
        /// 実際に使用しているサイズ。
        /// </summary>
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

        void ICollection.CopyTo(Array destinationArray, int sourceIndex)
        {
            CopyTo(sourceIndex, (T[])destinationArray, 0, Count - sourceIndex);
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
            Items[Count - 1] = default(T)!;
            Count -= 1;

            return true;
        }

        #endregion

    }

    /// <summary>
    /// <see cref="ChunkBlock{T}"/>のリスト。
    /// <para><see cref="CopyTo"/>, <see cref="CopyFrom"/>で力尽きた。</para>
    /// TODO: 致命的にバグってる
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChunkedList<T> : ICollection<T>, ICollection, IReadOnlyList<T>
    {
        public ChunkedList(int capacity, int blockSize)
        {
            Capacity = capacity;
            BlockSize = blockSize;

            Blocks = new List<ChunkBlock<T>>(Capacity);
        }

        #region property

        /// <summary>
        /// ブロック要素。
        /// </summary>
        List<ChunkBlock<T>> Blocks { get; }
        /// <summary>
        /// 現在のブロック数。
        /// </summary>
        //int BlockItemCount { get; set; }

        /// <summary>
        /// ブロック予約サイズ。
        /// </summary>
        public int Capacity { get; }
        /// <summary>
        /// 内部要素(<see cref="ChunkBlock{T}"/>)の予約サイズ。
        /// </summary>
        public int BlockSize { get; }

        #endregion

        #region function

        int GetLastChunkItemIndex()
        {
            if(Blocks.Count == 0) {
                return 0;
            }

            var count = Count;
            var index = 0;
            while(BlockSize <= count) {
                count -= BlockSize;
                index += 1;
            }

            return Count / BlockSize;
        }

        protected virtual ChunkBlock<T> CreateChunkItem()
        {
            return new ChunkBlock<T>(BlockSize);
        }

        ChunkBlock<T> GetOrCreateChunkItem(int chunkItemIndex)
        {
            /*
            if(BlockItemCount - 1 < chunkItemIndex) {
                if(BlockItemCount == Blocks.Count) {
                    //throw new OutOfMemoryException($"{nameof(Capacity)}: {Capacity}, {nameof(ChunkItemCount)}: {ChunkItemCount}");
#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                    Blocks.Add(null);
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
                }

                if(Blocks[chunkItemIndex] == null) {
                    var item = CreateChunkItem();
                    Blocks[chunkItemIndex] = item;
                }
                BlockItemCount += 1;
            }
            */
            Debug.Assert(chunkItemIndex <= Blocks.Count );

            if(Blocks.Count == chunkItemIndex) {
                var block = CreateChunkItem();
                Blocks.Add(block);
                return block;
            }

            return Blocks[chunkItemIndex];
        }

        public void CopyTo(int sourceIndex, T[] destinationArray, int destinationIndex, int destinationLength)
        {
            var chunkItemIndex = sourceIndex / BlockSize;
            var startSourceIndex = sourceIndex % BlockSize;
            var destinationCount = destinationLength / BlockSize + 1;

            var destinationDataLength = 0;
            for(int i = chunkItemIndex, j = 0; i < Blocks.Count && j < destinationCount; i++) {
                var item = Blocks[i];
                var dataLength = j == destinationCount - 1
                    ? destinationLength - destinationDataLength
                    : BlockSize - startSourceIndex
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

        //public void _CopyTo(int sourceIndex, Span<T> destination, int destinationIndex, int destinationLength)
        //{
        //    var destinationDataLength = 0;
        //    var startBlockIndex = sourceIndex / BlockSize;
        //    for(var blockIndex = startBlockIndex; blockIndex < Blocks.Count; blockIndex++) {
        //        if(blockIndex == startBlockIndex) {
        //            var block = Blocks[blockIndex];
        //            var index = sourceIndex - startBlockIndex * BlockSize;
        //            block.CopyTo()
        //            destination.CopyTo()
        //        }

        //    }
        //}

        public void CopyFrom(int destinationIndex, Array sourceArray, int sourceIndex, int sourceLength)
        {
            var chunkItemIndex = destinationIndex / BlockSize;
            var startDestinationIndex = destinationIndex % BlockSize;
            var sourceCount = chunkItemIndex + sourceLength / BlockSize + (sourceLength % BlockSize != 0 ? 1 : 0);
            var sourceDataLength = 0;
            for(int i = chunkItemIndex, j = 0; i < sourceCount; i++) {
                var dataLength = j == sourceCount - 1 || sourceLength < BlockSize
                    ? sourceLength - sourceDataLength
                    : BlockSize - startDestinationIndex
                ;
                if(dataLength == 0) {
                    break;
                }
                if(sourceArray.Length < sourceIndex + sourceDataLength + dataLength) {
                    dataLength = sourceLength - sourceDataLength;
                }

                var chunkItem = GetOrCreateChunkItem(i);
                chunkItem.CopyFrom(
                    startDestinationIndex,
                    (T[])sourceArray,
                    sourceIndex + sourceDataLength,
                    dataLength
                );
                sourceDataLength += dataLength;
                if(j == 0) {
                    startDestinationIndex = 0;
                }
                j += 1;
            }
            Count = Math.Max(Count, destinationIndex + sourceIndex + sourceDataLength);
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

                var chunkItemIndex = index / BlockSize;
                var workIndex = index - chunkItemIndex * BlockSize;

                return Blocks[chunkItemIndex][workIndex];
            }
            set
            {
                if(Count - 1 < index) {
                    throw new IndexOutOfRangeException();
                }

                var chunkItemIndex = index / BlockSize;
                var workIndex = index - chunkItemIndex * BlockSize;

                Blocks[chunkItemIndex][workIndex] = value;
            }
        }

        #endregion

        #region ICollection

        /// <summary>
        /// 実際に使用しているサイズ。
        /// </summary>
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
            for(var i = 0; i < Blocks.Count; i++) {
                Blocks[i].Clear();
            }

            Blocks.Clear();
            Count = 0;
        }

        public bool Contains(T item)
        {
            for(var i = 0; i < Blocks.Count; i++) {
                if(Blocks[i].Contains(item)) {
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
            CopyTo(sourceIndex, (T[])array, 0, Count - sourceIndex);
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
            for(var i = 0; i < Blocks.Count; i++) {
                var item = Blocks[i];
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
            for(var i = 0; i < Blocks.Count; i++) {
                var chunkItem = Blocks[i];
                var chunkCount = chunkItem.Count;
                if(chunkItem.Remove(item)) {
                    // 後ろの子らをずらす
                    var prevChunkItem = chunkItem;
                    for(var j = i + 1; j < Blocks.Count; j++) {
                        var nextChunkItem = Blocks[j];
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

        #region property

        public static int LargeObjectHeapSize { get; } = 85 * 1000;
        public static int DefaultChunkSize { get; } = 80 * 1024;

        public int ChunkSize { get; } = DefaultChunkSize;

        #endregion

        #region function

        #endregion
    }

    public class BinaryChunkedStream : Stream
    {
        public BinaryChunkedStream()
            : this(new BinaryChunkedList(Capacity, BinaryChunkedList.DefaultChunkSize))
        { }

        public BinaryChunkedStream(BinaryChunkedList binaryChunkedList)
        {
            BinaryChunkedList = binaryChunkedList;
        }

        #region property

        public static int Capacity { get; } = 255;

        public BinaryChunkedList BinaryChunkedList { get; }

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
            BinaryChunkedList.CopyFrom((int)Position, buffer, offset, count);
            Seek(count, SeekOrigin.Current);
        }

        #endregion
    }
}
