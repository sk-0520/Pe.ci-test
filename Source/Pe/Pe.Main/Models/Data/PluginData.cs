using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Data;
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
        /// なんかもうダメダメ。
        /// <para>プラグインの読み込み失敗時に発生するのでこの状態が状態として保存されることはない。</para>
        /// </summary>
        IllegalAssembly,
    }

    public class PluginStateData: DataBase
    {
        #region property

        public Guid PluginId { get; set; }

        public string Name { get; set; } = string.Empty;
        public PluginState State { get; set; }

        #endregion
    }

    public class PluginLoadStateData: DataBase
    {
        public PluginLoadStateData(Guid pluginId, string pluginName, Version pluginVersion, PluginState loadState, WeakReference<PluginLoadContext>? weekLoadContext, IPlugin? plugin)
        {
            PluginId = pluginId;
            PluginName = pluginName;
            PluginVersion = pluginVersion;
            LoadState = loadState;
            WeekLoadContext = weekLoadContext;
            Plugin = plugin;
        }

        #region property

        public Guid PluginId { get; }
        public string PluginName { get; }
        public Version PluginVersion { get; }
        public PluginState LoadState { get; }

        /// <summary>
        /// 対象プラグインの開放状態。
        /// <para><see cref="LoadState"/> が <see cref="PluginState.Disable"/> だと null。</para>
        /// </summary>
        public WeakReference<PluginLoadContext>? WeekLoadContext { get; }
        /// <summary>
        /// 対象プラグイン。
        /// <para><see cref="LoadState"/> が <see cref="PluginState.Enable"/> のみ有効でそれ以外の場合はもうたぶん解放されてる(はず)。</para>
        /// </summary>
        public IPlugin? Plugin { get; }
        #endregion
    }

}
