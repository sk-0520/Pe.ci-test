using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public class PluginContext : IPluginContext
    {
        public PluginContext(in PluginId pluginId, PluginStorage storage)
        {
            PluginId = pluginId;
            Storage = storage;
        }

        #region property

        public PluginId PluginId { get; }

        #endregion

        #region IPluginContext

        public PluginStorage Storage { get; }
        IPluginStorage IPluginContext.Storage => Storage;

        #endregion
    }

    public class PluginContextFactory
    {
        public PluginContextFactory(EnvironmentParameters environmentParameters)
        {
            EnvironmentParameters = environmentParameters;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }

        #endregion

        #region function

        string ConvertDirectoryName(PluginId pluginId)
        {
            return pluginId.Id.ToString();
        }

        public PluginContext Create(PluginId pluginId)
        {
            var dirName = ConvertDirectoryName(pluginId);

            var pluginFile = new PluginFile(
                new PluginFileStorage(new DirectoryInfo(Path.Combine(EnvironmentParameters.UserPluginDirectory.FullName, dirName))),
                new PluginFileStorage(new DirectoryInfo(Path.Combine(EnvironmentParameters.MachinePluginDirectory.FullName, dirName))),
                new PluginFileStorage(new DirectoryInfo(Path.Combine(EnvironmentParameters.TemporaryPluginDirectory.FullName, dirName)))
            );

            // DB渡す？ バリア渡す？ 遅延渡す？
            var pluginPersistent = new PluginPersistent(
                new PluginPersistentStorage(),
                new PluginPersistentStorage(),
                new PluginPersistentStorage()
            );

            var pluginStorage = new PluginStorage(
                pluginFile,
                pluginPersistent
            );

            return new PluginContext(pluginId, pluginStorage);
        }

        #endregion
    }
}
