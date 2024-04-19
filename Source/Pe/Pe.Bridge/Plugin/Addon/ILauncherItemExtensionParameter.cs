using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// ランチャーアイテムビルド時に Pe から渡されるデータ。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface ILauncherItemExtensionCreateParameter: IAddonParameter, ILauncherItemId
    {
        #region property

        ILauncherItemAddonContextWorker ContextWorker { get; }

        #endregion
    }

    /// <summary>
    /// ランチャーアイテム実行時に Pe から渡されるデータ。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface ILauncherItemExtensionExecuteParameter: IAddonParameter, ILauncherItemId
    {
        #region property

        ILauncherItemAddonViewSupporter ViewSupporter { get; }

        #endregion
    }
}
