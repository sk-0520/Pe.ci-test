using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class LauncherItemAddonContextFactory: PluginContextFactoryBase
    {
        public LauncherItemAddonContextFactory(IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager)
            :base(databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager)
        { }

        #region property

        #endregion

        #region function

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

        public LauncherItemAddonContext CreateContext(IPluginIdentifiers pluginIdentifiers)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginIdentifiers);
            return new LauncherItemAddonContext(pluginIdentifiers, launcherItemAddonStorage, UserAgentManager);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => "launcher-item-data";

        #endregion

    }
}
