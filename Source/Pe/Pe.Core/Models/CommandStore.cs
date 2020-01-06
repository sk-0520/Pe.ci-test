using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class CommandStore
    {
        #region property

        protected IDictionary<string, ICommand> CommandCache { get; } = new Dictionary<string, ICommand>();
        public IEnumerable<ICommand> Commands => CommandCache.Values;

        #endregion

        #region function

        public TCommand GetOrCreate<TCommand>(Func<TCommand> creator, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            where TCommand : ICommand
        {
            var sb = new StringBuilder();
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
    }

    public class ElementCommandStore : CommandStore
    {
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

        FrameworkElement? View { get; set; }

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
