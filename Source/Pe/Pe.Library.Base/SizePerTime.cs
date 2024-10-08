using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// 時間単位でのオクテット使用量を算出。
    /// </summary>
    /// <remarks>
    /// <para><see cref="Size"/>を使っておけば幸せになれる。</para>
    /// <para>色々あったけど byte 基準(<see cref="SizeConverter"/>も参照のこと)。</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var sizePerTime = new SizePerTime(TimeSpan.FromSeconds(1));
    /// sizePerTime.Start();
    /// while(nowDownload) {
    ///     var values = download();
    ///     downloadSize = values.Length;
    ///     sizePerTime.Add(downloadSize);
    ///     Debug.WriteLine(sizePerTime.Size);
    /// }
    /// </code>
    /// </example>
    public class SizePerTime
    {
        /// <summary>
        /// 1秒間隔で生成。
        /// </summary>
        public SizePerTime()
            : this(TimeSpan.FromSeconds(1))
        { }

        /// <summary>
        /// 更新間隔時間を指定して生成。
        /// </summary>
        /// <param name="baseTime">更新間隔。</param>
        public SizePerTime(TimeSpan baseTime)
        {
            if(Timeout.InfiniteTimeSpan == baseTime) {
                throw new ArgumentException(nameof(Timeout) + "." + nameof(Timeout.InfiniteTimeSpan), nameof(baseTime));
            }

            BaseTime = baseTime;
        }

        #region property

        /// <summary>
        /// 時間単位。
        /// </summary>
        public TimeSpan BaseTime { get; }

        private Stopwatch Stopwatch { get; } = new Stopwatch();

        /// <summary>
        /// <see cref="BaseTime"/>中での使用量。
        /// </summary>
        protected long SizeInBaseTime { get; private set; }
        /// <summary>
        /// 前回の<see cref="BaseTime"/>中での使用量。
        /// </summary>
        protected long PrevSize { get; private set; }
        /// <summary>
        /// 直近の<see cref="BaseTime"/>での使用量。
        /// </summary>
        public long Size { get; protected set; }

        #endregion

        #region function

        /// <summary>
        /// 計測開始。
        /// </summary>
        public void Start()
        {
            Stopwatch.Restart();
            SizeInBaseTime = 0;
        }

        /// <summary>
        /// 使用容量の加算。
        /// </summary>
        /// <param name="addSize"></param>
        public void Add(long addSize)
        {
            Debug.Assert(Stopwatch.IsRunning);

            var elapsedTime = Stopwatch.Elapsed;

            if(BaseTime <= elapsedTime) {
                Size = SizeInBaseTime + addSize;

                PrevSize = Size;
                SizeInBaseTime = 0;

                Stopwatch.Restart();
            } else {
                Size = PrevSize;
                SizeInBaseTime += addSize;
            }
        }

        #endregion
    }
}
