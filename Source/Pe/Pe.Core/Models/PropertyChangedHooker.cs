using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IReadOnlyHookItem
    {
        #region property

        /// <summary>
        /// 変更通知プロパティ名。
        /// </summary>
        string NotifyPropertyName { get; }
        /// <summary>
        /// 変更通知を送るプロパティ名。
        /// </summary>
        IReadOnlyList<string>? RaisePropertyNames { get; }
        /// <summary>
        /// 状態を更新するコマンド。
        /// </summary>
        IReadOnlyList<ICommand>? RaiseCommands { get; }
        /// <summary>
        /// 変更通知により呼び出される処理。
        /// </summary>
        Action? Callback { get; }

        #endregion
    }

    public class HookItem: IReadOnlyHookItem
    {
        public HookItem(string notifyPropertyName, IEnumerable<string>? raisePropertyNames, IEnumerable<ICommand>? raiseCommands, Action? callback)
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

        #region IReadOnlyHookItem

        /// <inheritdoc cref="IReadOnlyHookItem.NotifyPropertyName"/>
        public string NotifyPropertyName { get; }
        /// <inheritdoc cref="IReadOnlyHookItem.RaisePropertyNames"/>
        public List<string>? RaisePropertyNames { get; }
        IReadOnlyList<string>? IReadOnlyHookItem.RaisePropertyNames => RaisePropertyNames;
        /// <inheritdoc cref="IReadOnlyHookItem.RaiseCommands"/>
        public List<ICommand>? RaiseCommands { get; }
        IReadOnlyList<ICommand>? IReadOnlyHookItem.RaiseCommands => RaiseCommands;

        /// <inheritdoc cref="IReadOnlyHookItem.Callback"/>
        public Action? Callback { get; }

        #endregion
    }

    internal class CachedHookItem
    {
        public CachedHookItem(IEnumerable<string> raisePropertyNames, IEnumerable<ICommand> raiseCommands, IEnumerable<DelegateCommandBase> raiseDelegateCommands, IEnumerable<Action> callbacks)
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
    public class PropertyChangedHooker: DisposerBase
    {
        public PropertyChangedHooker(IDispatcherWrapper dispatcherWrapper, ILogger logger)
        {
            DispatcherWrapper = dispatcherWrapper;
            Logger = logger;
        }
        public PropertyChangedHooker(IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected ILogger Logger { get; }

        protected IDictionary<string, List<HookItem>> Hookers { get; } = new Dictionary<string, List<HookItem>>();
        private IDictionary<string, CachedHookItem> Cache { get; } = new Dictionary<string, CachedHookItem>();

        #endregion

        #region function

        private IReadOnlyHookItem AddHookCore(HookItem hookItem)
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

            if(Hookers.TryGetValue(hookItem.NotifyPropertyName, out var items)) {
                items.Add(hookItem);
            } else {
                var newItems = new List<HookItem>() {
                    hookItem
                };
                Hookers.Add(hookItem.NotifyPropertyName, newItems);
            }
            Cache.Remove(hookItem.NotifyPropertyName);

            return hookItem;
        }

        public IReadOnlyHookItem AddHook(HookItem hookItem)
        {
            if(hookItem == null) {
                throw new ArgumentNullException(nameof(hookItem));
            }
            ThrowIfDisposed();

            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyAndRaisePropertyName)
        {
            if(string.IsNullOrWhiteSpace(notifyAndRaisePropertyName)) {
                throw new ArgumentException(null, nameof(notifyAndRaisePropertyName));
            }
            ThrowIfDisposed();

            var hookItem = new HookItem(
                notifyAndRaisePropertyName,
                new[] { notifyAndRaisePropertyName },
                null,
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, string raisePropertyName)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(null, nameof(notifyPropertyName));
            }
            if(string.IsNullOrWhiteSpace(raisePropertyName)) {
                throw new ArgumentException(null, nameof(raisePropertyName));
            }
            ThrowIfDisposed();

            var hookItem = new HookItem(
                notifyPropertyName,
                new[] { raisePropertyName },
                null,
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, params string[] raisePropertyNames)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(raisePropertyNames == null || raisePropertyNames.Length == 0) {
                throw new ArgumentException(null, nameof(raisePropertyNames));
            }
            ThrowIfDisposed();

            var hookItem = new HookItem(
                notifyPropertyName,
                raisePropertyNames,
                null,
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, ICommand raiseCommand)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(raiseCommand == null) {
                throw new ArgumentNullException(nameof(raiseCommand));
            }
            ThrowIfDisposed();

            var hookItem = new HookItem(
                notifyPropertyName,
                null,
                new[] { raiseCommand },
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, params ICommand[] raiseCommands)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(null, nameof(notifyPropertyName));
            }
            if(raiseCommands == null || raiseCommands.Length == 0) {
                throw new ArgumentException(null, nameof(raiseCommands));
            }
            ThrowIfDisposed();

            var hookItem = new HookItem(
                notifyPropertyName,
                null,
                raiseCommands,
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, Action callback)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(null, nameof(notifyPropertyName));
            }
            if(callback == null) {
                throw new ArgumentException(null, nameof(callback));
            }
            ThrowIfDisposed();

            var hookItem = new HookItem(
                notifyPropertyName,
                null,
                null,
                callback
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, IEnumerable<string> raisePropertyNames, IEnumerable<ICommand> raiseCommands)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(null, nameof(notifyPropertyName));
            }
            if(raisePropertyNames == null) {
                throw new ArgumentException(null, nameof(raisePropertyNames));
            }
            if(raiseCommands == null) {
                throw new ArgumentException(null, nameof(raiseCommands));
            }
            ThrowIfDisposed();

            var hookItem = new HookItem(
                notifyPropertyName,
                raisePropertyNames,
                raiseCommands,
                null
            );
            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyPropertyName, IEnumerable<string> raisePropertyNames, IEnumerable<ICommand> raiseCommands, Action callback)
        {
            if(string.IsNullOrWhiteSpace(notifyPropertyName)) {
                throw new ArgumentException(null, nameof(notifyPropertyName));
            }
            if(raisePropertyNames == null) {
                throw new ArgumentException(null, nameof(raisePropertyNames));
            }
            if(raiseCommands == null) {
                throw new ArgumentException(null, nameof(raiseCommands));
            }
            if(callback == null) {
                throw new ArgumentException(null, nameof(callback));
            }
            ThrowIfDisposed();

            var hookItem = new HookItem(
                notifyPropertyName,
                raisePropertyNames,
                raiseCommands,
                callback
            );
            return AddHookCore(hookItem);
        }

        private CachedHookItem MakeCache(IEnumerable<IReadOnlyHookItem> hookItems)
        {
            ThrowIfDisposed();

            var commands = hookItems
                .Where(i => i.RaiseCommands != null)
                .SelectMany(i => i.RaiseCommands!)
                .ToList()
            ;

            var result = new CachedHookItem(
                hookItems.Where(i => i.RaisePropertyNames != null).SelectMany(i => i.RaisePropertyNames!),
                commands.Where(i => !(i is DelegateCommandBase)),
                commands.OfType<DelegateCommandBase>(),
                hookItems.Where(i => i.Callback != null).Select(i => i.Callback!)
            );

            return result;
        }

        private bool ExecuteProperties(IReadOnlyList<string> raisePropertyNames, Action<string> raiser)
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
                    arg.raiser(raisePropertyName);
                }
            }, (@this: this, raisePropertyNames, raiser));

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

        private bool ExecuteCache(CachedHookItem hookItemCache, Action<string> raiser)
        {
            ThrowIfDisposed();

            var property = ExecuteProperties(hookItemCache.RaisePropertyNames, raiser);
            var command = ExecuteCommands(hookItemCache.RaiseCommands, hookItemCache.RaiseDelegateCommands);
            var callback = ExecuteCallback(hookItemCache.Callbacks);

            return property || command || callback;
        }

        public bool Execute(string? notifyPropertyName, Action<string> raiser)
        {
            ThrowIfDisposed();

            if(notifyPropertyName == null) {
                return false;
            }

            if(!Hookers.TryGetValue(notifyPropertyName, out var hookItems)) {
                return false;
            }

            if(raiser == null) {
                throw new ArgumentNullException(nameof(raiser));
            }

            if(!Cache.TryGetValue(notifyPropertyName, out var hookItemCache)) {
                hookItemCache = MakeCache(hookItems);
                Cache.Add(notifyPropertyName, hookItemCache);
            }

            return ExecuteCache(hookItemCache, raiser);
        }

        public bool Execute(PropertyChangedEventArgs e, Action<string> raiser) => Execute(e.PropertyName, raiser);

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Cache.Clear();
                Hookers.Clear();
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    public static class PropertyChangedHookerExtensions
    {
        #region function

        public static void AddProperties(this PropertyChangedHooker propertyChangedHooker, Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach(var property in properties) {
                propertyChangedHooker.AddHook(property.Name);
            }
        }
        public static void AddProperties<T>(this PropertyChangedHooker propertyChangedHooker) => AddProperties(propertyChangedHooker, typeof(T));


        #endregion
    }
}
