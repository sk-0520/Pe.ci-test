using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences
{
    public class PreferencesContextFactory: PluginContextFactory
    {
        public PreferencesContextFactory(IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager)
            : base(databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager)
        { }

        #region function

        public PreferencesLoadContext CreateLoadContext(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers, databaseCommanderPack, true);
            return new PreferencesLoadContext(pluginIdentifiers, pluginStorage, UserAgentManager, new SkeletonImplements());
        }

        public PreferencesCheckContext CreateCheckContext(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers, databaseCommanderPack, true);
            return new PreferencesCheckContext(pluginIdentifiers, pluginStorage, UserAgentManager);
        }

        public PreferencesSaveContext CreateSaveContext(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers, databaseCommanderPack, false);
            return new PreferencesSaveContext(pluginIdentifiers, pluginStorage, UserAgentManager);
        }

        public PreferencesEndContext CreateEndContext(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers, databaseCommanderPack, true);
            return new PreferencesEndContext(pluginIdentifiers, pluginStorage, UserAgentManager);
        }

        #endregion
    }
}
