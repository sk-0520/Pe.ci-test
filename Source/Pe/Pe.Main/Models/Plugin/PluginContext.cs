using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public class PluginInitializeContext: IPluginInitializeContext
    {
        public PluginInitializeContext(in PluginId pluginId, PluginStorage storage)
        {
            PluginId = pluginId;
            Storage = storage;
        }

        #region property

        public PluginId PluginId { get; }

        #endregion

        #region IPluginInitializeContext

        public PluginStorage Storage { get; }
        IPluginStorage IPluginInitializeContext.Storage => Storage;

        #endregion
    }

    public class PluginContext : IPluginContext
    {
        public PluginContext(in PluginId pluginId, PluginStorage storage, IUserAgentFactory userAgentFactory)
        {
            PluginId = pluginId;
            Storage = storage;
            UserAgentFactory = userAgentFactory;
        }

        #region property

        public PluginId PluginId { get; }

        #endregion

        #region IPluginContext

        public PluginStorage Storage { get; }
        IPluginStorage IPluginContext.Storage => Storage;

        public IUserAgentFactory UserAgentFactory { get; }

        #endregion
    }

    public class PluginContextFactory
    {
        public PluginContextFactory(EnvironmentParameters environmentParameters, IUserAgentFactory userAgentFactory)
        {
            EnvironmentParameters = environmentParameters;
            UserAgentFactory = userAgentFactory;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        IUserAgentFactory UserAgentFactory { get; }
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
            return new PluginContext(pluginId, pluginStorage, UserAgentFactory);
        }

        #endregion
    }
}
