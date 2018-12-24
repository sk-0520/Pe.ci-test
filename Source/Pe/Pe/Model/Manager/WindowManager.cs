using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    public enum WindowKind
    {
        Accept,
        Startup,
        ImportPrograms,
        LauncherToolbar,
        LauncherCommand,
        LauncherCustomInput,
        LauncherEdit
    }

    public class WindowItem
    {
        public WindowItem(Window window)
        {
            Window = window;
        }

        #region property

        Window Window { get; }

        #endregion
    }

    public class WindowManager : ManagerBase
    {
        public WindowManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region property
        #endregion

        #region function

        #endregion
    }
}
