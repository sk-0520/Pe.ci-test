using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public class PluginContextFactory: PluginContextFactoryBase
    {
        public PluginContextFactory(IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager)
            : base(databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager)
        { }

        #region property


        #endregion

        #region function

        protected virtual PluginFile CreatePluginFile(IPluginIdentifiers pluginIdentifiers)
        {

            var pluginFile = new PluginFile(
                new PluginFileStorage(GetUserDirectory(pluginIdentifiers)),
                new PluginFileStorage(GetMachineDirectory(pluginIdentifiers)),
                new PluginFileStorage(GetTemporaryDirectory(pluginIdentifiers))
            );

            return pluginFile;
        }

        protected virtual PluginPersistent CrteatePluginPersistentCommander(IPluginIdentifiers pluginIdentifiers, IDatabaseCommandsPack databaseCommandsPack, bool isReadOnly)
        {
            var pluginPersistent = new PluginPersistent(
                new PluginPersistentStorage(pluginIdentifiers, databaseCommandsPack.Main, DatabaseStatementLoader, isReadOnly),
                new PluginPersistentStorage(pluginIdentifiers, databaseCommandsPack.File, DatabaseStatementLoader, isReadOnly),
                new PluginPersistentStorage(pluginIdentifiers, databaseCommandsPack.Temporary, DatabaseStatementLoader, isReadOnly)
            );

            return pluginPersistent;
        }

        protected virtual PluginPersistent CrteatePluginPersistentLazyWriter(IPluginIdentifiers pluginIdentifiers, IDatabaseCommandsPack databaseCommandsPack, IDatabaseLazyWriterPack databaseLazyWriterPack)
        {
            var pluginPersistent = new PluginPersistent(
                new PluginPersistentStorage(pluginIdentifiers, databaseCommandsPack.Main, databaseLazyWriterPack.Main, DatabaseStatementLoader),
                new PluginPersistentStorage(pluginIdentifiers, databaseCommandsPack.File, databaseLazyWriterPack.File, DatabaseStatementLoader),
                new PluginPersistentStorage(pluginIdentifiers, databaseCommandsPack.Temporary, databaseLazyWriterPack.Temporary, DatabaseStatementLoader)
            );

            return pluginPersistent;
        }


        protected virtual PluginStorage CreatePluginStorage(IPluginIdentifiers pluginIdentifiers, IDatabaseCommandsPack databaseCommandsPack, bool isReadOnly)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginIdentifiers),
                CrteatePluginPersistentCommander(pluginIdentifiers, databaseCommandsPack, isReadOnly)
            );

            return pluginStorage;
        }

        protected virtual PluginStorage CreatePluginStorage(IPluginIdentifiers pluginIdentifiers, IDatabaseCommandsPack databaseCommandsPack, IDatabaseLazyWriterPack databaseLazyWriterPack)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginIdentifiers),
                CrteatePluginPersistentLazyWriter(pluginIdentifiers, databaseCommandsPack, databaseLazyWriterPack)
            );

            return pluginStorage;
        }

        public PluginInitializeContext CreateInitializeContext(IPluginIdentifiers pluginIdentifiers, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers, databaseCommandsPack, true);
            return new PluginInitializeContext(pluginIdentifiers, pluginStorage);
        }

        public PluginUninitializeContext CreateUninitializeContext(IPluginIdentifiers pluginIdentifiers, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers, databaseCommandsPack, false);
            return new PluginUninitializeContext(pluginIdentifiers, pluginStorage);
        }

        public PluginContext CreateContext(IPluginIdentifiers pluginIdentifiers, IDatabaseCommandsPack databaseCommandsPack, bool isReadOnly)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers, databaseCommandsPack, isReadOnly);
            return new PluginContext(pluginIdentifiers, pluginStorage, UserAgentManager);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => "data";

        #endregion
    }
}
