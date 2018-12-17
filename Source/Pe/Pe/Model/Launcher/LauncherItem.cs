using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
        public string Option { get; set; }
        public string WorkDirectoryPath { get; set; }

        #endregion
    }

    public class IconData : DataBase
    {
        #region property

        public string Path { get; set; }
        public int Index { get; set; }

        #endregion
    }

    public class StandardStreamData : DataBase
    {
        #region proeprty

        public bool IsEnabledStandardOutput { get; set; }
        public bool IsEnabledStandardInput { get; set; }

        #endregion
    }

    public enum LauncherGroupImageName
    {
        Directory,
    }

    public class LauncherGroupData: DataBase
    {
        #region property

        public Guid LauncherGroupId { get; set; }
        public string Name { get; set; }
        public LauncherGroupImageName ImageName { get; set; }
        public Color ImageColor { get; set; }

        #endregion
    }

    public class LauncherItemSimpleNewData : DataBase
    {
        #region property

        public Guid LauncherItemId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public LauncherItemKind Kind { get; set; }

        public LauncherCommandData Command { get; set; } = new LauncherCommandData();

        public IconData Icon { get; set; } = new IconData();

        public bool IsEnabledCommandLauncher { get; set; }
        public bool IsEnabledCustomEnvVar { get; set; }

        public StandardStreamData StandardStream { get; set; } = new StandardStreamData();

        public LauncherItemPermission Permission { get; set; }

        public string Note { get; set; }

        #endregion
    }

}
