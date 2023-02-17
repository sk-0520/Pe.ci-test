using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Standard.Base;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// <see cref="ICommand"/>管理。
    /// </summary>
    public class CommandStore: DisposerBase
    {
        #region property

        protected IDictionary<string, ICommand> CommandCache { get; } = new Dictionary<string, ICommand>();
        public IEnumerable<ICommand> Commands => CommandCache.Values;

        #endregion

        #region function

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public TCommand GetOrCreate<TCommand>(Func<TCommand> creator, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            where TCommand : ICommand
        {
            var sb = new StringBuilder(128);
            sb.Append(GetType().FullName);
            sb.Append(':');
            sb.Append(callerMemberName);
            sb.Append(':');
            sb.Append(callerFilePath.GetHashCode());
            sb.Append(':');
            sb.Append(callerLineNumber);

            var key = sb.ToString();

            if(CommandCache.TryGetValue(key, out var cahceCommand)) {
                return (TCommand)cahceCommand;
            }

            var command = creator();
            CommandCache.Add(key, command);

            return command;
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                CommandCache.Clear();
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// View要素に対する<see cref="ICommand"/>管理。
    /// </summary>
    public class ElementCommandStore: CommandStore
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="view">対象View。</param>
        public ElementCommandStore(FrameworkElement view)
        {
            View = view;

            if(View.IsLoaded) {
                View.Unloaded += View_Unloaded;
            } else {
                View.Loaded += View_Loaded;
            }
        }

        #region property

        private FrameworkElement? View { get; set; }

        #endregion

        #region CommandStore

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(View != null) {
                    View.Loaded -= View_Loaded;
                    View.Unloaded -= View_Unloaded;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.Assert(View != null);

            View.Loaded -= View_Loaded;
            View.Unloaded += View_Unloaded;
        }

        private void View_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.Assert(View != null);

            CommandCache.Clear();

            View.Unloaded -= View_Unloaded;
            View = null;
        }
    }
}
