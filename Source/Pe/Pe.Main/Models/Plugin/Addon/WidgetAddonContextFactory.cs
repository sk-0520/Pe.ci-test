using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class WidgetAddonContextFactory: PluginContextFactoryBase
    {
        public WidgetAddonContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseDelayWriterPack databaseDelayWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseDelayWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region function


        public WidgetAddonCreateContext CreateCreateContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, true);
            return new WidgetAddonCreateContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        public WidgetAddonClosedContext CreateClosedContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, false);
            return new WidgetAddonClosedContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => WidgetDirectoryName;

        #endregion
    }
}
