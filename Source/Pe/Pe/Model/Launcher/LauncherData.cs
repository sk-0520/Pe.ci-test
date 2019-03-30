using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.View.Extend;

namespace ContentTypeTextNet.Pe.Main.Model.Launcher
{

    public enum LauncherItemKind
    {
        Unknown,
        File,
        Command,
        Embedded,
        Separator,
    }

    public enum LauncherItemPermission
    {
        Normal,
        Administrator,
        Another,
    }

    public enum LauncherToolbarIconDirection
    {
        LeftTop,
        Center,
        RightBottom,
    }

    public class LauncherCommandData : DataBase
    {
        #region property

        public string Command { get; set; }
        public string Option { get; set; }
        public string WorkDirectoryPath { get; set; }

        #endregion
    }

    public class LauncherIconData : DataBase
    {
        #region property

        public LauncherItemKind Kind { get; set; }

        public IconData Command { get; set; } = new IconData();
        public IconData Icon { get; set; } = new IconData();

        #endregion
    }

    public class StandardStreamData : DataBase
    {
        #region proeprty

        public bool IsEnabledStandardOutput { get; set; }
        public bool IsEnabledStandardInput { get; set; }

        #endregion
    }

    public enum LauncherGroupKind
    {
        Normal,
    }

    public enum LauncherGroupImageName
    {
        DirectoryNormal,
        DirectoryOpen,
    }

    public class LauncherGroupData : DataBase
    {
        #region property

        public Guid LauncherGroupId { get; set; }
        public string Name { get; set; }
        public LauncherGroupKind Kind { get; set; }
        public LauncherGroupImageName ImageName { get; set; }
        public Color ImageColor { get; set; }
        public long Sort { get; set; }

        #endregion
    }

    public class LauncherItemData : DataBase
    {
        #region property

        public Guid LauncherItemId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public virtual LauncherItemKind Kind { get; set; }

        public IconData Icon { get; set; } = new IconData();

        public bool IsEnabledCommandLauncher { get; set; }

        public string Note { get; set; }

        #endregion
    }

    public class LauncherFileItemData : LauncherItemData
    {
        #region property

        public LauncherCommandData Command { get; set; } = new LauncherCommandData();

        #endregion

        #region LauncherItemData

        public override LauncherItemKind Kind
        {
            get => LauncherItemKind.File;
            set
            {
                Debug.Assert(false);
                base.Kind = value;
            }
        }

        #endregion
    }

    public class LauncherToolbarsScreenData : DataBase, IScreenData
    {
        #region property

        public Guid LauncherToolbarId { get; set; }

        #endregion

        #region IScreenData

        public string ScreenName { get; set; }
        [PixelKind(Px.Device)]
        public long X { get; set; }
        [PixelKind(Px.Device)]
        public long Y { get; set; }
        [PixelKind(Px.Device)]
        public long Width { get; set; }
        [PixelKind(Px.Device)]
        public long Height { get; set; }

        #endregion
    }

    public class LauncherToolbarsDisplayData : DataBase
    {
        #region property

        public Guid LauncherToolbarId { get; set; }
        public Guid LauncherGroupId { get; set; }
        public AppDesktopToolbarPosition ToolbarPosition { get; set; }
        public LauncherToolbarIconDirection IconDirection { get; set; }
        public IconScale IconScale { get; set; }
        public Guid FontId { get; set; }
        public TimeSpan AutoHideTimeout { get; set; }
        public long TextWidth { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTopmost { get; set; }
        public bool IsAutoHide { get; set; }
        public bool IsIconOnly { get; set; }

        #endregion
    }

}
