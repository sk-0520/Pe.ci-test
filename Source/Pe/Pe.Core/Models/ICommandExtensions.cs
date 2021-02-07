using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public static class ICommandExtensions
    {
        public static void ExecuteIfCanExecute(this ICommand command, object? parameter)
        {
            //Debug.Assert(command != null);

            if(command.CanExecute(parameter)) {
                command.Execute(parameter);
            }
        }
    }
}
