using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ContentTypeTextNet.Pe.Core.Models;

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

    public class CronItemSetting: IReadOnlyCronItemSetting
    {
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

    public interface ICronExecutor
    {
        #region property

        #endregion

        #region function

        Task ExecuteAsyn(CancellationToken cancellationToken);

        #endregion
    }

    internal class CronItem
    {
        public CronItem(Guid cronItemId, IReadOnlyCronItemSetting setting, ICronExecutor executor)
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
        /// <param name="pollingTime"><see cref="PollingTime" /></param>
        /// <param name="excludeTime"><see cref="ExcludeTime"/></param>
        public CronScheduler(TimeSpan pollingTime, TimeSpan excludeTime)
        {
            PollingTime = pollingTime;
            ExcludeTime = excludeTime;
        }

        #region property

        /// <summary>
        /// 監視間隔。
        /// </summary>
        public TimeSpan PollingTime { get; }
        /// <summary>
        /// アイテム実行時に指定時間からこの時間を超過しているものを対象外とする。
        /// <para>停止後に実行すると一気に実行アイテムが流れそうなのでそれを抑制。</para>
        /// </summary>
        public TimeSpan ExcludeTime { get; }

        ISet<CronItem> Items { get; } = new HashSet<CronItem>();

        System.Timers.Timer? Timer { get; set; }

        public bool IsRunning => Timer != null && Timer.Enabled;

        #endregion

        #region function

        public void Start()
        {
            if(IsRunning) {
                throw new InvalidOperationException(nameof(IsRunning));
            }

            Timer = new System.Timers.Timer() {
                Interval = PollingTime.TotalMilliseconds,
            };
            Timer.Elapsed += Timer_Elapsed;
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

        CronItem AddScheduleCore(IReadOnlyCronItemSetting setting, ICronExecutor executor)
        {
            var cronItemId = Guid.NewGuid();
            var item = new CronItem(cronItemId, setting, executor);
            Items.Add(item);
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

        #endregion

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }


    }
}
