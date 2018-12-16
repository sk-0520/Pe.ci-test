using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Launcher
{

    public enum LauncherItemKind
    {
        File,
        Command,
        Application,
    }

    public enum LauncherItemPermission
    {
        Normal,
        Administrator,
        Another,
    }

    public class LauncherCommandData : DataBase
    {
        #region property

        public string Command { get; set; }
        public string CommandOption { get; set; }
        public DirectoryInfo WorkDirectory { get; set; }

        #endregion
    }

    public class IconData : DataBase
    {
        #region property

        public FileInfo File { get; }
        public int Index { get; }

        #endregion
    }

    public class StandardStreamData : DataBase
    {
        #region proeprty

        public bool IsEnabledStandardOutput { get; set; }
        public bool IsEnabledStandardInput { get; set; }

        #endregion
    }

}
