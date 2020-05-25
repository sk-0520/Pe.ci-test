using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Manager;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public class PluginContextFactory
    {
        public PluginContextFactory(EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager)
        {
            EnvironmentParameters = environmentParameters;
            UserAgentManager = userAgentManager;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        IUserAgentManager UserAgentManager { get; }
        #endregion

        #region function

        string ConvertDirectoryName(IPluginIdentifiers pluginId)
        {
            return pluginId.PluginId.ToString();
        }

        PluginFile CreatePluginFile(IPluginIdentifiers pluginId)
        {
            var dirName = ConvertDirectoryName(pluginId);

            var pluginFile = new PluginFile(
                new PluginFileStorage(new DirectoryInfo(Path.Combine(EnvironmentParameters.UserPluginDirectory.FullName, dirName))),
                new PluginFileStorage(new DirectoryInfo(Path.Combine(EnvironmentParameters.MachinePluginBaseDirectory.FullName, dirName))),
                new PluginFileStorage(new DirectoryInfo(Path.Combine(EnvironmentParameters.TemporaryPluginDirectory.FullName, dirName)))
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
    }
}
