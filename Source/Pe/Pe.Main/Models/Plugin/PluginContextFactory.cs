using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Manager;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal class PluginContextFactory: PluginContextFactoryBase
    {
        public PluginContextFactory(EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager)
            :base(environmentParameters, userAgentManager)
        { }

        #region property

        #endregion

        #region function

        PluginFile CreatePluginFile(IPluginIdentifiers pluginId)
        {

            var pluginFile = new PluginFile(
                new PluginFileStorage(GetUserDirectory(pluginId)),
                new PluginFileStorage(GetMachineDirectory(pluginId)),
                new PluginFileStorage(GetTemporaryDirectory(pluginId))
            );

            return pluginFile;
        }

        PluginPersistent CrteatePluginPersistent(IPluginIdentifiers pluginId)
        {
            // DB渡す？ バリア渡す？ 遅延渡す？
            var pluginPersistent = new PluginPersistent(
                new PluginPersistentStorage(),
                new PluginPersistentStorage(),
                new PluginPersistentStorage()
            );

            return pluginPersistent;
        }

        PluginStorage CreatePluginStorage(IPluginIdentifiers pluginId)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginId),
                CrteatePluginPersistent(pluginId)
            );

            return pluginStorage;
        }

        public PluginInitializeContext CreateInitializeContext(IPluginIdentifiers pluginIdentifiers)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers);
            return new PluginInitializeContext(pluginIdentifiers, pluginStorage);
        }

        public PluginContext CreateContext(IPluginIdentifiers pluginIdentifiers)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers);
            return new PluginContext(pluginIdentifiers, pluginStorage, UserAgentManager);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => "data";

        #endregion
    }
}
