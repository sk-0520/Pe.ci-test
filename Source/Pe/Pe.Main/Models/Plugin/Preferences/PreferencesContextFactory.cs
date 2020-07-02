using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences
{
    internal class PreferencesContextFactory: PluginContextFactory
    {
        public PreferencesContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region function

        public PreferencesLoadContext CreateLoadContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, true);
            return new PreferencesLoadContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PreferencesCheckContext CreateCheckContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, true);
            return new PreferencesCheckContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PreferencesSaveContext CreateSaveContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, false);
            return new PreferencesSaveContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PreferencesEndContext CreateEndContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, true);
            return new PreferencesEndContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        #endregion
    }
}
