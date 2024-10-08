using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public enum ScheduleKind
    {
        Plain,
        Minutes,
        Hour,
        Days,
        Months,
    }

    /// <summary>
    /// 二重起動時の処理方法。
    /// </summary>
    public enum MultipleExecuteMode
    {
        /// <summary>
        /// 起動しない。
        /// </summary>
        Skip,
        /// <summary>
        /// ただ起動する。
        /// </summary>
        Start,
        /// <summary>
        /// 前処理をキャンセル（努力目標）して実行。
        /// </summary>
        CancelAndStart,
    }

    [DateTimeKind(DateTimeKind.Local)]
    public interface IReadOnlyCronItemSetting
    {
        #region property

        /// <summary>
        /// 分 一覧。
        /// </summary>
        IReadOnlyList<int> Minutes { get; }
        /// <summary>
        /// 時間 一覧。
        /// </summary>
        IReadOnlyList<int> Hours { get; }
        /// <summary>
        /// 日 一覧。
        /// </summary>
        IReadOnlyList<int> Days { get; }
        /// <summary>
        /// 月 一覧。
        /// </summary>
        IReadOnlyList<int> Months { get; }
        /// <summary>
        /// 曜日 一覧。
        /// </summary>
        IReadOnlyList<DayOfWeek> DayOfWeeks { get; }

        /// <summary>
        /// 同一アイテムが実行中の場合の処理。
        /// </summary>
        MultipleExecuteMode Mode { get; }

        #endregion
    }

    public static class IReadOnlyCronItemSettingExtensions
    {
        #region function

        /// <summary>
        /// アイテムが時間から有効であるか判定。
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static bool IsEnabled(this IReadOnlyCronItemSetting setting, DateTime timestamp)
        {
            static bool IsIn<T>(T value, IEnumerable<T> range)
                where T : struct
            {
                foreach(var element in range) {
                    if(value.Equals(element)) {
                        return true;
                    }
                }

                return false;
            }

            if(!IsIn(timestamp.Minute, setting.Minutes)) {
                return false;
            }
            if(!IsIn(timestamp.Hour, setting.Hours)) {
                return false;
            }

            if(setting.DayOfWeeks.Count != 7) {
                if(IsIn(timestamp.DayOfWeek, setting.DayOfWeeks)) {
                    // 曜日指定があれば日は無視して月確認
                    if(IsIn(timestamp.Month, setting.Months)) {
                        return true;
                    }
                } else if(setting.Days.Count == 31) {
                    // 曜日指定ありで日指定なしの場合は曜日に合わない時点で一致しないと判定
                    return false;
                }
            }

            if(!IsIn(timestamp.Day, setting.Days)) {
                return false;
            }

            if(!IsIn(timestamp.Month, setting.Months)) {
                return false;
            }

            return true;
        }

        #endregion
    }

    /// <inheritdoc cref="IReadOnlyCronItemSetting"/>
    [DateTimeKind(DateTimeKind.Local)]
    public class CronItemSetting: IReadOnlyCronItemSetting
    {
        public CronItemSetting()
        { }

        internal CronItemSetting(int minutes, int hour, int day, int month)
        {
            Minutes.Add(ThrowIfNotMinutes(minutes, nameof(minutes)));
            Hours.Add(ThrowIfNotHour(hour, nameof(hour)));
            Days.Add(ThrowIfNotDay(day, nameof(day)));
            Months.Add(ThrowIfNotMonth(month, nameof(month)));
            DayOfWeeks.AddRange(Enum.GetValues<DayOfWeek>());
        }

        internal CronItemSetting(int minutes, int hour, int day, int month, DayOfWeek dayOfWeek)
        {
            Minutes.Add(ThrowIfNotMinutes(minutes, nameof(minutes)));
            Hours.Add(ThrowIfNotHour(hour, nameof(hour)));
            Days.Add(ThrowIfNotDay(day, nameof(day)));
            Months.Add(ThrowIfNotMonth(month, nameof(month)));
            DayOfWeeks.Add(dayOfWeek);
        }

        #region function

        private static int ThrowIfNotInMinMax(int value, int min, int max, string valueArgumentName)
        {
            if(min <= value && value <= max) {
                return value;
            }

            throw new ArgumentException(null, valueArgumentName);
        }

        private static int ThrowIfNotMinutes(int value, string valueArgumentName) => ThrowIfNotInMinMax(value, 0, 59, valueArgumentName);
        private static int ThrowIfNotHour(int value, string valueArgumentName) => ThrowIfNotInMinMax(value, 0, 23, valueArgumentName);
        /// <summary>
        /// 年情報がないのでうるう年とか月最終日は知らん。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueArgumentName"></param>
        /// <returns></returns>
        private static int ThrowIfNotDay(int value, string valueArgumentName) => ThrowIfNotInMinMax(value, 1, 31, valueArgumentName);
        private static int ThrowIfNotMonth(int value, string valueArgumentName) => ThrowIfNotInMinMax(value, 1, 12, valueArgumentName);

        #endregion

        #region IReadOnlyCronItemSetting

        /// <inheritdoc cref="IReadOnlyCronItemSetting.Minutes"/>
        public List<int> Minutes { get; set; } = new List<int>();
        IReadOnlyList<int> IReadOnlyCronItemSetting.Minutes => Minutes;

        /// <inheritdoc cref="IReadOnlyCronItemSetting.Hours"/>
        public List<int> Hours { get; set; } = new List<int>();
        IReadOnlyList<int> IReadOnlyCronItemSetting.Hours => Hours;

        /// <inheritdoc cref="IReadOnlyCronItemSetting.Days"/>
        public List<int> Days { get; set; } = new List<int>();
        IReadOnlyList<int> IReadOnlyCronItemSetting.Days => Days;

        /// <inheritdoc cref="IReadOnlyCronItemSetting.Months"/>
        public List<int> Months { get; set; } = new List<int>();
        IReadOnlyList<int> IReadOnlyCronItemSetting.Months => Months;

        /// <inheritdoc cref="IReadOnlyCronItemSetting.DayOfWeeks"/>
        public List<DayOfWeek> DayOfWeeks { get; set; } = new List<DayOfWeek>();
        IReadOnlyList<DayOfWeek> IReadOnlyCronItemSetting.DayOfWeeks => DayOfWeeks;

        public MultipleExecuteMode Mode { get; set; }

        #endregion
    }

    public class CronItemSettingFactory
    {
        public CronItemSettingFactory(int seed)
        {
            Random = new Random(seed);
        }

        public CronItemSettingFactory()
            : this(Environment.TickCount)
        { }

        #region property

        private Random Random { get; }

        #endregion

        #region function

        private IEnumerable<int> ConvertRange(string value, int min, int max)
        {
            if(value == "*") {
                return Enumerable.Range(min, max);
            } else if(value.Contains('/')) {
                var elements = value.Split('/');
                if(elements.Length != 2) {
                    throw new ArgumentException(null, value);
                }
                var a = elements[0];
                var b = elements[1];
                throw new NotImplementedException("やるべきこと #550 に対してやること多すぎるからちょっと今はこれ以上しんどいので気が向いたら対応する");
            } else {
                var numericRange = new NumericRange(false, ",", "-");
                return numericRange.Parse(value);
            }
        }

        private IEnumerable<DayOfWeek> ConvertWeek(string value)
        {
            if(value == "*") {
                return Enum.GetValues<DayOfWeek>();
            } else {
                var numericRange = new NumericRange(false, ",", "-");
                return numericRange.Parse(value)
                    .Select(i => i == 7 ? 0 : i) // 7 は 日曜日(0) 判定
                    .Select(i => (DayOfWeek)i)
                ;
            }
        }

        private bool TryParseCore(string cronPattern, [NotNullWhen(false)] out Exception? resultException, [NotNullWhen(true)] out CronItemSetting? resultSetting)
        {
            if(string.IsNullOrWhiteSpace(cronPattern)) {
                resultException = new ArgumentException(null, nameof(cronPattern));
                resultSetting = default;
                return false;
            }

            var values = cronPattern.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
            if(values.Length == 1 && values[0][0] == '@') {
                switch(values[0]) {
                    case "@hourly":
                        resultException = default;
                        resultSetting = new CronItemSetting();
                        resultSetting.Minutes.Add(Random.Next(1, 59));
                        return true;

                    case "@daily":
                        resultException = default;
                        resultSetting = new CronItemSetting();
                        resultSetting.Minutes.Add(Random.Next(1, 59));
                        resultSetting.Hours.Add(Random.Next(0, 23));
                        return true;

                    default:
                        resultException = new Exception($"{nameof(cronPattern)}: {values[0]}");
                        resultSetting = default;
                        return false;
                }
            }
            if(values.Length != 5) {
                resultException = new Exception($"{nameof(cronPattern)}: {cronPattern} -> {values.Length}");
                resultSetting = default;
                return false;
            }

            var minutes = values[0];
            var hour = values[1];
            var day = values[2];
            var month = values[3];
            var week = values[4];

            try {
                var setting = new CronItemSetting();

                setting.Minutes.AddRange(ConvertRange(minutes, 0, 60));
                setting.Hours.AddRange(ConvertRange(hour, 0, 24));
                setting.Days.AddRange(ConvertRange(day, 1, 31));
                setting.Months.AddRange(ConvertRange(month, 1, 12));
                setting.DayOfWeeks.AddRange(ConvertWeek(week));

                resultException = default;
                resultSetting = setting;
                return true;
            } catch(Exception ex) {
                resultException = ex;
                resultSetting = default;
                return false;
            }
        }

        public bool TryParse(string cronPattern, [NotNullWhen(true)] out CronItemSetting? result)
        {
            return TryParseCore(cronPattern, out _, out result);
        }

        public CronItemSetting Parse(string cronPattern)
        {
            if(TryParseCore(cronPattern, out var ex, out var result)) {
                return result;
            }
            throw ex;
        }

        #endregion
    }

    public interface ICronExecutor
    {
        #region property

        #endregion

        #region function

        Task ExecuteAsync(CancellationToken cancellationToken);

        #endregion
    }

    /// <summary>
    /// 状態を持ち歩くジョブ。
    /// <para>TODO: 内部で<see cref="CancellationTokenSource"/>をどうこうするのはなんかちゃうよなぁ。</para>
    /// </summary>
    internal class CronJob: DisposerBase
    {
        public CronJob(ScheduleJobId scheduleJobId, IReadOnlyCronItemSetting setting, ICronExecutor executor, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            ScheduleJobId = scheduleJobId;
            Setting = setting;
            Executor = executor;
        }

        #region property

        private ILogger Logger { get; }

        /// <summary>
        /// ジョブ自身のID。
        /// </summary>
        public ScheduleJobId ScheduleJobId { get; }

        /// <summary>
        /// 設定。
        /// </summary>
        public IReadOnlyCronItemSetting Setting { get; }

        /// <summary>
        /// 実行処理。
        /// </summary>
        private ICronExecutor Executor { get; }

        /// <summary>
        /// 自身の実行キャンセル通知。
        /// </summary>
        private CancellationTokenSource? CancellationTokenSource { get; set; }

        /// <summary>
        /// 最後に実行した時間。
        /// </summary>
        /// <remarks>
        /// <para>実行を開始した時間で実行終了の時間ではない。</para>
        /// </remarks>
        [DateTimeKind(DateTimeKind.Local)]
        public DateTime LastExecuteTimestamp { get; private set; } = DateTime.MinValue;
        /// <summary>
        /// 現在実行中か。
        /// </summary>
        public bool IsRunning { get; private set; }

        #endregion

        #region function

        public Task ExecuteAsync()
        {
            if(IsRunning) {
                switch(Setting.Mode) {
                    case MultipleExecuteMode.Skip:
                        Logger.LogInformation("実行中ジョブのため未実行, {0}", ScheduleJobId);
                        return Task.CompletedTask;

                    case MultipleExecuteMode.Start:
                        Logger.LogTrace("実行中ジョブの重複実行, {0}", ScheduleJobId);
                        break;

                    case MultipleExecuteMode.CancelAndStart: {
                            Logger.LogTrace("実行中ジョブ停止からの実行, {0}", ScheduleJobId);
                            var cts = CancellationTokenSource;
                            if(cts != null) {
                                try {
                                    cts.Cancel();
                                } catch(Exception ex) {
                                    Logger.LogError(ex, "ジョブキャンセル失敗のため未実行, {0}, {1}", ex.Message, ScheduleJobId);
                                    return Task.CompletedTask;
                                }
                            }
                            break;
                        }

                    default:
                        throw new NotImplementedException();
                }
            }

            if(Setting.Mode == MultipleExecuteMode.CancelAndStart) {
                CancellationTokenSource?.Dispose();
                CancellationTokenSource = new CancellationTokenSource();
            }

            var cancelToken = CancellationTokenSource?.Token ?? CancellationToken.None;

            IsRunning = true;
            LastExecuteTimestamp = DateTime.Now;

            return Executor.ExecuteAsync(cancelToken).ContinueWith(t => {
                if(t.IsCompletedSuccessfully) {
                    if(!IsRunning) {
                        Logger.LogWarning("実行終了したが実行中フラグが実行していないことになっている, {0}", ScheduleJobId);
                    }

                    IsRunning = false;
                    CancellationTokenSource?.Dispose();
                    CancellationTokenSource = null;
                } else if(t.Exception != null) {
                    Logger.LogError(t.Exception, "{0}", t.Exception.Message);
                }
            });
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(CancellationTokenSource != null) {
                        if(!CancellationTokenSource.IsCancellationRequested) {
                            try {
                                CancellationTokenSource.Cancel();
                            } catch(AggregateException ex) {
                                Logger.LogWarning(ex, "{0}, {1}", ex.Message, ScheduleJobId);
                            }
                        }
                        CancellationTokenSource.Dispose();
                        CancellationTokenSource = null;
                    }
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// 1分間隔で処理を監視し、ジョブを起動させる。
    /// </summary>
    public class CronScheduler: DisposerBase
    {
        #region define

        private sealed class CronExecutorWrapper: ICronExecutor
        {
            public CronExecutorWrapper(Func<CancellationToken, Task> executor)
            {
                Executor = executor;
            }

            public CronExecutorWrapper(Func<Task> executor)
            {
                Executor = c => executor();
            }

            #region property

            Func<CancellationToken, Task> Executor { get; }

            #endregion

            #region ICronExecutor

            public Task ExecuteAsync(CancellationToken cancellationToken) => Executor(cancellationToken);

            #endregion
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        public CronScheduler(IIdFactory idFactory, ILoggerFactory loggerFactory)
        {
            IdFactory = idFactory;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <inheritdoc cref="IIdFactory "/>
        private IIdFactory IdFactory { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        private ISet<CronJob> Jobs { get; } = new HashSet<CronJob>();

        private System.Timers.Timer? Timer { get; set; }

        public bool IsRunning => Timer != null && Timer.Enabled;

        #endregion

        #region function

        /// <summary>
        /// 次回実行時までの待機時間を判定。
        /// </summary>
        /// <returns></returns>
        internal TimeSpan GetNextJobWaitTime([DateTimeKind(DateTimeKind.Local)] DateTime nowTime)
        {
            //var nextTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, nowTime.Hour, nowTime.Minute, 0).AddMinutes(1);
            var nextTime = new DateTime(nowTime.Ticks - (nowTime.Ticks % TimeSpan.TicksPerMinute), DateTimeKind.Local).AddMinutes(1);
            return nextTime - nowTime;
        }

        public void Start()
        {
            if(IsRunning) {
                throw new InvalidOperationException(nameof(IsRunning));
            }

            Timer = new System.Timers.Timer();
            Timer.Elapsed += Timer_Elapsed;
            Timer.Interval = GetNextJobWaitTime(DateTime.Now).TotalMilliseconds;

            Timer.Start();
        }

        public void Stop()
        {
            if(!IsRunning) {
                throw new InvalidOperationException(nameof(IsRunning));
            }

            Debug.Assert(Timer != null);

            Timer.Elapsed -= Timer_Elapsed;
            Timer.Stop();
            Timer.Dispose();
            Timer = null;
        }

        private CronJob AddScheduleCore(IReadOnlyCronItemSetting setting, ICronExecutor executor)
        {
            var scheduleJobId = IdFactory.CreateScheduleJobId();
            var item = new CronJob(scheduleJobId, setting, executor, LoggerFactory);
            Jobs.Add(item);
            return item;
        }

        public ScheduleJobId AddSchedule(IReadOnlyCronItemSetting setting, ICronExecutor executor)
        {
            return AddScheduleCore(setting, executor).ScheduleJobId;
        }

        public ScheduleJobId AddSchedule(IReadOnlyCronItemSetting setting, Func<CancellationToken, Task> executor)
        {
            return AddScheduleCore(setting, new CronExecutorWrapper(executor)).ScheduleJobId;
        }

        public ScheduleJobId AddSchedule(IReadOnlyCronItemSetting setting, Func<Task> executor)
        {
            return AddScheduleCore(setting, new CronExecutorWrapper(executor)).ScheduleJobId;
        }

        /// <summary>
        /// 指定のスケジュールを破棄。
        /// </summary>
        /// <remarks>
        /// <para>実行中のものは実行しっぱで参照が切れる。</para>
        /// </remarks>
        /// <param name="scheduleJobId"></param>
        /// <returns></returns>
        public bool RemoveSchedule(ScheduleJobId scheduleJobId)
        {
            var job = Jobs.FirstOrDefault(i => i.ScheduleJobId == scheduleJobId);
            if(job == null) {
                return false;
            }
            return Jobs.Remove(job);
        }

        /// <summary>
        /// すべてのスケジュールを破棄。
        /// </summary>
        /// <remarks>
        /// <para>実行中のものは実行しっぱで参照が切れる。</para>
        /// </remarks>
        public void ClearAllSchedule()
        {
            Jobs.Clear();
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(IsRunning) {
                        Stop();
                    }
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// 時間及び設定から実行可能なジョブ一覧を取得。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private IReadOnlyList<CronJob> GetJobsByTime([DateTimeKind(DateTimeKind.Local)] DateTime dateTime)
        {
            // ここで実行中だからどうとかは行わない
            return Jobs
                .Where(i => i.LastExecuteTimestamp < dateTime) // 試してもしてないけど時刻変更対応
                .Where(i => i.Setting.IsEnabled(dateTime))
                .ToList()
            ;
        }

        private void ExecuteJobs(IEnumerable<CronJob> jobs)
        {
            foreach(var job in jobs) {
                Logger.LogTrace("ジョブ実行: {0} = {1}, {2} = {3}, {4}", nameof(job.Setting.Mode), job.Setting.Mode, nameof(job.IsRunning), job.IsRunning, job.ScheduleJobId);
                job.ExecuteAsync().ConfigureAwait(false);
            }
        }

        #endregion

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Debug.Assert(Timer != null);
            Timer.Stop();

            var jobs = GetJobsByTime(e.SignalTime);
            ExecuteJobs(jobs);

            Timer.Interval = GetNextJobWaitTime(DateTime.Now).TotalMilliseconds;
            Timer.Start();
        }
    }
}
