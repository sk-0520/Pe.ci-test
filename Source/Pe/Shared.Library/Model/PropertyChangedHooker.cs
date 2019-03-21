using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
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
        IReadOnlyList<string> RaisePropertyNames { get; }
        /// <summary>
        /// 状態を更新するコマンド。
        /// </summary>
        IReadOnlyList<ICommand> RaiseCommands { get; }
        /// <summary>
        /// 変更通知により呼び出される処理。
        /// </summary>
        Action Callback { get; }

        #endregion
    }

    public class HookItem : IReadOnlyHookItem
    {
        public HookItem(string notifyPropertyName, IEnumerable<string> raisePropertyNames, IEnumerable<ICommand> raiseCommands, Action callback)
        {
        }

        #region IReadOnlyHookItem

        /// <summary>
        /// 変更通知プロパティ名。
        /// </summary>
        public string NotifyPropertyName { get; }
        /// <summary>
        /// 変更通知を送るプロパティ名。
        /// </summary>
        public List<string> RaisePropertyNames { get; }
        IReadOnlyList<string> IReadOnlyHookItem.RaisePropertyNames => RaisePropertyNames;
        /// <summary>
        /// 状態を更新するコマンド。
        /// </summary>
        public List<ICommand> RaiseCommands { get; }
        IReadOnlyList<ICommand> IReadOnlyHookItem.RaiseCommands => RaiseCommands;

        /// <summary>
        /// 変更通知により呼び出される処理。
        /// </summary>
        public Action Callback { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> を受けて何かを更新する ViewModel でよく使うあれな処理の管理役。
    /// </summary>
    public class PropertyChangedHooker : DisposerBase
    {
        public PropertyChangedHooker(ILogger logger)
        {
            Logger = logger;
        }
        public PropertyChangedHooker(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        protected IDictionary<string, List<HookItem>> Hookers { get; } = new Dictionary<string, List<HookItem>>();

        #endregion

        #region function

        IReadOnlyHookItem AddHookCore(HookItem hookItem)
        {
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

            return hookItem;
        }

        public IReadOnlyHookItem AddHook(HookItem HookItem)
        {
            if(HookItem == null) {
                throw new ArgumentNullException(nameof(HookItem));
            }

            return AddHookCore(hookItem);
        }
        public IReadOnlyHookItem AddHook(string notifyAndRaisePropertyName)
        {
            if(string.IsNullOrWhiteSpace(notifyAndRaisePropertyName)) {
                throw new ArgumentException(nameof(notifyAndRaisePropertyName));
            }

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
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(string.IsNullOrWhiteSpace(raisePropertyName)) {
                throw new ArgumentException(nameof(raisePropertyName));
            }

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
                throw new ArgumentException(nameof(raisePropertyNames));
            }

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
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(raiseCommands == null || raiseCommands.Length == 0) {
                throw new ArgumentException(nameof(raiseCommands));
            }

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
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(callback == null) {
                throw new ArgumentException(nameof(callback));
            }

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
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(raisePropertyNames == null) {
                throw new ArgumentException(nameof(raisePropertyNames));
            }
            if(raiseCommands == null) {
                throw new ArgumentException(nameof(raiseCommands));
            }

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
                throw new ArgumentException(nameof(notifyPropertyName));
            }
            if(raisePropertyNames == null) {
                throw new ArgumentException(nameof(raisePropertyNames));
            }
            if(raiseCommands == null) {
                throw new ArgumentException(nameof(raiseCommands));
            }
            if(callback == null) {
                throw new ArgumentException(nameof(callback));
            }

            var hookItem = new HookItem(
                notifyPropertyName,
                raisePropertyNames,
                raiseCommands,
                callback
            );
            return AddHookCore(hookItem);
        }

        public bool Do(string noifyPropertyName, Action<string> raiser)
        {
            if(raiser == null) {
                throw new ArgumentNullException(nameof(raiser));
            }

            if(Hookers.TryGetValue(noifyPropertyName, out var items)) {
                // つなげる処理を書きたいのよ
            }

            return false;
        }

        public bool Do(PropertyChangedEventArgs e, Action<string> raiser) => Do(e.PropertyName, raiser);

        #endregion
    }
}
