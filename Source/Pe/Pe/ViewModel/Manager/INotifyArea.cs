using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Manager
{
    public interface INotifyArea
    {
        #region property

        string MenuHeader { get; }
        DependencyObject MenuIcon { get; }
        bool MenuIsEnabled { get; }

        #endregion
    }
}
