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

        /// <summary>
        /// 格納データ。
        /// </summary>
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
    /// TODO: 致命的にバグってる, 直してみた気がしたけどあかんわ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChunkedList<T> : ICollection<T>, ICollection, IReadOnlyList<T>
    {
        public ChunkedList(int capacity, int blockSize)
        {
            if(capacity == 0) {
                throw new ArgumentException(nameof(capacity));
            }
            if(blockSize == 0) {
                throw new ArgumentException(nameof(blockSize));
            }

            BlockSize = blockSize;

            Blocks = new List<ChunkBlock<T>>(capacity);
        }

        #region property

        /// <summary>
        /// ブロック要素。
        /// </summary>
        List<ChunkBlock<T>> Blocks { get; }

        /// <summary>
        /// 内部要素(<see cref="ChunkBlock{T}"/>)のサイズ。
        /// </summary>
        public int BlockSize { get; }

        #endregion

        #region function

        /// <summary>
        /// 最後のブロックのインデックスを取得。
        /// </summary>
        /// <returns></returns>
        int GetLastBlockIndex()
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

        /// <summary>
        /// ブロックの生成。
        /// <para>生成オブジェクト変更用IF。</para>
        /// </summary>
        /// <returns></returns>
        protected virtual ChunkBlock<T> CreateBlock()
        {
            return new ChunkBlock<T>(BlockSize);
        }

        /// <summary>
        /// 指定したインデックスのブロックを生成。
        /// </summary>
        /// <param name="chunkItemIndex"></param>
        /// <returns></returns>
        ChunkBlock<T> GetOrCreateBlock(int chunkItemIndex)
        {
            Debug.Assert(chunkItemIndex <= Blocks.Count );

            if(Blocks.Count == chunkItemIndex) {
                var block = CreateBlock();
                Blocks.Add(block);
                return block;
            }

            return Blocks[chunkItemIndex];
        }

        public void CopyTo(int sourceIndex, T[] destination, int destinationIndex, int destinationLength)
        {
            var destinationDataLength = 0;
            var startBlockIndex = sourceIndex / BlockSize;

            for(var blockIndex = startBlockIndex; blockIndex < Blocks.Count; blockIndex++) {
                var targetBlock = Blocks[blockIndex];

                if(blockIndex == startBlockIndex) {
                    // 最初
                    var index = sourceIndex - startBlockIndex * BlockSize;
                    var blockFreeSize = BlockSize - index;
                    var length = Math.Min(destinationLength, blockFreeSize);
                    targetBlock.CopyTo(index, destination, destinationIndex, length);
                    destinationDataLength = length;
                } else if(blockIndex == Blocks.Count - 1) {
                    // 最後
                    var length = destinationLength - destinationDataLength;
                    targetBlock.CopyTo(0, destination, destinationDataLength + destinationIndex, length);
                    destinationDataLength += length;
                } else {
                    var dstIndex = destinationDataLength + destinationIndex;
                    var length = 0;
                    if(destinationDataLength + BlockSize <= destinationLength) {
                        length = BlockSize;
                    } else {
                        length = destinationLength - destinationDataLength;
                    }
                    targetBlock.CopyTo(0, destination, dstIndex, length);
                    destinationDataLength += length;
                }
                if(destinationLength <= destinationDataLength) {
                    break;
                }
            }
        }

        public void CopyFrom(int destinationIndex, T[] sourceArray, int sourceIndex, int sourceLength)
        {
            var sourceDataLength = 0;
            var blockCount = (sourceIndex + sourceLength) / BlockSize;
            if((sourceLength % BlockSize) != 0) {
                blockCount += 1;
            }
            var startBlockIndex = destinationIndex / BlockSize;

            for(var blockIndex = startBlockIndex; blockIndex < startBlockIndex + blockCount; blockIndex++) {
                var targetBlock = GetOrCreateBlock(blockIndex);
                if(BlockSize - targetBlock.Count < sourceLength) {
                    blockCount += 1;
                }

                if(blockIndex == startBlockIndex) {
                    // 最初
                    var index = destinationIndex - startBlockIndex * BlockSize;
                    var blockFreeSize = BlockSize - index;
                    var length = Math.Min(Math.Min(sourceArray.Length - sourceIndex, blockFreeSize), sourceLength);
                    targetBlock.CopyFrom(index, sourceArray, sourceIndex, length);
                    sourceDataLength = length;
                } else {
                    // 最後もクソもない
                    var srcIndex = sourceDataLength + sourceIndex;
                    var length = Math.Min(Math.Min(sourceArray.Length - srcIndex, BlockSize), sourceLength - sourceDataLength);
                    targetBlock.CopyFrom(0, sourceArray, srcIndex, length);
                    sourceDataLength += length;
                }
                if(sourceLength <= sourceDataLength) {
                    break;
                }
            }
            //Count = Math.Max(Count, destinationIndex + (sourceLength - sourceIndex));
            if(Blocks.Count != 0) {
                Count = ((Blocks.Count - 1) * BlockSize) + Blocks.Last().Count;
            } else {
                Count = 0;
            }
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
            var blockIndex = GetLastBlockIndex();
            var block = GetOrCreateBlock(blockIndex);
            block.Add(item);
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
            CopyTo(sourceIndex, (T[])array, 0, Count - sourceIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for(var i = 0; i < Blocks.Count; i++) {
                var block = Blocks[i];
                for(var j = 0; j < block.Count; j++) {
                    yield return block[j];
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
                var block = Blocks[i];
                if(block.Remove(item)) {
                    // 後ろの子らをずらす
                    var prevBlock = block;
                    for(var j = i + 1; j < Blocks.Count; j++) {
                        var nextBlock = Blocks[j];
                        prevBlock.Add(nextBlock[0]);
                        nextBlock.Remove(nextBlock[0]);
                        prevBlock = nextBlock;
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
        public BinaryChunkedList()
            : this(DefaultCapacity, DefaultBlockSize)
        { }

        public BinaryChunkedList(int capacity)
            : this(capacity, DefaultBlockSize)
        { }

        public BinaryChunkedList(int capacity, int blockSize)
            : base(capacity, blockSize)
        {
            if(LargeObjectHeapSize <= blockSize) {
                throw new ArgumentOutOfRangeException(nameof(blockSize));
            }
        }

        #region property

        public static int LargeObjectHeapSize { get; } = 85 * 1000;
        public static int DefaultCapacity { get; } = 255;
        public static int DefaultBlockSize { get; } = 80 * 1024;

        #endregion

        #region function

        #endregion
    }

    public class BinaryChunkedStream : Stream
    {
        public BinaryChunkedStream()
            : this(new BinaryChunkedList(Capacity, BinaryChunkedList.DefaultBlockSize))
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
