using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Main.ViewModel
{
    public interface IDialogCommand
    {
        #region command

        ICommand AffirmativeCommand { get; }
        ICommand NegativeCommand { get; }

        #endregion
    }
}
