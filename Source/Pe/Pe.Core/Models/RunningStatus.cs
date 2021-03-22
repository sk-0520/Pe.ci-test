using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 実行状態。
    /// </summary>
    public enum RunningState
    {
        /// <summary>
        /// 実行していない。
        /// </summary>
        None,
        /// <summary>
        /// 実行中。
        /// </summary>
        Running,
        /// <summary>
        /// 実行完了。
        /// <para>実行は終了している。</para>
        /// </summary>
        End,
        /// <summary>
        /// 実行取り消し。
        /// <para>実行は終了している。</para>
        /// </summary>
        Cancel,
        /// <summary>
        /// 実行中エラー。
        /// <para>実行は終了している。</para>
        /// </summary>
        Error,
    }

    /// <summary>
    /// 通知可能な実行状態。
    /// </summary>
    public interface IRunningStatus: INotifyPropertyChanged
    {
        #region property

        /// <summary>
        /// 実行状態。
        /// </summary>
        RunningState State { get; }

        #endregion
    }

    /// <summary>
    /// 実行状態。
    /// </summary>
    public class RunningStatus: BindModelBase, IRunningStatus
    {
        #region variable

        RunningState _state;

        #endregion

        public RunningStatus(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region IRunningStatus

        /// <inheritdoc cref="IRunningStatus.State"/>
        public RunningState State
        {
            get => this._state;
            set => SetProperty(ref this._state, value);
        }

        #endregion
    }
}
