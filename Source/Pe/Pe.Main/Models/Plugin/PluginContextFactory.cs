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
        public PluginContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region property


        #endregion

        #region function

        protected virtual PluginFiles CreatePluginFile(IPluginInformations pluginInformations)
        {

            var pluginFile = new PluginFiles(
                new PluginFileStorage(GetUserDirectory(pluginInformations.PluginIdentifiers)),
                new PluginFileStorage(GetMachineDirectory(pluginInformations.PluginIdentifiers)),
                new PluginFileStorage(GetTemporaryDirectory(pluginInformations.PluginIdentifiers))
            );

            return pluginFile;
        }

        /// <summary>
        /// 上流依存DBアクセス処理の生成。
        /// </summary>
        /// <param name="pluginInformations"></param>
        /// <param name="databaseCommandsPack"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        protected virtual PluginPersistents CrteatePluginPersistentCommander(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack, bool isReadOnly)
        {
            var pluginPersistent = new PluginPersistents(
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseCommandsPack.Main, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseCommandsPack.File, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseCommandsPack.Temporary, DatabaseStatementLoader, isReadOnly, LoggerFactory)
            );

            return pluginPersistent;
        }

        /// <summary>
        /// 逐次実行DBアクセス処理の生成。
        /// </summary>
        /// <param name="pluginInformations"></param>
        /// <param name="databaseBarrierPack"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        protected virtual PluginPersistents CrteatePluginPersistentBarrier(IPluginInformations pluginInformations, IDatabaseBarrierPack databaseBarrierPack, bool isReadOnly)
        {
            var pluginPersistent = new PluginPersistents(
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.Main, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.File, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.Temporary, DatabaseStatementLoader, isReadOnly, LoggerFactory)
            );

            return pluginPersistent;
        }

        /// <summary>
        /// 遅延実行DBアクセス処理の生成。
        /// </summary>
        /// <param name="pluginInformations"></param>
        /// <param name="databaseBarrierPack"></param>
        /// <param name="databaseLazyWriterPack"></param>
        /// <returns></returns>
        protected virtual PluginPersistents CrteatePluginPersistentLazyWriter(IPluginInformations pluginInformations, IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack)
        {
            var pluginPersistent = new PluginPersistents(
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.Main, databaseLazyWriterPack.Main, DatabaseStatementLoader, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.File, databaseLazyWriterPack.File, DatabaseStatementLoader, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.Temporary, databaseLazyWriterPack.Temporary, DatabaseStatementLoader, LoggerFactory)
            );

            return pluginPersistent;
        }

        protected virtual PluginStorage CreatePluginStorage(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack, bool isReadOnly)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginInformations),
                CrteatePluginPersistentCommander(pluginInformations, databaseCommandsPack, isReadOnly)
            );

            return pluginStorage;
        }

        protected virtual PluginStorage CreatePluginStorage(IPluginInformations pluginInformations, IDatabaseBarrierPack databaseBarrierPack, bool isReadOnly)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginInformations),
                CrteatePluginPersistentBarrier(pluginInformations, databaseBarrierPack, isReadOnly)
            );

            return pluginStorage;
        }

        protected virtual PluginStorage CreatePluginStorage(IPluginInformations pluginInformations, IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginInformations),
                CrteatePluginPersistentLazyWriter(pluginInformations, databaseBarrierPack, databaseLazyWriterPack)
            );

            return pluginStorage;
        }

        public PluginInitializeContext CreateInitializeContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, true);
            return new PluginInitializeContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PluginUninitializeContext CreateUninitializeContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, false);
            return new PluginUninitializeContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PluginLoadContext CreateLoadContex(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, true);
            return new PluginLoadContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PluginUnloadContext CreateUnloadContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, false);
            return new PluginUnloadContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        public PluginContext CreateContext(IPluginInformations pluginInformations, IDatabaseCommandsPack databaseCommandsPack, bool isReadOnly)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseCommandsPack, isReadOnly);
            return new PluginContext(pluginInformations.PluginIdentifiers, pluginStorage);
        }

        static internal NullPluginContext CreateNullContext(ILoggerFactory loggerFactory)
        {
            var nullPluginInformation = new NullPluginInformation();
            var nullStorage = new NullPluginStorage(nullPluginInformation.PluginIdentifiers, loggerFactory);
            return new NullPluginContext(nullStorage);
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => "data";

        #endregion
    }
}
