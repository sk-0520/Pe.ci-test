using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Bridge.ViewModels
{
    public interface ISkeletonImplements
    {
        #region function

        ISkeletonImplements Clone();

        ICommand CreateCommand(Action execute);
        ICommand CreateCommand<TParameter>(Action<TParameter> execute);
        ICommand CreateCommand(Action execute, Func<bool> canExecute);
        ICommand CreateCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute);

        #endregion
    }

    /// <summary>
    /// プラグイン側でのViewModel作成を簡略化。
    /// <para><see cref="ISkeletonImplements"/>が本体。</para>
    /// </summary>
    public abstract class ViewModelSkeleton: INotifyPropertyChanged, IDisposable
    {
        protected ViewModelSkeleton(ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
            Implements = skeletonImplements;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        /// <summary>
        /// こいつ経由でViewModel処理を行う。
        /// </summary>
        protected ISkeletonImplements Implements { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }
        #endregion

        #region function

        protected void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if(propertyChanged != null) {
                var e = new PropertyChangedEventArgs(propertyName);
                propertyChanged(this, e);
            }
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if(Equals(storage, value)) {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// オブジェクトのプロパティに値設定。
        /// </summary>
        /// <typeparam name="TValue">設定値のデータ。</typeparam>
        /// <param name="obj">対象オブジェクト。</param>
        /// <param name="value">設定値。</param>
        /// <param name="targetMemberName">設定対象となる<paramref name="obj"/>のメンバ名。未設定で呼び出しメンバ名。</param>
        /// <param name="notifyPropertyName">値設定後に通知するプロパティ名。未設定で呼び出しメンバ名。</param>
        /// <returns>設定されたか。同一値の場合は設定しない。</returns>
        protected virtual bool SetPropertyValue<TValue>(object obj, [AllowNull] TValue value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            var type = obj.GetType();
            var propertyInfo = type.GetProperty(targetMemberName)!;

            var nowValue = (TValue?)propertyInfo.GetValue(obj);

            if(!Equals(nowValue, value)) {

                propertyInfo.SetValue(obj, value);

                OnPropertyChanged(notifyPropertyName);

                return true;
            }

            return false;
        }

        public ICommand CreateCommand(Action execute) => Implements.CreateCommand(execute);
        public ICommand CreateCommand<TParameter>(Action<TParameter> execute) => Implements.CreateCommand(execute);
        public ICommand CreateCommand(Action execute, Func<bool> canExecute) => Implements.CreateCommand(execute, canExecute);
        public ICommand CreateCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute) => Implements.CreateCommand(execute, canExecute);

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region IDisposable

        public bool IsDisposed { get; private set; }

        protected void ThrowIfDisposed([CallerMemberName] string _callerMemberName = "")
        {
            if(IsDisposed) {
                throw new ObjectDisposedException(_callerMemberName);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                IsDisposed = true;
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        ~ViewModelSkeleton()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: false);
        }


        #endregion
    }
}
