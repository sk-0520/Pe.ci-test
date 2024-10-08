using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// <see langword="foreach" /> でくるくる回ってるよ！
    /// </summary>
    public interface ICounter
    {
        #region property

        /// <summary>
        /// 最大数。
        /// </summary>
        int MaxCount { get; }
        /// <summary>
        /// 現在数。
        /// </summary>
        int CurrentCount { get; }

        /// <summary>
        /// ループ中の初回か。
        /// </summary>
        bool IsFirst { get; }
        /// <summary>
        /// ループ中の最終か。
        /// </summary>
        bool IsLast { get; }

        /// <summary>
        /// ループ完了か。
        /// </summary>
        bool IsCompleted { get; }

        #endregion
    }

    /// <inheritdoc cref="ICounter"/>
    public class Counter: ICounter, IEnumerable<ICounter>
    {
        public Counter(int maxCount)
        {
            MaxCount = maxCount;
        }

        #region function

        /// <summary>
        /// 強制的にカウントアップ。
        /// </summary>
        /// <returns>カウントアップできたか。</returns>
        public bool Increment()
        {
            if(CurrentCount == MaxCount) {
                return false;
            }

            CurrentCount += 1;
            IsCompleted = CurrentCount == MaxCount;
            return true;
        }

        #endregion

        #region ICounter

        /// <inheritdoc cref="ICounter.MaxCount" />
        public int MaxCount { get; }

        /// <inheritdoc cref="ICounter.CurrentCount" />
        public int CurrentCount { get; private set; } = 1;

        /// <inheritdoc cref="ICounter" />
        public bool IsFirst => CurrentCount == 1;
        /// <inheritdoc cref="ICounter.IsLast" />
        public bool IsLast => CurrentCount == MaxCount;

        /// <inheritdoc cref="ICounter.IsCompleted" />
        public bool IsCompleted { get; private set; }

        #endregion

        #region IEnumerable

        /// <inheritdoc cref="IEnumerable.GetEnumerator()"/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{ICounter}.GetEnumerator()"/>
        public IEnumerator<ICounter> GetEnumerator()
        {
            IsCompleted = false;

            for(; CurrentCount <= MaxCount; CurrentCount += 1) {
                yield return this;
            }

            IsCompleted = true;
        }

        #endregion
    }
}
