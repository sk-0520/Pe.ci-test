using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    class LauncherRequest
    {
    }
    public class LauncherFileSelectRequestParameter : FileDialogRequestParameter
    {
        #region peoperty

        public bool IsFile { get; set; }

        #endregion
    }

    public class LauncherFileSelectRequestResponse : FileDialogRequestResponse
    {
        #region peoperty
        #endregion
    }

    public class LauncherIconSelectRequestParameter : RequestParameter
    {
        #region peoperty

        public bool IsFile { get; set; }

        #endregion
    }

    public class LauncherIconSelectRequestResponse : FileDialogRequestResponse
    {
        #region peoperty
        #endregion
    }

}
