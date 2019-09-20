using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public static class ICommandExtensions
    {
        public static void ExecuteIfCanExecute(this ICommand command, object parameter)
        {
            Debug.Assert(command != null);

            if(command.CanExecute(parameter)) {
                command.Execute(parameter);
            }
        }
    }
}
