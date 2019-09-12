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

namespace ContentTypeTextNet.Pe.Main.Model.Data
{

    public enum LauncherItemKind
    {
        Unknown,
        File,
        Command,
        Script,
        Directory,
        Embedded,
        Addin,
        Separator,
    }

    public enum LauncherToolbarIconDirection
    {
        LeftTop,
        Center,
        RightBottom,
    }

    public enum LauncherPathKind
    {
        Unknown,
        File,
        Directory,
    }

    public enum LauncherEnvironmentVariableKind
    {
        Update,
        Remove,
    }

    public interface ILauncherExecutePathParameter
    {
        #region property

        string Path { get; set; }
        string Option { get; set; }
        string WorkDirectoryPath { get; set; }

        #endregion
    }

    public interface ILauncherExecuteCustomParameter
    {
        #region property

        bool IsEnabledCustomEnvironmentVariable { get; set; }
        bool IsEnabledStandardInputOutput { get; set; }
        bool RunAdministrator { get; set; }
        #endregion
    }

    public class LauncherExecutePathData : DataBase, ILauncherExecutePathParameter
    {
        #region ILauncherExecutePathParameter

        public string Path { get; set; }
        public string Option { get; set; }
        public string WorkDirectoryPath { get; set; }

        #endregion
    }

    public class LauncherFileData : DataBase, ILauncherExecutePathParameter, ILauncherExecuteCustomParameter
    {
        #region ILauncherExecutePathParameter

        public string Path { get; set; }
        public string Option { get; set; }
        public string WorkDirectoryPath { get; set; }

        #endregion

        #region ILauncherExecuteCustomParameter

        public bool IsEnabledCustomEnvironmentVariable { get; set; }
        public bool IsEnabledStandardInputOutput { get; set; }
        public bool RunAdministrator { get; set; }

        #endregion
    }

    public class LauncherEnvironmentVariableItem
    {
        #region property

        public LauncherEnvironmentVariableKind Kind { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        #endregion
    }

    public class LauncherIconData : DataBase
    {
        #region property

        public LauncherItemKind Kind { get; set; }

        public IconData Path { get; set; } = new IconData();
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

        public string Comment { get; set; }

        #endregion
    }

    public sealed class LauncherFileItemData : LauncherItemData
    {
        #region property

        public LauncherExecutePathData PathExecute { get; set; } = new LauncherExecutePathData();

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

    #region LauncherItemDetailData

    public abstract class LauncherDetailDataBase
    { }

    public class LauncherFileDetailData : LauncherDetailDataBase
    {
        #region property

        public LauncherExecutePathData PathData { get; set; }

        public LauncherPathKind Kind { get; set; }

        public FileSystemInfo FileSystemInfo { get; set; }

        #endregion
    }

    #endregion

    public class LauncherToolbarsScreenData : DataBase, IScreenData
    {
        #region property

        public Guid LauncherToolbarId { get; set; }

# endregionIReadOnlyData

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
