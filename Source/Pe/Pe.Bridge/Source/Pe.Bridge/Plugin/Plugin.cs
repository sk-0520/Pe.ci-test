using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    public interface IPlugin
    {
        #region property

        PluginId PluginId { get; }

        IPluginInformation IPluginInformation { get; }

        #endregion
    }
}
