using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Transactions;
using System.Xml.Schema;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
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
        /// <param name="this"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static bool IsLive(this IReadOnlyCronItemSetting @this, DateTime timestamp)
        {
            return false;
        }

        #endregion
    }

    [DateTimeKind(DateTimeKind.Local)]
    public class CronItemSetting: IReadOnlyCronItemSetting
    {
        public CronItemSetting()
        { }

        internal CronItemSetting(int minutes, int hour, int day, int month)
        {
            Minutes.Add(EnforceMinutes(minutes, nameof(minutes)));
            Hours.Add(EnforceHour(hour, nameof(hour)));
            Days.Add(EnforceDay(day, nameof(day)));
            Months.Add(EnforceMonth(month, nameof(month)));
            DayOfWeeks.AddRange(EnumUtility.GetMembers<DayOfWeek>());
        }

        internal CronItemSetting(int minutes, int hour, int day, int month, DayOfWeek dayOfWeek)
        {
            Minutes.Add(EnforceMinutes(minutes, nameof(minutes)));
            Hours.Add(EnforceHour(hour, nameof(hour)));
            Days.Add(EnforceDay(day, nameof(day)));
            Months.Add(EnforceMonth(month, nameof(month)));
            DayOfWeeks.Add(dayOfWeek);
        }

        #region function

        static int Enforce(int value, int min, int max, string valueArgumentName)
        {
            if(min <= value && value <= max) {
                return value;
            }

            throw new ArgumentException(valueArgumentName);
        }

        static int EnforceMinutes(int value, string valueArgumentName) => Enforce(value, 0, 59, valueArgumentName);
        static int EnforceHour(int value, string valueArgumentName) => Enforce(value, 0, 23, valueArgumentName);
        /// <summary>
        /// 年情報がないのでうるう年とか月最終日は知らん。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueArgumentName"></param>
        /// <returns></returns>
        static int EnforceDay(int value, string valueArgumentName) => Enforce(value, 1, 31, valueArgumentName);
        static int EnforceMonth(int value, string valueArgumentName) => Enforce(value, 1, 12, valueArgumentName);

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

        public MultipleExecuteMode Mode { get; }

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

        #region function

        Random Random { get; }

        #endregion

        #region function

        IEnumerable<int> ConvertRange(string value, int min, int max)
        {
            if(value == "*") {
                return Enumerable.Range(min, max);
            } else if(value.Contains('/')) {
                var elements = value.Split('/');
                if(elements.Length != 2) {
                    throw new ArgumentException(value);
                }
                var a = elements[0];
                var b = elements[1];
                throw new NotImplementedException("やるべきこと #550 に対してやること多すぎるからちょっと今はこれ以上しんどいので気が向いたら対応する");
            } else {
                var numericRange = new NumericRange(false, ",", "-");
                return numericRange.Parse(value);
            }
        }

        IEnumerable<DayOfWeek> ConvertWeek(string value)
        {
            if(value == "*") {
                return EnumUtility.GetMembers<DayOfWeek>();
            } else {
                var numericRange = new NumericRange(false, ",", "-");
                return numericRange.Parse(value)
                    .Select(i => i == 7 ? 0: i) // 7 は 日曜日(0) 判定
                    .Select(i => (DayOfWeek)i)
                ;
            }
        }

        bool TryParseCore(string cronPattern, [NotNullWhen(false)] out Exception? resultException, [NotNullWhen(true)] out CronItemSetting? resultSetting)
        {
            if(string.IsNullOrWhiteSpace(cronPattern)) {
                resultException = new ArgumentException(nameof(cronPattern));
                resultSetting = default;
                return false;
            }

            var values = cronPattern.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
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

        Task ExecuteAsyn(CancellationToken cancellationToken);

        #endregion
    }

    internal class CronJob
    {
        public CronJob(Guid cronItemId, IReadOnlyCronItemSetting setting, ICronExecutor executor)
        {
            CronItemId = cronItemId;
            Setting = setting;
            Executor = executor;
        }

        #region property

        public Guid CronItemId { get; }
        public IReadOnlyCronItemSetting Setting { get; }
        public ICronExecutor Executor { get; }

        public CancellationTokenSource? CancellationTokenSource { get; set; }

        #endregion
    }

    /// <summary>
    /// 1分間隔で処理を監視し、ジョブを起動させる。
    /// </summary>
    public class CronScheduler: DisposerBase
    {
        #region define

        class CronExecutorWrapper: ICronExecutor
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

            public Task ExecuteAsyn(CancellationToken cancellationToken) => Executor(cancellationToken);

            #endregion
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        public CronScheduler(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        ISet<CronJob> Jobs { get; } = new HashSet<CronJob>();

        System.Timers.Timer? Timer { get; set; }

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

        CronJob AddScheduleCore(IReadOnlyCronItemSetting setting, ICronExecutor executor)
        {
            var cronItemId = Guid.NewGuid();
            var item = new CronJob(cronItemId, setting, executor);
            Jobs.Add(item);
            return item;
        }

        public Guid AddSchedule(IReadOnlyCronItemSetting setting, ICronExecutor executor)
        {
            return AddScheduleCore(setting, executor).CronItemId;
        }

        public Guid AddSchedule(IReadOnlyCronItemSetting setting, Func<CancellationToken, Task> executor)
        {
            return AddScheduleCore(setting, new CronExecutorWrapper(executor)).CronItemId;
        }

        public Guid AddSchedule(IReadOnlyCronItemSetting setting, Func<Task> executor)
        {
            return AddScheduleCore(setting, new CronExecutorWrapper(executor)).CronItemId;
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Stop();
                }
            }

            base.Dispose(disposing);
        }

        internal IEnumerable<CronJob> GetItemFromTime([DateTimeKind(DateTimeKind.Local)] DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        internal void ExecuteItems(IEnumerable<CronJob> items)
        {

        }

        #endregion

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.Assert(Timer != null);
            Timer.Stop();

            var items = GetItemFromTime(e.SignalTime);
            ExecuteItems(items);

            Timer.Start();
        }


    }
}
