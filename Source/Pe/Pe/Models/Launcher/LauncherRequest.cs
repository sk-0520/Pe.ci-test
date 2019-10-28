using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class LauncherFileSelectRequestParameter : FileDialogRequestParameter
    {
        #region peoperty

        /// <summary>
        /// ファイルを選択するか。
        /// </summary>
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

        public string FileName { get; set; } = string.Empty;
        public int IconIndex { get; set; }

        #endregion
    }

    public class LauncherIconSelectRequestResponse : CancelResponse
    {
        #region peoperty

        public string FileName { get; set; } = string.Empty;
        public int IconIndex { get; set; }


        #endregion
    }

}
