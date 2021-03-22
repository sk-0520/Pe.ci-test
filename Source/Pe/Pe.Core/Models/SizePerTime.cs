using System;
using System.Diagnostics;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 時間単位でのオクテット使用量を算出。
    /// <para><see cref="Size"/>を使っておけば幸せになれる。</para>
    /// <para>色々あったけど byte 基準。</para>
    /// </summary>
    /// <example>
    /// <code>
    /// var sizePerTime = new SizePerTime(TimeSpan.FromSeconds(1));
    /// sizePerTime.Start();
    /// while(nowDownload) {
    ///     var values = download();
    ///     donwloadSize = values.Length;
    ///     sizePerTime.Add(donwloadSize);
    ///     Debug.WriteLine(sizePerTime.Size);
    /// }
    /// </code>
    /// </example>
    public class SizePerTime
    {
        public SizePerTime(TimeSpan baseTime)
        {
            BaseTime = baseTime;
        }

        #region property

        /// <summary>
        /// 時間単位。
        /// </summary>
        public TimeSpan BaseTime { get; }

        Stopwatch TimeStopWatch { get; } = new Stopwatch();

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
            TimeStopWatch.Restart();
            SizeInBaseTime = 0;
        }

        /// <summary>
        /// 使用容量の加算。
        /// </summary>
        /// <param name="addSize"></param>
        public void Add(long addSize)
        {
            Debug.Assert(TimeStopWatch.IsRunning);

            var elapsedTime = TimeStopWatch.Elapsed;

            if(BaseTime <= elapsedTime) {
                Size = SizeInBaseTime + addSize;

                PrevSize = Size;
                SizeInBaseTime = 0;

                TimeStopWatch.Restart();
            } else {
                Size = PrevSize;
                SizeInBaseTime += addSize;
            }
        }

        #endregion

    }
}
