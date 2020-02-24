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

        string ConvertDirectoryName(PluginId pluginId)
        {
            return pluginId.Id.ToString();
        }

        PluginFile CreatePluginFile(in PluginId pluginId)
        {
            var dirName = ConvertDirectoryName(pluginId);

            var pluginFile = new PluginFile(
                new PluginFileStorage(new DirectoryInfo(Path.Combine(EnvironmentParameters.UserPluginDirectory.FullName, dirName))),
                new PluginFileStorage(new DirectoryInfo(Path.Combine(EnvironmentParameters.MachinePluginDirectory.FullName, dirName))),
                new PluginFileStorage(new DirectoryInfo(Path.Combine(EnvironmentParameters.TemporaryPluginDirectory.FullName, dirName)))
            );

            return pluginFile;
        }

        PluginPersistent CrteatePluginPersistent(in PluginId pluginId)
        {
            // DB渡す？ バリア渡す？ 遅延渡す？
            var pluginPersistent = new PluginPersistent(
                new PluginPersistentStorage(),
                new PluginPersistentStorage(),
                new PluginPersistentStorage()
            );

            return pluginPersistent;
        }

        PluginStorage CreatePluginStorage(in PluginId pluginId)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginId),
                CrteatePluginPersistent(pluginId)
            );

            return pluginStorage;
        }

        public PluginInitializeContext CreateInitializeContext(PluginId pluginId)
        {
            var pluginStorage = CreatePluginStorage(pluginId);
            return new PluginInitializeContext(pluginId, pluginStorage);
        }

        public PluginContext CreateContext(in PluginId pluginId)
        {
            var pluginStorage = CreatePluginStorage(pluginId);
            return new PluginContext(pluginId, pluginStorage, UserAgentManager);
        }

        #endregion
    }
}
