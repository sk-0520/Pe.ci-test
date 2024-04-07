using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Logic;

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

    /// <summary>
    /// ランチャーツールバーのアイコン(ボタン)位置。
    /// </summary>
    public enum LauncherToolbarIconDirection
    {
        /// <summary>
        /// 水平: 左から, 垂直: 上から。
        /// </summary>
        [EnumResource]
        LeftTop,
        /// <summary>
        /// 中央から。
        /// </summary>
        [EnumResource]
        Center,
        /// <summary>
        /// 水平: 右から, 垂直: 下から。
        /// </summary>
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
        ShowMode ShowMode { get; set; }
        bool IsEnabledCustomEnvironmentVariable { get; set; }
        bool IsEnabledStandardInputOutput { get; set; }
        Encoding StandardInputOutputEncoding { get; set; }
        bool RunAdministrator { get; set; }
        #endregion
    }

    [Serializable, DataContract]
    public class LauncherExecutePathData: ILauncherExecutePathParameter
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
        public ShowMode ShowMode { get; set; } = ShowMode.Normal;
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

    public class LauncherEnvironmentVariableData
    {
        #region property

        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public bool IsRemove => string.IsNullOrEmpty(Value);

        #endregion
    }

    public class LauncherIconData
    {
        #region property

        public LauncherItemKind Kind { get; set; }

        public IconData Path { get; set; } = new IconData();
        public IconData Icon { get; set; } = new IconData();

        #endregion
    }

    public class StandardStreamData
    {
        #region property

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


    public class LauncherGroupData: ILauncherGroupId
    {
        #region property

        public string Name { get; set; } = string.Empty;
        public LauncherGroupKind Kind { get; set; }
        public LauncherGroupImageName ImageName { get; set; }
        public Color ImageColor { get; set; }
        public long Sequence { get; set; }

        #endregion

        #region ILauncherGroupId

        public LauncherGroupId LauncherGroupId { get; set; }

        #endregion
    }
    public interface ILauncherGroupId
    {
        #region property

        LauncherGroupId LauncherGroupId { get; }

        #endregion
    }

    public class LauncherItemData
    {
        #region property

        public LauncherItemId LauncherItemId { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public virtual LauncherItemKind Kind { get; set; }

        public IconData Icon { get; set; } = new IconData();

        public bool IsEnabledCommandLauncher { get; set; }

        public string Comment { get; set; } = string.Empty;

        #endregion
    }

    /// <summary>
    /// ランチャーアイテム履歴データ種別。
    /// </summary>
    public enum LauncherHistoryKind
    {
        /// <summary>
        /// コマンドラインオプション。
        /// </summary>
        Option,
        /// <summary>
        /// 作業ディレクトリ。
        /// </summary>
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

    public class LauncherAddonDetailData: LauncherDetailDataBase
    {
        #region property

        public bool IsEnabled { get; set; }

        /// <summary>
        /// <see cref="IsEnabled"/> が有効な場合は非<see langword="null" />となる。
        /// </summary>
        public ILauncherItemExtension? Extension { get; set; }

        #endregion
    }

    #endregion

    public interface ILauncherToolbarId
    {
        #region property

        LauncherToolbarId LauncherToolbarId { get; }

        #endregion
    }

    public class LauncherToolbarsScreenData: ILauncherToolbarId, IScreenData
    {
        #region ILauncherToolbarId

        public LauncherToolbarId LauncherToolbarId { get; set; }

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

    public class LauncherToolbarsDisplayData: ILauncherToolbarId
    {
        #region property

        public LauncherGroupId LauncherGroupId { get; set; }
        public AppDesktopToolbarPosition ToolbarPosition { get; set; }
        public LauncherToolbarIconDirection IconDirection { get; set; }
        public IconBox IconBox { get; set; }
        public FontId FontId { get; set; }
        public TimeSpan DisplayDelayTime { get; set; }
        public TimeSpan AutoHideTime { get; set; }
        public int TextWidth { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTopmost { get; set; }
        public bool IsAutoHide { get; set; }
        public bool IsIconOnly { get; set; }

        #endregion

        #region ILauncherToolbarId

        public LauncherToolbarId LauncherToolbarId { get; set; }

        #endregion

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

    public class LauncherRedoData: IReadOnlyLauncherRedoData
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
        public LauncherIconStatus(IconBox iconBox, Point dpiScale, [DateTimeKind(DateTimeKind.Utc)] DateTime lastUpdatedTimestamp)
        {
            IconScale = new IconScale(iconBox, dpiScale);
            LastUpdatedTimestamp = lastUpdatedTimestamp;
        }

        #region property

        public IconScale IconScale { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime LastUpdatedTimestamp { get; }

        #endregion
    }

    public class LauncherSettingCommonData: LauncherItemData
    {
        #region property

        public IList<string> Tags { get; set; } = new List<string>();

        #endregion
    }
}
