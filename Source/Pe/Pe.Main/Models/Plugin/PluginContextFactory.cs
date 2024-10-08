using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public class PluginContextFactory: PluginContextFactoryBase
    {
        public PluginContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseDelayWriterPack databaseDelayWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseDelayWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region property


        #endregion

        #region function

        public PluginInitializeContext CreateInitializeContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, true);
            return new PluginInitializeContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        public PluginFinalizeContext CreateFinalizeContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, false);
            return new PluginFinalizeContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        public PluginLoadContext CreateLoadContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, true);
            return new PluginLoadContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        public PluginUnloadContext CreateUnloadContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, false);
            return new PluginUnloadContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => CommonDirectoryName;

        #endregion
    }
}
