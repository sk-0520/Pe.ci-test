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

        public PreferencesLoadContext CreateLoadContext(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseContextsPack, true);
            return new PreferencesLoadContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PreferencesCheckContext CreateCheckContext(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseContextsPack, true);
            return new PreferencesCheckContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PreferencesSaveContext CreateSaveContext(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseContextsPack, false);
            return new PreferencesSaveContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PreferencesEndContext CreateEndContext(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseContextsPack, true);
            return new PreferencesEndContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        #endregion
    }
}
