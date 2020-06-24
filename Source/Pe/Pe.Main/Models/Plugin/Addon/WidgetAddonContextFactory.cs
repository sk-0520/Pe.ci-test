using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class WidgetAddonContextFactory: PluginContextFactoryBase
    {
        public WidgetAddonContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region function


        public WidgetAddonCreateContext CreateCreateContex(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, true);
            return new WidgetAddonCreateContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public WidgetAddonClosedContext CreateClosedContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, false);
            return new WidgetAddonClosedContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => WidgetDirectoryName;

        #endregion
    }
}
