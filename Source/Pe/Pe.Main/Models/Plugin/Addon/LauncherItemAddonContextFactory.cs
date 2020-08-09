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


        LauncherItemAddonFiles CreateLauncherItemAddonFile(IPluginInformations  pluginInformations)
        {
            var dirName = ConvertDirectoryName(pluginInformations.PluginIdentifiers);

            var pluginFile = new LauncherItemAddonFiles(
                new LauncherItemAddonFileStorage(GetUserDirectory(pluginInformations.PluginIdentifiers)),
                new LauncherItemAddonFileStorage(GetMachineDirectory(pluginInformations.PluginIdentifiers)),
                new LauncherItemAddonFileStorage(GetTemporaryDirectory(pluginInformations.PluginIdentifiers))
            );

            return pluginFile;
        }

        LauncherItemAddonPersistents CreateLauncherItemAddonPersistentCommander(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack, bool isReadOnly)
        {
            var pluginPersistent = new LauncherItemAddonPersistents(
                new LauncherItemAddonPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseCommandsPack.Main, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new LauncherItemAddonPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseCommandsPack.File, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new LauncherItemAddonPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseCommandsPack.Temporary, DatabaseStatementLoader, isReadOnly, LoggerFactory)
            );

            return pluginPersistent;
        }

        LauncherItemAddonStorage CreateLauncherItemAddonStorage(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack, bool isReadOnly)
        {
            var pluginStorage = new LauncherItemAddonStorage(
                CreateLauncherItemAddonFile(pluginInformations),
                CreateLauncherItemAddonPersistentCommander(pluginInformations, databaseCommandsPack, isReadOnly)
            );

            return pluginStorage;
        }

        public LauncherItemAddonContext CreateContext(IPluginInformations pluginInformations, Guid launcherItemId, IDatabaseCommandsPack databaseCommandsPack, bool isReadOnly)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginInformations, databaseCommandsPack, isReadOnly);
            return new LauncherItemAddonContext(pluginInformations.PluginIdentifiers, launcherItemId, launcherItemAddonStorage);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => "launcher-item-data";

        #endregion

    }
}
