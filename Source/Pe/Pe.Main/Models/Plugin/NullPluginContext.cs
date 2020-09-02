using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal class NullPluginContext: IPluginContext
    {
        public NullPluginContext(NullPluginStorage storage)
        {
            Storage = storage;
        }

        #region IPluginContext

        /// <inheritdoc cref="IPluginContext.Storage"/>
        public NullPluginStorage Storage { get; }
        IPluginStorage IPluginContext.Storage => Storage;

        /// <inheritdoc cref="IPluginContext.UserAgentFactory"/>
        public IHttpUserAgentFactory UserAgentFactory => throw new NotSupportedException();

        /// <inheritdoc cref="IPluginCommonContext.IsAvailable"/>
        public bool IsAvailable => true;

        #endregion
    }
}
