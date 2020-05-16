using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
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

    public class StatusChangedEventArgs : EventArgs
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
        IResultSuccessValue<IDisposable> ChangeLimitedBoolean(StatusProperty statusProperty, bool newValue);

        #endregion

        #region property

        bool CanCallNotifyAreaMenu { get; }

        #endregion

        #region function




        #endregion
    }

    public class StatusManager : ManagerBase, IStatusManager
    {
        public StatusManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region property

        #endregion

        #region function

        void OnStatusChanged(StatusChangedMode mode, StatusProperty statusProperty, Type valueType, object? oldValue, object? newValue)
        {
            var e = new StatusChangedEventArgs(mode, statusProperty, valueType, oldValue, newValue);
            StatusChanged?.Invoke(this, e);
        }

        void OnStatusChanged<T>(StatusChangedMode mode, StatusProperty statusProperty, T oldValue, T newValue)
        {
            OnStatusChanged(mode, statusProperty, typeof(T), oldValue, newValue);
        }

        #endregion

        #region IStatusManager

        public event EventHandler<StatusChangedEventArgs>? StatusChanged;

        public IResult ChangeBoolean(StatusProperty statusProperty, bool value)
        {
            var oldValue = statusProperty switch
            {
                StatusProperty.CanCallNotifyAreaMenu => CanCallNotifyAreaMenu,
                _ => throw new NotImplementedException(),
            };

            if(oldValue == value) {
                return new Result(false);
            }

            switch(statusProperty) {
                case StatusProperty.CanCallNotifyAreaMenu:
                    CanCallNotifyAreaMenu = value;
                    break;

                default:
                    throw new NotImplementedException();
            }

            OnStatusChanged(StatusChangedMode.Changed, statusProperty, oldValue, value);

            return new Result(true);
        }

        public IResultSuccessValue<IDisposable> ChangeLimitedBoolean(StatusProperty statusProperty, bool value)
        {
            var oldValue = statusProperty switch
            {
                StatusProperty.CanCallNotifyAreaMenu => CanCallNotifyAreaMenu,
                _ => throw new NotImplementedException(),
            };

            if(oldValue == value) {
                return ResultSuccessValue.Failure<IDisposable>();
            }

            switch(statusProperty) {
                case StatusProperty.CanCallNotifyAreaMenu:
                    CanCallNotifyAreaMenu = value;
                    break;

                default:
                    throw new NotImplementedException();
            }

            OnStatusChanged(StatusChangedMode.TemporaryChanged, statusProperty, oldValue, value);

            return ResultSuccessValue.Success((IDisposable)new ActionDisposer(d => {
                switch(statusProperty) {
                    case StatusProperty.CanCallNotifyAreaMenu:
                        CanCallNotifyAreaMenu = oldValue;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                OnStatusChanged(StatusChangedMode.TemporaryRestore, statusProperty, value, oldValue);
            }));
        }

        public bool CanCallNotifyAreaMenu { get; private set; } = true;


        #endregion

    }
}
