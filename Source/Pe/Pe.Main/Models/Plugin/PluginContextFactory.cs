using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal class PluginContextFactory: PluginContextFactoryBase
    {
        public PluginContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region property


        #endregion

        #region function

        public PluginInitializeContext CreateInitializeContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, true);
            return new PluginInitializeContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PluginUninitializeContext CreateUninitializeContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, false);
            return new PluginUninitializeContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PluginLoadContext CreateLoadContex(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, true);
            return new PluginLoadContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PluginUnloadContext CreateUnloadContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, false);
            return new PluginUnloadContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => CommonDirectoryName;

        #endregion
    }
}
