using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class LauncherItemAddonContextFactory: PluginContextFactoryBase
    {
        public LauncherItemAddonContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region property

        #endregion

        #region function

        public LauncherItemAddonContextWorker CreateWorker(Guid launcherItemId)
        {
            return new LauncherItemAddonContextWorker(this, LoggerFactory);
        }


        LauncherItemAddonFiles CreateLauncherItemAddonFile(IPluginIdentifiers pluginId)
        {
            var dirName = ConvertDirectoryName(pluginId);

            var pluginFile = new LauncherItemAddonFiles(
                new LauncherItemAddonFileStorage(GetUserDirectory(pluginId)),
                new LauncherItemAddonFileStorage(GetMachineDirectory(pluginId)),
                new LauncherItemAddonFileStorage(GetTemporaryDirectory(pluginId))
            );

            return pluginFile;
        }

        LauncherItemAddonPersistents CreateLauncherItemAddonPersistent(IPluginIdentifiers pluginId)
        {
            // DB渡す？ バリア渡す？ 遅延渡す？
            var pluginPersistent = new LauncherItemAddonPersistents(
                new LauncherItemAddonPersistentStorage(),
                new LauncherItemAddonPersistentStorage(),
                new LauncherItemAddonPersistentStorage()
            );

            return pluginPersistent;
        }

        LauncherItemAddonStorage CreateLauncherItemAddonStorage(IPluginIdentifiers pluginId)
        {
            var pluginStorage = new LauncherItemAddonStorage(
                CreateLauncherItemAddonFile(pluginId),
                CreateLauncherItemAddonPersistent(pluginId)
            );

            return pluginStorage;
        }

        public LauncherItemAddonContext CreateContext(IPluginIdentifiers pluginIdentifiers, Guid launcherItemId)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginIdentifiers);
            return new LauncherItemAddonContext(pluginIdentifiers, launcherItemId, launcherItemAddonStorage);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => "launcher-item-data";

        #endregion

    }
}
