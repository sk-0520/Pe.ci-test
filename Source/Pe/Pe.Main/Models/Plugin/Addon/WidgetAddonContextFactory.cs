using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class WidgetAddonContextFactory: PluginContextFactoryBase
    {
        public WidgetAddonContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region function


        public WidgetAddonCreateContext CreateCreateContex(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseContextsPack, true);
            return new WidgetAddonCreateContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public WidgetAddonClosedContext CreateClosedContext(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseContextsPack, false);
            return new WidgetAddonClosedContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => WidgetDirectoryName;

        #endregion
    }
}
