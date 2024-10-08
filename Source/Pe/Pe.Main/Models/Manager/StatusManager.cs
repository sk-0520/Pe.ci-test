using System;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    /// <summary>
    /// ステータス変更モード。
    /// </summary>
    public enum StatusChangedMode
    {
        /// <summary>
        /// 永続的変更。
        /// </summary>
        Changed,
        /// <summary>
        /// 一時的変更。
        /// </summary>
        TemporaryChanged,
        /// <summary>
        /// 一時的変更の戻し。
        /// </summary>
        TemporaryRestore,
    }

    public enum StatusProperty
    {
        /// <summary>
        /// 💩＜ｳﾝﾁ!
        /// </summary>
        Unknown,
        /// <summary>
        /// <see cref="IStatusManager.CanCallNotifyAreaMenu"/>
        /// </summary>
        CanCallNotifyAreaMenu,
    }

    public class StatusChangedEventArgs: EventArgs
    {
        public StatusChangedEventArgs(StatusChangedMode mode, StatusProperty statusProperty, Type valueType, object? oldValue, object? newValue)
        {
            Mode = mode;
            ValueType = valueType;
            OldValue = oldValue;
            NewValue = newValue;
            StatusProperty = statusProperty;
        }

        #region property

        public StatusChangedMode Mode { get; }
        public Type ValueType { get; }
        public StatusProperty StatusProperty { get; }
        public object? OldValue { get; }
        public object? NewValue { get; }

        #endregion
    }


    /// <summary>
    /// アプリケーションの状態管理。
    /// </summary>
    public interface IStatusManager
    {
        #region event

        event EventHandler<StatusChangedEventArgs> StatusChanged;

        #endregion

        #region function

        IResult ChangeBoolean(StatusProperty statusProperty, bool newValue);
        IResultSuccess<IDisposable> ChangeLimitedBoolean(StatusProperty statusProperty, bool newValue);

        #endregion

        #region property

        bool CanCallNotifyAreaMenu { get; }

        #endregion

        #region function




        #endregion
    }

    public class StatusManager: ManagerBase, IStatusManager
    {
        public StatusManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region property

        #endregion

        #region function

        private void OnStatusChanged(StatusChangedMode mode, StatusProperty statusProperty, Type valueType, object? oldValue, object? newValue)
        {
            var e = new StatusChangedEventArgs(mode, statusProperty, valueType, oldValue, newValue);
            StatusChanged?.Invoke(this, e);
        }

        private void OnStatusChanged<T>(StatusChangedMode mode, StatusProperty statusProperty, T oldValue, T newValue)
        {
            OnStatusChanged(mode, statusProperty, typeof(T), oldValue, newValue);
        }

        #endregion

        #region IStatusManager

        public event EventHandler<StatusChangedEventArgs>? StatusChanged;

        public IResult ChangeBoolean(StatusProperty statusProperty, bool newValue)
        {
            var oldValue = statusProperty switch {
                StatusProperty.CanCallNotifyAreaMenu => CanCallNotifyAreaMenu,
                _ => throw new NotImplementedException(),
            };

            if(oldValue == newValue) {
                return new Result(false);
            }

            switch(statusProperty) {
                case StatusProperty.CanCallNotifyAreaMenu:
                    CanCallNotifyAreaMenu = newValue;
                    break;

                default:
                    throw new NotImplementedException();
            }

            OnStatusChanged(StatusChangedMode.Changed, statusProperty, oldValue, newValue);

            return new Result(true);
        }

        public IResultSuccess<IDisposable> ChangeLimitedBoolean(StatusProperty statusProperty, bool newValue)
        {
            var oldValue = statusProperty switch {
                StatusProperty.CanCallNotifyAreaMenu => CanCallNotifyAreaMenu,
                _ => throw new NotImplementedException(),
            };

            if(oldValue == newValue) {
                return Result.CreateFailure<IDisposable>();
            }

            switch(statusProperty) {
                case StatusProperty.CanCallNotifyAreaMenu:
                    CanCallNotifyAreaMenu = newValue;
                    break;

                default:
                    throw new NotImplementedException();
            }

            OnStatusChanged(StatusChangedMode.TemporaryChanged, statusProperty, oldValue, newValue);

            return Result.CreateSuccess((IDisposable)new ActionDisposer(d => {
                switch(statusProperty) {
                    case StatusProperty.CanCallNotifyAreaMenu:
                        CanCallNotifyAreaMenu = oldValue;
                        break;

                    default:
                        throw new NotImplementedException();
                }

#pragma warning disable S2234 // 戻し処理の通知なのでパラメータ名がテレコになってるのはOK
                OnStatusChanged(StatusChangedMode.TemporaryRestore, statusProperty, newValue, oldValue);
#pragma warning restore S2234
            }));
        }

        public bool CanCallNotifyAreaMenu { get; private set; } = true;


        #endregion
    }
}
