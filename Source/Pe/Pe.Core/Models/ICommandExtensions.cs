using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public static class ICommandExtensions
    {
        public static bool ExecuteIfCanExecute(this ICommand command, object? parameter)
        {
            if(command.CanExecute(parameter)) {
                command.Execute(parameter);
                return true;
            }

            return false;
        }
    }
}
