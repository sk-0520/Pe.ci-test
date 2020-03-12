using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IFixedQueue<T>: IEnumerable<T>
    {
        #region property

        int Limit { get; }

        int Count { get; }
        bool IsEmpty { get; }

        #endregion

        #region function

        void Clear();
        void CopyTo(T[] array, int index);
        T[] ToArray();

        void Enqueue(T item);
        bool TryDequeue(out T result);
        bool TryPeek(out T result);

        #endregion
    }

    /// <summary>
    /// 最大数制限ありのキュー。
    /// <see cref="Queue{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FixedQueue<T> : IFixedQueue<T>
    {
        public FixedQueue(int limit)
        {
            if(limit < 1) {
                throw new ArgumentException(nameof(limit));
            }

            Limit = limit;
        }

        #region property

        private Queue<T> Queue { get; } = new Queue<T>();

        public int Limit { get; }

        /// <inheritdoc cref="Queue{T}.Count"/>
        public int Count { get; }
        /// <inheritdoc cref="Queue{T}.IsEmpty"/>
        public bool IsEmpty { get; }

        #endregion

        #region function

        /// <inheritdoc cref="Queue{T}.Clear"/>
        public void Clear() => Queue.Clear();
        /// <inheritdoc cref="Queue{T}.CopyTo(T[], int)"/>
        public void CopyTo(T[] array, int index) => Queue.CopyTo(array, index);
        /// <inheritdoc cref="Queue{T}.ToArray"/>
        public T[] ToArray() => Queue.ToArray();

        /// <inheritdoc cref="Queue{T}.Enqueue(T)"/>
        public void Enqueue(T item)
        {
            Queue.Enqueue(item);
            while(Limit < Queue.Count) {
                Queue.TryDequeue(out _);
            }
        }

        /// <inheritdoc cref="Queue{T}.TryDequeue(out T)"/>
        public bool TryDequeue(out T result) => Queue.TryDequeue(out result);

        /// <inheritdoc cref="Queue{T}.TryPeek(out T)"/>
        public bool TryPeek(out T result) => Queue.TryPeek(out result);

        #endregion

        #region IEnumerable

        /// <inheritdoc cref="Queue{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator() => Queue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }

    /// <summary>
    /// 最大数制限ありのキュー。
    /// <see cref="ConcurrentQueue{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentFixedQueue<T> : IFixedQueue<T>
    {
        public ConcurrentFixedQueue(int limit)
        {
            if(limit < 1) {
                throw new ArgumentException(nameof(limit));
            }

            Limit = limit;
        }

        #region property

        private ConcurrentQueue<T> Queue { get; } = new ConcurrentQueue<T>();

        public int Limit { get; }

        /// <inheritdoc cref="ConcurrentQueue{T}.Count"/>
        public int Count => Queue.Count;
        /// <inheritdoc cref="ConcurrentQueue{T}.IsEmpty"/>
        public bool IsEmpty => Queue.IsEmpty;

        #endregion

        #region function

        /// <inheritdoc cref="ConcurrentQueue{T}.Clear"/>
        public void Clear() => Queue.Clear();
        /// <inheritdoc cref="ConcurrentQueue{T}.CopyTo(T[], int)"/>
        public void CopyTo(T[] array, int index) => Queue.CopyTo(array, index);
        /// <inheritdoc cref="ConcurrentQueue{T}.ToArray"/>
        public T[] ToArray() => Queue.ToArray();

        /// <inheritdoc cref="ConcurrentQueue{T}.Enqueue(T)"/>
        public void Enqueue(T item)
        {
            Queue.Enqueue(item);
            while(Limit < Queue.Count) {
                Queue.TryDequeue(out _);
            }
        }

        /// <inheritdoc cref="ConcurrentQueue{T}.TryDequeue(out T)"/>
        public bool TryDequeue(out T result) => Queue.TryDequeue(out result);

        /// <inheritdoc cref="ConcurrentQueue{T}.TryPeek(out T)"/>
        public bool TryPeek(out T result) => Queue.TryPeek(out result);

        #endregion

        #region IEnumerable

        /// <inheritdoc cref="ConcurrentQueue{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator() => Queue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
