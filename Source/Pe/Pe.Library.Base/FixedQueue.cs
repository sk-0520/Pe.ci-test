using System;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    public interface IReadOnlyFixedQueue<T>: IEnumerable<T>
    {
        #region property

        /// <summary>
        /// 上限。
        /// </summary>
        int Limit { get; }

        /// <inheritdoc cref="Queue{T}.Count"/>
        int Count { get; }

        /// <summary>
        /// 空か。
        /// </summary>
        bool IsEmpty { get; }

        #endregion

        #region function

        /// <inheritdoc cref="Queue{T}.CopyTo(T[], int)"/>
        void CopyTo(T[] array, int index);
        /// <inheritdoc cref="Queue{T}.ToArray"/>
        T[] ToArray();

        /// <inheritdoc cref="Queue{T}.TryPeek(out T)"/>
        bool TryPeek([MaybeNullWhen(false)] out T result);

        #endregion
    }

    /// <summary>
    /// 最大数制限ありのキュー。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFixedQueue<T>: IReadOnlyFixedQueue<T>
    {
        #region function

        /// <inheritdoc cref="Queue{T}.Clear"/>
        void Clear();

        /// <inheritdoc cref="Queue{T}.Enqueue(T)"/>
        void Enqueue(T item);
        /// <inheritdoc cref="Queue{T}.TryDequeue(out T)"/>
        bool TryDequeue([MaybeNullWhen(false)] out T result);

        #endregion
    }

    /// <summary>
    /// <inheritdoc cref="IFixedQueue{T}"/>
    /// </summary>
    /// <seealso cref="Queue{T}"/>
    /// <typeparam name="T"></typeparam>
    public class FixedQueue<T>: IFixedQueue<T>
    {
        public FixedQueue(int limit)
        {
            if(limit < 1) {
                throw new ArgumentException(null, nameof(limit));
            }

            Limit = limit;
        }

        #region property

        private Queue<T> Queue { get; } = new Queue<T>();

        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.Limit"/>
        public int Limit { get; }

        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.Count"/>
        public int Count => Queue.Count;
        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.IsEmpty"/>
        public bool IsEmpty => Queue.Count == 0;

        #endregion

        #region function

        /// <inheritdoc cref="IFixedQueue{T}.Clear"/>
        public void Clear() => Queue.Clear();
        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.CopyTo(T[], int)"/>
        public void CopyTo(T[] array, int index) => Queue.CopyTo(array, index);
        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.ToArray"/>
        public T[] ToArray() => Queue.ToArray();

        /// <inheritdoc cref="IFixedQueue{T}.Enqueue(T)"/>
        public void Enqueue(T item)
        {
            Queue.Enqueue(item);
            while(Limit < Queue.Count) {
                Queue.TryDequeue(out _);
            }
        }

        /// <inheritdoc cref="IFixedQueue{T}.TryDequeue(out T)"/>
        public bool TryDequeue([MaybeNullWhen(false)] out T result) => Queue.TryDequeue(out result);

        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.TryPeek(out T)"/>
        public bool TryPeek([MaybeNullWhen(false)] out T result) => Queue.TryPeek(out result);


        #endregion

        #region IEnumerable

        /// <inheritdoc cref="Queue{T}.GetEnumerator"/>
        [SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public IEnumerator<T> GetEnumerator() => Queue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }

    /// <summary>
    /// スレッド セーフ<inheritdoc cref="IFixedQueue{T}"/>
    /// </summary>
    /// <seealso cref="ConcurrentQueue{T}"/>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentFixedQueue<T>: IFixedQueue<T>
    {
        public ConcurrentFixedQueue(int limit)
        {
            if(limit < 1) {
                throw new ArgumentException(null, nameof(limit));
            }

            Limit = limit;
        }

        #region property

        private ConcurrentQueue<T> Queue { get; } = new ConcurrentQueue<T>();

        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.Count"/>
        public int Limit { get; }

        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.Count"/>
        public int Count => Queue.Count;
        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.IsEmpty"/>
        public bool IsEmpty => Queue.IsEmpty;

        #endregion

        #region function

        /// <inheritdoc cref="IFixedQueue{T}.Clear"/>
        public void Clear() => Queue.Clear();
        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.CopyTo(T[], int)"/>
        public void CopyTo(T[] array, int index) => Queue.CopyTo(array, index);
        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.ToArray"/>
        public T[] ToArray() => Queue.ToArray();

        /// <inheritdoc cref="IFixedQueue{T}.Enqueue(T)"/>
        public void Enqueue(T item)
        {
            Queue.Enqueue(item);
            while(Limit < Queue.Count) {
                Queue.TryDequeue(out _);
            }
        }

        /// <inheritdoc cref="IFixedQueue{T}.TryDequeue(out T)"/>
        public bool TryDequeue([MaybeNullWhen(false)] out T result) => Queue.TryDequeue(out result);

        /// <inheritdoc cref="IReadOnlyFixedQueue{T}.TryPeek(out T)"/>
        public bool TryPeek([MaybeNullWhen(false)] out T result) => Queue.TryPeek(out result);

        #endregion

        #region IEnumerable

        /// <inheritdoc cref="ConcurrentQueue{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator() => Queue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
