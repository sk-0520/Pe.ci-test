using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Serialization;
using ContentTypeTextNet.Pe.Main.Models.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// プラグイン読み込み状態
    /// </summary>
    public enum PluginState
    {
        /// <summary>
        /// 読み込み対象。
        /// </summary>
        Enable,
        /// <summary>
        /// 読み込み非対象。
        /// </summary>
        Disable,
        /// <summary>
        /// バージョン不一致。
        /// </summary>
        IllegalVersion,

        /// <summary>
        /// アンインストール対象。
        /// </summary>
        /// <remarks>
        /// <para>次回起動時に該当プラグインは破棄する。</para>
        /// </remarks>
        Uninstall,

        /// <summary>
        /// なんかもうダメダメ。
        /// </summary>
        /// <remarks>
        /// <para>プラグインの読み込み失敗時に発生するのでこの状態が状態として保存されることはない。</para>
        /// </remarks>
        IllegalAssembly,
    }

    public enum PluginInstallMode
    {
        [EnumResource]
        New,
        [EnumResource]
        Update,
    }

    /// <summary>
    /// プラグインインストール時のチェック用アセンブリ読み込み方法。
    /// </summary>
    public enum PluginInstallAssemblyMode
    {
        /// <summary>
        /// 直接読み込み。
        /// </summary>
        Direct,
        /// <summary>
        /// 別プロセスで読み込む。
        /// </summary>
        Process,
    }

    public interface IPluginId
    {
        #region property

        PluginId PluginId { get; }

        #endregion
    }

    /// <summary>
    /// プラグインインストール時に使用される設定ファイル。
    /// </summary>
    [Serializable, DataContract]
    public class PluginAssemblyInfo
    {
        #region property

        /// <summary>
        /// ファイル名。
        /// </summary>
        public string FileName { get; init; } = string.Empty;

        #endregion
    }

    [Serializable, DataContract]
    public class PluginStateData: IPluginIdentifiers
    {
        #region property

        public PluginId PluginId { get; set; }

        public string PluginName { get; set; } = string.Empty;
        public PluginState State { get; set; }

        #endregion
    }

    public interface IPluginLoadState
    {
        #region property

        public PluginId PluginId { get; }
        public string PluginName { get; }
        public Version PluginVersion { get; }
        public PluginState LoadState { get; }

        #endregion
    }

    public record PluginLoadStateInfo(
        PluginId PluginId,
        string PluginName,
        Version PluginVersion,
        PluginState LoadState
    ): IPluginLoadState;

    [Serializable, DataContract]
    public class PluginLoadStateData: IPluginLoadState
    {
        public PluginLoadStateData(PluginId pluginId, string pluginName, Version pluginVersion, PluginState loadState, PluginAssemblyLoadContext? loadContext, IPlugin? plugin)
        {
            PluginId = pluginId;
            PluginName = pluginName;
            PluginVersion = pluginVersion;
            LoadState = loadState;
            LoadContext = loadContext;
            Plugin = plugin;
        }

        #region property

        /// <summary>
        /// 対象プラグインの開放状態。
        /// </summary>
        /// <remarks>
        /// <para><see cref="LoadState"/> が <see cref="PluginState.Disable"/> だと null。</para>
        /// </remarks>
        public PluginAssemblyLoadContext? LoadContext { get; private set; }
        /// <summary>
        /// 対象プラグイン。
        /// </summary>
        /// <remarks>
        /// <para><see cref="LoadState"/> が <see cref="PluginState.Enable"/> のみ有効でそれ以外の場合はもうたぶん解放されてる(はず)。</para>
        /// </remarks>
        public IPlugin? Plugin { get; }

        #endregion

        #region function

        public PluginAssemblyLoadContext FreeLoadContext()
        {
            if(LoadContext is null) {
                throw new InvalidOperationException();
            }

            var result = LoadContext;
            LoadContext = null;
            return result;
        }

        #endregion

        #region IPluginLoadState

        public PluginId PluginId { get; }
        public string PluginName { get; }
        [JsonConverter(typeof(JsonTextSerializer.VersionConverter))]
        public Version PluginVersion { get; }
        public PluginState LoadState { get; }

        #endregion
    }

    public class PluginSettingRawValue
    {
        public PluginSettingRawValue(PluginPersistenceFormat format, string value)
        {
            Format = format;
            Value = value;
        }

        #region property

        public PluginPersistenceFormat Format { get; }
        public string Value { get; }

        #endregion
    }

    [Serializable, DataContract]
    public class PluginWidgetSettingData
    {
        #region property

        [PixelKind(Px.Logical)]
        public double X { get; set; }
        [PixelKind(Px.Logical)]
        public double Y { get; set; }
        [PixelKind(Px.Logical)]
        public double Width { get; set; }
        [PixelKind(Px.Logical)]
        public double Height { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTopmost { get; set; }

        #endregion
    }

    public record PluginInstallData(
        PluginId PluginId,
        string PluginName,
        Version PluginVersion,
        PluginInstallMode PluginInstallMode,
        string ExtractedDirectoryPath,
        string PluginDirectoryPath
    ): IPluginId;

    /// <summary>
    ///
    /// </summary>
    /// <param name="Version">最終使用バージョン。</param>
    public record PluginLastUsedData(
        PluginId PluginId,
        string Name,
        Version Version
    ): IPluginId;
}
