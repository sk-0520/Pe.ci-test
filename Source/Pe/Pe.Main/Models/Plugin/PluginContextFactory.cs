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

        protected virtual PluginPersistent CrteatePluginPersistentCommander(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack, bool isReadOnly)
        {
            var pluginPersistent = new PluginPersistent(
                new PluginPersistentStorage(pluginIdentifiers, databaseCommanderPack.Main, isReadOnly),
                new PluginPersistentStorage(pluginIdentifiers, databaseCommanderPack.File, isReadOnly),
                new PluginPersistentStorage(pluginIdentifiers, databaseCommanderPack.Temporary, isReadOnly)
            );

            return pluginPersistent;
        }

        protected virtual PluginPersistent CrteatePluginPersistentLazyWriter(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack, IDatabaseLazyWriterPack databaseLazyWriterPack)
        {
            var pluginPersistent = new PluginPersistent(
                new PluginPersistentStorage(pluginIdentifiers, databaseCommanderPack.Main, databaseLazyWriterPack.Main),
                new PluginPersistentStorage(pluginIdentifiers, databaseCommanderPack.File, databaseLazyWriterPack.File),
                new PluginPersistentStorage(pluginIdentifiers, databaseCommanderPack.Temporary, databaseLazyWriterPack.Temporary)
            );

            return pluginPersistent;
        }


        protected virtual PluginStorage CreatePluginStorage(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack, bool isReadOnly)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginIdentifiers),
                CrteatePluginPersistentCommander(pluginIdentifiers, databaseCommanderPack, isReadOnly)
            );

            return pluginStorage;
        }

        protected virtual PluginStorage CreatePluginStorage(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack, IDatabaseLazyWriterPack databaseLazyWriterPack)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginIdentifiers),
                CrteatePluginPersistentLazyWriter(pluginIdentifiers, databaseCommanderPack, databaseLazyWriterPack)
            );

            return pluginStorage;
        }

        public PluginInitializeContext CreateInitializeContext(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack, bool isReadOnly)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers, databaseCommanderPack, isReadOnly);
            return new PluginInitializeContext(pluginIdentifiers, pluginStorage);
        }

        public PluginUninitializeContext CreateUninitializeContext(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack, bool isReadOnly)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers, databaseCommanderPack, isReadOnly);
            return new PluginUninitializeContext(pluginIdentifiers, pluginStorage);
        }

        public PluginContext CreateContext(IPluginIdentifiers pluginIdentifiers, IApplicationPack<IDatabaseCommander> databaseCommanderPack, bool isReadOnly)
        {
            var pluginStorage = CreatePluginStorage(pluginIdentifiers, databaseCommanderPack, isReadOnly);
            return new PluginContext(pluginIdentifiers, pluginStorage, UserAgentManager);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => "data";

        #endregion
    }
}
