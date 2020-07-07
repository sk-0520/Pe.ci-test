using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Views.Extend;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// ランチャーアイテム種別。
    /// </summary>
    public enum LauncherItemKind
    {
        /// <summary>
        /// 💩。
        /// </summary>
        [EnumResource]
        Unknown,
        /// <summary>
        /// ファイルアイテム。
        /// <para>可能な限りPATHを考慮するので旧来のコマンドに近い挙動も可能。</para>
        /// </summary>
        [EnumResource]
        File,
        /// <summary>
        /// ストアアプリ。
        /// <para>プロトコルとかエイリアスであれこれ。</para>
        /// <para><see cref="File"/>と違って小難しい処理は無理。</para>
        /// </summary>
        [EnumResource]
        StoreApp,
        /// <summary>
        /// プラグインアイテム。
        /// <para>プラグインのみぞ知る機能。</para>
        /// </summary>
        [EnumResource]
        Addon,
        /// <summary>
        /// セパレータ。
        /// <para>いる、これ？</para>
        /// </summary>
        [EnumResource]
        Separator,
    }

    public enum LauncherToolbarIconDirection
    {
        [EnumResource]
        LeftTop,
        [EnumResource]
        Center,
        [EnumResource]
        RightBottom,
    }

    /// <summary>
    /// ランチャーツールバーへのD&amp;D処理。
    /// </summary>
    public enum LauncherToolbarContentDropMode
    {
        /// <summary>
        /// 指定して実行。
        /// </summary>
        [EnumResource]
        ExtendsExecute,
        /// <summary>
        /// D&amp;Dデータをパラメータとして直接実行。
        /// </summary>
        [EnumResource]
        DirectExecute,
    }

    public interface ILauncherItemId
    {
        #region property

        Guid LauncherItemId { get; }

        #endregion
    }

    public interface ILauncherExecutePathParameter
    {
        #region property

        string Path { get; set; }
        string Option { get; set; }
        string WorkDirectoryPath { get; set; }

        #endregion
    }

    public class LauncherExecutePathParameter: ILauncherExecutePathParameter
    {
        public LauncherExecutePathParameter(string path, string option, string workDirectoryPath)
        {
            Path = path;
            Option = option;
            WorkDirectoryPath = workDirectoryPath;
        }

        #region ILauncherExecutePathParameter

        public string Path { get; set; }
        public string Option { get; set; }
        public string WorkDirectoryPath { get; set; }

        #endregion
    }

    public interface ILauncherExecuteCustomParameter
    {
        #region property

        string Caption { get; set; }

        bool IsEnabledCustomEnvironmentVariable { get; set; }
        bool IsEnabledStandardInputOutput { get; set; }
        Encoding StandardInputOutputEncoding { get; set; }
        bool RunAdministrator { get; set; }
        #endregion
    }

    public class LauncherExecutePathData: DataBase, ILauncherExecutePathParameter
    {
        #region ILauncherExecutePathParameter

        public string Path { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;
        public string WorkDirectoryPath { get; set; } = string.Empty;

        #endregion
    }

    public class LauncherFileData: LauncherExecutePathData, ILauncherExecuteCustomParameter
    {
        #region ILauncherExecuteCustomParameter

        public string Caption { get; set; } = string.Empty;
        public bool IsEnabledCustomEnvironmentVariable { get; set; }
        public bool IsEnabledStandardInputOutput { get; set; }
        public Encoding StandardInputOutputEncoding { get; set; } = EncodingConverter.DefaultStandardInputOutputEncoding;
        public bool RunAdministrator { get; set; }
        #endregion
    }

    public class LauncherStoreAppData
    {
        #region property

        public string ProtocolAlias { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;
        #endregion
    }

    public class LauncherEnvironmentVariableData: DataBase
    {
        #region property

        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public bool IsRemove => string.IsNullOrEmpty(Value);

        #endregion
    }

    public class LauncherIconData: DataBase
    {
        #region property

        public LauncherItemKind Kind { get; set; }

        public IconData Path { get; set; } = new IconData();
        public IconData Icon { get; set; } = new IconData();

        #endregion
    }

    public class StandardStreamData: DataBase
    {
        #region proeprty

        public bool IsEnabledStandardOutput { get; set; }
        public bool IsEnabledStandardInput { get; set; }

        #endregion
    }

    public enum LauncherGroupKind
    {
        [EnumResource]
        Normal,
    }

    public enum LauncherGroupImageName
    {
        DirectoryNormal,
        DirectoryOpen,
        File,
        Gear,
        Config,
        Builder,
        Bookmark,
        Book,
        Light,
        Shortcut,
        Storage,
        Cloud,
        User,
    }

    /// <summary>
    /// グループメニューの表示位置。
    /// </summary>
    public enum LauncherGroupPosition
    {
        [EnumResource]
        Top,
        [EnumResource]
        Bottom,
    }


    public class LauncherGroupData: DataBase, ILauncherGroupId
    {
        #region property

        public string Name { get; set; } = string.Empty;
        public LauncherGroupKind Kind { get; set; }
        public LauncherGroupImageName ImageName { get; set; }
        public Color ImageColor { get; set; }
        public long Sequence { get; set; }

        #endregion

        #region ILauncherGroupId
        public Guid LauncherGroupId { get; set; }
        #endregion
    }
    public interface ILauncherGroupId
    {
        #region property

        Guid LauncherGroupId { get; }

        #endregion
    }

    public class LauncherItemData: DataBase
    {
        #region property

        public Guid LauncherItemId { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public virtual LauncherItemKind Kind { get; set; }

        public IconData Icon { get; set; } = new IconData();

        public bool IsEnabledCommandLauncher { get; set; }

        public string Comment { get; set; } = string.Empty;

        #endregion
    }

    [Obsolete]
    internal class LauncherItemOldImportData: LauncherItemData
    {
        #region property

        public long ExecuteCount { get; set; }
        public DateTime LastExecuteTimestamp { get; set; }

        #endregion
    }

    public enum LauncherHistoryKind
    {
        Option,
        WorkDirectory,
    }

    public class LauncherHistoryData
    {
        #region property

        public LauncherHistoryKind Kind { get; set; }
        public string Value { get; set; } = string.Empty;
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime LastExecuteTimestamp { get; set; }
        #endregion

    }

    #region LauncherItemDetailData

    public abstract class LauncherDetailDataBase
    { }

    public class LauncherFileDetailData: LauncherDetailDataBase
    {
        #region property

        public LauncherExecutePathData PathData { get; set; } = new LauncherExecutePathData();
        public string FullPath { get; set; } = string.Empty;

        #endregion
    }

    #endregion

    public interface ILauncherToolbarId
    {
        #region property

        Guid LauncherToolbarId { get; }

        #endregion
    }

    public class LauncherToolbarsScreenData: DataBase, ILauncherToolbarId, IScreenData
    {
        #region ILauncherToolbarId

        public Guid LauncherToolbarId { get; set; }

        #endregion

        #region IScreenData

        public string ScreenName { get; set; } = string.Empty;
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

    public class LauncherToolbarsDisplayData: DataBase, ILauncherToolbarId
    {
        #region property

        public Guid LauncherGroupId { get; set; }
        public AppDesktopToolbarPosition ToolbarPosition { get; set; }
        public LauncherToolbarIconDirection IconDirection { get; set; }
        public IconBox IconBox { get; set; }
        public Guid FontId { get; set; }
        public TimeSpan AutoHideTime { get; set; }
        public int TextWidth { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTopmost { get; set; }
        public bool IsAutoHide { get; set; }
        public bool IsIconOnly { get; set; }

        #endregion

        #region ILauncherToolbarId

        public Guid LauncherToolbarId { get; set; }

        #endregion

    }

    [Obsolete]
    internal class LauncherToolbarsOldData: LauncherToolbarsDisplayData
    {
        public IScreen? Screen { get; set; }
    }

    internal class LauncherFileItemData
    {
        public LauncherFileItemData(LauncherItemData item, LauncherFileData file)
        {
            Item = item;
            File = file;
        }

        #region property
        public LauncherItemData Item { get; }
        public LauncherFileData File { get; }
        #endregion
    }

    /// <summary>
    /// 再実施待機方法。
    /// </summary>
    public enum RedoMode
    {
        /// <summary>
        /// 再実施しない。
        /// </summary>
        [EnumResource]
        None,
        /// <summary>
        /// 一定時間繰り返す。
        /// </summary>
        [EnumResource]
        Timeout,
        /// <summary>
        /// 指定回数繰り返す。
        /// </summary>
        [EnumResource]
        Count,
        /// <summary>
        /// 一定時間内で指定回数繰り返す。
        /// </summary>
        [EnumResource]
        TimeoutOrCount,
    }

    public interface IReadOnlyLauncherRedoData
    {
        #region property

        RedoMode RedoMode { get; }
        TimeSpan WaitTime { get; }
        int RetryCount { get; }
        IReadOnlyCollection<int> SuccessExitCodes { get; }

        #endregion
    }

    public class LauncherRedoData: DataBase, IReadOnlyLauncherRedoData
    {
        #region IReadOnlyLauncherRedoData

        public RedoMode RedoMode { get; set; }
        public TimeSpan WaitTime { get; set; }
        public int RetryCount { get; set; }
        public List<int> SuccessExitCodes { get; set; } = new List<int>();
        IReadOnlyCollection<int> IReadOnlyLauncherRedoData.SuccessExitCodes => SuccessExitCodes;

        #endregion

        #region function

        public static LauncherRedoData GetDisable() => new LauncherRedoData() {
            RedoMode = RedoMode.None,
            RetryCount = 1,
            WaitTime = TimeSpan.FromSeconds(1),
        };

        #endregion
    }

    public class LauncherIconStatus
    {
        public LauncherIconStatus(IconBox iconBox, Point iconScale, [DateTimeKind(DateTimeKind.Utc)] DateTime lastUpdatedTimestamp)
        {
            IconBox = iconBox;
            IconScale = iconScale;
            LastUpdatedTimestamp = lastUpdatedTimestamp;
        }

        #region proeprty

        public IconBox IconBox { get; }
        public Point IconScale { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime LastUpdatedTimestamp { get; }

        #endregion
    }


}
