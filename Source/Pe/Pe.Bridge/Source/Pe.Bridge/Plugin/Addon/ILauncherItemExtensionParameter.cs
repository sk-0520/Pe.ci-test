using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// ランチャーアイテム実行時に Pe から渡されるデータ。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemExtensionExecuteParameter: IAddonParameter
    {
        #region property

        ILauncherItemAddonViewSupporter ViewSupporter { get; }
        ILauncherItemAddonContextTask ContextTask { get; }

        #endregion
    }
}
