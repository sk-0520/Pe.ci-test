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
    public class PreferencesContextFactory: PluginContextFactory
    {
        public PreferencesContextFactory(IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region function

        public PreferencesLoadContext CreateLoadContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommanderPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommanderPack, true);
            return new PreferencesLoadContext(pluginInformations.PluginIdentifiers, pluginStorage, UserAgentManager, new SkeletonImplements());
        }

        public PreferencesCheckContext CreateCheckContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommanderPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommanderPack, true);
            return new PreferencesCheckContext(pluginInformations.PluginIdentifiers, pluginStorage, UserAgentManager);
        }

        public PreferencesSaveContext CreateSaveContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommanderPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommanderPack, false);
            return new PreferencesSaveContext(pluginInformations.PluginIdentifiers, pluginStorage, UserAgentManager);
        }

        public PreferencesEndContext CreateEndContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommanderPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommanderPack, true);
            return new PreferencesEndContext(pluginInformations.PluginIdentifiers, pluginStorage, UserAgentManager);
        }

        #endregion
    }
}
