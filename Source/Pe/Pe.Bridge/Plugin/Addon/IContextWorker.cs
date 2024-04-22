using System;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// プラグインコンテキストが必要な処理を実施する。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IContextWorker
    {
        #region function

        void RunPlugin(Action<IPluginContext> callback);

        #endregion
    }

    /// <summary>
    /// ランチャーアイテムアドオンコンテキストが必要な処理を実施する。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface ILauncherItemAddonContextWorker
    {
        #region function

        /// <summary>
        /// ランチャーアイテム処理実施。
        /// </summary>
        /// <param name="callback">真を返せばコミットする。</param>
        void RunLauncherItemAddon(Func<ILauncherItemAddonContext, bool> callback);

        #endregion
    }
}
