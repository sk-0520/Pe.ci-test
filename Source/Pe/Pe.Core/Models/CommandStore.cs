using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class CommandStore
    {
        #region property

        IDictionary<string, ICommand> CommandCache { get; } = new Dictionary<string, ICommand>();
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
}
