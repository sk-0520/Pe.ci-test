using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Manager;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences
{
    public class PreferencesContextFactory: PluginContextFactory
    {
        public PreferencesContextFactory(EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager)
            : base(environmentParameters, userAgentManager)
        { }

        #region function

        public PreferencesLoadContext CreateLoadContext(IPluginIdentifiers pluginIdentifiers)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers);
            return new PreferencesLoadContext(pluginIdentifiers, pluginStorage, UserAgentManager, new SkeletonImplements());
        }

        public PreferencesCheckContext CreateCheckContext(IPluginIdentifiers pluginIdentifiers)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers);
            return new PreferencesCheckContext(pluginIdentifiers, pluginStorage, UserAgentManager);
        }

        public PreferencesSaveContext CreateSaveContext(IPluginIdentifiers pluginIdentifiers)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers);
            return new PreferencesSaveContext(pluginIdentifiers, pluginStorage, UserAgentManager);
        }

        public PreferencesEndContext CreateEndContext(IPluginIdentifiers pluginIdentifiers)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers);
            return new PreferencesEndContext(pluginIdentifiers, pluginStorage, UserAgentManager);
        }

        #endregion
    }
}
