using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Base.Throw;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IReadOnlyObserveItem
    {
        #region property

        /// <summary>
        /// 変更通知プロパティ名。
        /// </summary>
        string NotifyPropertyName { get; }
        /// <summary>
        /// 変更通知を送るプロパティ名。
        /// </summary>
        IReadOnlyCollection<string>? RaisePropertyNames { get; }
        /// <summary>
        /// 状態を更新するコマンド。
        /// </summary>
        IReadOnlyCollection<ICommand>? RaiseCommands { get; }
        /// <summary>
        /// 変更通知により呼び出される処理。
        /// </summary>
        Action? Callback { get; }

        #endregion
    }

    public class ObserveItem: IReadOnlyObserveItem
    {
        public ObserveItem(string notifyPropertyName, IEnumerable<string>? raisePropertyNames, IEnumerable<ICommand>? raiseCommands, Action? callback)
        {
            NotifyPropertyName = notifyPropertyName;

            if(raisePropertyNames != null) {
                RaisePropertyNames = raisePropertyNames.ToList();
            }

            if(raiseCommands != null) {
                RaiseCommands = raiseCommands.ToList();
            }

            Callback = callback;
        }

        #region IReadOnlyObserveItem

        /// <inheritdoc cref="IReadOnlyObserveItem.NotifyPropertyName"/>
        public string NotifyPropertyName { get; }
        /// <inheritdoc cref="IReadOnlyObserveItem.RaisePropertyNames"/>
        public List<string>? RaisePropertyNames { get; }
        IReadOnlyCollection<string>? IReadOnlyObserveItem.RaisePropertyNames => RaisePropertyNames;
        /// <inheritdoc cref="IReadOnlyObserveItem.RaiseCommands"/>
        public List<ICommand>? RaiseCommands { get; }
        IReadOnlyCollection<ICommand>? IReadOnlyObserveItem.RaiseCommands => RaiseCommands;

        /// <inheritdoc cref="IReadOnlyObserveItem.Callback"/>
        public Action? Callback { get; }

        #endregion
    }

    internal class CachedObserveItem
    {
        public CachedObserveItem(IEnumerable<string> raisePropertyNames, IEnumerable<ICommand> raiseCommands, IEnumerable<DelegateCommandBase> raiseDelegateCommands, IEnumerable<Action> callbacks)
        {
            RaisePropertyNames = raisePropertyNames.ToList();
            RaiseCommands = raiseCommands.ToList();
            RaiseDelegateCommands = raiseDelegateCommands.ToList();
            Callbacks = callbacks.ToList();
        }

        #region property

        public IReadOnlyList<string> RaisePropertyNames { get; }
        public IReadOnlyList<ICommand> RaiseCommands { get; }
        public IReadOnlyList<DelegateCommandBase> RaiseDelegateCommands { get; }
        public IReadOnlyList<Action> Callbacks { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> を受けて何かを更新する ViewModel でよく使うあれな処理の管理役。
    /// </summary>
    /// <remarks>
    /// <para>基点となる <see cref="PropertyChangedEventArgs.PropertyName"/>の重複は実行時にマージ・キャッシュまで面倒を見る。</para>
    /// </remarks>
    public class PropertyChangedObserver: DisposerBase
    {
        HashSet<string> RaisePropertyNames { get; } = new HashSet<string>();
        public PropertyChangedObserver(IDispatcherWrapper dispatcherWrapper, ILogger logger)
        {
            DispatcherWrapper = dispatcherWrapper;
            Logger = logger;
        }
        public PropertyChangedObserver(IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected ILogger Logger { get; }

        protected IDictionary<string, List<ObserveItem>> Items { get; } = new Dictionary<string, List<ObserveItem>>();
        private IDictionary<string, CachedObserveItem> Cache { get; } = new Dictionary<string, CachedObserveItem>();

        #endregion

        #region function

        private IReadOnlyObserveItem AddObserverCore(ObserveItem hookItem)
        {
            ThrowIfDisposed();

            if(string.IsNullOrWhiteSpace(hookItem.NotifyPropertyName)) {
                throw new ArgumentException($"{nameof(hookItem.NotifyPropertyName)}");
            }
            if(hookItem.RaisePropertyNames == null && hookItem.RaiseCommands == null && hookItem.Callback == null) {
                throw new ArgumentException($"null: {nameof(hookItem.RaisePropertyNames)}, {nameof(hookItem.RaiseCommands)}, {nameof(hookItem.Callback)}");
            }

            if(hookItem.RaisePropertyNames != null) {
                if(hookItem.RaisePropertyNames.Count == 0) {
                    throw new ArgumentException($"{nameof(hookItem.RaisePropertyNames)}: 0");
                }
                if(hookItem.RaisePropertyNames.Any(i => string.IsNullOrWhiteSpace(i))) {
                    throw new ArgumentException($"{nameof(hookItem.RaisePropertyNames)}: invalid name");
                }
            }

            if(hookItem.RaiseCommands != null) {
                if(hookItem.RaiseCommands.Count == 0) {
                    throw new ArgumentException($"{nameof(hookItem.RaiseCommands)}: 0");
                }
                if(hookItem.RaiseCommands.Any(i => i == null)) {
                    throw new ArgumentException($"{nameof(hookItem.RaiseCommands)}: null element");
                }
            }

            if(Items.TryGetValue(hookItem.NotifyPropertyName, out var items)) {
                items.Add(hookItem);
            } else {
                var newItems = new List<ObserveItem>() {
                    hookItem
                };
                Items.Add(hookItem.NotifyPropertyName, newItems);
            }
            Cache.Remove(hookItem.NotifyPropertyName);

            return hookItem;
        }

        public IReadOnlyObserveItem AddObserver(ObserveItem hookItem)
        {
            ArgumentNullException.ThrowIfNull(hookItem);

            ThrowIfDisposed();

            return AddObserverCore(hookItem);
        }
        public IReadOnlyObserveItem AddObserver(string notifyAndRaisePropertyName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(notifyAndRaisePropertyName);

            ThrowIfDisposed();

            var hookItem = new ObserveItem(
                notifyAndRaisePropertyName,
                new[] { notifyAndRaisePropertyName },
                null,
                null
            );
            return AddObserverCore(hookItem);
        }
        public IReadOnlyObserveItem AddObserver(string notifyPropertyName, string raisePropertyName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(notifyPropertyName);
            ArgumentException.ThrowIfNullOrWhiteSpace(raisePropertyName);

            ThrowIfDisposed();

            var hookItem = new ObserveItem(
                notifyPropertyName,
                new[] { raisePropertyName },
                null,
                null
            );
            return AddObserverCore(hookItem);
        }
        public IReadOnlyObserveItem AddObserver(string notifyPropertyName, IEnumerable<string> raisePropertyNames)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(notifyPropertyName);
            ArgumentEmptyCollectionException.ThrowIfEmpty(raisePropertyNames, nameof(raisePropertyNames));

            ThrowIfDisposed();

            var hookItem = new ObserveItem(
                notifyPropertyName,
                raisePropertyNames,
                null,
                null
            );
            return AddObserverCore(hookItem);
        }
        public IReadOnlyObserveItem AddObserver(string notifyPropertyName, ICommand raiseCommand)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(notifyPropertyName);
            ArgumentNullException.ThrowIfNull(raiseCommand);

            ThrowIfDisposed();

            var hookItem = new ObserveItem(
                notifyPropertyName,
                null,
                new[] { raiseCommand },
                null
            );
            return AddObserverCore(hookItem);
        }
        public IReadOnlyObserveItem AddObserver(string notifyPropertyName, IEnumerable<ICommand> raiseCommands)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(notifyPropertyName);
            ArgumentEmptyCollectionException.ThrowIfEmpty(raiseCommands, nameof(raiseCommands));

            ThrowIfDisposed();

            var hookItem = new ObserveItem(
                notifyPropertyName,
                null,
                raiseCommands,
                null
            );
            return AddObserverCore(hookItem);
        }
        public IReadOnlyObserveItem AddObserver(string notifyPropertyName, Action callback)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(notifyPropertyName);
            ArgumentNullException.ThrowIfNull(callback);

            ThrowIfDisposed();

            var hookItem = new ObserveItem(
                notifyPropertyName,
                null,
                null,
                callback
            );
            return AddObserverCore(hookItem);
        }

        private CachedObserveItem MakeCache(IEnumerable<IReadOnlyObserveItem> hookItems)
        {
            ThrowIfDisposed();

            var commands = hookItems
                .Where(i => i.RaiseCommands != null)
                .SelectMany(i => i.RaiseCommands!)
                .ToList()
            ;

            var result = new CachedObserveItem(
                hookItems.Where(i => i.RaisePropertyNames != null).SelectMany(i => i.RaisePropertyNames!),
                commands.Where(i => !(i is DelegateCommandBase)),
                commands.OfType<DelegateCommandBase>(),
                hookItems.Where(i => i.Callback != null).Select(i => i.Callback!)
            );

            return result;
        }

        private bool ExecuteProperties(IReadOnlyList<string> raisePropertyNames, Action<string> propertyCallback)
        {
            ThrowIfDisposed();

            if(raisePropertyNames.Count == 0) {
                return false;
            }

            DispatcherWrapper.BeginAsync(arg => {
                if(arg.@this.IsDisposed) {
                    return;
                }

                foreach(var raisePropertyName in arg.raisePropertyNames) {
                    arg.propertyCallback(raisePropertyName);
                }
            }, (@this: this, raisePropertyNames, propertyCallback));

            return true;
        }
        private bool ExecuteCommands(IReadOnlyList<ICommand> raiseCommands, IReadOnlyList<DelegateCommandBase> raiseDelegateCommands)
        {
            ThrowIfDisposed();

            if(raiseCommands.Count == 0 && raiseDelegateCommands.Count == 0) {
                return false;
            }

            DispatcherWrapper.BeginAsync(arg => {
                if(arg.@this.IsDisposed) {
                    return;
                }

                foreach(var raiseCommand in arg.raiseDelegateCommands) {
                    raiseCommand.RaiseCanExecuteChanged();
                }
            }, (@this: this, raiseDelegateCommands));

            if(raiseCommands.Count != 0) {
                // 個別にやる方法はわからん
                DispatcherWrapper.BeginAsync(() => {
                    CommandManager.InvalidateRequerySuggested();
                });
            }

            return true;
        }
        private bool ExecuteCallback(IReadOnlyList<Action> callbacks)
        {
            ThrowIfDisposed();

            if(callbacks.Count == 0) {
                return false;
            }

            DispatcherWrapper.BeginAsync(arg => {
                if(arg.@this.IsDisposed) {
                    return;
                }

                foreach(var callback in arg.callbacks) {
                    callback();
                }
            }, (@this: this, callbacks));

            return true;
        }

        private bool ExecuteCache(CachedObserveItem hookItemCache, Action<string> propertyCallback)
        {
            ThrowIfDisposed();

            var property = ExecuteProperties(hookItemCache.RaisePropertyNames, propertyCallback);
            var command = ExecuteCommands(hookItemCache.RaiseCommands, hookItemCache.RaiseDelegateCommands);
            var callback = ExecuteCallback(hookItemCache.Callbacks);

            return property || command || callback;
        }

        public bool Execute(string? notifyPropertyName, Action<string> propertyCallback)
        {
            ThrowIfDisposed();

            if(notifyPropertyName == null) {
                return false;
            }

            if(!Items.TryGetValue(notifyPropertyName, out var hookItems)) {
                return false;
            }

            if(propertyCallback == null) {
                throw new ArgumentNullException(nameof(propertyCallback));
            }

            if(!Cache.TryGetValue(notifyPropertyName, out var hookItemCache)) {
                hookItemCache = MakeCache(hookItems);
                Cache.Add(notifyPropertyName, hookItemCache);
            }

            return ExecuteCache(hookItemCache, propertyCallback);
        }

        public bool Execute(PropertyChangedEventArgs e, Action<string> propertyCallback) => Execute(e.PropertyName, propertyCallback);

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Cache.Clear();
                Items.Clear();
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    public static class PropertyChangedObserverExtensions
    {
        #region function

        public static void AddProperties(this PropertyChangedObserver propertyChangedObserver, Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach(var property in properties) {
                propertyChangedObserver.AddObserver(property.Name);
            }
        }
        public static void AddProperties<T>(this PropertyChangedObserver propertyChangedHooker) => AddProperties(propertyChangedHooker, typeof(T));


        #endregion
    }
}
