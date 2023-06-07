using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences
{
    internal class PreferencesContextFactory: PluginContextFactory
    {
        public PreferencesContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region function

        public PreferencesLoadContext CreatePreferencesLoadContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, true);
            return new PreferencesLoadContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        public PreferencesCheckContext CreateCheckContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, true);
            return new PreferencesCheckContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        public PreferencesSaveContext CreateSaveContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, false);
            return new PreferencesSaveContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        public PreferencesEndContext CreateEndContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, true);
            return new PreferencesEndContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        #endregion
    }
}
