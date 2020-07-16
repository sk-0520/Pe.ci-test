using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// プラグインコンテキストが必要な処理を実施する。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IContextTask
    {
        #region function

        void RunPlugin(Action<IPluginContext> callback);

        #endregion
    }

    /// <summary>
    /// ランチャーアイテムアドオンコンテキストが必要な処理を実施する。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemAddonContextTask
    {
        #region function

        void RunLauncherItemAddon(Action<ILauncherItemAddonContext> callback);

        #endregion
    }
}
