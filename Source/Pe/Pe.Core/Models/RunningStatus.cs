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
        /// </summary>
        /// <remarks>
        /// <para>実行は終了している。</para>
        /// </remarks>
        End,
        /// <summary>
        /// 実行取り消し。
        /// </summary>
        /// <remarks>
        /// <para>実行は終了している。</para>
        /// </remarks>
        Cancel,
        /// <summary>
        /// 実行中エラー。
        /// </summary>
        /// <remarks>
        /// <para>実行は終了している。</para>
        /// </remarks>
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

        private RunningState _state;

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
